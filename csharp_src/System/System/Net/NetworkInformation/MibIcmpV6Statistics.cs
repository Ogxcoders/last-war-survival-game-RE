using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation;

internal class MibIcmpV6Statistics : IcmpV6Statistics
{
	private StringDictionary dic;

	public override long DestinationUnreachableMessagesReceived => Get("InDestUnreachs");

	public override long DestinationUnreachableMessagesSent => Get("OutDestUnreachs");

	public override long EchoRepliesReceived => Get("InEchoReplies");

	public override long EchoRepliesSent => Get("OutEchoReplies");

	public override long EchoRequestsReceived => Get("InEchos");

	public override long EchoRequestsSent => Get("OutEchos");

	public override long ErrorsReceived => Get("InErrors");

	public override long ErrorsSent => Get("OutErrors");

	public override long MembershipQueriesReceived => Get("InGroupMembQueries");

	public override long MembershipQueriesSent => Get("OutGroupMembQueries");

	public override long MembershipReductionsReceived => Get("InGroupMembReductiions");

	public override long MembershipReductionsSent => Get("OutGroupMembReductiions");

	public override long MembershipReportsReceived => Get("InGroupMembRespons");

	public override long MembershipReportsSent => Get("OutGroupMembRespons");

	public override long MessagesReceived => Get("InMsgs");

	public override long MessagesSent => Get("OutMsgs");

	public override long NeighborAdvertisementsReceived => Get("InNeighborAdvertisements");

	public override long NeighborAdvertisementsSent => Get("OutNeighborAdvertisements");

	public override long NeighborSolicitsReceived => Get("InNeighborSolicits");

	public override long NeighborSolicitsSent => Get("OutNeighborSolicits");

	public override long PacketTooBigMessagesReceived => Get("InPktTooBigs");

	public override long PacketTooBigMessagesSent => Get("OutPktTooBigs");

	public override long ParameterProblemsReceived => Get("InParmProblems");

	public override long ParameterProblemsSent => Get("OutParmProblems");

	public override long RedirectsReceived => Get("InRedirects");

	public override long RedirectsSent => Get("OutRedirects");

	public override long RouterAdvertisementsReceived => Get("InRouterAdvertisements");

	public override long RouterAdvertisementsSent => Get("OutRouterAdvertisements");

	public override long RouterSolicitsReceived => Get("InRouterSolicits");

	public override long RouterSolicitsSent => Get("OutRouterSolicits");

	public override long TimeExceededMessagesReceived => Get("InTimeExcds");

	public override long TimeExceededMessagesSent => Get("OutTimeExcds");

	public MibIcmpV6Statistics(StringDictionary dic)
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
