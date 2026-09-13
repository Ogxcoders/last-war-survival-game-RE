using System;
using System.Collections.Generic;
using GameFramework;
using Protobuf;
using UnityEngine;
using UnityEngine.Playables;
using XLua;

public class WorldBuilding : MonoBehaviour, ITouchPickable, ITouchObjectClickHandler, ITouchObject, ITouchObjectBeginLongTabHandler, ITouchObjectEndLongTabHandler, ITouchObjectPointerDownHandler, IDynamicLoadBuilding
{
	public enum BuildSceneType
	{
		City = 1,
		World,
		Fake
	}

	public class Param
	{
		public int buildId;

		public long buildUuid;

		public int point;

		public int serverId;

		public PlaceBuildType BuildTopType;

		public LuaTable noPutPoint;

		public LuaTable param;

		public BuildSceneType buildSceneType;
	}

	public class AnimName
	{
		public const string Idle = "idle";

		public const string Click = "trigger";

		public const string Place = "placed";

		public const string Work = "working";

		public const string StartWork = "start";

		public const string EndWork = "end";

		public const string WorkIdle = "work_idle";

		public const string SelfWork = "self_working";

		public const string SelfEndWork = "self_end";
	}

	public enum AnimTimeState
	{
		Play,
		Stop
	}

	private enum BuildShowState
	{
		Normal,
		Box,
		Ruins
	}

	private const string WEREWOLF_LEVEL = "??";

	public static float ScreenRangeLeft;

	public static float ScreenRangeRight;

	public static float ScreenRangeTop;

	public static float ScreenRangeDown;

	private AnimTimeState _curAnimState;

	private float _curTime;

	private float _allTime;

	[SerializeField]
	private GameObject buildIcon;

	[SerializeField]
	protected GameObject buildModel;

	[SerializeField]
	private GameObject _shadow;

	[SerializeField]
	private GameObject _baseGlass;

	[SerializeField]
	private GameObject _effectGo;

	[SerializeField]
	private GPUSkinningAnimator gpuAnim;

	[SerializeField]
	private SimpleAnimation simpleAnim;

	[SerializeField]
	private SimpleAnimation _effectAnim;

	[SerializeField]
	private GameObject _foldUpGo;

	[SerializeField]
	private GameObject _normalObj;

	[SerializeField]
	private GameObject _boxObj;

	[SerializeField]
	private GameObject _ruinsObj;

	[SerializeField]
	private SimpleAnimation _boxAnim;

	[SerializeField]
	private UIEventTrigger _focusEventTrigger;

	[SerializeField]
	private SimpleAnimation buildingAnim;

	[SerializeField]
	private Animator buildingAnimator;

	[SerializeField]
	private WorldBuildingAniEffect buildingAniEffect;

	private DynamicLoadBuilding _dynamicLoadBuilding;

	private UIWorldLabel[] cityLabels;

	private Vector2Int tilePos;

	private Vector3 worldPos;

	private BasementDome domeObj;

	private InstanceRequest tempInstance;

	private AutoAdjustLod adjuster;

	private TouchObjectEventTrigger boxTouchEvent;

	private InstanceRequest instanceSeasonBlock;

	[HideInInspector]
	public string previewIconPath;

	[HideInInspector]
	public string previewName;

	[HideInInspector]
	public WorldPreviewType previewType;

	public int build_Id;

	public int tileX;

	public int tileY;

	public int scan;

	public int build_type;

	private int tab_type;

	private int max_level = -1;

	private Param _param;

	private string _animName;

	private BuildingGrowEffect _grow;

	private float _minX;

	private float _maxX;

	private float _minY;

	private float _maxY;

	private bool _isShowRobot;

	private bool? _canShowCityLabel;

	private string _ownerUuid;

	protected int _level;

	private Dictionary<AnimTimeState, float> _buildingLevelBuildActionTime;

	private int _offset_range;

	private int _state;

	private List<StatusStateBase> _status = new List<StatusStateBase>();

	private WorldBuildingAniEffectAni _aniEffectAni;

	private BuildingPlayIdleAniManager _playIdleAniManager;

	private TimelinePlayer _timelinePlayer;

	private InstanceRequest meteoriteRequest;

	private UICityMeteorite meteoriteComp;

	private float _fakeAutoDestroy_checkTime;

	private InstanceRequest _epidemicRequest;

	private int _epidemicCurId;

	private float _epidemicSkillCTime;

	private float _epidemicSkillETime;

	private InstanceRequest maxLevelEffect;

	private bool _glassVisible = true;

	private bool dynamicLoad => _dynamicLoadBuilding != null;

	public WorldPreviewType PreviewType => previewType;

	public Vector2Int TilePos => tilePos;

	public long Uuid { get; set; }

	public AutoAdjustLod AdjustLod => adjuster;

	public bool OpenFakeAutoDestroy { get; private set; }

	float ITouchObject.Priority => 1f;

