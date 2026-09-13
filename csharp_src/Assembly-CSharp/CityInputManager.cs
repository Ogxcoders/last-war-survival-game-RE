using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

public class CityInputManager : CityManagerBase
{
	private static readonly float m_LoadDelay = 0.15f;

	private TouchInputController touchInput;

	private bool isLongTabStart;

	private bool isLongTabEnd;

	private bool isFingerDownUIObject;

	public long marchUuid;

	private BasementTower _tower;

	private List<ITouchObject> touchObjects = new List<ITouchObject>();

	private ITouchObject touchEnterObj;

	private ITouchObject touchPress;

	private ITouchObject touchDrag;

	private ITouchObject touchLongTab;

	private LuaTable joyStickPara;

	private bool _alreadyDestory;

	private bool _isShowLoad;

	private bool _isClickSelectedPickable;

	private ITouchPickable selectedPickable;

	public List<int> touchPickablePos;

	private InstanceRequest touchEffectReq;

	private bool touchEffectVisible;

	private GameObject touchEffect;

	public GameObject AttackRangeEffect;

	private InstanceRequest AttackRangeEffectReq;

	private bool AttackRangeEffectVisible;

	private bool _isCanTouch = true;

	private InstanceRequest QuanEffectRangeReq;

	private bool QuanEffectRangeVisible;

	private SimpleAnimation BlankAnimator;

	private int _curDragIndex;

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

	public CityInputManager(CityScene scene)
		: base(scene)
	{
		touchPickablePos = new List<int>();
		_isCanTouch = true;
		QuanEffectRangeVisible = false;
		touchInput = new TouchInputController();
	}

	public override void Init()
	{
		isLongTabStart = false;
		isLongTabEnd = false;
		isFingerDownUIObject = false;
		_curDragIndex = 0;
		_alreadyDestory = false;
		touchInput = new TouchInputController();
		touchInput.OnFingerDown += OnFingerDown;
		touchInput.OnFingerUp += OnFingerUp;
		touchInput.OnDragStart += OnDragStart;
		touchInput.OnDragUpdate += OnDragUpdate;
		touchInput.OnDragStop += OnDragStop;
		touchInput.OnInputClick += OnInputClick;
		touchInput.OnLongTapProgress += OnLongTapProgress;
	}

	public override void UnInit()
	{
		base.UnInit();
		_alreadyDestory = true;
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
		if (touchEffectReq != null)
		{
			touchEffectReq.Destroy();
			touchEffectReq = null;
		}
	}

	public void SetTouchInputControllerEnable(bool able)
	{
		touchInput.enabled = able;
	}

	public bool GetTouchInputControllerEnable()
	{
		return touchInput.enabled;
	}

