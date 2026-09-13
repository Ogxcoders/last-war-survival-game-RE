using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameFramework;
using Protobuf;
using UnityEngine;
using UnityEngine.Playables;

public class WorldTroop : IWorldLodWatcher, WorldCulling.ICullingObject
{
	private class BuffVfx
	{
		private InstanceRequest _instanceRequest;

		public HashSet<int> ContainsBuffId = new HashSet<int>();

		public string Path { get; private set; }

		public long EndTimeStamp { get; set; }

		public long LastPlayTime { get; set; }

		public bool CanVisible { get; private set; }

		public bool Active { get; private set; }

		public void SetPath(string path)
		{
			Path = path;
		}

		public void SetInstReqs(InstanceRequest req)
		{
			_instanceRequest = req;
		}

		public void UpdateEndTime(long timestamp)
		{
			if (timestamp >= EndTimeStamp)
			{
				EndTimeStamp = timestamp;
			}
		}

		public void SetActive(bool active)
		{
			Active = active;
			RefreshActive();
		}

		public void SetVisible(bool visible)
		{
			CanVisible = visible;
			RefreshActive();
		}

		private void RefreshActive()
		{
			if (_instanceRequest != null && _instanceRequest.isDone)
			{
				_instanceRequest.gameObject.SetActive(CanVisible && Active);
			}
		}

		public void AddBuffId(int buffId)
		{
			ContainsBuffId.Add(buffId);
		}

		public int RemoveBuffId(int buffId)
		{
			ContainsBuffId.Remove(buffId);
			int count = ContainsBuffId.Count;
			if (count <= 0)
			{
				DeleteInstance();
			}
			return count;
		}

		private void DeleteInstance()
		{
			if (_instanceRequest != null)
			{
				_instanceRequest.Destroy();
				_instanceRequest = null;
			}
		}

		public void Clear()
		{
			DeleteInstance();
			ContainsBuffId.Clear();
			Path = string.Empty;
			EndTimeStamp = 0L;
			LastPlayTime = 0L;
			CanVisible = false;
		}
	}

	private long lodWatcherUuid = -1L;

	private bool monsterModelLoaded;

	private WorldIconRendererFacade.HappyIcon happyIconMonster;

	private WorldIconRendererFacade.HappyIcon happyLvMonster;

	private const string WorldMapIconColName = "worldmap_icon";

	private Dictionary<string, int> mapIcon2HappyIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
	{
		{ "lyp_daditu_yeguai", 1 },
		{ "lyp_daditu_yeguai02", 2 },
		{ "lyp_daditu_jijieguai", 3 },
		{ "lyp_daditu_jijieguai02", 4 },
		{ "zyf_daditu_guaiwu_huang01", 5 },
		{ "zyf_daditu_guaiwu_huang02", 6 },
		{ "zyf_daditu_guaiwu_huang03", 7 },
		{ "zyf_daditu_guaiwu_huang04", 8 },
		{ "season_monster_general", 9 }
	};

	private float debugGPUInstanceProp = -1f;

	private int happyIconIdx = -1;

	private List<BuffVfx> _tacticalCardLoopIconVfxList = new List<BuffVfx>();

	private Dictionary<string, BuffVfx> _tacticalCardLoopIconVfxDic = new Dictionary<string, BuffVfx>();

	private WorldTroopManager troopManager;

	public float delStartTick;

	private WorldTroopStateMachine sm;

	private InstanceRequest requestInst;

	private InstanceRequest fdInst;

	private SimpleAnimation simpleAni;

	private InstanceRequest _troopDestinationInst;

	private WorldTroopDestinationSignal _troopDestination;

	protected bool isSHowDestination;

	private InstanceRequest shieldRequest;

	private List<InstanceRequest> normalAttackInstList = new List<InstanceRequest>();

	private List<InstanceRequest> skillInstList = new List<InstanceRequest>();

	private List<InstanceRequest> fdInstList = new List<InstanceRequest>();

	private InstanceRequest detectEventInst;

	private InstanceRequest berserkBossAttackEffectInst;

	private InstanceRequest bloodQueenCrazyEffect;

	private InstanceRequest berserkBossDeadEffectInst;

	private InstanceRequest _zombieRushEffectInst;

	private Dictionary<int, InstanceRequest> _statusEffInstDic;

	private Transform transform;

	private Transform cameraFollowTransform;

	private WorldScene world;

	private Vector3 cachedWorldPosition;

	private bool cullVisible;

	private bool _visible;

	private int monsterLevel;

	private bool isDelayDestroy;

	private float delayDestroySec;

	private bool IsAddShield;

	private GameObject model;

	private GameObject icon;

	private Dictionary<string, GameObject> performanceDict = new Dictionary<string, GameObject>();

	private List<GameObject> attackEffects;

	private UIWorldLabel[] labels;

	private SimpleAnimation[] anims;

	private GPUSkinningAnimator[] gpuAnims;

	private TimelinePlayer timelinePlayer;

	private ModelHeight _modelHeight;

	private TouchObjectEventTrigger touchEvent;

	private TouchObjectEventTrigger bubbleTouchEvent;

	private float oriHpBarWidth;

	private SpriteRenderer hpSliderRender;

	private SuperTextMesh hpText;

	private GameObject runningBossRallyTip;

	private Transform hpRoot;

	private UIWhistleBar whistleBar;

	private UIBossHpBar uiBossHpBar;

	private UIFlowerCarHpBar uiFlowerCarHpBar;

	private UIAtkCDBar uiAtkCDBar;

	private GameObject worldBossTip;

	private GameObject rotationRoot;

	private GameObject berserkBossTombRoot;

	private GameObject berserkBossNormalRoot;

	private GameObject berserkBossRewardRoot;

	private Transform modelTextRoot;

	private TextMeshProEx[] modelTexts;

	private const float BoundingSphereRadius = 2f;

	private BoundingSphere boundingSphere = new BoundingSphere(Vector3.zero, 2f);

	private WorldMarch marchInfo;

	private SpriteRenderer spriteRenderer;

	private GameObject headObj;

	private SpriteRenderer headBg;

	private SpriteRenderer headIcon;

	private CircleMeshInstanced headIconInstance;

	private SpriteRenderer marchStateIcon;

	private List<WorldTroopUnit> troopUnits = new List<WorldTroopUnit>();

	private Dictionary<Renderer, Material> _rendererMaterils = new Dictionary<Renderer, Material>();

	private AutoAdjustLod adjuster;

	private S4MonsterEffList s4MonsterEffList;

	private WorldTroopEffectActivity worldTroopEffectActivity;

	private Dictionary<ETroopSoundType, WorldTroopSoundBase> _worldTroopSoundMap = new Dictionary<ETroopSoundType, WorldTroopSoundBase>();

	private const int ZombieRetreatPlotCD = 30;

	private const int ZombieRetreatPlotDuration = 5;

	private float zombieRetreatPlotDelay;

	private static int zombieRetreatPlotEndTS = 0;

	private long lastCheckTime;

	private uint invasionCallDialogType;

	private static readonly int Prop_Fresnel_switch = Shader.PropertyToID("_Fresnel_switch");

	private static readonly int Prop_Fresnel_Color_switch = Shader.PropertyToID("_Fresnel_Color_switch");

	private const int WORLD_MONSTER_FOCUS_OFFSET_Z = 5;

	private const string sheldPath = "Assets/Main/Prefabs/Effect/World/VFX_Shield.prefab";

	public const string skillWordPath = "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab";

	public const string normalWordPath = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab";

	public const string cureWordPath = "Assets/Main/Prefabs/UI/BattleWord/BattleCureBloodTip.prefab";

	public const string battleVictoryPath = "Assets/Main/Prefabs/Effect/World/VFX_VictoryNoUI.prefab";

	public const string battleFailurePath = "Assets/Main/Prefabs/Effect/World/VFX_FailureNoUI.prefab";

	public const string battleDefeatPath = "Assets/Main/Prefabs/Effect/World/VFX_DefeatNoUI.prefab";

	public const string battleBoomPath = "Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_duboom_002.prefab";

	public const string clearEffectPath = "Assets/Main/Prefabs/World/Saiji/Eff_saiji_dsj_dikuai_fangzhi.prefab";

	public const string virusPoisonedBubblePath = "Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_kuosan_du_01.prefab";

	public const string strongholdMonsterBornPath = "Assets/Main/Prefabs/World/Saiji/Eff_Common_chusheng_01.prefab";

	public const string bossBoomPath = "Assets/Main/Prefabs/World/Saiji/Eff_shiti_boom.prefab";

	public const string scoutSuccessPath = "Assets/Main/Prefabs/Effect/World/VFX_ScoutNoUI.prefab";

	public const string BigSandwormBornVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormBornVFX.prefab";

	public const string HugeSandwormBornVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormBornVFX.prefab";

	public const string BigSandwormDeadVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormDeadVFX.prefab";

	public const string HugeSandwormDeadVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormDeadVFX.prefab";

	public const string BigSandwormEscapeVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormEscapeVFX.prefab";

	public const string HugeSandwormEscapeVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormEscapeVFX.prefab";

	public const string BigSandwormAttackVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormAttackVFX.prefab";

	public const string HugeSandwormAttackVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormAttackVFX.prefab";

	public const string BigSandwormStunVFX = "Assets/Main/Prefabs/Effect/World/Sandworm/BigSandwormStunVFX.prefab";

	public const string HugeSandwormStunVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormStunVFX.prefab";

	private const string fdBirthEPath = "Assets/_Art/Effect/prefab/Arms/Feidie/VFX_feidie_brith.prefab";

	private const string fdOutPutEPath = "Assets/_Art/Effect/prefab/Arms/Feidie/VFX_feidie.prefab";

	private const string fdPath = "Assets/_Art/Models/Vehicle/FeiDie/prefab/A_vehicle_fd.prefab";

	private const string challengeBossEffect = "Assets/_Art/Effect/prefab/monster/Dashachong/VFX_shachong_zhaohuan.prefab";

	private const string bloodQueenCrazyEffectPath = "Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_crazy.prefab";

	private const string berserkBossAttackEffectPath = "Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_crazy.prefab";

	private const string berserkBossDeadEffectPath = "Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_kill.prefab";

	private const string zombieRushBaseBoomEffectPath = "Assets/_Art_LastWar/Effect/Prefab/daditu/Eff_daditu_jid_boom_01.prefab";

	private const string MummyBornVfx = "Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_monster_xiaosangshi_bron.prefab";

	private const string MummySummonFlyVfx = "Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_A_build_zhaohuanshenxing_attack_d.prefab";

	private const string ybcPath = "Model/A_vehicle_ybc_prefab";

	private const string iconPath = "Icon";

	private const string arrowTipPath = "ArrowTip";

	private const string performancePath = "Model/Performance";

	private const string spritePath = "Icon/Sprite";

	private const string lablePath = "Label";

	private const string modelPath1 = "Model";

	private const string modelPath2 = "ModelLayer/Model";

	private const string fdEffectPoint = "A_vehicle_fd_skin/Root";

	private const string marchPath = "March_{0}";

	private const string hangRotationRoot = "rotationRoot";

	public const string Anim_Birth = "birth";

	public const string Anim_Idle = "idle";

	public const string Anim_Idle1 = "idle01";

	public const string Anim_Idle2 = "idle02";

	public const string Anim_Run = "run";

	public const string Anim_Walk = "walk";

	public const string Anim_Attack = "attack";

	public const string Anim_Attack_Loop = "attack_move";

	public const string Anim_Hit = "hit";

	public const string Anim_Death = "death";

	public const string Anim_Dead = "dead";

	public const string Anim_Stop = "stop";

	public const string Anim_Back = "back";

	public const string Anim_Pick_Garbage = "xiaoren_work";

	public const string Anim_Pick_Garbage_Run = "xiaoren_run";

	public const string Anim_Pick_Garbage_Success = "xiaoren_show";

	public const string Anim_Detect_Rescue_Start = "huangseyunshuji_01_show";

	public const string Anim_Stun = "stun";

	public const string Anim_Born = "born";

	public const string Anim_Escape = "escape";

	public const string Anim_Eachother = "fight_eachother";

	public const string Anim_Beat = "beat";

	public const string Anim_Get_Beat = "get_beat";

	public const string Anim_Weak = "weak";

	public const string HpBarPath = "HPBar";

	public const string HpBarSliderPath = "HPBar/Slider";

	public const string HpBarTextPath = "HPBar/Text";

	public const string RallyTipPath = "HPBar/RallyTip";

	public const string BerserkBossTombRootPath = "Model/Tomb";

	public const string BerserkBossNormalRootPath = "Model/Normal";

	public const string ModelTextContentPath = "ModelLabel";

	public const string BerserkBossRewardPath = "rewardBubble";

	private const string MonsterColName = "model_name";

	private const string SimpleMonsterColName = "simple_model_name";

	private const string huangPoint = "A_vehicle_ybc_prefab/root";

	public const float AttackRange = 6f;

	public const float range = 4f;

	public const float DummyAttackRange = 3.3f;

	public const float DummyAttackCityRange = 8f;

	public float monsterAttackRange;

	public WorldIconRendererFacade.HappyIcon happyTroopCircile;

	public long defAtkUuid;

	private bool detectEventActiveCache = true;

	private static readonly Vector3[] TankPos = new Vector3[2]
	{
		new Vector3(-0.8f, 0.051f, 2.354f),
		new Vector3(0.739f, 0.051f, 2.313f)
	};

	private static readonly Vector3[] InfantryPos = new Vector3[4]
	{
		new Vector3(-2.038f, -0.138f, 0.79099995f),
		new Vector3(-1.987f, -0.138f, -0.9279999f),
		new Vector3(2.121f, -0.138f, -0.9279999f),
		new Vector3(2.121f, -0.138f, 0.7419999f)
	};

	private static readonly Vector3[] PlanePos = new Vector3[2]
	{
		new Vector3(-1.733f, 2.263f, -1.612f),
		new Vector3(1.909f, 2.443f, -1.8340001f)
	};

	private static readonly Vector3[] GarbageBirthPos = new Vector3[5]
	{
		new Vector3(0f, 0f, 3f),
		new Vector3(2.1f, 0f, 0.4f),
		new Vector3(1.5f, 0f, 1.65f),
		new Vector3(-1.5f, 0f, 1.65f),
		new Vector3(-2.1f, 0f, 0.4f)
	};

	private bool isBattle;

	private bool _isDestroy;

	private long _marchUuid = -1L;

	private bool _isCityStrongholdBoss;

	private bool _OverTimingFinshTime;

	private Coroutine _worldBossTip;

	private bool _idlePlayAttack;

	private bool _hasOldMarch;

	private NewMarchType _oldMarchType;

	private int[] _oldMarchPath;

	private float _oldMarchMonsterHpRatio;

	private byte _oldMarchState;

	private ITimer bornTimer;

	private const string WhistleDisappearVFX = "Assets/Main/SeasonRes/S4/Prefabs/Effect/Eff_ljw_s4_boss_s4_tiangou_xiao_shi.prefab";

	private float releaseSkillTick;

	private Dictionary<string, GameObject> hitEffectDic = new Dictionary<string, GameObject>();

	private Tweener _hpAnimation;

	private int stunVFX;

	private const string S4WanderBossPrefabPath = "A_Monster_Group_S4_1/A_Monster_Boss_S4_tiangou (1)";

	private Tweener _zombieRushDisappearAnimation;

	private List<int> _battleCardWaitRemoveList;

	public long Uid => lodWatcherUuid;

