using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

public class WorldInputManager : WorldManagerBase
{
	private static readonly float m_LoadDelay = 0.15f;

	protected TouchInputController touchInput;

	protected bool isLongTabStart;

	protected bool isLongTabEnd;

	protected bool isFingerDownUIObject;

	public long marchUuid;

	public long formationUuid;

	public int formationPointId;

	protected BasementTower _tower;

	protected List<ITouchObject> touchObjects = new List<ITouchObject>();

	protected List<ITouchObject> touchObjectsMulti = new List<ITouchObject>();

	protected List<ITouchObject> touchObjectsHighestPriorityCache = new List<ITouchObject>();

	protected DesertEventTrigger desertTrigger = new DesertEventTrigger();

	protected ITouchObject touchEnterObj;

	protected ITouchObject touchPress;

	protected ITouchObject touchDrag;

	protected ITouchObject touchLongTab;

	protected bool _isShowLoad;

	protected bool _isClickSelectedPickable;

	protected ITouchPickable selectedPickable;

	public List<int> touchPickablePos;

	protected InstanceRequest touchEffectReq;

	protected bool touchEffectVisible;

	protected GameObject touchEffect;

	protected GameObject touchEffectBuildGird;

	protected GameObject touchEffectBuildGirdSeason;

	protected bool _isCanTouch = true;

	protected InstanceRequest QuanEffectRangeReq;

	protected bool QuanEffectRangeVisible;

	protected SimpleAnimation BlankAnimator;

	protected InstanceRequest LongPressEffectReq;

	protected bool LongPressEffectVisible;

	protected GameObject LongPressEffectGo;

	protected int lodCache;

	protected int _curDragIndex;

	protected LuaTable uiParam;

	protected static readonly int BlankTitleEffectMaxLod = 2;

	private int initFrameCount = -1;

	public int curIndex { get; set; }

	public Vector2Int curTouchTile { get; set; }

	public Vector3 curTouchPoint { get; set; }

	public ITouchPickable SelectBuild
	{
		get
		{
			return selectedPickable;
		}
		set
		{
			selectedPickable = value;
		}
	}

	public WorldInputManager(WorldScene scene)
		: base(scene)
	{
		touchPickablePos = new List<int>();
		_isCanTouch = true;
		QuanEffectRangeVisible = false;
		LongPressEffectVisible = false;
	}

