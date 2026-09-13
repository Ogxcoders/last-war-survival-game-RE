using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeInspectorNamespace;

public class PointerEventListener : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerClickHandler
{
	public delegate void PointerEvent(PointerEventData eventData);

	public event PointerEvent PointerDown;

	public event PointerEvent PointerUp;

	public event PointerEvent PointerClick;

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		if (this.PointerDown != null)
		{
			this.PointerDown(eventData);
		}
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		if (this.PointerUp != null)
		{
			this.PointerUp(eventData);
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		if (this.PointerClick != null)
		{
			this.PointerClick(eventData);
		}
	}
}
