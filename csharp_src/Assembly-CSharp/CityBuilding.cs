using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class CityBuilding : MonoBehaviour, ITouchPickable, ITouchObjectClickHandler, ITouchObject, ITouchObjectBeginLongTabHandler, ITouchObjectEndLongTabHandler, IDynamicLoadBuilding
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

		public PlaceBuildType BuildTopType;

		public LuaTable noPutPoint;

		public BuildSceneType buildSceneType;

		public bool noDoAnim;

		public bool visible;
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

	private enum BuildShowState
	{
		Normal,
		Box,
		Ruins,
		Upgrade
	}

	public static float ScreenRangeLeft;

	public static float ScreenRangeRight;

	public static float ScreenRangeTop;

	public static float ScreenRangeDown;

	[SerializeField]
	private GameObject buildIcon;

	[SerializeField]
	protected GameObject buildModel;

	[SerializeField]
	private GameObject _shadow;

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
	private SuperTextMesh _levelText;

	[SerializeField]
	private GameObject _upgradeObj;

	private GameObject _buildUpLevelTip;

	private GameObject _buildModel;

	private DynamicLoadBuilding _dynamicLoadBuilding;

	private Vector2Int tilePos;

	public int build_Id;

	public int tileX;

	public int tileY;

	public int scan;

	public int build_type;

	private Param _param;

	private string _animName;

	private BuildingGrowEffect _grow;

	private float _minX;

	private float _maxX;

	private float _minY;

	private float _maxY;

	private bool _isShowRobot;

	private string _ownerUuid;

	protected int _level;

	private int _state;

	private bool _canDoAnim = true;

	private CityBuildingSwitchBase _cityBuildingSwitch;

	private ActivityAlarmClockCityBuildingEffect _activityAlarmClockCityBuildingEffect;

	private BuildingPlayIdleAniManager _playIdleAniManager;

	private WorldBuildingAniEffect buildingAniEffect;

	private List<CityBuildingDecorationAnimBase> _decorationAnims;

	public WorldPreviewType PreviewType => WorldPreviewType.Default;

	private bool dynamicLoad => _dynamicLoadBuilding != null;

	public Vector2Int TilePos
	{
		get
		{
			return tilePos;
		}
		set
		{
			SetTilePos1(value, SceneManager.World.TileToWorld(value));
		}
	}

	public long Uuid { get; set; }

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

	public void SetBuildUpLevelTipActive(bool active)
	{
		if ((bool)_buildUpLevelTip)
		{
			_buildUpLevelTip.SetActive(active);
		}
	}

	public BuildPointInfo GetBuildInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfoByUuid(Uuid) as BuildPointInfo;
		}
		return null;
	}

	public PointInfo GetPointInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfoByUuid(Uuid);
		}
		return null;
	}

	private void Awake()
	{
		if (base.transform != null)
		{
			Transform transform = base.transform.Find("ModelGo/BuildUpLevelTip");
			if (transform != null)
			{
				_buildUpLevelTip = transform.gameObject;
			}
			buildingAniEffect = base.transform.GetComponentInChildren<WorldBuildingAniEffect>();
		}
	}

	protected internal virtual void CSInit(object userData)
	{
		build_Id = 0;
		_level = 0;
		_grow = base.gameObject.GetComponent<BuildingGrowEffect>();
		_minX = ScreenRangeLeft;
		_maxX = (float)Screen.width - ScreenRangeRight;
		_minY = ScreenRangeDown;
		_maxY = (float)Screen.height - ScreenRangeTop;
		_param = userData as Param;
		_isShowRobot = false;
		_decorationAnims = new List<CityBuildingDecorationAnimBase>();
		if (_normalObj != null)
		{
			_buildModel = _normalObj;
			_buildModel.SetActive(value: true);
		}
		else
		{
			Debug.LogError("_normalObj is null, " + base.name);
		}
		if (_param != null)
		{
			SetCanDoAnim(!_param.noDoAnim);
			Uuid = _param.buildUuid;
			int num = 0;
			switch (_param.buildSceneType)
			{
			case BuildSceneType.City:
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid == null)
				{
					break;
				}
				num = buildingDataByUuid.buildId;
				switch (num)
				{
				case 770000:
				{
					List<GameObject> list = new List<GameObject> { _normalObj };
					Transform transform = base.transform.Find("ModelGo/Normal_fire");
					bool flag = true;
					if (transform != null)
					{
						transform.gameObject.SetActive(value: false);
						list.Add(transform.gameObject);
					}
					else
					{
						flag = false;
					}
					transform = base.transform.Find("ModelGo/Normal_overload");
					if (transform != null)
					{
						transform.gameObject.SetActive(value: false);
						list.Add(transform.gameObject);
					}
					else
					{
						flag = false;
					}
					if (flag)
					{
						_cityBuildingSwitch = new PersonalFurnaceSwitch();
						_cityBuildingSwitch.Init(list, new List<EventId> { EventId.BUILDING_FURNACE_DATA_UPDATE }, BuildModelChange);
					}
					break;
				}
				case 10224000:
					AddActivityAlarmClockEffect();
					break;
				}
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
			scan = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "scan").ToInt();
			build_type = GameEntry.ConfigCache.GetTemplateData("building", build_Id, "build_type").ToInt();
			refeshDate();
			if (_foldUpGo != null)
			{
				_foldUpGo.SetActive(value: false);
			}
			CheckPlaced();
			if (_param.buildSceneType == BuildSceneType.Fake)
			{
				FromUILoad();
			}
			if (num == 808000 || num == 809000 || num == 810000 || num == 811000)
			{
				BasementCollider[] componentsInChildren = GetComponentsInChildren<BasementCollider>(includeInactive: true);
				foreach (BasementCollider basementCollider in componentsInChildren)
				{
					if (basementCollider != null)
					{
						basementCollider.CSInit(this, num, _param.point);
					}
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
			SetVisible(_param.visible);
		}
		else
		{
			SetVisible(visible: false);
		}
		int skinId = InitPlayIdleAniManager();
		InitDynamicModel(skinId);
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.OnStart();
		}
	}

	protected internal virtual void CSUninit()
	{
		UnloadDynamicModel();
		_cityBuildingSwitch?.Release();
		Uuid = -1L;
		tilePos = Vector2Int.zero;
		_param = null;
		_animName = null;
		_focusEventTrigger = null;
		_grow = null;
		_buildModel = null;
		_activityAlarmClockCityBuildingEffect = null;
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.ClearAllData();
			_playIdleAniManager = null;
		}
		if (_decorationAnims == null)
		{
			return;
		}
		for (int i = 0; i < _decorationAnims.Count; i++)
		{
			if (_decorationAnims[i] != null)
			{
				_decorationAnims[i].Dispose();
			}
		}
		_decorationAnims.Clear();
	}

	protected internal virtual void CSUpdate(float elapseSeconds)
	{
		if (_playIdleAniManager != null)
		{
			_playIdleAniManager.OnUpdate();
		}
		if (_decorationAnims == null || _decorationAnims.Count <= 0)
		{
			return;
		}
		List<CityBuildingDecorationAnimBase> list = new List<CityBuildingDecorationAnimBase>();
		for (int i = 0; i < _decorationAnims.Count; i++)
		{
			if (_decorationAnims[i] != null && _decorationAnims[i].Update(elapseSeconds) == CityBuildingDecorationAnimBase.StateType.Finish)
			{
				_decorationAnims[i].Dispose();
				list.Add(_decorationAnims[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			_decorationAnims.Remove(list[j]);
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
			case BuildSceneType.Fake:
			{
				LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
				if (buildingDataByUuid != null && buildingDataByUuid.buildId == 10100000)
				{
					num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCityMainSkinId");
				}
				break;
			}
			case BuildSceneType.World:
			{
				BuildPointInfo buildInfo = GetBuildInfo();
				if (buildInfo != null)
				{
					num = buildInfo.GetSkinId();
				}
				break;
			}
			}
			if (num > 0 && _param.buildSceneType != BuildSceneType.Fake)
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
		else if (_playIdleAniManager != null)
		{
			_playIdleAniManager.ClearAllData();
			_playIdleAniManager = null;
		}
		return num;
	}

	public void OnEnable()
	{
		if (_playIdleAniManager != null && _playIdleAniManager.isPlaying)
		{
			_playIdleAniManager.OnStart();
		}
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
		_param.point = num;
		if (num != 0)
		{
			SetTilePos1(SceneManager.World.IndexToTilePos(num), SceneManager.World.TileIndexToWorld(num));
		}
		InitBuildingGrow();
		SetLevelText();
	}

	private void SetTilePos1(Vector2Int pos, Vector3 worldpos)
	{
		if (pos != tilePos)
		{
			tilePos = pos;
			base.transform.position = SceneManager.World.TileToWorld(tilePos);
			SetTouchPickAblePos();
		}
	}

	private void SetTouchPickAblePos()
	{
		if (SceneManager.World.SelectBuild == this)
		{
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, int, int>("CSharpCallLuaInterface.GetBuildTileIndex", build_Id, SceneManager.World.TilePosToIndex(tilePos));
			GameEntry.Event.Fire(EventId.UIPlaceBuildChangePos, SceneManager.World.TilePosToIndex(tilePos));
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
			SetTilePos1(SceneManager.World.IndexToTilePos(num2), SceneManager.World.TileIndexToWorld(num2));
			SetTouchPickAblePos();
			GameEntry.Lua.UIManager.OpenWindow("UIPlaceBuild", build_Id, num, num2, (int)placeBuildType);
		}
		if (_grow != null)
		{
			_grow.enabled = false;
			_grow.ShowBuildGridSelection();
			ShowShadow(isShow: false);
			ShowEffect(isShow: false);
			int num3 = GameEntry.Lua.CallWithReturn<int, int, int, long, LuaTable>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, num2, num, param);
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
			SetTilePos1(SceneManager.World.WorldToTile(pos), pos);
			int param = SceneManager.World.WorldToTileIndex(pos);
			PutState putState = PutState.None;
			putState = (PutState)((_param == null) ? GameEntry.Lua.CallWithReturn<int, int, int, long, List<int>>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, param, 0L, null) : GameEntry.Lua.CallWithReturn<int, int, int, long, List<int>>("CSharpCallLuaInterface.IsCanPutDownByBuild", build_Id, param, _param.buildUuid, null));
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
		PlayerType playType = PlayerType.PlayerSelf;
		QueueState queueState = QueueState.DEFAULT;
		int num = 0;
		long serverTime = GameEntry.Timer.GetServerTime();
		switch (_param.buildSceneType)
		{
		case BuildSceneType.City:
		{
			playType = PlayerType.PlayerSelf;
			_ = GameEntry.Data.Player.Uid;
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
					_ = luaTable.Get<long>("startTime") / 1000;
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
			playType = buildInfo.GetPlayerType();
			_ = buildInfo.ownerUid;
			if (buildInfo.destroyStartTime > 0)
			{
				_ = buildInfo.startTime;
				num = buildInfo.endTime;
				queueState = QueueState.Ruins;
				break;
			}
			if (buildInfo.endTime > 0)
			{
				_ = buildInfo.startTime;
				num = buildInfo.endTime;
				queueState = QueueState.UPGRADE;
				break;
			}
			_ = buildInfo.startTime;
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
				Mathf.RoundToInt((float)buildingParam.startTime / 1000f);
				num = Mathf.RoundToInt((float)buildingParam.endTime / 1000f);
			}
			GameEntry.BuildAnimatorManager.RemoveOneBuild(_param.point);
			break;
		}
		}
		bool flag = false;
		switch (queueState)
		{
		case QueueState.UPGRADE:
			if (build_type == 2)
			{
				DoBuildPlaceAnim();
				_grow.ShowNormal(playType);
				ShowShadow(isShow: true);
				SetBuildShowState(BuildShowState.Normal);
			}
			else if (!flag)
			{
				if (_grow.isWorking)
				{
					_grow.EndAnim();
				}
				if (_grow.enabled)
				{
					_grow.enabled = false;
				}
				_grow.ShowNormal(playType);
				ShowShadow(isShow: true);
				ShowEffect(isShow: false);
				if (num > serverTime / 1000)
				{
					SetBuildShowState(BuildShowState.Upgrade);
				}
				else
				{
					SetBuildShowState(BuildShowState.Box);
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
			_grow.ShowNormal(playType);
			ShowShadow(isShow: true);
			SetBuildShowState(BuildShowState.Normal);
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
			_grow.ShowNormal(playType);
			ShowShadow(isShow: true);
			SetBuildShowState(BuildShowState.Normal);
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
			_grow.ShowNormal(playType);
			ShowShadow(isShow: true);
			SetBuildShowState(BuildShowState.Ruins);
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
			_grow.ShowNormal(playType);
			ShowShadow(isShow: true);
			SetBuildShowState(BuildShowState.Normal);
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
			_grow.ShowNormal(playType);
			ShowShadow(isShow: true);
			SetBuildShowState(BuildShowState.Normal);
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
		Vector3 worldPos = SceneManager.World.WorldToScreenPoint(pos);
		float x = worldPos.x;
		float y = worldPos.y;
		if (x < _minX)
		{
			worldPos.x = _minX;
		}
		else if (x > _maxX)
		{
			worldPos.x = _maxX;
		}
		if (y < _minY)
		{
			worldPos.y = _minY;
		}
		else if (y > _maxY)
		{
			worldPos.y = _maxY;
		}
		return SceneManager.World.ScreenPointToWorld(worldPos);
	}

	public bool HasAnimClip(string animName)
	{
		return false;
	}

	public float PlayAnim(string animName)
	{
		float result = 0f;
		if (!_canDoAnim)
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
		return result;
	}

	public void PlayAnimationEffect(string animName, float startTime = 0f)
	{
		if (buildingAniEffect != null)
		{
			buildingAniEffect.PlayAnimation(animName, startTime);
		}
	}

	public float PlayCrossFadeAnim(string animName, float mixTIme)
	{
		float result = 0f;
		if (!_canDoAnim)
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
				result = ((!(mixTIme <= 0f)) ? UIUtils.PlayCrossFadeAnimationReturnTime(simpleAnim, animName, mixTIme) : UIUtils.PlayAnimationReturnTime(simpleAnim, animName));
			}
			PlayAnimationEffect(animName);
		}
		return result;
	}

	public void DoBuildClickAnim()
	{
	}

	public void DoBuildPlaceAnim()
	{
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
			int param = SceneManager.World.TilePosToIndex(tilePos);
			GameEntry.Lua.Call("UIUtil.OnClickWorld", param, 1);
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

	public float GetHeight()
	{
		if (_grow != null)
		{
			return _grow.GetHeight();
		}
		return 0f;
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

	protected virtual bool IsSelfControlWorkAnim()
	{
		return false;
	}

	private void SetBuildShowState(BuildShowState state)
	{
		switch (state)
		{
		case BuildShowState.Normal:
			if (_buildModel != null && !_buildModel.activeSelf)
			{
				_buildModel.SetActive(value: true);
			}
			if (_boxObj != null && _boxObj.activeSelf)
			{
				_boxObj.SetActive(value: false);
			}
			if (_ruinsObj != null && _ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: false);
			}
			if (_upgradeObj != null && _upgradeObj.activeSelf)
			{
				_upgradeObj.SetActive(value: false);
			}
			break;
		case BuildShowState.Box:
			if (_boxObj != null)
			{
				ShowBoxAndBoxAnim(showOpenAnim: false);
			}
			if (_buildModel != null && _buildModel.activeSelf)
			{
				_buildModel.SetActive(value: false);
			}
			if (_ruinsObj != null && _ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: false);
			}
			if (_upgradeObj != null && _upgradeObj.activeSelf)
			{
				_upgradeObj.SetActive(value: false);
			}
			ShowEffect(isShow: false);
			break;
		case BuildShowState.Ruins:
			if (_ruinsObj != null && !_ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: true);
			}
			if (_boxObj != null && _boxObj.activeSelf)
			{
				_boxObj.SetActive(value: false);
			}
			if (_buildModel != null && _buildModel.activeSelf)
			{
				_buildModel.SetActive(value: false);
			}
			if (_upgradeObj != null && _upgradeObj.activeSelf)
			{
				_upgradeObj.SetActive(value: false);
			}
			ShowEffect(isShow: false);
			break;
		case BuildShowState.Upgrade:
			if (_upgradeObj != null && !_upgradeObj.activeSelf)
			{
				_upgradeObj.SetActive(value: true);
			}
			if (_boxObj != null && _boxObj.activeSelf)
			{
				_boxObj.SetActive(value: false);
			}
			if (_buildModel != null && !_buildModel.activeSelf)
			{
				_buildModel.SetActive(value: true);
			}
			if (_ruinsObj != null && _ruinsObj.activeSelf)
			{
				_ruinsObj.SetActive(value: false);
			}
			ShowEffect(isShow: false);
			break;
		}
	}

	public void ChangeToBox()
	{
		if (_buildModel != null && _buildModel.activeSelf)
		{
			_buildModel.SetActive(value: false);
		}
		if (_upgradeObj != null && _upgradeObj.activeSelf)
		{
			_upgradeObj.SetActive(value: false);
		}
		if (_boxObj != null)
		{
			ShowBoxAndBoxAnim(showOpenAnim: true);
		}
		ShowEffect(isShow: false);
	}

	public virtual void OnBattleAtkUpdate(long targetUuid)
	{
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

	private void SetLevelText()
	{
		if (_levelText != null)
		{
			_levelText.text = GameEntry.Localization.GetString("300665", _level);
		}
	}

	public void SetCanDoAnim(bool canDoAnim)
	{
		_canDoAnim = canDoAnim;
	}

	public void SetVisible(bool visible)
	{
		base.gameObject.SetActive(visible);
	}

	private void BuildModelChange(GameObject model)
	{
		if (model != null)
		{
			bool active = true;
			if (_buildModel != null)
			{
				active = _buildModel.activeSelf;
				_buildModel.SetActive(value: false);
			}
			_buildModel = model;
			_buildModel.SetActive(active);
		}
	}

	public void AddActivityAlarmClockEffect()
	{
		bool isShowServerTime = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetActivityAlarmClockShowServerTimeMode");
		_activityAlarmClockCityBuildingEffect = base.gameObject.GetComponent<ActivityAlarmClockCityBuildingEffect>();
		if (_activityAlarmClockCityBuildingEffect == null)
		{
			_activityAlarmClockCityBuildingEffect = base.gameObject.AddComponent<ActivityAlarmClockCityBuildingEffect>();
		}
		_activityAlarmClockCityBuildingEffect.UpdateData(isShowServerTime);
	}

	public void UpdateActivityAlarmClockTimeShow(bool isShowServerTime)
	{
		if (_activityAlarmClockCityBuildingEffect != null && build_Id == 10224000)
		{
			_activityAlarmClockCityBuildingEffect.UpdateData(isShowServerTime);
		}
	}

	private void ShowBoxAndBoxAnim(bool showOpenAnim)
	{
		if (_boxObj == null)
		{
			return;
		}
		SoftReferencePrefab componentInChildren = _boxObj.GetComponentInChildren<SoftReferencePrefab>();
		bool flag = componentInChildren != null;
		if (!_boxObj.activeSelf)
		{
			if (flag)
			{
				componentInChildren.LoadedCallback = (Action<GameObject>)Delegate.Combine(componentInChildren.LoadedCallback, (Action<GameObject>)delegate(GameObject go)
				{
					SimpleAnimation componentInChildren2 = go.GetComponentInChildren<SimpleAnimation>();
					if (showOpenAnim)
					{
						componentInChildren2.Play("open");
						componentInChildren2.PlayQueued("idle");
						if (!GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsSceneCameraDisable"))
						{
							GameEntry.Lua.Call("CSharpCallLuaInterface.PlaySound", 62236, param2: false);
						}
					}
					else
					{
						componentInChildren2.Play("idle");
					}
				});
			}
			_boxObj.SetActive(value: true);
		}
		else if (flag || _boxAnim == null)
		{
			_boxAnim = _boxObj.GetComponentInChildren<SimpleAnimation>();
		}
		if (_boxAnim != null)
		{
			if (showOpenAnim)
			{
				_boxAnim.Play("open");
				_boxAnim.PlayQueued("idle");
			}
			else
			{
				_boxAnim.Play("idle");
			}
		}
	}

	public void PlayBoxShake()
	{
		if (!(_boxObj == null))
		{
			SimpleAnimation componentInChildren = _boxObj.GetComponentInChildren<SimpleAnimation>();
			if (componentInChildren != null)
			{
				componentInChildren.Play("idle_fresnel");
			}
		}
	}

	public GameObject GetUpgradeObj()
	{
		return _upgradeObj;
	}

	public void InitDynamicModel(int skinId, Action onLoaded = null)
	{
		if (skinId > 0 && GameEntry.ConfigCache.GetTemplateData("lw_decoration", skinId, "is_advanced") == "1")
		{
			_dynamicLoadBuilding = new DynamicLoadBuilding();
			_dynamicLoadBuilding.Init(this, skinId, DynamicLoadBuilding.ModelType.City, _normalObj, onLoaded);
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

	private bool InitDecorationAnims()
	{
		if (_decorationAnims == null)
		{
			_decorationAnims = new List<CityBuildingDecorationAnimBase>();
		}
		else
		{
			_decorationAnims.Clear();
		}
		if (_param == null)
		{
			return false;
		}
		bool result = false;
		if (_param.buildSceneType == BuildSceneType.City)
		{
			LuaBuildData buildingDataByUuid = GameEntry.Data.Building.GetBuildingDataByUuid(_param.buildUuid);
			if (buildingDataByUuid != null)
			{
				int num = 0;
				if (buildingDataByUuid.buildId == 10100000)
				{
					num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCityMainSkinId");
				}
				if (num > 0)
				{
					int num2 = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "status_id").ToInt();
					if (num2 > 0)
					{
						StatusType2 statusType = (StatusType2)GameEntry.ConfigCache.GetTemplateData("lw_status", num2, "type2").ToInt();
						if (statusType == StatusType2.SkinAnimationType2025Easter)
						{
							string templateData = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "act_spec_status_para");
							if (!templateData.IsNullOrEmpty())
							{
								CityBuildingDecorationAnim2025Easter cityBuildingDecorationAnim2025Easter = new CityBuildingDecorationAnim2025Easter(this, num, templateData, num2);
								cityBuildingDecorationAnim2025Easter.Start();
								_decorationAnims.Add(cityBuildingDecorationAnim2025Easter);
								result = true;
								if (_playIdleAniManager != null)
								{
									_playIdleAniManager.ClearAllData();
									_playIdleAniManager = null;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	public float GetAnimationLength(string animName)
	{
		if (!string.IsNullOrEmpty(animName) && simpleAnim != null)
		{
			SimpleAnimation.State state = simpleAnim.GetState(animName);
			if (state != null)
			{
				return state.length;
			}
		}
		return 0f;
	}

	public bool CheckPlayAnimation()
	{
		return simpleAnim != null;
	}

	public void OnDynamicModelLoad(int skinId, SimpleAnimation buildingAnim, WorldBuildingAniEffect buildingAniEffect, WorldBuildingAniEffectAni buildingAniEffectAni)
	{
		simpleAnim = buildingAnim;
		this.buildingAniEffect = buildingAniEffect;
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
		else if (_playIdleAniManager != null)
		{
			_playIdleAniManager.ClearAllData();
			_playIdleAniManager = null;
		}
		InitDecorationAnims();
	}

	public void OnDynamicModelUnload()
	{
		simpleAnim = null;
		buildingAniEffect = null;
	}
}
