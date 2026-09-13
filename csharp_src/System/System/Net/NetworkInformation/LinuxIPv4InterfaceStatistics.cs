namespace System.Net.NetworkInformation;

internal class LinuxIPv4InterfaceStatistics : IPv4InterfaceStatistics
{
	private LinuxNetworkInterface linux;

	public override long BytesReceived => Read("statistics/rx_bytes");

	public override long BytesSent => Read("statistics/tx_bytes");

	public override long IncomingPacketsDiscarded => Read("statistics/rx_dropped");

	public override long IncomingPacketsWithErrors => Read("statistics/rx_errors");

	public override long IncomingUnknownProtocolPackets => 0L;

	public override long NonUnicastPacketsReceived => Read("statistics/multicast");

	public override long NonUnicastPacketsSent => Read("statistics/multicast");

	public override long OutgoingPacketsDiscarded => Read("statistics/tx_dropped");

	public override long OutgoingPacketsWithErrors => Read("statistics/tx_errors");

	public override long OutputQueueLength => 1024L;

	public override long UnicastPacketsReceived => Read("statistics/rx_packets");

	public override long UnicastPacketsSent => Read("statistics/tx_packets");

	public LinuxIPv4InterfaceStatistics(LinuxNetworkInterface parent)
	{
		linux = parent;
	}

	private long Read(string file)
	{
		try
		{
			return long.Parse(LinuxNetworkInterface.ReadLine(linux.IfacePath + file));
		}
		catch
		{
			return 0L;
		}
	}
}
