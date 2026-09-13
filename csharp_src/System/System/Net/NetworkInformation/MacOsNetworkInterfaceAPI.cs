using System.Collections.Generic;
using System.Net.NetworkInformation.MacOsStructs;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal class MacOsNetworkInterfaceAPI : UnixNetworkInterfaceAPI
{
	private const int AF_INET = 2;

	private const int AF_LINK = 18;

	protected readonly int AF_INET6;

	public MacOsNetworkInterfaceAPI()
	{
		AF_INET6 = 30;
	}

	protected MacOsNetworkInterfaceAPI(int AF_INET6)
	{
		this.AF_INET6 = AF_INET6;
	}

	public override NetworkInterface[] GetAllNetworkInterfaces()
	{
		Dictionary<string, MacOsNetworkInterface> dictionary = new Dictionary<string, MacOsNetworkInterface>();
		if (UnixNetworkInterfaceAPI.getifaddrs(out var ifap) != 0)
		{
			throw new SystemException("getifaddrs() failed");
		}
		try
		{
			IntPtr intPtr = ifap;
			while (intPtr != IntPtr.Zero)
			{
				System.Net.NetworkInformation.MacOsStructs.ifaddrs ifaddrs2 = (System.Net.NetworkInformation.MacOsStructs.ifaddrs)Marshal.PtrToStructure(intPtr, typeof(System.Net.NetworkInformation.MacOsStructs.ifaddrs));
				IPAddress iPAddress = IPAddress.None;
				string ifa_name = ifaddrs2.ifa_name;
				int index = -1;
				byte[] array = null;
				NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
				if (ifaddrs2.ifa_addr != IntPtr.Zero)
				{
					sockaddr sockaddr = (sockaddr)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(sockaddr));
					if (sockaddr.sa_family == AF_INET6)
					{
						System.Net.NetworkInformation.MacOsStructs.sockaddr_in6 sockaddr_in7 = (System.Net.NetworkInformation.MacOsStructs.sockaddr_in6)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr_in6));
						iPAddress = new IPAddress(sockaddr_in7.sin6_addr.u6_addr8, sockaddr_in7.sin6_scope_id);
					}
					else if (sockaddr.sa_family == 2)
					{
						iPAddress = new IPAddress(((System.Net.NetworkInformation.MacOsStructs.sockaddr_in)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr_in))).sin_addr);
					}
					else if (sockaddr.sa_family == 18)
					{
						sockaddr_dl sockaddr_dl = default(sockaddr_dl);
						sockaddr_dl.Read(ifaddrs2.ifa_addr);
						array = new byte[sockaddr_dl.sdl_alen];
						Array.Copy(sockaddr_dl.sdl_data, sockaddr_dl.sdl_nlen, array, 0, Math.Min(array.Length, sockaddr_dl.sdl_data.Length - sockaddr_dl.sdl_nlen));
						index = sockaddr_dl.sdl_index;
						int sdl_type = sockaddr_dl.sdl_type;
						if (Enum.IsDefined(typeof(MacOsArpHardware), sdl_type))
						{
							switch ((MacOsArpHardware)sdl_type)
							{
							case MacOsArpHardware.ETHER:
								networkInterfaceType = NetworkInterfaceType.Ethernet;
								break;
							case MacOsArpHardware.ATM:
								networkInterfaceType = NetworkInterfaceType.Atm;
								break;
							case MacOsArpHardware.SLIP:
								networkInterfaceType = NetworkInterfaceType.Slip;
								break;
							case MacOsArpHardware.PPP:
								networkInterfaceType = NetworkInterfaceType.Ppp;
								break;
							case MacOsArpHardware.LOOPBACK:
								networkInterfaceType = NetworkInterfaceType.Loopback;
								array = null;
								break;
							case MacOsArpHardware.FDDI:
								networkInterfaceType = NetworkInterfaceType.Fddi;
								break;
							}
						}
					}
				}
				MacOsNetworkInterface value = null;
				if (!dictionary.TryGetValue(ifa_name, out value))
				{
					value = new MacOsNetworkInterface(ifa_name, ifaddrs2.ifa_flags);
					dictionary.Add(ifa_name, value);
				}
				if (!iPAddress.Equals(IPAddress.None))
				{
					value.AddAddress(iPAddress);
				}
				if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
				{
					value.SetLinkLayerInfo(index, array, networkInterfaceType);
				}
				intPtr = ifaddrs2.ifa_next;
			}
		}
		finally
		{
			UnixNetworkInterfaceAPI.freeifaddrs(ifap);
		}
		NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
		int num = 0;
		foreach (MacOsNetworkInterface value2 in dictionary.Values)
		{
			array2[num] = value2;
			num++;
		}
		return array2;
	}

	public override int GetLoopbackInterfaceIndex()
	{
		return UnixNetworkInterfaceAPI.if_nametoindex("lo0");
	}

	public override IPAddress GetNetMask(IPAddress address)
	{
		if (UnixNetworkInterfaceAPI.getifaddrs(out var ifap) != 0)
		{
			throw new SystemException("getifaddrs() failed");
		}
		try
		{
			IntPtr intPtr = ifap;
			while (intPtr != IntPtr.Zero)
			{
				System.Net.NetworkInformation.MacOsStructs.ifaddrs ifaddrs2 = (System.Net.NetworkInformation.MacOsStructs.ifaddrs)Marshal.PtrToStructure(intPtr, typeof(System.Net.NetworkInformation.MacOsStructs.ifaddrs));
				if (ifaddrs2.ifa_addr != IntPtr.Zero && ((sockaddr)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(sockaddr))).sa_family == 2)
				{
					IPAddress obj = new IPAddress(((System.Net.NetworkInformation.MacOsStructs.sockaddr_in)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(System.Net.NetworkInformation.MacOsStructs.sockaddr_in))).sin_addr);
					if (address.Equals(obj))
					{
						return new IPAddress(((sockaddr_in)Marshal.PtrToStructure(ifaddrs2.ifa_netmask, typeof(sockaddr_in))).sin_addr);
					}
				}
				intPtr = ifaddrs2.ifa_next;
			}
		}
		finally
		{
			UnixNetworkInterfaceAPI.freeifaddrs(ifap);
		}
		return null;
	}
}
