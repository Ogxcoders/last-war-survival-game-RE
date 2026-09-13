using UnityEngine;

public class EditorInputManager : WorldInputManager
{
	private EditorSceneBuilding editorScene;

	public EditorInputManager(EditorSceneBuilding scene)
		: base(scene)
	{
		editorScene = scene;
	}

	public override void Init()
	{
		isLongTabStart = false;
		isLongTabEnd = false;
		isFingerDownUIObject = false;
		formationUuid = 0L;
		formationPointId = 0;
		_curDragIndex = 0;
		touchInput = world.TouchInputController;
		touchInput.OnFingerDown += OnFingerDownEditor;
		touchInput.OnFingerUp += OnFingerUpEditor;
		touchInput.OnDragStart += OnDragStartEditor;
		touchInput.OnDragUpdate += OnDragUpdateEditor;
		touchInput.OnDragStop += OnDragStopEditor;
		touchInput.OnInputClick += OnInputClickEditor;
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, base.OnUpdateLod);
		uiParam = GameEntry.Lua.Env.NewTable();
		uiParam.Set("anim", value: true);
	}

	public override void UnInit()
	{
		touchInput.OnFingerDown -= OnFingerDownEditor;
		touchInput.OnFingerUp -= OnFingerUpEditor;
		touchInput.OnDragStart -= OnDragStartEditor;
		touchInput.OnDragUpdate -= OnDragUpdateEditor;
		touchInput.OnDragStop -= OnDragStopEditor;
		touchInput.OnInputClick -= OnInputClickEditor;
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
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, base.OnUpdateLod);
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

	private void OnFingerDownEditor(Vector3 pos)
	{
		HideBlankTitleEffect();
		isFingerDownUIObject = GetIsPointerOverUIObject();
		isLongTabStart = false;
		isLongTabEnd = false;
		Vector2Int touchTilePos = world.GetTouchTilePos();
		base.curIndex = world.TilePosToIndex(touchTilePos);
		_curDragIndex = base.curIndex;
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
		if (selectedPickable != null)
		{
			_isClickSelectedPickable = touchPickablePos.Contains(base.curIndex);
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
					base.curIndex = raycastHitExploreAndSamplePoint;
				}
			}
		}
		_ = isFingerDownUIObject;
		GameEntry.Event.Fire(EventId.OnWorldInputPointDown, base.curIndex);
	}

	private void OnFingerUpEditor()
	{
		_isClickSelectedPickable = false;
		isFingerDownUIObject = false;
		if (isLongTabEnd)
		{
			if (BlankAnimator != null && QuanEffectRangeVisible)
			{
				BlankAnimator.Play("V_zdbx_targetImg");
				BlankAnimator.PlayQueued("V_zdbx_quan_rotation");
				if (world.IsTileWalkable(world.TileIndexToWorld(base.curIndex)) && lodCache <= WorldInputManager.BlankTitleEffectMaxLod && world.GetPointInfo(base.curIndex) != null)
				{
					HideBlankTitleEffect();
				}
			}
		}
		else
		{
			HideBlankTitleEffect();
		}
		isLongTabStart = false;
		isLongTabEnd = false;
		world.CanMoving = true;
		base.curIndex = -1;
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

	private void OnDragStartEditor(Vector3 dragStartpos, bool isLongTap)
	{
		HideLoad();
		HideBlankTitleEffect();
		HideTouchEffect();
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteBeginDrag(touchDrag, dragStartpos);
		}
		GameEntry.Event.Fire(EventId.OnWorldInputDragBegin);
	}

	private void OnDragUpdateEditor(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
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
		int num = world.WorldToTileIndex(world.GetTouchPoint());
		if (_curDragIndex != num)
		{
			_curDragIndex = num;
			GameEntry.Event.Fire(EventId.OnWorldInputPointDrag, num);
		}
	}

	private void OnDragStopEditor(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		GameEntry.Event.Fire(EventId.OnWorldInputDragEnd);
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteEndDrag(touchDrag, dragStopPos);
		}
	}

	private void OnInputClickEditor(Vector3 clickPosition, bool isDoubleClick, bool isLongTap)
	{
		Vector3 touchPoint = world.GetTouchPoint();
		world.WorldToTileIndex(touchPoint);
	}
}
