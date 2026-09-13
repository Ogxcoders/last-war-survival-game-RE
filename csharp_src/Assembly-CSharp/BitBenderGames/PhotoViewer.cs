using UnityEngine;

namespace BitBenderGames;

public class PhotoViewer : MonoBehaviour
{
	private Camera uiCamera;

	private RectTransform myRectTransform;

	private TouchInputController2 touchInputController;

	private Vector2 parentRectSizeDelta;

	private float photoOriginalWidth;

	private float photoOriginalHeight;

	private float minSizeDeltaX;

	private float maxSizeDeltaX;

	private float minSizeDeltaY;

	private float maxSizeDeltaY;

	private float zoomSpeed = 6.5f;

	private float minZoomRatio = 0.5f;

	private float maxZoomRatio = 4f;

	private float curZoomRatio = 1f;

	private Vector2 beforeDragAnchoredPos;

	private Vector2 beforePinchSizeDelta;

	private Vector2 beforeDragPhotoCenterPos;

	private Vector2 dragingPhotoCenterPos;

	private float moveSpeed = 7f;

	private float reboundTargetRectX;

	private float beforeReboundToEdgeRectX;

	private float reboundTargetRectY;

	private float beforeReboundToEdgeRectY;

	private bool isStartMoveToCenter;

	private void Awake()
	{
		myRectTransform = GetComponent<RectTransform>();
		touchInputController = GetComponent<TouchInputController2>();
	}

	private void OnEnable()
	{
		uiCamera = GameEntry.UICamera;
		Canvas component = GameEntry.UIContainer.GetComponent<Canvas>();
		float x = (float)Screen.width / component.scaleFactor;
		float y = (float)Screen.height / component.scaleFactor;
		parentRectSizeDelta = new Vector2(x, y);
		myRectTransform.anchoredPosition = Vector2.zero;
		beforeDragAnchoredPos = Vector2.zero;
		beforePinchSizeDelta = Vector2.zero;
		beforeDragPhotoCenterPos = Vector2.zero;
		dragingPhotoCenterPos = Vector2.zero;
		isStartMoveToCenter = false;
		touchInputController.ClearAllEvent();
		BindDrag();
		BindPinch();
	}

	public void SetPhotoOriginalSize(float originalWidth, float originalHeight)
	{
		photoOriginalWidth = originalWidth;
		photoOriginalHeight = originalHeight;
		minSizeDeltaX = photoOriginalWidth * minZoomRatio;
		maxSizeDeltaX = photoOriginalWidth * maxZoomRatio;
		minSizeDeltaY = photoOriginalHeight * minZoomRatio;
		maxSizeDeltaY = photoOriginalHeight * maxZoomRatio;
	}

	public void BindClick(TouchInputController2.InputClickDelegate OnInputClick)
	{
		touchInputController.ClearEventOnInputClick();
		touchInputController.OnInputClick += OnInputClick;
	}

