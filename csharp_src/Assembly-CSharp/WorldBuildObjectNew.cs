using System.Collections.Generic;
using GameFramework;
using UnityEngine;

public class WorldBuildObjectNew : WorldPointObject, IWorldLodWatcher
{
	private int debugGPUInstanceProp = -1;

	private WorldIconRendererFacade.HappyIcon happyHouse;

	private WorldIconRendererFacade.HappyIcon happyPin;

	private WorldIconRendererFacade.HappyIcon happyLevel;

	private WorldIconRendererFacade.HappyIcon happyShell;

	private WorldIconRendererFacade.HappyIcon happyAssistance;

	private static Dictionary<int, int> _happyLvSpecial = new Dictionary<int, int>
	{
		{ 205, 201 },
		{ 210, 202 },
		{ 250, 202 },
		{ 300, 203 },
		{ 999, 204 }
	};

	public const int MAX_CITY_DEFENCE = 10000;

	private WorldBuilding cityBuilding;

	public long bUuid;

	private bool isDoFoldingUp;

	private float foldUpTime;

	private float startTime;

	private Vector3 startPos;

	private Vector3 endPos;

	private int bloodNum;

	private SpriteRenderer spriteRenderer;

	private GameObject playerHeadObj;

	private UIPlayerHead playerHead;

	private SpriteRenderer spriteRenderer2;

	private SpriteRenderer spriteRenderer21;

	private SpriteRenderer spriteRenderer22;

	private GameObject playerHeadObj2;

	private UIPlayerHead playerHead2;

	private bool needFireOutView;

	private long _destroyStartTime = -1L;

	private int virusLayer;

	private int buildState;

	private ITimer delayCreateTimer;

	private AllianceOfficialSkillType aosType;

	private bool needShowGoddessMummyBorn;

	protected InstanceRequest detectEventInst;

	private InstanceRequest headReq;

	private UISelfExplosion uiSelfExplosion;

	private InstanceRequest selfExplosionInstance;

	private InstanceRequest sandWormReq;

	private SandWormMono sandWormMono;

	public Transform firePoint;

	private InstanceRequest werewolfHpReq;

	private UIWerewolfHpBar werewolfHpMono;

	private const string WEREWOLF_HP_PREFAB = "Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfHPBar.prefab";

	private const string ANIM_IDLE = "idle";

	private const string ANIM_IN = "in";

	private const string ANIM_ALIVE = "alive";

	private const string ANIM_OUT = "out";

	private const string ANIM_HIT = "hit";

	private int buildingLevel = 1;

	private float biShowFullRenderTime = -1f;

	protected bool fullRenderModeLoaded;

	private const string BaseIconSelf = "Assets/Main/Sprites/LodIcon/huojian3.png";

	private const string BaseIconDefault = "Assets/Main/Sprites/LodIcon/huojian1.png";

	private const string BaseIconAlly = "Assets/Main/Sprites/LodIcon/huojian4.png";

	private const string BaseIconLeader = "Assets/Main/Sprites/LodIcon/huojian5.png";

	private const string BaseIconOther = "Assets/Main/Sprites/LodIcon/huojian6.png";

	private const string BaseIconAllianceEnemy = "Assets/Main/Sprites/LodIcon/huojian_enemy_alliance.png";

	private const string BaseIconZoneEnemy = "Assets/Main/Sprites/LodIcon/huojian_enemy_zone.png";

	private const string BaseIconSeasonEnemy = "Assets/Main/Sprites/LodIcon/huojian_enemy_season.png";

	private const string BaseIconSeasonCamp = "Assets/Main/Sprites/LodIcon/huojian_camp_season.png";

	private const string BaseIconSeasonAssist = "Assets/Main/Sprites/LodIcon/huojian_assist_season.png";

	private const string BasePinSelf = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_lv.png";

	private const string BasePinDefault = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_bai.png";

	private const string BasePinAlly = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_lan.png";

	private const string BasePinLeader = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_zi.png";

	private const string BasePinAllianceEnemy = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_enemy_alliance.png";

	private const string BasePinZoneEnemy = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_enemy_zone.png";

	private const string BasePinSeasonEnemy = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_enemy_season.png";

	private const string BasePinSeasonCamp = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_camp_season.png";

	private const string BasePinSeasonAssist = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_assist_season.png";

	private const string WerewolfIconSelf = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_lv.png";

	private const string WerewolfIconAlly = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_lan.png";

	private const string WerewolfIconOther = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_hong.png";

	private static readonly string[] IconSelf = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_04.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_06.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_04.png" };

	private static readonly string[] IconDefault = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_00.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_07.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_00.png" };

	private static readonly string[] IconAlly = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_02.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_02.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_02.png" };

	private static readonly string[] IconAllianceEnemy = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_01.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_01.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_01.png" };

	private static readonly string[] IconSeasonEnemy = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_qianhong.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_05.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_qianhong.png" };

	private static readonly string[] IconSeasonCamp = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_qianlan.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_03.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_qianlan.png" };

	private static readonly string[] IconSeasonAssist = new string[3] { "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_shenlan.png", "Assets/Main/Sprites/LodIcon/zyf_S3_daditu_nvshenjineng_04.png", "Assets/Main/Sprites/LodIcon/mjc_S2_daditu_feidan_shenlan.png" };

	private string curIconPath;

	private float cityFireRefreshTimer;

