using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal class LinuxNetworkInterfaceAPI : UnixNetworkInterfaceAPI
{
	private const int AF_INET = 2;

	private const int AF_INET6 = 10;

	private const int AF_PACKET = 17;

	private static void FreeInterfaceAddresses(IntPtr ifap)
	{
		AndroidPlatform.FreeInterfaceAddresses(ifap);
	}

	private static int GetInterfaceAddresses(out IntPtr ifap)
	{
		return AndroidPlatform.GetInterfaceAddresses(out ifap);
	}

	public override NetworkInterface[] GetAllNetworkInterfaces()
	{
		Dictionary<string, LinuxNetworkInterface> dictionary = new Dictionary<string, LinuxNetworkInterface>();
		if (GetInterfaceAddresses(out var ifap) != 0)
		{
			throw new SystemException("getifaddrs() failed");
		}
		try
		{
			IntPtr intPtr = ifap;
			while (intPtr != IntPtr.Zero)
			{
				ifaddrs ifaddrs2 = (ifaddrs)Marshal.PtrToStructure(intPtr, typeof(ifaddrs));
				IPAddress iPAddress = IPAddress.None;
				string text = ifaddrs2.ifa_name;
				int index = -1;
				byte[] array = null;
				NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
				int num = 0;
				if (ifaddrs2.ifa_addr != IntPtr.Zero)
				{
					sockaddr_in sockaddr_in7 = (sockaddr_in)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(sockaddr_in));
					if (sockaddr_in7.sin_family == 10)
					{
						sockaddr_in6 sockaddr_in8 = (sockaddr_in6)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(sockaddr_in6));
						iPAddress = new IPAddress(sockaddr_in8.sin6_addr.u6_addr8, sockaddr_in8.sin6_scope_id);
					}
					else if (sockaddr_in7.sin_family == 2)
					{
						iPAddress = new IPAddress(sockaddr_in7.sin_addr);
					}
					else if (sockaddr_in7.sin_family == 17)
					{
						sockaddr_ll sockaddr_ll2 = (sockaddr_ll)Marshal.PtrToStructure(ifaddrs2.ifa_addr, typeof(sockaddr_ll));
						if (sockaddr_ll2.sll_halen > sockaddr_ll2.sll_addr.Length)
						{
							intPtr = ifaddrs2.ifa_next;
							continue;
						}
						array = new byte[sockaddr_ll2.sll_halen];
						Array.Copy(sockaddr_ll2.sll_addr, 0, array, 0, array.Length);
						index = sockaddr_ll2.sll_ifindex;
						int sll_hatype = sockaddr_ll2.sll_hatype;
						if (Enum.IsDefined(typeof(LinuxArpHardware), sll_hatype))
						{
							switch ((LinuxArpHardware)sll_hatype)
							{
							case LinuxArpHardware.ETHER:
							case LinuxArpHardware.EETHER:
								networkInterfaceType = NetworkInterfaceType.Ethernet;
								break;
							case LinuxArpHardware.PRONET:
								networkInterfaceType = NetworkInterfaceType.TokenRing;
								break;
							case LinuxArpHardware.ATM:
								networkInterfaceType = NetworkInterfaceType.Atm;
								break;
							case LinuxArpHardware.SLIP:
							case LinuxArpHardware.CSLIP:
							case LinuxArpHardware.SLIP6:
							case LinuxArpHardware.CSLIP6:
								networkInterfaceType = NetworkInterfaceType.Slip;
								break;
							case LinuxArpHardware.PPP:
								networkInterfaceType = NetworkInterfaceType.Ppp;
								break;
							case LinuxArpHardware.LOOPBACK:
								networkInterfaceType = NetworkInterfaceType.Loopback;
								array = null;
								break;
							case LinuxArpHardware.FDDI:
								networkInterfaceType = NetworkInterfaceType.Fddi;
								break;
							case LinuxArpHardware.TUNNEL:
							case LinuxArpHardware.TUNNEL6:
							case LinuxArpHardware.SIT:
							case LinuxArpHardware.IPDDP:
							case LinuxArpHardware.IPGRE:
							case LinuxArpHardware.IP6GRE:
								networkInterfaceType = NetworkInterfaceType.Tunnel;
								break;
							}
						}
					}
				}
				LinuxNetworkInterface value = null;
				if (string.IsNullOrEmpty(text))
				{
					int num2 = ++num;
					text = "\0" + num2;
				}
				if (!dictionary.TryGetValue(text, out value))
				{
					value = new LinuxNetworkInterface(text);
					dictionary.Add(text, value);
				}
				if (!iPAddress.Equals(IPAddress.None))
				{
					value.AddAddress(iPAddress);
				}
				if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
				{
					if (networkInterfaceType == NetworkInterfaceType.Ethernet && Directory.Exists(value.IfacePath + "wireless"))
					{
						networkInterfaceType = NetworkInterfaceType.Wireless80211;
					}
					value.SetLinkLayerInfo(index, array, networkInterfaceType);
				}
				intPtr = ifaddrs2.ifa_next;
			}
		}
		finally
		{
			FreeInterfaceAddresses(ifap);
		}
		NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
		int num3 = 0;
		foreach (LinuxNetworkInterface value2 in dictionary.Values)
		{
			array2[num3] = value2;
			num3++;
		}
		return array2;
	}

	public override int GetLoopbackInterfaceIndex()
	{
		return UnixNetworkInterfaceAPI.if_nametoindex("lo");
	}

	public override IPAddress GetNetMask(IPAddress address)
	{
		foreach (ifaddrs networkInterface in GetNetworkInterfaces())
		{
			if (!(networkInterface.ifa_addr == IntPtr.Zero))
			{
				sockaddr_in sockaddr_in7 = (sockaddr_in)Marshal.PtrToStructure(networkInterface.ifa_addr, typeof(sockaddr_in));
				if (sockaddr_in7.sin_family == 2 && address.Equals(new IPAddress(sockaddr_in7.sin_addr)))
				{
					return new IPAddress(((sockaddr_in)Marshal.PtrToStructure(networkInterface.ifa_netmask, typeof(sockaddr_in))).sin_addr);
				}
			}
		}
		return null;
	}

	private static IEnumerable<ifaddrs> GetNetworkInterfaces()
	{
		IntPtr ifap = IntPtr.Zero;
		try
		{
			if (GetInterfaceAddresses(out ifap) == 0)
			{
				IntPtr intPtr = ifap;
				while (intPtr != IntPtr.Zero)
				{
					ifaddrs addr = (ifaddrs)Marshal.PtrToStructure(intPtr, typeof(ifaddrs));
					yield return addr;
					intPtr = addr.ifa_next;
				}
			}
		}
		finally
		{
			if (ifap != IntPtr.Zero)
			{
				FreeInterfaceAddresses(ifap);
			}
		}
	}
}
