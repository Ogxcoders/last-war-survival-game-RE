namespace System.Runtime;

internal struct TracePayload
{
	private string serializedException;

	private string eventSource;

	private string appDomainFriendlyName;

	private string extendedData;

	private string hostReference;

	public string SerializedException => serializedException;

	public string EventSource => eventSource;

	public string AppDomainFriendlyName => appDomainFriendlyName;

	public string ExtendedData => extendedData;

	public string HostReference => hostReference;

	public TracePayload(string serializedException, string eventSource, string appDomainFriendlyName, string extendedData, string hostReference)
	{
		this.serializedException = serializedException;
		this.eventSource = eventSource;
		this.appDomainFriendlyName = appDomainFriendlyName;
		this.extendedData = extendedData;
		this.hostReference = hostReference;
	}
}
