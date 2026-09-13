using System;

namespace UnityEngine.UIElements;

internal class EventCallbackFunctor<TEventType> : EventCallbackFunctorBase where TEventType : EventBase<TEventType>, new()
{
	private readonly EventCallback<TEventType> m_Callback;

	private readonly long m_EventTypeId;

	public EventCallbackFunctor(EventCallback<TEventType> callback, CallbackPhase phase)
		: base(phase)
	{
		m_Callback = callback;
		m_EventTypeId = EventBase<TEventType>.TypeId();
	}

	public override void Invoke(EventBase evt)
	{
		if (evt == null)
		{
			throw new ArgumentNullException("evt");
		}
		if (evt.eventTypeId != m_EventTypeId || !PhaseMatches(evt))
		{
			return;
		}
		using (new EventDebuggerLogCall(m_Callback, evt))
		{
			m_Callback(evt as TEventType);
		}
	}

	public override bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase)
	{
		return m_EventTypeId == eventTypeId && m_Callback == callback && base.phase == phase;
	}
}
internal class EventCallbackFunctor<TEventType, TCallbackArgs> : EventCallbackFunctorBase where TEventType : EventBase<TEventType>, new()
{
	private readonly EventCallback<TEventType, TCallbackArgs> m_Callback;

	private readonly long m_EventTypeId;

	internal TCallbackArgs userArgs { get; set; }

	public EventCallbackFunctor(EventCallback<TEventType, TCallbackArgs> callback, TCallbackArgs userArgs, CallbackPhase phase)
		: base(phase)
	{
		this.userArgs = userArgs;
		m_Callback = callback;
		m_EventTypeId = EventBase<TEventType>.TypeId();
	}

	public override void Invoke(EventBase evt)
	{
		if (evt == null)
		{
			throw new ArgumentNullException("evt");
		}
		if (evt.eventTypeId != m_EventTypeId || !PhaseMatches(evt))
		{
			return;
		}
		using (new EventDebuggerLogCall(m_Callback, evt))
		{
			m_Callback(evt as TEventType, userArgs);
		}
	}

	public override bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase)
	{
		return m_EventTypeId == eventTypeId && m_Callback == callback && base.phase == phase;
	}
}
