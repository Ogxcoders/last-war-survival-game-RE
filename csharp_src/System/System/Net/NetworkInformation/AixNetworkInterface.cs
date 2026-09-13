namespace System.Net.NetworkInformation;

internal sealed class AixNetworkInterface : UnixNetworkInterface
{
	private uint _ifa_flags;

	private int _ifru_mtu;

	public override OperationalStatus OperationalStatus
	{
		get
		{
			if ((_ifa_flags & 1) == 1)
			{
				return OperationalStatus.Up;
			}
			return OperationalStatus.Unknown;
		}
	}

	public override bool SupportsMulticast => (_ifa_flags & 0x8000000) == 134217728;

	internal AixNetworkInterface(string name, uint ifa_flags, int ifru_mtu)
		: base(name)
	{
		_ifa_flags = ifa_flags;
		_ifru_mtu = ifru_mtu;
	}

	public override IPInterfaceProperties GetIPProperties()
	{
		if (ipproperties == null)
		{
			ipproperties = new AixIPInterfaceProperties(this, addresses, _ifru_mtu);
		}
		return ipproperties;
	}

	public override IPv4InterfaceStatistics GetIPv4Statistics()
	{
		if (ipv4stats == null)
		{
			ipv4stats = new AixIPv4InterfaceStatistics(this);
		}
		return ipv4stats;
	}
}
