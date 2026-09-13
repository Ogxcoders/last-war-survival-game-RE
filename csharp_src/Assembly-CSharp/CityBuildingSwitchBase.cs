using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CityBuildingSwitchBase
{
	protected List<GameObject> _models;

	protected Action<GameObject> _changeModelCall;

	private List<EventId> _eventIdList;

	public void Init(List<GameObject> models, List<EventId> eventIdList, Action<GameObject> changeCallback)
	{
		_models = models;
		_changeModelCall = changeCallback;
		_eventIdList = eventIdList;
		if (eventIdList != null)
		{
			foreach (EventId eventId in eventIdList)
			{
				GameEntry.Event.Subscribe(eventId, DoModelLogic);
			}
		}
		OnInit();
	}

	public void Release()
	{
		OnRelease();
		_models = null;
		_changeModelCall = null;
		if (_eventIdList == null)
		{
			return;
		}
		foreach (EventId eventId in _eventIdList)
		{
			GameEntry.Event.Unsubscribe(eventId, DoModelLogic);
		}
	}

	protected virtual void OnInit()
	{
	}

	protected virtual void OnRelease()
	{
	}

	public abstract void DoModelLogic(object userData);
}
