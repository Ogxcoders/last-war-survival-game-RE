using System;
using System.Collections.Generic;
using System.Text;
using Protobuf;
using UnityEngine;
using XLua;

public class WorldAllianceBuilding : MonoBehaviour, ITouchPickable, ITouchObjectClickHandler, ITouchObject
{
	public enum AllianceBuildSceneType
	{
		World = 1,
		Fake
	}

	public class Param
	{
		public int buildId;

		public long buildUuid;

		public int point;

		public int serverId;

		public int tileSize;

		public PlaceBuildType BuildTopType;

		public LuaTable noPutPoint;

		public AllianceBuildSceneType buildSceneType;

		public int PositionId;

		public bool IsCreate;
	}

	[HideInInspector]
	public string previewIconPath;

	[HideInInspector]
	public string previewName;

	[HideInInspector]
	public WorldPreviewType previewType;

	public static float ScreenRangeLeft;

	public static float ScreenRangeRight;

	public static float ScreenRangeTop;

	public static float ScreenRangeDown;

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

	private UIWorldLabel[] cityLabels;

	private Vector2Int tilePos;

	private Vector3 worldPos;

	private BasementDome domeObj;

	private InstanceRequest tempInstance;

	private AutoAdjustLod adjuster;

	public int build_Id;

	public int tiles;

	private int tileX;

	private int tileY;

	private Param _param;

	private string _animName;

	private BuildingGrowEffect _grow;

	private float _minX;

	private float _maxX;

	private float _minY;

	private float _maxY;

	private bool? _canShowCityLabel;

	private string _ownerUuid;

	protected int _level;

	private int _offset_range;

	private int _state;

	private bool _glassVisible = true;

	public WorldPreviewType PreviewType => previewType;

	public Vector2Int TilePos => tilePos;

	public long Uuid { get; set; }

	float ITouchObject.Priority => 1f;

	Vector2Int ITouchObject.TilePos => Vector2Int.zero;

	public PointInfo GetBuildInfo()
	{
		if (SceneManager.World != null)
		{
			return SceneManager.World.GetPointInfoByUuid(Uuid);
		}
		return null;
	}

	public PointInfo GetPointInfo()
	{
		return GetBuildInfo();
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
		cityLabels = GetComponentsInChildren<UIWorldLabel>(includeInactive: true);
		previewType = WorldPreviewType.AllianceBuilding;
		if (_param != null)
		{
			Uuid = _param.buildUuid;
			int num = 0;
			switch (_param.buildSceneType)
			{
			case AllianceBuildSceneType.World:
				num = _param.buildId;
				break;
			case AllianceBuildSceneType.Fake:
			{
				num = _param.buildId;
				UIWorldLabel[] array = cityLabels;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].gameObject.SetActive(value: false);
				}
				GameObject gameObject = base.transform.Find("Icon/Arrow")?.gameObject;
				if (gameObject != null)
				{
					gameObject.SetActive(value: false);
				}
				break;
			}
			}
			build_Id = num;
			if (_param.BuildTopType == PlaceBuildType.CityAttachment)
			{
				string tabName = "season_builders_alliance_list";
				string str = GameEntry.ConfigCache.TryGetTemplateData(tabName, build_Id, "size_x");
				tileX = (str.IsNullOrEmpty() ? 3 : str.ToInt());
				string str2 = GameEntry.ConfigCache.TryGetTemplateData(tabName, build_Id, "size_y");
				tileY = (str2.IsNullOrEmpty() ? 3 : str2.ToInt());
				tiles = Math.Max(tileX, tileY);
			}
			else
			{
				tiles = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", build_Id, "res_size").ToInt();
				tileX = tiles;
				tileY = tiles;
			}
			refeshDate();
			if (_foldUpGo != null)
			{
				_foldUpGo.SetActive(value: false);
			}
			if (_param.buildSceneType == AllianceBuildSceneType.Fake)
			{
				if (_param.BuildTopType == PlaceBuildType.CityAttachment || _param.BuildTopType == PlaceBuildType.Build || _param.BuildTopType == PlaceBuildType.Replace)
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
			if (_param.buildId == 91003)
			{
				base.gameObject.transform.rotation = Quaternion.Euler(0f, 135 - 90 * _param.PositionId, 0f);
				GameEntry.Lua.Call("CSharpCallLuaInterface.CreateAllianceBuildSquad", _param.point, base.gameObject.transform, _param.IsCreate);
			}
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	protected internal virtual void CSUninit()
	{
		if (_param != null && _param.buildId == 91003)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveAllianceBuildSquad", _param.point);
		}
		Uuid = -1L;
		tilePos = Vector2Int.zero;
		_param = null;
		_animName = null;
		_focusEventTrigger = null;
		_grow = null;
		cityLabels = null;
	}

