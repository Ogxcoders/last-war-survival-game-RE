using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class LFTouchThrough : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField]
	private bool _passClick = true;

	[SerializeField]
	private bool _passPointerClick = true;

	private readonly List<RaycastResult> _results = new List<RaycastResult>(8);

	private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
	{
		_results.Clear();
		EventSystem.current.RaycastAll(data, _results);
		if (_results.Count < 1)
		{
			return;
		}
		GameObject gameObject = data.pointerCurrentRaycast.gameObject;
		foreach (RaycastResult result in _results)
		{
			if (!(result.gameObject == base.gameObject) && !(result.gameObject == gameObject))
			{
				ExecuteEvents.Execute(result.gameObject, data, function);
				break;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (_passClick || _passPointerClick)
		{
			PassEvent(eventData, ExecuteEvents.pointerClickHandler);
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (_passClick)
		{
			PassEvent(eventData, ExecuteEvents.beginDragHandler);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_passClick)
		{
			PassEvent(eventData, ExecuteEvents.endDragHandler);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_passClick)
		{
			PassEvent(eventData, ExecuteEvents.dragHandler);
		}
	}

	public void ToggleThrough(bool t)
	{
		_passClick = t;
	}

	public void SetPassPointer(bool t)
	{
		_passPointerClick = t;
	}
}
