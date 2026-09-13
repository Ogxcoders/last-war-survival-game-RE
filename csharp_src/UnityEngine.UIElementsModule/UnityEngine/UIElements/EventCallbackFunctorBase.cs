using System;

namespace UnityEngine.UIElements;

internal abstract class EventCallbackFunctorBase
{
	public CallbackPhase phase { get; private set; }

	protected EventCallbackFunctorBase(CallbackPhase phase)
	{
		this.phase = phase;
	}

	public abstract void Invoke(EventBase evt);

	public abstract bool IsEquivalentTo(long eventTypeId, Delegate callback, CallbackPhase phase);

	protected bool PhaseMatches(EventBase evt)
	{
		switch (phase)
		{
		case CallbackPhase.TrickleDownAndTarget:
			if (evt.propagationPhase != PropagationPhase.TrickleDown && evt.propagationPhase != PropagationPhase.AtTarget)
			{
				return false;
			}
			break;
		case CallbackPhase.TargetAndBubbleUp:
			if (evt.propagationPhase != PropagationPhase.AtTarget && evt.propagationPhase != PropagationPhase.BubbleUp)
			{
				return false;
			}
			break;
		}
		return true;
	}
}
