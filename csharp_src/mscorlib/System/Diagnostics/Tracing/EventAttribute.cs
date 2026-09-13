namespace System.Diagnostics.Tracing;

[AttributeUsage(AttributeTargets.Method)]
public sealed class EventAttribute : Attribute
{
	public int EventId { get; private set; }

	public EventActivityOptions ActivityOptions { get; set; }

	public EventLevel Level { get; set; }

	public EventKeywords Keywords { get; set; }

	public EventOpcode Opcode { get; set; }

	public EventChannel Channel { get; set; }

	public string Message { get; set; }

	public EventTask Task { get; set; }

	public EventTags Tags { get; set; }

	public byte Version { get; set; }

	public EventAttribute(int eventId)
	{
		EventId = eventId;
	}
}
