#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public abstract class EventBase : IDisposable
{
	[Flags]
	internal enum EventPropagation
	{
		None = 0,
		Bubbles = 1,
		TricklesDown = 2,
		Cancellable = 4
	}

	[Flags]
	private enum LifeCycleStatus
	{
		None = 0,
		PropagationStopped = 1,
		ImmediatePropagationStopped = 2,
		DefaultPrevented = 4,
		Dispatching = 8,
		Pooled = 0x10,
		IMGUIEventIsValid = 0x20,
		StopDispatch = 0x40,
		PropagateToIMGUI = 0x80,
		Dispatched = 0x200,
		Processed = 0x400
	}

	private static long s_LastTypeId = 0L;

	private static ulong s_NextEventId = 0uL;

	private PropagationPaths m_Path;

	private IEventHandler m_Target;

	private IEventHandler m_CurrentTarget;

	private Event m_ImguiEvent;

	public virtual long eventTypeId => -1L;

	public long timestamp { get; private set; }

	internal ulong eventId { get; private set; }

	internal ulong triggerEventId { get; private set; }

	internal EventPropagation propagation { get; set; }

	internal PropagationPaths path
	{
		get
		{
			if (m_Path == null)
			{
				PropagationPaths.Type type = (tricklesDown ? PropagationPaths.Type.TrickleDown : PropagationPaths.Type.None);
				type = (PropagationPaths.Type)((int)type | (bubbles ? 2 : 0));
				m_Path = PropagationPaths.Build(leafTarget as VisualElement, type);
				EventDebugger.LogPropagationPaths(this, m_Path);
			}
			return m_Path;
		}
		set
		{
			if (value != null)
			{
				m_Path = PropagationPaths.Copy(value);
			}
		}
	}

	internal bool doNotSendToRootIMGUIContainer { get; set; }

	private LifeCycleStatus lifeCycleStatus { get; set; }

	public bool bubbles => (propagation & EventPropagation.Bubbles) != 0;

	public bool tricklesDown => (propagation & EventPropagation.TricklesDown) != 0;

	internal IEventHandler leafTarget { get; private set; }

	public IEventHandler target
	{
		get
		{
			return m_Target;
		}
		set
		{
			m_Target = value;
			if (leafTarget == null)
			{
				leafTarget = value;
			}
		}
	}

	internal List<IEventHandler> skipElements { get; } = new List<IEventHandler>();

	public bool isPropagationStopped
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.PropagationStopped) != 0;
		}
		private set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.PropagationStopped;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.PropagationStopped;
			}
		}
	}

	public bool isImmediatePropagationStopped
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.ImmediatePropagationStopped) != 0;
		}
		private set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.ImmediatePropagationStopped;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.ImmediatePropagationStopped;
			}
		}
	}

	public bool isDefaultPrevented
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.DefaultPrevented) != 0;
		}
		private set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.DefaultPrevented;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.DefaultPrevented;
			}
		}
	}

	public PropagationPhase propagationPhase { get; internal set; }

	public virtual IEventHandler currentTarget
	{
		get
		{
			return m_CurrentTarget;
		}
		internal set
		{
			m_CurrentTarget = value;
			if (imguiEvent != null)
			{
				if (currentTarget is VisualElement ele)
				{
					imguiEvent.mousePosition = ele.WorldToLocal(originalMousePosition);
				}
				else
				{
					imguiEvent.mousePosition = originalMousePosition;
				}
			}
		}
	}

	public bool dispatch
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.Dispatching) != 0;
		}
		internal set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.Dispatching;
				dispatched = true;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.Dispatching;
			}
		}
	}

	private bool dispatched
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.Dispatched) != 0;
		}
		set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.Dispatched;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.Dispatched;
			}
		}
	}

	internal bool processed
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.Processed) != 0;
		}
		private set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.Processed;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.Processed;
			}
		}
	}

	internal bool stopDispatch
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.StopDispatch) != 0;
		}
		set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.StopDispatch;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.StopDispatch;
			}
		}
	}

	internal bool propagateToIMGUI
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.PropagateToIMGUI) != 0;
		}
		set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.PropagateToIMGUI;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.PropagateToIMGUI;
			}
		}
	}

	private bool imguiEventIsValid
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.IMGUIEventIsValid) != 0;
		}
		set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.IMGUIEventIsValid;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.IMGUIEventIsValid;
			}
		}
	}

	public Event imguiEvent
	{
		get
		{
			return imguiEventIsValid ? m_ImguiEvent : null;
		}
		protected set
		{
			if (m_ImguiEvent == null)
			{
				m_ImguiEvent = new Event();
			}
			if (value != null)
			{
				m_ImguiEvent.CopyFrom(value);
				imguiEventIsValid = true;
				originalMousePosition = value.mousePosition;
			}
			else
			{
				imguiEventIsValid = false;
			}
		}
	}

	public Vector2 originalMousePosition { get; private set; }

	protected bool pooled
	{
		get
		{
			return (lifeCycleStatus & LifeCycleStatus.Pooled) != 0;
		}
		set
		{
			if (value)
			{
				lifeCycleStatus |= LifeCycleStatus.Pooled;
			}
			else
			{
				lifeCycleStatus &= ~LifeCycleStatus.Pooled;
			}
		}
	}

	protected static long RegisterEventType()
	{
		return ++s_LastTypeId;
	}

	internal void SetTriggerEventId(ulong id)
	{
		triggerEventId = id;
	}

	[Obsolete("Override PreDispatch(IPanel panel) instead.")]
	protected virtual void PreDispatch()
	{
	}

	protected internal virtual void PreDispatch(IPanel panel)
	{
		PreDispatch();
	}

	[Obsolete("Override PostDispatch(IPanel panel) instead.")]
	protected virtual void PostDispatch()
	{
	}

	protected internal virtual void PostDispatch(IPanel panel)
	{
		PostDispatch();
		processed = true;
	}

	internal bool Skip(IEventHandler h)
	{
		return skipElements.Contains(h);
	}

	public void StopPropagation()
	{
		isPropagationStopped = true;
	}

	public void StopImmediatePropagation()
	{
		isPropagationStopped = true;
		isImmediatePropagationStopped = true;
	}

	public void PreventDefault()
	{
		if ((propagation & EventPropagation.Cancellable) == EventPropagation.Cancellable)
		{
			isDefaultPrevented = true;
		}
	}

	internal void MarkReceivedByDispatcher()
	{
		Debug.Assert(!dispatched, "Events cannot be dispatched more than once.");
		dispatched = true;
	}

	protected virtual void Init()
	{
		LocalInit();
	}

	private void LocalInit()
	{
		timestamp = (long)(Time.realtimeSinceStartup * 1000f);
		triggerEventId = 0uL;
		eventId = s_NextEventId++;
		propagation = EventPropagation.None;
		m_Path?.Release();
		m_Path = null;
		leafTarget = null;
		target = null;
		skipElements.Clear();
		isPropagationStopped = false;
		isImmediatePropagationStopped = false;
		isDefaultPrevented = false;
		propagationPhase = PropagationPhase.None;
		originalMousePosition = Vector2.zero;
		m_CurrentTarget = null;
		dispatch = false;
		stopDispatch = false;
		propagateToIMGUI = true;
		dispatched = false;
		processed = false;
		imguiEventIsValid = false;
		pooled = false;
	}

	protected EventBase()
	{
		m_ImguiEvent = null;
		LocalInit();
	}

	internal abstract void Acquire();

	public abstract void Dispose();
}
public abstract class EventBase<T> : EventBase where T : EventBase<T>, new()
{
	private static readonly long s_TypeId = EventBase.RegisterEventType();

	private static readonly ObjectPool<T> s_Pool = new ObjectPool<T>();

	private int m_RefCount;

	public override long eventTypeId => s_TypeId;

	protected EventBase()
	{
		m_RefCount = 0;
	}

	public static long TypeId()
	{
		return s_TypeId;
	}

	protected override void Init()
	{
		base.Init();
		if (m_RefCount != 0)
		{
			Debug.Log("Event improperly released.");
			m_RefCount = 0;
		}
	}

	public static T GetPooled()
	{
		T val = s_Pool.Get();
		val.Init();
		val.pooled = true;
		val.Acquire();
		return val;
	}

	internal static T GetPooled(EventBase e)
	{
		T val = GetPooled();
		if (e != null)
		{
			val.SetTriggerEventId(e.eventId);
		}
		return val;
	}

	private static void ReleasePooled(T evt)
	{
		if (evt.pooled)
		{
			evt.Init();
			s_Pool.Release(evt);
			evt.pooled = false;
		}
	}

	internal override void Acquire()
	{
		m_RefCount++;
	}

	public sealed override void Dispose()
	{
		if (--m_RefCount == 0)
		{
			ReleasePooled((T)this);
		}
	}
}
