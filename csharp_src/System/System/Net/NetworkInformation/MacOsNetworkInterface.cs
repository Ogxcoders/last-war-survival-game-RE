namespace System.Net.NetworkInformation;

internal sealed class MacOsNetworkInterface : UnixNetworkInterface
{
	private uint _ifa_flags;

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

	public override bool SupportsMulticast => (_ifa_flags & 0x8000) == 32768;

	internal MacOsNetworkInterface(string name, uint ifa_flags)
		: base(name)
	{
		_ifa_flags = ifa_flags;
	}

	public override IPInterfaceProperties GetIPProperties()
	{
		if (ipproperties == null)
		{
			ipproperties = new MacOsIPInterfaceProperties(this, addresses);
		}
		return ipproperties;
	}

	public override IPv4InterfaceStatistics GetIPv4Statistics()
	{
		if (ipv4stats == null)
		{
			ipv4stats = new MacOsIPv4InterfaceStatistics(this);
		}
		return ipv4stats;
	}
}
