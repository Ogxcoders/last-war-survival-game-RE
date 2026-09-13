using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class EventDebugger
{
	private Dictionary<IPanel, List<EventDebuggerCallTrace>> m_EventCalledObjects;

	private Dictionary<IPanel, List<EventDebuggerDefaultActionTrace>> m_EventDefaultActionObjects;

	private Dictionary<IPanel, List<EventDebuggerPathTrace>> m_EventPathObjects;

	private Dictionary<IPanel, List<EventDebuggerTrace>> m_EventProcessedEvents;

	private Dictionary<IPanel, Stack<EventDebuggerTrace>> m_StackOfProcessedEvent;

	private readonly Dictionary<IPanel, long> m_ModificationCount;

	private readonly bool m_Log;

	public IPanel panel { get; set; }

	public void UpdateModificationCount()
	{
		if (panel != null)
		{
			long num = 0L;
			if (m_ModificationCount.ContainsKey(panel))
			{
				num = m_ModificationCount[panel];
			}
			num++;
			m_ModificationCount[panel] = num;
		}
	}

	public void BeginProcessEvent(EventBase evt, IEventHandler mouseCapture)
	{
		AddBeginProcessEvent(evt, mouseCapture);
		UpdateModificationCount();
	}

	public void EndProcessEvent(EventBase evt, long duration, IEventHandler mouseCapture)
	{
		AddEndProcessEvent(evt, duration, mouseCapture);
		UpdateModificationCount();
	}

	public void LogCall(int cbHashCode, string cbName, EventBase evt, bool propagationHasStopped, bool immediatePropagationHasStopped, bool defaultHasBeenPrevented, long duration, IEventHandler mouseCapture)
	{
		AddCallObject(cbHashCode, cbName, evt, propagationHasStopped, immediatePropagationHasStopped, defaultHasBeenPrevented, duration, mouseCapture);
		UpdateModificationCount();
	}

	public void LogIMGUICall(EventBase evt, long duration, IEventHandler mouseCapture)
	{
		AddIMGUICall(evt, duration, mouseCapture);
		UpdateModificationCount();
	}

	public void LogExecuteDefaultAction(EventBase evt, PropagationPhase phase, long duration, IEventHandler mouseCapture)
	{
		AddExecuteDefaultAction(evt, phase, duration, mouseCapture);
		UpdateModificationCount();
	}

	public static void LogPropagationPaths(EventBase evt, PropagationPaths paths)
	{
	}

	private void LogPropagationPathsInternal(EventBase evt, PropagationPaths paths)
	{
		PropagationPaths paths2 = ((paths == null) ? new PropagationPaths() : new PropagationPaths(paths));
		AddPropagationPaths(evt, paths2);
		UpdateModificationCount();
	}

	public List<EventDebuggerCallTrace> GetCalls(IPanel panel, EventDebuggerEventRecord evt = null)
	{
		List<EventDebuggerCallTrace> list = null;
		if (m_EventCalledObjects.ContainsKey(panel))
		{
			list = m_EventCalledObjects[panel];
		}
		if (evt != null && list != null)
		{
			List<EventDebuggerCallTrace> list2 = new List<EventDebuggerCallTrace>();
			foreach (EventDebuggerCallTrace item in list)
			{
				if (item.eventBase.eventId == evt.eventId)
				{
					list2.Add(item);
				}
			}
			list = list2;
		}
		return list;
	}

	public List<EventDebuggerDefaultActionTrace> GetDefaultActions(IPanel panel, EventDebuggerEventRecord evt = null)
	{
		List<EventDebuggerDefaultActionTrace> list = null;
		if (m_EventDefaultActionObjects.ContainsKey(panel))
		{
			list = m_EventDefaultActionObjects[panel];
		}
		if (evt != null && list != null)
		{
			List<EventDebuggerDefaultActionTrace> list2 = new List<EventDebuggerDefaultActionTrace>();
			foreach (EventDebuggerDefaultActionTrace item in list)
			{
				if (item.eventBase.eventId == evt.eventId)
				{
					list2.Add(item);
				}
			}
			list = list2;
		}
		return list;
	}

	public List<EventDebuggerPathTrace> GetPropagationPaths(IPanel panel, EventDebuggerEventRecord evt = null)
	{
		List<EventDebuggerPathTrace> list = null;
		if (m_EventPathObjects.ContainsKey(panel))
		{
			list = m_EventPathObjects[panel];
		}
		if (evt != null && list != null)
		{
			List<EventDebuggerPathTrace> list2 = new List<EventDebuggerPathTrace>();
			foreach (EventDebuggerPathTrace item in list)
			{
				if (item.eventBase.eventId == evt.eventId)
				{
					list2.Add(item);
				}
			}
			list = list2;
		}
		return list;
	}

	public List<EventDebuggerTrace> GetBeginEndProcessedEvents(IPanel panel, EventDebuggerEventRecord evt = null)
	{
		List<EventDebuggerTrace> list = null;
		if (m_EventProcessedEvents.ContainsKey(panel))
		{
			list = m_EventProcessedEvents[panel];
		}
		if (evt != null && list != null)
		{
			List<EventDebuggerTrace> list2 = new List<EventDebuggerTrace>();
			foreach (EventDebuggerTrace item in list)
			{
				if (item.eventBase.eventId == evt.eventId)
				{
					list2.Add(item);
				}
			}
			list = list2;
		}
		return list;
	}

	public long GetModificationCount(IPanel panel)
	{
		long result = -1L;
		if (panel != null && m_ModificationCount.ContainsKey(panel))
		{
			result = m_ModificationCount[panel];
		}
		return result;
	}

	public void ClearLogs()
	{
		UpdateModificationCount();
		if (panel == null)
		{
			m_EventCalledObjects.Clear();
			m_EventDefaultActionObjects.Clear();
			m_EventPathObjects.Clear();
			m_EventProcessedEvents.Clear();
			m_StackOfProcessedEvent.Clear();
		}
		else
		{
			m_EventCalledObjects.Remove(panel);
			m_EventDefaultActionObjects.Remove(panel);
			m_EventPathObjects.Remove(panel);
			m_EventProcessedEvents.Remove(panel);
			m_StackOfProcessedEvent.Remove(panel);
		}
	}

	public void ReplayEvents(List<EventDebuggerEventRecord> eventBases)
	{
		if (eventBases == null)
		{
			return;
		}
		foreach (EventDebuggerEventRecord eventBasis in eventBases)
		{
			Event obj = new Event
			{
				button = eventBasis.button,
				clickCount = eventBasis.clickCount,
				modifiers = eventBasis.modifiers,
				mousePosition = eventBasis.mousePosition
			};
			if (eventBasis.eventTypeId == EventBase<MouseMoveEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.MouseMove;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.MouseMove), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<MouseDownEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.MouseDown;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.MouseDown), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<MouseUpEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.MouseUp;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.MouseUp), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<ContextClickEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.ContextClick;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.ContextClick), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.MouseEnterWindow;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.MouseEnterWindow), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.MouseLeaveWindow;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.MouseLeaveWindow), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<WheelEvent>.TypeId() && eventBasis.hasUnderlyingPhysicalEvent)
			{
				obj.type = EventType.ScrollWheel;
				obj.delta = eventBasis.delta;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.ScrollWheel), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<KeyDownEvent>.TypeId())
			{
				obj.type = EventType.KeyDown;
				obj.character = eventBasis.character;
				obj.keyCode = eventBasis.keyCode;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.KeyDown), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<KeyUpEvent>.TypeId())
			{
				obj.type = EventType.KeyUp;
				obj.character = eventBasis.character;
				obj.keyCode = eventBasis.keyCode;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.KeyUp), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<DragUpdatedEvent>.TypeId())
			{
				obj.type = EventType.DragUpdated;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.DragUpdated), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<DragPerformEvent>.TypeId())
			{
				obj.type = EventType.DragPerform;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.DragPerform), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<DragExitedEvent>.TypeId())
			{
				obj.type = EventType.DragExited;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.DragExited), panel, DispatchMode.Default);
			}
			else if (eventBasis.eventTypeId == EventBase<ValidateCommandEvent>.TypeId())
			{
				obj.type = EventType.ValidateCommand;
				obj.commandName = eventBasis.commandName;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.ValidateCommand), panel, DispatchMode.Default);
			}
			else
			{
				if (eventBasis.eventTypeId != EventBase<ExecuteCommandEvent>.TypeId())
				{
					if (eventBasis.eventTypeId == EventBase<IMGUIEvent>.TypeId())
					{
						Debug.Log("Skipped IMGUI event (" + eventBasis.eventBaseName + "): " + eventBasis);
					}
					else
					{
						Debug.Log("Skipped event (" + eventBasis.eventBaseName + "): " + eventBasis);
					}
					continue;
				}
				obj.type = EventType.ExecuteCommand;
				obj.commandName = eventBasis.commandName;
				panel.dispatcher.Dispatch(UIElementsUtility.CreateEvent(obj, EventType.ExecuteCommand), panel, DispatchMode.Default);
			}
			Debug.Log("Replayed event (" + eventBasis.eventBaseName + "): " + obj);
		}
	}

	public Dictionary<string, long> ComputeHistogram(List<EventDebuggerEventRecord> eventBases)
	{
		if (panel == null || !m_EventProcessedEvents.ContainsKey(panel))
		{
			return null;
		}
		List<EventDebuggerTrace> list = m_EventProcessedEvents[panel];
		if (list == null)
		{
			return null;
		}
		Dictionary<string, long> dictionary = new Dictionary<string, long>();
		foreach (EventDebuggerTrace item in list)
		{
			if (eventBases == null || eventBases.Count == 0 || eventBases.Contains(item.eventBase))
			{
				string eventBaseName = item.eventBase.eventBaseName;
				long num = item.duration;
				if (dictionary.ContainsKey(eventBaseName))
				{
					long num2 = dictionary[eventBaseName];
					num += num2;
				}
				dictionary[eventBaseName] = num;
			}
		}
		return dictionary;
	}

	public EventDebugger()
	{
		m_EventCalledObjects = new Dictionary<IPanel, List<EventDebuggerCallTrace>>();
		m_EventDefaultActionObjects = new Dictionary<IPanel, List<EventDebuggerDefaultActionTrace>>();
		m_EventPathObjects = new Dictionary<IPanel, List<EventDebuggerPathTrace>>();
		m_StackOfProcessedEvent = new Dictionary<IPanel, Stack<EventDebuggerTrace>>();
		m_EventProcessedEvents = new Dictionary<IPanel, List<EventDebuggerTrace>>();
		m_ModificationCount = new Dictionary<IPanel, long>();
		m_Log = true;
	}

	private void AddCallObject(int cbHashCode, string cbName, EventBase evt, bool propagationHasStopped, bool immediatePropagationHasStopped, bool defaultHasBeenPrevented, long duration, IEventHandler mouseCapture)
	{
		if (m_Log)
		{
			EventDebuggerCallTrace item = new EventDebuggerCallTrace(panel, evt, cbHashCode, cbName, propagationHasStopped, immediatePropagationHasStopped, defaultHasBeenPrevented, duration, mouseCapture);
			List<EventDebuggerCallTrace> list;
			if (m_EventCalledObjects.ContainsKey(panel))
			{
				list = m_EventCalledObjects[panel];
			}
			else
			{
				list = new List<EventDebuggerCallTrace>();
				m_EventCalledObjects.Add(panel, list);
			}
			list.Add(item);
		}
	}

	private void AddExecuteDefaultAction(EventBase evt, PropagationPhase phase, long duration, IEventHandler mouseCapture)
	{
		if (m_Log)
		{
			EventDebuggerDefaultActionTrace item = new EventDebuggerDefaultActionTrace(panel, evt, phase, duration, mouseCapture);
			List<EventDebuggerDefaultActionTrace> list;
			if (m_EventDefaultActionObjects.ContainsKey(panel))
			{
				list = m_EventDefaultActionObjects[panel];
			}
			else
			{
				list = new List<EventDebuggerDefaultActionTrace>();
				m_EventDefaultActionObjects.Add(panel, list);
			}
			list.Add(item);
		}
	}

	private void AddPropagationPaths(EventBase evt, PropagationPaths paths)
	{
		if (m_Log)
		{
			EventDebuggerPathTrace item = new EventDebuggerPathTrace(panel, evt, paths);
			List<EventDebuggerPathTrace> list;
			if (m_EventPathObjects.ContainsKey(panel))
			{
				list = m_EventPathObjects[panel];
			}
			else
			{
				list = new List<EventDebuggerPathTrace>();
				m_EventPathObjects.Add(panel, list);
			}
			list.Add(item);
		}
	}

	private void AddIMGUICall(EventBase evt, long duration, IEventHandler mouseCapture)
	{
		if (m_Log)
		{
			EventDebuggerCallTrace item = new EventDebuggerCallTrace(panel, evt, 0, "OnGUI", propagationHasStopped: false, immediatePropagationHasStopped: false, defaultHasBeenPrevented: false, duration, mouseCapture);
			List<EventDebuggerCallTrace> list;
			if (m_EventCalledObjects.ContainsKey(panel))
			{
				list = m_EventCalledObjects[panel];
			}
			else
			{
				list = new List<EventDebuggerCallTrace>();
				m_EventCalledObjects.Add(panel, list);
			}
			list.Add(item);
		}
	}

	private void AddBeginProcessEvent(EventBase evt, IEventHandler mouseCapture)
	{
		EventDebuggerTrace item = new EventDebuggerTrace(panel, evt, -1L, mouseCapture);
		Stack<EventDebuggerTrace> stack;
		if (m_StackOfProcessedEvent.ContainsKey(panel))
		{
			stack = m_StackOfProcessedEvent[panel];
		}
		else
		{
			stack = new Stack<EventDebuggerTrace>();
			m_StackOfProcessedEvent.Add(panel, stack);
		}
		List<EventDebuggerTrace> list;
		if (m_EventProcessedEvents.ContainsKey(panel))
		{
			list = m_EventProcessedEvents[panel];
		}
		else
		{
			list = new List<EventDebuggerTrace>();
			m_EventProcessedEvents.Add(panel, list);
		}
		list.Add(item);
		stack.Push(item);
	}

	private void AddEndProcessEvent(EventBase evt, long duration, IEventHandler mouseCapture)
	{
		bool flag = false;
		if (m_StackOfProcessedEvent.ContainsKey(panel))
		{
			Stack<EventDebuggerTrace> stack = m_StackOfProcessedEvent[panel];
			if (stack.Count > 0)
			{
				EventDebuggerTrace eventDebuggerTrace = stack.Peek();
				if (eventDebuggerTrace.eventBase.eventId == evt.eventId)
				{
					stack.Pop();
					eventDebuggerTrace.duration = duration;
					flag = true;
				}
			}
		}
		if (!flag)
		{
			EventDebuggerTrace item = new EventDebuggerTrace(panel, evt, duration, mouseCapture);
			List<EventDebuggerTrace> list;
			if (m_EventProcessedEvents.ContainsKey(panel))
			{
				list = m_EventProcessedEvents[panel];
			}
			else
			{
				list = new List<EventDebuggerTrace>();
				m_EventProcessedEvents.Add(panel, list);
			}
			list.Add(item);
		}
	}

	public static string GetObjectDisplayName(object obj, bool withHashCode = true)
	{
		if (obj == null)
		{
			return string.Empty;
		}
		string text = obj.GetType().Name;
		if (obj is VisualElement)
		{
			VisualElement visualElement = obj as VisualElement;
			if (!string.IsNullOrEmpty(visualElement.name))
			{
				text = text + "#" + visualElement.name;
			}
		}
		if (withHashCode)
		{
			text = text + " (" + obj.GetHashCode().ToString("x8") + ")";
		}
		return text;
	}
}
