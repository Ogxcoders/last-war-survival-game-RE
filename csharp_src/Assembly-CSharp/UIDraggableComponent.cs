using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDraggableComponent : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
	private bool isDragging;

	private RectTransform rectTransform;

	private Canvas canvas;

	private Vector2 originalPosition;

	private Vector2 pointerStartLocalPosition;

	private RectTransform parentRectTransform;

	public Action onEndDragCallback;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvas = GetComponentInParent<Canvas>();
		parentRectTransform = rectTransform.parent as RectTransform;
		onEndDragCallback = null;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging = true;
		originalPosition = rectTransform.anchoredPosition;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out pointerStartLocalPosition);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (isDragging && RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
		{
			Vector2 vector = localPoint - pointerStartLocalPosition;
			rectTransform.anchoredPosition = originalPosition + vector;
			ClampToScreenBounds();
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
		onEndDragCallback?.Invoke();
	}

	public void ClampToScreenBounds()
	{
		Vector3[] array = new Vector3[4];
		rectTransform.GetWorldCorners(array);
		Camera cam = ((canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : canvas.worldCamera);
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(cam, array[0]);
		Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(cam, array[2]);
		Vector2 zero = Vector2.zero;
		if (vector.x < 0f)
		{
			zero.x = 0f - vector.x;
		}
		else if (vector2.x > (float)Screen.width)
		{
			zero.x = (float)Screen.width - vector2.x;
		}
		if (vector.y < 0f)
		{
			zero.y = 0f - vector.y;
		}
		else if (vector2.y > (float)Screen.height)
		{
			zero.y = (float)Screen.height - vector2.y;
		}
		if (zero != Vector2.zero)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, zero + new Vector2(Screen.width / 2, Screen.height / 2), cam, out var localPoint);
			Vector2 vector3 = localPoint - parentRectTransform.rect.center;
			rectTransform.anchoredPosition += vector3;
		}
	}

	private void OnDestroy()
	{
		onEndDragCallback = null;
	}
}
