namespace System.Net.NetworkInformation;

internal class AixIPv4InterfaceStatistics : IPv4InterfaceStatistics
{
	public override long BytesReceived => 0L;

	public override long BytesSent => 0L;

	public override long IncomingPacketsDiscarded => 0L;

	public override long IncomingPacketsWithErrors => 0L;

	public override long IncomingUnknownProtocolPackets => 0L;

	public override long NonUnicastPacketsReceived => 0L;

	public override long NonUnicastPacketsSent => 0L;

	public override long OutgoingPacketsDiscarded => 0L;

	public override long OutgoingPacketsWithErrors => 0L;

	public override long OutputQueueLength => 0L;

	public override long UnicastPacketsReceived => 0L;

	public override long UnicastPacketsSent => 0L;

	public AixIPv4InterfaceStatistics(AixNetworkInterface parent)
	{
	}
}
