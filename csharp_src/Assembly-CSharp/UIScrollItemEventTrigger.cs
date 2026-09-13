using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIScrollItemEventTrigger : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IDropHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
	public ScrollRect scrollRect;

	public bool autoFindScrollRect;

	public int pixelClickThreshold = 10;

	public Action<PointerEventData> onBeginDrag;

	public Action<PointerEventData> onDrag;

	public Action<PointerEventData> onDrop;

	public Action<PointerEventData> onEndDrag;

	public Action<PointerEventData> onPointerClick;

	public Action<PointerEventData> onPointerDown;

	public Action<PointerEventData> onPointerUp;

	private void OnTransformParentChanged()
	{
		if (autoFindScrollRect)
		{
			if (base.transform.parent != null && scrollRect == null)
			{
				scrollRect = GetComponentInParent<ScrollRect>();
			}
			else if (base.transform.parent == null)
			{
				scrollRect = null;
			}
		}
	}

	public void OnBeginDrag(PointerEventData data)
	{
		if (scrollRect != null)
		{
			scrollRect.OnBeginDrag(data);
		}
		onBeginDrag?.Invoke(data);
	}

	public void OnDrag(PointerEventData data)
	{
		if (scrollRect != null)
		{
			scrollRect.OnDrag(data);
		}
		onDrag?.Invoke(data);
	}

	public void OnDrop(PointerEventData data)
	{
		onDrop?.Invoke(data);
	}

	public void OnEndDrag(PointerEventData data)
	{
		if (scrollRect != null)
		{
			scrollRect.OnEndDrag(data);
		}
		onEndDrag?.Invoke(data);
	}

	public void OnPointerClick(PointerEventData data)
	{
		if (pixelClickThreshold <= 0 || !(Vector2.Distance(data.pressPosition, data.position) > (float)pixelClickThreshold))
		{
			onPointerClick?.Invoke(data);
		}
	}

	public void OnPointerDown(PointerEventData data)
	{
		onPointerDown?.Invoke(data);
	}

	public void OnPointerUp(PointerEventData data)
	{
		onPointerUp?.Invoke(data);
	}
}
