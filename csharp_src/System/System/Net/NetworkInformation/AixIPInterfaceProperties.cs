using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Net.NetworkInformation;

internal class AixIPInterfaceProperties : UnixIPInterfaceProperties
{
	private int _mtu;

	public override GatewayIPAddressInformationCollection GatewayAddresses
	{
		get
		{
			IPAddressCollection iPAddressCollection = new IPAddressCollection();
			if (!ParseRouteInfo_icall(iface.Name.ToString(), out var gw_addr_list))
			{
				return new GatewayIPAddressInformationCollection();
			}
			for (int i = 0; i < gw_addr_list.Length; i++)
			{
				try
				{
					IPAddress iPAddress = IPAddress.Parse(gw_addr_list[i]);
					if (!iPAddress.Equals(IPAddress.Any) && !iPAddressCollection.Contains(iPAddress))
					{
						iPAddressCollection.InternalAdd(iPAddress);
					}
				}
				catch (ArgumentNullException)
				{
				}
			}
			return SystemGatewayIPAddressInformation.ToGatewayIpAddressInformationCollection(iPAddressCollection);
		}
	}

	public AixIPInterfaceProperties(AixNetworkInterface iface, List<IPAddress> addresses)
		: this(iface, addresses, 0)
	{
	}

	public AixIPInterfaceProperties(AixNetworkInterface iface, List<IPAddress> addresses, int mtu)
		: base(iface, addresses)
	{
		_mtu = mtu;
	}

	public override IPv4InterfaceProperties GetIPv4Properties()
	{
		if (ipv4iface_properties == null)
		{
			ipv4iface_properties = new AixIPv4InterfaceProperties(iface as AixNetworkInterface, _mtu);
		}
		return ipv4iface_properties;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool ParseRouteInfo_icall(string iface, out string[] gw_addr_list);
}
