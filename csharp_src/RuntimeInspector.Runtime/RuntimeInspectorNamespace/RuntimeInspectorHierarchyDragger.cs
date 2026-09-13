using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class RuntimeInspectorHierarchyDragger : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public GameObject target;

	private Vector2 _offset;

	public bool dragging { get; private set; }

	public void OnPointerDown(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(target.transform as RectTransform, eventData.position, eventData.pressEventCamera, out _offset);
		dragging = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		dragging = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(target.transform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out var localPoint);
		target.transform.localPosition = localPoint - _offset;
	}
}
