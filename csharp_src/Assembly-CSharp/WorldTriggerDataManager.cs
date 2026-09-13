using System.Collections.Generic;
using FibMatrix;
using GameFramework;
using Protobuf;
using Sfs2X.Entities.Data;
using Sfs2X.Util;
using UnityEngine;
using XLua;

public class WorldTriggerDataManager : WorldManagerBase
{
	private HashSet<long> allUuidSet = new HashSet<long>();

	private HashSet<long> oldAllUuidSet = new HashSet<long>();

	private Dictionary<long, WorldTriggerData> allTriggers;

	private ObjectPool<WorldTriggerData> pool;

	private Dictionary<int, WorldTriggerConfig> triggerConfigs;

	private long vibrationTime;

	public WorldTriggerDataManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		allTriggers = new Dictionary<long, WorldTriggerData>();
		pool = new ObjectPool<WorldTriggerData>();
		triggerConfigs = new Dictionary<int, WorldTriggerConfig>(16);
		GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetAllWorldTriggerConfig")?.ForEach(delegate(int id, LuaTable data)
		{
			triggerConfigs[id] = new WorldTriggerConfig(id, data);
		});
	}

	public override void UnInit()
	{
		ClearAllTriggers();
		triggerConfigs.Clear();
		pool.Dispose();
	}

	private void ClearAllTriggers()
	{
		pool.RecycleNoClear(allTriggers.Values);
		allTriggers.Clear();
		allUuidSet.Clear();
	}

	public void HandleWorldGetBlock(ISFSObject msg, int serverId, int worldId)
	{
		HashSet<long> hashSet = allUuidSet;
		HashSet<long> hashSet2 = oldAllUuidSet;
		oldAllUuidSet = hashSet;
		allUuidSet = hashSet2;
		if (msg != null)
		{
			ISFSArray iSFSArray = msg.TryGetArray("ls");
			if (iSFSArray != null)
			{
				foreach (ISFSObject item2 in iSFSArray)
				{
					long item = AddOrUpdateOneTrigger(item2, serverId, worldId);
					oldAllUuidSet.Remove(item);
				}
			}
		}
		bool flag = SceneSkinManager.Instance.GetCurSkinMeta()?.IsNineNationMode() ?? false;
		foreach (long item3 in oldAllUuidSet)
		{
			if (!flag || serverId <= 0 || !allTriggers.TryGetValue(item3, out var value) || value.serverId == serverId)
			{
				DeleteOneTrigger(item3);
			}
		}
		oldAllUuidSet.Clear();
	}

	public void HandlePushWorldTriggerUpdate(ISFSObject msg, int serverId, int worldId)
	{
		AddOrUpdateOneTrigger(msg, serverId, worldId);
	}

	public void HandlePushWorldTriggerDel(ISFSObject msg, int serverId, int worldId)
	{
		long uuid = msg.TryGetLong("uuid");
		DeleteOneTrigger(uuid);
		if (!msg.TryGetBool("trigger"))
		{
			return;
		}
		WorldTriggerData worldTriggerData = pool.Allocate();
		worldTriggerData.ParseData(msg, serverId, worldId);
		if (triggerConfigs.TryGetValue(worldTriggerData.cfgId, out var value))
		{
			float num = ((value.type == 5) ? 0.5f : 2f);
			int pointId = worldTriggerData.pointId;
			int plotId = value.plot;
			Vector3 pos = SceneManager.World.TileIndexToWorld(pointId, worldTriggerData.serverId);
			world.CreateVFX(value.prefab, pos, num);
			world.CreateVFX(value.explode, pos, 2f, num);
			string uid = msg.TryGetString("triggerUid");
			if (uid == GameEntry.Data.Player.Uid)
			{
				UIUtils.ShowTips("season_mastery_s2_landmine_tips_2", 3f, GameEntry.Localization.GetString(value.name, value.level));
			}
			GameEntry.Timer.RegisterTimer(num, delegate
			{
				if (world != null)
				{
					PointInfo pointInfo = world.GetPointInfo(pointId);
					if (pointInfo != null && pointInfo is BuildPointInfo && plotId > 0)
					{
						GameEntry.Lua.Call("CSharpCallLuaInterface.PlayWorldBuildTopBubblePlot", plotId, pointInfo.uuid, pointInfo.ownerUid);
					}
				}
				if (uid == GameEntry.Data.Player.Uid)
				{
					long serverTime = GameEntry.Timer.GetServerTime();
					if (vibrationTime + 3000 < serverTime)
					{
						vibrationTime = serverTime;
						if (Vibrator.HapticsSupported())
						{
							Vibrator.LightImpact();
						}
					}
				}
			});
		}
		pool.Recycle(worldTriggerData);
	}

	public void HandlePushWorldLightUpdate(ISFSObject message)
	{
		ByteArray byteArray = message.GetByteArray("lightDataChange");
		if (byteArray?.Bytes != null)
		{
			PushLightChange pushLightChange = PushLightChange.Parser.ParseFrom(byteArray.Bytes);
			if (pushLightChange != null)
			{
				LightDataManager.GetInstance().HandleLightDataChange(pushLightChange, world);
			}
		}
	}

	private long AddOrUpdateOneTrigger(ISFSObject msg, int serverId, int worldId)
	{
		long num = msg.TryGetLong("uuid");
		if (!allTriggers.TryGetValue(num, out var value))
		{
			value = pool.Allocate();
		}
		value.ParseData(msg, serverId, worldId);
		if (triggerConfigs.TryGetValue(value.cfgId, out var value2))
		{
			value.SetConfig(value2);
		}
		else
		{
			Log.Error("WorldTriggerConfig cant find! id=" + value.cfgId);
		}
		allTriggers[num] = value;
		allUuidSet.Add(num);
		world.CreateOrRefreshOneTrigger(value);
		return num;
	}

	private void DeleteOneTrigger(long uuid)
	{
		if (allTriggers.TryGetValue(uuid, out var value))
		{
			pool.Recycle(value);
			allTriggers.Remove(uuid);
		}
		allUuidSet.Remove(uuid);
		world.RemoveOneTrigger(uuid);
	}

	public WorldTriggerData GetWorldTriggerData(long uuid)
	{
		if (allTriggers.TryGetValue(uuid, out var value))
		{
			return value;
		}
		return null;
	}

	public WorldTriggerData GetTriggerDataByPointId(int pointId, int serverId)
	{
		Vector2Int vector2Int = TileCoord.IndexToTilePos(pointId, ForceChangeScene.World);
		foreach (WorldTriggerData value in allTriggers.Values)
		{
			if (value.IsInRange(vector2Int.x, vector2Int.y, serverId))
			{
				return value;
			}
		}
		return null;
	}
}
