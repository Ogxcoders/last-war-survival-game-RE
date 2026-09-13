using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FibMatrix;
using GameFramework;
using Protobuf;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VEngine;

public class WorldTroopManager : WorldManagerBase
{
	public class LittleSmart : LittleSmartUpdater<WorldTroopManager, LittleSmart>
	{
		private Dictionary<long, int> uuid2Index;

		private List<WorldTroopLittleSmart> troopList;

		private Queue<int> emptyIndex;

		private long serverTime;

		private WorldGpuInstancingRenderer renderer;

		private Asset materialRequester;

		private int lastCount;

		private List<WorldMarch> tempList = new List<WorldMarch>(1024);

		private const int MAX_CONVERT_PER_FRAME = 20;

		private bool converting;

		protected override string LuaCheckEnableFuncName => "CSharpCallLuaInterface.IsLittleSmartTroopEnable";

		protected override EventId ChangeTroopModeEventId => EventId.ChangeLittleSmartTroopMode;

		public override string EditorDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("---World Troop Manager---");
			stringBuilder.AppendLine("小聪明模式是否开启:" + (IsLittleSmartModeEnable ? "开启" : "关闭"));
			stringBuilder.AppendLine($"当前行军数量:{host.troopsDict.Count}");
			stringBuilder.AppendLine($"当前行军(小聪明)数量:{uuid2Index.Count}");
			if (host.troopsDict.Count > 0)
			{
				stringBuilder.AppendLine("各类型的数量为:");
				Dictionary<NewMarchType, int> dictionary = new Dictionary<NewMarchType, int>();
				foreach (KeyValuePair<long, StepUpdateTroop> item in host.troopsDict)
				{
					NewMarchType type = item.Value.troop.GetMarchInfo().type;
					if (dictionary.TryGetValue(type, out var value))
					{
						dictionary[type] = value + 1;
					}
					else
					{
						dictionary[type] = 1;
					}
				}
				foreach (KeyValuePair<NewMarchType, int> item2 in dictionary)
				{
					stringBuilder.AppendLine($"{item2.Key.ToString()}:{item2.Value}");
				}
			}
			return stringBuilder.ToString();
		}

