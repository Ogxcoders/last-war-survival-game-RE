using System.Collections.Generic;

namespace BestHTTP.SignalRCore.Messages;

public sealed class SupportedTransport
{
	public string Name { get; private set; }

	public List<string> SupportedFormats { get; private set; }

	internal SupportedTransport(string transportName, List<string> transferFormats)
	{
		Name = transportName;
		SupportedFormats = transferFormats;
	}
}