	public override void Init()
	{
		isLongTabStart = false;
		isLongTabEnd = false;
		isFingerDownUIObject = false;
		formationUuid = 0L;
		formationPointId = 0;
		_curDragIndex = 0;
		touchInput = world.Camera.TouchInputController;
		touchInput.OnFingerDown += OnFingerDown;
		touchInput.OnFingerUp += OnFingerUp;
		touchInput.OnDragStart += OnDragStart;
		touchInput.OnDragUpdate += OnDragUpdate;
		touchInput.OnDragStop += OnDragStop;
		touchInput.OnInputClick += OnInputClick;
		initFrameCount = Time.frameCount;
		touchInput.OnLongTapProgress += OnLongTapProgress;
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnUpdateLod);
		uiParam = GameEntry.Lua.Env.NewTable();
		uiParam.Set("anim", value: true);
	}

	public override void UnInit()
	{
		base.UnInit();
		touchInput.OnFingerDown -= OnFingerDown;
		touchInput.OnFingerUp -= OnFingerUp;
		touchInput.OnDragStart -= OnDragStart;
		touchInput.OnDragUpdate -= OnDragUpdate;
		touchInput.OnDragStop -= OnDragStop;
		touchInput.OnInputClick -= OnInputClick;
		touchInput.OnLongTapProgress -= OnLongTapProgress;
		if (QuanEffectRangeReq != null)
		{
			QuanEffectRangeReq.Destroy();
			QuanEffectRangeReq = null;
			BlankAnimator = null;
		}
		if (LongPressEffectReq != null)
		{
			LongPressEffectReq.Destroy();
			LongPressEffectReq = null;
			LongPressEffectGo = null;
		}
		if (touchEffectReq != null)
		{
			touchEffectReq.Destroy();
			touchEffectReq = null;
		}
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnUpdateLod);
		touchObjects.Clear();
		touchObjectsMulti.Clear();
		touchObjectsHighestPriorityCache.Clear();
		if (desertTrigger != null)
		{
			desertTrigger.onPointerClick = null;
			desertTrigger.previewName = null;
			desertTrigger.previewIconPath = null;
		}
	}

	protected void OnUpdateLod(object lodObj)
	{
		int num = (int)lodObj;
		if (lodCache <= BlankTitleEffectMaxLod && num > BlankTitleEffectMaxLod)
		{
			if (QuanEffectRangeVisible)
			{
				HideBlankTitleEffect();
			}
			if (LongPressEffectVisible)
			{
				HideLongPressEffect();
			}
		}
		lodCache = num;
	}

	public void SetSelectedPickable(ITouchPickable pickable)
	{
		_isClickSelectedPickable = true;
		selectedPickable = pickable;
	}

	public void DragSelectedPickable(Vector3 position)
	{
		selectedPickable?.Drag(position);
	}

	private void OnLongTapProgress(float progress)
	{
		if (!isFingerDownUIObject)
		{
			if (progress >= 1f && !isLongTabEnd)
			{
				isLongTabEnd = true;
				OnLongTabEnd();
			}
			else if (progress > m_LoadDelay && !isLongTabStart)
			{
				isLongTabStart = true;
				OnLongTapStart();
			}
		}
	}

	private void OnLongTapStart()
	{
		if (CanUseInput())
		{
			if (GameEntry.Lua.UIManager.IsWindowOpen("UIWorldBlackTile"))
			{
				GameEntry.Lua.UIManager.DestroyWindow("UIWorldBlackTile");
			}
			if (!TouchObjectEvent.ExecuteBeginLongTab(touchLongTab) && world.IsTileWalkable(curTouchPoint) && lodCache <= BlankTitleEffectMaxLod)
			{
				ShowLongPressEffect(curIndex);
			}
		}
	}

	private void OnLongTabEnd()
	{
		if (CanUseInput())
		{
			HideLoad();
			DCPlayer player = GameEntry.Data.Player;
			if (player.IsInBattleField(3) || player.IsInBattleField(2))
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.OpenPingSign", curIndex);
			}
			TouchObjectEvent.ExecuteEndLongTab(touchLongTab);
		}
	}

	private void ClickWorld()
	{
		Vector3 vector = curTouchPoint;
		_ = curTouchTile;
		PointInfo pointInfo = SceneManager.World.GetPointInfo(curIndex);
		world.marchUuid = 0L;
		bool flag = false;
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.IsDesertMode())
		{
			flag = !GameEntry.Data.Player.IsInBattleField();
		}
		if (touchPress != null)
		{
			if (SceneManager.World.IsTileWalkable(vector) && lodCache <= BlankTitleEffectMaxLod && pointInfo == null)
			{
				if (flag)
				{
					GameEntry.Lua.Call("UIUtil.OnClickWorld", curIndex, 0);
				}
				else
				{
					GameEntry.Lua.UIManager.OpenWindow("UIWorldBlackTile", uiParam, curIndex.ToString());
				}
				ShowTouchEffect(vector, flag);
			}
			else
			{
				GameEntry.Lua.Call("UIUtil.OnClickWorld", curIndex, 1);
				HideTouchEffect();
			}
		}
		else
		{
			if (world != null && world.Camera != null && world.Camera.GetCurrentCameraState() == MobileTouchCamera.State.MoveTo)
			{
				return;
			}
			if (pointInfo == null || pointInfo.pointType == WorldPointType.Other)
			{
				GameEntry.Lua.Call("UIUtil.OnClickWorld", curIndex, 0);
				ShowTouchEffect(vector, flag);
			}
			else
			{
				GameEntry.Lua.Call("UIUtil.OnClickWorld", pointInfo.mainIndex, 1);
				HideTouchEffect();
			}
		}
		if (pointInfo != null && pointInfo.CheckClickIsValid(lodCache))
		{
			return;
		}
		Vector3 vector2 = vector;
		if (lodCache >= 3)
		{
			if (curIndex <= 0)
			{
				Vector2Int vector2Int = world.ClampTilePos(curTouchTile);
				int serverIdFromWorldPos = SeasonDataManager.Instance.GetServerIdFromWorldPos(vector2);
				world.AutoLookat(TileCoord.TileFloatToWorld(vector2Int, serverIdFromWorldPos), world.InitZoom);
			}
			else
			{
				world.AutoLookat(vector2, world.InitZoom);
			}
		}
		else if (pointInfo != null && GameEntry.Data.Player.IsInBattleField())
		{
			vector2 = world.TileIndexToWorld(pointInfo.mainIndex);
			if (pointInfo.pointType == WorldPointType.WorldResource)
			{
				vector2.z -= 1f;
			}
			else
			{
				vector2.z -= 5f;
			}
			world.AutoLookat(vector2);
		}
	}

	private void OnInputClick(Vector3 clickPosition, bool isDoubleClick, bool isLongTap)
	{
		if (isLongTap || isFingerDownUIObject)
		{
			return;
		}
		if (initFrameCount > 0)
		{
			int frameCount = Time.frameCount;
			if (frameCount <= 0)
			{
				initFrameCount = -1;
			}
			else
			{
				if (frameCount - initFrameCount < 2)
				{
					return;
				}
				initFrameCount = -1;
			}
		}
		Vector3 touchPoint = SceneManager.World.GetTouchPoint();
		selectedPickable?.Drag(touchPoint);
		int num = SceneManager.World.WorldToTileIndex(touchPoint);
		GameEntry.Event.Fire(EventId.OnWorldInputPointClick, num);
		if (CanUseInput())
		{
			if (isDoubleClick)
			{
				TouchObjectEvent.ExecuteDoubleClick(touchPress);
			}
			else if (touchObjectsHighestPriorityCache.Count > 0)
			{
				TouchObjectEvent.ExecuteClick(touchObjectsHighestPriorityCache[0]);
			}
			else if (touchObjectsMulti.Count > 1)
			{
				GameEntry.Lua.Call("UIUtil.OnClickMultiObjects", touchObjectsMulti);
			}
			else if ((touchObjectsMulti.Count != 1 || touchObjectsMulti[0] == null || !TouchObjectEvent.ExecuteClick(touchObjectsMulti[0])) && (touchPress == null || !TouchObjectEvent.ExecuteClick(touchPress)))
			{
				ClickWorld();
			}
		}
	}

	public void ShowLongPressEffect(int pointIndex)
	{
		DCPlayer player = GameEntry.Data.Player;
		if (!player.IsInBattleField(3) && !player.IsInBattleField(2))
		{
			return;
		}
		LongPressEffectVisible = true;
		if (LongPressEffectReq == null)
		{
			LongPressEffectReq = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Effect/BattleField/Eff_ui_longpress_signal.prefab");
			LongPressEffectReq.completed += delegate
			{
				LongPressEffectGo = LongPressEffectReq.gameObject;
				LongPressEffectGo.transform.SetParent(world.DynamicObjNode);
				LongPressEffectGo.transform.position = world.TileIndexToWorld(pointIndex);
				LongPressEffectGo.SetActive(LongPressEffectVisible);
			};
		}
		else if (LongPressEffectGo != null)
		{
			LongPressEffectGo.SetActive(value: true);
			LongPressEffectGo.transform.position = world.TileIndexToWorld(pointIndex);
		}
	}

	public void HideLongPressEffect()
	{
		LongPressEffectVisible = false;
		if (LongPressEffectGo != null)
		{
			LongPressEffectGo.SetActive(value: false);
		}
	}

	public void ShowBlankTitleEffect(int pointIndex)
	{
		QuanEffectRangeVisible = true;
		if (QuanEffectRangeReq == null)
		{
			QuanEffectRangeReq = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/BuildEffect/V_zdbx_quan.prefab");
			QuanEffectRangeReq.completed += delegate
			{
				GameObject gameObject = QuanEffectRangeReq.gameObject;
				gameObject.transform.SetParent(world.DynamicObjNode);
				gameObject.transform.position = world.TileIndexToWorld(pointIndex);
				BlankAnimator = gameObject.transform.GetComponent<SimpleAnimation>();
				if (QuanEffectRangeVisible)
				{
					gameObject.SetActive(value: true);
					BlankAnimator.Play("V_zdbx_quan");
				}
				else
				{
					gameObject.SetActive(value: false);
				}
			};
		}
		else if (BlankAnimator != null)
		{
			BlankAnimator.gameObject.SetActive(value: true);
			BlankAnimator.transform.position = world.TileIndexToWorld(pointIndex);
			BlankAnimator.Play("V_zdbx_quan");
		}
	}

	public void HideBlankTitleEffect()
	{
		QuanEffectRangeVisible = false;
		if (BlankAnimator != null)
		{
			BlankAnimator.Stop();
			BlankAnimator.gameObject.SetActive(value: false);
		}
	}

	private void OnFingerDown(Vector3 pos)
	{
		HideBlankTitleEffect();
		isFingerDownUIObject = GetIsPointerOverUIObject();
		isLongTabStart = false;
		isLongTabEnd = false;
		curTouchPoint = world.GetTouchPoint();
		curTouchTile = TileCoord.WorldToTile(curTouchPoint);
		curIndex = world.TilePosToIndex(curTouchTile);
		_curDragIndex = curIndex;
		if (!isFingerDownUIObject)
		{
			touchPress = TouchObjectEvent.GetFirstEventObject<ITouchObjectPointerDownHandler>(touchObjects);
			PointInfo pointInfo = world.GetPointInfo(curIndex);
			bool flag = false;
			if (pointInfo != null)
			{
				BuildPointInfo buildPointInfo = pointInfo as BuildPointInfo;
				if (pointInfo.ownerUid != GameEntry.Data.Player.Uid)
				{
					flag = true;
				}
				else if (buildPointInfo != null && buildPointInfo.itemId != 418000)
				{
					flag = true;
				}
			}
			if (pointInfo == null || flag)
			{
				Transform transform = SceneManager.World.BuildBubbleNode.Find("TurretAttackRangeEffect");
				if (transform != null)
				{
					transform.gameObject.SetActive(value: false);
				}
			}
			TouchObjectEvent.ExecutePointerDown(touchPress);
			if (touchPress == null)
			{
				touchPress = TouchObjectEvent.GetFirstEventObject<ITouchObjectClickHandler>(touchObjects);
			}
			touchDrag = TouchObjectEvent.GetFirstEventObject<ITouchObjectBeginDragHandler>(touchObjects);
			touchLongTab = TouchObjectEvent.GetFirstEventObject<ITouchObjectBeginLongTabHandler>(touchObjects);
		}
		if (selectedPickable != null)
		{
			_isClickSelectedPickable = touchPickablePos.Contains(curIndex);
			world.CanMoving = !_isClickSelectedPickable;
		}
		else if (!isFingerDownUIObject)
		{
			WorldScene.selectMarchUuid = GetRaycastHitMarch(pos);
			if (WorldScene.selectMarchUuid == 0L)
			{
				int raycastHitExploreAndSamplePoint = GetRaycastHitExploreAndSamplePoint(pos);
				if (raycastHitExploreAndSamplePoint > 0)
				{
					curIndex = raycastHitExploreAndSamplePoint;
				}
			}
		}
		if (!isFingerDownUIObject)
		{
			world.TrackMarch(0L);
		}
		GameEntry.Event.Fire(EventId.OnWorldInputPointDown, curIndex);
	}

	private void RaycastTouchObject(Vector3 screenPos)
	{
		touchObjects.Clear();
		touchObjectsMulti.Clear();
		touchObjectsHighestPriorityCache.Clear();
		RaycastHit[] array = Physics.RaycastAll(world.ScreenPointToRay(screenPos), float.PositiveInfinity, LayerMask.GetMask("Default", "WorldArmy", "Train"));
		for (int i = 0; i < array.Length; i++)
		{
			ITouchObject componentInParent = array[i].collider.GetComponentInParent<ITouchObject>();
			if (componentInParent != null && !touchObjects.Contains(componentInParent))
			{
				touchObjects.Add(componentInParent);
				if (componentInParent.PreviewType == WorldPreviewType.HighThanMultiObjects)
				{
					touchObjectsHighestPriorityCache.Add(componentInParent);
				}
			}
		}
		if (touchObjects.Count <= 0)
		{
			return;
		}
		int num = 0;
		touchObjects.Sort((ITouchObject a, ITouchObject b) => b.Priority.CompareTo(a.Priority));
		touchObjectsHighestPriorityCache.Sort((ITouchObject a, ITouchObject b) => b.Priority.CompareTo(a.Priority));
		for (int num2 = 0; num2 < touchObjects.Count; num2++)
		{
			ITouchObject touchObject = touchObjects[num2];
			if (touchObject != null)
			{
				if (touchObject.PreviewType == WorldPreviewType.GUI)
				{
					touchObjectsMulti.Clear();
					touchObjectsMulti.Add(touchObject);
					return;
				}
				if (touchObject.Priority > 0f && touchObject.PreviewType != WorldPreviewType.Default && touchObject.PreviewType != WorldPreviewType.Rally && touchObject.PreviewType != WorldPreviewType.Troop && touchObject.PreviewType != WorldPreviewType.HighThanMultiObjects)
				{
					touchObjectsMulti.Add(touchObject);
				}
				if (touchObject.PreviewType == WorldPreviewType.MeteoriteRes)
				{
					num++;
				}
				else if (touchObject.PreviewType != WorldPreviewType.AllianceCity)
				{
					num = int.MinValue;
				}
			}
		}
		if (touchObjectsMulti.Count == 0)
		{
			for (int num3 = 0; num3 < touchObjects.Count; num3++)
			{
				if (touchObjects[num3].Priority > 0f && (touchObjects[num3].PreviewType == WorldPreviewType.Rally || touchObjects[num3].PreviewType == WorldPreviewType.Troop))
				{
					touchObjectsMulti.Add(touchObjects[num3]);
				}
			}
		}
		bool flag = GameEntry.Data.Player.IsInBattleField();
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (!flag && curSkinMeta != null && curSkinMeta.IsDesertMode())
		{
			Vector2Int touchTilePos = world.GetTouchTilePos();
			int pointIndex = world.TilePosToIndex(touchTilePos);
			if (SceneManager.World.GetPointInfo(pointIndex) == null)
			{
				WorldMarch marchesByStartIndex = SceneManager.World.GetMarchesByStartIndex(pointIndex);
				if (marchesByStartIndex == null || !marchesByStartIndex.IsMonsterOrBoss())
				{
					bool flag2 = false;
					foreach (ITouchObject item in touchObjectsMulti)
					{
						if (item.PreviewType == WorldPreviewType.Troop || item.PreviewType == WorldPreviewType.Rally || item.PreviewType == WorldPreviewType.Truck)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						desertTrigger.previewType = WorldPreviewType.SeasonDesert;
						desertTrigger.previewName = pointIndex.ToString();
						WorldDesertInfo worldDesertInfo = SceneManager.World.GetWorldDesertInfo(pointIndex);
						desertTrigger.previewIconPath = ((worldDesertInfo == null) ? "100" : worldDesertInfo.desertId.ToString());
						touchObjectsMulti.Add(desertTrigger);
					}
				}
			}
		}
		else if (!flag && curSkinMeta != null && !curSkinMeta.IsDesertMode() && touchObjectsMulti.Count > 0)
		{
			if (!curSkinMeta.IsNotSeason())
			{
				int num4 = -1;
				int num5 = -1;
				for (int num6 = 0; num6 < touchObjectsMulti.Count; num6++)
				{
					ITouchObject touchObject2 = touchObjectsMulti[num6];
					if (touchObject2.PreviewType == WorldPreviewType.Boss || touchObject2.PreviewType == WorldPreviewType.Monster)
					{
						num4 = num6;
					}
					else if (touchObject2.PreviewType == WorldPreviewType.AllianceCity)
					{
						PointInfo pointInfo = touchObject2.GetPointInfo();
						if (pointInfo != null && pointInfo.pointType == WorldPointType.WORLD_CITY_STRONGHOLD)
						{
							num5 = num6;
						}
					}
				}
				if (num4 >= 0 && num5 >= 0)
				{
					touchObjectsMulti.RemoveAt(num5);
				}
			}
			if (num == 1)
			{
				for (int num7 = touchObjectsMulti.Count - 1; num7 >= 0; num7--)
				{
					if (touchObjectsMulti[num7].PreviewType != WorldPreviewType.MeteoriteRes)
					{
						touchObjectsMulti.RemoveAt(num7);
					}
				}
			}
		}
		touchObjectsMulti.Sort((ITouchObject a, ITouchObject b) => a.PreviewType.CompareTo(b.PreviewType));
	}

	private void OnFingerUp()
	{
		_isClickSelectedPickable = false;
		isFingerDownUIObject = false;
		if (isLongTabEnd)
		{
			HideLongPressEffect();
			if (BlankAnimator != null && QuanEffectRangeVisible)
			{
				BlankAnimator.Play("V_zdbx_targetImg");
				BlankAnimator.PlayQueued("V_zdbx_quan_rotation");
				if (SceneManager.World.IsTileWalkable(curTouchPoint) && lodCache <= BlankTitleEffectMaxLod)
				{
					if (SceneManager.World.GetPointInfo(curIndex) == null)
					{
						GameEntry.Lua.UIManager.OpenWindow("UIWorldBlackTile", curIndex.ToString());
					}
					else
					{
						HideBlankTitleEffect();
						GameEntry.Lua.Call("UIUtil.OnClickWorld", curIndex, 1);
					}
				}
			}
		}
		else
		{
			HideLongPressEffect();
			HideBlankTitleEffect();
		}
		isLongTabStart = false;
		isLongTabEnd = false;
		world.CanMoving = true;
		curIndex = -1;
		_curDragIndex = -1;
		WorldScene.selectMarchUuid = 0L;
		formationUuid = 0L;
		formationPointId = 0;
		_tower = null;
		TouchObjectEvent.ExecutePointerUp(touchPress);
		touchPress = null;
		touchDrag = null;
		touchLongTab = null;
		touchObjects.Clear();
		HideLoad();
		GameEntry.Event.Fire(EventId.OnWorldInputPointUp);
	}

	public void SetDragFormationData(long uuid, int pointId)
	{
		formationUuid = uuid;
		formationPointId = pointId;
	}

	private void OnDragStart(Vector3 dragStartpos, bool isLongTap)
	{
		HideLoad();
		HideBlankTitleEffect();
		HideTouchEffect();
		if (!isFingerDownUIObject)
		{
			GameEntry.Lua.Call("UIUtil.DragWorldCloseWorldUI");
			TouchObjectEvent.ExecuteBeginDrag(touchDrag, dragStartpos);
		}
		GameEntry.Event.Fire(EventId.OnWorldInputDragBegin);
	}

	private void OnDragUpdate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
	{
		if (selectedPickable != null && _isClickSelectedPickable)
		{
			Vector3 touchPoint = world.GetTouchPoint();
			selectedPickable.Drag(selectedPickable.GetClosestPoint(touchPoint));
		}
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteDrag(touchDrag, dragPosStart, dragPosCurrent);
		}
		int num = SceneManager.World.WorldToTileIndex(SceneManager.World.GetTouchPoint());
		if (_curDragIndex != num)
		{
			_curDragIndex = num;
			GameEntry.Event.Fire(EventId.OnWorldInputPointDrag, num);
		}
	}

	private void OnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		GameEntry.Event.Fire(EventId.OnWorldInputDragEnd);
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteEndDrag(touchDrag, dragStopPos);
		}
	}

	public long GetRaycastHitMarch(Vector3 screenPos)
	{
		if (Physics.Raycast(world.ScreenPointToRay(screenPos), out var hitInfo, float.PositiveInfinity, LayerMask.GetMask("WorldArmy")))
		{
			GameObject gameObject = hitInfo.transform.gameObject;
			if (hitInfo.transform.gameObject.name.Contains("Model"))
			{
				gameObject = hitInfo.transform.parent.gameObject;
			}
			string[] array = gameObject.name.Split(new char[1] { '_' });
			if (array.Length > 1 && array[0] == "March")
			{
				return long.Parse(array[1]);
			}
		}
		return 0L;
	}

	protected int GetRaycastHitExploreAndSamplePoint(Vector3 screenPos)
	{
		if (Physics.Raycast(world.ScreenPointToRay(screenPos), out var hitInfo, float.PositiveInfinity, LayerMask.GetMask("WorldArmy")))
		{
			GameObject gameObject = hitInfo.transform.gameObject;
			if (hitInfo.transform.gameObject.name.Contains("Model"))
			{
				gameObject = hitInfo.transform.parent.gameObject;
			}
			string[] array = gameObject.name.Split(new char[1] { '_' });
			if (array.Length > 1 && array[0] == "WorldPointObject")
			{
				return int.Parse(array[1]);
			}
		}
		return 0;
	}

	protected bool GetIsPointerOverUIObject()
	{
		if (TouchWrapper.TouchCount > 0)
		{
			foreach (WrappedTouch touch in TouchWrapper.Touches)
			{
				if (EventSystem.current.IsPointerOverGameObject(touch.FingerId))
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetClickWorldBulidingPos()
	{
		return curIndex;
	}

	public void ShowLoad(Vector3 pos)
	{
		float x = pos.x;
		float y = pos.y;
		float z = pos.z;
		string userData = x + ";" + y + ";" + z;
		GameEntry.Event.Fire(EventId.ShowLoadEditorBuild, userData);
		_isShowLoad = true;
	}

	public void HideLoad()
	{
		if (_isShowLoad)
		{
			GameEntry.Event.Fire(EventId.CLOSE_LOADEDITORBUILD);
			_isShowLoad = false;
		}
	}

	public bool CanUseInput()
	{
		return _isCanTouch;
	}

	public void SetUseInput(bool canUse)
	{
		_isCanTouch = canUse;
	}

	public void ShowTouchEffect(Vector2Int tilePos)
	{
		ShowTouchEffect(tilePos, isInSeason: false);
	}

	public void ShowTouchEffect(Vector2Int tilePos, bool isInSeason)
	{
		Vector3 worldPos = world.TileToWorld(tilePos);
		ShowTouchEffect(worldPos, isInSeason);
	}

	public void ShowTouchEffect(Vector3 worldPos, bool isInSeason)
	{
		touchEffectVisible = true;
		if (touchEffectReq == null)
		{
			touchEffectReq = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/World/TouchTerrainEffect.prefab");
			touchEffectReq.completed += delegate
			{
				touchEffect = touchEffectReq.gameObject;
				if (touchEffectVisible)
				{
					touchEffect.SetActive(value: true);
					touchEffect.transform.position = TileCoord.WorldToClosestGridWorld(worldPos);
					Transform transform = touchEffect.transform.Find("BuildGirdSeason");
					if ((bool)transform)
					{
						touchEffectBuildGirdSeason = transform.gameObject;
						touchEffectBuildGirdSeason.SetActive(isInSeason);
					}
					transform = touchEffect.transform.Find("BuildGird");
					if ((bool)transform)
					{
						touchEffectBuildGird = transform.gameObject;
						touchEffectBuildGird.SetActive(!isInSeason);
					}
				}
				else
				{
					touchEffect.SetActive(value: false);
				}
			};
		}
		else if (touchEffect != null)
		{
			touchEffect.SetActive(value: true);
			touchEffect.transform.position = TileCoord.WorldToClosestGridWorld(worldPos);
			if ((bool)touchEffectBuildGirdSeason)
			{
				touchEffectBuildGirdSeason.SetActive(isInSeason);
			}
			if ((bool)touchEffectBuildGird)
			{
				touchEffectBuildGird.SetActive(!isInSeason);
			}
		}
	}

	public void HideTouchEffect()
	{
		touchEffectVisible = false;
		if (touchEffect != null)
		{
			touchEffect.SetActive(value: false);
		}
	}

	public override void OnUpdate(float deltaTime)
	{
		if (TouchWrapper.TouchCount > 0)
		{
			RaycastTouchObject(TouchWrapper.Touch0.Position);
		}
		ProessTouchEnterAndExit();
		if (TouchWrapper.TouchCount == 0 && touchObjects.Count > 0)
		{
			touchObjects.Clear();
		}
	}

	private void ProessTouchEnterAndExit()
	{
		ITouchObject touchObject = null;
		if (touchObjects.Count > 0)
		{
			touchObject = touchObjects[0];
		}
		if (touchObject == null)
		{
			if (touchEnterObj != null && touchEnterObj is ITouchObjectPointerExitHandler)
			{
				((ITouchObjectPointerExitHandler)touchEnterObj).OnPointerExit();
			}
			touchEnterObj = null;
		}
		else if (touchObject != touchEnterObj)
		{
			touchEnterObj = touchObject;
			if (touchEnterObj is ITouchObjectPointerEnterHandler)
			{
				((ITouchObjectPointerEnterHandler)touchEnterObj).OnPointerEnter();
			}
		}
	}
}
