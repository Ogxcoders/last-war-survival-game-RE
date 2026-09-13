#define UNITY_ASSERTIONS
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public sealed class EventDispatcher
{
	private struct EventRecord
	{
		public EventBase m_Event;

		public IPanel m_Panel;
	}

	private struct DispatchContext
	{
		public uint m_GateCount;

		public Queue<EventRecord> m_Queue;
	}

	private List<IEventDispatchingStrategy> m_DispatchingStrategies;

	private static readonly ObjectPool<Queue<EventRecord>> k_EventQueuePool = new ObjectPool<Queue<EventRecord>>();

	private Queue<EventRecord> m_Queue;

	private uint m_GateCount;

	private Stack<DispatchContext> m_DispatchContexts = new Stack<DispatchContext>();

	private bool m_Immediate = false;

	internal PointerDispatchState pointerState { get; } = new PointerDispatchState();

	private bool dispatchImmediately => m_Immediate || m_GateCount == 0;

	internal EventDispatcher()
	{
		m_DispatchingStrategies = new List<IEventDispatchingStrategy>();
		m_DispatchingStrategies.Add(new PointerCaptureDispatchingStrategy());
		m_DispatchingStrategies.Add(new MouseCaptureDispatchingStrategy());
		m_DispatchingStrategies.Add(new KeyboardEventDispatchingStrategy());
		m_DispatchingStrategies.Add(new PointerEventDispatchingStrategy());
		m_DispatchingStrategies.Add(new MouseEventDispatchingStrategy());
		m_DispatchingStrategies.Add(new CommandEventDispatchingStrategy());
		m_DispatchingStrategies.Add(new IMGUIEventDispatchingStrategy());
		m_DispatchingStrategies.Add(new DefaultDispatchingStrategy());
		m_Queue = k_EventQueuePool.Get();
	}

	internal void Dispatch(EventBase evt, IPanel panel, DispatchMode dispatchMode)
	{
		evt.MarkReceivedByDispatcher();
		if (evt.eventTypeId == EventBase<IMGUIEvent>.TypeId())
		{
			Event imguiEvent = evt.imguiEvent;
			if (imguiEvent.rawType == EventType.Repaint)
			{
				return;
			}
		}
		if (dispatchImmediately || dispatchMode == DispatchMode.Immediate)
		{
			ProcessEvent(evt, panel);
			return;
		}
		evt.Acquire();
		m_Queue.Enqueue(new EventRecord
		{
			m_Event = evt,
			m_Panel = panel
		});
	}

	internal void PushDispatcherContext()
	{
		m_DispatchContexts.Push(new DispatchContext
		{
			m_GateCount = m_GateCount,
			m_Queue = m_Queue
		});
		m_GateCount = 0u;
		m_Queue = k_EventQueuePool.Get();
	}

	internal void PopDispatcherContext()
	{
		Debug.Assert(m_GateCount == 0, "All gates should have been opened before popping dispatch context.");
		Debug.Assert(m_Queue.Count == 0, "Queue should be empty when popping dispatch context.");
		k_EventQueuePool.Release(m_Queue);
		m_GateCount = m_DispatchContexts.Peek().m_GateCount;
		m_Queue = m_DispatchContexts.Peek().m_Queue;
		m_DispatchContexts.Pop();
	}

	internal void CloseGate()
	{
		m_GateCount++;
	}

	internal void OpenGate()
	{
		Debug.Assert(m_GateCount != 0);
		if (m_GateCount != 0)
		{
			m_GateCount--;
		}
		if (m_GateCount == 0)
		{
			ProcessEventQueue();
		}
	}

	private void ProcessEventQueue()
	{
		Queue<EventRecord> queue = m_Queue;
		m_Queue = k_EventQueuePool.Get();
		ExitGUIException ex = null;
		try
		{
			while (queue.Count > 0)
			{
				EventRecord eventRecord = queue.Dequeue();
				EventBase eventBase = eventRecord.m_Event;
				IPanel panel = eventRecord.m_Panel;
				try
				{
					ProcessEvent(eventBase, panel);
				}
				catch (ExitGUIException ex2)
				{
					Debug.Assert(ex == null);
					ex = ex2;
				}
				finally
				{
					eventBase.Dispose();
				}
			}
		}
		finally
		{
			k_EventQueuePool.Release(queue);
		}
		if (ex != null)
		{
			throw ex;
		}
	}

	private void ProcessEvent(EventBase evt, IPanel panel)
	{
		Event imguiEvent = evt.imguiEvent;
		bool flag = imguiEvent != null && imguiEvent.rawType == EventType.Used;
		using (new EventDispatcherGate(this))
		{
			evt.PreDispatch(panel);
			if (!evt.stopDispatch && !evt.isPropagationStopped)
			{
				ApplyDispatchingStrategies(evt, panel, flag);
			}
			if (evt.path != null)
			{
				foreach (VisualElement targetElement in evt.path.targetElements)
				{
					evt.target = targetElement;
					EventDispatchUtilities.ExecuteDefaultAction(evt, panel);
				}
				evt.target = evt.leafTarget;
			}
			else
			{
				EventDispatchUtilities.ExecuteDefaultAction(evt, panel);
			}
			evt.PostDispatch(panel);
			Debug.Assert(flag || evt.isPropagationStopped || imguiEvent == null || imguiEvent.rawType != EventType.Used, "Event is used but not stopped.");
		}
	}

	private void ApplyDispatchingStrategies(EventBase evt, IPanel panel, bool imguiEventIsInitiallyUsed)
	{
		foreach (IEventDispatchingStrategy dispatchingStrategy in m_DispatchingStrategies)
		{
			if (dispatchingStrategy.CanDispatchEvent(evt))
			{
				dispatchingStrategy.DispatchEvent(evt, panel);
				Debug.Assert(imguiEventIsInitiallyUsed || evt.isPropagationStopped || evt.imguiEvent == null || evt.imguiEvent.rawType != EventType.Used, "Unexpected condition: !evt.isPropagationStopped && evt.imguiEvent.rawType == EventType.Used.");
				if (evt.stopDispatch || evt.isPropagationStopped)
				{
					break;
				}
			}
		}
	}
}
