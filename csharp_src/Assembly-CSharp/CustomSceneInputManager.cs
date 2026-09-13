using System.Collections.Generic;
using BitBenderGames;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomSceneInputManager
{
	private static readonly float m_LoadDelay = 0.15f;

	private TouchInputController touchInput;

	private bool isLongTabStart;

	private bool isLongTabEnd;

	private bool isFingerDownUIObject;

	private List<ITouchObject> touchObjects = new List<ITouchObject>();

	private ITouchObject touchEnterObj;

	private ITouchObject touchPress;

	private ITouchObject touchDrag;

	private ITouchObject touchLongTab;

	private bool _alreadyDestory;

	private Camera _camera;

	public void Init(Camera camera)
	{
		isLongTabStart = false;
		isLongTabEnd = false;
		isFingerDownUIObject = false;
		_alreadyDestory = false;
		touchInput = new TouchInputController();
		touchInput.OnFingerDown += OnFingerDown;
		touchInput.OnFingerUp += OnFingerUp;
		touchInput.OnDragStart += OnDragStart;
		touchInput.OnDragUpdate += OnDragUpdate;
		touchInput.OnDragStop += OnDragStop;
		touchInput.OnInputClick += OnInputClick;
		touchInput.OnLongTapProgress += OnLongTapProgress;
		_camera = camera;
	}

	public void UnInit()
	{
		_alreadyDestory = true;
		touchInput.OnFingerDown -= OnFingerDown;
		touchInput.OnFingerUp -= OnFingerUp;
		touchInput.OnDragStart -= OnDragStart;
		touchInput.OnDragUpdate -= OnDragUpdate;
		touchInput.OnDragStop -= OnDragStop;
		touchInput.OnInputClick -= OnInputClick;
		touchInput.OnLongTapProgress -= OnLongTapProgress;
		_camera = null;
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
	}

	private void OnLongTabEnd()
	{
		TouchObjectEvent.ExecuteEndLongTab(touchLongTab);
	}

	private void OnInputClick(Vector3 clickPosition, bool isDoubleClick, bool isLongTap)
	{
		if (!isLongTap && !isFingerDownUIObject)
		{
			if (isDoubleClick)
			{
				TouchObjectEvent.ExecuteDoubleClick(touchPress);
			}
			else if (touchPress != null)
			{
				TouchObjectEvent.ExecuteClick(touchPress);
			}
		}
	}

	private void OnFingerDown(Vector3 pos)
	{
		if (_alreadyDestory)
		{
			return;
		}
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
	}

	private Ray ScreenPointToRay(Vector3 screenPos)
	{
		return _camera.ScreenPointToRay(screenPos);
	}

	private void RaycastTouchObject(Vector3 screenPos)
	{
		touchObjects.Clear();
		if (_camera == null)
		{
			return;
		}
		RaycastHit[] array = Physics.RaycastAll(ScreenPointToRay(screenPos), float.PositiveInfinity, LayerMask.GetMask("Default"));
		for (int i = 0; i < array.Length; i++)
		{
			ITouchObject componentInParent = array[i].collider.GetComponentInParent<ITouchObject>();
			if (componentInParent != null)
			{
				touchObjects.Add(componentInParent);
			}
		}
		if (touchObjects.Count > 0)
		{
			touchObjects.Sort((ITouchObject a, ITouchObject b) => b.Priority.CompareTo(a.Priority));
		}
	}

	private void OnFingerUp()
	{
		if (!_alreadyDestory)
		{
			isFingerDownUIObject = false;
			isLongTabStart = false;
			isLongTabEnd = false;
			TouchObjectEvent.ExecutePointerUp(touchPress);
			touchPress = null;
			touchDrag = null;
			touchLongTab = null;
			touchObjects.Clear();
		}
	}

	private void OnDragStart(Vector3 dragStartpos, bool isLongTap)
	{
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteBeginDrag(touchDrag, dragStartpos);
		}
	}

	private void OnDragUpdate(Vector3 dragPosStart, Vector3 dragPosCurrent, Vector3 correctionOffset)
	{
		TouchObjectEvent.ExecuteDrag(touchDrag, dragPosStart, dragPosCurrent);
	}

	private void OnDragStop(Vector3 dragStopPos, Vector3 dragFinalMomentum)
	{
		if (!isFingerDownUIObject)
		{
			TouchObjectEvent.ExecuteEndDrag(touchDrag, dragStopPos);
		}
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

	public void OnUpdate()
	{
		if (TouchWrapper.TouchCount > 0)
		{
			RaycastTouchObject(TouchWrapper.Touch0.Position);
		}
		if (touchInput.enabled)
		{
			touchInput.OnUpdate();
		}
		if (TouchWrapper.TouchCount == 0 && touchObjects.Count > 0)
		{
			touchObjects.Clear();
		}
	}
}