	public bool CanShowLittleSmartMode
	{
		get
		{
			if (!WorldInstancingRenderers.DeviceSupportInstancing)
			{
				return false;
			}
			WorldScene worldScene = world;
			if ((object)worldScene == null || !worldScene.EnableWorldMonsterGPUInstancing)
			{
				return false;
			}
			if (happyIconIdx < 0)
			{
				string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "worldmap_icon");
				if (string.IsNullOrEmpty(templateData))
				{
					happyIconIdx = 0;
				}
				else if (!mapIcon2HappyIndex.TryGetValue(templateData, out happyIconIdx))
				{
					happyIconIdx = 0;
				}
			}
			if (GMSwitch.IsGM)
			{
				if (debugGPUInstanceProp < 0f)
				{
					debugGPUInstanceProp = UnityEngine.Random.Range(1, 101);
				}
				if (debugGPUInstanceProp > (float)GMSwitch.GetInt("DebugWorldPointGPUInstanceProp", 100))
				{
					return false;
				}
			}
			return happyIconIdx > 0;
		}
	}

	private bool CanShowHappyIcon
	{
		get
		{
			if ((world?.CurrentLodLevel ?? 0) > 2 && monsterLevel <= 200 && !monsterModelLoaded)
			{
				return CanShowLittleSmartMode;
			}
			return false;
		}
	}

	protected Dictionary<int, InstanceRequest> StatusEffInstDic
	{
		get
		{
			if (_statusEffInstDic == null)
			{
				_statusEffInstDic = new Dictionary<int, InstanceRequest>();
			}
			return _statusEffInstDic;
		}
	}

	public S4MonsterEffList S4MonsterEffList => s4MonsterEffList;

	public bool isDestroy => _isDestroy;

	public bool IsInstanced
	{
		get
		{
			if (requestInst != null)
			{
				return requestInst.state == InstanceRequest.State.Instanced;
			}
			return false;
		}
	}

	public bool IsInvalid
	{
		get
		{
			if (marchInfo != null && marchInfo.IsValid)
			{
				if (!isDelayDestroy)
				{
					return world.GetMarch(marchInfo.uuid) == null;
				}
				return false;
			}
			return true;
		}
	}

	public bool IsDelayDestroyed
	{
		get
		{
			if (isDelayDestroy)
			{
				return delayDestroySec <= 0f;
			}
			return false;
		}
	}

	public bool IsDelayDestroy => isDelayDestroy;

	public bool DelayApply
	{
		get
		{
			return marchInfo.delayApply;
		}
		set
		{
			marchInfo.delayApply = value;
		}
	}

	public float DelayApplyTime
	{
		get
		{
			return marchInfo.delayApplyTime;
		}
		set
		{
			marchInfo.delayApplyTime = value;
		}
	}

	public bool idlePlayAttack => _idlePlayAttack;

	public byte oldMarchState
	{
		get
		{
			return _oldMarchState;
		}
		set
		{
			_oldMarchState = value;
		}
	}

	public float RelaseSkillTick
	{
		get
		{
			return releaseSkillTick;
		}
		set
		{
			releaseSkillTick = value;
		}
	}

	public int CullingBoundsIndex { get; set; } = -1;

	private void DestroyHappyIcon()
	{
		happyIconMonster?.Destroy();
		happyIconMonster = null;
		happyLvMonster?.Destroy();
		happyLvMonster = null;
	}

	public void UpdateLod(int lod)
	{
		if (!monsterModelLoaded && !CanShowHappyIcon)
		{
			string enemyPrefabPath = GetEnemyPrefabPath();
			StartLoadPrefab(enemyPrefabPath);
			DestroyHappyIcon();
		}
	}

	private void InstantiateMonsterObject(ref string prefabPath)
	{
		if (CanShowHappyIcon)
		{
			if (happyIconMonster == null)
			{
				happyIconMonster = world?.IconRendererFacade?.CreateIcon("HappyWorldMonsterIcon");
				happyIconMonster?.Refresh(GetPosition(), happyIconIdx);
			}
			if (happyLvMonster == null)
			{
				happyLvMonster = world?.IconRendererFacade?.CreateIcon("HappyWorldMonsterLv");
				happyLvMonster?.Refresh(GetPosition(), monsterLevel);
			}
			lodWatcherUuid = marchInfo?.uuid ?? (-1);
			if (lodWatcherUuid > 0)
			{
				world?.RegisterLodWatcher(this);
			}
		}
		else if (!monsterModelLoaded)
		{
			prefabPath = GetEnemyPrefabPath();
		}
	}

	public bool IsBattle()
	{
		return isBattle;
	}

	public void SetIsBattle(bool value)
	{
		isBattle = value;
	}

	public WorldTroop(WorldScene world)
	{
		sm = new WorldTroopStateMachine(this);
		this.world = world;
		_isDestroy = false;
	}

	public void Create(WorldMarch march, WorldTroopManager manager)
	{
		if (march == null)
		{
			return;
		}
		troopManager = manager;
		marchInfo = march;
		_marchUuid = march.uuid;
		_hasOldMarch = true;
		_oldMarchType = march.type;
		_oldMarchPath = (int[])march.path?.Clone();
		_oldMarchMonsterHpRatio = march.monsterHpRatio;
		if (march.IsSandWorm() && march.sandWormData != null)
		{
			_oldMarchState = march.sandWormData.GetState();
		}
		if (IsMonsterTroop() || IsWorldBossTroop())
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "level");
			if (!templateData.IsNullOrEmpty())
			{
				monsterLevel = templateData.ToInt();
			}
			else
			{
				monsterLevel = 1;
			}
			float num = march.GetMonsterAttackRange();
			if (num > 0f)
			{
				monsterAttackRange = num;
			}
		}
		InstantiateTroopObject();
		InitSoundMap();
		GameEntry.Event.Subscribe(EventId.MarchItemUpdateSelf, UpdateSelfMarch);
		if (marchInfo.ownerLightUuid > 0)
		{
			if (marchInfo.IsLightMarch())
			{
				LightDataManager.GetInstance().TryAddLightMarch(marchInfo, world);
			}
			else
			{
				LightDataManager.GetInstance().TryRemoveLightMarch(marchInfo);
			}
		}
		CreateTroopCircleVfx();
	}

	private void CreateTroopCircleVfx()
	{
		WorldMarch worldMarch = marchInfo;
		if (worldMarch != null && worldMarch.NeedCreateTroopLine() && happyTroopCircile == null && happyTroopCircile == null)
		{
			happyTroopCircile = world?.IconRendererFacade?.CreateIcon("HappyTroopCircle");
			Vector4 lineColorVec = WorldTroopLineManager.GetLineColorVec4(marchInfo);
			happyTroopCircile?.SetColor(lineColorVec);
		}
	}

	private void InitSoundMap()
	{
		if (IsPlayerTroop())
		{
			for (ETroopSoundType eTroopSoundType = ETroopSoundType.TroopMarchingSound; eTroopSoundType <= ETroopSoundType.TroopAttackingSound; eTroopSoundType++)
			{
				WorldTroopSoundBase worldTroopSoundBase = eTroopSoundType switch
				{
					ETroopSoundType.TroopMarchingSound => new WorldPlayerTroopSound(), 
					ETroopSoundType.TroopAttackingSound => new WorldPlayerAttackSound(), 
					_ => new WorldTroopSoundBase(), 
				};
				worldTroopSoundBase.Init(this);
				_worldTroopSoundMap.Add(eTroopSoundType, worldTroopSoundBase);
			}
			UpdateSound();
		}
	}

	public void DelayDestroy(float delaySec)
	{
		isDelayDestroy = true;
		delayDestroySec = delaySec;
		if (marchInfo.IsWanderBoss() || marchInfo.IsGeneralAllyBoss())
		{
			marchInfo.monsterHpRatio = 0f;
			TryShowSpuerRunningBossPlot(delaySec - 0.2f);
			SetHpBar(marchInfo.monsterHpRatio, ani: true);
		}
		else if (marchInfo.IsS4WanderBoss())
		{
			uiBossHpBar?.RefreshView(0f, 0f, ani: true);
		}
		else if (marchInfo.IsFlowerCar())
		{
			uiFlowerCarHpBar?.SetData(0f, 0f, ani: true);
		}
		else if (marchInfo.type == NewMarchType.ACT_BERSERK_BOSS)
		{
			RefreshBerserkBossDeadEffectShow();
		}
		else if (marchInfo.IsMonsterOrOrdinaryBoss())
		{
			TryShowInvasionMonsterPlot(delaySec - 0.2f);
			if (marchInfo.IsAlChallengeKirov())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.DelayRemoveKillZombieKirovActionCtrl", marchInfo.uuid, delayDestroySec);
			}
		}
		else if (marchInfo.type == NewMarchType.SANDFISH)
		{
			if (touchEvent != null)
			{
				touchEvent.previewType = WorldPreviewType.Default;
			}
		}
		else if (marchInfo.IsSandWorm())
		{
			world.CreateBattleVFX((marchInfo.monsterSpecialType == 40) ? "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormDeadVFX.prefab" : "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormDeadVFX.prefab", 5f, delegate(GameObject go)
			{
				go.transform.position = GetPosition();
				if (anims != null && anims.Length != 0)
				{
					SimpleAnimation simpleAnimation = anims[0];
					go.transform.rotation = simpleAnimation.transform.rotation;
				}
			});
		}
		else if (marchInfo.IsBloodyQueenMonster())
		{
			if (GetMonsterSpecialType() == 48)
			{
				uiAtkCDBar?.SetValue(0L, 0L);
			}
			if (hpRoot != null && hpRoot.gameObject != null)
			{
				hpRoot.gameObject.SetActive(value: false);
			}
		}
	}

	public void Destroy(long marchuuid = 0L)
	{
		happyTroopCircile?.Destroy();
		happyTroopCircile = null;
		DestroyHappyIcon();
		if (lodWatcherUuid > 0)
		{
			world?.UnregisterLodWatcher(this);
			lodWatcherUuid = -1L;
		}
		monsterModelLoaded = false;
		_isDestroy = true;
		if (sm != null)
		{
			sm.Dispose();
		}
		if (transform != null)
		{
			Vector3 position = transform.position;
			if (marchInfo.type == NewMarchType.CAMEL && world != null)
			{
				world.CreateVFX("Assets/_Art_LastWar/Effect/Prefab/S3/Eff_S3_zhuanhua.prefab", position, 2f);
			}
		}
		if (adjuster != null && marchInfo != null && marchInfo.IsMonster())
		{
			WorldScene.RecordWorldObjectState(WorldScene.WorldBIType.MonsterTroop, adjuster.LowLodLevelHasShowed);
		}
		if (_isCityStrongholdBoss)
		{
			GameEntry.Event.Fire(EventId.HideCityStrongholdBoss, marchuuid);
		}
		if (marchInfo != null && marchInfo.IsCityBoss)
		{
			Vector3 position2 = marchInfo.position;
			WorldTroop troop = world.GetTroop(marchInfo.uuid);
			if (troop != null)
			{
				position2 = troop.GetPosition();
			}
			int zoneIdByPosId = WorldZoneMapData.GetZoneIdByPosId(TileCoord.WorldToTileIndex(position2, ForceChangeScene.World), 5);
			GameEntry.Event.Fire(EventId.CityGhostHideFinish, zoneIdByPosId);
		}
		if (marchInfo != null && marchInfo.IsCityBattleS1Monster())
		{
			GameEntry.Event.Fire(EventId.HideS1RestCityDefendMonster, GetMarchUUID());
		}
		DestroyTroopObject();
		world.RemovePosAndRotationDataByMarchUuid(marchuuid);
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.Destroy();
		}
		troopUnits.Clear();
		if (touchEvent != null)
		{
			touchEvent.onPointerUp = null;
			touchEvent.onPointerDown = null;
			touchEvent.onPointerClick = null;
			touchEvent.onPointerDoubleClick = null;
			touchEvent.onBeginDrag = null;
			touchEvent.onDrag = null;
			touchEvent.onEndDrag = null;
			touchEvent.previewIconPath = null;
			touchEvent.previewName = null;
			touchEvent = null;
		}
		if (requestInst != null)
		{
			requestInst.Destroy();
			requestInst = null;
		}
		if (bubbleTouchEvent != null)
		{
			bubbleTouchEvent.onPointerClick = null;
			bubbleTouchEvent = null;
		}
		if ((marchInfo.type == NewMarchType.NORMAL || marchInfo.type == NewMarchType.CROSS_NORMAL || marchInfo.type == NewMarchType.FAKE_ATTACK || marchInfo.type == NewMarchType.ASSEMBLY_MARCH || marchInfo.type == NewMarchType.ALL_OUT || marchInfo.type == NewMarchType.TREAT_VIRUS || marchInfo.type == NewMarchType.SCOUT || marchInfo.type == NewMarchType.CROSS_SCOUT || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_DONATE || marchInfo.type == NewMarchType.MONSTER_CHALLENGE_DONATE) && marchuuid != 0L)
		{
			GameEntry.Event.Fire(EventId.HideTroopAtkBuildIcon, marchuuid);
			GameEntry.Event.Fire(EventId.HideMarchTrans, marchuuid);
			GameEntry.Event.Fire(EventId.HideTroopName, marchuuid);
		}
		if (marchuuid != 0L)
		{
			GameEntry.Event.Fire(EventId.HideTroopHead, marchuuid);
			if (marchInfo != null && marchInfo.thermalConductor != null)
			{
				GameEntry.Event.Fire(EventId.IcePointObjectOut, marchuuid);
			}
		}
		if (headIconInstance != null)
		{
			headIconInstance.Release();
			headIconInstance = null;
		}
		if (shieldRequest != null)
		{
			shieldRequest.Destroy();
			shieldRequest = null;
		}
		RemoveAttack();
		if (skillInstList != null)
		{
			foreach (InstanceRequest skillInst in skillInstList)
			{
				skillInst?.Destroy();
			}
			skillInstList.Clear();
		}
		if (fdInstList != null)
		{
			foreach (InstanceRequest fdInst in fdInstList)
			{
				fdInst?.Destroy();
			}
			fdInstList.Clear();
		}
		if (detectEventInst != null)
		{
			detectEventInst.Destroy();
			detectEventInst = null;
		}
		if (bloodQueenCrazyEffect != null)
		{
			bloodQueenCrazyEffect.Destroy();
			bloodQueenCrazyEffect = null;
		}
		if (berserkBossAttackEffectInst != null)
		{
			berserkBossAttackEffectInst.Destroy();
			berserkBossAttackEffectInst = null;
		}
		if (berserkBossDeadEffectInst != null)
		{
			berserkBossDeadEffectInst.Destroy();
			berserkBossDeadEffectInst = null;
		}
		if (_zombieRushEffectInst != null)
		{
			_zombieRushEffectInst.Destroy();
			_zombieRushEffectInst = null;
		}
		DestroyTroopDestinationSignal();
		if (marchuuid != 0L)
		{
			GameEntry.Lua.Call("UIUtil.CloseWorldMarchTileUI", marchuuid);
		}
		if (GameEntry.Lua.UIManager.IsWindowOpen("UIAllianceRally"))
		{
			GameEntry.Lua.UIManager.DestroyWindow("UIAllianceRally");
		}
		if (_worldTroopSoundMap != null)
		{
			foreach (KeyValuePair<ETroopSoundType, WorldTroopSoundBase> item in _worldTroopSoundMap)
			{
				item.Value.UnInit();
			}
			_worldTroopSoundMap.Clear();
		}
		_worldTroopSoundMap = null;
		GameEntry.Event.Unsubscribe(EventId.MarchItemUpdateSelf, UpdateSelfMarch);
		if (_marchUuid != -1)
		{
			GameEntry.Event.Fire(EventId.WorldTroopGameObjectDestroy, _marchUuid);
		}
		_marchUuid = -1L;
		if (marchInfo != null && marchInfo.ownerLightUuid > 0)
		{
			LightDataManager.GetInstance().TryRemoveLightMarch(marchInfo);
		}
		if (_statusEffInstDic != null)
		{
			foreach (KeyValuePair<int, InstanceRequest> item2 in _statusEffInstDic)
			{
				item2.Value.Destroy();
			}
			_statusEffInstDic = null;
		}
		_battleCardWaitRemoveList = null;
		monsterAttackRange = 0f;
		RemoveBloodQueenGunnerLine();
		if (_tacticalCardLoopIconVfxList != null)
		{
			foreach (BuffVfx tacticalCardLoopIconVfx in _tacticalCardLoopIconVfxList)
			{
				tacticalCardLoopIconVfx?.Clear();
			}
			_tacticalCardLoopIconVfxList.Clear();
		}
		if (_tacticalCardLoopIconVfxDic != null)
		{
			_tacticalCardLoopIconVfxDic.Clear();
		}
	}

	public void LogDebug(string msg)
	{
		if (marchInfo != null)
		{
			_ = marchInfo.ownerUid == GameEntry.Data.Player.Uid;
		}
	}

	public void UpdateSelfMarch(object o)
	{
		CheckShowTroopDestination();
	}

	private void ShowTroopDestinationSignal(EnumDestinationSignalType signalType, int tileSize)
	{
		if (!(model == null))
		{
			if (_troopDestinationInst == null)
			{
				CreateTroopDestinationSignal(signalType, tileSize);
				isSHowDestination = true;
			}
			else if (_troopDestinationInst != null && _troopDestination != null)
			{
				Vector3 position = GetPosition();
				_troopDestination.SetDestinationForMarch(position, signalType, tileSize);
				isSHowDestination = true;
			}
		}
	}

	private void HideTroopDestinationSignal()
	{
		if (_troopDestinationInst != null)
		{
			if (_troopDestination != null)
			{
				_troopDestination.HideDestination();
			}
			else
			{
				DestroyTroopDestinationSignal();
			}
		}
		isSHowDestination = false;
	}

	private void CreateTroopDestinationSignal(EnumDestinationSignalType signalType, int tileSize)
	{
		if (model == null)
		{
			return;
		}
		_troopDestinationInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
		_troopDestinationInst.completed += delegate
		{
			if (model == null)
			{
				DestroyTroopDestinationSignal();
			}
			else
			{
				_troopDestinationInst.gameObject.transform.SetParent(model.transform);
				_troopDestination = _troopDestinationInst.gameObject.GetComponent<WorldTroopDestinationSignal>();
				Vector3 position = GetPosition();
				_troopDestination.SetDestinationForMarch(position, signalType, tileSize);
			}
		};
	}

	private void DestroyTroopDestinationSignal()
	{
		if (_troopDestinationInst != null)
		{
			_troopDestinationInst.Destroy();
			_troopDestinationInst = null;
			_troopDestination = null;
		}
		isSHowDestination = false;
	}

	private void CheckShowTroopDestination()
	{
	}

	private string GetEnemyPrefabPath()
	{
		string text = string.Empty;
		int monsterId = marchInfo.monsterId;
		bool flag = world.IsInSimpleMode();
		int key = (flag ? 1003 : 4);
		if (WorldScene.ModelPathDic.TryGetValue(monsterId, out var value) && value.TryGetValue(key, out var value2))
		{
			text = value2;
		}
		if (text.IsNullOrEmpty())
		{
			if (flag)
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "simple_model_name");
				if (string.IsNullOrEmpty(text))
				{
					text = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "model_name");
				}
			}
			else
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "model_name");
			}
			string text2 = ChangeModelNameByActivity(marchInfo.monsterId.ToString());
			if (!string.IsNullOrEmpty(text2))
			{
				text = text2;
			}
			if (!WorldScene.ModelPathDic.ContainsKey(monsterId))
			{
				WorldScene.ModelPathDic[monsterId] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[monsterId][key] = text;
		}
		if (string.IsNullOrEmpty(text))
		{
			Log.Error("monster prefab is null, " + marchInfo.monsterId);
		}
		return text;
	}

	private void StartLoadPrefab(string prefabPath)
	{
		if (prefabPath == null || prefabPath.IsNullOrEmpty())
		{
			return;
		}
		monsterModelLoaded = true;
		int priority = (WorldMarchDataManager.IsMyMarch(marchInfo) ? 3 : 0);
		requestInst = world.InstantiateAsyncDynamicObj(prefabPath, priority);
		if (requestInst == null)
		{
			return;
		}
		try
		{
			requestInst.completed += OnGameObjectCreate;
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message + ex.StackTrace);
		}
	}

	private void InstantiateTroopObject()
	{
		string prefabPath = null;
		switch (marchInfo.type)
		{
		case NewMarchType.ASSEMBLY_MARCH:
			prefabPath = "Assets/Main/Prefabs/March/WorldTroop.prefab";
			break;
		case NewMarchType.NORMAL:
		case NewMarchType.DIRECT_MOVE_MARCH:
		case NewMarchType.ALL_OUT:
		case NewMarchType.FAKE_ATTACK:
		case NewMarchType.CROSS_NORMAL:
			if (GameEntry.Data.Player.GetUid() == marchInfo.ownerUid)
			{
				if (marchInfo.target == MarchTargetType.SAMPLE)
				{
					prefabPath = "Assets/Main/Prefabs/March/WorldTroopJunkman.prefab";
					break;
				}
				string text = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetMarchPrefabPath", (int)marchInfo.target);
				prefabPath = ((!string.IsNullOrEmpty(text)) ? text : "Assets/Main/Prefabs/March/WorldTroop.prefab");
			}
			else
			{
				string allianceId = GameEntry.Data.Player.GetAllianceId();
				prefabPath = ((!string.IsNullOrEmpty(allianceId) && allianceId == marchInfo.allianceUid) ? "Assets/Main/Prefabs/March/WorldTroopAlliance.prefab" : ((!marchInfo.ShowFiveHero()) ? "Assets/Main/Prefabs/March/WorldTroopOtherYbc.prefab" : "Assets/Main/Prefabs/March/WorldTroopOther.prefab"));
			}
			break;
		case NewMarchType.TRAIN:
			prefabPath = ((marchInfo.train.type != TrainType.Train) ? "Assets/Main/Prefabs/March/WorldTroopTruck.prefab" : "Assets/Main/Prefabs/March/WorldTroopTrain.prefab");
			break;
		case NewMarchType.ZONE_TRAIN:
			prefabPath = "Assets/Main/SeasonRes/S5/Prefabs/World/WorldTroopHSR.prefab";
			break;
		case NewMarchType.FLOWER_TRAIN:
			prefabPath = "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/WorldTroopFlowerTrain.prefab";
			break;
		case NewMarchType.BOSS:
		case NewMarchType.ACT_BOSS:
		case NewMarchType.CHALLENGE_BOSS:
		case NewMarchType.RUNNING_BOSS:
		case NewMarchType.ZOMBIE_RUSH:
		case NewMarchType.DARK_KNIGHT_CITY:
		case NewMarchType.ACT_BERSERK_BOSS:
		case NewMarchType.BEHEMOTH_BOSS:
		case NewMarchType.BEHEMOTN_SKILL:
		case NewMarchType.MUMMY:
		case NewMarchType.ZONE_MOBILIZATION_BOSS:
		case NewMarchType.RUNNING_MUMMY:
		case NewMarchType.SANDFISH:
		case NewMarchType.CAMEL:
		case NewMarchType.BLOODY_QUEEN:
			prefabPath = GetEnemyPrefabPath();
			break;
		case NewMarchType.MONSTER:
			InstantiateMonsterObject(ref prefabPath);
			break;
		case NewMarchType.DARKNESS_MONSTER:
			InstantiateMonsterObject(ref prefabPath);
			break;
		case NewMarchType.ZOMBIE_RETREAT:
			prefabPath = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetRetreatModelPath", marchInfo.cityId);
			break;
		case NewMarchType.SCOUT:
		case NewMarchType.TREAT_VIRUS:
		case NewMarchType.LOTTO_RECEIVE:
		case NewMarchType.MONSTER_CHALLENGE_DONATE:
		case NewMarchType.CROSS_SCOUT:
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopScout.prefab";
			break;
		case NewMarchType.ZONE_MOBILIZATION_DONATE:
		case NewMarchType.REBUILD_CAR:
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopMobilizationDonate.prefab";
			break;
		case NewMarchType.RESOURCE_HELP:
			prefabPath = "Assets/Main/Prefabs/March/WorldTroop.prefab";
			break;
		case NewMarchType.GOLLOES_EXPLORE:
			prefabPath = "Assets/Main/Prefabs/March/GolloesExploreTroop.prefab";
			break;
		case NewMarchType.GOLLOES_TRADE:
			prefabPath = "Assets/Main/Prefabs/March/GolloesTradeTroop.prefab";
			break;
		case NewMarchType.EXPLORE:
			prefabPath = "Assets/Main/Prefabs/March/WorldTroop.prefab";
			break;
		case NewMarchType.PUZZLE_BOSS:
			prefabPath = "Assets/Main/Prefabs/Monsters/MonsterActBoss.prefab";
			break;
		case NewMarchType.DETECT_ZOMBIE_BUS_TRAIN:
			prefabPath = "Assets/Main/Prefabs/March/WorldTroopZombieBusTrain.prefab";
			break;
		case NewMarchType.POWER_WORKER:
			prefabPath = "Assets/Main/SeasonRes/S4/Prefabs/World/WorldTroopWorkerMan.prefab";
			break;
		}
		StartLoadPrefab(prefabPath);
	}

	private void DestroyTroopObject()
	{
		if (bornTimer != null)
		{
			GameEntry.Timer.CancelTimer(bornTimer);
			bornTimer = null;
			if (model != null)
			{
				model.SetActive(value: true);
			}
		}
		if (stunVFX > 0)
		{
			world.RemoveVFX(stunVFX);
			stunVFX = 0;
		}
		if (IsWorldBossTroop() || marchInfo.IsAisila())
		{
			if (_worldBossTip != null)
			{
				YieldUtils.StopDelayActionWithOutContext(_worldBossTip);
			}
			if (worldBossTip != null)
			{
				worldBossTip.SetActive(value: false);
			}
		}
		if (s4MonsterEffList != null)
		{
			s4MonsterEffList.Dispose();
			s4MonsterEffList = null;
		}
		if (worldTroopEffectActivity != null)
		{
			worldTroopEffectActivity.Dispose();
			worldTroopEffectActivity = null;
		}
		UnLoadFd();
		ClearEffect();
		ResetHpBar();
		uiBossHpBar?.Dispose();
		uiBossHpBar = null;
		uiFlowerCarHpBar?.Dispose();
		uiFlowerCarHpBar = null;
		whistleBar?.Dispose();
		whistleBar = null;
		foreach (KeyValuePair<Renderer, Material> rendererMateril in _rendererMaterils)
		{
			UnityEngine.Object.Destroy(rendererMateril.Key.material);
			rendererMateril.Key.sharedMaterial = rendererMateril.Value;
		}
		_rendererMaterils.Clear();
		if (marchInfo != null)
		{
			if (marchInfo.ShowFiveHero())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveWorldSquad", marchInfo.uuid);
			}
			else if (marchInfo.ShowTrain())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveTrain", marchInfo.train.uuid);
			}
			else if (marchInfo.type == NewMarchType.ZONE_TRAIN)
			{
				world.RemoveHSRTroop(marchInfo.uuid);
			}
			else if (marchInfo.ShowFlowerTrain())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveFlowerTrain", marchInfo.flowerTrain.uuid);
			}
			else if (marchInfo.IsDrillBase())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveAllyDrillBase", marchInfo.uuid);
			}
			else if (marchInfo.IsAisila())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveAisillaCtrl", marchInfo.uuid);
			}
			else if (GetMonsterSpecialType() == 10)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveMonsterProtection", marchInfo.uuid);
			}
			else if (marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveZMBossActionCtrl", marchInfo.uuid);
			}
			else if (marchInfo.IsZombieBusTrain())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.DeleteTroopZombieBusTrain", marchInfo.uuid);
			}
			else if (marchInfo.IsAlChallengeKirov())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveKillZombieKirovActionCtrl", marchInfo.uuid);
			}
		}
		if (requestInst != null)
		{
			requestInst.Destroy();
		}
		if (attackEffects != null)
		{
			attackEffects.Clear();
			attackEffects = null;
		}
		requestInst = null;
		transform = null;
		cameraFollowTransform = null;
		model = null;
		icon = null;
		labels = null;
		anims = null;
		gpuAnims = null;
		timelinePlayer = null;
		cullVisible = false;
		invasionCallDialogType = 0u;
		uiAtkCDBar?.Dispose();
		uiAtkCDBar = null;
		world.RemoveCullingBounds(this);
	}

	public WorldMarch GetMarchInfo()
	{
		return marchInfo;
	}

	public bool WorldTroopObjectIsCreate()
	{
		return transform != null;
	}

	private void OnGameObjectCreate(InstanceRequest request)
	{
		SceneManager.World.RemoveCreateMarchRecordTime(marchInfo);
		if (request.gameObject == null)
		{
			return;
		}
		if (marchInfo.SupportLittleSmartTroopMode())
		{
			troopManager?.TryDestroyLittleSmartTroop(marchInfo.uuid);
		}
		this.transform = request.gameObject.transform;
		cameraFollowTransform = this.transform;
		this.transform.parent = world.DynamicObjNode;
		world.AddCullingBounds(this);
		icon = this.transform.Find("Icon").gameObject;
		model = (this.transform.Find("Model") ?? this.transform.Find("ModelLayer/Model")).gameObject;
		InitLod(request.gameObject);
		if (marchInfo != null && marchInfo.IsMonsterOrBoss())
		{
			GameObject gameObject = this.transform.Find("Model/WorldModel")?.gameObject;
			if (gameObject != null)
			{
				model = gameObject;
			}
		}
		anims = model.GetComponentsInChildren<SimpleAnimation>();
		gpuAnims = model.GetComponentsInChildren<GPUSkinningAnimator>();
		if (model.transform.childCount > 0)
		{
			Transform child = model.transform.GetChild(0);
			if (child != null)
			{
				timelinePlayer = child.GetComponentInChildren<TimelinePlayer>();
			}
		}
		_modelHeight = this.transform.GetComponent<ModelHeight>();
		spriteRenderer = this.transform.Find("Icon/Sprite")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
		if (icon != null)
		{
			headObj = icon.transform.Find("head")?.gameObject;
			if (headObj != null)
			{
				headBg = headObj.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				headIconInstance = headObj.transform.Find("headIconNew")?.GetComponentInChildren<CircleMeshInstanced>(includeInactive: true);
				if (headIconInstance == null)
				{
					headIcon = headObj.transform.Find("headIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
				}
				marchStateIcon = headObj.transform.Find("stateIcon")?.GetComponentInChildren<SpriteRenderer>(includeInactive: true);
			}
		}
		rotationRoot = this.transform.Find("rotationRoot")?.gameObject;
		touchEvent = this.transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
		touchEvent.onPointerDown = OnPointerDown;
		touchEvent.onPointerClick = OnClick;
		touchEvent.onPointerDoubleClick = OnDoubleClick;
		touchEvent.onBeginDrag = OnBeginDrag;
		touchEvent.onDrag = OnDrag;
		touchEvent.onEndDrag = OnEndDrag;
		touchEvent.onPointerUp = OnPointerUp;
		if (marchInfo.IsMonsterOrBoss())
		{
			labels = this.transform.GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
			UIWorldLabel[] array = labels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetLevel(monsterLevel);
			}
			if (marchInfo.IsDrillBase())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateAllyDrillBase", marchInfo, this.transform);
			}
			if (GetMonsterSpecialType() == 10)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateMonsterProtection", marchInfo.uuid);
			}
			if (marchInfo.IsOrdinaryBoss() && marchInfo.startPos == marchInfo.targetPos && !marchInfo.IsCityBattleS1Monster())
			{
				PointInfo pointInfo = world.GetPointInfo(marchInfo.targetPos);
				if (pointInfo != null && (pointInfo.pointType == WorldPointType.WORLD_ALLIANCE_CITY || pointInfo.pointType == WorldPointType.WORLD_CITY_STRONGHOLD))
				{
					_isCityStrongholdBoss = true;
					GameEntry.Event.Fire(EventId.ShowCityStrongholdBoss, GetMarchUUID());
				}
			}
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "attack_effect");
			if (!string.IsNullOrEmpty(templateData))
			{
				if (attackEffects == null)
				{
					attackEffects = new List<GameObject>();
				}
				else
				{
					attackEffects.Clear();
				}
				string[] array2 = templateData.Split(new char[1] { '|' });
				foreach (string n in array2)
				{
					Transform transform = model.transform.Find(n);
					if (transform != null)
					{
						GameObject gameObject2 = transform.gameObject;
						gameObject2.SetActive(value: false);
						attackEffects.Add(gameObject2);
					}
				}
			}
			if (marchInfo.IsAisila())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateAisillaCtrl", marchInfo, this.transform);
			}
			else if (marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateZMBossActionCtrl", marchInfo, this.transform);
			}
			else if (marchInfo.IsAlChallengeKirov())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateKillZombieKirovActionCtrl", marchInfo, this.transform);
			}
			else if (marchInfo.IsCityBattleS1Monster())
			{
				GameEntry.Event.Fire(EventId.ShowS1RestCityDefendMonster, GetMarchUUID());
			}
			ShowS4Effect();
			ShowActivityEffect(request.PrefabPath, request.gameObject);
		}
		else
		{
			GameEntry.Event.Fire(EventId.HideTroopHead, GetMarchUUID());
			GameEntry.Event.Fire(EventId.ShowTroopName, GetMarchUUID());
		}
		if (marchInfo.IsMasstroops())
		{
			fdInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Models/Vehicle/FeiDie/prefab/A_vehicle_fd.prefab");
			fdInst.completed += delegate(InstanceRequest r)
			{
				Transform transform18 = this.transform.Find("Model/A_vehicle_ybc_prefab");
				if (transform18 != null)
				{
					transform18.gameObject.SetActive(value: false);
				}
				r.gameObject.transform.SetParent(model.transform);
				r.gameObject.transform.localPosition = Vector3.zero;
				r.gameObject.transform.localScale = Vector3.one;
				r.gameObject.transform.localRotation = Quaternion.identity;
				simpleAni = r.gameObject.GetComponentInChildren<SimpleAnimation>();
				CreateFdBirthEffect();
				simpleAni.Play("birth");
				simpleAni.PlayQueued("run");
			};
		}
		else if (marchInfo.ShowFiveHero())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateWorldSquad", marchInfo.uuid, model.transform, this.transform.GetComponent<Collider>());
			Transform transform2 = this.transform.Find("Model/A_vehicle_ybc_prefab");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(value: false);
			}
		}
		else if (marchInfo.ShowTrain())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateTrain", marchInfo, model.transform);
			if (marchInfo.train.type == TrainType.Train)
			{
				Transform transform3 = this.transform.Find("CameraFollow");
				if (transform3 != null)
				{
					cameraFollowTransform = transform3;
				}
			}
		}
		else if (marchInfo.type == NewMarchType.ZONE_TRAIN)
		{
			world.AddHSRTroop(marchInfo, model.transform);
			Transform transform4 = this.transform.Find("CameraFollow");
			if (transform4 != null)
			{
				cameraFollowTransform = transform4;
			}
		}
		else if (marchInfo.ShowFlowerTrain())
		{
			Transform transform5 = this.transform.Find("CameraFollow");
			if (transform5 != null)
			{
				cameraFollowTransform = transform5;
			}
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateFlowerTrain", marchInfo, model.transform, cameraFollowTransform.transform);
		}
		else if (marchInfo.IsZombieBusTrain())
		{
			if (touchEvent != null)
			{
				BoxCollider component = touchEvent.transform.GetComponent<BoxCollider>();
				float zombieBusTrainLength = marchInfo.GetZombieBusTrainLength();
				component.size = new Vector3(1f, 2f, zombieBusTrainLength);
				component.center = new Vector3(0f, 1f, -0.5f * zombieBusTrainLength);
			}
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateTroopZombieBusTrain", marchInfo, this.transform);
			Transform transform6 = this.transform.Find("CameraFollow");
			if (transform6 != null)
			{
				cameraFollowTransform = transform6;
			}
		}
		else
		{
			Transform transform7 = this.transform.Find("Model/A_vehicle_ybc_prefab");
			if (transform7 != null)
			{
				transform7.gameObject.SetActive(value: true);
			}
		}
		performanceDict.Clear();
		Transform transform8 = this.transform.Find("Model/Performance");
		if (transform8 != null)
		{
			foreach (Transform item in transform8)
			{
				performanceDict.Add(item.name, item.gameObject);
			}
		}
		if (marchInfo.IsWanderBoss() || marchInfo.IsGeneralAllyBoss() || marchInfo.type == NewMarchType.ACT_BERSERK_BOSS || marchInfo.type == NewMarchType.BEHEMOTH_BOSS || marchInfo.type == NewMarchType.BLOODY_QUEEN || marchInfo.IsCityBoss || marchInfo.IsSandWorm())
		{
			Transform transform10 = this.transform.Find("HPBar");
			Transform transform11 = this.transform.Find("HPBar/Slider");
			Transform transform12 = this.transform.Find("HPBar/Text");
			hpRoot = transform10;
			if (transform10 != null && transform11 != null && transform12 != null)
			{
				hpSliderRender = transform11.GetComponent<SpriteRenderer>();
				hpText = transform12.GetComponent<SuperTextMesh>();
				if (hpSliderRender != null && hpText != null)
				{
					transform10.gameObject.SetActive(value: true);
					oriHpBarWidth = hpSliderRender.size.x;
				}
				else
				{
					oriHpBarWidth = 0f;
				}
				SetHpBar();
				Transform transform13 = this.transform.Find("HPBar/RallyTip");
				if (transform13 != null)
				{
					runningBossRallyTip = transform13.gameObject;
					runningBossRallyTip.SetActive(value: false);
				}
			}
		}
		if (marchInfo.IsS4WanderBoss())
		{
			Transform transform14 = (hpRoot = this.transform.Find("HPBar"));
			if (transform14 != null)
			{
				uiBossHpBar = transform14.GetComponent<UIBossHpBar>();
				uiBossHpBar?.RefreshView(0f, marchInfo.monsterHpRatio);
				whistleBar = transform14.GetComponent<UIWhistleBar>();
				whistleBar?.RefreshView(marchInfo);
			}
		}
		else if (marchInfo.IsFlowerCar())
		{
			Transform transform15 = (hpRoot = this.transform.Find("HPBar"));
			if (transform15 != null)
			{
				uiFlowerCarHpBar = transform15.GetComponent<UIFlowerCarHpBar>();
				uiFlowerCarHpBar?.SetData(marchInfo.darknessMonsterData.hpRatio, marchInfo.darknessMonsterData.flowerCarMonsterData.armorRatio);
			}
		}
		if (marchInfo.IsBloodyQueenMonster() && GetMonsterSpecialType() == 48)
		{
			Transform transform16 = this.transform.Find("AtkBar");
			if (transform16 != null && marchInfo.bloodyQueenMonster != null)
			{
				uiAtkCDBar = transform16.GetComponent<UIAtkCDBar>();
				uiAtkCDBar?.SetValue(marchInfo.bloodyQueenMonster.attackStartTime, marchInfo.bloodyQueenMonster.attackEndTime);
			}
		}
		if (marchInfo.type == NewMarchType.ACT_BERSERK_BOSS)
		{
			berserkBossTombRoot = this.transform.Find("Model/Tomb")?.gameObject;
			berserkBossNormalRoot = this.transform.Find("Model/Normal")?.gameObject;
			modelTextRoot = this.transform.Find("ModelLabel");
			if (modelTextRoot != null)
			{
				modelTexts = modelTextRoot.GetComponentsInChildren<TextMeshProEx>(includeInactive: true);
			}
			berserkBossRewardRoot = this.transform.Find("rewardBubble")?.gameObject;
			if (berserkBossRewardRoot != null)
			{
				bubbleTouchEvent = berserkBossRewardRoot.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (bubbleTouchEvent != null)
				{
					bubbleTouchEvent.onPointerClick = OnClick;
				}
			}
			RefreshBerserkBossState((int)marchInfo.monsterHpRatio);
		}
		else if (marchInfo.type == NewMarchType.REBUILD_CAR)
		{
			SetRebuildInfo(this.transform);
		}
		else if (marchInfo.type == NewMarchType.BLOODY_QUEEN)
		{
			RefreshBloodQueenCrazyEffectShow();
		}
		InitTroopColor();
		InitPositionRotation();
		EnterInitState();
		ShowDetectEvent();
		UpdateIconSprite();
		UpdatePerformance();
		AdjustIcon();
		this.transform.gameObject.SetActive(_visible);
		GameEntry.Event.Fire(EventId.WorldTroopGameObjectCreateFinish, marchInfo.uuid.ToString());
		if (IsWorldBossTroop() && this.transform != null)
		{
			Transform transform17 = this.transform.Find("ArrowTip");
			if (transform17 != null)
			{
				worldBossTip = transform17.gameObject;
			}
			else
			{
				NewMarchType newMarchType = NewMarchType.DEFAULT;
				int num = -1;
				MarchTargetType marchTargetType = MarchTargetType.STATE;
				int num2 = -1;
				if (marchInfo != null)
				{
					num = marchInfo.monsterId;
					newMarchType = marchInfo.type;
					num2 = marchInfo.cityId;
					marchTargetType = marchInfo.target;
				}
				Log.Error($"WorldTroop::ArrowTipNotFound! uuid:{marchInfo.uuid}, type:{newMarchType},cityId: {num2}, monsterId: {num}, targetType: {marchTargetType}");
				worldBossTip = null;
			}
		}
		if (marchInfo.type == NewMarchType.RUNNING_BOSS)
		{
			SetRunningBossRotation();
		}
		InitBossBornStage(0u);
		OnCreateBornAni();
		ShowBattleCardStatusEff();
		RefreshBloodQueenGunnerLine();
	}

	private void ShowS4Effect()
	{
		if (world.GetCurSeasonType() == SeasonType.Darkness && !world.IsInSimpleMode())
		{
			if (s4MonsterEffList != null)
			{
				s4MonsterEffList.Dispose();
			}
			s4MonsterEffList = model.GetComponent<S4MonsterEffList>();
			if (s4MonsterEffList != null)
			{
				s4MonsterEffList.Init(this, world, marchInfo);
			}
		}
	}

	private void ShowActivityEffect(string prefabPath, GameObject parent)
	{
		string effectByActivity = WorldTroopEffectActivity.GetEffectByActivity(marchInfo.monsterId, prefabPath);
		if (!string.IsNullOrEmpty(effectByActivity))
		{
			if (worldTroopEffectActivity == null)
			{
				worldTroopEffectActivity = new WorldTroopEffectActivity();
			}
			worldTroopEffectActivity.BuildEffect(effectByActivity, model);
		}
	}

	private void SetAttackEffectActive(bool isActive)
	{
		if (!(model != null) || attackEffects == null)
		{
			return;
		}
		foreach (GameObject attackEffect in attackEffects)
		{
			attackEffect.SetActive(isActive);
		}
	}

	private void OnWorldBossAttacked(object userData)
	{
		long uuid = ((userData is long) ? ((long)userData) : 0);
		WorldMarch march = world.GetMarch(uuid);
		if (march == null || marchInfo == null || march.targetUuid != marchInfo.uuid)
		{
			return;
		}
		bool flag = marchInfo.type == NewMarchType.BEHEMOTH_BOSS;
		if (marchInfo.IsFrozen() || flag || anims == null || anims.Length == 0)
		{
			return;
		}
		SimpleAnimation anim = anims[0];
		SimpleAnimation.State state = anim.GetState("attack");
		if (state == null || anim.IsPlaying("attack"))
		{
			return;
		}
		TryShowWorldBossTip(state.length);
		Vector3 vector = SceneManager.World.TileIndexToWorld(march.startPos);
		anim.transform.rotation = Quaternion.LookRotation(vector - GetPosition());
		if (marchInfo.IsAisila())
		{
			return;
		}
		anim.Rewind();
		anim.Play("attack");
		SetAttackEffectActive(isActive: true);
		anim.PlayQueued("idle");
		YieldUtils.DelayActionWithOutContext(delegate
		{
			anim.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			SetAttackEffectActive(isActive: false);
		}, state.length);
		if (!marchInfo.IsCityBoss || !(world != null) || SceneManager.World.GetLodLevel() >= 3)
		{
			return;
		}
		string prefabPath = "Assets/Main/SeasonRes/S4/Prefabs/Effect/S4_chengshiBoss/eff_Boss_S4wushi_atk.prefab";
		world.CreateBattleVFX(prefabPath, 3f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
		});
	}

	private void TryShowWorldBossTip(float showTime)
	{
		if (_worldBossTip != null)
		{
			YieldUtils.StopDelayActionWithOutContext(_worldBossTip);
		}
		if (labels.Length == 0 || !(worldBossTip != null))
		{
			return;
		}
		worldBossTip.SetActive(value: true);
		List<string> list = new List<string>();
		string text = GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetActivityBossShout");
		if (string.IsNullOrEmpty(text))
		{
			list.Add("456016");
			list.Add("456021");
			list.Add("456022");
			list.Add("456023");
			list.Add("456024");
		}
		else
		{
			string[] array = text.Split(new char[1] { ',' });
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(array[i]);
			}
		}
		int index = new System.Random().Next(list.Count);
		string tip = GameEntry.Localization.GetString(list[index]);
		UIWorldLabel[] array2 = labels;
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j].SetTip(tip);
		}
		_worldBossTip = YieldUtils.DelayActionWithOutContext(delegate
		{
			if (worldBossTip != null)
			{
				worldBossTip.SetActive(value: false);
			}
		}, showTime);
	}

	public void TryShowWorldBossTipByText(string text, float showTime, Action callback = null)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (_worldBossTip != null)
		{
			YieldUtils.StopDelayActionWithOutContext(_worldBossTip);
		}
		if (labels == null || labels.Length == 0 || !(worldBossTip != null))
		{
			return;
		}
		if (worldBossTip != null)
		{
			worldBossTip.SetActive(value: true);
		}
		string tip = GameEntry.Localization.GetString(text);
		UIWorldLabel[] array = labels;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetTip(tip);
		}
		_worldBossTip = YieldUtils.DelayActionWithOutContext(delegate
		{
			if (worldBossTip != null)
			{
				worldBossTip.SetActive(value: false);
			}
			callback?.Invoke();
		}, showTime);
	}

	public void InitLod(GameObject gameObject)
	{
		LodType lodType = LodType.None;
		bool noOptimizeActivate = false;
		if (marchInfo.type == NewMarchType.NORMAL || marchInfo.type == NewMarchType.CROSS_NORMAL || marchInfo.type == NewMarchType.FAKE_ATTACK || marchInfo.type == NewMarchType.ASSEMBLY_MARCH || marchInfo.type == NewMarchType.SCOUT || marchInfo.type == NewMarchType.CROSS_SCOUT || marchInfo.type == NewMarchType.POWER_WORKER || marchInfo.type == NewMarchType.EXPLORE || marchInfo.type == NewMarchType.RESOURCE_HELP || marchInfo.type == NewMarchType.GOLLOES_EXPLORE || marchInfo.type == NewMarchType.GOLLOES_TRADE || marchInfo.type == NewMarchType.DIRECT_MOVE_MARCH || marchInfo.type == NewMarchType.ALL_OUT || marchInfo.type == NewMarchType.CHALLENGE_BOSS || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_DONATE || marchInfo.type == NewMarchType.MONSTER_CHALLENGE_DONATE)
		{
			string allianceId = GameEntry.Data.Player.GetAllianceId();
			lodType = ((marchInfo.ownerUid == GameEntry.Data.Player.Uid) ? LodType.TroopSelf : ((allianceId.IsNullOrEmpty() || !(marchInfo.allianceUid == allianceId)) ? LodType.TroopOther : LodType.TroopAlly));
			noOptimizeActivate = true;
		}
		else if (marchInfo.IsMonsterOrOrdinaryBoss() || marchInfo.type == NewMarchType.CAMEL)
		{
			lodType = LodType.Monster;
			noOptimizeActivate = false;
		}
		else if (marchInfo.IsWanderBoss() || marchInfo.IsS4WanderBoss() || marchInfo.type == NewMarchType.SANDFISH || marchInfo.type == NewMarchType.MUMMY || marchInfo.type == NewMarchType.BLOODY_QUEEN || marchInfo.type == NewMarchType.BEHEMOTH_BOSS)
		{
			lodType = LodType.RunningBoss;
			noOptimizeActivate = false;
		}
		else if (marchInfo.IsFlowerCar())
		{
			lodType = LodType.FlowerCar;
			noOptimizeActivate = false;
		}
		else if (marchInfo.type == NewMarchType.DARK_KNIGHT_CITY)
		{
			lodType = LodType.DarkKnight;
			noOptimizeActivate = false;
		}
		else if (marchInfo.type == NewMarchType.ZOMBIE_RETREAT)
		{
			lodType = LodType.ZombieRetreat;
			noOptimizeActivate = false;
		}
		else if (marchInfo.type == NewMarchType.ZOMBIE_RUSH)
		{
			lodType = LodType.ZombieRush;
			noOptimizeActivate = false;
		}
		else if (IsWorldBossTroop() || marchInfo.type == NewMarchType.CHALLENGE_BOSS)
		{
			lodType = LodType.WorldBoss;
			noOptimizeActivate = false;
		}
		else if (marchInfo.type == NewMarchType.TRAIN)
		{
			string allianceId2 = GameEntry.Data.Player.GetAllianceId();
			lodType = ((marchInfo.ownerUid == GameEntry.Data.Player.Uid) ? LodType.TrainSelf : ((allianceId2.IsNullOrEmpty() || !(marchInfo.allianceUid == allianceId2)) ? LodType.TrainOther : LodType.TrainAlly));
			noOptimizeActivate = true;
		}
		else if (marchInfo.type == NewMarchType.ZONE_TRAIN)
		{
			lodType = LodType.TrainSelf;
			noOptimizeActivate = true;
		}
		else if (marchInfo.type == NewMarchType.FLOWER_TRAIN)
		{
			lodType = LodType.FlowerTrain;
			noOptimizeActivate = true;
		}
		else if (marchInfo.type == NewMarchType.ACT_BERSERK_BOSS)
		{
			lodType = LodType.BerserkBoss;
			noOptimizeActivate = false;
		}
		else if (marchInfo.type == NewMarchType.DETECT_ZOMBIE_BUS_TRAIN)
		{
			lodType = LodType.ZombieBusTrain;
			noOptimizeActivate = true;
		}
		adjuster = gameObject.GetComponent<AutoAdjustLod>();
		if (lodType != LodType.None)
		{
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(lodType);
			adjuster.SetNoOptimizeActivate(noOptimizeActivate);
		}
		else if (adjuster != null)
		{
			UnityEngine.Object.Destroy(adjuster);
		}
	}

	public void UnLoadFd()
	{
		if (fdInst != null)
		{
			CreateFdBirthEffect();
			fdInst.Destroy();
			fdInst = null;
		}
	}

	public void MoveFd()
	{
		if (fdInst != null && fdInst.gameObject != null)
		{
			fdInst.gameObject.transform.DOLocalMove(Vector3.back * 5f, 0.5f);
			CreateFdOutPutEffect();
		}
	}

	public void CreateFdBirthEffect()
	{
		if (adjuster != null && !adjuster.IsMainShow())
		{
			return;
		}
		InstanceRequest effectInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/Arms/Feidie/VFX_feidie_brith.prefab");
		fdInstList.Add(effectInst);
		effectInst.completed += delegate(InstanceRequest r)
		{
			if (fdInst != null && fdInst.gameObject != null)
			{
				Transform parent = fdInst.gameObject.transform.Find("A_vehicle_fd_skin/Root");
				r.gameObject.transform.SetParent(parent);
				r.gameObject.transform.localPosition = new Vector3(-2f, 0f, 0f);
				r.gameObject.transform.localScale = Vector3.one;
				r.gameObject.transform.localRotation = Quaternion.identity;
				YieldUtils.DelayActionWithOutContext(delegate
				{
					effectInst.Destroy();
				}, 0.5f);
			}
			else
			{
				effectInst.Destroy();
			}
		};
	}

	public void CreateFdOutPutEffect()
	{
		if (adjuster != null && adjuster.IsMainShow())
		{
			return;
		}
		InstanceRequest effectInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/Arms/Feidie/VFX_feidie.prefab");
		fdInstList.Add(effectInst);
		effectInst.completed += delegate(InstanceRequest r)
		{
			if (fdInst != null && fdInst.gameObject != null)
			{
				Transform parent = fdInst.gameObject.transform.Find("A_vehicle_fd_skin/Root");
				r.gameObject.transform.SetParent(parent);
				r.gameObject.transform.localPosition = new Vector3(-2f, 0f, 0f);
				r.gameObject.transform.localScale = Vector3.one;
				r.gameObject.transform.localRotation = Quaternion.identity;
				YieldUtils.DelayActionWithOutContext(delegate
				{
					effectInst.Destroy();
				}, 1f);
			}
			else
			{
				effectInst.Destroy();
			}
		};
	}

	private void InitTroopColor()
	{
	}

	private void EnterInitState()
	{
		if (GetMarchStatus() == MarchStatus.MOVING || GetMarchStatus() == MarchStatus.BACK_HOME || GetMarchStatus() == MarchStatus.CHASING || GetMarchStatus() == MarchStatus.IN_WORM_HOLE)
		{
			if (!marchInfo.inBattle)
			{
				if (marchInfo.path.Length > 1)
				{
					sm.ChangeState(WorldTroopState.Move);
				}
				else if (marchInfo.IsOrdinaryBoss() && GetMonsterSpecialType() == 27)
				{
					sm.ChangeState(WorldTroopState.None);
				}
				else if (GetMarchInfo().IsSandWorm() && GetMarchInfo().sandWormData.IsStunning())
				{
					sm.ChangeState(WorldTroopState.Stun);
				}
				else if (GetMonsterSpecialType() == 47 || GetMonsterSpecialType() == 49)
				{
					sm.ChangeState(WorldTroopState.BloodyQueenMonsterWait);
				}
				else
				{
					sm.ChangeState(WorldTroopState.Idle);
				}
			}
			else
			{
				sm.ChangeState(WorldTroopState.Attack);
			}
		}
		else if (GetMarchStatus() == MarchStatus.STATION)
		{
			sm.ChangeState(WorldTroopState.Idle);
			if (IsMonsterTroop())
			{
				GameEntry.Event.Fire(EventId.MonsterMoveEnd, marchInfo.uuid);
			}
		}
		else if (GetMarchStatus() == MarchStatus.ATTACKING && GetMarchInfo().type != NewMarchType.ACT_BERSERK_BOSS)
		{
			sm.ChangeState(WorldTroopState.Attack);
		}
		else if (GetMarchStatus() == MarchStatus.PICKING || GetMarchStatus() == MarchStatus.SAMPLING || GetMarchStatus() == MarchStatus.GOLLOES_EXPLORING)
		{
			sm.ChangeState(WorldTroopState.PickingGarbage);
		}
		else if (GetMarchStatus() == MarchStatus.DESTROY_WAIT)
		{
			sm.ChangeState(WorldTroopState.AttackBuild);
		}
		else if (GetMarchStatus() == MarchStatus.TRANSPORT_BACK_HOME)
		{
			sm.ChangeState(WorldTroopState.TransPortBackHome);
		}
		else if (GetMarchInfo().type == NewMarchType.ACT_BERSERK_BOSS)
		{
			sm.ChangeState(WorldTroopState.BerserkBossAttack);
		}
		else if (GetMarchInfo().type == NewMarchType.BEHEMOTH_BOSS)
		{
			if (GetMarchInfo().status == MarchStatus.BEHEMOTH_ATTACK_CITY)
			{
				sm.ChangeState(WorldTroopState.WorldTroopPlayAttack);
			}
			else if (GetMarchInfo().status == MarchStatus.BEHEMOTH_ARRIVING)
			{
				float delay = UnityEngine.Random.Range(1, 5);
				YieldUtils.DelayActionWithOutContext(delegate
				{
					if (!_isDestroy)
					{
						PlayAnim("skill");
					}
				}, delay);
			}
			else
			{
				sm.ChangeState(WorldTroopState.None);
			}
		}
		else if (GetMarchStatus() == MarchStatus.WAITING && GetMarchInfo().type == NewMarchType.BLOODY_QUEEN)
		{
			sm.ChangeState(WorldTroopState.BloodyQueenMonsterWait);
		}
		else
		{
			sm.ChangeState(WorldTroopState.None);
		}
	}

	public void RefreshState()
	{
		EnterInitState();
	}

	private void InitPositionRotation()
	{
		MarchStatus marchStatus = GetMarchStatus();
		if ((marchStatus == MarchStatus.MOVING || marchStatus == MarchStatus.BACK_HOME || marchStatus == MarchStatus.CHASING || marchStatus == MarchStatus.IN_WORM_HOLE) && marchInfo.path.Length > 1)
		{
			float passedLen = marchInfo.GetPassedLen();
			WorldTroopPathSegment[] array = CreatePathSegment();
			CalcMoveOnPath(array, 0, passedLen, out var pathIdx, out var _, out var pos);
			SetPosition(pos);
			SetRotation(Quaternion.LookRotation(array[pathIdx].dir));
			return;
		}
		if (marchStatus == MarchStatus.TRANSPORT_BACK_HOME || GetMarchInfo().type == NewMarchType.ACT_BERSERK_BOSS || (GetMarchInfo().type == NewMarchType.RUNNING_MUMMY && marchStatus == MarchStatus.STATION))
		{
			SetPosition(SceneManager.World.TileIndexToWorld(marchInfo.startPos, marchInfo.srcServer));
		}
		else
		{
			SetPosition(SceneManager.World.TileIndexToWorld(marchInfo.targetPos, marchInfo.targetServer));
		}
		if (marchInfo.stationDir == Vector2Int.zero)
		{
			SetRotation(Quaternion.identity);
		}
		else if (!marchInfo.IsOrdinaryBoss() && marchInfo.type != NewMarchType.ZONE_MOBILIZATION_BOSS)
		{
			SetRotation(Quaternion.LookRotation(new Vector3(marchInfo.stationDir.x, 0f, marchInfo.stationDir.y)));
		}
	}

	public bool IsBossTroop()
	{
		return marchInfo.IsOrdinaryBoss();
	}

	public bool IsMonsterTroop()
	{
		if (marchInfo.type != NewMarchType.MONSTER && marchInfo.type != NewMarchType.BOSS && marchInfo.type != NewMarchType.DARKNESS_MONSTER && marchInfo.type != NewMarchType.CHALLENGE_BOSS && marchInfo.type != NewMarchType.DARK_KNIGHT_CITY && marchInfo.type != NewMarchType.RUNNING_BOSS && marchInfo.type != NewMarchType.RUNNING_MUMMY && marchInfo.type != NewMarchType.SANDFISH && marchInfo.type != NewMarchType.ZOMBIE_RUSH && marchInfo.type != NewMarchType.MUMMY && marchInfo.type != NewMarchType.ACT_BERSERK_BOSS && marchInfo.type != NewMarchType.BEHEMOTH_BOSS && marchInfo.type != NewMarchType.BEHEMOTN_SKILL && marchInfo.type != NewMarchType.ZONE_MOBILIZATION_BOSS && marchInfo.type != NewMarchType.CAMEL && marchInfo.type != NewMarchType.REBUILD_CAR)
		{
			return marchInfo.type == NewMarchType.BLOODY_QUEEN;
		}
		return true;
	}

	public int GetMonsterSpecialType()
	{
		if (marchInfo.IsMonsterOrBoss())
		{
			if (!int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "special"), out var result))
			{
				Log.Error($"special is error: id: {marchInfo.monsterId}");
				return -1;
			}
			return result;
		}
		return -1;
	}

	public bool IsCityStrongholdMonsterTroop()
	{
		if (marchInfo.IsMonster())
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "special");
			if ("20".Equals(templateData))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsPlayerTroop()
	{
		if (marchInfo.type != NewMarchType.NORMAL && marchInfo.type != NewMarchType.CROSS_NORMAL && marchInfo.type != NewMarchType.FAKE_ATTACK && marchInfo.type != NewMarchType.ASSEMBLY_MARCH && marchInfo.type != NewMarchType.ALL_OUT)
		{
			return marchInfo.type == NewMarchType.DIRECT_MOVE_MARCH;
		}
		return true;
	}

	public bool IsWorldBossTroop()
	{
		if (marchInfo != null)
		{
			if (marchInfo.type != NewMarchType.ACT_BOSS)
			{
				return marchInfo.type == NewMarchType.PUZZLE_BOSS;
			}
			return true;
		}
		return false;
	}

	public bool IsAttackWorldBoss()
	{
		return marchInfo.target == MarchTargetType.DIRECT_ATTACK_ACT_BOSS;
	}

	public bool IsAttackAisilla()
	{
		return marchInfo.target == MarchTargetType.MONSTER_INVASION_BOSS;
	}

	public bool IsAttackRadarPlayer()
	{
		if (marchInfo.target == MarchTargetType.ATTACK_CITY || marchInfo.target == MarchTargetType.ATTACK_WINTER_STORM_CITY || marchInfo.target == MarchTargetType.ATTACK_EPIDEMIC_CITY)
		{
			PointInfo pointInfo = world.GetPointInfo(marchInfo.targetPos);
			if (pointInfo != null && pointInfo is BuildPointInfo { specialType: Protobuf.SpecialType.DetectEvent })
			{
				return true;
			}
		}
		return false;
	}

	public bool IsMarchTargetAttack()
	{
		MarchTargetType marchTargetType = GetMarchTargetType();
		if (marchTargetType != MarchTargetType.ATTACK_ARMY && marchTargetType != MarchTargetType.ATTACK_MONSTER && marchTargetType != MarchTargetType.ATTACK_SANDWORM && marchTargetType != MarchTargetType.DIRECT_ATTACK_ACT_BOSS && marchTargetType != MarchTargetType.ATTACK_BUILDING && marchTargetType != MarchTargetType.ATTACK_ALLIANCE_CITY && marchTargetType != MarchTargetType.ATTACK_ARMY_COLLECT && marchTargetType != MarchTargetType.RALLY_FOR_BOSS && marchTargetType != MarchTargetType.RALLY_SANDWORM && marchTargetType != MarchTargetType.RALLY_FOR_BUILDING && marchTargetType != MarchTargetType.RALLY_FOR_ALLIANCE_CITY && marchTargetType != MarchTargetType.ATTACK_CITY && marchTargetType != MarchTargetType.ATTACK_WINTER_STORM_CITY && marchTargetType != MarchTargetType.ATTACK_WINTER_ENTITY && marchTargetType != MarchTargetType.ATTACK_ROAD && marchTargetType != MarchTargetType.RALLY_FOR_CITY && marchTargetType != MarchTargetType.RALLY_THRONE && marchTargetType != MarchTargetType.RALLY_DRAGON_BUILDING && marchTargetType != MarchTargetType.ATTACK_THRONE && marchTargetType != MarchTargetType.ASSISTANCE_THRONE && marchTargetType != MarchTargetType.DIG_ICE_ALLY && marchTargetType != MarchTargetType.DIG_ICE_ENEMY && marchTargetType != MarchTargetType.EXPLORE && marchTargetType != MarchTargetType.MONSTER_INVASION_BOSS && marchTargetType != MarchTargetType.ATTACK_PLAYER_RUIN_BUILDING && marchTargetType != MarchTargetType.RALLY_EPIDEMIC_CITY && marchTargetType != MarchTargetType.ATTACK_EPIDEMIC_CITY && marchTargetType != MarchTargetType.ATTACK_EPIDEMIC_BUILDING)
		{
			return marchTargetType == MarchTargetType.RALLY_EPIDEMIC_BUILDING;
		}
		return true;
	}

	public bool CanAttack(float curPathLen)
	{
		if (monsterAttackRange > 0f)
		{
			if (curPathLen > monsterAttackRange)
			{
				return false;
			}
		}
		else if (curPathLen > 3.3f && curPathLen > 8f)
		{
			return false;
		}
		if (string.Equals(GameEntry.Data.Player.GetData("SimpleModeOn"), "YES") && !marchInfo.IsBloodyQueenMonster())
		{
			return false;
		}
		if (world.GetLodLevel() >= 3)
		{
			return false;
		}
		MarchTargetType marchTargetType = GetMarchTargetType();
		switch (marchTargetType)
		{
		case MarchTargetType.RALLY_THRONE:
		case MarchTargetType.SCOUT_THRONE:
		case MarchTargetType.ATTACK_THRONE:
		case MarchTargetType.ASSISTANCE_THRONE:
		case MarchTargetType.ATTACK_SERVER_THRONE_BUILDING:
		case MarchTargetType.ASSISTANCE_SERVER_THRONE_BUILDING:
		case MarchTargetType.SCOUT_SERVER_THRONE_BUILDING:
		case MarchTargetType.RALLY_SERVER_THRONE_BUILDING:
			return false;
		case MarchTargetType.WHISTLE_MONSTER:
			if (0f < curPathLen)
			{
				return curPathLen <= 3.3f;
			}
			return false;
		default:
			if (marchInfo.type == NewMarchType.RUNNING_BOSS && marchTargetType == MarchTargetType.RUNNING_BOSS_ATTACK_CITY)
			{
				return curPathLen <= 3.3f;
			}
			if (marchInfo.type == NewMarchType.RUNNING_MUMMY && marchInfo.targetUuid > 0)
			{
				return curPathLen <= 3.3f;
			}
			if (marchInfo.type == NewMarchType.MUMMY && marchInfo.targetUuid > 0)
			{
				return curPathLen <= 3.3f;
			}
			if (marchInfo.type == NewMarchType.DARK_KNIGHT_CITY)
			{
				return curPathLen <= 8f;
			}
			if (marchInfo.type == NewMarchType.ZOMBIE_RUSH && marchTargetType == MarchTargetType.ZOMBIE_BOSS_ATTACK_CITY)
			{
				return curPathLen <= 3.3f;
			}
			if (marchInfo.type == NewMarchType.ASSEMBLY_MARCH)
			{
				WorldTroop targetTroop = GetTargetTroop();
				if (targetTroop != null && (targetTroop.marchInfo.IsDrillBaseNewBossHugeSandWorm() || targetTroop.marchInfo.IsDrillBaseRoadHog()))
				{
					return true;
				}
			}
			if (monsterAttackRange > 0f)
			{
				return curPathLen <= monsterAttackRange;
			}
			if (!(marchInfo.ownerUid == GameEntry.Data.Player.Uid) || !GameEntry.Lua.CallWithReturn<bool, long>("CSharpCallLuaInterface.HasWorldBattleProcess", marchInfo.uuid))
			{
				return false;
			}
			if (curPathLen <= 8f && (marchTargetType == MarchTargetType.ATTACK_ALLIANCE_CITY || marchTargetType == MarchTargetType.ATTACK_CITY_STRONGHOLD))
			{
				return true;
			}
			if (curPathLen <= 3.3f && IsMarchTargetAttack())
			{
				return true;
			}
			return false;
		}
	}

	public bool IsMarchTargetChangable()
	{
		return GetMarchTargetType() == MarchTargetType.SCOUT_TROOP;
	}

	public bool IsFakeAttackMonsterMarch()
	{
		return marchInfo.isFakeAttack;
	}

	public bool IsMummyMarch()
	{
		return marchInfo.isMummyMarch;
	}

	public void UpdateFakeAttackMonsterMarch()
	{
		if (IsFakeAttackMonsterMarch())
		{
			world.UpdateFakeAttackMonsterMarch(marchInfo.uuid);
		}
	}

	public WorldTroopPathSegment[] CreatePathSegment()
	{
		return marchInfo.CreatePathSegment();
	}

	public static void CalcMoveOnPath(WorldTroopPathSegment[] path, int startIndex, float startPathLen, out int pathIdx, out float pathLen, out Vector3 pos)
	{
		pathIdx = startIndex;
		pathLen = startPathLen;
		while (pathIdx < path.Length && pathLen > path[pathIdx].dist)
		{
			pathLen -= path[pathIdx].dist;
			pathIdx++;
		}
		if (pathIdx < path.Length - 1)
		{
			pos = path[pathIdx].pos + path[pathIdx].dir * pathLen;
		}
		else
		{
			pos = path[^1].pos;
		}
	}

	public void Refresh(WorldMarch march)
	{
		isDelayDestroy = false;
		if (transform == null || march == null)
		{
			return;
		}
		marchInfo = march;
		_idlePlayAttack = false;
		if (_hasOldMarch && _oldMarchType != march.type)
		{
			DestroyTroopObject();
			InstantiateTroopObject();
		}
		else
		{
			if (marchInfo.type == NewMarchType.NORMAL || marchInfo.type == NewMarchType.CROSS_NORMAL || marchInfo.type == NewMarchType.FAKE_ATTACK)
			{
				GameEntry.Event.Fire(EventId.ShowTroopName, GetMarchUUID());
			}
			if (marchInfo.type == NewMarchType.RUNNING_BOSS || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
			{
				int[] oldMarchPath = _oldMarchPath;
				int num = ((oldMarchPath != null) ? oldMarchPath.Length : 0);
				bool flag = num > 1 && marchInfo.path.Length < 2;
				bool flag2 = num < 2 && marchInfo.path.Length > 1;
				if (flag)
				{
					_idlePlayAttack = true;
					SetRunningBossRotation();
					SetPosition(SceneManager.World.TileIndexToWorld(marchInfo.targetPos, marchInfo.targetServer));
					GameEntry.Event.Fire(EventId.TroopPositionChange, marchInfo.uuid);
				}
				else if (flag2)
				{
					SetIsBattle(value: false);
					InitPositionRotation();
					EnterInitState();
				}
			}
		}
		UpdateIconSprite();
		UpdatePerformance();
		ShowScoutSuccess();
		RefreshTrain();
		RefreshFlowerTrain();
		if (marchInfo.ShowFiveHero())
		{
			if (marchInfo.isMummyMarch)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadRefreshMummyMarchSkin", marchInfo.uuid);
			}
			if (marchInfo.HasMeteorite)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadRefreshMeteorite", marchInfo.uuid);
			}
		}
		if ((marchInfo.IsWanderBoss() || marchInfo.IsSandWorm() || marchInfo.IsGeneralAllyBoss() || marchInfo.IsBloodyQueenMonster()) && transform != null)
		{
			if (_hasOldMarch && Math.Abs(_oldMarchMonsterHpRatio - marchInfo.monsterHpRatio) > 0.01f)
			{
				SetHpBar(_oldMarchMonsterHpRatio, ani: true);
			}
			if (marchInfo.monsterRallyNum > 0)
			{
				runningBossRallyTip?.SetActive(value: true);
			}
			else
			{
				runningBossRallyTip?.SetActive(value: false);
			}
			if (marchInfo.monsterSpecialType == 40 && _hasOldMarch && _oldMarchState == 0 && marchInfo.sandWormData.IsStunning())
			{
				_oldMarchState = 1;
				sm.ChangeState(WorldTroopState.Stun);
			}
		}
		else if (march.IsS4WanderBoss() && transform != null)
		{
			if (_hasOldMarch && Math.Abs(_oldMarchMonsterHpRatio - marchInfo.monsterHpRatio) > 0.01f)
			{
				uiBossHpBar?.RefreshView(_oldMarchMonsterHpRatio, marchInfo.monsterHpRatio, ani: true);
			}
			whistleBar?.RefreshView(marchInfo);
		}
		else if (march.IsFlowerCar() && transform != null)
		{
			uiFlowerCarHpBar?.SetData(marchInfo.darknessMonsterData.hpRatio, marchInfo.darknessMonsterData.flowerCarMonsterData.armorRatio, ani: true);
		}
		if (march.type == NewMarchType.ACT_BERSERK_BOSS && transform != null)
		{
			float oldMarchMonsterHpRatio = _oldMarchMonsterHpRatio;
			RefreshBerserkBossState(oldMarchMonsterHpRatio, hpAni: true);
		}
		if (march.type == NewMarchType.BEHEMOTH_BOSS)
		{
			if (march.status == MarchStatus.BEHEMOTH_ATTACK_CITY || march.status == MarchStatus.BEHEMOTH_ARRIVING)
			{
				EnterInitState();
			}
			if (_hasOldMarch && Math.Abs(_oldMarchMonsterHpRatio - marchInfo.monsterHpRatio) > 0.01f)
			{
				SetHpBar(_oldMarchMonsterHpRatio, ani: true);
			}
		}
		if (marchInfo.type == NewMarchType.RUNNING_BOSS)
		{
			SetRunningBossScale(marchInfo.monsterHpRatio);
		}
		if (marchInfo.IsZombieBusTrain())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshTroopZombieBusTrain", marchInfo);
			EnterInitState();
		}
		if (marchInfo != null && marchInfo.IsCityBattleS1Monster())
		{
			GameEntry.Event.Fire(EventId.RefreshS1RestCityDefendMonster, GetMarchUUID());
		}
		if (marchInfo.type == NewMarchType.BLOODY_QUEEN)
		{
			RefreshBloodQueenCrazyEffectShow();
		}
		_hasOldMarch = true;
		_oldMarchType = march.type;
		_oldMarchPath = (int[])march.path?.Clone();
		_oldMarchMonsterHpRatio = march.monsterHpRatio;
		if (marchInfo.ownerLightUuid > 0)
		{
			if (marchInfo.IsLightMarch())
			{
				LightDataManager.GetInstance().TryAddLightMarch(marchInfo, world);
			}
			else
			{
				LightDataManager.GetInstance().TryRemoveLightMarch(marchInfo);
			}
		}
		if (marchInfo.IsBloodyQueenMonster() && GetMonsterSpecialType() == 48)
		{
			uiAtkCDBar?.SetValue(marchInfo.bloodyQueenMonster.attackStartTime, marchInfo.bloodyQueenMonster.attackEndTime);
		}
		RefreshBloodQueenGunnerLine();
	}

	public void RefreshTrain()
	{
		if (marchInfo.ShowTrain())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshTrain", marchInfo);
			EnterInitState();
		}
		else if (marchInfo.type == NewMarchType.ZONE_TRAIN)
		{
			world.RefreshHSRTroop(marchInfo);
		}
	}

	public void RefreshFlowerTrain()
	{
		if (marchInfo.ShowFlowerTrain())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RefreshFlowerTrain", marchInfo);
		}
	}

	public Transform GetTransform()
	{
		return transform;
	}

	public Transform GetCameraFollowTransform()
	{
		return cameraFollowTransform;
	}

	public float GetCameraFollowOffset()
	{
		return (marchInfo.type == NewMarchType.DARKNESS_MONSTER) ? 5 : 0;
	}

	public GameObject GetModel()
	{
		return model;
	}

	private void OnPointerDown()
	{
	}

	private void OnPointerUp()
	{
	}

	private void OnClick()
	{
		if (IsWorldBossTroop())
		{
			if (IsS1SeasonPreBoss())
			{
				if (!IsWeakS1SeasonPreBoss())
				{
					WorldTroopState currentState = sm.GetCurrentState();
					if (currentState == WorldTroopState.S1SeasonPreActBossAttackEachOther)
					{
						if (sm.GetState(currentState) is WorldTroopStateS1SeasonPreActBossAttackEachOther { PlayCirHit: false })
						{
							TryShowWorldBossTipByText(GetS1SeasonPreBossSpeak(3), (float)new System.Random().NextDouble() + 2f);
						}
					}
					else
					{
						TryShowWorldBossTipByText(GetS1SeasonPreBossSpeak(3), (float)new System.Random().NextDouble() + 2f);
					}
				}
			}
			else
			{
				TryShowWorldBossTip((float)new System.Random().NextDouble() + 2f);
			}
		}
		if (GMSwitch.DebugClickLogWarning)
		{
			Log.Warning("ccc:" + marchInfo.Description());
			Log.Warning("Prefab=" + GetEnemyPrefabPath());
		}
		GameEntry.Lua.Call("UIUtil.OnClickWorldTroop", marchInfo.uuid);
	}

	private void OnDoubleClick()
	{
	}

	private void OnBeginDrag(Vector3 dragStartPos)
	{
	}

	private void OnDrag(Vector3 dragStartPos, Vector3 dragCurrPos)
	{
	}

	private void OnEndDrag(Vector3 dragStopPos)
	{
	}

	public void OnUpdate(float deltaTime)
	{
		sm.OnUpdate(deltaTime);
		UpdateTroopUnits();
		if (marchInfo.type == NewMarchType.ZOMBIE_RETREAT && model != null)
		{
			if (zombieRetreatPlotDelay < 0f)
			{
				TryShowRetreatPlot();
			}
			else
			{
				zombieRetreatPlotDelay -= deltaTime;
			}
		}
		if (isDelayDestroy)
		{
			delayDestroySec -= deltaTime;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (serverTime - lastCheckTime > 1000)
		{
			lastCheckTime = serverTime;
			if (marchInfo != null && marchInfo.type == NewMarchType.ACT_BERSERK_BOSS && marchInfo.status == MarchStatus.BERSERK_BOSS_WAITING)
			{
				RefreshBerserkBossCutDownTime();
			}
			else if (marchInfo.IsSandWorm())
			{
				CheckSandWormEscape(serverTime);
			}
			else if (marchInfo != null && marchInfo.IsAllyBaseAttackCitySandWorm() && !_OverTimingFinshTime && serverTime > marchInfo.endTime - 2500)
			{
				_OverTimingFinshTime = true;
				GameEntry.Event.Fire(EventId.ActBossAttack, marchInfo.uuid);
			}
			ShowBattleCardStatusEff();
		}
		if (serverTime - lastCheckTime > 100)
		{
			CheckLoopIconVfx(serverTime);
		}
		if (marchInfo.IsBloodyQueenMonster() && GetMonsterSpecialType() == 48)
		{
			uiAtkCDBar?.OnUpdate();
		}
	}

	private void TryShowRetreatPlot()
	{
		zombieRetreatPlotDelay = 30f;
		int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
		if (serverTimeSeconds >= zombieRetreatPlotEndTS)
		{
			zombieRetreatPlotEndTS = serverTimeSeconds + 5;
			string text = "[" + marchInfo.allianceAbbr + "]" + marchInfo.allianceName;
			string param = GameEntry.Localization.GetString("new_city_activity_battle_tips1020", text);
			GameEntry.Lua.Call("CSharpCallLuaInterface.PlayPlotBubble3DFollow", param, model.transform, 5);
		}
	}

	private void TryShowInvasionMonsterPlot(float duration)
	{
		if (invasionCallDialogType != 0 && invasionCallDialogType <= 3 && (GetMonsterSpecialType() == 9 || GetMonsterSpecialType() == 10) && marchInfo != null && !(model == null))
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.PlayPlotBubble3D", invasionCallDialogType, marchInfo, model.transform, duration);
			invasionCallDialogType = 0u;
		}
	}

	private void TryShowSpuerRunningBossPlot(float duration)
	{
		if (int.Parse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "special")) == 32 && marchInfo != null && !(model == null))
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.PlayPlotBubble3DOfRunningBoss", marchInfo, model.transform, duration);
		}
	}

	public void InitBossBornStage(uint type)
	{
		if (marchInfo != null && !(model == null) && marchInfo.IsOrdinaryBoss() && GetMonsterSpecialType() == 10 && (type != 0 || GameEntry.Lua.CallWithReturn<bool, long>("CSharpCallLuaInterface.GetBossSpeakMark", _marchUuid)))
		{
			invasionCallDialogType = 2u;
			GameEntry.Lua.Call("CSharpCallLuaInterface.PlayPlotBubble3D", invasionCallDialogType, marchInfo, model.transform, 2.14f);
			invasionCallDialogType = 0u;
		}
	}

	private void OnCreateBornAni()
	{
		if (marchInfo.IsWanderBoss())
		{
			if (GetMonsterSpecialType() == 13 && GameEntry.Lua.CallWithReturn<bool, long>("CSharpCallLuaInterface.GetMonsterTroopCreate", _marchUuid))
			{
				PlayBornAni();
			}
		}
		else if (marchInfo.IsSandWorm())
		{
			if (marchInfo.sandWormData == null || !marchInfo.sandWormData.IsBirthing())
			{
				return;
			}
			if (model == null || bornTimer != null)
			{
				return;
			}
			model.SetActive(value: false);
			Transform effStatic = model.transform.GetChild(0)?.Find("Eff_static");
			if (effStatic != null)
			{
				effStatic.gameObject.SetActive(value: false);
			}
			if (hpRoot != null)
			{
				hpRoot.gameObject.SetActive(value: false);
			}
			string prefabPath = ((marchInfo.monsterSpecialType == 40) ? "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormBornVFX.prefab" : "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormBornVFX.prefab");
			world.CreateVFX(prefabPath, GetPosition(), 6f);
			bornTimer = GameEntry.Timer.RegisterTimer(2f, delegate
			{
				if (!(model == null))
				{
					model.SetActive(value: true);
					if (hpRoot != null)
					{
						hpRoot.gameObject.SetActive(value: true);
					}
					PlayBornAni();
					bornTimer = null;
				}
			});
			GameEntry.Timer.RegisterTimer(3f, delegate
			{
				if (effStatic != null)
				{
					effStatic.gameObject.SetActive(value: true);
				}
			});
		}
		else if (marchInfo.type == NewMarchType.RUNNING_MUMMY)
		{
			if (marchInfo.IsBirthing())
			{
				PlayBornAni();
				world.CreateVFX("Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_monster_xiaosangshi_bron.prefab", GetPosition(), 4f);
			}
		}
		else if (marchInfo.type == NewMarchType.MUMMY)
		{
			if (!marchInfo.IsBirthing())
			{
				return;
			}
			if (model == null || bornTimer != null)
			{
				return;
			}
			WorldPointObject objectByPoint = world.GetObjectByPoint(marchInfo.homePos);
			WorldBuildObjectNew bo;
			if (objectByPoint == null || (bo = objectByPoint as WorldBuildObjectNew) == null)
			{
				return;
			}
			model.SetActive(value: false);
			InstanceRequest req = GameEntry.Resource.InstantiateAsync("Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_A_build_zhaohuanshenxing_attack_d.prefab");
			req.completed += delegate
			{
				if (bo.firePoint == null)
				{
					req.Destroy();
				}
				else
				{
					Transform transform = req.gameObject.transform;
					transform.position = bo.firePoint.position;
					DOTween.Sequence().Append(AnimationHelper.DOBezierCurve3D(transform, bo.firePoint.position, GetPosition(), 0.8f, 0.5f, forward: false)).SetEase(Ease.Linear)
						.OnComplete(delegate
						{
							req.Destroy();
						});
				}
			};
			bornTimer = GameEntry.Timer.RegisterTimer(0.8f, delegate
			{
				if (!(model == null))
				{
					model.SetActive(value: true);
					PlayBornAni();
					world.CreateVFX("Assets/Main/SeasonRes/S3/Prefabs/S3_zhaohuanshenxiang/Eff_ljw_S3_monster_xiaosangshi_bron.prefab", GetPosition(), 4f);
					bornTimer = null;
				}
			});
		}
		else
		{
			if (!marchInfo.IsCityBoss || marchInfo.cityBossNum == 0L || marchInfo.type != NewMarchType.BOSS)
			{
				return;
			}
			PlayBornAni();
			if (!(world != null) || SceneManager.World.GetLodLevel() >= 3)
			{
				return;
			}
			string prefabPath2 = "Assets/Main/SeasonRes/S4/Prefabs/Effect/S4_chengshiBoss/eff_Boss_S4wushi_bron.prefab";
			world.CreateBattleVFX(prefabPath2, 3f, delegate(GameObject go)
			{
				if (transform != null)
				{
					go.transform.SetParent(transform);
					go.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			});
			Vector3 position = marchInfo.position;
			WorldTroop troop = world.GetTroop(marchInfo.uuid);
			if (troop != null)
			{
				position = troop.GetPosition();
			}
			int zoneIdByPosId = WorldZoneMapData.GetZoneIdByPosId(TileCoord.WorldToTileIndex(position, ForceChangeScene.World), 5);
			GameEntry.Event.Fire(EventId.CityGhostCreateFinish, zoneIdByPosId);
		}
	}

	public bool PlayBornAni()
	{
		if (anims != null)
		{
			PlayAnim("born");
			PlayQueued("idle");
			return true;
		}
		return false;
	}

	public void MarkInvasionMonsterDialogFlag(uint flag)
	{
		if (marchInfo != null && marchInfo.IsMonsterOrOrdinaryBoss())
		{
			int monsterSpecialType = GetMonsterSpecialType();
			if ((monsterSpecialType == 9 && flag == 1) || (monsterSpecialType == 10 && flag == 3))
			{
				invasionCallDialogType = flag;
			}
		}
	}

	public void AdjustIcon()
	{
	}

	private void UpdateIconSprite()
	{
		int ownerServer = marchInfo.ownerServer;
		bool flag = marchInfo.ownerUid == GameEntry.Data.Player.Uid;
		string allianceId = GameEntry.Data.Player.GetAllianceId();
		string text = null;
		string text2 = null;
		Color color = Color.white;
		WorldPreviewType previewType = WorldPreviewType.Troop;
		if (marchInfo.type == NewMarchType.NORMAL || marchInfo.type == NewMarchType.CROSS_NORMAL || marchInfo.type == NewMarchType.FAKE_ATTACK || marchInfo.type == NewMarchType.ALL_OUT)
		{
			bool flag2 = !allianceId.IsNullOrEmpty() && marchInfo.allianceUid == allianceId;
			bool flag3 = flag2 && marchInfo.ownerUid == GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetAllianceLeaderUid");
			if (!marchInfo.isMummyMarch)
			{
				text = (flag ? "Assets/Main/Sprites/LodIcon/troop_green.png" : (flag2 ? ((!flag3) ? "Assets/Main/Sprites/LodIcon/troop_blue.png" : "Assets/Main/Sprites/LodIcon/troop_leader.png") : (world.IsMyEnemy(ownerServer, marchInfo.allianceUid) switch
				{
					PlayerType.PlayerAllianceEnemy => "Assets/Main/Sprites/LodIcon/troop_enemy_alliance.png", 
					PlayerType.PlayerZoneEnemy => "Assets/Main/Sprites/LodIcon/troop_enemy_zone.png", 
					PlayerType.PlayerSeasonEnemy => "Assets/Main/Sprites/LodIcon/troop_enemy_season.png", 
					PlayerType.PlayerSeasonCamp => "Assets/Main/Sprites/LodIcon/troop_camp_season.png", 
					PlayerType.PlayerSeasonAssist => "Assets/Main/Sprites/LodIcon/troop_assist_season.png", 
					_ => "Assets/Main/Sprites/LodIcon/troop_white.png", 
				})));
			}
			else
			{
				text = "Assets/Main/Sprites/LodIcon/zyf_S3_wujisuofang_6-1.png";
				color = (flag ? ((Color)GameDefines.CityLabelTextColor.Green) : (flag2 ? ((!flag3) ? ((Color)GameDefines.CityLabelTextColor.Blue) : ((Color)GameDefines.CityLabelTextColor.Purple)) : (world.IsMyEnemy(ownerServer, marchInfo.allianceUid) switch
				{
					PlayerType.PlayerAllianceEnemy => GameDefines.CityLabelTextColor.AllianceEnemy, 
					PlayerType.PlayerZoneEnemy => GameDefines.CityLabelTextColor.ZoneEnemy, 
					PlayerType.PlayerSeasonEnemy => GameDefines.CityLabelTextColor.SeasonEnemy, 
					PlayerType.PlayerSeasonCamp => GameDefines.CityLabelTextColor.SeasonCamp, 
					PlayerType.PlayerSeasonAssist => GameDefines.CityLabelTextColor.SeasonAssist, 
					_ => GameDefines.CityLabelTextColor.White, 
				})));
			}
		}
		else if (marchInfo.type == NewMarchType.DIRECT_MOVE_MARCH)
		{
			previewType = WorldPreviewType.Default;
		}
		else if (marchInfo.type == NewMarchType.SCOUT || marchInfo.type == NewMarchType.CROSS_SCOUT || marchInfo.type == NewMarchType.TREAT_VIRUS || marchInfo.type == NewMarchType.LOTTO_RECEIVE || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_DONATE || marchInfo.type == NewMarchType.MONSTER_CHALLENGE_DONATE)
		{
			bool flag4 = !allianceId.IsNullOrEmpty() && marchInfo.allianceUid == allianceId;
			bool flag5 = flag4 && marchInfo.ownerUid == GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetAllianceLeaderUid");
			text = (flag ? "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_05.png" : (flag4 ? ((!flag5) ? "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_02.png" : "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_leader.png") : (world.IsMyEnemy(ownerServer, marchInfo.allianceUid) switch
			{
				PlayerType.PlayerAllianceEnemy => "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_enemy_alliance.png", 
				PlayerType.PlayerZoneEnemy => "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_enemy_zone.png", 
				PlayerType.PlayerSeasonEnemy => "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_enemy_season.png", 
				PlayerType.PlayerSeasonCamp => "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_camp_season.png", 
				PlayerType.PlayerSeasonAssist => "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_assist_season.png", 
				_ => "Assets/Main/Sprites/LodIcon/lyp_daditu_zhencha_04.png", 
			})));
		}
		else if (marchInfo.type == NewMarchType.ZOMBIE_RETREAT)
		{
			previewType = WorldPreviewType.Default;
		}
		else if (marchInfo.target == MarchTargetType.RALLY_FOR_BOSS || marchInfo.target == MarchTargetType.RALLY_FOR_BUILDING || marchInfo.target == MarchTargetType.RALLY_FOR_CITY || marchInfo.target == MarchTargetType.RALLY_THRONE || marchInfo.target == MarchTargetType.RALLY_ALLIANCE_BUILDING || marchInfo.target == MarchTargetType.RALLY_CITY_STRONGHOLD || marchInfo.target == MarchTargetType.RALLY_SERVER_THRONE_BUILDING || marchInfo.target == MarchTargetType.RALLY_DRAGON_BUILDING || marchInfo.target == MarchTargetType.RALLY_FOR_ALLIANCE_CITY || marchInfo.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || marchInfo.target == MarchTargetType.RALLY_EPIDEMIC_CITY)
		{
			bool flag6 = !allianceId.IsNullOrEmpty() && marchInfo.allianceUid == allianceId;
			bool flag7 = flag6 && marchInfo.ownerUid == GameEntry.Lua.CallWithReturn<string>("CSharpCallLuaInterface.GetAllianceLeaderUid");
			text = (flag ? "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_05.png" : (flag6 ? ((!flag7) ? "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_02.png" : "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_leader.png") : (world.IsMyEnemy(ownerServer, marchInfo.allianceUid) switch
			{
				PlayerType.PlayerAllianceEnemy => "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_enemy_alliance.png", 
				PlayerType.PlayerZoneEnemy => "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_enemy_zone.png", 
				PlayerType.PlayerSeasonEnemy => "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_enemy_season.png", 
				PlayerType.PlayerSeasonCamp => "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_camp_season.png", 
				PlayerType.PlayerSeasonAssist => "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_assist_season.png", 
				_ => "Assets/Main/Sprites/LodIcon/lyp_daditu_jijie_04.png", 
			})));
			previewType = WorldPreviewType.Rally;
		}
		else if (marchInfo.type == NewMarchType.TRAIN)
		{
			int quality = marchInfo.train.config.quality;
			int num = Mathf.Min(5, quality);
			if (marchInfo.train.type == TrainType.Truck)
			{
				text = ((quality != 10) ? $"Assets/Main/Sprites/LodIcon/zyf_daditu_kache_00{num}.png" : "Assets/Main/Sprites/LodIcon/lyt_daditu_lu.png");
				previewType = WorldPreviewType.Truck;
			}
			else
			{
				text = $"Assets/Main/Sprites/LodIcon/zyf_daditu_huoche_0{num}.png";
				previewType = WorldPreviewType.Train;
			}
			if (spriteRenderer != null)
			{
				spriteRenderer.flipX = Vector3.Dot(marchInfo.MoveDir, Vector3.right) > 0f;
			}
		}
		else if (marchInfo.type == NewMarchType.ZONE_TRAIN)
		{
			text = "Assets/Main/Sprites/LodIcon/zyf_daditu_huoche_05.png";
			previewType = WorldPreviewType.HSR;
			if (spriteRenderer != null)
			{
				spriteRenderer.flipX = Vector3.Dot(marchInfo.MoveDir, Vector3.right) > 0f;
			}
		}
		else if (marchInfo.IsFlowerTrain())
		{
			text = GameEntry.Lua.CallWithReturn<string, long, int>("CSharpCallLuaInterface.GetFlowerTrainLodIcon", marchInfo.uuid, 1);
		}
		else if (marchInfo.IsMonster())
		{
			text2 = "Assets/Main/Sprites/LodIcon/lyp_daditu_yeguai.png";
			previewType = WorldPreviewType.Monster;
		}
		else if (marchInfo.IsBoss())
		{
			text2 = "Assets/Main/Sprites/LodIcon/lyp_daditu_jijieguai.png";
			previewType = ((!marchInfo.IsDrillBase()) ? WorldPreviewType.Boss : WorldPreviewType.AllyDrillBase);
		}
		touchEvent.previewType = previewType;
		if (!string.IsNullOrEmpty(text))
		{
			touchEvent.previewIconPath = text;
			touchEvent.previewName = UIUtils.FormatServerAllianceName(ownerServer, marchInfo.allianceAbbr, marchInfo.ownerName);
		}
		else if (!string.IsNullOrEmpty(text2))
		{
			touchEvent.previewIconPath = text2;
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "level");
			string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "name");
			if (marchInfo.type == NewMarchType.RUNNING_MUMMY || marchInfo.type == NewMarchType.MUMMY)
			{
				string text3 = UIUtils.FormatAllianceAndName(marchInfo.allianceAbbr, marchInfo.ownerName);
				touchEvent.previewName = GameEntry.Localization.GetString("science_condition", templateData, GameEntry.Localization.GetString(templateData2, text3));
			}
			else
			{
				touchEvent.previewName = GameEntry.Localization.GetString("science_condition", templateData, GameEntry.Localization.GetString(templateData2));
			}
		}
		if (!(spriteRenderer != null))
		{
			return;
		}
		if (!string.IsNullOrEmpty(text))
		{
			spriteRenderer.LoadSprite(text);
			spriteRenderer.color = color;
		}
		if (headObj == null)
		{
			return;
		}
		if (flag && (marchInfo.type == NewMarchType.NORMAL || marchInfo.type == NewMarchType.CROSS_NORMAL || marchInfo.type == NewMarchType.FAKE_ATTACK || marchInfo.type == NewMarchType.ALL_OUT))
		{
			HeroInfo heroInfo = marchInfo?.GetLeaderHero();
			if (heroInfo != null)
			{
				string text4 = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetHeroIcon", heroInfo.heroId);
				if (headIconInstance != null)
				{
					headIconInstance.LoadSprite(text4);
				}
				else
				{
					headIcon.LoadSprite(text4);
				}
				headObj.SetActive(value: true);
			}
			else
			{
				headObj.SetActive(value: false);
			}
			string text5 = GameEntry.Lua.CallWithReturn<string, WorldMarch>("CSharpCallLuaInterface.GetMarchStateIcon", marchInfo);
			marchStateIcon.LoadSprite(text5);
		}
		else if (marchInfo.type == NewMarchType.SCOUT || marchInfo.type == NewMarchType.CROSS_SCOUT)
		{
			headObj.SetActive(flag);
		}
		else
		{
			headObj.SetActive(value: false);
		}
	}

	public void UpdatePerformance()
	{
		UpdateViewForSnowSeason();
		if (performanceDict.Count != 0)
		{
			UpdateSound();
		}
	}

	private void UpdateViewForSnowSeason()
	{
		if (marchInfo.thermalConductor != null)
		{
			GameEntry.Event.Fire(EventId.MonsterIceEffectRefresh, marchInfo.uuid);
		}
	}

	private void ShowDetectEvent()
	{
		if (!string.IsNullOrEmpty(marchInfo.eventId) && marchInfo.belongUid == GameEntry.Data.Player.Uid)
		{
			if (detectEventInst != null)
			{
				return;
			}
			detectEventInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/March/WorldDetectInfo.prefab");
			detectEventInst.completed += delegate
			{
				detectEventInst.gameObject.transform.SetParent(this.transform);
				detectEventInst.gameObject.transform.localPosition = Vector3.zero;
				if (marchInfo.IsZombieBusTrain())
				{
					detectEventInst.gameObject.transform.localScale = Vector3.one * 1.2f;
				}
				SpriteRenderer component = detectEventInst.gameObject.transform.Find("Transform/Detect_event_quality_icon").GetComponent<SpriteRenderer>();
				SpriteRenderer component2 = detectEventInst.gameObject.transform.Find("Transform/Detect_event_icon").GetComponent<SpriteRenderer>();
				string eventId = marchInfo.eventId;
				float y = GameEntry.Lua.CallWithReturn<float, string>("CSharpCallLuaInterface.GetWorldDetectIconHighById", eventId);
				string text = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldDetectBgById", eventId);
				string text2 = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetWorldDetectIconById", eventId);
				component2.transform.localPosition = new Vector3(0f, y, 0f);
				component.LoadSprite(text);
				component2.LoadSprite(text2);
				Transform transform = detectEventInst.gameObject.transform;
				bubbleTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (bubbleTouchEvent != null)
				{
					bubbleTouchEvent.onPointerClick = OnClick;
				}
			};
		}
		else
		{
			HideDetectEvent();
		}
	}

	private void HideDetectEvent()
	{
		if (detectEventInst != null)
		{
			detectEventInst.gameObject.transform.SetParent(null);
			detectEventInst.Destroy();
		}
		detectEventInst = null;
	}

	private void UpdateTroopUnits()
	{
		if (troopUnits.Count <= 0)
		{
			return;
		}
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.Update();
		}
		if (troopUnits.All((WorldTroopUnit i) => i.IsBackFinish()))
		{
			ClearTroopUnits();
		}
	}

	public void Attack()
	{
		if (marchInfo.status == MarchStatus.STATION)
		{
			if (sm.GetCurrentState() != WorldTroopState.Attack)
			{
				sm.ChangeState(WorldTroopState.AttackBegin);
			}
		}
		else if (IsBossTroop() && sm.GetCurrentState() != WorldTroopState.Attack)
		{
			sm.ChangeState(WorldTroopState.Attack);
		}
	}

	public void TryPlayS4IdleAnim(string animName)
	{
		if (sm.GetCurrentState() == WorldTroopState.Idle)
		{
			PlayAnim(animName);
		}
	}

	public void ShowBattleHurt(int hurt, WorldMarchDataManager.BattleWordType worldType)
	{
		if (!(transform == null) && !IsAddShield && (!(adjuster != null) || adjuster.IsMainShow()))
		{
			string path = string.Empty;
			switch (worldType)
			{
			case WorldMarchDataManager.BattleWordType.Cure:
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleCureBloodTip.prefab";
				break;
			case WorldMarchDataManager.BattleWordType.Normal:
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab";
				break;
			case WorldMarchDataManager.BattleWordType.Skill:
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleDecBloodTip.prefab";
				break;
			}
			world.ShowBattleBlood(new BattleDecBloodTip.Param
			{
				startPos = GetPosition(),
				num = hurt,
				path = path
			}, path);
		}
	}

	public void ClearEffect()
	{
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.StopAttackEffect();
		}
	}

	public void ShowBattleSuccess()
	{
		if (IsMonsterTroop() || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		world.CreateBattleVFX("Assets/Main/Prefabs/Effect/World/VFX_VictoryNoUI.prefab", 2.14f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localScale = Vector3.one * 2f;
				go.transform.localPosition = new Vector3(0f, 3.55f, 0f);
			}
		});
	}

	public void ShowBattleFailed()
	{
		if (IsMonsterTroop() || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		world.CreateBattleVFX("Assets/Main/Prefabs/Effect/World/VFX_FailureNoUI.prefab", 2.14f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localScale = Vector3.one * 2f;
				go.transform.localPosition = new Vector3(0f, 3.55f, 0f);
			}
		});
	}

	public void ShowBattleDefeat()
	{
		if ((adjuster != null && !adjuster.IsMainShow()) || !IsMonsterTroop())
		{
			return;
		}
		switch (GetMonsterSpecialType())
		{
		case 20:
			world.CreateBattleVFX("Assets/Main/Prefabs/World/Saiji/Eff_saiji_dsj_dikuai_fangzhi.prefab", 2f, delegate(GameObject go)
			{
				if (transform != null)
				{
					go.transform.SetParent(transform);
					go.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			});
			break;
		case 22:
			world.CreateBattleVFX("Assets/Main/Prefabs/World/Saiji/Eff_shiti_boom.prefab", 2f, delegate(GameObject go)
			{
				if (transform != null)
				{
					go.transform.SetParent(transform);
					go.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			});
			break;
		}
		if (marchInfo.IsAisila() || marchInfo.IsAlChallengeKirov() || marchInfo.type == NewMarchType.ZOMBIE_RUSH || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS || marchInfo.type == NewMarchType.SANDFISH)
		{
			return;
		}
		world.CreateBattleVFX("Assets/Main/Prefabs/Effect/World/VFX_DefeatNoUI.prefab", 2.14f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localPosition = new Vector3(0f, 3.55f, 0f);
			}
		});
		if (GetMonsterSpecialType() != 49)
		{
			return;
		}
		world.CreateBattleVFX("Assets/_Art_LastWar/Effect/Prefab/Common/Eff_Common_duboom_002.prefab", 1f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
		});
	}

	public void ShowScoutSuccess()
	{
		if ((marchInfo.type != NewMarchType.SCOUT && marchInfo.type != NewMarchType.CROSS_SCOUT) || marchInfo.target != MarchTargetType.BACK_HOME || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		world.CreateBattleVFX("Assets/Main/Prefabs/Effect/World/VFX_ScoutNoUI.prefab", 2.14f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localScale = Vector3.one;
				go.transform.localPosition = new Vector3(0f, 3.55f, 0f);
			}
		});
	}

	public bool IsAttackTargetInRange()
	{
		float num = Vector3.Distance(GetPosition(), GetTargetTroopPosition());
		if (num < 6f && num > 0f)
		{
			return true;
		}
		return false;
	}

	public Vector3 GetPosition()
	{
		if (transform != null)
		{
			return cachedWorldPosition;
		}
		return marchInfo.position;
	}

	public Vector3 GetTargetTroopPosition()
	{
		WorldTroop troop = world.GetTroop(marchInfo.targetUuid);
		if (troop != null)
		{
			return troop.GetPosition();
		}
		if (GetMarchStatus() == MarchStatus.TRANSPORT_BACK_HOME)
		{
			return world.TileIndexToWorld(marchInfo.startPos);
		}
		return world.TileIndexToWorld(marchInfo.targetPos);
	}

	public Quaternion GetStationRotation()
	{
		if (marchInfo.stationDir == Vector2Int.zero || IsWorldBossTroop())
		{
			return Quaternion.identity;
		}
		return Quaternion.LookRotation(new Vector3(marchInfo.stationDir.x, 0f, marchInfo.stationDir.y));
	}

	public long GetMarchUUID()
	{
		return marchInfo.uuid;
	}

	public MarchTargetType GetMarchTargetType()
	{
		return marchInfo.target;
	}

	public int GetMarchTargetPos()
	{
		return marchInfo.targetPos;
	}

	public int GetMarchTargetServer()
	{
		return marchInfo.targetServer;
	}

	public bool NeedGetRealTargetPos()
	{
		if (marchInfo.target == MarchTargetType.ATTACK_BUILDING)
		{
			return true;
		}
		if (marchInfo.target == MarchTargetType.ATTACK_ARMY || marchInfo.target == MarchTargetType.ATTACK_MONSTER || marchInfo.target == MarchTargetType.RALLY_FOR_BOSS || marchInfo.target == MarchTargetType.EXPLORE || marchInfo.target == MarchTargetType.SAMPLE || marchInfo.target == MarchTargetType.RALLY_THRONE || marchInfo.target == MarchTargetType.RALLY_DRAGON_BUILDING || marchInfo.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || marchInfo.target == MarchTargetType.ATTACK_THRONE || marchInfo.target == MarchTargetType.ASSISTANCE_THRONE || marchInfo.target == MarchTargetType.PICK_GARBAGE || marchInfo.target == MarchTargetType.RALLY_FOR_BUILDING)
		{
			return true;
		}
		return false;
	}

	public int GetRealMarchTargetPos()
	{
		if (marchInfo.target == MarchTargetType.ATTACK_BUILDING || marchInfo.target == MarchTargetType.RALLY_FOR_BUILDING)
		{
			PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(marchInfo.targetUuid);
			if (pointInfoByUuid != null && pointInfoByUuid.pointType == WorldPointType.PlayerBuilding)
			{
				return pointInfoByUuid.mainIndex;
			}
		}
		else if (marchInfo.target == MarchTargetType.ATTACK_ARMY || marchInfo.target == MarchTargetType.ATTACK_MONSTER || marchInfo.target == MarchTargetType.RALLY_THRONE || marchInfo.target == MarchTargetType.RALLY_DRAGON_BUILDING || marchInfo.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || marchInfo.target == MarchTargetType.ATTACK_THRONE || marchInfo.target == MarchTargetType.ASSISTANCE_THRONE || marchInfo.target == MarchTargetType.RALLY_FOR_BOSS)
		{
			WorldTroop troop = world.GetTroop(marchInfo.targetUuid);
			if (troop != null)
			{
				Vector3 position = troop.GetPosition();
				return world.WorldToTileIndex(position);
			}
		}
		else if (marchInfo.target == MarchTargetType.EXPLORE || marchInfo.target == MarchTargetType.SAMPLE || marchInfo.target == MarchTargetType.PICK_GARBAGE)
		{
			PointInfo pointInfoByUuid2 = world.GetPointInfoByUuid(marchInfo.targetUuid);
			if (pointInfoByUuid2 != null)
			{
				return pointInfoByUuid2.pointIndex;
			}
		}
		else if (marchInfo.target == MarchTargetType.GOLLOES_EXPLORE)
		{
			return (int)marchInfo.targetUuid;
		}
		return 0;
	}

	public MarchStatus GetMarchStatus()
	{
		return marchInfo.status;
	}

	public long GetMarchStartTime()
	{
		return marchInfo.startTime;
	}

	public long GetMarchBlackStartTime()
	{
		return marchInfo.blackStartTime;
	}

	public long GetMarchBlackEndTime()
	{
		return marchInfo.blackEndTime;
	}

	public int[] GetMovePath()
	{
		return marchInfo.path;
	}

	public int GetMovePathCount()
	{
		return marchInfo.path.Length;
	}

	public float GetSpeed()
	{
		return marchInfo.GetMoveSpeed();
	}

	public float GetBlackSpeed()
	{
		return marchInfo.GetBlackSpeed();
	}

	public void SetPosition(Vector3 position)
	{
		if (transform != null)
		{
			transform.position = position;
			boundingSphere.position = position;
			cachedWorldPosition = position;
		}
		happyTroopCircile?.Refresh(position, 0);
	}

	public void SetLocalPosition(Vector3 position)
	{
		if (transform != null)
		{
			transform.localPosition = position;
			boundingSphere.position = transform.TransformPoint(position);
			cachedWorldPosition = transform.position;
		}
	}

	public Quaternion GetRotation()
	{
		if (model != null)
		{
			return model.transform.rotation;
		}
		return Quaternion.identity;
	}

	public void SetRotation(Quaternion rotation, bool includeAnims = false)
	{
		if (!(model != null) || marchInfo.IsDrillBaseNewBossHugeSandWorm() || marchInfo.IsDrillBaseRoadHog())
		{
			return;
		}
		if (!model.transform.rotation.Equals(rotation))
		{
			model.transform.rotation = rotation;
			GameEntry.Event.Fire(EventId.TroopRotation, marchInfo.uuid);
		}
		if (includeAnims)
		{
			SimpleAnimation[] array = anims;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].transform.rotation = rotation;
			}
		}
		if (happyTroopCircile != null)
		{
			float y = rotation.eulerAngles.y;
			happyTroopCircile.SetEulerAngleY(y);
		}
	}

	public void CreateTroopDestination()
	{
	}

	public void DestroyTroopDestination()
	{
	}

	public void UpdateTroopDestination(int targetPos)
	{
	}

	public void SetStateSpeed(string animName, float speed)
	{
		if (anims != null)
		{
			SimpleAnimation[] array = anims;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetStateSpeed(animName, speed);
			}
		}
	}

	public void PlayAnim(string animName)
	{
		PlayAnim(animName, rewind: false);
	}

	public void PlayAnim(string animName, bool rewind)
	{
		if (marchInfo.IsAisila() || marchInfo.IsDrillBaseNewBossHugeSandWorm() || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS || marchInfo.IsAlChallengeKirov())
		{
			return;
		}
		if (anims != null)
		{
			if (anims.Length > 1)
			{
				if ("death".Equals(animName) || "dead".Equals(animName))
				{
					world.StartCoroutine(DelayDeath(anims));
				}
				else
				{
					for (int i = 0; i < anims.Length; i++)
					{
						SimpleAnimation simpleAnimation = anims[i];
						SimpleAnimation.State state = simpleAnimation.GetState(animName);
						if (state != null)
						{
							simpleAnimation[animName].time = UnityEngine.Random.Range(0f, state.length);
							simpleAnimation.Play(animName);
						}
						else if ("death".Equals(animName))
						{
							state = simpleAnimation.GetState("dead");
							if (state != null)
							{
								simpleAnimation["dead"].time = UnityEngine.Random.Range(0f, state.length);
								simpleAnimation.Play("dead");
							}
						}
					}
				}
			}
			else if (anims.Length == 1)
			{
				if (anims[0].GetState(animName) != null)
				{
					anims[0].Play(animName);
				}
				else if ("death".Equals(animName) && anims[0].GetState("dead") != null)
				{
					anims[0].Play("dead");
				}
			}
		}
		if (gpuAnims != null)
		{
			for (int j = 0; j < gpuAnims.Length; j++)
			{
				gpuAnims[j].Play(animName, UnityEngine.Random.Range(0f, 1f));
			}
		}
		if (timelinePlayer != null)
		{
			if ("idle" == animName || "idle01" == animName || "idle02" == animName || "run" == animName || "walk" == animName || "stun" == animName)
			{
				timelinePlayer.PlayTimeline(animName, toIdle: false, DirectorWrapMode.Loop, rewind: false);
			}
			else
			{
				timelinePlayer.PlayTimeline(animName);
			}
		}
		if (marchInfo.ShowFiveHero())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadPlayAnim", marchInfo.uuid, animName, rewind);
		}
	}

	public void PlayQueued(string animName, QueueMode queueMode = QueueMode.CompleteOthers)
	{
		if (marchInfo.IsDrillBaseNewBossHugeSandWorm())
		{
			return;
		}
		if (anims != null)
		{
			if (anims.Length > 1)
			{
				if ("death".Equals(animName) || "dead".Equals(animName))
				{
					world.StartCoroutine(DelayDeath(anims));
				}
				else
				{
					for (int i = 0; i < anims.Length; i++)
					{
						SimpleAnimation simpleAnimation = anims[i];
						SimpleAnimation.State state = simpleAnimation.GetState(animName);
						if (state != null)
						{
							simpleAnimation[animName].time = UnityEngine.Random.Range(0f, state.length);
							simpleAnimation.PlayQueued(animName, queueMode);
						}
						else if ("death".Equals(animName))
						{
							state = simpleAnimation.GetState("dead");
							if (state != null)
							{
								simpleAnimation["dead"].time = UnityEngine.Random.Range(0f, state.length);
								simpleAnimation.PlayQueued("dead", queueMode);
							}
						}
					}
				}
			}
			else if (anims.Length == 1)
			{
				if (anims[0].GetState(animName) != null)
				{
					anims[0].PlayQueued(animName, queueMode);
				}
				else if ("death".Equals(animName) && anims[0].GetState("dead") != null)
				{
					anims[0].PlayQueued("dead", queueMode);
				}
			}
		}
		if (gpuAnims != null)
		{
			for (int j = 0; j < gpuAnims.Length; j++)
			{
				gpuAnims[j].PlayQueued(animName);
			}
		}
		if (timelinePlayer != null)
		{
			timelinePlayer.PlayIdleQueued();
		}
		if (marchInfo.ShowFiveHero())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadPlayAnim", marchInfo.uuid, animName, param3: false);
		}
	}

	public void PlayIdleQueued()
	{
		if ((IsS1SeasonPreBoss() && IsWeakS1SeasonPreBoss()) || marchInfo.IsAisila() || marchInfo.IsDrillBaseNewBossHugeSandWorm())
		{
			return;
		}
		if (anims != null && anims.Length != 0)
		{
			SimpleAnimation[] array = anims;
			foreach (SimpleAnimation simpleAnimation in array)
			{
				if (simpleAnimation.GetState("idle") != null)
				{
					simpleAnimation.PlayQueued("idle");
				}
			}
		}
		if (gpuAnims != null)
		{
			GPUSkinningAnimator[] array2 = gpuAnims;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].PlayQueued("idle");
			}
		}
		if (timelinePlayer != null)
		{
			timelinePlayer.PlayIdleQueued();
		}
	}

	private IEnumerator DelayDeath(SimpleAnimation[] theAnims)
	{
		if (theAnims == null || theAnims.Length == 0)
		{
			yield break;
		}
		foreach (SimpleAnimation simpleAnimation in theAnims)
		{
			SimpleAnimation.State state = simpleAnimation.GetState("death");
			if (state != null)
			{
				simpleAnimation["death"].time = UnityEngine.Random.Range(0f, state.length);
				simpleAnimation.Rewind("death");
				simpleAnimation.Play("death");
				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.2f));
			}
			else
			{
				state = simpleAnimation.GetState("dead");
				if (state != null)
				{
					simpleAnimation["dead"].time = UnityEngine.Random.Range(0f, state.length);
					simpleAnimation.Play("dead");
					yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.2f));
				}
			}
			if (!marchInfo.IsCityBoss || !(world != null) || SceneManager.World.GetLodLevel() >= 3)
			{
				continue;
			}
			string prefabPath = "Assets/Main/SeasonRes/S4/Prefabs/Effect/S4_chengshiBoss/eff_Boss_S4wushi_dead.prefab";
			world.CreateBattleVFX(prefabPath, 3f, delegate(GameObject go)
			{
				if (transform != null)
				{
					go.transform.SetParent(transform);
					go.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			});
		}
	}

	public void PlayLoopAttackAnim()
	{
		if (marchInfo.type == NewMarchType.ZOMBIE_RUSH)
		{
			ShowZombieRushFightingEffect();
		}
		if (anims != null)
		{
			SimpleAnimation[] array = anims;
			foreach (SimpleAnimation simpleAnimation in array)
			{
				SimpleAnimation.State state = simpleAnimation.GetState("attack");
				if (state == null)
				{
					continue;
				}
				if (state.normalizedTime >= 1f)
				{
					state.normalizedTime = 0f;
				}
				if (state.normalizedTime == 0f && marchInfo.NeedShowLoopAttackEffect())
				{
					SetAttackEffectActive(isActive: true);
					YieldUtils.DelayActionWithOutContext(delegate
					{
						SetAttackEffectActive(isActive: false);
					}, state.length);
				}
				simpleAnimation.Play("attack");
			}
		}
		if (marchInfo.ShowFiveHero())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadPlayAnim", marchInfo.uuid, "attack", param3: true);
		}
		if (timelinePlayer != null)
		{
			timelinePlayer.PlayTimeline("attack", toIdle: false, DirectorWrapMode.Loop, rewind: false);
		}
	}

	public MarchStatus GetDefenderMarchStatus()
	{
		return world.GetTroop(marchInfo.targetUuid)?.GetMarchStatus() ?? MarchStatus.DEFAULT;
	}

	public WorldTroop GetTargetTroop()
	{
		long targetUuid = defAtkUuid;
		if (targetUuid == 0L)
		{
			targetUuid = marchInfo.targetUuid;
		}
		return world.GetTroop(targetUuid);
	}

	public WorldPointObject GetTargetPointInfo()
	{
		return world.GetObjectByUuid(marchInfo.targetUuid);
	}

	public void TryPlayBornAnim()
	{
		world.StartCoroutine(DelayBornAnim(anims));
	}

	private IEnumerator DelayBornAnim(SimpleAnimation[] theAnims)
	{
		if (theAnims != null && theAnims.Length != 0)
		{
			foreach (SimpleAnimation simpleAnimation in theAnims)
			{
				SimpleAnimation.State state = simpleAnimation.GetState("death");
				if (state != null)
				{
					simpleAnimation["death"].time = UnityEngine.Random.Range(0f, state.length);
					simpleAnimation.Rewind("death");
					simpleAnimation.Play("death");
					state = simpleAnimation.GetState("born");
					if (state != null)
					{
						simpleAnimation.PlayQueued("born");
					}
					else
					{
						state = simpleAnimation.GetState("birth");
						if (state != null)
						{
							simpleAnimation.PlayQueued("birth");
						}
					}
					yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.2f));
					continue;
				}
				state = simpleAnimation.GetState("dead");
				if (state == null)
				{
					continue;
				}
				simpleAnimation["dead"].time = UnityEngine.Random.Range(0f, state.length);
				simpleAnimation.Play("dead");
				state = simpleAnimation.GetState("born");
				if (state != null)
				{
					simpleAnimation.PlayQueued("born");
				}
				else
				{
					state = simpleAnimation.GetState("birth");
					if (state != null)
					{
						simpleAnimation.PlayQueued("birth");
					}
				}
				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.2f));
			}
		}
		yield return new WaitForSeconds(1f);
		world.CreateBattleVFX("Assets/Main/Prefabs/World/Saiji/Eff_Common_chusheng_01.prefab", 2f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
		});
	}

	private void EnterDefendState(Vector3 attackDir, float attackDuration)
	{
		if (IsS1SeasonPreBoss())
		{
			if (IsWeakS1SeasonPreBoss())
			{
				return;
			}
			WorldTroopState currentState = sm.GetCurrentState();
			if (currentState == WorldTroopState.S1SeasonPreActBossAttackEachOther && sm.GetState(currentState) is WorldTroopStateS1SeasonPreActBossAttackEachOther { PlayCirHit: not false })
			{
				return;
			}
		}
		if (sm.GetState(WorldTroopState.Defend) is WorldTroopStateDefend worldTroopStateDefend)
		{
			worldTroopStateDefend.SetParam(attackDir, attackDuration);
			sm.ChangeState(WorldTroopState.Defend);
		}
	}

	public void TriggerDefenderToAfraidLight()
	{
		if (marchInfo != null && world.GetBloodyNightState() == BloodyNightState.Silent)
		{
			WorldTroop targetTroop = GetTargetTroop();
			if (targetTroop != null && targetTroop.S4MonsterEffList != null)
			{
				targetTroop.S4MonsterEffList.ShowAfraidLight();
			}
		}
	}

	public void TriggerDefenderToDefend(float attackDuration, Vector3 attackDir)
	{
		if (marchInfo == null)
		{
			return;
		}
		WorldTroop targetTroop = GetTargetTroop();
		if (targetTroop != null)
		{
			WorldMarch worldMarch = targetTroop.GetMarchInfo();
			if ((!worldMarch.IsFrozen() && targetTroop.IsMonsterTroop() && worldMarch.type != NewMarchType.BEHEMOTH_BOSS && targetTroop.GetMarchInfo().type != NewMarchType.ACT_BERSERK_BOSS && (!worldMarch.IsSandWorm() || !worldMarch.sandWormData.IsStunning()) && !targetTroop.GetMarchInfo().IsBloodyQueenMonster()) || IsAttackWorldBoss() || IsAttackAisilla())
			{
				if (targetTroop.GetMarchInfo().IsDrillBaseNewBossHugeSandWorm())
				{
					GameEntry.Event.Fire(EventId.ActBossAttack, marchInfo.uuid);
				}
				else if (!IsDelayDestroy)
				{
					targetTroop.EnterDefendState(attackDir, attackDuration);
				}
			}
		}
		else if (marchInfo.target == MarchTargetType.ATTACK_SANDWORM)
		{
			WorldPointObject targetPointInfo = GetTargetPointInfo();
			if (targetPointInfo != null && targetPointInfo is WorldBuildObjectNew worldBuildObjectNew)
			{
				worldBuildObjectNew.SandWormAttack(attackDir);
			}
		}
	}

	public void TriggerDefenderToBeWhistled(float whistleDuration)
	{
		if (marchInfo == null)
		{
			return;
		}
		WorldTroop targetTroop = GetTargetTroop();
		if (targetTroop != null)
		{
			targetTroop.whistleBar?.SetWhistling();
			world.CreateVFX("Assets/Main/SeasonRes/S4/Prefabs/Effect/Eff_ljw_s4_boss_s4_tiangou_xiao_shi.prefab", targetTroop.GetS4WanderBossWorldPosition(), 2.5f, Mathf.Max(0f, whistleDuration - 0.1f));
			world.CreateVFX("Assets/Main/SeasonRes/S4/Prefabs/Effect/Eff_ljw_s4_boss_s4_tiangou_xiao_shi.prefab", Vector3.zero, 2.5f, whistleDuration + 0.1f, delegate(GameObject go)
			{
				go.transform.position = targetTroop.GetS4WanderBossWorldPosition();
			});
		}
	}

	public void PlaySandWormAnim(string animName, Vector3 target)
	{
		if (anims == null || anims.Length == 0)
		{
			return;
		}
		for (int i = 0; i < anims.Length; i++)
		{
			SimpleAnimation anim = anims[i];
			if (!(anim != null) || !(anim.transform != null) || anim.GetState(animName) == null)
			{
				continue;
			}
			anim.Rewind(animName);
			anim.Play(animName);
			if (animName == "attack")
			{
				anim.transform.rotation = Quaternion.LookRotation(target - GetPosition());
				world.CreateBattleVFX((marchInfo.monsterSpecialType == 40) ? "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormAttackVFX.prefab" : "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormAttackVFX.prefab", 3f, delegate(GameObject go)
				{
					go.transform.position = GetPosition();
					go.transform.rotation = anim.transform.localRotation;
				});
			}
		}
	}

	public float PlayDefendAnim(string animName, Vector3 attackDir, float attackDuration)
	{
		if (timelinePlayer != null)
		{
			return PlayDefendTimeLine(animName, attackDir);
		}
		if (anims == null || anims.Length == 0)
		{
			return 0f;
		}
		float num = 0f;
		for (int i = 0; i < anims.Length; i++)
		{
			SimpleAnimation anim = anims[i];
			if (anim == null || anim.transform == null)
			{
				continue;
			}
			SimpleAnimation.State state = anim.GetState(animName);
			if (state == null)
			{
				continue;
			}
			if (animName == "attack")
			{
				anim.transform.rotation = Quaternion.LookRotation(-attackDir);
				if (marchInfo.IsAisila())
				{
					return attackDuration;
				}
				if (i == 0 && marchInfo.IsBoss())
				{
					SetAttackEffectActive(isActive: true);
					YieldUtils.DelayActionWithOutContext(delegate
					{
						SetAttackEffectActive(isActive: false);
						if ((IsWorldBossTroop() || marchInfo.IsAisila()) && anim != null && anim.transform != null)
						{
							anim.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
						}
					}, state.length);
					if (marchInfo.IsSandWorm())
					{
						world.CreateBattleVFX((marchInfo.monsterSpecialType == 40) ? "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormAttackVFX.prefab" : "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormAttackVFX.prefab", 3f, delegate(GameObject go)
						{
							go.transform.position = GetPosition();
							go.transform.rotation = anim.transform.localRotation;
						});
					}
					if (IsWorldBossTroop() || marchInfo.IsAisila())
					{
						if (IsS1SeasonPreBoss())
						{
							TryShowWorldBossTipByText(GetS1SeasonPreBossSpeak(2), state.length);
						}
						else
						{
							TryShowWorldBossTip(state.length);
						}
					}
				}
			}
			anim.Rewind(animName);
			anim.Play(animName);
			int num2 = Mathf.CeilToInt(attackDuration / state.length);
			for (int num3 = 1; num3 < num2; num3++)
			{
				anim.PlayQueued(animName);
			}
			float b = (float)num2 * state.length;
			num = Mathf.Max(num, b);
			if (marchInfo.IsFlowerCar())
			{
				break;
			}
		}
		if (animName == "attack" && marchInfo.IsCityBoss && world != null && SceneManager.World.GetLodLevel() < 3)
		{
			string prefabPath = "Assets/Main/SeasonRes/S4/Prefabs/Effect/S4_chengshiBoss/eff_Boss_S4wushi_atk.prefab";
			world.CreateBattleVFX(prefabPath, 3f, delegate(GameObject go)
			{
				if (transform != null)
				{
					go.transform.SetParent(transform);
					go.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			});
		}
		return num;
	}

	private float PlayDefendTimeLine(string animName, Vector3 attackDir)
	{
		if (timelinePlayer == null)
		{
			return 0f;
		}
		timelinePlayer.transform.rotation = Quaternion.LookRotation(-attackDir);
		return (float)timelinePlayer.PlayTimeline(animName, toIdle: true);
	}

	public void CleanAllQueuedStates()
	{
		if (anims != null)
		{
			SimpleAnimation[] array = anims;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ForceCleanAllQueuedStates();
			}
		}
	}

	public void TryPlay(string animName, Vector3 target)
	{
		if (marchInfo.IsAisila() || marchInfo.IsDrillBaseNewBossHugeSandWorm() || !IsMonsterTroop() || anims == null || anims.Length == 0)
		{
			return;
		}
		for (int i = 0; i < anims.Length; i++)
		{
			SimpleAnimation simpleAnimation = anims[i];
			if (!(simpleAnimation != null) || !(simpleAnimation.transform != null))
			{
				continue;
			}
			SimpleAnimation.State state = simpleAnimation.GetState(animName);
			if (state == null)
			{
				continue;
			}
			simpleAnimation.Rewind(animName);
			simpleAnimation.Play(animName);
			if (!marchInfo.IsWanderBoss())
			{
				simpleAnimation.transform.rotation = Quaternion.LookRotation(target - GetPosition());
			}
			if (marchInfo.IsBoss() && animName == "attack")
			{
				SetAttackEffectActive(isActive: true);
				YieldUtils.DelayActionWithOutContext(delegate
				{
					SetAttackEffectActive(isActive: false);
				}, state.length);
			}
		}
	}

	public List<Vector3> GetDefenderPositionList()
	{
		if (IsMarchTargetAttack())
		{
			WorldTroop targetTroop = GetTargetTroop();
			if (targetTroop != null && targetTroop.anims != null && targetTroop.anims.Length != 0)
			{
				List<Vector3> list = new List<Vector3>();
				for (int i = 0; i < targetTroop.anims.Length; i++)
				{
					SimpleAnimation simpleAnimation = targetTroop.anims[i];
					if (simpleAnimation != null && simpleAnimation.transform != null)
					{
						list.Add(simpleAnimation.transform.position);
					}
				}
				return list;
			}
		}
		return null;
	}

	public Vector3 GetDefenderPosition()
	{
		MarchTargetType marchTargetType = GetMarchTargetType();
		long targetUuid = defAtkUuid;
		if (targetUuid == 0L)
		{
			targetUuid = marchInfo.targetUuid;
		}
		switch (marchTargetType)
		{
		case MarchTargetType.ATTACK_MONSTER:
		case MarchTargetType.ATTACK_ARMY:
		case MarchTargetType.RALLY_FOR_BOSS:
		case MarchTargetType.ATTACK_ARMY_COLLECT:
		case MarchTargetType.DIRECT_ATTACK_ACT_BOSS:
		{
			WorldTroop troop2 = world.GetTroop(targetUuid);
			if (troop2 != null)
			{
				return troop2.GetPosition();
			}
			WorldMarch march2 = world.GetMarch(targetUuid);
			if (march2 != null)
			{
				return march2.position;
			}
			break;
		}
		case MarchTargetType.STATE:
		{
			WorldTroop troop = world.GetTroop(defAtkUuid);
			if (troop != null)
			{
				return troop.GetPosition();
			}
			WorldMarch march = world.GetMarch(targetUuid);
			if (march != null)
			{
				return march.position;
			}
			if (world.GetObjectByUuid(targetUuid) is WorldBuildObjectNew worldBuildObjectNew2)
			{
				return worldBuildObjectNew2.GetPosition();
			}
			PointInfo pointInfoByUuid4 = world.GetPointInfoByUuid(targetUuid);
			if (pointInfoByUuid4 != null)
			{
				return world.TileIndexToWorld(pointInfoByUuid4.mainIndex);
			}
			break;
		}
		case MarchTargetType.ATTACK_BUILDING:
		case MarchTargetType.RALLY_FOR_BUILDING:
		case MarchTargetType.ATTACK_CITY:
		case MarchTargetType.ATTACK_ROAD:
		case MarchTargetType.RALLY_THRONE:
		case MarchTargetType.ATTACK_THRONE:
		case MarchTargetType.ASSISTANCE_THRONE:
		case MarchTargetType.RALLY_DRAGON_BUILDING:
		case MarchTargetType.ATTACK_WINTER_STORM_CITY:
		case MarchTargetType.RALLY_EPIDEMIC_BUILDING:
		case MarchTargetType.ATTACK_EPIDEMIC_CITY:
			if (world.GetObjectByUuid(targetUuid) is WorldBuildObjectNew worldBuildObjectNew)
			{
				return worldBuildObjectNew.GetPosition();
			}
			if (world.GetObjectByUuid(targetUuid) is WorldBoardObject)
			{
				PointInfo pointInfoByUuid3 = world.GetPointInfoByUuid(targetUuid);
				if (pointInfoByUuid3 != null)
				{
					return world.TileIndexToWorld(pointInfoByUuid3.mainIndex);
				}
			}
			break;
		default:
			if (marchInfo.target == MarchTargetType.EXPLORE || marchInfo.target == MarchTargetType.SAMPLE || marchInfo.target == MarchTargetType.PICK_GARBAGE)
			{
				PointInfo pointInfoByUuid = world.GetPointInfoByUuid(targetUuid);
				if (pointInfoByUuid != null)
				{
					return SceneManager.World.TileIndexToWorld(pointInfoByUuid.pointIndex);
				}
			}
			else if (marchInfo.target == MarchTargetType.ATTACK_ALLIANCE_CITY || marchInfo.target == MarchTargetType.ATTACK_THRONE || marchInfo.target == MarchTargetType.RALLY_THRONE || marchInfo.target == MarchTargetType.RALLY_DRAGON_BUILDING || marchInfo.target == MarchTargetType.RALLY_EPIDEMIC_BUILDING || marchInfo.target == MarchTargetType.RALLY_FOR_ALLIANCE_CITY)
			{
				PointInfo pointInfoByUuid2 = world.GetPointInfoByUuid(targetUuid);
				if (pointInfoByUuid2 != null)
				{
					return SceneManager.World.TileIndexToWorld(pointInfoByUuid2.mainIndex) + new Vector3(-6f, 0f, -6f);
				}
			}
			else if (marchInfo.target == MarchTargetType.GOLLOES_EXPLORE)
			{
				return SceneManager.World.TileIndexToWorld((int)marchInfo.targetUuid);
			}
			break;
		}
		if (marchInfo.IsMonsterOrOrdinaryBoss())
		{
			WorldTroop troop3 = world.GetTroop(targetUuid);
			if (troop3 != null)
			{
				return troop3.GetPosition();
			}
		}
		else
		{
			if (IsWorldBossTroop())
			{
				return SceneManager.World.TileIndexToWorld(marchInfo.targetPos);
			}
			if (marchInfo.target == MarchTargetType.ATTACK_DESERT)
			{
				return SceneManager.World.TileIndexToWorld(marchInfo.targetUuid.ToInt());
			}
		}
		return SceneManager.World.TileIndexToWorld(marchInfo.targetPos);
	}

	public void TroopUnitPickSuccess()
	{
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.PickGarbageSuccess();
		}
	}

	public void TroopUnitPickBack()
	{
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.PickMoveBack();
		}
		world.HideTroopDestination(GetMarchUUID());
	}

	public void BackTroopUnits()
	{
		DelShield();
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.Back();
		}
		troopUnits.Clear();
	}

	public void ClearTroopUnits()
	{
		troopUnits.ForEach(delegate(WorldTroopUnit i)
		{
			i.Destroy();
		});
		troopUnits.Clear();
	}

	public void TroopUnitsBirthThenPickGarbage(bool appear = true)
	{
		if (troopUnits.Count > 0 || !IsPickGarbageTroop())
		{
			return;
		}
		List<Vector3> list = new List<Vector3>();
		list = ((marchInfo.type == NewMarchType.GOLLOES_EXPLORE) ? GetPickPoint((int)marchInfo.targetUuid, GetPosition()) : ((!(world.GetObjectByUuid(marchInfo.targetUuid) is WorldDetectEventItemObject worldDetectEventItemObject)) ? GetPickPoint(marchInfo.targetPos, GetPosition()) : worldDetectEventItemObject.GetPickPoint(GetPosition())));
		int num = GarbageBirthPos.Length;
		for (int i = 0; i < num; i++)
		{
			Vector3 vector = GarbageBirthPos[i];
			Vector3 birthDest = GetPosition() + model.transform.right * vector.x + model.transform.forward * vector.z;
			Vector3 pickDest = GetPosition();
			if (list.Count > 0)
			{
				if (list.Count > i)
				{
					pickDest = list[i];
				}
				else
				{
					pickDest = list[list.Count - 1];
				}
			}
			PickGarbageTroopUnit entity = new PickGarbageTroopUnit(WorldTroopUnit.UnitType.Junkman, this);
			entity.CreateInstance(delegate
			{
				entity.SetPosition(GetPosition() + new Vector3(0f, 1.6f, 0f));
				if (appear)
				{
					entity.BirthThenMoveToGarbage(birthDest, pickDest, GetDefenderPosition());
				}
				else
				{
					entity.BirthThenPickGarbage(birthDest, pickDest, GetDefenderPosition());
				}
			}, model.transform);
			troopUnits.Add(entity);
		}
	}

	public List<Vector3> GetPickPoint(int endP, Vector3 startPt)
	{
		List<Vector3> list = new List<Vector3>();
		if (endP > 0)
		{
			MarchTargetType marchTargetType = GetMarchTargetType();
			Vector3 vector = SceneManager.World.TileIndexToWorld(endP);
			float num = 1.5f;
			int i = 0;
			int num2 = 5;
			float num3 = 360 / num2;
			double num4 = Math.Atan2(x: startPt.x - vector.x, y: startPt.z - vector.z) * 180.0 / Math.PI;
			if (marchTargetType == MarchTargetType.SEASON_FARMER_SEND_RES)
			{
				num = 4f;
			}
			for (; i < num2; i++)
			{
				double num5 = (double)(num3 * (float)i) + num4;
				double num6 = (double)num * Math.Cos(num5 * Math.PI / 180.0);
				double num7 = (double)num * Math.Sin(num5 * Math.PI / 180.0);
				Vector3 item = vector + new Vector3((float)num6, 0f, (float)num7);
				list.Add(item);
			}
		}
		return list;
	}

	public bool IsPickGarbageTroop()
	{
		MarchTargetType marchTargetType = GetMarchTargetType();
		if (marchTargetType != MarchTargetType.PICK_GARBAGE && marchTargetType != MarchTargetType.SAMPLE)
		{
			return marchTargetType == MarchTargetType.SEASON_FARMER_SEND_RES;
		}
		return true;
	}

	public bool IsGolloesExplore()
	{
		return marchInfo.type == NewMarchType.GOLLOES_EXPLORE;
	}

	public bool IsExplore()
	{
		return marchInfo.type == NewMarchType.EXPLORE;
	}

	public bool IsScoutTroop()
	{
		if (marchInfo.type != NewMarchType.SCOUT && marchInfo.type != NewMarchType.CROSS_SCOUT && marchInfo.type != NewMarchType.TREAT_VIRUS && marchInfo.type != NewMarchType.ZONE_MOBILIZATION_DONATE)
		{
			return marchInfo.type == NewMarchType.MONSTER_CHALLENGE_DONATE;
		}
		return true;
	}

	public bool IsDetectRescue()
	{
		return GetMarchTargetType() == MarchTargetType.RESCUE_FAKE_MARCH;
	}

	public void ReSetEntityTarget()
	{
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.target = GetTargetTroop();
		}
	}

	public void PlayAttackEffect()
	{
		foreach (WorldTroopUnit troopUnit in troopUnits)
		{
			troopUnit.PlayAttackEffect();
		}
	}

	public void LookAtTarget()
	{
		if (IsMonsterTroop())
		{
			SetRotation(Quaternion.LookRotation(GetDefenderPosition() - GetPosition()));
		}
	}

	public void SetRotationRoot()
	{
		if (!(rotationRoot != null) || IsMonsterTroop() || IsWorldBossTroop())
		{
			return;
		}
		Quaternion rot = Quaternion.LookRotation(GetDefenderPosition() - GetPosition());
		int pointId = world.WorldToTileIndex(GetPosition());
		int curPosAndRotationTroopNum = world.GetCurPosAndRotationTroopNum(marchInfo.uuid, pointId, rot);
		if (curPosAndRotationTroopNum != -1)
		{
			Quaternion quaternion = Quaternion.Euler(rot.eulerAngles + new Vector3(0f, 20 * curPosAndRotationTroopNum, 0f));
			if (!rotationRoot.transform.rotation.Equals(quaternion))
			{
				rotationRoot.transform.rotation = quaternion;
			}
		}
	}

	public void TroopUnitsBirthThenAttack()
	{
	}

	public void AddShield()
	{
		if (IsAddShield || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		IsAddShield = true;
		shieldRequest = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Effect/World/VFX_Shield.prefab");
		shieldRequest.completed += delegate
		{
			if (model != null && model.transform != null)
			{
				shieldRequest.gameObject.transform.SetParent(model.transform);
				shieldRequest.gameObject.transform.localPosition = Vector3.zero;
				shieldRequest.gameObject.transform.localRotation = Quaternion.identity;
				shieldRequest.gameObject.transform.localScale = Vector3.one;
			}
			else
			{
				shieldRequest.Destroy();
			}
		};
	}

	public void DelShield()
	{
		if (!IsAddShield)
		{
			return;
		}
		IsAddShield = false;
		if (shieldRequest == null)
		{
			return;
		}
		if (shieldRequest.gameObject != null)
		{
			Animator component = shieldRequest.gameObject.GetComponent<Animator>();
			if (component != null)
			{
				component.SetTrigger("close");
			}
			YieldUtils.DelayActionWithOutContext(delegate
			{
				if (shieldRequest != null)
				{
					shieldRequest.Destroy();
				}
			}, 0.6f);
		}
		else
		{
			shieldRequest.Destroy();
		}
	}

	public void ShowRallyMarchAttack()
	{
		if (fdInst != null && fdInst.gameObject != null)
		{
			simpleAni = fdInst.gameObject.GetComponentInChildren<SimpleAnimation>();
			simpleAni.Play("attack");
			simpleAni.PlayQueued("run");
		}
	}

	public void ShowAttack()
	{
		if (adjuster != null && !adjuster.IsMainShow())
		{
			return;
		}
		InstanceRequest requestInst = GameEntry.Resource.InstantiateAsync("Assets/_Art/Effect/prefab/scene/VFX_putonggongji.prefab");
		normalAttackInstList.Add(requestInst);
		requestInst.completed += delegate
		{
			if (model != null && model.transform != null)
			{
				Transform transform = model.transform.Find("A_vehicle_ybc_prefab/root");
				if (transform != null)
				{
					requestInst.gameObject.transform.SetParent(transform.transform);
				}
				else
				{
					requestInst.gameObject.transform.SetParent(model.transform);
				}
				requestInst.gameObject.transform.localPosition = Vector3.zero;
				requestInst.gameObject.transform.localRotation = Quaternion.Euler(0f, -90f, 0f);
				GameEntry.Sound.PlayEffect("effect_attack");
				YieldUtils.DelayActionWithOutContext(delegate
				{
					if (requestInst != null)
					{
						requestInst.Destroy();
					}
				}, 1f);
			}
			else
			{
				requestInst.Destroy();
			}
		};
	}

	public void RemoveAttack()
	{
		if (normalAttackInstList == null)
		{
			return;
		}
		foreach (InstanceRequest normalAttackInst in normalAttackInstList)
		{
			normalAttackInst?.Destroy();
		}
		normalAttackInstList.Clear();
	}

	public void DoSkill(int useSkillID, DamageType damageType, int heroId, Dictionary<long, List<string>> useSkillList, long useSkillUid)
	{
		if (adjuster != null && !adjuster.IsMainShow())
		{
			return;
		}
		string effectPath = GameEntry.ConfigCache.GetTemplateData("skill", useSkillID, "effect_path");
		InstanceRequest requestInst = GameEntry.Resource.InstantiateAsync(effectPath);
		skillInstList.Add(requestInst);
		requestInst.completed += delegate
		{
			if (model != null && this.transform != null)
			{
				Transform transform = model.transform.Find("A_vehicle_ybc_prefab/root");
				if (transform != null)
				{
					requestInst.gameObject.transform.SetParent(transform.transform);
				}
				else
				{
					requestInst.gameObject.transform.SetParent(model.transform);
				}
				requestInst.gameObject.transform.localPosition = Vector3.zero;
				requestInst.gameObject.transform.localRotation = Quaternion.identity;
				requestInst.gameObject.transform.localScale = Vector3.one;
				if (effectPath.IndexOf("VFX_jiangjun_attack", StringComparison.Ordinal) >= 0)
				{
					GameEntry.Sound.PlayEffect("effect_skill");
				}
				if (effectPath.IndexOf("VFX_Nvniuzai_attack") >= 0)
				{
					LineRenderer componentInChildren = requestInst.gameObject.transform.GetComponentInChildren<LineRenderer>();
					if (componentInChildren != null)
					{
						componentInChildren.SetPosition(0, componentInChildren.transform.position);
						WorldTroop targetTroop = GetTargetTroop();
						if (targetTroop == null)
						{
							requestInst.Destroy();
							return;
						}
						componentInChildren.SetPosition(1, new Vector3(targetTroop.transform.position.x, componentInChildren.transform.position.y, targetTroop.transform.position.z));
					}
				}
				YieldUtils.DelayActionWithOutContext(delegate
				{
					if (requestInst != null)
					{
						requestInst.Destroy();
					}
				}, 2f);
			}
			else
			{
				requestInst.Destroy();
			}
		};
	}

	public void TroopUnitsAttack(bool isBirth = false)
	{
	}

	public void OnDrawGizmos()
	{
		if (transform != null && model != null)
		{
			Vector3 position = transform.position;
			if (GetMarchStatus() == MarchStatus.MOVING)
			{
				Gizmos.color = Color.green;
			}
			else if (GetMarchStatus() == MarchStatus.CHASING)
			{
				Gizmos.color = Color.gray;
			}
			else if (GetMarchStatus() == MarchStatus.STATION)
			{
				Gizmos.color = Color.blue;
			}
			else if (GetMarchStatus() == MarchStatus.BACK_HOME)
			{
				Gizmos.color = Color.magenta;
			}
			else if (GetMarchStatus() == MarchStatus.ATTACKING)
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = Color.white;
			}
			Gizmos.DrawCube(position, Vector3.one);
			if (sm.CurrentStateType == WorldTroopState.Move)
			{
				Gizmos.color = Color.green;
			}
			else if (sm.CurrentStateType == WorldTroopState.Idle)
			{
				Gizmos.color = Color.blue;
			}
			else if (sm.CurrentStateType == WorldTroopState.None)
			{
				Gizmos.color = Color.magenta;
			}
			else if (sm.CurrentStateType == WorldTroopState.Attack)
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = Color.white;
			}
			Gizmos.DrawSphere(position + Vector3.up, 0.5f);
			Gizmos.DrawLine(position, position + model.transform.forward * 2f);
		}
	}

	public BoundingSphere GetBoundingSphere()
	{
		return boundingSphere;
	}

	public void OnCullingStateVisible(bool visible)
	{
		cullVisible = visible;
	}

	public float GetHeight()
	{
		if (_modelHeight != null)
		{
			return _modelHeight.GetHeight();
		}
		return 0f;
	}

	public void ChangeFsmState(WorldTroopState stateType)
	{
		sm.ChangeState(stateType);
	}

	public void PlayHitEffect(string effectPath)
	{
	}

	public void ClearHitEffect()
	{
	}

	public void HideJunkMan()
	{
		if (model != null)
		{
			Transform transform = model.transform.Find("A_soldie@xiaoren_skin");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
			transform = model.transform.Find("A_build_zhenchaji");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
		}
		if (this.transform != null)
		{
			Transform transform2 = this.transform.Find("WorldTroopName(Clone)");
			if (transform2 == null)
			{
				transform2 = this.transform.Find("WorldTroopName");
			}
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(value: false);
			}
		}
	}

	public void ShowJunkMan()
	{
		if (model != null)
		{
			Transform transform = model.transform.Find("A_soldie@xiaoren_skin");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: true);
			}
			transform = model.transform.Find("A_build_zhenchaji");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: true);
			}
		}
		if (this.transform != null)
		{
			Transform transform2 = this.transform.Find("WorldTroopName(Clone)");
			if (transform2 == null)
			{
				transform2 = this.transform.Find("WorldTroopName");
			}
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(value: true);
			}
		}
	}

	public void SetVisible(bool active)
	{
		if (_visible != active)
		{
			_visible = active;
			if (transform != null)
			{
				transform.gameObject.SetActive(_visible);
			}
		}
	}

	public void AttackOnce(Vector3 targetPos)
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadAttack", marchInfo.uuid, targetPos, 0);
	}

	public void AttackOnceWithIndex(Vector3 targetPos, int index)
	{
		GameEntry.Lua.Call("CSharpCallLuaInterface.WorldSquadAttack", marchInfo.uuid, targetPos, index);
	}

	private string ChangeModelNameByActivity(string id)
	{
		string text = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetActivityDropMonsterModel", id);
		if (text == null)
		{
			return string.Empty;
		}
		string text2 = $"Assets/Main/Prefabs/Monsters/{text}.prefab";
		if (GameEntry.Resource.HasAsset(text2))
		{
			return text2;
		}
		return string.Empty;
	}

	private void SetHpBar(float preHpRatio = 0f, bool ani = false)
	{
		if (oriHpBarWidth == 0f)
		{
			return;
		}
		if (ani)
		{
			if (_hpAnimation != null)
			{
				_hpAnimation.Kill();
			}
			_hpAnimation = DOTween.To(() => preHpRatio, delegate(float b)
			{
				if (!isDestroy)
				{
					float num3 = (preHpRatio - b) / (preHpRatio - marchInfo.monsterHpRatio);
					float num4 = 0f;
					if (num3 < 0.5f)
					{
						num4 = 0.5f * (num3 / 0.5f) + 1f;
						hpText.transform.localScale = new Vector3(num4, num4, num4);
					}
					else if (num3 < 1f)
					{
						num4 = 1.5f - 0.5f * ((num3 - 0.5f) / 0.5f);
						hpText.transform.localScale = new Vector3(num4, num4, num4);
					}
					float num5 = b / 100f;
					float num6 = oriHpBarWidth * num5;
					float num7 = (oriHpBarWidth - num6) / 2f;
					hpSliderRender.size = new Vector2(num6, hpSliderRender.size.y);
					hpSliderRender.transform.localPosition = new Vector3(0f - num7, 0f, 0f);
					hpText.text = b.ToString("F2") + "%";
				}
			}, marchInfo.monsterHpRatio, 1f).OnComplete(delegate
			{
				_hpAnimation = null;
				if (!isDestroy)
				{
					float value2 = marchInfo.monsterHpRatio / 100f;
					hpText.transform.localScale = Vector3.one;
					value2 = Mathf.Clamp(value2, 0f, 1f);
					float num3 = oriHpBarWidth * value2;
					float num4 = (oriHpBarWidth - num3) / 2f;
					hpSliderRender.size = new Vector2(num3, hpSliderRender.size.y);
					hpSliderRender.transform.localPosition = new Vector3(0f - num4, 0f, 0f);
					hpText.text = marchInfo.monsterHpRatio.ToString("F2") + "%";
				}
			});
		}
		else
		{
			float value = marchInfo.monsterHpRatio / 100f;
			value = Mathf.Clamp(value, 0f, 1f);
			float num = oriHpBarWidth * value;
			float num2 = (oriHpBarWidth - num) / 2f;
			hpSliderRender.size = new Vector2(num, hpSliderRender.size.y);
			hpSliderRender.transform.localPosition = new Vector3(0f - num2, 0f, 0f);
			hpText.text = marchInfo.monsterHpRatio.ToString("F2") + "%";
			if (marchInfo.type == NewMarchType.RUNNING_BOSS)
			{
				SetRunningBossScale(marchInfo.monsterHpRatio);
			}
		}
	}

	private void SetRunningBossScale(float per)
	{
		if (requestInst == null)
		{
			return;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "special");
		string s = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "running_boss", "k15");
		if (int.Parse(templateData) != 32)
		{
			return;
		}
		Transform transform = requestInst.gameObject.transform.Find("Model/WorldModel/Eff_s_WorldMonster_Boss_crazy");
		if (!float.TryParse(GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetConfigStr", "running_boss", "k10"), out var result))
		{
			result = 1.3f;
		}
		if (!float.TryParse(s, out var result2))
		{
			result2 = 0.2f;
		}
		if (per / 100f <= result2)
		{
			requestInst.gameObject.transform.localScale = new Vector3(result, result, result);
			if (transform != null)
			{
				transform.gameObject.SetActive(value: true);
			}
		}
		else
		{
			requestInst.gameObject.transform.localScale = Vector3.one;
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
		}
	}

	private void ResetHpBar()
	{
		if (oriHpBarWidth != 0f)
		{
			Transform transform = this.transform.Find("HPBar");
			if (transform != null)
			{
				transform.gameObject.SetActive(value: false);
			}
			hpSliderRender.size = new Vector2(oriHpBarWidth, hpSliderRender.size.y);
			hpSliderRender.transform.localPosition = Vector3.zero;
			hpText.transform.localScale = Vector3.one;
			oriHpBarWidth = 0f;
		}
		hpSliderRender = null;
		hpText = null;
		hpRoot = null;
	}

	public void OnMoveStateEnd()
	{
		if (marchInfo.IsWanderBoss() || marchInfo.type == NewMarchType.ZONE_MOBILIZATION_BOSS)
		{
			world.DestroyTroopLine(marchInfo.uuid);
		}
	}

	public void SetLookAt(Vector3 target, bool ignoreState)
	{
		if ((ignoreState || (sm != null && sm.CurrentStateType == WorldTroopState.Idle)) && model != null)
		{
			model.transform.LookAt(target);
			GameEntry.Event.Fire(EventId.TroopRotation, marchInfo.uuid);
		}
	}

	public void RefreshPosition()
	{
		if (sm != null && sm.CurrentStateType == WorldTroopState.Move)
		{
			sm.GetState(WorldTroopState.Move).RefreshPosition();
		}
	}

	public void OnMonsterIceBroken()
	{
		if (sm != null && sm.CurrentStateType == WorldTroopState.Idle)
		{
			sm.GetState(WorldTroopState.Idle).OnMonsterIceBroken();
		}
	}

	public bool IsS1SeasonPreBoss()
	{
		if (marchInfo.type == NewMarchType.ACT_BOSS)
		{
			return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsS1SeasonPreBoss", marchInfo.monsterId);
		}
		return false;
	}

	public bool IsWeakS1SeasonPreBoss()
	{
		return marchInfo.actBossState == 1;
	}

	private string GetS1SeasonPreBossSpeak(int speakType)
	{
		string[] array = GameEntry.Lua.CallWithReturn<string[], int, int>("CSharpCallLuaInterface.GetS1SeasonPreBossSpeakByType", GetMarchInfo().monsterId, speakType);
		if (array == null || array.Length == 0)
		{
			return string.Empty;
		}
		int num = new System.Random().Next(array.Length);
		return array[num];
	}

	public float TryGetAnimStateTimeLength(string animName)
	{
		if (anims == null)
		{
			return 0f;
		}
		for (int i = 0; i < anims.Length; i++)
		{
			SimpleAnimation simpleAnimation = anims[i];
			if (!(simpleAnimation == null) && !(simpleAnimation.transform == null))
			{
				return simpleAnimation.GetState(animName).length;
			}
		}
		return 0f;
	}

	public bool IsCanPlayAni()
	{
		if (marchInfo.IsWanderBoss())
		{
			if (!isBattle)
			{
				return sm.GetCurrentState() == WorldTroopState.Idle;
			}
			return true;
		}
		return !marchInfo.IsFrozen();
	}

	private bool SetRunningBossRotation()
	{
		int num = GameEntry.Lua.CallWithReturnInt("CSharpCallLuaInterface.GetTroopTargetPointId", marchInfo.uuid);
		if (num > 0)
		{
			SetLookAt(SceneManager.World.TileIndexToWorld(num), ignoreState: true);
			return true;
		}
		return false;
	}

	public void UpdateWhenLodChange(int curLod)
	{
		UpdateSoundByLod(curLod);
		ShowBattleCardStatusEff();
	}

	public void UpdateSoundByLod(int curLod)
	{
		foreach (KeyValuePair<ETroopSoundType, WorldTroopSoundBase> item in _worldTroopSoundMap)
		{
			item.Value.UpdateLOD(curLod);
		}
		UpdateSound();
	}

	public void UpdateSound()
	{
		WorldTroopState currentStateType = sm.CurrentStateType;
		foreach (KeyValuePair<ETroopSoundType, WorldTroopSoundBase> item in _worldTroopSoundMap)
		{
			item.Value.UpdateSound(currentStateType);
		}
	}

	public void SetStunVFX(bool active)
	{
		if (active)
		{
			if (stunVFX == 0)
			{
				stunVFX = world.CreateVFX((marchInfo.monsterSpecialType == 40) ? "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormStunVFX.prefab" : "Assets/Main/Prefabs/Effect/World/Sandworm/BigSandwormStunVFX.prefab", Vector3.zero, 600f, 0f, delegate(GameObject go)
				{
					go.transform.SetParent(model.transform);
					go.transform.localPosition = Vector3.zero;
				});
			}
		}
		else if (stunVFX > 0)
		{
			world.RemoveVFX(stunVFX);
			stunVFX = 0;
		}
	}

	private Vector3 GetS4WanderBossWorldPosition()
	{
		if (model != null)
		{
			Transform transform = model.transform.Find("A_Monster_Group_S4_1/A_Monster_Boss_S4_tiangou (1)");
			if (transform != null)
			{
				return transform.position;
			}
		}
		return GetPosition();
	}

	private void SetRebuildInfo(Transform trans)
	{
		if (marchInfo != null && trans != null)
		{
			marchInfo.ownerName = marchInfo.userInfo.TryGetString("name");
			marchInfo.pic = marchInfo.userInfo.TryGetString("pic");
			marchInfo.allianceAbbr = marchInfo.userInfo.TryGetString("abbr");
			marchInfo.headSkinId = marchInfo.userInfo.TryGetInt("headSkinId");
			marchInfo.headSkinET = marchInfo.userInfo.TryGetLong("headSkinET");
			marchInfo.picVer = marchInfo.userInfo.TryGetInt("picVer");
		}
	}

	private void RefreshBerserkBossState(float preHpRatio = 0f, bool hpAni = false)
	{
		if (marchInfo.type != NewMarchType.ACT_BERSERK_BOSS)
		{
			return;
		}
		if (berserkBossTombRoot != null)
		{
			berserkBossTombRoot.SetActive(marchInfo.status == MarchStatus.BERSERK_BOSS_WAITING);
		}
		if (modelTextRoot != null)
		{
			modelTextRoot.gameObject.SetActive(marchInfo.status == MarchStatus.BERSERK_BOSS_WAITING);
		}
		if (berserkBossNormalRoot != null)
		{
			berserkBossNormalRoot.SetActive(marchInfo.status == MarchStatus.ATTACKING);
		}
		if (marchInfo.status == MarchStatus.BERSERK_BOSS_WAITING)
		{
			RefreshBerserkBossCutDownTime();
			if (berserkBossRewardRoot != null)
			{
				berserkBossRewardRoot.SetActive(value: false);
			}
			if (hpRoot != null)
			{
				hpRoot.gameObject.SetActive(value: false);
			}
		}
		else if (marchInfo.status == MarchStatus.ATTACKING)
		{
			if (Math.Abs(preHpRatio - marchInfo.monsterHpRatio) > 0.01f)
			{
				SetHpBar(preHpRatio, hpAni);
			}
			bool isAlreadyBerserkBossReward = GameEntry.Data.Player.GetIsAlreadyBerserkBossReward(marchInfo.uuid);
			if (hpRoot != null)
			{
				hpRoot.gameObject.SetActive(marchInfo.status == MarchStatus.ATTACKING && marchInfo.curHp > 0);
			}
			if (berserkBossRewardRoot != null)
			{
				berserkBossRewardRoot.SetActive(marchInfo.status == MarchStatus.ATTACKING && marchInfo.curHp <= 0 && !isAlreadyBerserkBossReward);
			}
			RefreshBerserkBossAttackEffectShow();
		}
	}

	private void RefreshBerserkBossCutDownTime()
	{
		if (modelTexts != null && modelTexts.Length == 2)
		{
			modelTexts[0].text = GameEntry.Localization.GetString("activity_berserkboss_state_01");
			long num = marchInfo.endTime - GameEntry.Timer.GetServerTime();
			if (num <= 0)
			{
				num = 0L;
			}
			modelTexts[1].text = GameEntry.Timer.MilliSecondToFmtString(num);
		}
	}

	private void CheckSandWormEscape(long now)
	{
		long num = marchInfo.sandWormData.expireTime - 2000;
		if (num - 1000 < now && now < num)
		{
			PlayAnim("escape");
			world.CreateVFX((marchInfo.monsterSpecialType == 40) ? "Assets/Main/SeasonRes/S3/Prefabs/Effect/HugeSandwormEscapeVFX.prefab" : "Assets/Main/SeasonRes/S3/Prefabs/Effect/BigSandwormEscapeVFX.prefab", GetPosition(), 4f);
		}
	}

	private void RefreshBerserkBossAttackEffectShow()
	{
		if (marchInfo.type != NewMarchType.ACT_BERSERK_BOSS || (adjuster != null && !adjuster.IsMainShow()) || marchInfo.status != MarchStatus.ATTACKING)
		{
			return;
		}
		if (marchInfo.curHp > 0)
		{
			if (berserkBossAttackEffectInst == null)
			{
				berserkBossAttackEffectInst = GameEntry.Resource.InstantiateAsync("Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_crazy.prefab");
				berserkBossAttackEffectInst.completed += delegate
				{
					if (berserkBossAttackEffectInst.gameObject != null)
					{
						GameObject gameObject = berserkBossAttackEffectInst.gameObject;
						gameObject.SetActive(value: true);
						gameObject.transform.SetParent(berserkBossNormalRoot.transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localScale = Vector3.one;
						gameObject.transform.localRotation = Quaternion.identity;
					}
				};
			}
			else
			{
				InstanceRequest instanceRequest = berserkBossAttackEffectInst;
				if (instanceRequest == null || instanceRequest.gameObject?.activeInHierarchy != true)
				{
					berserkBossAttackEffectInst?.gameObject?.SetActive(value: true);
				}
			}
		}
		else
		{
			InstanceRequest instanceRequest2 = berserkBossAttackEffectInst;
			if (instanceRequest2 != null && instanceRequest2.gameObject?.activeInHierarchy == true)
			{
				berserkBossAttackEffectInst?.gameObject?.SetActive(value: false);
			}
		}
	}

	private void RefreshBerserkBossDeadEffectShow()
	{
		if (marchInfo.type != NewMarchType.ACT_BERSERK_BOSS || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		berserkBossDeadEffectInst = GameEntry.Resource.InstantiateAsync("Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_kill.prefab");
		berserkBossDeadEffectInst.completed += delegate
		{
			if (berserkBossDeadEffectInst.gameObject != null)
			{
				GameObject gameObject = berserkBossDeadEffectInst.gameObject;
				gameObject.SetActive(value: true);
				gameObject.transform.SetParent(berserkBossNormalRoot.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				YieldUtils.DelayActionWithOutContext(delegate
				{
					if (sm != null)
					{
						sm.ChangeState(WorldTroopState.Death);
					}
				}, 0.65f);
			}
		};
	}

	public void ShowZombieRushDefendSuccess()
	{
		WorldPointObject obj = SceneManager.World.GetObjectByUuid(marchInfo.targetUuid);
		if (obj == null)
		{
			return;
		}
		world.CreateBattleVFX("Assets/Main/Prefabs/Effect/World/VFX_VictoryNoUI.prefab", 2.14f, delegate(GameObject go)
		{
			if (obj.GetGameObject().transform != null)
			{
				go.transform.SetParent(obj.GetGameObject().transform);
				go.transform.localPosition = new Vector3(0f, 5f, 0f);
			}
		});
	}

	public void ShowZombieRushDefendFailed()
	{
		WorldPointObject obj = SceneManager.World.GetObjectByUuid(marchInfo.targetUuid);
		if (obj == null)
		{
			return;
		}
		if (_zombieRushEffectInst != null)
		{
			_zombieRushEffectInst.Destroy();
			_zombieRushEffectInst = null;
		}
		world.CreateBattleVFX("Assets/Main/Prefabs/Effect/World/VFX_DefeatNoUI.prefab", 2.14f, delegate(GameObject go)
		{
			if (obj.GetGameObject().transform != null)
			{
				go.transform.SetParent(obj.GetGameObject().transform);
				go.transform.localPosition = new Vector3(0f, 5f, 0f);
			}
		});
		world.CreateBattleVFX("Assets/_Art_LastWar/Effect/Prefab/daditu/Eff_daditu_jid_boom_01.prefab", 2.14f, delegate(GameObject go)
		{
			if (obj.GetGameObject().transform != null)
			{
				go.transform.SetParent(obj.GetGameObject().transform);
				go.transform.localPosition = new Vector3(0f, 0f, 0f);
			}
		});
		YieldUtils.DelayActionWithOutContext(delegate
		{
			world.CreateBattleVFX("Assets/_Art_LastWar/Effect/Prefab/daditu/Eff_daditu_jid_boom_01.prefab", 2.14f, delegate(GameObject go)
			{
				if (obj.GetGameObject().transform != null)
				{
					go.transform.SetParent(obj.GetGameObject().transform);
					go.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
			});
		}, 2f);
		sm.ChangeState(WorldTroopState.None);
		PlayAnim("run");
		Vector3 endValue = GetPosition() + model.transform.forward.normalized * 3f;
		if (_zombieRushDisappearAnimation != null)
		{
			_zombieRushDisappearAnimation.Kill();
		}
		_zombieRushDisappearAnimation = DOTween.To(() => GetPosition(), delegate(Vector3 b)
		{
			if (!isDestroy)
			{
				SetPosition(b);
			}
		}, endValue, 5f).OnComplete(delegate
		{
			_zombieRushDisappearAnimation = null;
			if (!isDestroy)
			{
				transform.gameObject.SetActive(value: false);
			}
		});
	}

	public void ShowBloodyQueenDefendFailed()
	{
		sm.ChangeState(WorldTroopState.None);
		PlayAnim("run");
		Vector3 endValue = GetPosition() + model.transform.forward.normalized * 6f;
		if (_zombieRushDisappearAnimation != null)
		{
			_zombieRushDisappearAnimation.Kill();
		}
		_zombieRushDisappearAnimation = DOTween.To(() => GetPosition(), delegate(Vector3 b)
		{
			if (!isDestroy)
			{
				SetPosition(b);
			}
		}, endValue, 5f).OnComplete(delegate
		{
			_zombieRushDisappearAnimation = null;
			if (!isDestroy)
			{
				transform.gameObject.SetActive(value: false);
			}
		});
	}

	private void ShowZombieRushFightingEffect()
	{
		if (marchInfo.type != NewMarchType.ZOMBIE_RUSH || !SceneManager.MarchDataMgr.IsTargetForMine(marchInfo) || _zombieRushEffectInst != null || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		_zombieRushEffectInst = GameEntry.Resource.InstantiateAsync("Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_kill.prefab");
		_zombieRushEffectInst.completed += delegate
		{
			if (_zombieRushEffectInst.gameObject != null)
			{
				GameObject gameObject = _zombieRushEffectInst.gameObject;
				gameObject.SetActive(value: true);
				gameObject.transform.SetParent(transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one * 0.6f;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		};
	}

	public void ShowZombieDead()
	{
		if (sm == null)
		{
			return;
		}
		sm.ChangeState(WorldTroopState.Death);
		if (!marchInfo.IsZombieRushAltered())
		{
			return;
		}
		world.CreateBattleVFX("Assets/_Art_LastWar/Effect/Prefab/Prefab02/Eff_s_Dabache_boom.prefab", 4f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one * 0.5f;
			}
		});
		world.CreateBattleVFX("Assets/_Art_LastWar/Effect/Prefab/Prefab02/Eff_s_Dabache_death_smoke.prefab", 4f, delegate(GameObject go)
		{
			if (transform != null)
			{
				go.transform.SetParent(transform);
				go.transform.localPosition = Vector3.zero;
				go.transform.localScale = Vector3.one * 0.8f;
			}
		});
	}

	private void ShowBattleCardStatusEff()
	{
		if (!WorldStatusEffectUtils.IsNeedShowStatusEffect())
		{
			foreach (KeyValuePair<int, InstanceRequest> item in StatusEffInstDic)
			{
				if (item.Value.gameObject != null)
				{
					if (item.Value.gameObject.activeSelf)
					{
						item.Value.gameObject.SetActive(value: false);
					}
					continue;
				}
				if (_battleCardWaitRemoveList == null)
				{
					_battleCardWaitRemoveList = new List<int>();
				}
				_battleCardWaitRemoveList.Add(item.Key);
			}
			if (_battleCardWaitRemoveList != null && _battleCardWaitRemoveList.Count > 0)
			{
				for (int i = 0; i < _battleCardWaitRemoveList.Count; i++)
				{
					RemoveStatusEff(_battleCardWaitRemoveList[i]);
				}
				_battleCardWaitRemoveList.Clear();
			}
			foreach (BuffVfx tacticalCardLoopIconVfx in _tacticalCardLoopIconVfxList)
			{
				tacticalCardLoopIconVfx?.Clear();
			}
			_tacticalCardLoopIconVfxList.Clear();
			_tacticalCardLoopIconVfxDic.Clear();
		}
		else
		{
			if (!IsPlayerTroop())
			{
				return;
			}
			List<ArmyInfo> list = marchInfo?.armyInfos;
			if (list == null)
			{
				return;
			}
			for (int j = 0; j < list.Count; j++)
			{
				ArmyInfo armyInfo = list[j];
				if (armyInfo == null)
				{
					continue;
				}
				List<ArmyUnitBuff> unitBuffs = armyInfo.UnitBuffs;
				if (unitBuffs == null)
				{
					continue;
				}
				for (int k = 0; k < unitBuffs.Count; k++)
				{
					ArmyUnitBuff armyUnitBuff = unitBuffs[k];
					int buffId = armyUnitBuff.buffId;
					if (armyUnitBuff.expireTime - GameEntry.Timer.GetServerTime() / 1000 >= 0)
					{
						if (!StatusEffInstDic.ContainsKey(buffId))
						{
							GenOneStatusEff(buffId);
						}
						else
						{
							InstanceRequest instanceRequest = StatusEffInstDic[buffId];
							if (instanceRequest.gameObject != null && !instanceRequest.gameObject.activeSelf)
							{
								instanceRequest.gameObject.SetActive(value: true);
							}
						}
						UpdateLoopIconVfx(buffId, (long)armyUnitBuff.expireTime * 1000L);
					}
					else
					{
						RemoveStatusEff(buffId);
						RemoveLoopIconVfx(buffId);
					}
				}
			}
		}
	}

	private void GenOneStatusEff(int buffId)
	{
		if (StatusEffInstDic.ContainsKey(buffId))
		{
			return;
		}
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", buffId, "buff_effect");
		if (string.IsNullOrEmpty(templateData))
		{
			return;
		}
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(templateData);
		request.completed += delegate
		{
			if (request.isError)
			{
				request.Destroy();
			}
			else
			{
				GameObject gameObject = request.gameObject;
				if (gameObject != null)
				{
					gameObject.transform.SetParent(transform);
					gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
				if (!WorldStatusEffectUtils.IsNeedShowStatusEffect())
				{
					gameObject.SetActive(value: false);
				}
			}
		};
		StatusEffInstDic[buffId] = request;
	}

	private void CheckLoopIconVfx(long curTime)
	{
		int count = _tacticalCardLoopIconVfxList.Count;
		if (count > 0)
		{
			for (int num = count - 1; num >= 0; num--)
			{
				BuffVfx buffVfx = _tacticalCardLoopIconVfxList[num];
				if (curTime > buffVfx.EndTimeStamp)
				{
					_tacticalCardLoopIconVfxList.RemoveAt(num);
					_tacticalCardLoopIconVfxDic.Remove(buffVfx.Path);
					buffVfx.Clear();
				}
			}
		}
		if (_tacticalCardLoopIconVfxList.Count < 1)
		{
			return;
		}
		BuffVfx buffVfx2 = _tacticalCardLoopIconVfxList[0];
		if (buffVfx2.LastPlayTime > 0)
		{
			if (_tacticalCardLoopIconVfxList.Count == 1)
			{
				buffVfx2.SetActive(active: true);
			}
			else if (curTime - buffVfx2.LastPlayTime >= 3000)
			{
				buffVfx2.SetActive(active: false);
				_tacticalCardLoopIconVfxList.RemoveAt(0);
				_tacticalCardLoopIconVfxList.Add(buffVfx2);
				BuffVfx buffVfx3 = _tacticalCardLoopIconVfxList[0];
				buffVfx3.LastPlayTime = curTime;
				buffVfx3.SetActive(active: true);
			}
		}
	}

	private void UpdateLoopIconVfx(int buffId, long endTime)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", buffId, "card_march_icon_effect");
		if (string.IsNullOrEmpty(templateData))
		{
			return;
		}
		if (!_tacticalCardLoopIconVfxDic.TryGetValue(templateData, out var buffVfx))
		{
			buffVfx = new BuffVfx();
			buffVfx.SetPath(templateData);
			InstanceRequest request = GameEntry.Resource.InstantiateAsync(templateData);
			request.completed += delegate
			{
				if (request.isError)
				{
					request.Destroy();
				}
				else
				{
					GameObject gameObject = request.gameObject;
					if (gameObject != null)
					{
						gameObject.transform.SetParent(transform);
						gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					}
					buffVfx.LastPlayTime = GameEntry.Timer.GetServerTime();
					if (_tacticalCardLoopIconVfxDic.ContainsKey(buffVfx.Path))
					{
						_tacticalCardLoopIconVfxList.Add(buffVfx);
					}
				}
			};
			buffVfx.SetInstReqs(request);
			_tacticalCardLoopIconVfxDic.Add(buffVfx.Path, buffVfx);
		}
		buffVfx.AddBuffId(buffId);
		buffVfx.UpdateEndTime(endTime);
		bool visible = WorldStatusEffectUtils.IsNeedShowStatusEffect();
		buffVfx.SetVisible(visible);
	}

	private void RemoveStatusEff(int buffId)
	{
		if (StatusEffInstDic.ContainsKey(buffId))
		{
			InstanceRequest instanceRequest = StatusEffInstDic[buffId];
			if (instanceRequest != null)
			{
				instanceRequest.Destroy();
				StatusEffInstDic.Remove(buffId);
			}
		}
	}

	private void RemoveLoopIconVfx(int buffId)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", buffId, "card_march_icon_effect");
		if (string.IsNullOrEmpty(templateData) || !_tacticalCardLoopIconVfxDic.ContainsKey(templateData) || _tacticalCardLoopIconVfxDic[templateData].RemoveBuffId(buffId) != 0)
		{
			return;
		}
		_tacticalCardLoopIconVfxDic.Remove(templateData);
		for (int num = _tacticalCardLoopIconVfxList.Count - 1; num >= 0; num--)
		{
			if (_tacticalCardLoopIconVfxList[num].Path == templateData)
			{
				_tacticalCardLoopIconVfxList.RemoveAt(num);
				break;
			}
		}
	}

	public void OnPushBloodQueenGunnerAttack()
	{
		TryPlay("attack", GetMarchInfo().bloodyQueenMonster.standWorldPos);
		if (GetMarchStatus() == MarchStatus.WAITING)
		{
			PlayQueued("idle");
		}
	}

	private void RefreshBloodQueenCrazyEffectShow()
	{
		if (marchInfo.type != NewMarchType.BLOODY_QUEEN || (adjuster != null && !adjuster.IsMainShow()))
		{
			return;
		}
		if (marchInfo.bloodyQueenMonster != null && marchInfo.bloodyQueenMonster.state == 1)
		{
			if (bloodQueenCrazyEffect == null)
			{
				bloodQueenCrazyEffect = GameEntry.Resource.InstantiateAsync("Assets/_Art_LastWar/Effect/Prefab/Boss/Eff_s_WorldMonster_Boss_crazy.prefab");
				bloodQueenCrazyEffect.completed += delegate
				{
					if (bloodQueenCrazyEffect.gameObject != null)
					{
						GameObject gameObject = bloodQueenCrazyEffect.gameObject;
						gameObject.SetActive(value: true);
						gameObject.transform.SetParent(transform);
						gameObject.transform.localPosition = Vector3.zero;
						gameObject.transform.localScale = Vector3.one;
						gameObject.transform.localRotation = Quaternion.identity;
					}
				};
			}
			else
			{
				InstanceRequest instanceRequest = bloodQueenCrazyEffect;
				if (instanceRequest == null || instanceRequest.gameObject?.activeInHierarchy != true)
				{
					bloodQueenCrazyEffect?.gameObject?.SetActive(value: true);
				}
			}
		}
		else
		{
			InstanceRequest instanceRequest2 = bloodQueenCrazyEffect;
			if (instanceRequest2 != null && instanceRequest2.gameObject?.activeInHierarchy == true)
			{
				bloodQueenCrazyEffect?.gameObject?.SetActive(value: false);
			}
		}
	}

	private void RefreshBloodQueenGunnerLine()
	{
		if (marchInfo != null && marchInfo.IsBloodyQueenQueenGunner())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.CreateWorldTroopLineQueen", marchInfo.uuid, marchInfo.startWorldPos, marchInfo.bloodyQueenMonster.standWorldPos);
		}
	}

	private void RemoveBloodQueenGunnerLine()
	{
		if (marchInfo != null && marchInfo.IsBloodyQueenQueenGunner())
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveWorldTroopLineQueen", marchInfo.uuid);
		}
	}
}
