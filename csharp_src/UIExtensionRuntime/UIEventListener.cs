using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : EventTrigger
{
	public Action<GameObject> onClick;

	public Action<GameObject> onDown;

	public Action<GameObject> onEnter;

	public Action<GameObject> onExit;

	public Action<GameObject> onUp;

	public Action<GameObject> onSelect;

	public Action<GameObject> onUpdateSelect;

	public Action<GameObject, Vector2, Vector2> onDrag;

	public Action<GameObject, Vector2, Vector2> onEndDrag;

	public Action<GameObject, Vector2, Vector2> onBeginDrag;

	public static UIEventListener Get(GameObject go)
	{
		UIEventListener uIEventListener = go.GetComponent<UIEventListener>();
		if (uIEventListener == null)
		{
			uIEventListener = go.AddComponent<UIEventListener>();
		}
		return uIEventListener;
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (onDrag != null)
		{
			onDrag(base.gameObject, eventData.position, eventData.delta);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (onEndDrag != null)
		{
			onEndDrag(base.gameObject, eventData.position, eventData.delta);
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (onBeginDrag != null)
		{
			onBeginDrag(base.gameObject, eventData.position, eventData.delta);
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null)
		{
			onClick(base.gameObject);
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (onDown != null)
		{
			onDown(base.gameObject);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (onEnter != null)
		{
			onEnter(base.gameObject);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (onExit != null)
		{
			onExit(base.gameObject);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (onUp != null)
		{
			onUp(base.gameObject);
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
		if (onSelect != null)
		{
			onSelect(base.gameObject);
		}
	}

	public override void OnUpdateSelected(BaseEventData eventData)
	{
		if (onUpdateSelect != null)
		{
			onUpdateSelect(base.gameObject);
		}
	}
}
