using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation;

internal class MibIcmpV4Statistics : IcmpV4Statistics
{
	private StringDictionary dic;

	public override long AddressMaskRepliesReceived => Get("InAddrMaskReps");

	public override long AddressMaskRepliesSent => Get("OutAddrMaskReps");

	public override long AddressMaskRequestsReceived => Get("InAddrMasks");

	public override long AddressMaskRequestsSent => Get("OutAddrMasks");

	public override long DestinationUnreachableMessagesReceived => Get("InDestUnreachs");

	public override long DestinationUnreachableMessagesSent => Get("OutDestUnreachs");

	public override long EchoRepliesReceived => Get("InEchoReps");

	public override long EchoRepliesSent => Get("OutEchoReps");

	public override long EchoRequestsReceived => Get("InEchos");

	public override long EchoRequestsSent => Get("OutEchos");

	public override long ErrorsReceived => Get("InErrors");

	public override long ErrorsSent => Get("OutErrors");

	public override long MessagesReceived => Get("InMsgs");

	public override long MessagesSent => Get("OutMsgs");

	public override long ParameterProblemsReceived => Get("InParmProbs");

	public override long ParameterProblemsSent => Get("OutParmProbs");

	public override long RedirectsReceived => Get("InRedirects");

	public override long RedirectsSent => Get("OutRedirects");

	public override long SourceQuenchesReceived => Get("InSrcQuenchs");

	public override long SourceQuenchesSent => Get("OutSrcQuenchs");

	public override long TimeExceededMessagesReceived => Get("InTimeExcds");

	public override long TimeExceededMessagesSent => Get("OutTimeExcds");

	public override long TimestampRepliesReceived => Get("InTimestampReps");

	public override long TimestampRepliesSent => Get("OutTimestampReps");

	public override long TimestampRequestsReceived => Get("InTimestamps");

	public override long TimestampRequestsSent => Get("OutTimestamps");

	public MibIcmpV4Statistics(StringDictionary dic)
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