	public virtual void refeshDate()
	{
		if (_param != null)
		{
			int point = _param.point;
			switch (_param.buildSceneType)
			{
			case AllianceBuildSceneType.World:
				_level = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", build_Id, "level").ToInt();
				break;
			case AllianceBuildSceneType.Fake:
				point = _param.point;
				_level = 0;
				break;
			}
			_offset_range = 0;
			_param.point = point;
			if (point != 0)
			{
				SetTilePos1(_param.serverId, SceneManager.World.IndexToTilePos(point));
			}
			InitBuildingGrow();
		}
	}

	public void UpdateCityLabel(long obj)
	{
		if (Uuid != obj)
		{
			return;
		}
		string text = "";
		PointInfo pointInfoByUuid = SceneManager.World.GetPointInfoByUuid(obj);
		string text2 = null;
		if (pointInfoByUuid != null)
		{
			AllianceBuildingPointInfo allianceBuildingPointInfo = AllianceBuildingPointInfo.Parser.ParseFrom(pointInfoByUuid.extraInfo);
			if (allianceBuildingPointInfo != null)
			{
				int serverId = pointInfoByUuid.serverId;
				int srcServerId = pointInfoByUuid.srcServerId;
				int sourceServerId = GameEntry.Data.Player.GetSourceServerId();
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (srcServerId != 0 && (srcServerId != sourceServerId || serverId != sourceServerId))
				{
					stringBuilder.Append($"#{pointInfoByUuid.srcServerId} ");
					flag = true;
				}
				if (!string.IsNullOrEmpty(allianceBuildingPointInfo.AlAbbr))
				{
					stringBuilder.Append("[" + allianceBuildingPointInfo.AlAbbr + "]");
					text = allianceBuildingPointInfo.AlAbbr;
				}
				string templateData = GameEntry.ConfigCache.GetTemplateData("alliance_res_build", build_Id, "name");
				stringBuilder.Append(GameEntry.Localization.GetString(templateData));
				PlayerType playerType = pointInfoByUuid.GetPlayerType();
				GameDefines.CityLabelColorType color;
				if (playerType == PlayerType.PlayerAlliance)
				{
					color = GameDefines.CityLabelColorType.Blue;
				}
				else if (GameEntry.GlobalData.serverType == 9)
				{
					color = ((!GameEntry.Data.Player.IsAllianceSelfCamp(allianceBuildingPointInfo.AllianceId)) ? GameDefines.CityLabelColorType.Red : GameDefines.CityLabelColorType.Yellow);
				}
				else if (allianceBuildingPointInfo.AllianceId.IsNullOrEmpty())
				{
					color = ((!flag) ? GameDefines.CityLabelColorType.White : GameDefines.CityLabelColorType.Red);
				}
				else
				{
					string fightAllianceId = GameEntry.Data.Player.GetFightAllianceId();
					color = ((!fightAllianceId.IsNullOrEmpty() && fightAllianceId == allianceBuildingPointInfo.AllianceId) ? GameDefines.CityLabelColorType.Red : ((!flag) ? GameDefines.CityLabelColorType.White : GameDefines.CityLabelColorType.Red));
				}
				UIWorldLabel[] array = cityLabels;
				foreach (UIWorldLabel obj2 in array)
				{
					obj2.gameObject.SetActive(value: true);
					obj2.SetNameBgSkin();
					obj2.SetName(stringBuilder.ToString(), color);
					obj2.SetLevel(_level, color);
				}
				text2 = stringBuilder.ToString();
			}
		}
		previewName = text2;
		GameObject gameObject = base.transform.Find("Icon/Arrow")?.gameObject;
		if (!(gameObject != null))
		{
			return;
		}
		if (text.IsNullOrEmpty())
		{
			gameObject.SetActive(value: false);
			return;
		}
		NewText newText = gameObject.transform.Find("nameObj/nameBg/name")?.GetComponent<NewText>();
		if (newText != null)
		{
			newText.text = text;
			gameObject.SetActive(value: true);
		}
		else
		{
			gameObject.SetActive(value: false);
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
			int param = 0;
			if (_param != null)
			{
				param = (int)_param.BuildTopType;
			}
			SceneManager.World.touchPickablePos = GameEntry.Lua.CallWithReturn<List<int>, int, int, int>("CSharpCallLuaInterface.GetAllianceBuildTileIndex", build_Id, SceneManager.World.TilePosToIndex(tilePos + new Vector2Int((tileX - 1) / 2, (tileY - 1) / 2)), param);
			GameEntry.Event.Fire(EventId.UIPlaceAllianceBuildChangePos, worldPos);
		}
	}

