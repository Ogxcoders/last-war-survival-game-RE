namespace UnityEngine.UIElements;

internal class EventDebuggerCallTrace : EventDebuggerTrace
{
	public int callbackHashCode { get; }

	public string callbackName { get; }

	public bool propagationHasStopped { get; }

	public bool immediatePropagationHasStopped { get; }

	public bool defaultHasBeenPrevented { get; }

	public EventDebuggerCallTrace(IPanel panel, EventBase evt, int cbHashCode, string cbName, bool propagationHasStopped, bool immediatePropagationHasStopped, bool defaultHasBeenPrevented, long duration, IEventHandler mouseCapture)
		: base(panel, evt, duration, mouseCapture)
	{
		callbackHashCode = cbHashCode;
		callbackName = cbName;
		this.propagationHasStopped = propagationHasStopped;
		this.immediatePropagationHasStopped = immediatePropagationHasStopped;
		this.defaultHasBeenPrevented = defaultHasBeenPrevented;
	}
}
