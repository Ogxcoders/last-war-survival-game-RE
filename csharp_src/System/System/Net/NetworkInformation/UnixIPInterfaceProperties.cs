using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal abstract class UnixIPInterfaceProperties : IPInterfaceProperties
{
	protected IPv4InterfaceProperties ipv4iface_properties;

	protected UnixNetworkInterface iface;

	private List<IPAddress> addresses;

	private IPAddressCollection dns_servers;

	public override IPAddressInformationCollection AnycastAddresses
	{
		get
		{
			IPAddressInformationCollection iPAddressInformationCollection = new IPAddressInformationCollection();
			foreach (IPAddress address in addresses)
			{
				iPAddressInformationCollection.InternalAdd(new SystemIPAddressInformation(address, isDnsEligible: false, isTransient: false));
			}
			return iPAddressInformationCollection;
		}
	}

	[System.MonoTODO("Always returns an empty collection.")]
	public override IPAddressCollection DhcpServerAddresses => new IPAddressCollection();

	public override IPAddressCollection DnsAddresses
	{
		get
		{
			GetDNSServersFromOS();
			return dns_servers;
		}
	}

	public override string DnsSuffix => string.Empty;

	[System.MonoTODO("Always returns true")]
	public override bool IsDnsEnabled => true;

	[System.MonoTODO("Always returns false")]
	public override bool IsDynamicDnsEnabled => false;

	public override MulticastIPAddressInformationCollection MulticastAddresses
	{
		get
		{
			MulticastIPAddressInformationCollection multicastIPAddressInformationCollection = new MulticastIPAddressInformationCollection();
			foreach (IPAddress address in addresses)
			{
				byte[] addressBytes = address.GetAddressBytes();
				if (addressBytes[0] >= 224 && addressBytes[0] <= 239)
				{
					multicastIPAddressInformationCollection.InternalAdd(new SystemMulticastIPAddressInformation(new SystemIPAddressInformation(address, isDnsEligible: true, isTransient: false)));
				}
			}
			return multicastIPAddressInformationCollection;
		}
	}

	public override UnicastIPAddressInformationCollection UnicastAddresses
	{
		get
		{
			UnicastIPAddressInformationCollection unicastIPAddressInformationCollection = new UnicastIPAddressInformationCollection();
			foreach (IPAddress address in addresses)
			{
				switch (address.AddressFamily)
				{
				case AddressFamily.InterNetwork:
				{
					byte b = address.GetAddressBytes()[0];
					if (b < 224 || b > 239)
					{
						unicastIPAddressInformationCollection.InternalAdd(new LinuxUnicastIPAddressInformation(address));
					}
					break;
				}
				case AddressFamily.InterNetworkV6:
					if (!address.IsIPv6Multicast)
					{
						unicastIPAddressInformationCollection.InternalAdd(new LinuxUnicastIPAddressInformation(address));
					}
					break;
				}
			}
			return unicastIPAddressInformationCollection;
		}
	}

	[System.MonoTODO("Always returns an empty collection.")]
	public override IPAddressCollection WinsServersAddresses => new IPAddressCollection();

	public UnixIPInterfaceProperties(UnixNetworkInterface iface, List<IPAddress> addresses)
	{
		this.iface = iface;
		this.addresses = addresses;
	}

	public override IPv6InterfaceProperties GetIPv6Properties()
	{
		throw new NotImplementedException();
	}

	[DllImport("__Internal")]
	private static extern int _monodroid_get_dns_servers(out IntPtr dns_servers_array);

	private void GetDNSServersFromOS()
	{
		IntPtr dns_servers_array;
		int num = _monodroid_get_dns_servers(out dns_servers_array);
		if (num <= 0)
		{
			return;
		}
		IntPtr[] array = new IntPtr[num];
		Marshal.Copy(dns_servers_array, array, 0, num);
		dns_servers = new IPAddressCollection();
		IntPtr[] array2 = array;
		foreach (IntPtr intPtr in array2)
		{
			string ipString = Marshal.PtrToStringAnsi(intPtr);
			Marshal.FreeHGlobal(intPtr);
			if (IPAddress.TryParse(ipString, out var address))
			{
				dns_servers.InternalAdd(address);
			}
		}
		Marshal.FreeHGlobal(dns_servers_array);
	}
}
