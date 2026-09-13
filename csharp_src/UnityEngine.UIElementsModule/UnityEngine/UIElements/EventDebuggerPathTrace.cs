namespace UnityEngine.UIElements;

internal class EventDebuggerPathTrace : EventDebuggerTrace
{
	public PropagationPaths paths { get; }

	public EventDebuggerPathTrace(IPanel panel, EventBase evt, PropagationPaths paths)
		: base(panel, evt, -1L, null)
	{
		this.paths = paths;
	}
}
