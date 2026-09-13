using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class WindowDragHandler : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	private const int NON_EXISTING_TOUCH = -98456;

	private RectTransform rectTransform;

	private int pointerId = -98456;

	private Vector2 initialTouchPos;

	private void Awake()
	{
		rectTransform = (RectTransform)base.transform;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (pointerId != -98456)
		{
			eventData.pointerDrag = null;
			return;
		}
		pointerId = eventData.pointerId;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out initialTouchPos);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData.pointerId == pointerId)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
			rectTransform.anchoredPosition += localPoint - initialTouchPos;
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (eventData.pointerId == pointerId)
		{
			pointerId = -98456;
		}
	}
}
