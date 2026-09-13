using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class EventCallbackList
{
	private List<EventCallbackFunctorBase> m_List;

	public int trickleDownCallbackCount { get; private set; }

	public int bubbleUpCallbackCount { get; private set; }

	public int Count => m_List.Count;

	public EventCallbackFunctorBase this[int i]
	{
		get
		{
			return m_List[i];
		}
		set
		{
			m_List[i] = value;
		}
	}

	public EventCallbackList()
	{
		m_List = new List<EventCallbackFunctorBase>();
		trickleDownCallbackCount = 0;
		bubbleUpCallbackCount = 0;
	}

	public EventCallbackList(EventCallbackList source)
	{
		m_List = new List<EventCallbackFunctorBase>(source.m_List);
		trickleDownCallbackCount = 0;
		bubbleUpCallbackCount = 0;
	}

	public bool Contains(long eventTypeId, Delegate callback, CallbackPhase phase)
	{
		return Find(eventTypeId, callback, phase) != null;
	}

	public EventCallbackFunctorBase Find(long eventTypeId, Delegate callback, CallbackPhase phase)
	{
		for (int i = 0; i < m_List.Count; i++)
		{
			if (m_List[i].IsEquivalentTo(eventTypeId, callback, phase))
			{
				return m_List[i];
			}
		}
		return null;
	}

	public bool Remove(long eventTypeId, Delegate callback, CallbackPhase phase)
	{
		for (int i = 0; i < m_List.Count; i++)
		{
			if (m_List[i].IsEquivalentTo(eventTypeId, callback, phase))
			{
				m_List.RemoveAt(i);
				switch (phase)
				{
				case CallbackPhase.TrickleDownAndTarget:
					trickleDownCallbackCount--;
					break;
				case CallbackPhase.TargetAndBubbleUp:
					bubbleUpCallbackCount--;
					break;
				}
				return true;
			}
		}
		return false;
	}

	public void Add(EventCallbackFunctorBase item)
	{
		m_List.Add(item);
		if (item.phase == CallbackPhase.TrickleDownAndTarget)
		{
			trickleDownCallbackCount++;
		}
		else if (item.phase == CallbackPhase.TargetAndBubbleUp)
		{
			bubbleUpCallbackCount++;
		}
	}

	public void AddRange(EventCallbackList list)
	{
		m_List.AddRange(list.m_List);
		foreach (EventCallbackFunctorBase item in list.m_List)
		{
			if (item.phase == CallbackPhase.TrickleDownAndTarget)
			{
				trickleDownCallbackCount++;
			}
			else if (item.phase == CallbackPhase.TargetAndBubbleUp)
			{
				bubbleUpCallbackCount++;
			}
		}
	}

	public void Clear()
	{
		m_List.Clear();
		trickleDownCallbackCount = 0;
		bubbleUpCallbackCount = 0;
	}
}
