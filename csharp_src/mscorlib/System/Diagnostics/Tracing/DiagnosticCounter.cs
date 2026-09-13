namespace System.Diagnostics.Tracing;

public abstract class DiagnosticCounter : IDisposable
{
	public string DisplayName { get; set; }

	public string DisplayUnits { get; set; }

	public EventSource EventSource { get; }

	public string Name { get; }

	internal DiagnosticCounter(string name, EventSource eventSource)
	{
	}

	internal DiagnosticCounter()
	{
	}

	public void AddMetadata(string key, string value)
	{
	}

	public void Dispose()
	{
	}
}