	Vector2Int ITouchObject.TilePos
	{
		get
		{
			if (_param.buildSceneType == BuildSceneType.City)
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid != null)
				{
					return SceneManager.World.IndexToTilePos(buildingDataByUuid.pointId);
				}
			}
			else if (_param.buildSceneType == BuildSceneType.World)
			{
				BuildPointInfo buildInfo = GetBuildInfo();
				return SceneManager.World.IndexToTilePos(buildInfo.pointIndex);
			}
			return Vector2Int.zero;
		}
	}

	public AutoAdjustLod AutoAdjustLod => adjuster;

	public bool OnPointerDown()
	{
		return false;
	}

	public PointInfo GetPointInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfoByUuid(Uuid);
		}
		return null;
	}

	public BuildPointInfo GetBuildInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfoByUuid(Uuid) as BuildPointInfo;
		}
		return null;
	}

	private void Awake()
	{
	}

	protected internal virtual void CSInit(object userData)
	{
		_buildingLevelBuildActionTime = new Dictionary<AnimTimeState, float>();
		build_Id = 0;
		_level = 0;
		_grow = base.gameObject.GetComponent<BuildingGrowEffect>();
		_minX = ScreenRangeLeft;
		_maxX = (float)Screen.width - ScreenRangeRight;
		_minY = ScreenRangeDown;
		_maxY = (float)Screen.height - ScreenRangeTop;
		_param = userData as Param;
		_isShowRobot = false;
		OpenFakeAutoDestroy = _param.buildSceneType == BuildSceneType.Fake;
		_fakeAutoDestroy_checkTime = 0f;
		cityLabels = GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
		if (_param != null)
		{
			Uuid = _param.buildUuid;
			int num = 0;
			switch (_param.buildSceneType)
			{
			case BuildSceneType.City:
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid != null)
				{
					num = buildingDataByUuid.buildId;
				}
				GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
				break;
			}
			case BuildSceneType.World:
			{
				BuildPointInfo buildInfo = GetBuildInfo();
				if (buildInfo != null)
				{
					num = buildInfo.itemId;
				}
				break;
			}
			case BuildSceneType.Fake:
				num = _param.buildId;
				break;
			}
			build_Id = num;
			string templateData = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "tiles_2");
			if (!templateData.IsNullOrEmpty())
			{
				string[] array = templateData.Split(new char[1] { ';' });
				if (array.Length >= 2)
				{
					tileX = array[0].ToInt();
					tileY = array[1].ToInt();
				}
			}
			if (_boxObj != null)
			{
				Transform transform = _boxObj.gameObject.transform;
				boxTouchEvent = transform.GetComponentInChildren<TouchObjectEventTrigger>(includeInactive: true);
				if (boxTouchEvent != null)
				{
					boxTouchEvent.onPointerClick = OnClickUpdateBox;
				}
			}
			scan = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "scan").ToInt();
			build_type = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "build_type").ToInt();
			tab_type = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "tab_type").ToInt();
			max_level = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "max_level").ToInt();
			refeshDate();
			if (tab_type == 4 && _param.buildSceneType == BuildSceneType.Fake)
			{
				UIWorldLabel[] array2 = cityLabels;
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i].gameObject.transform.Set_localScale(0.001f, 0.001f, 0.001f);
				}
			}
			if (_foldUpGo != null)
			{
				_foldUpGo.SetActive(value: false);
			}
			CheckPlaced();
			if (_param.buildSceneType == BuildSceneType.Fake)
			{
				if (_param.BuildTopType == PlaceBuildType.Build || _param.BuildTopType == PlaceBuildType.Replace)
				{
					FromUILoad();
				}
				else
				{
					EnterMoveCityState((int)_param.BuildTopType);
				}
			}
			if (_focusEventTrigger != null)
			{
				_focusEventTrigger.onPointerClick = delegate
				{
					Vector3 lookat = base.transform.position + new Vector3(1 - tileX, 1 - tileY, 0f);
					SceneManager.World.AutoLookat(lookat, 23f, 0.3f);
				};
			}
			int skinId = InitPlayIdleAniManager();
			InitDynamicModel(skinId);
			InitLod(base.gameObject);
			if (!InitStatus())
			{
				PlayAnimationAndEffectReturnTime("idle");
			}
		}
		else
		{
			base.gameObject.SetActive(value: false);
			if (_playIdleAniManager != null)
			{
				_playIdleAniManager.ClearAllData();
				_playIdleAniManager = null;
			}
		}
	}

	protected internal virtual void CSUninit()
	{
		UnloadDynamicModel();
		if (_status != null)
		{
			for (int i = 0; i < _status.Count; i++)
			{
				if (_status[i] != null)
				{
					_status[i].Dispose();
				}
			}
			_status.Clear();
		}
		if (instanceSeasonBlock != null)
		{
			instanceSeasonBlock.Destroy();
			instanceSeasonBlock = null;
		}
		if (maxLevelEffect != null)
		{
			maxLevelEffect.Destroy();
			maxLevelEffect = null;
		}
		DestroyMeteorite();
		DestroyEpidemicSkill();
		CancelAnimTimer();
		Uuid = -1L;
		tilePos = Vector2Int.zero;
		_param = null;
		_animName = null;
		_focusEventTrigger = null;
		if (domeObj != null)
		{
			domeObj.CSUnInit();
			domeObj = null;
		}
		if (boxTouchEvent != null)
		{
			boxTouchEvent.onPointerClick = null;
			boxTouchEvent = null;
		}
		PlayAnimationAndEffectReturnTime("idle");
		_grow = null;
		DestroyDomeInstance();
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
		cityLabels = null;
		previewIconPath = null;
		previewName = null;
		OpenFakeAutoDestroy = false;
		_fakeAutoDestroy_checkTime = 0f;
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.ClearAllData();
			_playIdleAniManager = null;
		}
	}

	protected internal virtual void CSUpdate(float elapseSeconds)
	{
		if (_allTime > 0f)
		{
			_curTime += Time.deltaTime;
			if (_curTime >= _allTime)
			{
				_allTime = 0f;
				_curTime = 0f;
				ChangeAnimTimerCallBack();
			}
		}
		if (_status != null && _status.Count > 0)
		{
			List<StatusStateBase> list = new List<StatusStateBase>();
			for (int i = 0; i < _status.Count; i++)
			{
				StatusStateBase statusStateBase = _status[i];
				if (statusStateBase.Update(elapseSeconds) == StatusStateBase.StateType.Finish)
				{
					statusStateBase.Dispose();
					list.Add(statusStateBase);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				_status.Remove(list[j]);
			}
		}
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.OnUpdate();
		}
		if (_epidemicSkillETime > 0f)
		{
			_epidemicSkillCTime += Time.deltaTime;
			if (_epidemicSkillCTime >= _epidemicSkillETime)
			{
				DestroyEpidemicSkill();
			}
			else if (_normalObj != null && _normalObj.activeSelf)
			{
				_normalObj.SetActive(value: false);
			}
		}
	}

	public void CheckFakeAutoDestroy()
	{
		if (!OpenFakeAutoDestroy)
		{
			return;
		}
		_fakeAutoDestroy_checkTime += Time.deltaTime;
		if (!(_fakeAutoDestroy_checkTime >= 3f))
		{
			return;
		}
		_fakeAutoDestroy_checkTime = 0f;
		if (GameEntry.Lua.UIManager.IsWindowOpen("UIMoveCity"))
		{
			return;
		}
		if (SceneManager.World != null)
		{
			SceneManager.World.UIDestroyRreCreateBuild();
			return;
		}
		CSUninit();
		if (base.gameObject != null)
		{
			base.gameObject.Destroy();
		}
	}

	public void InitLod(GameObject gameObject)
	{
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo == null)
		{
			return;
		}
		PlayerType playerType = buildInfo.GetPlayerType();
		LodType lodType = LodType.None;
		if (buildInfo.AOSType != AllianceOfficialSkillType.None && playerType != PlayerType.PlayerSelf)
		{
			lodType = LodType.AresMissile;
		}
		else if (build_Id == 10100000)
		{
			switch (playerType)
			{
			case PlayerType.PlayerSelf:
				lodType = LodType.MainSelf;
				break;
			case PlayerType.PlayerAlliance:
			case PlayerType.PlayerAllianceLeader:
				lodType = LodType.MainAlly;
				break;
			default:
				lodType = LodType.MainOther;
				break;
			}
		}
		else if (tab_type == 4)
		{
			lodType = LodType.SeasonCity;
			if (playerType == PlayerType.PlayerSelf)
			{
				if (instanceSeasonBlock != null)
				{
					instanceSeasonBlock.Destroy();
					instanceSeasonBlock = null;
				}
				instanceSeasonBlock = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Building/BuildBlockSelf.prefab");
				instanceSeasonBlock.completed += delegate
				{
					SceneInterface world = SceneManager.World;
					GameObject gameObject2 = instanceSeasonBlock.gameObject;
					if (gameObject2 != null && _param != null && _param.point > 0 && world != null)
					{
						gameObject2.transform.SetParent(world.DynamicObjNode);
						gameObject2.transform.position = world.TileIndexToWorld(_param.point);
						gameObject2.SetActive(value: true);
					}
					else if (gameObject2 != null)
					{
						gameObject2.SetActive(value: false);
					}
				};
			}
		}
		adjuster = gameObject.GetComponent<AutoAdjustLod>();
		if (lodType != LodType.None)
		{
			if (adjuster == null)
			{
				adjuster = gameObject.AddComponent<AutoAdjustLod>();
			}
			adjuster.SetLodType(lodType);
			adjuster.SetLodUpdateCallback(delegate(int i)
			{
				OnLodChangeFunc(i);
			});
		}
		else if (adjuster != null)
		{
			UnityEngine.Object.Destroy(adjuster);
		}
	}

	protected void OnClickUpdateBox()
	{
		int point = _param.point;
		if (point > 0)
		{
			if (SceneManager.World.GetLodLevel() >= 3)
			{
				Vector3 lookat = SceneManager.World.TileIndexToWorld(point);
				SceneManager.World.AutoLookat(lookat, SceneManager.World.InitZoom);
			}
			else
			{
				GameEntry.Lua.Call("UIUtil.OnClickWorld", point, 1);
			}
		}
	}

	public float DoFoldUpAnim()
	{
		if (_foldUpGo != null)
		{
			_foldUpGo.SetActive(value: true);
		}
		if (_grow != null)
		{
			_grow.enabled = true;
			_grow.DisappearBuild(Uuid, build_Id, GameEntry.Timer.GetServerTimeSeconds(), GameEntry.Timer.GetServerTimeSeconds() + 1, tileX, tileY, GameEntry.Data.Player.Uid);
		}
		return 0.7f;
	}

	private int InitPlayIdleAniManager()
	{
		bool flag = false;
		string text = null;
		int num = 0;
		if (_param != null)
		{
			switch (_param.buildSceneType)
			{
			case BuildSceneType.City:
			{
				LuaBuildData buildingDataByUuid2 = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid2 != null && buildingDataByUuid2.buildId == 10100000)
				{
					num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCityMainSkinId");
				}
				break;
			}
			case BuildSceneType.World:
			{
				BuildPointInfo buildInfo2 = GetBuildInfo();
				if (buildInfo2 != null)
				{
					num = buildInfo2.GetSkinId();
				}
				break;
			}
			case BuildSceneType.Fake:
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid != null && buildingDataByUuid.buildId == 10100000)
				{
					num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCityMainSkinId");
				}
				if (num <= 0)
				{
					BuildPointInfo buildInfo = GetBuildInfo();
					if (buildInfo != null)
					{
						num = buildInfo.GetSkinId();
					}
				}
				break;
			}
			}
			if (num > 0)
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "act_idle_status_para");
				if (!string.IsNullOrEmpty(text))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			if (_playIdleAniManager == null)
			{
				_playIdleAniManager = new BuildingPlayIdleAniManager();
			}
			_playIdleAniManager.InitData(text, (string s, float f) => PlayCrossFadeAnim(s, f));
		}
		else
		{
			ClearPlayIdleAniManager();
		}
		return num;
	}

	public void ClearPlayIdleAniManager()
	{
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.ClearAllData();
			_playIdleAniManager = null;
		}
	}

	public void DoExtendDome()
	{
		if (domeObj != null)
		{
			domeObj.ShowExtend();
		}
	}

	public void DoUpgradeDome()
	{
		GameEntry.Event.Fire(EventId.ShowDomeGlass, Uuid);
		if (domeObj != null)
		{
			domeObj.CSUnInit();
			domeObj = null;
		}
		ShowGlass(upgradeDome: true);
	}

	public virtual void refeshDate()
	{
		if (_param == null)
		{
			return;
		}
		int num = 0;
		switch (_param.buildSceneType)
		{
		case BuildSceneType.City:
		{
			LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
			if (buildingDataByUuid != null)
			{
				num = buildingDataByUuid.pointId;
				_level = buildingDataByUuid.level;
				_state = buildingDataByUuid.state;
			}
			break;
		}
		case BuildSceneType.World:
		{
			BuildPointInfo buildInfo = GetBuildInfo();
			if (buildInfo != null)
			{
				num = buildInfo.mainIndex;
				_level = buildInfo.level;
				if (buildInfo.ownerUid == GameEntry.Data.Player.Uid && GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid) != null)
				{
					_state = buildInfo.state;
				}
			}
			break;
		}
		case BuildSceneType.Fake:
			num = _param.point;
			_level = 0;
			break;
		}
		_buildingLevelBuildActionTime.Clear();
		if (_level > 0)
		{
			_offset_range = SceneManager.World.GetBuildOffsetRangeByBuildId(_level + build_Id);
			string templateData = GameEntry.ConfigCache.GetTemplateData("building", _level + build_Id, "building_action");
			if (!templateData.IsNullOrEmpty())
			{
				string[] array = templateData.Split(new char[1] { ';' });
				if (array.Length > 1)
				{
					_buildingLevelBuildActionTime.Add(AnimTimeState.Play, array[0].ToFloat());
					_buildingLevelBuildActionTime.Add(AnimTimeState.Stop, array[1].ToFloat());
				}
			}
		}
		else
		{
			_offset_range = 0;
		}
		_param.point = num;
		if (num != 0)
		{
			SetTilePos1(_param.serverId, SceneManager.World.IndexToTilePos(num));
		}
		InitBuildingGrow();
		if (domeObj != null)
		{
			domeObj.RefreshData();
		}
	}

	private void SetTilePos1(int theServerId, Vector2Int pos, Vector3? _worldPos = null)
	{
		if (pos != tilePos)
		{
			tilePos = pos;
			if (_worldPos.HasValue)
			{
				worldPos = TileCoord.WorldToClosestGridWorld(_worldPos.Value);
			}
			else
			{
				worldPos = TileCoord.TileToWorld(tilePos, theServerId);
			}
			base.transform.position = worldPos;
			SetTouchPickAblePos();
		}
	}

	private void SetTouchPickAblePos()
	{
		if (SceneManager.World.SelectBuild == this)
		{
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, int, int>("CSharpCallLuaInterface.GetBuildTileIndex", build_Id, SceneManager.World.TilePosToIndex(tilePos));
			GameEntry.Event.Fire(EventId.UIPlaceBuildChangePos, worldPos);
		}
	}

	private void FromUILoad()
	{
		SceneManager.World.SetSelectedPickable(this);
		long num = 0L;
		int num2 = 0;
		LuaTable param = null;
		PlaceBuildType placeBuildType = PlaceBuildType.Build;
		if (_param != null)
		{
			num = _param.buildUuid;
			if (GameEntry.Lua.CallWithReturn<int>("DataCenter.GuideManager:GetGuideType") == 4)
			{
				string text = GameEntry.Lua.CallWithReturn<string, string>("DataCenter.GuideManager:GetGuideTemplateParam", "para2");
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(new char[1] { ',' });
					if (array.Length > 1)
					{
						_param.point = SceneManager.World.TilePosToIndex(GameEntry.Data.Building.GetMainPos() + new Vector2Int(array[0].ToInt(), array[1].ToInt()));
					}
					GameEntry.Lua.Call("DataCenter.GuideManager:DoNext");
				}
			}
			num2 = _param.point;
			placeBuildType = _param.BuildTopType;
			param = _param.noPutPoint;
			SetTilePos1(_param.serverId, SceneManager.World.IndexToTilePos(num2));
			SetTouchPickAblePos();
			GameEntry.Lua.UIManager.OpenWindow("UIPlaceBuild", build_Id, num, num2, (int)placeBuildType);
		}
		if (_grow != null)
		{
			_grow.enabled = false;
			_grow.ShowBuildGridSelection();
			ShowShadow(isShow: false);
			ShowEffect(isShow: false);
			int num3 = GameEntry.Lua.CallWithReturn<int, int, int, long, LuaTable, int>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, num2, num, param, _param.serverId);
			_grow.ShowCanPlace(num3 == 1);
		}
	}

	public void EnterMoveCityState(int moveCityType)
	{
		SceneManager.World.SetSelectedPickable(this);
		long num = 0L;
		int num2 = SceneManager.World.TilePosToIndex(tilePos);
		LuaTable param = null;
		PlaceBuildType buildTopType = PlaceBuildType.MoveCity;
		if (_param != null)
		{
			_param.BuildTopType = buildTopType;
			num = _param.buildUuid;
			param = _param.noPutPoint;
			SetTouchPickAblePos();
			GameEntry.Lua.UIManager.OpenWindow("UIMoveCity", build_Id, num, num2, moveCityType, _param.param);
		}
		if (_grow != null)
		{
			_grow.enabled = false;
			_grow.ShowBuildGridSelection();
			ShowShadow(isShow: false);
			ShowEffect(isShow: false);
			int num3 = GameEntry.Lua.CallWithReturn<int, int, int, long, LuaTable, int>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, num2, num, param, _param.serverId);
			_grow.ShowCanPlace(num3 == 1);
		}
	}

	public Transform GetTransform()
	{
		if (base.transform != null)
		{
			return base.transform;
		}
		return null;
	}

	public bool PointInPick()
	{
		Vector2Int touchTilePos = SceneManager.World.GetTouchTilePos();
		Vector2Int vector2Int = new Vector2Int(TilePos.x, TilePos.y);
		if (touchTilePos == vector2Int)
		{
			return true;
		}
		if (tileX > 1 || tileY > 1)
		{
			return GameEntry.Lua.CallWithReturn<bool, int, int, int, int, int, int>("CSharpCallLuaInterface.CheckIsInBuildRange", touchTilePos.x, touchTilePos.y, vector2Int.x, vector2Int.y, tileX, tileY);
		}
		return false;
	}

	public void Drag(Vector3 pos)
	{
		if (SceneManager.World.GetTouchInputControllerEnable() && base.transform != null)
		{
			int serverIdFromWorldPos = SeasonDataManager.Instance.GetServerIdFromWorldPos(pos);
			SetTilePos1(serverIdFromWorldPos, SceneManager.World.WorldToTile(pos), pos);
			int param = SceneManager.World.WorldToTileIndex(pos);
			PutState putState = PutState.None;
			putState = (PutState)((_param == null) ? GameEntry.Lua.CallWithReturn<int, int, int, long, List<int>, int>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, param, 0L, null, serverIdFromWorldPos) : GameEntry.Lua.CallWithReturn<int, int, int, long, List<int>, int>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, param, _param.buildUuid, null, serverIdFromWorldPos));
			_grow.ShowCanPlace(putState == PutState.Ok);
		}
	}

	public virtual bool Select()
	{
		return false;
	}

	public bool CanLongTap()
	{
		return false;
	}

	T ITouchPickable.GetPickComponent<T>()
	{
		if (base.transform != null)
		{
			return base.transform.GetComponent<T>();
		}
		return null;
	}

	private void InitBuildingGrow()
	{
		if (_grow == null || _param == null)
		{
			return;
		}
		PlayerType playerType = PlayerType.PlayerSelf;
		QueueState queueState = QueueState.DEFAULT;
		int startTime = 0;
		int num = 0;
		string ownerUid = "";
		long serverTime = GameEntry.Timer.GetServerTime();
		switch (_param.buildSceneType)
		{
		case BuildSceneType.City:
		{
			playerType = PlayerType.PlayerSelf;
			ownerUid = GameEntry.Data.Player.Uid;
			LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
			if (buildingDataByUuid == null)
			{
				break;
			}
			if (buildingDataByUuid.buildUpdateTime > 0)
			{
				queueState = QueueState.UPGRADE;
			}
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, long>("CSharpCallLuaInterface.GetBuildStartTimeAndEndTime", _param.buildUuid);
			if (luaTable != null)
			{
				if (luaTable.ContainsKey("startTime"))
				{
					startTime = (int)(luaTable.Get<long>("startTime") / 1000);
				}
				if (luaTable.ContainsKey("endTime"))
				{
					num = (int)(luaTable.Get<long>("endTime") / 1000);
				}
			}
			break;
		}
		case BuildSceneType.World:
		{
			BuildPointInfo buildInfo = GetBuildInfo();
			if (buildInfo == null)
			{
				break;
			}
			playerType = buildInfo.GetPlayerType();
			ownerUid = buildInfo.ownerUid;
			if (buildInfo.destroyStartTime > 0)
			{
				startTime = buildInfo.startTime;
				num = buildInfo.endTime;
				queueState = QueueState.Ruins;
				break;
			}
			if (buildInfo.endTime > 0)
			{
				startTime = buildInfo.startTime;
				num = buildInfo.endTime;
				queueState = QueueState.UPGRADE;
				break;
			}
			startTime = buildInfo.startTime;
			num = buildInfo.endTime;
			queueState = buildInfo.GetShowState();
			if (queueState == QueueState.UPGRADE)
			{
				queueState = buildInfo.GetShowStateByIndex(1);
			}
			break;
		}
		case BuildSceneType.Fake:
		{
			BuildAnimatorManager.BuildAnimatorParam buildingParam = GameEntry.BuildAnimatorManager.GetBuildingParam(_param.point);
			if (buildingParam != null && buildingParam.endTime > serverTime)
			{
				startTime = Mathf.RoundToInt((float)buildingParam.startTime / 1000f);
				num = Mathf.RoundToInt((float)buildingParam.endTime / 1000f);
			}
			GameEntry.BuildAnimatorManager.RemoveOneBuild(_param.point);
			break;
		}
		}
		bool flag = scan == 1 || scan == 2;
		switch (queueState)
		{
		case QueueState.UPGRADE:
			if (build_type == 2)
			{
				DoBuildPlaceAnim();
				_grow.ShowNormal(playerType);
				ShowShadow(isShow: true);
				ShowGlass(upgradeDome: false);
				SetBuildShowState(playerType, BuildShowState.Normal);
			}
			else if (flag)
			{
				if (num > serverTime / 1000)
				{
					if (SceneManager.IsInCity())
					{
						_grow.enabled = true;
						_grow.StartBuild(_param.buildUuid, build_Id, startTime, num, tileX, tileY, ownerUid, isDomeUpdate: false, flag);
						ShowShadow(isShow: false);
						ShowEffect(isShow: false);
						ShowGlass(upgradeDome: true);
						SetBuildShowState(playerType, BuildShowState.Normal);
						break;
					}
					if (_grow.isWorking)
					{
						_grow.EndAnim();
					}
					if (_grow.enabled)
					{
						_grow.enabled = false;
					}
					_grow.ShowNormal(playerType);
					ShowShadow(isShow: true);
					ShowGlass(upgradeDome: false);
					ShowEffect(isShow: false);
					SetBuildShowState(playerType, BuildShowState.Normal);
				}
				else
				{
					if (_grow.isWorking)
					{
						_grow.EndAnim();
					}
					if (_grow.enabled)
					{
						_grow.enabled = false;
					}
					_grow.ShowNormal(playerType);
					ShowShadow(isShow: true);
					ShowGlass(upgradeDome: false);
					if (tab_type == 4)
					{
						SetBuildShowState(playerType, BuildShowState.Box);
					}
				}
			}
			else
			{
				if (_grow.isWorking)
				{
					_grow.EndAnim();
				}
				if (_grow.enabled)
				{
					_grow.enabled = false;
				}
				_grow.ShowNormal(playerType);
				ShowShadow(isShow: true);
				ShowGlass(upgradeDome: false);
				ShowEffect(isShow: false);
				if (num > serverTime / 1000)
				{
					SetBuildShowState(playerType, BuildShowState.Normal);
				}
				else if (tab_type == 4)
				{
					SetBuildShowState(playerType, BuildShowState.Box);
				}
			}
			break;
		case QueueState.TRAINING:
		case QueueState.FACTORY:
		{
			if (_grow.isWorking)
			{
				_grow.EndAnim();
			}
			string animName = _animName;
			if (animName == null || animName == "idle")
			{
				float num2 = PlayAnim("start");
				if (num2 > 0f)
				{
					GameEntry.Timer.RegisterTimer(num2, delegate
					{
						if (this != null && _param != null)
						{
							if (_animName == "start")
							{
								PlayAnim("working");
							}
							else
							{
								PlayAnim("idle");
							}
						}
					});
				}
				else
				{
					PlayAnim("working");
				}
			}
			if (_grow.enabled)
			{
				_grow.enabled = false;
			}
			_grow.ShowNormal(playerType);
			ShowShadow(isShow: true);
			ShowGlass(upgradeDome: false);
			SetBuildShowState(playerType, BuildShowState.Normal);
			break;
		}
		case QueueState.CURE_ARMY:
		case QueueState.REBIRTH_ARMY:
			if (_grow.isWorking)
			{
				_grow.EndAnim();
			}
			if (!IsSelfControlWorkAnim())
			{
				string animName = _animName;
				if (animName == null || animName == "idle")
				{
					float num3 = PlayAnim("start");
					if (num3 > 0f)
					{
						GameEntry.Timer.RegisterTimer(num3, delegate
						{
							if (this != null && _param != null)
							{
								if (_animName == "start")
								{
									PlayAnim("working");
								}
								else
								{
									PlayAnim("idle");
								}
							}
						});
					}
					else
					{
						PlayAnim("working");
					}
				}
			}
			if (_grow.enabled)
			{
				_grow.enabled = false;
			}
			_grow.ShowNormal(playerType);
			ShowShadow(isShow: true);
			ShowGlass(upgradeDome: false);
			SetBuildShowState(playerType, BuildShowState.Normal);
			break;
		case QueueState.Ruins:
			if (_grow.isWorking)
			{
				_grow.EndAnim();
			}
			PlayAnim("idle");
			if (_grow.enabled)
			{
				_grow.enabled = false;
			}
			_grow.ShowNormal(playerType);
			ShowShadow(isShow: true);
			ShowGlass(upgradeDome: false);
			SetBuildShowState(playerType, BuildShowState.Ruins);
			break;
		case QueueState.DEFAULT:
			if (_grow.isWorking)
			{
				_grow.EndAnim();
			}
			if (!IsSelfControlWorkAnim())
			{
				switch (_animName)
				{
				case null:
					PlayAnim("idle");
					break;
				case "working":
				{
					float num5 = PlayAnim("end");
					if (num5 > 0f)
					{
						GameEntry.Timer.RegisterTimer(num5, delegate
						{
							if (this != null && _param != null)
							{
								if (_animName == "end")
								{
									if (HasAnimClip("work_idle"))
									{
										PlayAnim("work_idle");
									}
									else
									{
										PlayAnim("idle");
									}
								}
								else
								{
									PlayAnim("idle");
								}
							}
						});
					}
					else
					{
						PlayAnim("idle");
					}
					break;
				}
				case "self_working":
				{
					float num4 = PlayAnim("self_end");
					if (num4 > 0f)
					{
						GameEntry.Timer.RegisterTimer(num4, delegate
						{
							if (this != null && _param != null)
							{
								if (_animName == "self_end")
								{
									if (HasAnimClip("work_idle"))
									{
										PlayAnim("work_idle");
									}
									else
									{
										PlayAnim("idle");
									}
								}
								else
								{
									PlayAnim("idle");
								}
							}
						});
					}
					else
					{
						PlayAnim("idle");
					}
					break;
				}
				}
			}
			if (_grow.enabled)
			{
				_grow.enabled = false;
			}
			_grow.ShowNormal(playerType);
			ShowShadow(isShow: true);
			ShowGlass(upgradeDome: false);
			SetBuildShowState(playerType, BuildShowState.Normal);
			break;
		default:
			if (_grow.isWorking)
			{
				_grow.EndAnim();
			}
			PlayAnim("idle");
			if (_grow.enabled)
			{
				_grow.enabled = false;
			}
			_grow.ShowNormal(playerType);
			ShowShadow(isShow: true);
			ShowGlass(upgradeDome: false);
			SetBuildShowState(playerType, BuildShowState.Normal);
			break;
		}
	}

	private void ShowShadow(bool isShow)
	{
		if (!(_shadow != null))
		{
			return;
		}
		if (isShow)
		{
			if (_grow != null && _grow.IsUseFakeShadow())
			{
				_shadow.SetActive(value: true);
			}
			else
			{
				_shadow.SetActive(value: false);
			}
		}
		else
		{
			_shadow.SetActive(value: false);
		}
	}

	void ITouchPickable.Click()
	{
	}

	public void DestroyDomeInstance()
	{
		if (tempInstance != null)
		{
			tempInstance.Destroy();
			tempInstance = null;
		}
	}

	private void ShowGlass(bool upgradeDome)
	{
	}

	private void ShowEffect(bool isShow)
	{
		if (_effectGo != null)
		{
			_effectGo.SetActive(isShow);
		}
	}

	public bool IsOutRange(Vector3 pos)
	{
		Vector3 vector = SceneManager.World.WorldToScreenPoint(pos);
		float x = vector.x;
		float y = vector.y;
		if (!(x < _minX) && !(x > _maxX) && !(y < _minY))
		{
			return y > _maxY;
		}
		return true;
	}

	public void ChangeTouchPos(int index)
	{
	}

	public void OnBattleDefUpdate(int damage)
	{
		if (damage > 0)
		{
			SceneManager.World.ShowBattleBlood(new BattleDecBloodTip.Param
			{
				startPos = base.transform.position,
				num = damage,
				path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab"
			});
		}
	}

	public void ResetParam(int posIndex = 0)
	{
		if (_param != null)
		{
			_param.point = posIndex;
		}
	}

	public void ChangeMove()
	{
		_param.BuildTopType = PlaceBuildType.Move;
		FromUILoad();
	}

	public Vector3 GetClosestPoint(Vector3 pos)
	{
		Vector3 vector = SceneManager.World.WorldToScreenPoint(pos);
		float x = vector.x;
		float y = vector.y;
		if (x < _minX)
		{
			vector.x = _minX;
		}
		else if (x > _maxX)
		{
			vector.x = _maxX;
		}
		if (y < _minY)
		{
			vector.y = _minY;
		}
		else if (y > _maxY)
		{
			vector.y = _maxY;
		}
		return SceneManager.World.ScreenPointToWorld(vector);
	}

	public bool HasAnimClip(string animName)
	{
		if (gpuAnim != null)
		{
			return gpuAnim.HasClip(animName);
		}
		if (simpleAnim != null)
		{
			return simpleAnim.GetState(animName) != null;
		}
		return false;
	}

	public float PlayAnim(string animName)
	{
		if (adjuster != null && !adjuster.IsMainShow())
		{
			return 0f;
		}
		float result = 0f;
		if (_state == 1)
		{
			return result;
		}
		if (!string.IsNullOrEmpty(animName))
		{
			_animName = animName;
			if (gpuAnim != null)
			{
				result = UIUtils.PlayAnimationReturnTime(gpuAnim, animName);
			}
			else if (simpleAnim != null)
			{
				result = UIUtils.PlayAnimationReturnTime(simpleAnim, animName);
			}
			string text = animName + "_effect";
			if (_effectAnim != null && _effectAnim.GetState(text) != null)
			{
				ShowEffect(isShow: true);
				UIUtils.PlayAnimationReturnTime(_effectAnim, text);
			}
			else
			{
				ShowEffect(isShow: false);
			}
		}
		else
		{
			ShowEffect(isShow: false);
		}
		ChangeAnimTimerState(AnimTimeState.Play);
		return result;
	}

	public void DoBuildClickAnim()
	{
		float num = 0f;
		if (gpuAnim != null)
		{
			num = UIUtils.PlayAnimationReturnTime(gpuAnim, "trigger");
		}
		else if (simpleAnim != null)
		{
			num = UIUtils.PlayAnimationReturnTime(simpleAnim, "trigger");
		}
		if (!(num > 0f))
		{
			return;
		}
		GameEntry.Timer.RegisterTimer(num, delegate
		{
			if (this != null && _param != null)
			{
				PlayAnim(_animName);
			}
		});
	}

	public void DoBuildPlaceAnim()
	{
		float num = 0f;
		if (gpuAnim != null)
		{
			num = UIUtils.PlayAnimationReturnTime(gpuAnim, "placed");
		}
		else if (simpleAnim != null)
		{
			num = UIUtils.PlayAnimationReturnTime(simpleAnim, "placed");
		}
		if (!(num > 0f))
		{
			return;
		}
		GameEntry.Timer.RegisterTimer(num, delegate
		{
			if (this != null && _param != null)
			{
				if (_animName != null)
				{
					PlayAnim(_animName);
				}
				else
				{
					PlayAnim("idle");
				}
			}
		});
	}

	public SimpleAnimation GetPlayBuildingAnim()
	{
		if (buildingAnim != null)
		{
			return buildingAnim;
		}
		return null;
	}

	private void CheckPlaced()
	{
		if (SceneManager.World != null && SceneManager.World.IsNeedPlayPlacedAnim(Uuid))
		{
			DoBuildPlaceAnim();
		}
	}

	public bool OnClick()
	{
		DoBuildClickAnim();
		if (SceneManager.IsInWorld())
		{
			bool flag = false;
			if (SceneManager.World.GetLodLevel() >= 3)
			{
				BuildPointInfo buildInfo = GetBuildInfo();
				if (buildInfo == null || buildInfo.AOSType == AllianceOfficialSkillType.None)
				{
					flag = true;
				}
			}
			if (flag)
			{
				Vector3 lookat = SceneManager.World.TileToWorld(tilePos);
				if (SceneManager.World.WorldSize == 3000)
				{
					Vector3 curTouchPoint = SceneManager.World.curTouchPoint;
					SceneManager.World.AutoLookat(curTouchPoint, SceneManager.World.InitZoom);
				}
				else
				{
					SceneManager.World.AutoLookat(lookat, SceneManager.World.InitZoom);
				}
			}
			else
			{
				int param = SceneManager.World.TilePosToIndex(tilePos);
				GameEntry.Lua.Call("UIUtil.OnClickWorld", param, 1);
			}
		}
		else if (SceneManager.IsInCity())
		{
			int param2 = SceneManager.World.TilePosToIndex(tilePos);
			GameEntry.Lua.Call("UIUtil.OnClickCity", param2, 1);
		}
		return true;
	}

	public bool OnBeginLongTap()
	{
		if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.CanMoveBuild", build_Id + _level))
		{
			if (_param.buildSceneType == BuildSceneType.City)
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid != null)
				{
					SceneManager.World.ShowLoad(GameEntry.Lua.CallWithReturn<Vector3, int, int, int>("CSharpCallLuaInterface.GetBuildModelCenterVec", buildingDataByUuid.pointId, tileX, tileY));
				}
			}
			else if (_param.buildSceneType == BuildSceneType.World)
			{
				BuildPointInfo buildInfo = GetBuildInfo();
				if (buildInfo.ownerUid == GameEntry.Data.Player.Uid)
				{
					BuildPointInfo buildPointInfo = buildInfo;
					if (buildPointInfo != null && buildPointInfo.itemId != 792000)
					{
						if (buildPointInfo.itemId == 791000)
						{
							if (GameEntry.Data.Building.GetBuildingDataByBuildId(792000) == null)
							{
								SceneManager.World.ShowLoad(GameEntry.Lua.CallWithReturn<Vector3, int, int, int>("CSharpCallLuaInterface.GetBuildModelCenterVec", buildInfo.mainIndex, tileX, tileY));
							}
						}
						else
						{
							SceneManager.World.ShowLoad(GameEntry.Lua.CallWithReturn<Vector3, int, int, int>("CSharpCallLuaInterface.GetBuildModelCenterVec", buildInfo.mainIndex, tileX, tileY));
						}
					}
				}
			}
		}
		return true;
	}

	public bool OnEndLongTap()
	{
		if (GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.CanMoveBuild", build_Id + _level))
		{
			if (_param.buildSceneType == BuildSceneType.City)
			{
				if (GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid) != null)
				{
					GameEntry.Event.Fire(EventId.ClickFarmBuildHide);
					ChangeMove();
				}
			}
			else if (_param.buildSceneType == BuildSceneType.World)
			{
				BuildPointInfo buildInfo = GetBuildInfo();
				if (buildInfo.pointType == WorldPointType.PlayerBuilding && buildInfo.ownerUid == GameEntry.Data.Player.Uid && buildInfo.itemId != 792000)
				{
					if (buildInfo.itemId == 791000)
					{
						if (GameEntry.Data.Building.GetBuildingDataByBuildId(792000) == null)
						{
							GameEntry.Event.Fire(EventId.ClickFarmBuildHide);
							ChangeMove();
						}
					}
					else
					{
						GameEntry.Event.Fire(EventId.ClickFarmBuildHide);
						ChangeMove();
					}
				}
			}
		}
		return true;
	}

	private void ChangeAnimTimerState(AnimTimeState animTimeState)
	{
		CancelAnimTimer();
		_curAnimState = animTimeState;
		if (_param != null)
		{
			float num = 0f;
			if (_buildingLevelBuildActionTime.ContainsKey(animTimeState))
			{
				num = _buildingLevelBuildActionTime[animTimeState];
			}
			if (num > 0f)
			{
				AddAnimTimer(num);
			}
		}
	}

	private void CancelAnimTimer()
	{
		_allTime = 0f;
		_curTime = 0f;
	}

	private void AddAnimTimer(float time)
	{
		CancelAnimTimer();
		_allTime = time;
		_curTime = 0f;
	}

	private void StopAnim()
	{
		if (gpuAnim != null)
		{
			gpuAnim.Stop();
		}
		else if (simpleAnim != null)
		{
			simpleAnim.Stop();
		}
		ShowEffect(isShow: false);
		ChangeAnimTimerState(AnimTimeState.Stop);
	}

	private void ChangeAnimTimerCallBack()
	{
		switch (_curAnimState)
		{
		case AnimTimeState.Play:
			StopAnim();
			break;
		case AnimTimeState.Stop:
			PlayAnim(_animName);
			break;
		}
	}

	public float GetHeight()
	{
		if (_grow != null)
		{
			return _grow.GetHeight();
		}
		return 0f;
	}

	public void ShowDome()
	{
		if (domeObj != null)
		{
			domeObj.ShowClickAnim();
		}
	}

	public void HideDome()
	{
		if (domeObj != null)
		{
			domeObj.HideClickAnim();
		}
	}

	public bool IsSelf()
	{
		switch (_param.buildSceneType)
		{
		case BuildSceneType.City:
			return true;
		case BuildSceneType.World:
		{
			BuildPointInfo buildInfo = GetBuildInfo();
			if (buildInfo != null && buildInfo.ownerUid == GameEntry.Data.Player.Uid)
			{
				return true;
			}
			break;
		}
		}
		return false;
	}

	public void UpdateCityLabel(long obj)
	{
		if (Uuid != obj)
		{
			return;
		}
		BuildPointInfo buildPointInfo = ((_param.BuildTopType == PlaceBuildType.MoveCity) ? (SceneManager.World.GetMyPointInfo() as BuildPointInfo) : (SceneManager.World.GetPointInfoByUuid(obj) as BuildPointInfo));
		if (buildPointInfo != null && (buildPointInfo.itemId == 10100000 || buildPointInfo.itemId == 792000 || buildPointInfo.itemId == 735000 || tab_type == 4))
		{
			int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
			int serverId = buildPointInfo.serverId;
			int srcServerId = buildPointInfo.srcServerId;
			string n;
			if (buildPointInfo.IsWerewolf)
			{
				n = GameEntry.Localization.GetString("season_s4_activity_1200011_name");
			}
			else
			{
				string text = GameEntry.Lua.CallWithReturn<string, string, string>("CSharpCallLuaInterface.GetRemarkOrRealName", buildPointInfo.ownerUid, buildPointInfo.playerName);
				if ((serverId != srcServerId && srcServerId != 0) || serverId != sourceServerId)
				{
					n = UIUtils.FormatServerAllianceName(srcServerId, buildPointInfo.alAbbr, text);
				}
				else
				{
					if (GameEntry.Data.Player.IsInBattleField())
					{
						Log.Error($"DragonMap cityLabel without serverId!!! uuid={obj}, serverId={serverId}, thisPointServerId={srcServerId}, mySourceServerId={sourceServerId}, BuildTopType={_param.BuildTopType}");
					}
					n = UIUtils.FormatAllianceAndName(buildPointInfo.alAbbr, text);
				}
			}
			GameDefines.CityLabelColorType color = GameDefines.CityLabelColorType.White;
			if (!GameEntry.Data.Player.IsInBattleField())
			{
				color = (buildPointInfo.IsWerewolf ? ((buildPointInfo.ownerUid == GameEntry.Data.Player.Uid) ? GameDefines.CityLabelColorType.Green : ((string.IsNullOrEmpty(buildPointInfo.allianceId) || buildPointInfo.allianceId != GameEntry.Data.Player.GetAllianceId()) ? GameDefines.CityLabelColorType.Red : GameDefines.CityLabelColorType.Blue)) : (buildPointInfo.GetPlayerType() switch
				{
					PlayerType.PlayerSelf => (buildPointInfo.specialType == Protobuf.SpecialType.None) ? GameDefines.CityLabelColorType.Green : GameDefines.CityLabelColorType.Red, 
					PlayerType.PlayerAlliance => GameDefines.CityLabelColorType.Blue, 
					PlayerType.PlayerAllianceLeader => GameDefines.CityLabelColorType.Purple, 
					_ => SceneManager.World.IsMyEnemy(srcServerId, buildPointInfo.allianceId) switch
					{
						PlayerType.PlayerAllianceEnemy => GameDefines.CityLabelColorType.AllianceEnemy, 
						PlayerType.PlayerZoneEnemy => GameDefines.CityLabelColorType.ZoneEnemy, 
						PlayerType.PlayerSeasonEnemy => GameDefines.CityLabelColorType.SeasonEnemy, 
						PlayerType.PlayerSeasonCamp => GameDefines.CityLabelColorType.SeasonCamp, 
						PlayerType.PlayerSeasonAssist => GameDefines.CityLabelColorType.SeasonAssist, 
						_ => GameDefines.CityLabelColorType.White, 
					}, 
				}));
			}
			else if (buildPointInfo.ownerUid == GameEntry.Data.Player.Uid)
			{
				color = GameDefines.CityLabelColorType.Green;
			}
			else
			{
				int worldType = GameEntry.Data.Player.GetWorldType();
				switch (worldType)
				{
				case 1:
					color = ((buildPointInfo.allianceId != GameEntry.Data.Player.GetAllianceId()) ? GameDefines.CityLabelColorType.Red : GameDefines.CityLabelColorType.Blue);
					break;
				case 2:
					color = (GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", buildPointInfo.ownerUid, worldType) ? GameDefines.CityLabelColorType.Red : GameDefines.CityLabelColorType.Blue);
					break;
				case 3:
					color = (GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", buildPointInfo.allianceId, worldType) ? GameDefines.CityLabelColorType.Red : GameDefines.CityLabelColorType.Blue);
					break;
				case 4:
					color = (GameDefines.CityLabelColorType)GameEntry.Lua.CallWithReturn<int, string>("BattlefieldDsbDuelUtils.GetBattlefieldBuildingLabelColorIdByAlliance", buildPointInfo.allianceId);
					break;
				}
			}
			bool flag = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsFromBIGCHINAorUsingLangZH");
			UIWorldLabel[] array = cityLabels;
			foreach (UIWorldLabel uIWorldLabel in array)
			{
				string text2 = buildPointInfo.countryFlag;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = WorldPointManager.defaultCountryFlag;
				}
				if (flag || string.IsNullOrEmpty(text2) || buildPointInfo.IsWerewolf)
				{
					uIWorldLabel.ShowFlag(s: false);
				}
				else
				{
					uIWorldLabel.ShowFlag(s: true);
					uIWorldLabel.SetFlag(text2);
				}
				uIWorldLabel.SetNameBgSkin(buildPointInfo.titleNameSkinId);
				uIWorldLabel.SetName(n, color);
				if (buildPointInfo.GetPlayerType() == PlayerType.PlayerSelf || buildPointInfo.GetPlayerType() == PlayerType.PlayerAlliance || buildPointInfo.GetPlayerType() == PlayerType.PlayerAllianceLeader)
				{
					bool isInFireworkQuickMode = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsInFireworkQuickMode");
					uIWorldLabel.SetFireworkQuickMode(isInFireworkQuickMode, buildPointInfo.ownerUid);
				}
				else
				{
					uIWorldLabel.SetFireworkQuickMode(isInFireworkQuickMode: false, buildPointInfo.ownerUid);
				}
				if (buildPointInfo.IsWerewolf)
				{
					uIWorldLabel.SetLevel("??");
				}
				else if (buildPointInfo.level <= 0)
				{
					uIWorldLabel.SetLevel(1);
				}
				else
				{
					uIWorldLabel.SetLevel(buildPointInfo.level);
				}
			}
		}
		else if (buildPointInfo == null && _param != null && _param.buildSceneType == BuildSceneType.Fake)
		{
			UIWorldLabel[] array = cityLabels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetNameColor(GameDefines.CityLabelColorType.Green);
			}
		}
	}

	public void DoGuideStartShow(int time)
	{
		base.gameObject.SetActive(value: true);
		if (!(_grow != null))
		{
			return;
		}
		int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
		_grow.enabled = true;
		_grow.StartBuild(_param.buildUuid, build_Id, serverTimeSeconds, serverTimeSeconds + time, tileX, tileY, GameEntry.Data.Player.GetUid(), isDomeUpdate: false, isShowRobet: false);
		ShowShadow(isShow: false);
		ShowEffect(isShow: false);
		ShowGlass(upgradeDome: true);
		GameEntry.Timer.RegisterTimer(time, delegate
		{
			if (this != null && _param != null)
			{
				if (_grow.isWorking)
				{
					_grow.EndAnim();
				}
				_grow.enabled = false;
				_grow.ShowNormal();
				ShowShadow(isShow: true);
				ShowGlass(upgradeDome: false);
				DoBuildPlaceAnim();
			}
		});
	}

	protected virtual bool IsSelfControlWorkAnim()
	{
		return false;
	}

	private void SetBuildShowState(PlayerType playerType, BuildShowState state)
	{
		if (adjuster != null && !adjuster.IsMainShow())
		{
			return;
		}
		if (state == BuildShowState.Normal || (state == BuildShowState.Box && playerType != PlayerType.PlayerSelf))
		{
			if (_boxObj != null && _boxObj.activeSelf)
			{
				_boxObj.SetActive(value: false);
				if (tab_type == 4 && _level >= max_level && playerType == PlayerType.PlayerSelf && maxLevelEffect == null)
				{
					string prefabPath = "Assets/Main/Prefabs/World/Saiji/Eff_saiji_dsj_dikuai_shengji.prefab";
					maxLevelEffect = GameEntry.Resource.InstantiateAsync(prefabPath);
					maxLevelEffect.completed += delegate
					{
						SceneInterface world = SceneManager.World;
						GameObject gameObject = maxLevelEffect.gameObject;
						if (gameObject != null && _param != null && _param.point > 0 && world != null)
						{
							gameObject.transform.SetParent(world.DynamicObjNode);
							gameObject.transform.position = world.TileIndexToWorld(_param.point);
							gameObject.SetActive(value: true);
						}
						else if (gameObject != null)
						{
							gameObject.SetActive(value: false);
						}
					};
				}
			}
			if (_normalObj != null && !_normalObj.activeSelf && _epidemicSkillETime <= 0f)
			{
				_normalObj.SetActive(value: true);
			}
			if (_ruinsObj != null && _ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: false);
			}
			if (_boxAnim != null)
			{
				_boxAnim.Stop();
			}
			if (tab_type == 4)
			{
				UIWorldLabel[] array = cityLabels;
				for (int num = 0; num < array.Length; num++)
				{
					array[num].gameObject.transform.Set_localScale(0.6f, 0.6f, 0.6f);
				}
			}
			return;
		}
		switch (state)
		{
		case BuildShowState.Box:
			if (_boxObj != null && !_boxObj.activeSelf)
			{
				_boxObj.SetActive(value: true);
			}
			if (_normalObj != null && _normalObj.activeSelf)
			{
				_normalObj.SetActive(value: false);
			}
			if (_ruinsObj != null && _ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: false);
			}
			if (_boxAnim != null)
			{
				_boxAnim.Play("idle");
			}
			ShowEffect(isShow: false);
			if (tab_type == 4)
			{
				UIWorldLabel[] array = cityLabels;
				for (int num = 0; num < array.Length; num++)
				{
					array[num].gameObject.transform.Set_localScale(0.001f, 0.001f, 0.001f);
				}
			}
			break;
		case BuildShowState.Ruins:
			if (_boxObj != null && _boxObj.activeSelf)
			{
				_boxObj.SetActive(value: false);
			}
			if (_normalObj != null && _normalObj.activeSelf)
			{
				_normalObj.SetActive(value: false);
			}
			if (_ruinsObj != null && !_ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: true);
			}
			if (_boxAnim != null)
			{
				_boxAnim.Stop();
			}
			ShowEffect(isShow: false);
			if (tab_type == 4)
			{
				UIWorldLabel[] array = cityLabels;
				for (int num = 0; num < array.Length; num++)
				{
					array[num].gameObject.transform.Set_localScale(0.001f, 0.001f, 0.001f);
				}
			}
			break;
		}
	}

	public void ChangeToBox()
	{
		if (tab_type == 4)
		{
			SetBuildShowState(PlayerType.PlayerSelf, BuildShowState.Box);
		}
	}

	public virtual void OnBattleAtkUpdate(long targetUuid)
	{
	}

	public virtual void OnBattleAtkEnd()
	{
	}

	public bool ContainsPos(Vector2Int pos)
	{
		Vector2Int vector2Int = tilePos;
		Vector2Int vector2Int2 = vector2Int - Vector2Int.one * (tileX - 1);
		if (pos.x >= vector2Int2.x && pos.x <= vector2Int.x && pos.y >= vector2Int2.y)
		{
			return pos.y <= vector2Int.y;
		}
		return false;
	}

	private void ShowRobot(int startTime, int endTime, string ownerUid, int tileSizeX, int tileSizeY, long uuid)
	{
		_isShowRobot = true;
		_ownerUuid = ownerUid;
		bool num = ownerUid != GameEntry.Data.Player.Uid;
		float duration = endTime - startTime;
		if (!num)
		{
			int num2 = GameEntry.Timer.GetServerTimeSeconds() - startTime;
			if (num2 < 0)
			{
				num2 = 0;
			}
			SceneManager.World.CreateBuildRobot(uuid, base.transform.position, GetHeight(), duration, tileSizeX, tileSizeY, num2 > 0);
		}
		else
		{
			SceneManager.World.CreateOtherBuildRobot(uuid, base.transform.position, GetHeight(), duration, tileSizeX, tileSizeY);
		}
	}

	private void RemoveRobot()
	{
		if (_isShowRobot && !SceneManager.IsInCity())
		{
			_isShowRobot = false;
			if (_ownerUuid != GameEntry.Data.Player.Uid)
			{
				SceneManager.World.AddToNeedRemoveList(_param.buildUuid);
			}
			else
			{
				SceneManager.World.ChangeBuildRobotState(_param.buildUuid, WorldBuildRobot.State.GoBackRotation);
			}
			_ownerUuid = null;
		}
	}

	private void UpdateBuildDataSignal(object userData)
	{
		long num = (long)userData;
		if (_param != null && num == _param.buildUuid)
		{
			refeshDate();
			if (_normalObj != null && _normalObj.activeSelf)
			{
				UpdateCityLabel(num);
			}
		}
	}

	public void ProfileToggleGlass()
	{
		_glassVisible = !_glassVisible;
		if ((bool)_baseGlass)
		{
			_baseGlass.SetLayerRecursively(LayerMask.NameToLayer(_glassVisible ? "Default" : "Hide"));
		}
	}

	public bool IsRuins()
	{
		if (_param.buildSceneType == BuildSceneType.World)
		{
			BuildPointInfo buildInfo = GetBuildInfo();
			if (buildInfo != null)
			{
				return buildInfo.destroyStartTime > 0;
			}
		}
		return false;
	}

	public void SetMoveState(bool isHide)
	{
		base.gameObject.SetActive(!isHide);
	}

	private bool InitStatus()
	{
		bool result = false;
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo != null && buildInfo.status != null && buildInfo.status.Count > 0)
		{
			if (GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1 == 0)
			{
				SceneSkinMeta baseSkinMeta = SceneSkinManager.Instance.GetBaseSkinMeta();
				if (baseSkinMeta != null && (baseSkinMeta.IsMummyMode() || baseSkinMeta.IsDarknessMode() || baseSkinMeta.IsNineNationMode()))
				{
					GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "SEASON_MUMMY_CURSE1");
					GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE2 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "SEASON_MUMMY_CURSE2");
					GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3 = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "SEASON_MUMMY_CURSE3");
				}
			}
			long serverTime = GameEntry.Timer.GetServerTime();
			_status = new List<StatusStateBase>();
			foreach (Status item in buildInfo.status)
			{
				if (item.ExpireTime > serverTime && AddNewStatus(item, buildInfo))
				{
					result = true;
				}
			}
		}
		return result;
	}

	private bool AddNewStatus(Status data, BuildPointInfo bi)
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", data.Id, "type2");
		if (templateData.ToInt() == 26)
		{
			if (bi.aosType == AllianceOfficialSkillType.None)
			{
				if (!int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_status", data.Id, "para1").Split(new char[1] { '|' })[0], out var result))
				{
					result = 0;
				}
				if (result == bi.skinId)
				{
					BuildingPlayAnimationStatus buildingPlayAnimationStatus = new BuildingPlayAnimationStatus(data.BeginTime, data.ExpireTime, this, data.Id);
					buildingPlayAnimationStatus.Start();
					_status.Add(buildingPlayAnimationStatus);
					return true;
				}
			}
		}
		else if (templateData.ToInt() == 28)
		{
			if (bi.aosType == AllianceOfficialSkillType.None && bi.skinId > 0)
			{
				string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_decoration", bi.skinId, "act_spec_status_para");
				if (!templateData2.IsNullOrEmpty())
				{
					BuildingPlayAnimationStatus2025Easter buildingPlayAnimationStatus2025Easter = new BuildingPlayAnimationStatus2025Easter(data.BeginTime, data.ExpireTime, this, data.Id, templateData2);
					buildingPlayAnimationStatus2025Easter.Start();
					_status.Add(buildingPlayAnimationStatus2025Easter);
					ClearPlayIdleAniManager();
					return true;
				}
			}
		}
		else
		{
			if (buildModel != null && (data.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1 || data.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE2 || data.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3))
			{
				MummyCurseStatus mummyCurseStatus = new MummyCurseStatus(data, this, buildModel, _normalObj);
				mummyCurseStatus.Start();
				_status.Add(mummyCurseStatus);
				return true;
			}
			if (buildModel != null && templateData.ToInt() == 37)
			{
				WhistleStatus whistleStatus = new WhistleStatus(data, buildModel.transform);
				whistleStatus.Start();
				_status.Add(whistleStatus);
				return true;
			}
			if (templateData.ToInt() == 80)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldBasePlayDisco", bi.uuid, bi.pointIndex, data.ExpireTime);
				return false;
			}
			if (templateData.ToInt() == 81)
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldBaseFirework", bi.uuid, bi.pointIndex, data.ExpireTime);
				return false;
			}
		}
		return false;
	}

	public bool CheckPlayAnimation()
	{
		if (!(buildingAnim != null))
		{
			return buildingAnimator;
		}
		return true;
	}

	public void PlayAnimation(string animName, float normalize = 0f)
	{
		if ((adjuster != null && !adjuster.IsMainShow()) || _state == 1 || string.IsNullOrEmpty(animName))
		{
			return;
		}
		if (buildingAnim != null)
		{
			if (buildingAnim.GetState(animName) != null)
			{
				buildingAnim.Stop();
				buildingAnim.SampleAnimationAtTime(animName, normalize);
				buildingAnim.Play(animName);
			}
		}
		else if (buildingAnimator != null)
		{
			buildingAnimator.Play(animName, 0, normalize);
		}
	}

	public void PlayCrossFadeAnimation(string animName, float time = 0.1f)
	{
		if ((adjuster != null && !adjuster.IsMainShow()) || _state == 1 || string.IsNullOrEmpty(animName))
		{
			return;
		}
		if (buildingAnim != null)
		{
			if (buildingAnim.GetState(animName) != null)
			{
				buildingAnim.CrossFade(animName, time);
			}
		}
		else if (buildingAnimator != null)
		{
			float animationLength = GetAnimationLength(animName);
			buildingAnimator.CrossFade(animName, time / animationLength);
		}
	}

	public void PlayAnimationNormalizeAndCrossFade(string animName, float normalize = 0f, float time = 0f)
	{
		if ((!(adjuster != null) || adjuster.IsMainShow()) && _state != 1 && !string.IsNullOrEmpty(animName))
		{
			float animationLength = GetAnimationLength(animName);
			if (normalize * animationLength < 0.2f && time > 0f)
			{
				PlayCrossFadeAnimation(animName, time);
			}
			else
			{
				PlayAnimation(animName, normalize);
			}
		}
	}

	public void PlayAnimationEffect(string animName, float startTime = 0f)
	{
		if (buildingAniEffect != null)
		{
			buildingAniEffect.PlayAnimation(animName, startTime);
		}
	}

	public void PlayAnimationEffectAni(string animName, float startTime = 0f)
	{
		if (_aniEffectAni != null)
		{
			_aniEffectAni.PlayAnimation(animName, startTime);
		}
	}

	public void PlayTimeline(string animName, bool toIdle = false, DirectorWrapMode mode = DirectorWrapMode.Hold)
	{
		if (_timelinePlayer == null)
		{
			_timelinePlayer = base.transform.GetComponentInChildren<TimelinePlayer>();
		}
		if (_timelinePlayer != null)
		{
			_timelinePlayer.PlayTimeline(animName, toIdle, mode);
		}
	}

	public void OnEnable()
	{
		if (_playIdleAniManager != null && _playIdleAniManager.isPlaying)
		{
			_playIdleAniManager.OnStart();
		}
	}

	public void OnLodChangeFunc(int lod)
	{
		if (_playIdleAniManager != null && _playIdleAniManager.isPlaying && _playIdleAniManager.curAniTime <= 0f)
		{
			_playIdleAniManager.OnStart();
		}
	}

	public void PlayAnimationAndEffectReturnTime(string animName, float normalize = 0f, float startTime = 0f, float crossTime = 0f)
	{
		bool flag = animName == "idle";
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (_playIdleAniManager != null)
		{
			if (flag)
			{
				_playIdleAniManager.OnStart(crossTime);
				flag2 = true;
			}
			else
			{
				if (_playIdleAniManager.isPlaying)
				{
					flag3 = true;
					flag4 = _playIdleAniManager.GetCurAniNeedMix();
				}
				_playIdleAniManager.OnStop();
			}
		}
		if (!flag2)
		{
			if (flag3 && flag4 && crossTime == 0f)
			{
				crossTime = 0.2f;
			}
			PlayAnimationNormalizeAndCrossFade(animName, normalize, crossTime);
			PlayAnimationEffect(animName, startTime);
			PlayAnimationEffectAni(animName, startTime);
		}
	}

	public float PlayCrossFadeAnim(string animName, float mixTIme)
	{
		float result = 0f;
		if (adjuster != null && !adjuster.IsMainShow())
		{
			return result;
		}
		if (_state == 1)
		{
			return result;
		}
		if (!string.IsNullOrEmpty(animName))
		{
			_animName = animName;
			if (buildingAnim != null)
			{
				result = ((!(mixTIme <= 0f)) ? UIUtils.PlayCrossFadeAnimationReturnTime(buildingAnim, animName, mixTIme) : UIUtils.PlayAnimationReturnTime(buildingAnim, animName));
			}
			PlayAnimationEffect(animName);
			PlayAnimationEffectAni(animName);
		}
		return result;
	}

	public float GetAnimationLength(string animName)
	{
		if (!string.IsNullOrEmpty(animName))
		{
			if (buildingAnim != null)
			{
				SimpleAnimation.State state = buildingAnim.GetState(animName);
				if (state != null)
				{
					return state.length;
				}
			}
			else if (buildingAnimator != null)
			{
				AnimationClip[] animationClips = buildingAnimator.runtimeAnimatorController.animationClips;
				if (animationClips != null)
				{
					int i = 0;
					for (int num = animationClips.Length; i < num; i++)
					{
						AnimationClip animationClip = animationClips[i];
						if (animationClip.name == animName + "_custom")
						{
							return animationClip.length;
						}
					}
				}
			}
		}
		return 0f;
	}

	public void UpdateStatus()
	{
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo == null)
		{
			return;
		}
		HashSet<int> hashSet = new HashSet<int>();
		if (buildInfo.status != null)
		{
			for (int i = 0; i < buildInfo.status.Count; i++)
			{
				Status status = buildInfo.status[i];
				for (int j = 0; j < _status.Count; j++)
				{
					StatusStateBase statusStateBase = _status[j];
					if (!hashSet.Contains(status.Id) && statusStateBase.statusId == status.Id)
					{
						hashSet.Add(status.Id);
						break;
					}
				}
			}
		}
		List<StatusStateBase> list = new List<StatusStateBase>();
		for (int k = 0; k < _status.Count; k++)
		{
			StatusStateBase statusStateBase2 = _status[k];
			if (!hashSet.Contains(statusStateBase2.statusId))
			{
				list.Add(statusStateBase2);
			}
		}
		for (int l = 0; l < list.Count; l++)
		{
			StatusStateBase statusStateBase3 = _status[l];
			if (statusStateBase3 != null)
			{
				statusStateBase3.Dispose();
				_status.Remove(statusStateBase3);
			}
		}
		if (buildInfo.status == null)
		{
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		for (int m = 0; m < buildInfo.status.Count; m++)
		{
			Status status2 = buildInfo.status[m];
			if (status2.ExpireTime <= serverTime)
			{
				continue;
			}
			if (hashSet.Contains(status2.Id))
			{
				for (int n = 0; n < _status.Count; n++)
				{
					StatusStateBase statusStateBase4 = _status[n];
					if (statusStateBase4 != null && status2.Id == statusStateBase4.statusId && (status2.BeginTime == statusStateBase4.startTime || status2.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE1 || status2.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE2 || status2.Id == GameDefines.SeasonStatusId.SEASON_MUMMY_CURSE3))
					{
						statusStateBase4.UpdateStatus(status2);
					}
				}
			}
			else
			{
				AddNewStatus(status2, buildInfo);
			}
		}
	}

	public void RefreshMeteorite()
	{
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo == null)
		{
			DestroyMeteorite();
		}
		else if (buildInfo.crystal <= 0 && buildInfo.nucleus <= 0)
		{
			DestroyMeteorite();
		}
		else if (meteoriteComp != null)
		{
			meteoriteComp.Refresh(buildInfo.crystal, buildInfo.nucleus);
		}
		else
		{
			if (meteoriteRequest != null)
			{
				return;
			}
			meteoriteRequest = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/MeteoriteResource/CityMeteorite.prefab");
			if (meteoriteRequest == null)
			{
				return;
			}
			meteoriteRequest.completed += delegate
			{
				if (meteoriteRequest != null && !meteoriteRequest.isError && !(meteoriteRequest.gameObject == null))
				{
					Transform transform = meteoriteRequest.gameObject.transform;
					transform.SetParent(buildModel.transform);
					transform.localPosition = Vector3.zero;
					meteoriteComp = transform.GetComponent<UICityMeteorite>();
					RefreshMeteorite();
				}
			};
		}
	}

	private void DestroyMeteorite()
	{
		if (meteoriteRequest != null)
		{
			meteoriteRequest.Destroy();
			meteoriteRequest = null;
			meteoriteComp = null;
		}
	}

	public void FakeEpidemicSkill()
	{
		if (_epidemicRequest != null)
		{
			return;
		}
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("CSharpCallLuaInterface.GetEpidemicCurSkillInfo");
		_epidemicCurId = luaTable.Get<int>("skillId");
		string prefabPath = luaTable.Get<string>("prefab");
		if (luaTable.ContainsKey("eTime"))
		{
			_epidemicSkillETime = luaTable.Get<int>("eTime");
		}
		else
		{
			_epidemicSkillETime = 2.1474836E+09f;
		}
		_epidemicSkillCTime = 0f;
		_epidemicSkillETime = 2.1474836E+09f;
		if (_normalObj != null && _normalObj.activeSelf)
		{
			_normalObj.SetActive(value: false);
		}
		_epidemicRequest = GameEntry.Resource.InstantiateAsync(prefabPath);
		_epidemicRequest.completed += delegate
		{
			if (_epidemicRequest.isError)
			{
				DestroyEpidemicSkill();
			}
			else
			{
				GameObject obj = _epidemicRequest.gameObject;
				Transform obj2 = obj.transform;
				obj2.SetParent(buildModel.transform);
				obj2.localPosition = Vector3.zero;
				BattleFieldObjNew component = obj.GetComponent<BattleFieldObjNew>();
				if (component != null)
				{
					component.SetState(2);
				}
			}
		};
	}

	public void RefreshEpidemicSkill()
	{
		BuildPointInfo buildInfo = GetBuildInfo();
		if (buildInfo == null || buildInfo.quarantineSkillId <= 0)
		{
			DestroyEpidemicSkill();
			return;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (buildInfo.quarantineSkillETime < serverTime)
		{
			DestroyEpidemicSkill();
			return;
		}
		if (_epidemicCurId != buildInfo.quarantineSkillId)
		{
			DestroyEpidemicSkill();
		}
		if (_epidemicRequest != null)
		{
			return;
		}
		int skillId = buildInfo.quarantineSkillId;
		string text = GameEntry.Lua.CallWithReturn<string, int>("CSharpCallLuaInterface.GetEpidemicSkillModelPath", skillId);
		if (text.IsNullOrEmpty())
		{
			DestroyEpidemicSkill();
			return;
		}
		_epidemicCurId = buildInfo.quarantineSkillId;
		_epidemicSkillCTime = 0f;
		_epidemicSkillETime = (float)(buildInfo.quarantineSkillETime - serverTime) / 1000f;
		_epidemicRequest = GameEntry.Resource.InstantiateAsync(text);
		_epidemicRequest.completed += delegate
		{
			if (_epidemicRequest.isError)
			{
				DestroyEpidemicSkill();
			}
			else
			{
				GameObject gameObject = _epidemicRequest.gameObject;
				Transform obj = gameObject.transform;
				obj.SetParent(buildModel.transform);
				obj.localPosition = Vector3.zero;
				if (_normalObj != null && _normalObj.activeSelf)
				{
					_normalObj.SetActive(value: false);
				}
				BuildPointInfo buildInfo2 = GetBuildInfo();
				bool flag = true;
				if (buildInfo2 != null)
				{
					flag = GameEntry.Lua.CallWithReturn<bool, string, int>("CSharpCallLuaInterface.IsBattleFieldEnemy", buildInfo2.allianceId, GameEntry.Data.Player.GetWorldType());
				}
				BattleFieldObjNew component = gameObject.GetComponent<BattleFieldObjNew>();
				if (component != null)
				{
					component.SetState(flag ? 1 : 2);
				}
				GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateBattleFieldSill", Uuid, skillId, gameObject);
				RefreshEpidemicSkill();
			}
		};
	}

	private void DestroyEpidemicSkill()
	{
		if (_epidemicRequest != null)
		{
			_epidemicRequest.Destroy();
			_epidemicRequest = null;
			GameEntry.Lua.Call("CSharpCallLuaInterface.UpdateBattleFieldSill", Uuid);
		}
		_epidemicCurId = 0;
		_epidemicSkillCTime = 0f;
		_epidemicSkillETime = 0f;
		if (_normalObj != null && !_normalObj.activeSelf)
		{
			_normalObj.SetActive(value: true);
		}
	}

	public void InitDynamicModel(int skinId, Action onLoaded = null)
	{
		if (skinId > 0 && GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "is_advanced") == "1")
		{
			_dynamicLoadBuilding = new DynamicLoadBuilding();
			_dynamicLoadBuilding.Init(this, skinId, DynamicLoadBuilding.ModelType.World, _normalObj, onLoaded);
			_dynamicLoadBuilding.LoadDynamicModel();
		}
	}

	public void UnloadDynamicModel()
	{
		if (dynamicLoad)
		{
			_dynamicLoadBuilding.UnloadDynamicModel();
			_dynamicLoadBuilding = null;
		}
	}

	public void OnDynamicModelLoad(int skinId, SimpleAnimation buildingAnim, WorldBuildingAniEffect buildingAniEffect, WorldBuildingAniEffectAni buildingAniEffectAni)
	{
		this.buildingAnim = buildingAnim;
		this.buildingAniEffect = buildingAniEffect;
		_aniEffectAni = buildingAniEffectAni;
		bool flag = false;
		string text = null;
		if (skinId > 0)
		{
			text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "act_idle_status_para");
			if (!string.IsNullOrEmpty(text))
			{
				flag = true;
			}
		}
		if (flag)
		{
			if (_playIdleAniManager == null)
			{
				_playIdleAniManager = new BuildingPlayIdleAniManager();
			}
			_playIdleAniManager.InitData(text, (string s, float f) => PlayCrossFadeAnim(s, f));
			_playIdleAniManager.OnStart();
		}
		else
		{
			ClearPlayIdleAniManager();
		}
		UpdateStatus();
	}

	public float GetCurIdleAniTIme()
	{
		float result = 0f;
		if (_playIdleAniManager != null)
		{
			result = _playIdleAniManager.curAniTime;
		}
		return result;
	}

	public void SetIdleAniManagerTryToNextAni()
	{
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.TryToNextAni();
		}
	}

	public void OnDynamicModelUnload()
	{
		buildingAnim = null;
		buildingAniEffect = null;
	}
}
