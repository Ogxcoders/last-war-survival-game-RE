namespace UnityEngine.UIElements;

internal class EventDebuggerDefaultActionTrace : EventDebuggerTrace
{
	public PropagationPhase phase { get; }

	public string targetName => base.eventBase.target.GetType().FullName;

	public EventDebuggerDefaultActionTrace(IPanel panel, EventBase evt, PropagationPhase phase, long duration, IEventHandler mouseCapture)
		: base(panel, evt, duration, mouseCapture)
	{
		this.phase = phase;
	}
}
