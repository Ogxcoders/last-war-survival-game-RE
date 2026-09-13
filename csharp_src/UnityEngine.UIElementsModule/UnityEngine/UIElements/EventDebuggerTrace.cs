namespace UnityEngine.UIElements;

internal class EventDebuggerTrace
{
	public EventDebuggerEventRecord eventBase { get; }

	public IEventHandler focusedElement { get; }

	public IEventHandler mouseCapture { get; }

	public long duration { get; set; }

	public EventDebuggerTrace(IPanel panel, EventBase evt, long duration, IEventHandler mouseCapture)
	{
		eventBase = new EventDebuggerEventRecord(evt);
		focusedElement = panel?.focusController?.focusedElement;
		this.mouseCapture = mouseCapture;
		this.duration = duration;
	}
}
