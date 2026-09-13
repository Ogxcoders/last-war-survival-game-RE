using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class EventCallbackListPool
{
	private readonly Stack<EventCallbackList> m_Stack = new Stack<EventCallbackList>();

	public EventCallbackList Get(EventCallbackList initializer)
	{
		EventCallbackList eventCallbackList;
		if (m_Stack.Count == 0)
		{
			eventCallbackList = ((initializer == null) ? new EventCallbackList() : new EventCallbackList(initializer));
		}
		else
		{
			eventCallbackList = m_Stack.Pop();
			if (initializer != null)
			{
				eventCallbackList.AddRange(initializer);
			}
		}
		return eventCallbackList;
	}

	public void Release(EventCallbackList element)
	{
		element.Clear();
		m_Stack.Push(element);
	}
}
