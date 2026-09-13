using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation;

internal class MibUdpStatistics : UdpStatistics
{
	private StringDictionary dic;

	public override long DatagramsReceived => Get("InDatagrams");

	public override long DatagramsSent => Get("OutDatagrams");

	public override long IncomingDatagramsDiscarded => Get("NoPorts");

	public override long IncomingDatagramsWithErrors => Get("InErrors");

	public override int UdpListeners => (int)Get("NumAddrs");

	public MibUdpStatistics(StringDictionary dic)
	{
		this.dic = dic;
	}

	private long Get(string name)
	{
		if (dic[name] == null)
		{
			return 0L;
		}
		return long.Parse(dic[name], NumberFormatInfo.InvariantInfo);
	}
}