		protected override void OnInit()
		{
			troopList = new List<WorldTroopLittleSmart>(1024);
			uuid2Index = new Dictionary<long, int>(1024);
			emptyIndex = new Queue<int>();
			materialRequester = GameEntry.Resource.LoadAssetAsync("Assets/Main/Material/TroopInstancing.mat", typeof(Material));
			Asset asset = materialRequester;
			asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
			{
				if (materialRequester != null && materialRequester.status == LoadableStatus.SuccessToLoad)
				{
					Material material = new Material(materialRequester.asset as Material);
					materialRequester.Release();
					materialRequester = null;
					renderer = WorldGpuInstancingRenderer.Create("WorldTroopSquad", WorldGpuInstancingUtils.CreateNormalMesh(), material, WorldTroopLittleSmart.RendererGroup.Create, RenderPassEvent.AfterRenderingTransparents, 15, 1, 7);
					OnRendererReady();
				}
			});
			converting = false;
			lastCount = 0;
		}

		private void OnRendererReady()
		{
			if (troopList == null)
			{
				return;
			}
			int i = 0;
			for (int count = troopList.Count; i < count; i++)
			{
				WorldTroopLittleSmart worldTroopLittleSmart = troopList[i];
				if (worldTroopLittleSmart != null && worldTroopLittleSmart.DataIndex < 0)
				{
					renderer.CreateIndex(worldTroopLittleSmart);
				}
			}
		}

		protected override void OnDispose()
		{
			if (materialRequester != null)
			{
				materialRequester.Release();
				materialRequester = null;
			}
			Clear();
			renderer?.DisposeRenderer();
		}

		protected override void OnLittleSmartModeChanged()
		{
			if (IsLittleSmartModeEnable)
			{
				tempList.Clear();
				if (host.troopsDict.Count <= 0)
				{
					return;
				}
				converting = true;
				foreach (KeyValuePair<long, StepUpdateTroop> item in host.troopsDict)
				{
					StepUpdateTroop value = item.Value;
					if (value.troop != null)
					{
						WorldMarch marchInfo = value.troop.GetMarchInfo();
						if (marchInfo != null && marchInfo.SupportLittleSmartTroopMode() && !value.troop.IsInstanced)
						{
							tempList.Add(marchInfo);
						}
					}
				}
				if (tempList.Count > 0)
				{
					int i = 0;
					for (int count = tempList.Count; i < count; i++)
					{
						WorldMarch worldMarch = tempList[i];
						RefreshOrCreateTroop(worldMarch);
						long uuid = worldMarch.uuid;
						host.DestroyTroopObj(uuid);
					}
					tempList.Clear();
				}
				if (host.createMarchDict.Count <= 0)
				{
					return;
				}
				foreach (KeyValuePair<long, WorldMarch> item2 in host.createMarchDict)
				{
					WorldMarch value2 = item2.Value;
					if (value2 != null && value2.SupportLittleSmartTroopMode())
					{
						tempList.Add(value2);
					}
				}
				if (tempList.Count > 0)
				{
					int j = 0;
					for (int count2 = tempList.Count; j < count2; j++)
					{
						WorldMarch worldMarch2 = tempList[j];
						RefreshOrCreateTroop(worldMarch2);
						long uuid2 = worldMarch2.uuid;
						host.createMarchDict.Remove(uuid2);
					}
					tempList.Clear();
				}
			}
			else
			{
				converting = false;
			}
		}

		protected override void OnLittleSmartUpdate(long serverTime, float delteTime)
		{
			if (converting)
			{
				int num = 20;
				foreach (KeyValuePair<long, StepUpdateTroop> item in host.troopsDict)
				{
					WorldMarch marchInfo = item.Value.troop.GetMarchInfo();
					if (marchInfo != null && marchInfo.SupportLittleSmartTroopMode())
					{
						tempList.Add(marchInfo);
						if (--num <= 0)
						{
							break;
						}
					}
				}
				if (tempList.Count > 0)
				{
					int i = 0;
					for (int count = tempList.Count; i < count; i++)
					{
						WorldMarch worldMarch = tempList[i];
						RefreshOrCreateTroop(worldMarch);
						host.DestroyTroopObj(worldMarch.uuid);
					}
					tempList.Clear();
				}
				else
				{
					converting = false;
				}
			}
			if (troopList.Count <= 0)
			{
				return;
			}
			int j = 0;
			for (int count2 = troopList.Count; j < count2; j++)
			{
				WorldTroopLittleSmart worldTroopLittleSmart = troopList[j];
				if (worldTroopLittleSmart != null)
				{
					worldTroopLittleSmart.Update(serverTime, delteTime);
					if (worldTroopLittleSmart.DataIndex >= 0)
					{
						renderer.UpdateData(worldTroopLittleSmart.DataIndex, worldTroopLittleSmart);
					}
				}
			}
		}

		public void RefreshOrCreateTroop(WorldMarch march)
		{
			if (!march.SupportLittleSmartTroopMode())
			{
				UnityEngine.Debug.LogError($"尝试让小聪明创建一个不支持的行军:{march.type}");
				return;
			}
			long uuid = march.uuid;
			WorldTroopLittleSmart worldTroopLittleSmart = null;
			if (!uuid2Index.TryGetValue(uuid, out var value))
			{
				worldTroopLittleSmart = WorldTroopLittleSmart.Create(uuid);
				renderer?.CreateIndex(worldTroopLittleSmart);
				if (emptyIndex.Count > 0)
				{
					value = emptyIndex.Dequeue();
					troopList[value] = worldTroopLittleSmart;
				}
				else
				{
					value = troopList.Count;
					troopList.Add(worldTroopLittleSmart);
				}
				uuid2Index.Add(uuid, value);
				worldTroopLittleSmart.InitByMarch(serverTime, march);
			}
			else
			{
				worldTroopLittleSmart = troopList[value];
				worldTroopLittleSmart.RefreshByMarch(serverTime, march);
			}
		}

		public void DestroyTroop(long marchUuid)
		{
			if (uuid2Index != null && uuid2Index.TryGetValue(marchUuid, out var value))
			{
				uuid2Index.Remove(marchUuid);
				emptyIndex.Enqueue(value);
				WorldTroopLittleSmart worldTroopLittleSmart = troopList[value];
				renderer?.ReleaseIndex(worldTroopLittleSmart);
				troopList[value] = null;
				WorldTroopLittleSmart.Recycle(worldTroopLittleSmart);
			}
		}

		private void Clear()
		{
			if (troopList == null || troopList.Count <= 0)
			{
				return;
			}
			int i = 0;
			for (int count = troopList.Count; i < count; i++)
			{
				if (troopList[i] != null)
				{
					WorldTroopLittleSmart.Recycle(troopList[i]);
				}
			}
			uuid2Index.Clear();
			troopList.Clear();
			emptyIndex.Clear();
		}

		public bool IsTroopCreate(long marchUuid)
		{
			if (uuid2Index != null)
			{
				return uuid2Index.ContainsKey(marchUuid);
			}
			return false;
		}

		protected override void OnDrawGizmos()
		{
			if (troopList.Count > 0)
			{
				int i = 0;
				for (int count = troopList.Count; i < count; i++)
				{
					troopList[i]?.OnDrawGizmos();
				}
			}
		}
	}

	private class StepUpdateTroop : IDisposable
	{
		public WorldTroop troop;

		public int updateFrame;

		public float updateElapsed;

		public void Dispose()
		{
			troop = null;
			updateFrame = 0;
			updateElapsed = 0f;
		}
	}

	private class BattleVFX
	{
		public float life;

		public InstanceRequest inst;
	}

	private LittleSmart littleSmart;

	private const float MaxFrameTime = 0.01f;

	private const float EdgeRateX = 0.08f;

	private const float EdgeRateY = 0.1f;

	private static ObjectPool<StepUpdateTroop> pool = new ObjectPool<StepUpdateTroop>();

	private Dictionary<long, StepUpdateTroop> troopsDict = new Dictionary<long, StepUpdateTroop>();

	private Stopwatch timer = new Stopwatch();

	private int updateId;

	private List<long> removeList = new List<long>();

	private Dictionary<long, WorldMarch> createMarchDict = new Dictionary<long, WorldMarch>();

	private InstanceRequest dragTroopLineInst;

	private WorldTroopLine dragTroopLine;

	private InstanceRequest _troopDestinationInst;

	private WorldTroopDestinationSignal _troopDestination;

	private Dictionary<NewMarchType, bool> _hideModelType = new Dictionary<NewMarchType, bool>();

	private int LOD;

	private int SelfMarchLod = 8;

	private int OtherMarchLod = 4;

	private List<BattleVFX> vfxList = new List<BattleVFX>();

	private float __debugLogTimer;

	private List<long> m_tempDestroyTroops = new List<long>();

	private Dictionary<long, int> battleTroopAndPointId = new Dictionary<long, int>();

	private Dictionary<int, Dictionary<long, Quaternion>> cacheBattleTroopRotationList = new Dictionary<int, Dictionary<long, Quaternion>>();

	public static WorldTroopManager EditorInstance => null;

	public static bool __DEBUG_LOG__ { get; private set; } = false;

	public static double time { get; private set; }

	public static bool busy => (double)Time.realtimeSinceStartup >= time;

	public int TroopCount => troopsDict.Count;

	public string EditorDescription()
	{
		return littleSmart?.EditorDescription();
	}

	public void RefreshOrCreateTroop(WorldMarch march)
	{
		if (march != null)
		{
			StepUpdateTroop value;
			if (march.SupportLittleSmartTroopMode() && littleSmart != null && littleSmart.IsLittleSmartModeEnable)
			{
				littleSmart.RefreshOrCreateTroop(march);
			}
			else if (troopsDict.TryGetValue(march.uuid, out value))
			{
				value.troop.Refresh(march);
			}
			else
			{
				createMarchDict[march.uuid] = march;
			}
		}
	}

	public WorldTroopManager(WorldScene scene)
		: base(scene)
	{
	}

	public override void Init()
	{
		__DEBUG_LOG__ = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsMarchStateDebugLogOpened");
		_hideModelType.Clear();
		LOD = world.GetLodLevel();
		GameEntry.Event.Subscribe(EventId.ShowWorldMarchByType, ShowWorldMarchByTypeSignal);
		GameEntry.Event.Subscribe(EventId.HideWorldMarchByType, HideWorldMarchByTypeSignal);
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnLodChanged);
		littleSmart?.Dispose();
		littleSmart = LittleSmartUpdater<WorldTroopManager, LittleSmart>.Create(this);
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.ShowWorldMarchByType, ShowWorldMarchByTypeSignal);
		GameEntry.Event.Unsubscribe(EventId.HideWorldMarchByType, HideWorldMarchByTypeSignal);
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnLodChanged);
		_hideModelType.Clear();
		foreach (StepUpdateTroop value in troopsDict.Values)
		{
			value.troop.Destroy(0L);
			pool.Recycle(value);
		}
		troopsDict.Clear();
		DestroyDragLine();
		DestroyTroopDestination();
		ClearBattleVFX();
		littleSmart?.Dispose();
		littleSmart = null;
	}

	public WorldTroop GetTroop(long marchUuid)
	{
		if (troopsDict.TryGetValue(marchUuid, out var value))
		{
			return value.troop;
		}
		return null;
	}

	public void CreateTroop(WorldMarch march)
	{
		RefreshOrCreateTroop(march);
	}

	public void RefreshNeedCreateTroop(WorldMarch march)
	{
		if (march != null && createMarchDict.ContainsKey(march.uuid))
		{
			RefreshOrCreateTroop(march);
		}
	}

	private float DestroyWorldTroop(long marchUuid, bool isBattleFailed = false)
	{
		WorldTroop troop = GetTroop(marchUuid);
		if (troop != null && troop.IsInstanced && !troop.IsDelayDestroyed)
		{
			WorldMarch marchInfo = troop.GetMarchInfo();
			if (marchInfo.type == NewMarchType.ZOMBIE_RUSH)
			{
				isBattleFailed = !isBattleFailed;
			}
			if (isBattleFailed)
			{
				if (marchInfo.type != NewMarchType.ZOMBIE_RUSH || troop.GetMarchInfo().IsInViewRect)
				{
					float num = 2.14f;
					if (marchInfo.type == NewMarchType.ACT_BERSERK_BOSS)
					{
						num = 3f;
					}
					else if (marchInfo.type == NewMarchType.ZOMBIE_RUSH)
					{
						num = (marchInfo.IsZombieRushAltered() ? 4.5f : 1.2f);
					}
					else if (marchInfo.type == NewMarchType.MUMMY || marchInfo.type == NewMarchType.RUNNING_MUMMY)
					{
						num = 5f;
					}
					else if (marchInfo.type == NewMarchType.BOSS)
					{
						if (troop.GetMonsterSpecialType() == 27)
						{
							num = 3.167f;
						}
						else if (marchInfo.IsAlChallengeKirov())
						{
							num = 1.5f;
						}
					}
					else if (marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
					{
						num = 4f;
					}
					troop.ShowBattleDefeat();
					troop.DelayDestroy(num);
					if (marchInfo.type != NewMarchType.ACT_BERSERK_BOSS)
					{
						troop.ShowZombieDead();
					}
					return num;
				}
			}
			else
			{
				if (troop.GetMarchInfo().IsEVP() && troop.GetMarchInfo().IsInViewRect)
				{
					float num2 = 5f;
					if (marchInfo.type == NewMarchType.MUMMY || marchInfo.type == NewMarchType.RUNNING_MUMMY)
					{
						troop.ShowBattleSuccess();
					}
					troop.DelayDestroy(num2);
					return num2;
				}
				if ((marchInfo.type == NewMarchType.BLOODY_QUEEN || troop.GetMarchInfo().IsInViewRect) && (troop.GetMonsterSpecialType() == 47 || troop.GetMonsterSpecialType() == 49) && GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.GetQueenOfBloodCityIsDie", marchInfo.bloodyQueenMonster.cityId))
				{
					float num3 = 5f;
					troop.ShowBloodyQueenDefendFailed();
					troop.DelayDestroy(num3);
					return num3;
				}
			}
		}
		createMarchDict.Remove(marchUuid);
		DestroyTroopObj(marchUuid);
		return 0f;
	}

	public float DestroyTroop(long marchUuid, bool isBattleFailed = false)
	{
		TryDestroyLittleSmartTroop(marchUuid);
		return DestroyWorldTroop(marchUuid, isBattleFailed);
	}

	public void TryDestroyLittleSmartTroop(long marchUuid)
	{
		littleSmart?.DestroyTroop(marchUuid);
	}

	private void DestroyLegarcyTroop(long marchUuid)
	{
	}

	public void UpdateTroop(WorldMarch march)
	{
		RefreshOrCreateTroop(march);
	}

	public void RefreshPosition(WorldMarch march)
	{
		if (troopsDict.TryGetValue(march.uuid, out var value))
		{
			value.troop.RefreshPosition();
		}
	}

	private void CreateTroopsAsync()
	{
		if (createMarchDict.Count <= 0)
		{
			return;
		}
		int num = 0;
		removeList.Clear();
		foreach (KeyValuePair<long, WorldMarch> item in createMarchDict)
		{
			if (busy)
			{
				break;
			}
			int num2 = OtherMarchLod;
			if (item.Value.ownerUid == GameEntry.Data.Player.Uid)
			{
				num2 = SelfMarchLod;
			}
			else if (CheckFifthLodMarchType(item.Value.type))
			{
				num2 = 5;
			}
			else if (item.Value.IsFlowerCar())
			{
				num2 = 8;
			}
			if (item.Value.IsValid)
			{
				if (LOD <= num2)
				{
					CreateTroopObj(item.Value);
				}
				else if (GameEntry.Data.Player.IsInBattleField(2) && item.Value.worldId > 0)
				{
					Log.Info($"WorldTroopManager::CreateTroops Async LOD checkFail uuid={item.Key}, LOD={LOD}, checkLog={num2}");
				}
			}
			removeList.Add(item.Key);
			num++;
		}
		foreach (long remove in removeList)
		{
			createMarchDict.Remove(remove);
		}
	}

	public void OnMonsterIceBroken(long uuid)
	{
		if (troopsDict.TryGetValue(uuid, out var value))
		{
			value.troop.OnMonsterIceBroken();
		}
	}

	public bool IsTroopCreate(long marchUuid)
	{
		if (littleSmart == null || !littleSmart.IsLittleSmartModeEnable)
		{
			if (!troopsDict.ContainsKey(marchUuid))
			{
				return createMarchDict.ContainsKey(marchUuid);
			}
			return true;
		}
		if ((littleSmart == null || !littleSmart.IsTroopCreate(marchUuid)) && !troopsDict.ContainsKey(marchUuid))
		{
			return createMarchDict.ContainsKey(marchUuid);
		}
		return true;
	}

	[Obsolete]
	public bool IsTroopInCreateQueue(long marchUuid)
	{
		return createMarchDict.ContainsKey(marchUuid);
	}

	public void OnDragUpdate(long marchUuid, Vector3 dragPosCurrent, long targetMarchUuid, int startPointId = 0, bool isFormation = false)
	{
		Vector3 one = Vector3.one;
		Vector3 touchPoint = world.GetTouchPoint();
		Vector3 one2 = Vector3.one;
		if (!isFormation)
		{
			WorldMarch march = world.GetMarch(marchUuid);
			if (march == null || march.ownerUid != GameEntry.Data.Player.Uid || march.type == NewMarchType.ASSEMBLY_MARCH)
			{
				DestroyDragLine();
				return;
			}
			world.CanMoving = false;
			if (march.status == MarchStatus.IN_WORM_HOLE)
			{
				UIUtils.ShowTips("129015", 3f);
				DestroyDragLine();
				if (_troopDestination != null)
				{
					_troopDestination.HideDestination();
				}
				return;
			}
			if (march.status == MarchStatus.ASSISTANCE || march.status == MarchStatus.COLLECTING)
			{
				one = SceneManager.World.TileIndexToWorld(march.targetPos);
			}
			else
			{
				one = march.position;
				WorldTroop troop = GetTroop(march.uuid);
				if (troop != null)
				{
					one = troop.GetPosition();
				}
			}
		}
		else
		{
			world.CanMoving = false;
			one = SceneManager.World.TileIndexToWorld(startPointId);
		}
		EdgeDragUpdate(dragPosCurrent);
		CreateDragLine();
		if (dragTroopLine != null)
		{
			dragTroopLine.SetDragPath(one, touchPoint);
		}
		one2 = touchPoint;
		int tileSize = 1;
		int num = world.WorldToTileIndex(world.GetTouchPoint());
		MarchTargetType targetType = GetTargetType(targetMarchUuid, num);
		EnumDestinationSignalType destinationType = GetDestinationType(marchUuid, targetMarchUuid, num, targetType, isFormation, ref one2, ref tileSize);
		float distanceToTarget = GetDistanceToTarget(one, one2);
		CreateTroopDestination();
		if (_troopDestination != null)
		{
			_troopDestination.SetDestination(one2, destinationType, targetType, tileSize, distanceToTarget);
		}
	}

	private void CreateDragLine()
	{
		if (dragTroopLineInst == null)
		{
			dragTroopLineInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopLineDrag.prefab");
			dragTroopLineInst.completed += delegate
			{
				dragTroopLineInst.gameObject.transform.SetParent(world.DynamicObjNode);
				dragTroopLine = dragTroopLineInst.gameObject.GetComponent<WorldTroopLine>();
			};
		}
	}

	private void DestroyDragLine()
	{
		if (dragTroopLineInst == null || !(dragTroopLine != null))
		{
			return;
		}
		dragTroopLine.HideDrag();
		GameEntry.Timer.RegisterTimer(1f, delegate
		{
			if (dragTroopLineInst != null)
			{
				dragTroopLineInst.Destroy();
				dragTroopLineInst = null;
				dragTroopLine = null;
			}
		});
	}

	private void CreateTroopDestination()
	{
		if (_troopDestinationInst == null)
		{
			_troopDestinationInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
			_troopDestinationInst.completed += delegate
			{
				_troopDestinationInst.gameObject.transform.SetParent(world.DynamicObjNode);
				_troopDestination = _troopDestinationInst.gameObject.GetComponent<WorldTroopDestinationSignal>();
			};
		}
	}

	private void DestroyTroopDestination()
	{
		if (_troopDestinationInst != null)
		{
			_troopDestinationInst.Destroy();
			_troopDestinationInst = null;
			_troopDestination = null;
		}
	}

	private float GetDistanceToTarget(Vector3 myPosV3, Vector3 targetPos)
	{
		Vector2Int a = world.WorldToTile(myPosV3);
		Vector2Int b = world.WorldToTile(targetPos);
		return world.TileDistance(a, b);
	}

	public EnumDestinationSignalType GetDestinationType(long marchUuid, long targetMarchUuid, int endPos, MarchTargetType targetType, bool isFormation, ref Vector3 realPos, ref int tileSize)
	{
		EnumDestinationSignalType result = EnumDestinationSignalType.None;
		if (marchUuid != 0L && !isFormation)
		{
			if (marchUuid == targetMarchUuid)
			{
				return result;
			}
			WorldMarch march = world.GetMarch(marchUuid);
			if (march == null || march.ownerUid != GameEntry.Data.Player.Uid || march.type == NewMarchType.ASSEMBLY_MARCH)
			{
				return result;
			}
			if (march.GetIsBroken())
			{
				return result;
			}
		}
		switch (targetType)
		{
		case MarchTargetType.RALLY_FOR_BOSS:
		case MarchTargetType.EXPLORE:
			result = EnumDestinationSignalType.None;
			break;
		case MarchTargetType.STATE:
			result = EnumDestinationSignalType.EmptyGround;
			break;
		case MarchTargetType.ATTACK_MONSTER:
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.DIRECT_ATTACK_ACT_BOSS:
		case MarchTargetType.MONSTER_INVASION_BOSS:
		{
			if (targetMarchUuid == 0L)
			{
				break;
			}
			WorldTroop troop = GetTroop(targetMarchUuid);
			if (troop == null)
			{
				break;
			}
			realPos = troop.GetPosition();
			switch (targetType)
			{
			case MarchTargetType.DIRECT_ATTACK_ACT_BOSS:
			case MarchTargetType.MONSTER_INVASION_BOSS:
			{
				WorldMarch march2 = world.GetMarch(targetMarchUuid);
				tileSize = 1;
				if (march2 != null && (march2.type == NewMarchType.ACT_BOSS || march2.type == NewMarchType.PUZZLE_BOSS || march2.type == NewMarchType.CHALLENGE_BOSS || march2.IsAisila()))
				{
					int num2 = GameEntry.Lua.CallWithReturn<int, string, int, string>("CSharpCallLuaInterface.GetTemplateData", "lw_world_monster", march2.monsterId, "size");
					if (num2 > 0)
					{
						tileSize = num2;
					}
				}
				break;
			}
			case MarchTargetType.ATTACK_ARMY:
				tileSize = 2;
				break;
			}
			result = EnumDestinationSignalType.EnemyMarch;
			break;
		}
		default:
		{
			if (endPos <= 0)
			{
				break;
			}
			PointInfo pointInfo = world.GetPointInfo(endPos);
			if (pointInfo != null)
			{
				if (pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_CITY)
				{
					tileSize = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetWorldPointTileSize", pointInfo.pointIndex);
				}
				else
				{
					tileSize = pointInfo.tileSize;
				}
				realPos = world.TileIndexToWorld(pointInfo.mainIndex);
				switch (targetType)
				{
				case MarchTargetType.COLLECT:
				case MarchTargetType.SAMPLE:
				case MarchTargetType.PICK_GARBAGE:
				case MarchTargetType.GOLLOES_EXPLORE:
					result = EnumDestinationSignalType.Other;
					break;
				case MarchTargetType.BACK_HOME:
					if (pointInfo.pointType == WorldPointType.PlayerRoad)
					{
						if (pointInfo is BoardPointInfo boardPointInfo3)
						{
							result = EnumDestinationSignalType.My;
							realPos = world.TileIndexToWorld(boardPointInfo3.inside);
							tileSize = 3;
						}
					}
					else if (pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo is BuildPointInfo buildPointInfo4)
					{
						result = EnumDestinationSignalType.My;
						realPos = world.TileIndexToWorld(buildPointInfo4.inside);
						tileSize = 3;
					}
					break;
				case MarchTargetType.ATTACK_BUILDING:
				case MarchTargetType.ATTACK_ARMY_COLLECT:
				case MarchTargetType.ATTACK_ROAD:
				case MarchTargetType.ATTACK_ALLIANCE_CITY:
				case MarchTargetType.ATTACK_THRONE:
				case MarchTargetType.ATTACK_WINTER_ENTITY:
				case MarchTargetType.ATTACK_CITY_STRONGHOLD:
				case MarchTargetType.ATTACK_EPIDEMIC_BUILDING:
					result = EnumDestinationSignalType.EnemyBuild;
					break;
				case MarchTargetType.ATTACK_CITY:
				case MarchTargetType.ATTACK_WINTER_STORM_CITY:
				case MarchTargetType.ATTACK_EPIDEMIC_CITY:
					if (pointInfo.pointType == WorldPointType.PlayerRoad)
					{
						if (pointInfo is BoardPointInfo boardPointInfo2)
						{
							result = EnumDestinationSignalType.EnemyBuild;
							realPos = world.TileIndexToWorld(boardPointInfo2.inside);
							tileSize = 3;
						}
					}
					else if (pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo is BuildPointInfo buildPointInfo2)
					{
						result = EnumDestinationSignalType.EnemyBuild;
						realPos = world.TileIndexToWorld(buildPointInfo2.inside);
						tileSize = 3;
					}
					break;
				case MarchTargetType.ASSISTANCE_CITY:
				case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
				case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
					if (pointInfo.pointType == WorldPointType.PlayerRoad)
					{
						if (pointInfo is BoardPointInfo boardPointInfo)
						{
							result = EnumDestinationSignalType.Alliance;
							realPos = world.TileIndexToWorld(boardPointInfo.inside);
							tileSize = 3;
						}
					}
					else if (pointInfo.pointType == WorldPointType.PlayerBuilding && pointInfo is BuildPointInfo buildPointInfo)
					{
						result = EnumDestinationSignalType.Alliance;
						realPos = world.TileIndexToWorld(buildPointInfo.inside);
						tileSize = 3;
					}
					break;
				case MarchTargetType.ASSISTANCE_ALLIANCE_CITY:
				case MarchTargetType.ASSISTANCE_THRONE:
					result = EnumDestinationSignalType.Alliance;
					break;
				case MarchTargetType.ASSISTANCE_BUILD:
					if (pointInfo is BuildPointInfo buildPointInfo3)
					{
						result = ((!(buildPointInfo3.ownerUid != GameEntry.Data.Player.Uid)) ? EnumDestinationSignalType.My : EnumDestinationSignalType.Alliance);
					}
					break;
				default:
					if (pointInfo.pointType == WorldPointType.DRAGON_BUILDING || pointInfo.pointType == WorldPointType.WINTER_ENTITY || pointInfo.pointType == WorldPointType.BATTLEFIELD_TYPE)
					{
						tileSize = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetWorldPointTileSize", pointInfo.pointIndex);
					}
					else if (pointInfo.pointType == WorldPointType.DRAGON_SCORE_POINT)
					{
						tileSize = 1;
					}
					break;
				}
			}
			else
			{
				if (targetType != MarchTargetType.ATTACK_CITY && targetType != MarchTargetType.ATTACK_WINTER_STORM_CITY && targetType != MarchTargetType.ATTACK_EPIDEMIC_CITY && targetType != MarchTargetType.ASSISTANCE_CITY && targetType != MarchTargetType.ASSISTANCE_WINTER_STORM_CITY && targetType != MarchTargetType.ASSISTANCE_EPIDEMIC_CITY && targetType != MarchTargetType.BACK_HOME)
				{
					break;
				}
				int num = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.CheckIsInBasementRange", endPos);
				if (num <= 0)
				{
					break;
				}
				PointInfo pointInfo2 = world.GetPointInfo(num);
				if (pointInfo2 != null)
				{
					tileSize = pointInfo2.tileSize;
					realPos = world.TileIndexToWorld(pointInfo2.mainIndex);
					switch (targetType)
					{
					case MarchTargetType.BACK_HOME:
						result = EnumDestinationSignalType.My;
						break;
					case MarchTargetType.ASSISTANCE_CITY:
					case MarchTargetType.ASSISTANCE_WINTER_STORM_CITY:
					case MarchTargetType.ASSISTANCE_EPIDEMIC_CITY:
						result = EnumDestinationSignalType.Alliance;
						break;
					default:
						result = EnumDestinationSignalType.EnemyBuild;
						break;
					}
				}
			}
			break;
		}
		}
		return result;
	}

	public MarchTargetType GetTargetType(long targetMarchUuid, int pointId)
	{
		MarchTargetType result = MarchTargetType.STATE;
		if (targetMarchUuid != 0L)
		{
			WorldMarch march = world.GetMarch(targetMarchUuid);
			if (march != null && march.ownerUid != GameEntry.Data.Player.Uid)
			{
				if (march.IsMonster() || march.type == NewMarchType.CHALLENGE_BOSS)
				{
					result = MarchTargetType.ATTACK_MONSTER;
				}
				else if (march.type == NewMarchType.ACT_BOSS || march.type == NewMarchType.PUZZLE_BOSS)
				{
					result = MarchTargetType.DIRECT_ATTACK_ACT_BOSS;
				}
				else if (march.IsOrdinaryBoss())
				{
					result = MarchTargetType.RALLY_FOR_BOSS;
				}
				else if (march.type != NewMarchType.EXPLORE && march.type != NewMarchType.SCOUT && march.type != NewMarchType.RESOURCE_HELP)
				{
					if (march.type == NewMarchType.GOLLOES_EXPLORE)
					{
						result = MarchTargetType.GOLLOES_EXPLORE;
					}
					else if (march.type == NewMarchType.GOLLOES_TRADE)
					{
						result = MarchTargetType.GOLLOES_TRADE;
					}
					else if (!march.GetIsBroken())
					{
						string allianceId = GameEntry.Data.Player.GetAllianceId();
						if (allianceId.IsNullOrEmpty() || !(allianceId == march.allianceUid))
						{
							result = MarchTargetType.ATTACK_ARMY;
						}
					}
				}
			}
		}
		else if (pointId > 0 && world.IsTileWalkable(world.TileIndexToWorld(pointId)))
		{
			PointInfo pointInfo = world.GetPointInfo(pointId);
			if (!world.IsCollectRangePoint(pointId) || pointInfo != null)
			{
				if (pointInfo != null && pointInfo.pointType != WorldPointType.Other)
				{
					if (pointInfo.pointType == WorldPointType.WorldCollectResource)
					{
						result = MarchTargetType.COLLECT;
					}
					else if (pointInfo.pointType == WorldPointType.EXPLORE_POINT)
					{
						result = MarchTargetType.EXPLORE;
					}
					else if (pointInfo.pointType == WorldPointType.SAMPLE_POINT || pointInfo.pointType == WorldPointType.SAMPLE_POINT_NEW)
					{
						result = MarchTargetType.SAMPLE;
					}
					else if (pointInfo.pointType == WorldPointType.RESCUE_POINT)
					{
						result = MarchTargetType.RESCUE_FAKE_MARCH;
					}
					else if (pointInfo.pointType == WorldPointType.HERO_DISPATCH)
					{
						result = MarchTargetType.DISPATCH_TASK;
					}
					else if (pointInfo.pointType == WorldPointType.GARBAGE)
					{
						result = MarchTargetType.PICK_GARBAGE;
					}
					else if (pointInfo.pointType == WorldPointType.WorldResource)
					{
						if (pointInfo is ResPointInfo { gatherMarchUuid: not 0L } resPointInfo)
						{
							string allianceId2 = GameEntry.Data.Player.GetAllianceId();
							WorldMarch march2 = world.GetMarch(resPointInfo.gatherMarchUuid);
							if (march2 != null && march2.ownerUid != GameEntry.Data.Player.Uid && (allianceId2.IsNullOrEmpty() || !(march2.allianceUid == allianceId2)))
							{
								result = MarchTargetType.ATTACK_ARMY_COLLECT;
							}
						}
					}
					else if (pointInfo.pointType == WorldPointType.PlayerBuilding)
					{
						if (pointInfo is BuildPointInfo buildPointInfo)
						{
							if (buildPointInfo.ownerUid == GameEntry.Data.Player.Uid)
							{
								result = ((buildPointInfo.inside == 0) ? MarchTargetType.ASSISTANCE_BUILD : MarchTargetType.BACK_HOME);
							}
							else
							{
								string allianceId3 = GameEntry.Data.Player.GetAllianceId();
								if (allianceId3.IsNullOrEmpty() || !(buildPointInfo.allianceId == allianceId3))
								{
									result = ((buildPointInfo.inside == 0) ? MarchTargetType.ATTACK_BUILDING : (GameEntry.Data.Player.GetWorldType() switch
									{
										2 => MarchTargetType.ATTACK_WINTER_STORM_CITY, 
										3 => MarchTargetType.ATTACK_EPIDEMIC_CITY, 
										_ => MarchTargetType.ATTACK_CITY, 
									}));
								}
								else if (buildPointInfo.inside != 0)
								{
									switch (GameEntry.Data.Player.GetWorldType())
									{
									case 2:
										result = MarchTargetType.ASSISTANCE_WINTER_STORM_CITY;
										break;
									case 3:
									case 4:
										result = MarchTargetType.ASSISTANCE_EPIDEMIC_CITY;
										break;
									default:
										result = MarchTargetType.ASSISTANCE_CITY;
										break;
									}
								}
								else
								{
									result = MarchTargetType.ASSISTANCE_BUILD;
								}
							}
						}
					}
					else if (pointInfo.pointType == WorldPointType.WORLD_CITY_STRONGHOLD || pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_CITY)
					{
						string text = GameEntry.Lua.CallWithReturn<string, int>("WorldBuildUtil.GetBuildAllianceId", pointInfo.pointIndex);
						string allianceId4 = GameEntry.Data.Player.GetAllianceId();
						result = ((pointInfo.pointType == WorldPointType.WORLD_CITY_STRONGHOLD) ? ((allianceId4.IsNullOrEmpty() || !(text == allianceId4)) ? MarchTargetType.ATTACK_CITY_STRONGHOLD : MarchTargetType.ASSISTANCE_CITY_STRONGHOLD) : ((allianceId4.IsNullOrEmpty() || !(text == allianceId4)) ? MarchTargetType.ATTACK_ALLIANCE_CITY : MarchTargetType.ASSISTANCE_ALLIANCE_CITY));
					}
					else if (pointInfo.pointType == WorldPointType.WORLD_CITY_OUTPOST)
					{
						if (pointInfo is WorldOutpostPoint worldOutpostPoint)
						{
							long serverTime = GameEntry.Timer.GetServerTime();
							if (worldOutpostPoint.battleStartTime > serverTime)
							{
								int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
								result = (((worldOutpostPoint.tmpOwnerServerId != 0 || worldOutpostPoint.ownerServerId != sourceServerId) && worldOutpostPoint.tmpOwnerServerId != sourceServerId) ? MarchTargetType.ATTACK_OUTPOST_BUILDING : MarchTargetType.ASSISTANCE_OUTPOST_BUILDING);
							}
						}
					}
					else if (pointInfo.pointType == WorldPointType.WORLD_CITY_OUTPOST_TOWER)
					{
						if (pointInfo is WorldOutpostTowerPoint worldOutpostTowerPoint)
						{
							long serverTime2 = GameEntry.Timer.GetServerTime();
							if (worldOutpostTowerPoint.battleStartTime > serverTime2)
							{
								int sourceServerId2 = GameEntry.Data.Player.GetSourceServerId();
								result = ((worldOutpostTowerPoint.tmpOwnerServerId != sourceServerId2) ? MarchTargetType.ATTACK_OUTPOST_BUILDING : MarchTargetType.ASSISTANCE_OUTPOST_BUILDING);
							}
						}
					}
					else if (pointInfo.pointType == WorldPointType.DRAGON_BUILDING)
					{
						string allianceId5 = GameEntry.Data.Player.GetAllianceId();
						DragonPointInfo dragonPointInfo = pointInfo as DragonPointInfo;
						DragonBuildingPointInfo dragonBuildingPointInfo = null;
						dragonBuildingPointInfo = ((dragonPointInfo == null) ? DragonBuildingPointInfo.Parser.ParseFrom(pointInfo.extraInfo) : dragonPointInfo.detail);
						result = ((dragonBuildingPointInfo == null || !(dragonBuildingPointInfo.AllianceId == allianceId5)) ? MarchTargetType.ATTACK_DRAGON_BUILDING : MarchTargetType.ASSISTANCE_DRAGON_BUILDING);
					}
					else if (pointInfo.pointType == WorldPointType.DRAGON_SCORE_POINT)
					{
						result = MarchTargetType.SCOUT_DRAGON_SCORE;
					}
					else if (pointInfo.pointType == WorldPointType.PlayerRoad)
					{
						if (pointInfo is BoardPointInfo boardPointInfo)
						{
							if (boardPointInfo.ownerUid == GameEntry.Data.Player.Uid)
							{
								result = ((boardPointInfo.inside != 0) ? MarchTargetType.BACK_HOME : MarchTargetType.STATE);
							}
							else
							{
								string allianceId6 = GameEntry.Data.Player.GetAllianceId();
								if (allianceId6.IsNullOrEmpty() || !(boardPointInfo.allianceId == allianceId6))
								{
									result = ((boardPointInfo.inside == 0) ? MarchTargetType.ATTACK_ROAD : (GameEntry.Data.Player.GetWorldType() switch
									{
										2 => MarchTargetType.ATTACK_WINTER_STORM_CITY, 
										3 => MarchTargetType.ATTACK_EPIDEMIC_CITY, 
										_ => MarchTargetType.ATTACK_CITY, 
									}));
								}
								else if (boardPointInfo.inside != 0)
								{
									switch (GameEntry.Data.Player.GetWorldType())
									{
									case 2:
										result = MarchTargetType.ASSISTANCE_WINTER_STORM_CITY;
										break;
									case 3:
									case 4:
										result = MarchTargetType.ASSISTANCE_EPIDEMIC_CITY;
										break;
									default:
										result = MarchTargetType.ASSISTANCE_CITY;
										break;
									}
								}
								else
								{
									result = MarchTargetType.STATE;
								}
							}
						}
					}
					else if (pointInfo.pointType == WorldPointType.WINTER_ENTITY)
					{
						WinterStormPointInfo winterStormPointInfo = pointInfo as WinterStormPointInfo;
						WinterEntityPointInfo winterEntityPointInfo = null;
						winterEntityPointInfo = ((winterStormPointInfo == null) ? WinterEntityPointInfo.Parser.ParseFrom(pointInfo.extraInfo) : winterStormPointInfo.detail);
						result = (GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", winterEntityPointInfo.OwnerUid, 2) ? MarchTargetType.ATTACK_WINTER_ENTITY : MarchTargetType.ASSISTANCE_WINTER_ENTITY);
					}
					else if (pointInfo.pointType == WorldPointType.BATTLEFIELD_TYPE)
					{
						BattlefieldBuildPointInfo battlefieldBuildPointInfo = pointInfo as BattlefieldBuildPointInfo;
						QuarantinePointInfo quarantinePointInfo = null;
						if (battlefieldBuildPointInfo != null)
						{
							quarantinePointInfo = battlefieldBuildPointInfo.detail;
						}
						if (quarantinePointInfo != null)
						{
							string tabName = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetBattleFieldEntityCfgName", GameEntry.Data?.Player?.GetWorldType() ?? 0);
							result = ((!(GameEntry.ConfigCache.GetTemplateData(tabName, quarantinePointInfo.BuildId, "type") == "6")) ? (GameEntry.Lua.CallWithReturn<bool, int, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", quarantinePointInfo.Role, GameEntry.Data?.Player?.GetWorldType() ?? 0) ? MarchTargetType.ATTACK_EPIDEMIC_BUILDING : MarchTargetType.ASSISTANCE_EPIDEMIC_BUILDING) : MarchTargetType.PIC_EPIDEMIC_SCORE);
						}
					}
				}
				else
				{
					int num = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.CheckIsInBasementRange", pointId);
					if (num > 0)
					{
						PointInfo pointInfo2 = world.GetPointInfo(num);
						if (pointInfo2 != null && pointInfo2.pointType == WorldPointType.PlayerBuilding)
						{
							if (pointInfo2 is BuildPointInfo { itemId: 10100000 } buildPointInfo2)
							{
								if (buildPointInfo2.ownerUid == GameEntry.Data.Player.Uid)
								{
									result = MarchTargetType.BACK_HOME;
								}
								else
								{
									string allianceId7 = GameEntry.Data.Player.GetAllianceId();
									if (allianceId7.IsNullOrEmpty() || !(buildPointInfo2.allianceId == allianceId7))
									{
										result = GameEntry.Data.Player.GetWorldType() switch
										{
											2 => MarchTargetType.ATTACK_WINTER_STORM_CITY, 
											3 => MarchTargetType.ATTACK_EPIDEMIC_CITY, 
											_ => MarchTargetType.ATTACK_CITY, 
										};
									}
									else
									{
										switch (GameEntry.Data.Player.GetWorldType())
										{
										case 2:
											result = MarchTargetType.ASSISTANCE_WINTER_STORM_CITY;
											break;
										case 3:
										case 4:
											result = MarchTargetType.ASSISTANCE_EPIDEMIC_CITY;
											break;
										default:
											result = MarchTargetType.ASSISTANCE_CITY;
											break;
										}
									}
								}
							}
						}
						else
						{
							result = MarchTargetType.STATE;
						}
					}
					else
					{
						result = MarchTargetType.STATE;
					}
				}
			}
		}
		return result;
	}

	public void OnDragStop(long marchUuid, long targetMarchUuid, bool isFormation = false)
	{
		if (!isFormation)
		{
			int param = world.WorldToTileIndex(world.GetTouchPoint());
			GameEntry.Lua.Call("MarchUtil.OnChangeSingleMarch", marchUuid, targetMarchUuid, param);
		}
		else
		{
			int param2 = world.WorldToTileIndex(world.GetTouchPoint());
			GameEntry.Lua.Call("MarchUtil.OnChangeSingleFormation", marchUuid, targetMarchUuid, param2);
		}
		DestroyDragLine();
		if (_troopDestination != null)
		{
			_troopDestination.SetDestinationOver();
		}
	}

	public void OnLodChanged(object userdata)
	{
		int lOD = (int)userdata;
		LOD = lOD;
		UpdateTroopWhenLoadChanged();
	}

	private void UpdateTroopWhenLoadChanged()
	{
		foreach (KeyValuePair<long, StepUpdateTroop> item in troopsDict)
		{
			item.Value.troop.UpdateWhenLodChange(LOD);
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (__DEBUG_LOG__)
		{
			__debugLogTimer += deltaTime;
		}
		littleSmart?.UpdateLittleSmart(deltaTime);
		time = Time.realtimeSinceStartup + 0.01f;
		UpdateAllTroops(deltaTime);
		UpdateBattleVFX(deltaTime);
		CreateTroopsAsync();
		if (__debugLogTimer > 60f)
		{
			__debugLogTimer = 0f;
		}
	}

	private void EdgeDragUpdate(Vector3 dragPosCurrent)
	{
		float num = dragPosCurrent.x / (float)Screen.width;
		float num2 = dragPosCurrent.y / (float)Screen.height;
		if (num < 0.08f || num > 0.92f || num2 < 0.1f || num2 > 0.9f)
		{
			Vector3 touchPoint = world.GetTouchPoint(dragPosCurrent);
			Vector3 curTarget = world.CurTarget;
			Vector3 vector = touchPoint - curTarget;
			float magnitude = vector.magnitude;
			if (magnitude > 0.1f)
			{
				Vector3 vector2 = vector / magnitude;
				float num3 = 1f * Time.deltaTime * world.GetLodDistance();
				Vector3 lookWorldPosition = curTarget + vector2 * num3;
				world.Lookat(lookWorldPosition);
			}
		}
	}

	private void UpdateAllTroops(float deltaTime)
	{
		if (ClientSwitch.IsOn(37))
		{
			StepUpdateAllTroops(deltaTime);
		}
		else
		{
			UpdateAllTroopsOld(deltaTime);
		}
	}

	private void StepUpdateAllTroops(float deltaTime)
	{
		int num = 4;
		bool flag = true;
		timer.Restart();
		m_tempDestroyTroops.Clear();
		foreach (KeyValuePair<long, StepUpdateTroop> item in troopsDict)
		{
			StepUpdateTroop value = item.Value;
			value.updateElapsed += deltaTime;
			if (value.updateFrame == updateId)
			{
				continue;
			}
			if (num <= 0 && timer.ElapsedMilliseconds >= 2)
			{
				flag = false;
				continue;
			}
			num--;
			value.updateFrame = updateId;
			value.troop.OnUpdate(value.updateElapsed);
			value.updateElapsed = 0f;
			if (value.troop.IsInvalid)
			{
				if (__debugLogTimer > 60f)
				{
					Log.Warning($"MarchDebugLog -> WorldTroopManager.UpdateAllTroops marchInfo[{item.Key}] is expired.");
				}
				m_tempDestroyTroops.Add(item.Key);
			}
			else if (item.Value.troop.IsDelayDestroyed)
			{
				m_tempDestroyTroops.Add(item.Key);
			}
		}
		foreach (long tempDestroyTroop in m_tempDestroyTroops)
		{
			DestroyTroop(tempDestroyTroop);
		}
		timer.Stop();
		if (flag)
		{
			updateId++;
		}
	}

	private void UpdateAllTroopsOld(float deltaTime)
	{
		m_tempDestroyTroops.Clear();
		foreach (KeyValuePair<long, StepUpdateTroop> item in troopsDict)
		{
			item.Value.troop.OnUpdate(deltaTime);
			if (item.Value.troop.IsInvalid)
			{
				if (__debugLogTimer > 60f)
				{
					Log.Warning($"MarchDebugLog -> WorldTroopManager.UpdateAllTroops marchInfo[{item.Key}] is expired.");
				}
				m_tempDestroyTroops.Add(item.Key);
			}
			else if (item.Value.troop.IsDelayDestroyed)
			{
				m_tempDestroyTroops.Add(item.Key);
			}
		}
		foreach (long tempDestroyTroop in m_tempDestroyTroops)
		{
			DestroyTroop(tempDestroyTroop);
		}
	}

	private WorldTroop CreateTroopObj(WorldMarch march)
	{
		if (!troopsDict.TryGetValue(march.uuid, out var value))
		{
			if (littleSmart != null && littleSmart.IsLittleSmartModeEnable && march.SupportLittleSmartTroopMode())
			{
				UnityEngine.Debug.LogError("[WorldNew]CreateTroopObj...");
			}
			value = pool.Allocate();
			value.troop = new WorldTroop(world);
			value.updateFrame = -1;
			value.updateElapsed = 0f;
			value.troop.Create(march, this);
			troopsDict.Add(march.uuid, value);
			bool visible = CanShow(march);
			value.troop.SetVisible(visible);
		}
		else
		{
			RefreshOrCreateTroop(march);
		}
		return value.troop;
	}

	private void DestroyTroopObj(long marchUuid)
	{
		if (troopsDict.TryGetValue(marchUuid, out var value))
		{
			troopsDict.Remove(marchUuid);
			value.troop.Destroy(marchUuid);
			pool.Recycle(value);
		}
	}

	public void CreateBattleVFX(string prefabPath, float life, Action<GameObject> onComplete)
	{
		InstanceRequest inst = GameEntry.Resource.InstantiateAsync(prefabPath);
		vfxList.Add(new BattleVFX
		{
			life = life,
			inst = inst
		});
		inst.completed += delegate
		{
			onComplete?.Invoke(inst.gameObject);
		};
	}

	private void UpdateBattleVFX(float deltaTime)
	{
		for (int i = 0; i < vfxList.Count; i++)
		{
			vfxList[i].life -= deltaTime;
			if (vfxList[i].life <= 0f)
			{
				vfxList[i].inst.Destroy();
				vfxList.RemoveAt(i);
				i--;
			}
		}
	}

	private void ClearBattleVFX()
	{
		for (int i = 0; i < vfxList.Count; i++)
		{
			vfxList[i].inst.Destroy();
		}
		vfxList.Clear();
	}

	public void OnDrawGizmos()
	{
		foreach (StepUpdateTroop value in troopsDict.Values)
		{
			value.troop.OnDrawGizmos();
		}
	}

	public float GetModelHeight(long marchUuid)
	{
		return GetTroop(marchUuid)?.GetHeight() ?? 0f;
	}

	public WorldTroop CreateGroupTroop(WorldMarch march)
	{
		return CreateTroopObj(march);
	}

	public int GetCurPosAndRotationTroopNum(long marchUuid, int pointId, Quaternion rot)
	{
		int result = 0;
		if (battleTroopAndPointId.ContainsKey(marchUuid))
		{
			int num = battleTroopAndPointId[marchUuid];
			if (num != pointId && cacheBattleTroopRotationList.ContainsKey(num))
			{
				Dictionary<long, Quaternion> dictionary = cacheBattleTroopRotationList[num];
				if (dictionary.ContainsKey(marchUuid))
				{
					dictionary.Remove(marchUuid);
				}
			}
		}
		battleTroopAndPointId[marchUuid] = pointId;
		if (cacheBattleTroopRotationList.ContainsKey(pointId))
		{
			Dictionary<long, Quaternion> dictionary2 = cacheBattleTroopRotationList[pointId];
			if (dictionary2.ContainsKey(marchUuid) && dictionary2[marchUuid].Equals(rot))
			{
				return -1;
			}
			int num2 = 0;
			foreach (KeyValuePair<long, Quaternion> item in dictionary2)
			{
				if (item.Value.Equals(rot))
				{
					num2++;
				}
			}
			dictionary2[marchUuid] = rot;
			return num2;
		}
		Dictionary<long, Quaternion> value = new Dictionary<long, Quaternion> { { marchUuid, rot } };
		cacheBattleTroopRotationList[pointId] = value;
		return result;
	}

	public void RemovePosAndRotationDataByMarchUuid(long marchUuid)
	{
		if (!battleTroopAndPointId.ContainsKey(marchUuid))
		{
			return;
		}
		int key = battleTroopAndPointId[marchUuid];
		battleTroopAndPointId.Remove(marchUuid);
		if (cacheBattleTroopRotationList.ContainsKey(key))
		{
			Dictionary<long, Quaternion> dictionary = cacheBattleTroopRotationList[key];
			if (dictionary.ContainsKey(marchUuid))
			{
				dictionary.Remove(marchUuid);
			}
		}
	}

	public void ShowWorldMarchByTypeSignal(object userData)
	{
		if (userData != null)
		{
			NewMarchType marchType = (NewMarchType)(long)userData;
			SetModelVisibleByMarchType(marchType, visible: true);
		}
	}

	public void HideWorldMarchByTypeSignal(object userData)
	{
		if (userData != null)
		{
			NewMarchType marchType = (NewMarchType)(long)userData;
			SetModelVisibleByMarchType(marchType, visible: false);
		}
	}

	public void SetModelVisibleByMarchTypeForLua(int marchType, bool visible)
	{
		SetModelVisibleByMarchType((NewMarchType)marchType, visible);
	}

	private void SetModelVisibleByMarchType(NewMarchType marchType, bool visible)
	{
		if (visible)
		{
			if (_hideModelType.ContainsKey(marchType))
			{
				_hideModelType.Remove(marchType);
			}
		}
		else if (!_hideModelType.ContainsKey(marchType))
		{
			_hideModelType.Add(marchType, value: true);
		}
		foreach (StepUpdateTroop value in troopsDict.Values)
		{
			if (GetHideTypeByMarchInfo(value.troop.GetMarchInfo()) == marchType)
			{
				value.troop.SetVisible(visible);
			}
		}
	}

	private NewMarchType GetHideTypeByMarchInfo(WorldMarch marchInfo)
	{
		if (marchInfo != null)
		{
			if (marchInfo.type == NewMarchType.MONSTER && !string.IsNullOrEmpty(marchInfo.eventId))
			{
				return NewMarchType.EXPLORE;
			}
			return marchInfo.type;
		}
		return NewMarchType.DEFAULT;
	}

	public bool CanShow(WorldMarch marchInfo)
	{
		NewMarchType hideTypeByMarchInfo = GetHideTypeByMarchInfo(marchInfo);
		return !_hideModelType.ContainsKey(hideTypeByMarchInfo);
	}

	private bool CheckFifthLodMarchType(NewMarchType type)
	{
		if (type != NewMarchType.RUNNING_BOSS && type != NewMarchType.RUNNING_MUMMY)
		{
			return type == NewMarchType.BEHEMOTH_BOSS;
		}
		return true;
	}
}