	private void BindDrag()
	{
		touchInputController.OnDragStart += delegate
		{
			beforeDragAnchoredPos = myRectTransform.anchoredPosition;
		};
		touchInputController.OnDragUpdate += delegate(Vector3 originalScreenPos, Vector3 updateScreenPos, Vector3 b)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(myRectTransform, originalScreenPos, uiCamera, out var localPoint);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(myRectTransform, updateScreenPos, uiCamera, out var localPoint2);
			Vector2 vector = localPoint2 - localPoint;
			myRectTransform.anchoredPosition = beforeDragAnchoredPos + vector;
		};
		touchInputController.OnDragStop += delegate
		{
			if (curZoomRatio <= 1f)
			{
				SetSpringBackToCenter();
			}
			else
			{
				SetSpringBack();
			}
		};
	}

	private void BindPinch()
	{
		touchInputController.OnPinchStart += delegate
		{
			beforePinchSizeDelta = myRectTransform.sizeDelta;
		};
		touchInputController.OnPinchUpdate += delegate(Vector3 pinchCenter, float pinchDistance, float pinchStartDistance)
		{
			float num = (pinchDistance - pinchStartDistance) * zoomSpeed;
			float num2 = photoOriginalWidth * num;
			float num3 = photoOriginalHeight * num;
			float num4 = Mathf.Clamp(beforePinchSizeDelta.x + num2, minSizeDeltaX, maxSizeDeltaX);
			float y = Mathf.Clamp(beforePinchSizeDelta.y + num3, minSizeDeltaY, maxSizeDeltaY);
			curZoomRatio = num4 / photoOriginalWidth;
			myRectTransform.sizeDelta = new Vector2(num4, y);
		};
		touchInputController.OnPinchStop += delegate
		{
			if (curZoomRatio <= 1f)
			{
				SetSpringBackToCenter();
			}
			else
			{
				SetSpringBack();
			}
		};
	}

	private void SetSpringBack()
	{
		beforeDragPhotoCenterPos = myRectTransform.anchoredPosition;
		dragingPhotoCenterPos = myRectTransform.anchoredPosition;
		if (myRectTransform.sizeDelta.x > parentRectSizeDelta.x)
		{
			if (myRectTransform.anchoredPosition.x > 0f)
			{
				beforeReboundToEdgeRectX = myRectTransform.anchoredPosition.x - myRectTransform.sizeDelta.x / 2f;
				reboundTargetRectX = (0f - parentRectSizeDelta.x) / 2f;
			}
			else
			{
				beforeReboundToEdgeRectX = myRectTransform.sizeDelta.x / 2f + myRectTransform.anchoredPosition.x;
				reboundTargetRectX = parentRectSizeDelta.x / 2f;
			}
		}
		if (myRectTransform.sizeDelta.y > parentRectSizeDelta.y)
		{
			if (myRectTransform.anchoredPosition.y > 0f)
			{
				beforeReboundToEdgeRectY = myRectTransform.anchoredPosition.y - myRectTransform.sizeDelta.y / 2f;
				reboundTargetRectY = (0f - parentRectSizeDelta.y) / 2f;
			}
			else
			{
				beforeReboundToEdgeRectY = myRectTransform.sizeDelta.y / 2f + myRectTransform.anchoredPosition.y;
				reboundTargetRectY = parentRectSizeDelta.y / 2f;
			}
		}
	}

	private void SetSpringBackToCenter()
	{
		beforeDragPhotoCenterPos = myRectTransform.anchoredPosition;
		dragingPhotoCenterPos = myRectTransform.anchoredPosition;
		isStartMoveToCenter = true;
	}

	private void Update()
	{
		if (curZoomRatio < 1f && !touchInputController.IsPinching)
		{
			curZoomRatio = Mathf.Lerp(curZoomRatio, 1f, Time.deltaTime * zoomSpeed);
			if (1f - curZoomRatio < 0.01f)
			{
				curZoomRatio = 1f;
			}
			float value = photoOriginalWidth * curZoomRatio;
			float value2 = photoOriginalHeight * curZoomRatio;
			float x = Mathf.Clamp(value, minSizeDeltaX, maxSizeDeltaX);
			float y = Mathf.Clamp(value2, minSizeDeltaY, maxSizeDeltaY);
			myRectTransform.sizeDelta = new Vector2(x, y);
		}
		if (curZoomRatio <= 1f && isStartMoveToCenter && !touchInputController.IsDragging && !touchInputController.IsPinching)
		{
			dragingPhotoCenterPos = Vector2.Lerp(dragingPhotoCenterPos, Vector2.zero, Time.deltaTime * moveSpeed);
			if (dragingPhotoCenterPos.magnitude <= 0.01f)
			{
				dragingPhotoCenterPos = Vector2.zero;
				isStartMoveToCenter = false;
			}
			myRectTransform.anchoredPosition = dragingPhotoCenterPos;
		}
		if (!(curZoomRatio > 1f) || touchInputController.IsDragging || touchInputController.IsPinching)
		{
			return;
		}
		if (myRectTransform.sizeDelta.x > parentRectSizeDelta.x)
		{
			if ((myRectTransform.anchoredPosition.x > 0f && reboundTargetRectX < beforeReboundToEdgeRectX) || (myRectTransform.anchoredPosition.x < 0f && reboundTargetRectX > beforeReboundToEdgeRectX))
			{
				float num = reboundTargetRectX - beforeReboundToEdgeRectX;
				dragingPhotoCenterPos.x = Mathf.Lerp(dragingPhotoCenterPos.x, beforeDragPhotoCenterPos.x + num, Time.deltaTime * moveSpeed);
				if (Mathf.Abs(beforeDragPhotoCenterPos.x + num - dragingPhotoCenterPos.x) < 0.01f)
				{
					dragingPhotoCenterPos.x = beforeDragPhotoCenterPos.x + num;
				}
				myRectTransform.anchoredPosition = new Vector2(dragingPhotoCenterPos.x, myRectTransform.anchoredPosition.y);
			}
		}
		else
		{
			dragingPhotoCenterPos.x = Mathf.Lerp(dragingPhotoCenterPos.x, 0f, Time.deltaTime * moveSpeed);
			if (Mathf.Abs(dragingPhotoCenterPos.x) < 0.01f)
			{
				dragingPhotoCenterPos.x = 0f;
			}
			myRectTransform.anchoredPosition = new Vector2(dragingPhotoCenterPos.x, myRectTransform.anchoredPosition.y);
		}
		if (myRectTransform.sizeDelta.y > parentRectSizeDelta.y)
		{
			if ((myRectTransform.anchoredPosition.y > 0f && reboundTargetRectY < beforeReboundToEdgeRectY) || (myRectTransform.anchoredPosition.y < 0f && reboundTargetRectY > beforeReboundToEdgeRectY))
			{
				float num2 = reboundTargetRectY - beforeReboundToEdgeRectY;
				dragingPhotoCenterPos.y = Mathf.Lerp(dragingPhotoCenterPos.y, beforeDragPhotoCenterPos.y + num2, Time.deltaTime * moveSpeed);
				if (Mathf.Abs(beforeDragPhotoCenterPos.y + num2 - dragingPhotoCenterPos.y) < 0.01f)
				{
					dragingPhotoCenterPos.y = beforeDragPhotoCenterPos.x + num2;
				}
				myRectTransform.anchoredPosition = new Vector2(myRectTransform.anchoredPosition.x, dragingPhotoCenterPos.y);
			}
		}
		else
		{
			dragingPhotoCenterPos.y = Mathf.Lerp(dragingPhotoCenterPos.y, 0f, Time.deltaTime * moveSpeed);
			if (Mathf.Abs(dragingPhotoCenterPos.y) < 0.01f)
			{
				dragingPhotoCenterPos.y = 0f;
			}
			myRectTransform.anchoredPosition = new Vector2(myRectTransform.anchoredPosition.x, dragingPhotoCenterPos.y);
		}
	}

	public void SetInputControllerState(bool state)
	{
		touchInputController.enabled = state;
	}
}
