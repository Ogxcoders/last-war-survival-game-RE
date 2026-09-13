using System.Collections.Generic;
using FibMatrix;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;

public class WorldWerewolfManager : WorldManagerBase
{
	private List<WorldWerewolfObject> allWerewolfObjects;

	private ObjectPool<WorldWerewolfObject> objectPool;

	private int curLod;

	private bool SupportGPUInstancing;

	private int maxHp;

	private const string ANIM_ATTACK = "attack";

	private Dictionary<int, byte> statusChange = new Dictionary<int, byte>();

	public int MaxHp
	{
		get
		{
			if (maxHp == 0)
			{
				maxHp = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetWerewolfMaxHp");
			}
			return maxHp;
		}
	}

	public WorldWerewolfManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		curLod = world.GetLodLevel();
		SupportGPUInstancing = WorldInstancingRenderers.DeviceSupportInstancing;
		allWerewolfObjects = new List<WorldWerewolfObject>();
		objectPool = new ObjectPool<WorldWerewolfObject>();
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnLodChanged);
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnLodChanged);
		ClearAll();
		objectPool.Dispose();
		maxHp = 0;
		statusChange.Clear();
	}

	private void ClearAll()
	{
		objectPool.Recycle(allWerewolfObjects);
	}

	public void HandleWorldGetBlock(WorldView2WolfPointInfoMsg msg)
	{
		ClearAll();
		if (msg == null)
		{
			return;
		}
		RepeatedField<WorldWolfPoints> list = msg.List;
		if (list == null)
		{
			return;
		}
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		int worldMainPos = GameEntry.Data.Building.GetWorldMainPos();
		foreach (WorldWolfPoints item in list)
		{
			RepeatedField<int> pointId = item.PointId;
			if (pointId == null)
			{
				continue;
			}
			string allianceId2 = item.AllianceId;
			bool isAlly = allianceId == allianceId2 && !string.IsNullOrEmpty(allianceId);
			foreach (int item2 in pointId)
			{
				if (item2 != worldMainPos)
				{
					WorldWerewolfObject worldWerewolfObject = objectPool.Allocate();
					worldWerewolfObject.Init(world, item2, isAlly, SupportGPUInstancing);
					allWerewolfObjects.Add(worldWerewolfObject);
				}
			}
		}
	}

	private void OnLodChanged(object userdata)
	{
		int num = (int)userdata;
		if (curLod >= 6 && num < 6)
		{
			ClearAll();
		}
		curLod = num;
	}

	public void HandlePushWolfStatusChange(ISFSObject msg)
	{
		if (world.CurrentLodLevel > 2)
		{
			return;
		}
		int num = msg.GetInt("type");
		switch (num)
		{
		case 4:
		{
			int pointIndex = msg.GetInt("pointId");
			WorldBuilding worldBuildingByPoint = world.GetWorldBuildingByPoint(pointIndex);
			if (worldBuildingByPoint != null)
			{
				worldBuildingByPoint.PlayTimeline("attack");
			}
			break;
		}
		case 2:
		case 3:
		case 5:
		{
			int key = msg.GetInt("pointId");
			statusChange[key] = (byte)num;
			break;
		}
		case 1:
			break;
		}
	}

	public WerewolfAnimState GetAnimState(int pointId)
	{
		WerewolfAnimState result = WerewolfAnimState.None;
		if (statusChange.TryGetValue(pointId, out var value))
		{
			result = (WerewolfAnimState)value;
			statusChange.Remove(pointId);
		}
		return result;
	}
}
