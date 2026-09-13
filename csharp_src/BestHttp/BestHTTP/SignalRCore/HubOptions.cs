using System;

namespace BestHTTP.SignalRCore;

public sealed class HubOptions
{
	public bool SkipNegotiation { get; set; }

	public TransportTypes PreferedTransport { get; set; }

	public TimeSpan PingInterval { get; set; }

	public int MaxRedirects { get; set; }

	public HubOptions()
	{
		SkipNegotiation = false;
		PreferedTransport = TransportTypes.WebSocket;
		PingInterval = TimeSpan.FromSeconds(15.0);
		MaxRedirects = 100;
	}
}
