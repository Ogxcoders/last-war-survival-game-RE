using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation;

internal class MibTcpStatistics : TcpStatistics
{
	private StringDictionary dic;

	public override long ConnectionsAccepted => Get("PassiveOpens");

	public override long ConnectionsInitiated => Get("ActiveOpens");

	public override long CumulativeConnections => Get("NumConns");

	public override long CurrentConnections => Get("CurrEstab");

	public override long ErrorsReceived => Get("InErrs");

	public override long FailedConnectionAttempts => Get("AttemptFails");

	public override long MaximumConnections => Get("MaxConn");

	public override long MaximumTransmissionTimeout => Get("RtoMax");

	public override long MinimumTransmissionTimeout => Get("RtoMin");

	public override long ResetConnections => Get("EstabResets");

	public override long ResetsSent => Get("OutRsts");

	public override long SegmentsReceived => Get("InSegs");

	public override long SegmentsResent => Get("RetransSegs");

	public override long SegmentsSent => Get("OutSegs");

	public MibTcpStatistics(StringDictionary dic)
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
