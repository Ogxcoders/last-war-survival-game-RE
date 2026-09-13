using System.Net;

namespace RGNetUtils;

public class UDPTracerouteHopResult
{
	public bool IsSuccess { get; set; }

	public string ErrorStr { get; set; }

	public double ArriveTime { get; set; }

	public int BytesRead { get; set; }

	public int IcmpType { get; set; }

	public int IcmpCode { get; set; }

	public IPEndPoint FromEndPoint { get; set; }

	public UDPTracerouteHopResult()
	{
		IsSuccess = false;
		ArriveTime = 0.0;
		BytesRead = 0;
	}

	public void SetTimeout()
	{
		IsSuccess = false;
		ErrorStr = "Timeout";
	}
}
