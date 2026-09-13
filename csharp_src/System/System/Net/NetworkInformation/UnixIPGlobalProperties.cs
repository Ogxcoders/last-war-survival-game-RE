namespace System.Net.NetworkInformation;

internal class UnixIPGlobalProperties : CommonUnixIPGlobalProperties
{
	public override TcpConnectionInformation[] GetActiveTcpConnections()
	{
		throw new NotImplementedException();
	}

	public override IPEndPoint[] GetActiveTcpListeners()
	{
		throw new NotImplementedException();
	}

	public override IPEndPoint[] GetActiveUdpListeners()
	{
		throw new NotImplementedException();
	}

	public override IcmpV4Statistics GetIcmpV4Statistics()
	{
		throw new NotImplementedException();
	}

	public override IcmpV6Statistics GetIcmpV6Statistics()
	{
		throw new NotImplementedException();
	}

	public override IPGlobalStatistics GetIPv4GlobalStatistics()
	{
		throw new NotImplementedException();
	}

	public override IPGlobalStatistics GetIPv6GlobalStatistics()
	{
		throw new NotImplementedException();
	}

	public override TcpStatistics GetTcpIPv4Statistics()
	{
		throw new NotImplementedException();
	}

	public override TcpStatistics GetTcpIPv6Statistics()
	{
		throw new NotImplementedException();
	}

	public override UdpStatistics GetUdpIPv4Statistics()
	{
		throw new NotImplementedException();
	}

	public override UdpStatistics GetUdpIPv6Statistics()
	{
		throw new NotImplementedException();
	}
}
