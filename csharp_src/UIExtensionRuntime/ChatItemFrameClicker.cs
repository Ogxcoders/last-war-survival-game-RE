using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatItemFrameClicker : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerUpHandler, IPointerDownHandler
{
	public float longPressThreshold = 1f;

	private float _clickTime;

	private bool _longPressClick;

	private readonly List<RaycastResult> _results = new List<RaycastResult>(8);

	private bool _pressing;

	private float _pressingTime;

	private Action _longPress;

	public void SetLongPressAction(Action _action)
	{
		_longPress = _action;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!_longPressClick)
		{
			PassEvent(eventData, ExecuteEvents.pointerClickHandler);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (Time.time - _clickTime > longPressThreshold)
		{
			_longPressClick = true;
		}
		_pressing = false;
		_pressingTime = 0f;
		PassEvent(eventData, ExecuteEvents.pointerUpHandler);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_clickTime = Time.time;
		_pressing = true;
		_pressingTime = 0f;
		_longPressClick = false;
		PassEvent(eventData, ExecuteEvents.pointerDownHandler);
	}

	private void Update()
	{
		if (_pressing)
		{
			_pressingTime += Time.deltaTime;
			if (_pressingTime > longPressThreshold)
			{
				_pressing = false;
				_longPress?.Invoke();
			}
		}
	}

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
				GameObject eventHandler = ExecuteEvents.GetEventHandler<T>(result.gameObject);
				if (eventHandler != null)
				{
					ExecuteEvents.Execute(eventHandler, data, function);
				}
				break;
			}
		}
	}
}