	public void SetSelectedPickable(ITouchPickable pickable)
	{
		_isClickSelectedPickable = true;
		selectedPickable = pickable;
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
			SceneManager.World.GetPointInfo(curIndex);
			if (touchLongTab == null || !TouchObjectEvent.ExecuteBeginLongTab(touchLongTab))
			{
				SceneManager.World.IsTileWalkable(SceneManager.World.TileIndexToWorld(curIndex));
			}
		}
	}

	private void OnLongTabEnd()
	{
		if (CanUseInput())
		{
			HideLoad();
			TouchObjectEvent.ExecuteEndLongTab(touchLongTab);
		}
	}

	private void ClickWorld()
	{
		TouchObjectEventTrigger touchObjectEventTrigger = touchPress as TouchObjectEventTrigger;
		if (touchPress != null && touchObjectEventTrigger != null)
		{
			GameEntry.Lua.Call("UIUtil.OnClickCity", SceneManager.World.TilePosToIndex(touchPress.TilePos), 1);
		}
		else
		{
			GameEntry.Lua.Call("UIUtil.OnClickCity", curIndex, 0);
		}
		SceneManager.World.IndexToTilePos(curIndex);
	}

	private void OnInputClick(Vector3 clickPosition, bool isDoubleClick, bool isLongTap)
	{
		if (isLongTap || isFingerDownUIObject)
		{
			return;
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
			else if (SceneManager.World.GetLodLevel() >= 2)
			{
				Vector2Int touchTilePos = SceneManager.World.GetTouchTilePos();
				SceneManager.World.AutoLookat(SceneManager.World.TileToWorld(touchTilePos), SceneManager.World.InitZoom);
			}
			else if (touchPress == null || !TouchObjectEvent.ExecuteClick(touchPress))
			{
				ClickWorld();
			}
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
				gameObject.transform.SetParent(SceneManager.World.DynamicObjNode);
				gameObject.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
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
			BlankAnimator.transform.position = SceneManager.World.TileIndexToWorld(pointIndex);
			BlankAnimator.Play("V_zdbx_quan");
		}
	}

	public void HideBlankTitleEffect()
	{
		QuanEffectRangeVisible = false;
		if (BlankAnimator != null)
		{
			BlankAnimator.gameObject.SetActive(value: false);
		}
	}

	private void OnFingerDown(Vector3 pos)
	{
		if (_alreadyDestory)
		{
			return;
		}
		HideBlankTitleEffect();
		isFingerDownUIObject = GetIsPointerOverUIObject();
		isLongTabStart = false;
		isLongTabEnd = false;
		if (!isFingerDownUIObject)
		{
			touchPress = TouchObjectEvent.GetFirstEventObject<ITouchObjectPointerDownHandler>(touchObjects);
			TouchObjectEvent.ExecutePointerDown(touchPress);
			if (touchPress == null)
			{
				touchPress = TouchObjectEvent.GetFirstEventObject<ITouchObjectClickHandler>(touchObjects);
			}
			touchDrag = TouchObjectEvent.GetFirstEventObject<ITouchObjectBeginDragHandler>(touchObjects);
			touchLongTab = TouchObjectEvent.GetFirstEventObject<ITouchObjectBeginLongTabHandler>(touchObjects);
		}
		curTouchPoint = scene.GetTouchPoint();
		curTouchTile = TileCoord.WorldToTile(curTouchPoint);
		curIndex = scene.TilePosToIndex(curTouchTile);
		if (SceneManager.World.GetPointType(curIndex) == 0)
		{
			int raycastHitGarbagePoint = GetRaycastHitGarbagePoint(pos);
			if (raycastHitGarbagePoint > 0)
			{
				curIndex = raycastHitGarbagePoint;
			}
		}
		_curDragIndex = curIndex;
		if (selectedPickable != null)
		{
			_isClickSelectedPickable = touchPickablePos.Contains(curIndex);
			SceneManager.World.CanMoving = !_isClickSelectedPickable;
		}
		if (!isFingerDownUIObject)
		{
			scene.TrackMarch(0L);
		}
		GameEntry.Event.Fire(EventId.OnWorldInputPointDown, curIndex);
		bool flag = GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.IsBeforePrologue");
		bool flag2 = GameEntry.Lua.CallWithReturn<bool>("DataCenter.GuideManager:InGuide");
		if (!isFingerDownUIObject && flag && !flag2)
		{
			SceneManager.World.CanMoving = false;
			if (joyStickPara == null)
			{
				joyStickPara = GameEntry.Lua.Env.NewTable();
				joyStickPara.Set("playEffect", value: false);
			}
			GameEntry.Lua.UIManager.OpenWindow("UIJoystick", joyStickPara, pos.x, pos.y);
		}
	}

	private void RaycastTouchObject(Vector3 screenPos)
	{
		if (SceneManager.World == null)
		{
			return;
		}
		touchObjects.Clear();
		RaycastHit[] array = Physics.RaycastAll(SceneManager.World.ScreenPointToRay(screenPos), float.PositiveInfinity, LayerMask.GetMask("Default", "WorldArmy"));
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].collider != null)
			{
				ITouchObject componentInParent = array[i].collider.GetComponentInParent<ITouchObject>();
				if (componentInParent != null)
				{
					touchObjects.Add(componentInParent);
				}
			}
		}
		if (touchObjects.Count > 0)
		{
			touchObjects.Sort((ITouchObject a, ITouchObject b) => b.Priority.CompareTo(a.Priority));
		}
	}

	private void OnFingerUp()
	{
		if (_alreadyDestory)
		{
			return;
		}
		_isClickSelectedPickable = false;
		isFingerDownUIObject = false;
		if (isLongTabEnd)
		{
			if (BlankAnimator != null && QuanEffectRangeVisible)
			{
				BlankAnimator.Play("V_zdbx_targetImg");
				BlankAnimator.PlayQueued("V_zdbx_quan_rotation");
				if (SceneManager.World.IsTileWalkable(SceneManager.World.TileIndexToWorld(curIndex)))
				{
					GameEntry.Lua.UIManager.OpenWindow("UIWorldBlackTile", curIndex.ToString());
				}
			}
		}
		else
		{
			HideBlankTitleEffect();
		}
		isLongTabStart = false;
		isLongTabEnd = false;
		SceneManager.World.CanMoving = true;
		curIndex = -1;
		marchUuid = 0L;
		_curDragIndex = -1;
		_tower = null;
		TouchObjectEvent.ExecutePointerUp(touchPress);
		touchPress = null;
		touchDrag = null;
		touchLongTab = null;
		touchObjects.Clear();
		HideLoad();
		GameEntry.Event.Fire(EventId.OnWorldInputPointUp);
		if (GameEntry.Lua.UIManager.IsWindowOpen("UIJoystick"))
		{
			SceneManager.World.CanMoving = true;
			GameEntry.Lua.UIManager.DestroyWindow("UIJoystick", joyStickPara);
		}
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
		if (selectedPickable != null)
		{
			if (_isClickSelectedPickable)
			{
				Vector3 touchPoint = SceneManager.World.GetTouchPoint();
				selectedPickable.Drag(selectedPickable.GetClosestPoint(touchPoint));
			}
		}
		else if (marchUuid != 0L)
		{
			SceneManager.World.OnTroopDragUpdate(marchUuid, dragPosCurrent, GetRaycastHitMarch(dragPosCurrent));
		}
		TouchObjectEvent.ExecuteDrag(touchDrag, dragPosStart, dragPosCurrent);
		int num = SceneManager.World.WorldToTileIndex(SceneManager.World.GetTouchPoint());
		if (_curDragIndex != num)
		{
			_curDragIndex = num;
			GameEntry.Event.Fire(EventId.OnWorldInputPointDrag, num);
		}
	}

	private void OnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		if (marchUuid != 0L)
		{
			SceneManager.World.OnTroopDragStop(marchUuid, GetRaycastHitMarch(dragStopPos));
		}
		GameEntry.Event.Fire(EventId.OnWorldInputDragEnd);
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteEndDrag(touchDrag, dragStopPos);
		}
	}

	public long GetRaycastHitMarch(Vector3 screenPos)
	{
		if (Physics.Raycast(SceneManager.World.ScreenPointToRay(screenPos), out var hitInfo, float.PositiveInfinity, LayerMask.GetMask("WorldArmy")))
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

	private int GetRaycastHitGarbagePoint(Vector3 screenPos)
	{
		if (Physics.Raycast(SceneManager.World.ScreenPointToRay(screenPos), out var hitInfo, float.PositiveInfinity, LayerMask.GetMask("WorldArmy")))
		{
			GameObject gameObject = hitInfo.transform.gameObject;
			if (hitInfo.transform.gameObject.name.Contains("Model"))
			{
				gameObject = hitInfo.transform.parent.gameObject;
			}
			string[] array = gameObject.name.Split(new char[1] { '_' });
			if (array.Length > 1 && array[0] == "Garbage")
			{
				return int.Parse(array[1]);
			}
		}
		return -1;
	}

	private bool GetIsPointerOverUIObject()
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
					touchEffect.transform.position = SceneManager.World.TileToWorld(tilePos);
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
			touchEffect.transform.position = SceneManager.World.TileToWorld(tilePos);
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
		if (touchInput.enabled)
		{
			touchInput.OnUpdate();
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
