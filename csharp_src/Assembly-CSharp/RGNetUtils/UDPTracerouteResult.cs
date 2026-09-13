using System.Collections.Generic;
using System.Net;

namespace RGNetUtils;

public class UDPTracerouteResult
{
	public string HostName { get; set; }

	public IPAddress IpAddress { get; set; }

	public List<List<UDPTracerouteHopResult>> HopResults { get; set; }

	public UDPTracerouteResult()
	{
		HopResults = new List<List<UDPTracerouteHopResult>>();
	}

	public UDPTracerouteResult(IPAddress ipAddress)
		: this()
	{
		IpAddress = ipAddress;
	}
}
