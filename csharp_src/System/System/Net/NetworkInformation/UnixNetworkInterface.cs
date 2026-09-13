using System.Collections.Generic;
using System.Net.Sockets;

namespace System.Net.NetworkInformation;

internal abstract class UnixNetworkInterface : NetworkInterface
{
	protected IPv4InterfaceStatistics ipv4stats;

	protected IPInterfaceProperties ipproperties;

	private string name;

	protected List<IPAddress> addresses;

	private byte[] macAddress;

	private NetworkInterfaceType type;

	public override string Description => name;

	public override string Id => name;

	public override bool IsReceiveOnly => false;

	public override string Name => name;

	public override NetworkInterfaceType NetworkInterfaceType => type;

	[System.MonoTODO("Parse dmesg?")]
	public override long Speed => 1000000L;

	internal int NameIndex => UnixNetworkInterfaceAPI.if_nametoindex(Name);

	internal UnixNetworkInterface(string name)
	{
		this.name = name;
		addresses = new List<IPAddress>();
	}

	internal void AddAddress(IPAddress address)
	{
		addresses.Add(address);
	}

	internal void SetLinkLayerInfo(int index, byte[] macAddress, NetworkInterfaceType type)
	{
		this.macAddress = macAddress;
		this.type = type;
	}

	public override PhysicalAddress GetPhysicalAddress()
	{
		if (macAddress != null)
		{
			return new PhysicalAddress(macAddress);
		}
		return PhysicalAddress.None;
	}

	public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
	{
		bool flag = networkInterfaceComponent == NetworkInterfaceComponent.IPv4;
		bool flag2 = !flag && networkInterfaceComponent == NetworkInterfaceComponent.IPv6;
		foreach (IPAddress address in addresses)
		{
			if (flag && address.AddressFamily == AddressFamily.InterNetwork)
			{
				return true;
			}
			if (flag2 && address.AddressFamily == AddressFamily.InterNetworkV6)
			{
				return true;
			}
		}
		return false;
	}
}