	private void FromUILoad()
	{
		SceneManager.World.SetSelectedPickable(this);
		long num = 0L;
		int num2 = 0;
		PlaceBuildType placeBuildType = PlaceBuildType.Build;
		if (_param != null)
		{
			num = _param.buildUuid;
			num2 = _param.point;
			placeBuildType = _param.BuildTopType;
			SetTilePos1(_param.serverId, SceneManager.World.IndexToTilePos(num2));
			SetTouchPickAblePos();
			GameEntry.Lua.UIManager.OpenWindow("UIPlaceWorldBuild", build_Id, num, num2, (int)placeBuildType);
		}
		if (_grow != null)
		{
			_grow.enabled = false;
			_grow.ShowBuildGridSelection();
			ShowShadow(isShow: false);
			int num3 = GameEntry.Lua.CallWithReturn<int, int, int>("CSharpCallLuaInterface.IsCanPutDownByAllianceBuild", build_Id, num2);
			_grow.ShowCanPlace(num3 == 1);
		}
	}

	public void EnterMoveCityState(int moveCityType)
	{
		SceneManager.World.SetSelectedPickable(this);
		long num = 0L;
		int num2 = SceneManager.World.TilePosToIndex(GameEntry.Data.Building.GetMainPos());
		PlaceBuildType buildTopType = PlaceBuildType.MoveCity;
		if (_param != null)
		{
			Vector3 curTarget = SceneManager.World.CurTarget;
			_param.BuildTopType = buildTopType;
			num = _param.buildUuid;
			SetTilePos1(_param.serverId, SceneManager.World.WorldToTile(curTarget), curTarget);
			SetTouchPickAblePos();
			GameEntry.Lua.UIManager.OpenWindow("UIMoveCity", build_Id, num, num2, moveCityType);
		}
		if (_grow != null)
		{
			_grow.enabled = false;
			_grow.ShowBuildGridSelection();
			ShowShadow(isShow: false);
			int num3 = GameEntry.Lua.CallWithReturn<int, int, int>("CSharpCallLuaInterface.IsCanPutDownByAllianceBuild", build_Id, num2);
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
		if (_param.BuildTopType != PlaceBuildType.CityAttachment && SceneManager.World.GetTouchInputControllerEnable() && base.transform != null)
		{
			int serverIdFromWorldPos = SeasonDataManager.Instance.GetServerIdFromWorldPos(pos);
			SetTilePos1(serverIdFromWorldPos, SceneManager.World.WorldToTile(pos), pos);
			int param = SceneManager.World.WorldToTileIndex(pos);
			PutState putState = PutState.None;
			putState = (PutState)GameEntry.Lua.CallWithReturn<int, int, int>("CSharpCallLuaInterface.IsCanPutDownByAllianceBuild", build_Id, param);
			_grow.ShowCanPlace(putState == PutState.Ok);
			UIWorldLabel[] array = cityLabels;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(value: false);
			}
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
		if (!(_grow == null) && _param != null)
		{
			PlayerType playType = PlayerType.PlayerSelf;
			GameEntry.Timer.GetServerTime();
			AllianceBuildSceneType buildSceneType = _param.buildSceneType;
			_ = 2;
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

	public bool OnClick()
	{
		if (SceneManager.IsInWorld())
		{
			int param = SceneManager.World.TilePosToIndex(tilePos);
			if (_param.tileSize > 5)
			{
				Vector2Int touchTilePos = SceneManager.World.GetTouchTilePos();
				param = SceneManager.World.TilePosToIndex(touchTilePos);
			}
			GameEntry.Lua.Call("UIUtil.OnClickWorld", param, 1);
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

	public bool ContainsPos(Vector2Int pos)
	{
		Vector2Int vector2Int = tilePos;
		Vector2Int vector2Int2 = vector2Int - Vector2Int.one * (tiles - 1);
		if (pos.x >= vector2Int2.x && pos.x <= vector2Int.x && pos.y >= vector2Int2.y)
		{
			return pos.y <= vector2Int.y;
		}
		return false;
	}

	public void ProfileToggleGlass()
	{
		_glassVisible = !_glassVisible;
		if ((bool)_baseGlass)
		{
			_baseGlass.SetLayerRecursively(LayerMask.NameToLayer(_glassVisible ? "Default" : "Hide"));
		}
	}
}
