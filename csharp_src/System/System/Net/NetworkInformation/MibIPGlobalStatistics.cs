using System.Collections.Specialized;
using System.Globalization;

namespace System.Net.NetworkInformation;

internal class MibIPGlobalStatistics : IPGlobalStatistics
{
	private StringDictionary dic;

	public override int DefaultTtl => (int)Get("DefaultTTL");

	public override bool ForwardingEnabled => Get("Forwarding") != 0;

	public override int NumberOfInterfaces => (int)Get("NumIf");

	public override int NumberOfIPAddresses => (int)Get("NumAddr");

	public override int NumberOfRoutes => (int)Get("NumRoutes");

	public override long OutputPacketRequests => Get("OutRequests");

	public override long OutputPacketRoutingDiscards => Get("RoutingDiscards");

	public override long OutputPacketsDiscarded => Get("OutDiscards");

	public override long OutputPacketsWithNoRoute => Get("OutNoRoutes");

	public override long PacketFragmentFailures => Get("FragFails");

	public override long PacketReassembliesRequired => Get("ReasmReqds");

	public override long PacketReassemblyFailures => Get("ReasmFails");

	public override long PacketReassemblyTimeout => Get("ReasmTimeout");

	public override long PacketsFragmented => Get("FragOks");

	public override long PacketsReassembled => Get("ReasmOks");

	public override long ReceivedPackets => Get("InReceives");

	public override long ReceivedPacketsDelivered => Get("InDelivers");

	public override long ReceivedPacketsDiscarded => Get("InDiscards");

	public override long ReceivedPacketsForwarded => Get("ForwDatagrams");

	public override long ReceivedPacketsWithAddressErrors => Get("InAddrErrors");

	public override long ReceivedPacketsWithHeadersErrors => Get("InHdrErrors");

	public override long ReceivedPacketsWithUnknownProtocol => Get("InUnknownProtos");

	public MibIPGlobalStatistics(StringDictionary dic)
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