	public bool CanShowLittleSmartMode
	{
		get
		{
			if (!WorldInstancingRenderers.DeviceSupportInstancing)
			{
				return false;
			}
			WorldScene worldScene = world;
			if ((object)worldScene == null || !worldScene.EnableWorldIconGPUInstancing)
			{
				return false;
			}
			BuildPointInfo buildPointInfo = BuildPointInfo;
			if (buildPointInfo == null)
			{
				return false;
			}
			if (buildPointInfo.worldId > 0)
			{
				return false;
			}
			if (buildPointInfo.AOSType != AllianceOfficialSkillType.None)
			{
				return false;
			}
			if (buildPointInfo.itemId != 10100000)
			{
				return false;
			}
			if (buildPointInfo.IsMine())
			{
				return false;
			}
			if (GMSwitch.IsGM)
			{
				if (debugGPUInstanceProp < 0)
				{
					debugGPUInstanceProp = Random.Range(1, 101);
				}
				if (debugGPUInstanceProp > GMSwitch.GetInt("DebugWorldPointGPUInstanceProp", 100))
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool IsLittleSmartModeWorking
	{
		get
		{
			if (happyHouse == null && happyPin == null)
			{
				return happyLevel != null;
			}
			return true;
		}
	}

	public long Uid => (long)pointType * 10000000L + pointIndex;

	protected BuildPointInfo BuildPointInfo => world?.GetPointInfoWithServer(pointIndex, serverId) as BuildPointInfo;

	protected override AutoAdjustLod AdjustLod
	{
		get
		{
			if (!(cityBuilding == null))
			{
				return cityBuilding.AdjustLod;
			}
			return null;
		}
	}

	public AsyncMono<WorldAssistanceLabelAsync>.Handle AssistanceLabel { get; private set; }

	public AsyncMono<WorldAssistanceHeroAsync>.Handle AssistanceHero { get; private set; }

	public AsyncMono<WorldCityFireEffectAsync>.Handle CityFire { get; private set; }

	public AsyncMono<WorldCityFireEffectAsync>.Handle MummyCityFire { get; private set; }

	public override AutoAdjustLod AutoAdjustLod
	{
		get
		{
			if (!(cityBuilding == null))
			{
				return cityBuilding.AutoAdjustLod;
			}
			return null;
		}
	}

	protected void DisposeLittleSmartMode()
	{
		happyHouse?.Destroy();
		happyPin?.Destroy();
		happyLevel?.Destroy();
		happyShell?.Destroy();
		happyAssistance?.Destroy();
		happyHouse = null;
		happyPin = null;
		happyLevel = null;
		happyShell = null;
		happyAssistance = null;
	}

	private int GetHouseIconIndex()
	{
		GetPlayerIconPath(out var _, out var _, out var iconIndex, out var _, out var _);
		return iconIndex;
	}

	private int GetPinIconIndex()
	{
		GetPlayerIconPath(out var _, out var _, out var _, out var pinIndex, out var _);
		return pinIndex;
	}

	private int GetBuildingLevel()
	{
		if (!(world.GetPointInfoWithServer(pointIndex, serverId) is BuildPointInfo buildPointInfo))
		{
			return 1;
		}
		if (buildPointInfo.IsWerewolf)
		{
			return 207;
		}
		int level = buildPointInfo.level;
		if (level >= 1 && level <= 200)
		{
			return level;
		}
		if (_happyLvSpecial.TryGetValue(level, out var value))
		{
			return value;
		}
		return 1;
	}

	public void UpdateLod(int lod)
	{
		UpdateLodShell(lod);
		RefreshHappyAssistanceCount();
		RefreshMonoAssistanceCount();
		RefreshCityFire();
		RefreshAssistanceHero();
		if (fullRenderModeLoaded)
		{
			return;
		}
		if (!CanShowLittleSmartMode)
		{
			ShowFullRenderMode();
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: false);
			SetLevelVisible(visible: false);
			return;
		}
		PlayerType playerType = BuildPointInfo?.GetPlayerType() ?? PlayerType.PlayerNone;
		if (playerType == PlayerType.PlayerNone)
		{
			Log.Info("Haha.WorldBuildObj.UpdateLod.failed!");
		}
		switch (playerType)
		{
		case PlayerType.PlayerAlliance:
		case PlayerType.PlayerAllianceLeader:
			UpdateLodAlliance(lod);
			return;
		case PlayerType.PlayerOther:
			UpdateLodOther(lod);
			return;
		}
		ShowFullRenderMode();
		SetHappyHouseVisible(visible: false);
		SetHappyPinVisible(visible: false);
		SetLevelVisible(visible: false);
	}

	private void UpdateLodShell(int lod)
	{
		if (!WorldInstancingRenderers.DeviceSupportInstancing)
		{
			return;
		}
		WorldScene worldScene = world;
		if ((object)worldScene == null || !worldScene.EnableWorldIconGPUInstancing)
		{
			return;
		}
		WorldScene worldScene2 = world;
		if ((object)worldScene2 == null || !worldScene2.EnableWorldAssistanceOpt)
		{
			return;
		}
		bool flag = true;
		BuildPointInfo buildPointInfo = BuildPointInfo;
		PlayerType playerType = BuildPointInfo?.GetPlayerType() ?? PlayerType.PlayerNone;
		switch (lod)
		{
		case 1:
		case 2:
			flag = false;
			break;
		case 3:
			flag = playerType == PlayerType.PlayerSelf || playerType == PlayerType.PlayerAlliance || playerType == PlayerType.PlayerAllianceLeader || playerType == PlayerType.PlayerOther;
			break;
		case 4:
			flag = playerType == PlayerType.PlayerSelf || playerType == PlayerType.PlayerAlliance || playerType == PlayerType.PlayerAllianceLeader;
			break;
		case 5:
		case 6:
		case 7:
			flag = playerType == PlayerType.PlayerSelf;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			if (buildPointInfo == null)
			{
				flag = false;
			}
			else if (buildPointInfo.protectEndTime <= 0 || buildPointInfo.protectEndTime <= GameEntry.Timer.GetServerTimeSeconds())
			{
				flag = false;
			}
		}
		SetHappyShellVisible(flag);
	}

	private void GetAssistanceCount(int lod, out int happyIcon, out int monoIcon)
	{
		happyIcon = 0;
		monoIcon = 0;
		BuildPointInfo buildPointInfo = BuildPointInfo;
		if (buildPointInfo == null || buildPointInfo.assistanceCount <= 0)
		{
			return;
		}
		monoIcon = (happyIcon = buildPointInfo.assistanceCount);
		PlayerType playerType = buildPointInfo.GetPlayerType();
		if (base.PointBattlefieldType != BattleFieldType.Default)
		{
			happyIcon = 0;
		}
		else
		{
			switch (lod)
			{
			case 1:
			case 2:
			{
				bool num2 = playerType == PlayerType.PlayerSelf || playerType == PlayerType.PlayerAlliance || playerType == PlayerType.PlayerAllianceLeader;
				happyIcon = 0;
				if (!num2)
				{
					monoIcon = 0;
				}
				break;
			}
			case 3:
			case 4:
			{
				bool num3 = playerType == PlayerType.PlayerSelf || playerType == PlayerType.PlayerAlliance || playerType == PlayerType.PlayerAllianceLeader;
				monoIcon = 0;
				if (!num3)
				{
					happyIcon = 0;
				}
				break;
			}
			case 5:
			case 6:
			case 7:
			{
				bool num = playerType == PlayerType.PlayerSelf;
				monoIcon = 0;
				if (!num)
				{
					happyIcon = 0;
				}
				break;
			}
			default:
				monoIcon = 0;
				happyIcon = 0;
				break;
			}
		}
		if (!WorldInstancingRenderers.DeviceSupportInstancing)
		{
			happyIcon = 0;
		}
		WorldScene worldScene = world;
		if ((object)worldScene == null || !worldScene.EnableWorldIconGPUInstancing)
		{
			happyIcon = 0;
		}
	}

	private void UpdateLodOther(int lod)
	{
		switch (lod)
		{
		case 1:
		case 2:
			ShowFullRenderMode();
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: false);
			SetLevelVisible(visible: false);
			break;
		case 3:
			SetHappyHouseVisible(visible: true);
			SetHappyPinVisible(visible: false);
			SetLevelVisible(visible: true);
			break;
		case 4:
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: true);
			SetLevelVisible(visible: true);
			break;
		case 5:
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: true);
			SetLevelVisible(visible: false);
			break;
		default:
			SetHappyPinVisible(visible: false);
			SetHappyHouseVisible(visible: false);
			SetLevelVisible(visible: false);
			break;
		}
	}

	private void UpdateLodAlliance(int lod)
	{
		switch (lod)
		{
		case 1:
		case 2:
			ShowFullRenderMode();
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: false);
			SetLevelVisible(visible: false);
			break;
		case 3:
		case 4:
			SetHappyHouseVisible(visible: true);
			SetHappyPinVisible(visible: false);
			SetLevelVisible(visible: true);
			break;
		case 5:
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: true);
			SetLevelVisible(visible: false);
			break;
		case 6:
			SetHappyHouseVisible(visible: false);
			SetHappyPinVisible(visible: true);
			SetLevelVisible(visible: false);
			break;
		default:
			SetHappyPinVisible(visible: false);
			SetHappyHouseVisible(visible: false);
			SetLevelVisible(visible: false);
			break;
		}
	}

	private void SetHappyHouseVisible(bool visible)
	{
		if (iconRendererFacade != null)
		{
			if (!visible && happyHouse != null)
			{
				happyHouse.Destroy();
				happyHouse = null;
			}
			else if (visible)
			{
				happyHouse = happyHouse ?? iconRendererFacade.CreateIcon("HappyHouse");
				happyHouse?.Refresh(base.WorldPosition, GetHouseIconIndex());
			}
		}
	}

	private void SetHappyPinVisible(bool visible)
	{
		if (iconRendererFacade != null)
		{
			if (!visible && happyPin != null)
			{
				happyPin.Destroy();
				happyPin = null;
			}
			else if (visible)
			{
				happyPin = happyPin ?? iconRendererFacade.CreateIcon("HappyPin");
				happyPin?.Refresh(base.WorldPosition, GetPinIconIndex());
			}
		}
	}

	private void SetLevelVisible(bool visible)
	{
		if (iconRendererFacade != null)
		{
			if (!visible && happyLevel != null)
			{
				happyLevel.Destroy();
				happyLevel = null;
			}
			else if (visible)
			{
				happyLevel = happyLevel ?? iconRendererFacade.CreateIcon("HappyLevel");
				happyLevel?.Refresh(base.WorldPosition, GetBuildingLevel());
			}
		}
	}

	private void SetHappyShellVisible(bool visible)
	{
		if (iconRendererFacade != null)
		{
			if (!visible && happyShell != null)
			{
				happyShell.Destroy();
				happyShell = null;
			}
			else if (visible)
			{
				happyShell = happyShell ?? iconRendererFacade.CreateIcon("HappyShell");
				happyShell?.Refresh(base.WorldPosition, 0);
			}
		}
	}

	private void RefreshHappyAssistanceCount()
	{
		GetAssistanceCount(world.CurrentLodLevel, out var happyIcon, out var _);
		bool flag = happyIcon > 0;
		if (iconRendererFacade != null)
		{
			if (!flag && happyAssistance != null)
			{
				happyAssistance.Destroy();
				happyAssistance = null;
			}
			else if (flag)
			{
				WorldScene worldScene = world;
				bool flag2 = (object)worldScene != null && worldScene.GetMyAssistanceCount(pointIndex) > 0;
				happyAssistance = happyAssistance ?? iconRendererFacade.CreateIcon("HappyAssistance");
				happyAssistance?.Refresh(base.WorldPosition, happyIcon + (flag2 ? 5 : 0));
			}
		}
	}

	private void GetPlayerIconPath(out string iconPath, out string pinPath, out int iconIndex, out int pinIndex, out MainBuildOrder sortingOrder)
	{
		iconPath = "Assets/Main/Sprites/LodIcon/huojian1.png";
		pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_bai.png";
		sortingOrder = MainBuildOrder.Other;
		iconIndex = 0;
		pinIndex = 0;
		BuildPointInfo buildPointInfo = BuildPointInfo;
		if (buildPointInfo == null)
		{
			Log.Info($"Haha.WorldBuildObj.GetPlayerIconPath.Failed.{pointIndex}");
			return;
		}
		PlayerType playerType = buildPointInfo.GetPlayerType();
		if (buildPointInfo.IsWerewolf)
		{
			switch (playerType)
			{
			case PlayerType.PlayerSelf:
				iconPath = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_lv.png";
				pinPath = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_lv.png";
				sortingOrder = MainBuildOrder.Self;
				iconIndex = 1;
				pinIndex = 1;
				break;
			case PlayerType.PlayerAlliance:
			case PlayerType.PlayerAllianceLeader:
				iconPath = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_lan.png";
				pinPath = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_lan.png";
				sortingOrder = MainBuildOrder.Ally;
				iconIndex = 10;
				pinIndex = 10;
				break;
			default:
				iconPath = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_hong.png";
				pinPath = "Assets/Main/SeasonRes/S4/Sprites/World/ljq_s4_xueselieren_zuobiao_hong.png";
				sortingOrder = MainBuildOrder.Other;
				iconIndex = 9;
				pinIndex = 9;
				break;
			}
			return;
		}
		switch (playerType)
		{
		case PlayerType.PlayerSelf:
			if (buildPointInfo.IsNormalType())
			{
				iconPath = "Assets/Main/Sprites/LodIcon/huojian3.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_lv.png";
				sortingOrder = MainBuildOrder.Self;
				iconIndex = 1;
				pinIndex = 1;
			}
			else
			{
				iconPath = "Assets/Main/Sprites/LodIcon/huojian1.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_bai.png";
				sortingOrder = MainBuildOrder.Other;
				iconIndex = 0;
				pinIndex = 0;
			}
			break;
		case PlayerType.PlayerAlliance:
			iconPath = "Assets/Main/Sprites/LodIcon/huojian4.png";
			pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_lan.png";
			sortingOrder = MainBuildOrder.Ally;
			iconIndex = 2;
			pinIndex = 2;
			break;
		case PlayerType.PlayerAllianceLeader:
			iconPath = "Assets/Main/Sprites/LodIcon/huojian5.png";
			pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_zi.png";
			sortingOrder = MainBuildOrder.Leader;
			iconIndex = 3;
			pinIndex = 3;
			break;
		case PlayerType.PlayerOther:
			switch (world.IsMyEnemy(buildPointInfo.srcServerId, buildPointInfo.allianceId))
			{
			case PlayerType.PlayerAllianceEnemy:
				iconPath = "Assets/Main/Sprites/LodIcon/huojian_enemy_alliance.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_enemy_alliance.png";
				sortingOrder = MainBuildOrder.Enemy;
				iconIndex = 4;
				pinIndex = 4;
				break;
			case PlayerType.PlayerZoneEnemy:
				iconPath = "Assets/Main/Sprites/LodIcon/huojian_enemy_zone.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_enemy_zone.png";
				sortingOrder = MainBuildOrder.Enemy;
				iconIndex = 5;
				pinIndex = 5;
				break;
			case PlayerType.PlayerSeasonEnemy:
				iconPath = "Assets/Main/Sprites/LodIcon/huojian_enemy_season.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_enemy_season.png";
				sortingOrder = MainBuildOrder.Enemy;
				iconIndex = 6;
				pinIndex = 6;
				break;
			case PlayerType.PlayerSeasonCamp:
				iconPath = "Assets/Main/Sprites/LodIcon/huojian_camp_season.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_camp_season.png";
				sortingOrder = MainBuildOrder.Enemy;
				iconIndex = 7;
				pinIndex = 7;
				break;
			case PlayerType.PlayerSeasonAssist:
				iconPath = "Assets/Main/Sprites/LodIcon/huojian_assist_season.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_assist_season.png";
				sortingOrder = MainBuildOrder.Enemy;
				iconIndex = 8;
				pinIndex = 8;
				break;
			default:
				iconPath = "Assets/Main/Sprites/LodIcon/huojian1.png";
				pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_bai.png";
				sortingOrder = MainBuildOrder.Other;
				break;
			}
			break;
		default:
			iconPath = "Assets/Main/Sprites/LodIcon/huojian1.png";
			pinPath = "Assets/Main/Sprites/LodIcon/zyf_daditu_dingwei_bai.png";
			sortingOrder = MainBuildOrder.Other;
			iconIndex = 0;
			pinIndex = 0;
			break;
		}
	}

	public WorldBuildObjectNew(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	protected override void ClearOldObject()
	{
		curIconPath = null;
		if (headReq != null)
		{
			headReq.Destroy();
			headReq = null;
		}
		if (selfExplosionInstance != null)
		{
			selfExplosionInstance.Destroy();
			selfExplosionInstance = null;
			uiSelfExplosion = null;
		}
		if (sandWormReq != null)
		{
			sandWormReq.Destroy();
			sandWormReq = null;
			sandWormMono?.Dispose();
			sandWormMono = null;
		}
		if (werewolfHpReq != null)
		{
			werewolfHpReq.Destroy();
			werewolfHpReq = null;
			werewolfHpMono?.Dispose();
			werewolfHpMono = null;
		}
		firePoint = null;
		base.ClearOldObject();
	}

	private void ShowFullRenderMode()
	{
		if (fullRenderModeLoaded)
		{
			return;
		}
		if (delayCreateTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayCreateTimer);
			delayCreateTimer = null;
		}
		base.CreateGameObject();
		BuildPointInfo bi = BuildPointInfo;
		if (bi == null)
		{
			return;
		}
		buildState = bi.buildState;
		if (buildState == 5)
		{
			string prefabPath = GameEntry.Lua.CallWithReturn<string, string, int>("CSharpCallLuaInterface.GetFlagPath", bi.allianceId, bi.srcServerId);
			AddOldObject();
			instance = GameEntry.Resource.InstantiateAsync(prefabPath);
			instance.completed += delegate
			{
				ClearOldObject();
				gameObject = instance.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(world.DynamicObjNode);
					gameObject.transform.position = base.WorldPosition;
				}
			};
			return;
		}
		fullRenderModeLoaded = true;
		biShowFullRenderTime = Time.realtimeSinceStartup;
		int level = bi.level;
		buildingLevel = level;
		virusLayer = bi.virusLayer;
		aosType = bi.AOSType;
		string text;
		switch (aosType)
		{
		case AllianceOfficialSkillType.AresMissile:
		{
			text = "Assets/Main/Prefabs/Building/A_build_S2_ares_missile_world.prefab";
			SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
			if (curSkinMeta != null && curSkinMeta.IsDarknessMode())
			{
				text = "Assets/Main/SeasonRes/S4/Prefabs/AllianceBuilding/Ares_missile_world_aisila.prefab";
			}
			break;
		}
		case AllianceOfficialSkillType.GoddessMummy:
			text = "Assets/Main/SeasonRes/S3/Prefabs/World/A_build_S3_goddess_mummy_world.prefab";
			break;
		case AllianceOfficialSkillType.MissileFactory:
			text = "Assets/Main/Prefabs/Building/A_build_S2_ares_missile_world.prefab";
			break;
		default:
			text = bi.GetModelPath();
			break;
		}
		if (text.IsNullOrEmpty())
		{
			return;
		}
		AddOldObject();
		instance = world.InstantiateAsyncDynamicObj(text);
		instance.completed += delegate
		{
			ClearOldObject();
			gameObject = instance.gameObject;
			if (gameObject != null)
			{
				gameObject.transform.SetParent(world.DynamicObjNode);
				Transform transform = gameObject.transform.Find("Icon");
				if (transform != null)
				{
					transform.gameObject.SetActive(bi.worldId <= 0);
				}
				cityBuilding = gameObject.GetComponent<WorldBuilding>();
				bUuid = bi.uuid;
				if (cityBuilding != null)
				{
					WorldBuilding.Param userData = new WorldBuilding.Param
					{
						point = pointIndex,
						buildUuid = bUuid,
						buildSceneType = WorldBuilding.BuildSceneType.World,
						serverId = serverId
					};
					cityBuilding.CSInit(userData);
					cityBuilding.UpdateCityLabel(bi.uuid);
					PlayCivilianToWolfAnim(bi);
				}
				Transform transform2 = gameObject.transform.Find("ModelGo/CityLabel/Virus");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value: false);
				}
				Transform transform3 = gameObject.transform.Find("ModelGo/CityLabel/Farmer");
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(value: false);
				}
				PlayAOSAnim(bi);
				gameObject.SetActive(isVisible);
				if (isVisible)
				{
					if (bi.IsMine() || aosType != AllianceOfficialSkillType.None)
					{
						GameEntry.Event.Fire(EventId.WORLD_BUILD_IN_VIEW, bUuid);
					}
					needFireOutView = true;
				}
				bloodNum = bi.curHp;
				_destroyStartTime = bi.destroyStartTime;
				int num = bi.itemId + bi.level;
				int num2 = GameEntry.ConfigCache.GetTemplateData("building", num, "max_hp").ToInt();
				if (GameEntry.Data.Player.IsInBattleField())
				{
					num2 = ((bi.curMaxHp <= 0) ? GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetPlayerMaxHp", GameEntry.Data.Player.GetWorldType()) : bi.curMaxHp);
				}
				if (num2 > 0 && aosType == AllianceOfficialSkillType.None && num2 > bi.curHp)
				{
					GameEntry.Event.Fire(EventId.ShowIsOnFire, bUuid);
					needFireOutView = true;
				}
				if (bi.itemId == 10100000)
				{
					needFireOutView = true;
					spriteRenderer = gameObject.transform.Find("Icon/Sprite").GetComponentInChildren<SpriteRenderer>(includeInactive: true);
					Transform transform4 = gameObject.transform.Find("Icon/Sprite2");
					if (transform4 != null)
					{
						spriteRenderer2 = transform4.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
					}
					Transform transform5 = gameObject.transform.Find("Icon2");
					if (transform5 != null)
					{
						Transform transform6 = transform5.Find("Sprite");
						Transform transform7 = transform5.Find("Sprite2");
						if (transform6 != null)
						{
							spriteRenderer21 = transform6.GetComponent<SpriteRenderer>();
						}
						if (transform7 != null)
						{
							spriteRenderer22 = transform7.GetComponent<SpriteRenderer>();
						}
					}
					if (aosType == AllianceOfficialSkillType.None)
					{
						GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldBaseRefresh", bi);
						SetPlayerHead(bi);
					}
					SetIconSprite(bi);
					if (bi.IsMine())
					{
						long num3 = GameEntry.Lua.CallWithReturn<long>("CSharpCallLuaInterface.GetOnMovingBuildUuid");
						if (num3 != 0L && num3 == bi.uuid)
						{
							gameObject.SetActive(value: false);
						}
					}
				}
				else if (bi.itemId == 735000)
				{
					string previewIconPath = bi.GetPlayerType() switch
					{
						PlayerType.PlayerSelf => "Assets/Main/Sprites/LodIcon/huojian3.png", 
						PlayerType.PlayerAlliance => "Assets/Main/Sprites/LodIcon/huojian4.png", 
						PlayerType.PlayerAllianceLeader => "Assets/Main/Sprites/LodIcon/huojian5.png", 
						PlayerType.PlayerOther => "Assets/Main/Sprites/LodIcon/huojian6.png", 
						_ => "Assets/Main/Sprites/LodIcon/huojian1.png", 
					};
					if (cityBuilding != null)
					{
						cityBuilding.previewIconPath = previewIconPath;
						cityBuilding.previewName = (bi.IsWerewolf ? bi.playerName : UIUtils.FormatServerAllianceName(bi.srcServerId, bi.alAbbr, bi.playerName));
						cityBuilding.previewType = WorldPreviewType.PlayerCity;
					}
					if (bi.quarantineRole != 0)
					{
						GameEntry.Event.Fire(EventId.WORLD_BUILD_IN_VIEW, bUuid);
					}
				}
				else if (bi.itemId == 10301000 || bi.itemId == 10301001)
				{
					GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldRuinRefresh", bi);
				}
				if (bi.IsMine() && (bi.itemId == 412000 || bi.itemId == 413000 || bi.itemId == 432000))
				{
					world.CreateAnimalObject(bi.uuid);
				}
				CheckShowTroopDestination();
				SetClickEvent();
				ShowDetectEvent();
				if (aosType == AllianceOfficialSkillType.None)
				{
					CheckShowTemperature();
				}
				CheckShowExplosion(bi);
				CheckShowWerewolfHp(bi);
				CheckShowSandWorm(bi, SandWormAnim.Idle);
				cityBuilding?.RefreshMeteorite();
				cityBuilding?.RefreshEpidemicSkill();
				PlayWolfToCivilianAnim(bi);
			}
		};
	}

	private void PlayAOSAnim(BuildPointInfo bi)
	{
		if (aosType == AllianceOfficialSkillType.AresMissile || aosType == AllianceOfficialSkillType.MissileFactory)
		{
			Transform transform = gameObject.transform.Find("ModelGo/Normal/AresMissile/skin");
			if (transform != null)
			{
				Animator component = transform.GetComponent<Animator>();
				if (component != null)
				{
					component.enabled = true;
				}
				SimpleAnimation component2 = transform.GetComponent<SimpleAnimation>();
				if (component2 != null)
				{
					component2.enabled = true;
					if (component2.IsPlaying("up"))
					{
						component2.Rewind();
					}
					else
					{
						component2.Play("up");
					}
				}
				Transform transform2 = gameObject.transform.Find("ModelGo/Normal/AresMissile/skin/To_unity/Root/Eff_daditu_zhanshenfeidan_01");
				Transform transform3 = gameObject.transform.Find("ModelGo/Normal/AresMissile/skin/To_unity/Root/huojian/Eff_daditu_zhanshenfeidan_02");
				Transform transform4 = gameObject.transform.Find("ModelGo/Normal/AresMissile/skin/To_unity/Root/Eff_daditu_zhanshenfeidan_03");
				Transform transform5 = gameObject.transform.Find("ModelGo/Normal/AresMissile/skin/To_unity/Root/Eff_daditu_zhanshenfeidan_04");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(value: false);
				}
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(value: false);
				}
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(value: false);
				}
				if (transform5 != null)
				{
					transform5.gameObject.SetActive(value: false);
				}
				return;
			}
			string n = "ModelGo/Normal/AresMissile/A_Build@aisila01_skin";
			transform = gameObject.transform.Find(n);
			if (!(transform != null))
			{
				return;
			}
			Animator component3 = transform.GetComponent<Animator>();
			if (component3 != null)
			{
				component3.enabled = true;
			}
			SimpleAnimation component4 = transform.GetComponent<SimpleAnimation>();
			if (component4 != null)
			{
				component4.enabled = true;
				if (component4.IsPlaying("born"))
				{
					component4.Rewind("born");
				}
				else
				{
					component4.Play("born");
				}
				component4.PlayQueued("idle");
				component4.PlayQueued("attack01");
			}
			string n2 = "ModelGo/Normal/AresMissile/A_Build@aisila01_skin/To_unity/DeformationSystem/Root/Root_M/Spine1_M/Chest_M/Neck0_M/Head_M/Eff_S4_Mars_Loop_Head_M";
			string n3 = "ModelGo/Normal/AresMissile/A_Build@aisila01_skin/To_unity/DeformationSystem/Root/Eff_S4_Mars_Fly_Root";
			Transform transform6 = gameObject.transform.Find(n2);
			Transform transform7 = gameObject.transform.Find(n3);
			if (transform6 != null)
			{
				transform6.gameObject.SetActive(value: false);
			}
			if (transform7 != null)
			{
				transform7.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if (aosType != AllianceOfficialSkillType.GoddessMummy)
			{
				return;
			}
			firePoint = gameObject.transform.Find("ModelGo/Normal/GoddessMummy/skin/To_unity/DeformationSystem/Root/ditai/wuqi/wuqi1");
			Transform transform8 = gameObject.transform.Find("ModelGo/Normal/GoddessMummy/skin");
			if (transform8 == null)
			{
				return;
			}
			Animator component5 = transform8.GetComponent<Animator>();
			if (component5 != null)
			{
				component5.enabled = true;
			}
			SimpleAnimation component6 = transform8.GetComponent<SimpleAnimation>();
			if (component6 == null)
			{
				return;
			}
			component6.enabled = true;
			Transform atkLoopVFX = transform8.Find("To_unity/DeformationSystem/Root/Eff_ljw_S3_A_bulid_zhaohuanshenxiang_attack_ioop");
			Transform weaponLoopVFX = transform8.Find("To_unity/DeformationSystem/Root/ditai/wuqi/wuqi1/Eff_ljw_S3_A_bulid_zhaohuanshenxiang_attack_wuqi_loop");
			if (!needShowGoddessMummyBorn && bi.IsMine())
			{
				needShowGoddessMummyBorn = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetNeedPlayBornAnim");
			}
			if (needShowGoddessMummyBorn)
			{
				needShowGoddessMummyBorn = false;
				component6.Rewind("born");
				component6.Play("born");
				component6.PlayQueued("chant");
				Transform transform9 = transform8.Find("To_unity/DeformationSystem/Root");
				float scale = 0.5f;
				if (transform9 != null && firePoint != null)
				{
					world.CreateVFX("Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_A_bulid_zhaohuanshenxiang_bron.prefab", transform9.position, 3f, 0f, delegate(GameObject go)
					{
						go.transform.localScale = new Vector3(scale, scale, scale);
					});
					world.CreateVFX("Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_A_bulid_zhaohuanshenxiang_bron2.prefab", transform9.position, 3f, 6.8f, delegate(GameObject go)
					{
						go.transform.localScale = new Vector3(scale, scale, scale);
					});
					world.CreateVFX("Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_A_bulid_zhaohuanshenxiang_born_glow.prefab", firePoint.position, 4f, 5.4f, delegate(GameObject go)
					{
						if (firePoint != null)
						{
							go.transform.SetParent(firePoint);
							go.transform.localPosition = Vector3.zero;
							go.transform.localScale = Vector3.one;
						}
					});
				}
				if (atkLoopVFX != null)
				{
					atkLoopVFX.gameObject.SetActive(value: false);
				}
				if (weaponLoopVFX != null)
				{
					weaponLoopVFX.gameObject.SetActive(value: false);
				}
				GameEntry.Timer.RegisterTimer(8.2f, delegate
				{
					if (atkLoopVFX != null)
					{
						atkLoopVFX.gameObject.SetActive(value: true);
					}
					if (weaponLoopVFX != null)
					{
						weaponLoopVFX.gameObject.SetActive(value: true);
					}
				});
			}
			else
			{
				component6.Play("chant");
				if (atkLoopVFX != null)
				{
					atkLoopVFX.gameObject.SetActive(value: true);
				}
				if (weaponLoopVFX != null)
				{
					weaponLoopVFX.gameObject.SetActive(value: true);
				}
			}
		}
	}

	public override void CreateGameObject()
	{
		world?.RegisterLodWatcher(this);
		UpdateLod(world.CurrentLodLevel);
	}

	public override void UpdateGameObject()
	{
		if (buildState == 5)
		{
			if (delayCreateTimer == null && world.GetPointInfo(pointIndex) is BuildPointInfo { buildState: not 5 } buildPointInfo)
			{
				string userData = buildPointInfo.uuid + ";" + buildPointInfo.mainIndex + ";0";
				GameEntry.Event.Fire(EventId.ShowDomeShowEffect, userData);
				delayCreateTimer = GameEntry.Timer.RegisterTimer(4f, CreateGameObject);
			}
			return;
		}
		UpdateLod(world.CurrentLodLevel);
		base.UpdateGameObject();
		if (gameObject == null)
		{
			return;
		}
		if (aosType == AllianceOfficialSkillType.None)
		{
			CheckShowTemperature();
		}
		if (!(cityBuilding != null))
		{
			return;
		}
		cityBuilding.refeshDate();
		cityBuilding.UpdateCityLabel(bUuid);
		cityBuilding.UpdateStatus();
		cityBuilding.RefreshMeteorite();
		cityBuilding.RefreshEpidemicSkill();
		if (!(world.GetPointInfo(pointIndex) is BuildPointInfo buildPointInfo2))
		{
			return;
		}
		CheckShowExplosion(buildPointInfo2);
		CheckShowWerewolfHp(buildPointInfo2);
		if (aosType != buildPointInfo2.AOSType)
		{
			if (aosType == AllianceOfficialSkillType.AresMissile && buildPointInfo2.AOSType == AllianceOfficialSkillType.None)
			{
				string n = "ModelGo/Normal/AresMissile/A_Build@aisila01_skin";
				Transform transform = gameObject.transform.Find(n);
				if (transform != null)
				{
					SimpleAnimation component = transform.GetComponent<SimpleAnimation>();
					if (component != null && component.isPlaying)
					{
						return;
					}
				}
			}
			Destroy();
			CreateGameObject();
			if (buildPointInfo2.AOSType != AllianceOfficialSkillType.GoddessMummy)
			{
				string prefabPath = "Assets/_Art/Effect/prefab/scene/Build/Dabenqianyi/VFX_world_zhucheng_qianyi_hui.prefab";
				world.CreateBattleVFX(prefabPath, 5f, delegate(GameObject go)
				{
					if (world != null && world.DynamicObjNode != null)
					{
						go.transform.SetParent(world.DynamicObjNode);
						go.transform.localScale = Vector3.one;
						go.transform.localPosition = base.WorldPosition;
					}
				});
			}
			else
			{
				needShowGoddessMummyBorn = true;
			}
			return;
		}
		if (virusLayer < buildPointInfo2.virusLayer && gameObject != null)
		{
			virusLayer = buildPointInfo2.virusLayer;
			if (!string.Equals(GameEntry.Data.Player.GetData("SimpleModeOn"), "YES"))
			{
				world.CreateBattleVFX("Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_kuosan_du_01.prefab", 2f, delegate(GameObject go)
				{
					if (gameObject != null && gameObject.transform != null)
					{
						go.transform.SetParent(gameObject.transform);
						go.transform.localPosition = new Vector3(0f, 0f, 0f);
					}
				});
			}
		}
		if (bloodNum != buildPointInfo2.curHp)
		{
			bloodNum = buildPointInfo2.curHp;
			GameEntry.Event.Fire(EventId.ShowIsOnFire, bUuid);
			needFireOutView = true;
		}
		if (buildPointInfo2.itemId == 10100000)
		{
			needFireOutView = true;
			GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldBaseRefresh", buildPointInfo2);
			SetIconSprite(buildPointInfo2);
		}
		else if (buildPointInfo2.itemId == 10301000 || buildPointInfo2.itemId == 10301001)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldRuinRefresh", buildPointInfo2);
		}
		else if (buildPointInfo2.itemId == 735000)
		{
			GameEntry.Event.Fire(EventId.WORLD_BUILD_IN_VIEW, bUuid);
			needFireOutView = true;
		}
		if (buildPointInfo2.destroyStartTime > 0 && _destroyStartTime <= 0)
		{
			needFireOutView = true;
			GameEntry.Event.Fire(EventId.ShowBuildDestroyEffect, bUuid);
		}
		else if (buildPointInfo2.destroyStartTime <= 0 && _destroyStartTime > 0)
		{
			GameEntry.Event.Fire(EventId.ShowBuildFixEffect, bUuid);
			needFireOutView = true;
		}
		GameEntry.Event.Fire(EventId.WorldCityBuildObjUpdate, bUuid);
		_destroyStartTime = buildPointInfo2.destroyStartTime;
		if (isVisible && buildPointInfo2.IsMine())
		{
			GameEntry.Event.Fire(EventId.WORLD_BUILD_IN_VIEW, bUuid);
			needFireOutView = true;
		}
	}

	private void CheckShowWerewolfHp(BuildPointInfo bi)
	{
		if (bi == null || gameObject == null || bi.worldId > 0)
		{
			return;
		}
		if (!bi.IsWerewolf)
		{
			if (werewolfHpReq != null)
			{
				werewolfHpMono?.Dispose();
				werewolfHpMono = null;
				werewolfHpReq.Destroy();
				werewolfHpReq = null;
			}
		}
		else if (werewolfHpMono != null)
		{
			werewolfHpMono.SetData(bi.wolfDecrHp, ani: true);
		}
		else
		{
			if (werewolfHpReq != null)
			{
				return;
			}
			werewolfHpReq = GameEntry.Resource.InstantiateAsync("Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfHPBar.prefab");
			werewolfHpReq.completed += delegate
			{
				GameObject gameObject = werewolfHpReq.gameObject;
				if (!(base.gameObject == null) && !(gameObject == null))
				{
					Transform transform = base.gameObject.transform.Find("ModelGo");
					if (transform == null)
					{
						Log.Error(base.gameObject.name + "上找不到ModelGo！");
					}
					else
					{
						gameObject.transform.SetParent(transform, worldPositionStays: false);
						gameObject.transform.localPosition = new Vector3(0f, 0f, -0.35f);
						gameObject.transform.localRotation = Quaternion.identity;
						gameObject.transform.localScale = Vector3.one;
						werewolfHpMono = gameObject.GetComponent<UIWerewolfHpBar>();
						werewolfHpMono.Init(world.GetWerewolfMaxHp());
						werewolfHpMono.SetData(bi.wolfDecrHp);
					}
				}
			};
		}
	}

	public void CheckShowSandWorm(BuildPointInfo bi, SandWormAnim anim)
	{
		if (bi == null || gameObject == null || bi.worldId > 0)
		{
			return;
		}
		if (anim == SandWormAnim.MoveCity)
		{
			gameObject.GetComponent<BuildEffect>()?.PlayAnim();
			return;
		}
		if (!bi.IsWormWrap())
		{
			if (sandWormReq == null)
			{
				return;
			}
			gameObject.GetComponent<BuildEffect>()?.PlayAnim();
			if (sandWormMono == null)
			{
				sandWormReq.Destroy();
				sandWormReq = null;
				return;
			}
			InstanceRequest req = sandWormReq;
			sandWormReq = null;
			sandWormMono.PlayAnim(SandWormAnim.Die);
			sandWormMono?.Dispose();
			sandWormMono = null;
			GameEntry.Timer.RegisterTimer(2f, delegate
			{
				req?.Destroy();
			});
			return;
		}
		gameObject.GetComponent<BuildEffect>()?.StopAnim();
		if (sandWormMono != null)
		{
			sandWormMono.Refresh(bi.sandWorm);
		}
		else
		{
			if (sandWormReq != null)
			{
				return;
			}
			string text = bi.sandWorm.GetPrefabPath();
			if (text.IsNullOrEmpty())
			{
				text = "Assets/Main/SeasonRes/Shared/Prefabs/Monster/SmallSandWorm.prefab";
			}
			sandWormReq = GameEntry.Resource.InstantiateAsync(text);
			sandWormReq.completed += delegate
			{
				GameObject gameObject = sandWormReq.gameObject;
				if (!(base.gameObject == null) && !(gameObject == null))
				{
					Transform transform = base.gameObject.transform.Find("ModelGo");
					if (transform == null)
					{
						Log.Error(base.gameObject.name + "上找不到ModelGo！");
					}
					else
					{
						gameObject.transform.SetParent(transform, worldPositionStays: false);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
						gameObject.transform.localScale = Vector3.one;
						sandWormMono = gameObject.GetComponent<SandWormMono>();
						sandWormMono.Init(bi.sandWorm, bi.pointIndex, bi.uuid);
						sandWormMono.PlayAnim(bi.sandWorm.IsBirthing() ? SandWormAnim.Appear : SandWormAnim.Idle);
					}
				}
			};
		}
	}

	public void SandWormAttack(Vector3 attackDir)
	{
		if (sandWormMono != null)
		{
			sandWormMono.AttackOnce(attackDir);
		}
	}

	private void CheckShowExplosion(BuildPointInfo bi)
	{
		if (bi == null || gameObject == null || bi.worldId > 0)
		{
			return;
		}
		long expireTime = bi.GetStatusExpireTime(27);
		if (expireTime == 0L)
		{
			if (selfExplosionInstance != null)
			{
				selfExplosionInstance.Destroy();
				selfExplosionInstance = null;
				uiSelfExplosion = null;
			}
		}
		else if (uiSelfExplosion != null)
		{
			uiSelfExplosion.SetExpireTime(expireTime);
		}
		else
		{
			if (selfExplosionInstance != null)
			{
				return;
			}
			selfExplosionInstance = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/UISelfExplosion.prefab");
			selfExplosionInstance.completed += delegate
			{
				GameObject gameObject = selfExplosionInstance.gameObject;
				if (!(base.gameObject == null) && !(gameObject == null))
				{
					Transform transform = base.gameObject.transform.Find("ModelGo/CityLabel");
					if (transform == null)
					{
						Log.Error(base.gameObject.name + "上找不到ModelGo/CityLabel！");
					}
					else
					{
						gameObject.transform.SetParent(transform, worldPositionStays: false);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localRotation = Quaternion.identity;
						gameObject.transform.localScale = Vector3.one;
						uiSelfExplosion = gameObject.GetComponent<UISelfExplosion>();
						uiSelfExplosion.SetExpireTime(expireTime);
					}
				}
			};
		}
	}

	private void RefreshMonoAssistanceCount()
	{
		GetAssistanceCount(world.CurrentLodLevel, out var _, out var monoIcon);
		if (monoIcon > 0)
		{
			if (AssistanceLabel == null)
			{
				AssistanceLabel = AsyncMono<WorldAssistanceLabelAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceLabelPlayer.prefab", world?.DynamicObjNode, delegate
				{
					Transform transform = AssistanceLabel?.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
						RefreshMonoAssistanceCount();
					}
				});
			}
			else if (AssistanceLabel.MonoInstance != null)
			{
				AssistanceLabel.SetActive(active: true);
				WorldAssistanceLabelAsync monoInstance = AssistanceLabel.MonoInstance;
				int count = monoIcon;
				int maxAssistanceCount = BuildPointInfo.maxAssistanceCount;
				WorldScene worldScene = world;
				monoInstance.SetAssistance(count, maxAssistanceCount, showMax: false, (object)worldScene != null && worldScene.GetMyAssistanceCount(pointIndex) > 0);
			}
		}
		else
		{
			AssistanceLabel?.SetActive(active: false);
		}
	}

	private void RefreshAssistanceHero()
	{
		int num = world?.GetMyAssistanceFirstHero(pointIndex) ?? 0;
		int currentLodLevel = world.CurrentLodLevel;
		if (num > 0 && currentLodLevel >= 3 && currentLodLevel <= 4)
		{
			if (AssistanceHero == null)
			{
				AssistanceHero = AsyncMono<WorldAssistanceHeroAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/WorldAssistanceHeroPlayer.prefab", world?.DynamicObjNode, delegate
				{
					Transform transform = AssistanceHero?.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
						RefreshAssistanceHero();
					}
				});
			}
			else if (AssistanceHero.MonoInstance != null)
			{
				AssistanceHero.SetActive(active: true);
				AssistanceHero.MonoInstance.SetHeroHead(num);
			}
		}
		else
		{
			AssistanceHero?.SetActive(active: false);
		}
	}

	private void SetIconSprite(BuildPointInfo info)
	{
		AllianceOfficialSkillType aOSType = info.AOSType;
		string iconPath;
		string pinPath;
		MainBuildOrder sortingOrder;
		if (aOSType != AllianceOfficialSkillType.None)
		{
			int num = (int)(aOSType - 1);
			switch (info.GetPlayerType())
			{
			case PlayerType.PlayerSelf:
				iconPath = IconSelf[num];
				pinPath = IconSelf[num];
				sortingOrder = MainBuildOrder.Self;
				break;
			case PlayerType.PlayerAlliance:
				iconPath = IconAlly[num];
				pinPath = IconAlly[num];
				sortingOrder = MainBuildOrder.Ally;
				break;
			case PlayerType.PlayerAllianceLeader:
				iconPath = IconAlly[num];
				pinPath = IconAlly[num];
				sortingOrder = MainBuildOrder.Leader;
				break;
			case PlayerType.PlayerOther:
				switch (world.IsMyEnemy(info.srcServerId, info.allianceId))
				{
				case PlayerType.PlayerAllianceEnemy:
					iconPath = IconAllianceEnemy[num];
					pinPath = IconAllianceEnemy[num];
					sortingOrder = MainBuildOrder.Enemy;
					break;
				case PlayerType.PlayerZoneEnemy:
					iconPath = IconSeasonEnemy[num];
					pinPath = IconSeasonEnemy[num];
					sortingOrder = MainBuildOrder.Enemy;
					break;
				case PlayerType.PlayerSeasonEnemy:
					iconPath = IconSeasonEnemy[num];
					pinPath = IconSeasonEnemy[num];
					sortingOrder = MainBuildOrder.Enemy;
					break;
				case PlayerType.PlayerSeasonCamp:
					iconPath = IconSeasonCamp[num];
					pinPath = IconSeasonCamp[num];
					sortingOrder = MainBuildOrder.Enemy;
					break;
				case PlayerType.PlayerSeasonAssist:
					iconPath = IconSeasonAssist[num];
					pinPath = IconSeasonAssist[num];
					sortingOrder = MainBuildOrder.Enemy;
					break;
				default:
					iconPath = IconDefault[num];
					pinPath = IconDefault[num];
					sortingOrder = MainBuildOrder.Other;
					break;
				}
				break;
			default:
				iconPath = IconDefault[num];
				pinPath = IconDefault[num];
				sortingOrder = MainBuildOrder.Other;
				break;
			}
			if (aOSType == AllianceOfficialSkillType.AresMissile)
			{
				SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
				if (curSkinMeta != null && curSkinMeta.IsDarknessMode())
				{
					iconPath = iconPath.Replace("Assets/Main/Sprites/LodIcon/mjc_S2", "Assets/Main/SeasonRes/Shared/Sprites/AllianceGovernmentSkill/missile04/ljq_S4");
					pinPath = iconPath;
				}
			}
		}
		else
		{
			GetPlayerIconPath(out iconPath, out pinPath, out var _, out var _, out sortingOrder);
		}
		if (!(iconPath == curIconPath))
		{
			curIconPath = iconPath;
			if (spriteRenderer != null)
			{
				spriteRenderer.LoadSprite(iconPath);
				spriteRenderer.sortingOrder = (int)sortingOrder;
			}
			if (spriteRenderer2 != null)
			{
				spriteRenderer2.LoadSprite(pinPath);
				spriteRenderer2.sortingOrder = (int)sortingOrder;
			}
			if (spriteRenderer21 != null)
			{
				spriteRenderer21.LoadSprite(iconPath);
				spriteRenderer21.sortingOrder = (int)sortingOrder;
			}
			if (spriteRenderer22 != null)
			{
				spriteRenderer22.LoadSprite(pinPath);
				spriteRenderer22.sortingOrder = (int)sortingOrder;
			}
			if (cityBuilding != null)
			{
				cityBuilding.previewIconPath = iconPath;
				cityBuilding.previewName = (info.IsWerewolf ? info.playerName : UIUtils.FormatServerAllianceName(info.srcServerId, info.alAbbr, info.playerName));
				cityBuilding.previewType = WorldPreviewType.PlayerCity;
			}
		}
	}

	public override void OnWorldColorDirty(string allianceId)
	{
		if (!(gameObject == null) && buildState != 5 && world.GetPointInfo(pointIndex) is BuildPointInfo { itemId: 10100000 } buildPointInfo && buildPointInfo.allianceId == allianceId)
		{
			UpdateLod(world.CurrentLodLevel);
			SetIconSprite(buildPointInfo);
			if (cityBuilding != null)
			{
				cityBuilding.UpdateCityLabel(bUuid);
			}
		}
	}

	private void SetPlayerHead(BuildPointInfo info)
	{
		if (info.GetPlayerType() != PlayerType.PlayerSelf || !info.IsNormalType() || headReq != null)
		{
			return;
		}
		headReq = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Building/WorldBaseHead.prefab");
		headReq.completed += delegate
		{
			if (!(headReq.gameObject == null))
			{
				headReq.gameObject.SetActive(value: true);
				Transform transform = headReq.gameObject.transform;
				transform.SetParent(gameObject.transform.Find("Icon/Sprite"));
				transform.localPosition = new Vector3(0f, 0.35f, 0f);
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.identity;
				string uid = GameEntry.Data.Player.Uid;
				string pic = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetPlayerPic");
				int picVer = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetPlayerPicVer");
				UIPlayerHead uIPlayerHead = transform.Find("headIcon")?.GetComponentInChildren<UIPlayerHead>(includeInactive: true);
				if (uIPlayerHead != null)
				{
					uIPlayerHead.SetData(uid, pic, picVer);
				}
			}
		};
	}

	public void FoldUpBuild()
	{
		if (cityBuilding != null)
		{
			foldUpTime = cityBuilding.DoFoldUpAnim();
			isDoFoldingUp = true;
			startTime = 0f;
			startPos = gameObject.transform.position;
			endPos = startPos + new Vector3(0f, 2.6f, 0f);
			if (world.GetPointInfo(pointIndex) is BuildPointInfo { itemId: 10100000 } buildPointInfo)
			{
				string userData = buildPointInfo.uuid + ";" + buildPointInfo.mainIndex + ";0";
				GameEntry.Event.Fire(EventId.ShowDomeHideEffect, userData);
			}
		}
		if (buildState == 5 && gameObject != null)
		{
			world.AddToDeleteList(pointIndex);
		}
	}

	public override void Destroy()
	{
		AssistanceLabel?.Destroy();
		AssistanceLabel = null;
		AssistanceHero?.Destroy();
		AssistanceHero = null;
		CityFire?.Destroy();
		CityFire = null;
		MummyCityFire?.Destroy();
		MummyCityFire = null;
		if (AdjustLod != null && pointType == 6)
		{
			WorldScene.RecordWorldObjectState(WorldScene.WorldBIType.PlayerBuilding, AdjustLod.LowLodLevelHasShowed);
		}
		fullRenderModeLoaded = false;
		world?.UnregisterLodWatcher(this);
		curIconPath = null;
		if (delayCreateTimer != null)
		{
			GameEntry.Timer.CancelTimer(delayCreateTimer);
			delayCreateTimer = null;
		}
		if (world.GetPointInfo(pointIndex) is BuildPointInfo buildPointInfo)
		{
			world.DestroyAnimalObject(buildPointInfo.uuid);
		}
		if (cityBuilding != null)
		{
			cityBuilding.CSUninit();
			cityBuilding = null;
		}
		if (needFireOutView)
		{
			GameEntry.Event.Fire(EventId.WORLD_BUILD_OUT_VIEW, bUuid);
		}
		if (detectEventInst != null)
		{
			detectEventInst.Destroy();
			detectEventInst = null;
		}
		if (headReq != null)
		{
			headReq.Destroy();
			headReq = null;
		}
		if (selfExplosionInstance != null)
		{
			selfExplosionInstance.Destroy();
			selfExplosionInstance = null;
			uiSelfExplosion = null;
		}
		if (sandWormReq != null)
		{
			sandWormReq.Destroy();
			sandWormReq = null;
			sandWormMono?.Dispose();
			sandWormMono = null;
		}
		if (werewolfHpReq != null)
		{
			werewolfHpReq.Destroy();
			werewolfHpReq = null;
			werewolfHpMono?.Dispose();
			werewolfHpMono = null;
		}
		firePoint = null;
		needShowGoddessMummyBorn = false;
		DisposeLittleSmartMode();
		if (biShowFullRenderTime > 0f)
		{
			if (SceneManager.IsInWorld())
			{
				WorldScene.RecordWorldCityFastBoy(Time.realtimeSinceStartup - biShowFullRenderTime);
			}
			biShowFullRenderTime = -1f;
		}
		base.Destroy();
	}

	public override void OnUpdate(float deltaTime)
	{
		if (cityBuilding != null)
		{
			cityBuilding.CSUpdate(deltaTime);
		}
		if (isDoFoldingUp)
		{
			startTime += deltaTime;
			if (startTime >= foldUpTime)
			{
				Vector3 vector = world.WorldToScreenPoint(gameObject.transform.position);
				if (world.GetPointInfo(pointIndex) is BuildPointInfo buildPointInfo && buildPointInfo.IsMine())
				{
					GameEntry.Lua.ShowFoldUpBuild(vector.x.ToString(), vector.y.ToString(), vector.z.ToString(), bUuid.ToString());
				}
				world.AddToDeleteList(pointIndex);
			}
			else
			{
				gameObject.transform.position = Vector3.Lerp(startPos, endPos, startTime / foldUpTime);
			}
		}
		cityFireRefreshTimer -= deltaTime;
		if (cityFireRefreshTimer <= 0f)
		{
			RefreshCityFire();
			cityFireRefreshTimer = 1f;
		}
	}

	private void RefreshCityFire()
	{
		bool flag = false;
		bool flag2 = false;
		BuildPointInfo buildPointInfo = BuildPointInfo;
		if (buildPointInfo != null)
		{
			long num = GameEntry.Timer?.GetServerTime() ?? 0;
			flag = buildPointInfo.destroyStartTime <= 0 && buildPointInfo.unavailableTime > num;
			flag2 = buildPointInfo.mummyCurseExpireTime > num;
			bool flag3 = !GameEntry.Data.Player.IsInBattleField();
			bool flag4 = false;
			PlayerType playerType = buildPointInfo.GetPlayerType();
			int num2 = world?.CurrentLodLevel ?? 0;
			switch (playerType)
			{
			case PlayerType.PlayerSelf:
				flag4 = num2 > 2;
				break;
			case PlayerType.PlayerAlliance:
			case PlayerType.PlayerAllianceLeader:
				flag4 = num2 > 2 && num2 <= 4;
				break;
			case PlayerType.PlayerOther:
				flag4 = num2 > 2 && num2 <= 3;
				break;
			}
			flag = flag && flag3 && flag4;
			flag2 = flag2 && flag3 && flag4;
		}
		if (flag2)
		{
			flag = false;
		}
		if (flag)
		{
			if (CityFire == null)
			{
				CityFire = AsyncMono<WorldCityFireEffectAsync>.Handle.Load("Assets/Main/Prefabs/MainCity/Eff_ui_zhushou_build_fire.prefab", world?.DynamicObjNode, delegate
				{
					Transform transform = CityFire?.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
						RefreshCityFire();
					}
				});
			}
			else
			{
				CityFire.SetActive(active: true);
			}
		}
		else
		{
			CityFire?.SetActive(active: false);
		}
		if (flag2)
		{
			if (MummyCityFire == null)
			{
				MummyCityFire = AsyncMono<WorldCityFireEffectAsync>.Handle.Load("Assets/Main/SeasonRes/Shared/Prefabs/World/Eff_ui_zhushou_build_fire_blue.prefab", world?.DynamicObjNode, delegate
				{
					Transform transform = MummyCityFire?.Transform;
					if (transform != null)
					{
						transform.localPosition = base.WorldPosition;
						RefreshCityFire();
					}
				});
			}
			else
			{
				MummyCityFire.SetActive(active: true);
			}
		}
		else
		{
			MummyCityFire?.SetActive(active: false);
		}
	}

	public override void OnUpdateIconScale(Quaternion rot, float scale)
	{
	}

	public Vector3 GetPosition()
	{
		return base.WorldPosition;
	}

	public float GetHeight()
	{
		if (cityBuilding != null)
		{
			return cityBuilding.GetHeight();
		}
		return 3f;
	}

	public WorldBuilding GetCityBuilding()
	{
		return cityBuilding;
	}

	public override void CheckShowTroopDestination()
	{
		bool flag = isSHowDestination;
		foreach (WorldMarch ownerMarch in world.GetOwnerMarches(GameEntry.Data.Player.Uid))
		{
			if (ownerMarch.IsVisibleMarch() && ownerMarch.targetUuid == bUuid && ownerMarch.targetUuid != 0L)
			{
				flag = false;
				Vector3 realPos = base.WorldPosition;
				int num = 1;
				EnumDestinationSignalType destinationType = world.GetDestinationType(ownerMarch.uuid, ownerMarch.targetUuid, pointIndex, ownerMarch.target, isFormation: false, ref realPos, ref num);
				ShowTroopDestinationSignal(realPos, destinationType, num);
				break;
			}
		}
		if (flag)
		{
			HideTroopDestinationSignal();
		}
	}

	protected bool NeedShowDetectEventIcon()
	{
		bool result = false;
		if (world.GetPointInfo(pointIndex) is BuildPointInfo buildPointInfo)
		{
			result = !buildPointInfo.IsNormalType();
		}
		return result;
	}

	protected void ShowDetectEvent()
	{
		if (!NeedShowDetectEventIcon())
		{
			return;
		}
		string text = "";
		text = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetDetectEventIdByPointId", pointIndex);
		string eventId = text;
		if (string.IsNullOrEmpty(eventId))
		{
			return;
		}
		detectEventInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/WorldFakePlayerDetectInfo.prefab");
		detectEventInst.completed += delegate
		{
			detectEventInst.gameObject.SetActive(value: true);
			detectEventInst.gameObject.transform.SetParent(gameObject.transform);
			detectEventInst.gameObject.transform.localPosition = Vector3.zero;
			SpriteRenderer component = detectEventInst.gameObject.transform.Find("Transform/Detect_event_quality_icon").GetComponent<SpriteRenderer>();
			SpriteRenderer component2 = detectEventInst.gameObject.transform.Find("Transform/Detect_event_icon").GetComponent<SpriteRenderer>();
			string spritePath = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldFakePlayerDetectBgById", eventId);
			string spritePath2 = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldDetectIconById", eventId);
			component.LoadSprite(spritePath);
			component2.LoadSprite(spritePath2);
			Transform transform = detectEventInst.gameObject.transform;
			bubbleTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
			if (bubbleTouchEvent != null)
			{
				bubbleTouchEvent.onPointerClick = base.OnClickPoint;
			}
		};
	}

	private void PlayCivilianToWolfAnim(BuildPointInfo bi)
	{
		if (!bi.IsWerewolf)
		{
			return;
		}
		if (bi.IsWerewolfBirthing())
		{
			string modelPathById = BuildPointInfo.GetModelPathById(bi.wolfCoveredInfo?.skinId ?? 0, bi.PointType, bi.IsNormalType(), bi.pointIndex, bi.itemId + bi.level, bi.uuid);
			world.CreateVFX(modelPathById, base.WorldPosition, 1.3f, 0f, delegate(GameObject go)
			{
				WorldBuilding component = go.GetComponent<WorldBuilding>();
				if (component != null)
				{
					component.CSInit(new WorldBuilding.Param
					{
						point = pointIndex,
						buildUuid = bUuid,
						buildSceneType = WorldBuilding.BuildSceneType.World
					});
					component.UpdateCityLabel(bi.uuid);
				}
			});
			cityBuilding.PlayTimeline("in");
		}
		else
		{
			cityBuilding.PlayTimeline("idle");
		}
	}

	private void PlayWolfToCivilianAnim(BuildPointInfo bi)
	{
		if (bi.IsWerewolf)
		{
			return;
		}
		WerewolfAnimState state = world.GetWerewolfAnimState(bi.pointIndex);
		string animName;
		float delaySec;
		switch (state)
		{
		default:
			return;
		case WerewolfAnimState.Hit:
			animName = "hit";
			delaySec = 3.25f;
			break;
		case WerewolfAnimState.Alive:
			animName = "alive";
			delaySec = 1.75f;
			break;
		case WerewolfAnimState.Out:
			animName = "out";
			delaySec = 1.75f;
			break;
		case WerewolfAnimState.Attack:
			return;
		}
		string prefabPath = "Assets/Main/SeasonRes/S4/Prefabs/Building/A_build_werewolf_world.prefab";
		world.CreateVFX(prefabPath, base.WorldPosition, 4.5f, 0f, delegate(GameObject go)
		{
			if (state == WerewolfAnimState.Hit)
			{
				Transform modelGo = go.transform.Find("ModelGo");
				if (modelGo != null)
				{
					world.CreateVFX("Assets/Main/SeasonRes/S4/Prefabs/World/WerewolfHPBar.prefab", base.WorldPosition, 2f, 0f, delegate(GameObject hpGo)
					{
						UIWerewolfHpBar component2 = hpGo.GetComponent<UIWerewolfHpBar>();
						byte b = (byte)world.GetWerewolfMaxHp();
						component2.Init(b);
						component2.SetData((byte)(b - 1));
						component2.SetData(b, ani: true);
						component2.transform.SetParent(modelGo);
						component2.transform.localPosition = Vector3.zero;
						component2.transform.localRotation = Quaternion.identity;
						component2.transform.localScale = Vector3.one;
					});
				}
			}
			WorldBuilding component = go.GetComponent<WorldBuilding>();
			if (component != null)
			{
				component.CSInit(new WorldBuilding.Param
				{
					point = pointIndex,
					buildUuid = bUuid,
					buildSceneType = WorldBuilding.BuildSceneType.World
				});
				component.UpdateCityLabel(bi.uuid);
				component.PlayTimeline(animName);
			}
		});
		Transform modelGoTrans = gameObject.transform.Find("ModelGo");
		if (modelGoTrans != null)
		{
			modelGoTrans.gameObject.SetActive(value: false);
		}
		GameEntry.Timer.RegisterTimer(delaySec, delegate
		{
			if (modelGoTrans != null)
			{
				modelGoTrans.gameObject.SetActive(value: true);
			}
		});
	}
}
