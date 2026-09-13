using System.Collections.Generic;
using System.Net.NetworkInformation.AixStructs;
using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal class AixNetworkInterfaceAPI : UnixNetworkInterfaceAPI
{
	private const int SOCK_DGRAM = 2;

	[DllImport("libc", SetLastError = true)]
	public static extern int socket(AixAddressFamily family, int type, int protocol);

	[DllImport("libc")]
	public static extern int close(int fd);

	[DllImport("libc", SetLastError = true)]
	public static extern int ioctl(int fd, AixIoctlRequest request, IntPtr arg);

	[DllImport("libc", SetLastError = true)]
	public static extern int ioctl(int fd, AixIoctlRequest request, ref int arg);

	[DllImport("libc", SetLastError = true)]
	public static extern int ioctl(int fd, AixIoctlRequest request, ref ifconf arg);

	[DllImport("libc", SetLastError = true)]
	public static extern int ioctl(int fd, AixIoctlRequest request, ref ifreq_flags arg);

	[DllImport("libc", SetLastError = true)]
	public static extern int ioctl(int fd, AixIoctlRequest request, ref ifreq_mtu arg);

	[DllImport("libc", SetLastError = true)]
	public static extern int ioctl(int fd, AixIoctlRequest request, ref ifreq_addrin arg);

	private unsafe static void ByteArrayCopy(byte* dst, byte* src, int elements)
	{
		for (int i = 0; i < 16; i++)
		{
			dst[i] = src[i];
		}
	}

	public unsafe override NetworkInterface[] GetAllNetworkInterfaces()
	{
		Dictionary<string, AixNetworkInterface> dictionary = new Dictionary<string, AixNetworkInterface>();
		ifconf arg = default(ifconf);
		arg.ifc_len = 0;
		arg.ifc_buf = IntPtr.Zero;
		int num = -1;
		try
		{
			num = socket(AixAddressFamily.AF_INET, 2, 0);
			if (num == -1)
			{
				throw new SystemException("socket for SIOCGIFCONF failed");
			}
			if (ioctl(num, AixIoctlRequest.SIOCGSIZIFCONF, ref arg.ifc_len) < 0 || arg.ifc_len < 1)
			{
				throw new SystemException("ioctl for SIOCGSIZIFCONF failed");
			}
			arg.ifc_buf = Marshal.AllocHGlobal(arg.ifc_len);
			if (ioctl(num, AixIoctlRequest.SIOCGIFCONF, ref arg) < 0)
			{
				throw new SystemException("ioctl for SIOCGIFCONF failed");
			}
			IntPtr ifc_buf = arg.ifc_buf;
			long num2 = arg.ifc_buf.ToInt64() + arg.ifc_len;
			ifreq ifreq = (ifreq)Marshal.PtrToStructure(ifc_buf, typeof(ifreq));
			while (ifc_buf.ToInt64() < num2)
			{
				ifreq = (ifreq)Marshal.PtrToStructure(ifc_buf, typeof(ifreq));
				IPAddress iPAddress = IPAddress.None;
				string text = null;
				int index = -1;
				byte[] array = null;
				NetworkInterfaceType networkInterfaceType = NetworkInterfaceType.Unknown;
				text = Marshal.PtrToStringAnsi(new IntPtr(ifreq.ifr_name));
				if (Enum.IsDefined(typeof(AixAddressFamily), (int)ifreq.ifru_addr.sa_family))
				{
					switch ((AixAddressFamily)ifreq.ifru_addr.sa_family)
					{
					case AixAddressFamily.AF_INET:
						iPAddress = new IPAddress(((System.Net.NetworkInformation.AixStructs.sockaddr_in)Marshal.PtrToStructure(ifc_buf + 16, typeof(System.Net.NetworkInformation.AixStructs.sockaddr_in))).sin_addr);
						break;
					case AixAddressFamily.AF_INET6:
					{
						System.Net.NetworkInformation.AixStructs.sockaddr_in6 sockaddr_in7 = (System.Net.NetworkInformation.AixStructs.sockaddr_in6)Marshal.PtrToStructure(ifc_buf + 16, typeof(System.Net.NetworkInformation.AixStructs.sockaddr_in6));
						iPAddress = new IPAddress(sockaddr_in7.sin6_addr.u6_addr8, sockaddr_in7.sin6_scope_id);
						break;
					}
					case AixAddressFamily.AF_LINK:
					{
						sockaddr_dl sockaddr_dl = default(sockaddr_dl);
						sockaddr_dl.Read(ifc_buf + 16);
						array = new byte[sockaddr_dl.sdl_alen];
						Array.Copy(sockaddr_dl.sdl_data, sockaddr_dl.sdl_nlen, array, 0, Math.Min(array.Length, sockaddr_dl.sdl_data.Length - sockaddr_dl.sdl_nlen));
						index = sockaddr_dl.sdl_index;
						int sdl_type = sockaddr_dl.sdl_type;
						if (Enum.IsDefined(typeof(AixArpHardware), sdl_type))
						{
							switch ((AixArpHardware)sdl_type)
							{
							case AixArpHardware.ETHER:
								networkInterfaceType = NetworkInterfaceType.Ethernet;
								break;
							case AixArpHardware.ATM:
								networkInterfaceType = NetworkInterfaceType.Atm;
								break;
							case AixArpHardware.SLIP:
								networkInterfaceType = NetworkInterfaceType.Slip;
								break;
							case AixArpHardware.PPP:
								networkInterfaceType = NetworkInterfaceType.Ppp;
								break;
							case AixArpHardware.LOOPBACK:
								networkInterfaceType = NetworkInterfaceType.Loopback;
								array = null;
								break;
							case AixArpHardware.FDDI:
								networkInterfaceType = NetworkInterfaceType.Fddi;
								break;
							}
						}
						break;
					}
					}
				}
				uint num3 = 0u;
				int ifru_mtu = 0;
				ifreq_flags arg2 = default(ifreq_flags);
				ByteArrayCopy(arg2.ifr_name, ifreq.ifr_name, 16);
				if (ioctl(num, AixIoctlRequest.SIOCGIFFLAGS, ref arg2) < 0)
				{
					throw new SystemException("ioctl for SIOCGIFFLAGS failed");
				}
				num3 = arg2.ifru_flags;
				ifreq_mtu arg3 = default(ifreq_mtu);
				ByteArrayCopy(arg3.ifr_name, ifreq.ifr_name, 16);
				if (ioctl(num, AixIoctlRequest.SIOCGIFMTU, ref arg3) >= 0)
				{
					ifru_mtu = arg3.ifru_mtu;
				}
				AixNetworkInterface value = null;
				if (!dictionary.TryGetValue(text, out value))
				{
					value = new AixNetworkInterface(text, num3, ifru_mtu);
					dictionary.Add(text, value);
				}
				if (!iPAddress.Equals(IPAddress.None))
				{
					value.AddAddress(iPAddress);
				}
				if (array != null || networkInterfaceType == NetworkInterfaceType.Loopback)
				{
					value.SetLinkLayerInfo(index, array, networkInterfaceType);
				}
				ifc_buf += 16 + ifreq.ifru_addr.sa_len;
			}
		}
		finally
		{
			if (arg.ifc_buf != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(arg.ifc_buf);
			}
			if (num != -1)
			{
				close(num);
			}
		}
		NetworkInterface[] array2 = new NetworkInterface[dictionary.Count];
		int num4 = 0;
		foreach (AixNetworkInterface value2 in dictionary.Values)
		{
			array2[num4] = value2;
			num4++;
		}
		return array2;
	}

	public override int GetLoopbackInterfaceIndex()
	{
		return UnixNetworkInterfaceAPI.if_nametoindex("lo0");
	}

	public unsafe override IPAddress GetNetMask(IPAddress address)
	{
		ifconf arg = default(ifconf);
		arg.ifc_len = 0;
		arg.ifc_buf = IntPtr.Zero;
		int num = -1;
		try
		{
			num = socket(AixAddressFamily.AF_INET, 2, 0);
			if (num == -1)
			{
				throw new SystemException("socket for SIOCGIFCONF failed");
			}
			if (ioctl(num, AixIoctlRequest.SIOCGSIZIFCONF, ref arg.ifc_len) < 0 || arg.ifc_len < 1)
			{
				throw new SystemException("ioctl for SIOCGSIZIFCONF failed");
			}
			arg.ifc_buf = Marshal.AllocHGlobal(arg.ifc_len);
			if (ioctl(num, AixIoctlRequest.SIOCGIFCONF, ref arg) < 0)
			{
				throw new SystemException("ioctl for SIOCGIFCONF failed");
			}
			IntPtr ifc_buf = arg.ifc_buf;
			long num2 = arg.ifc_buf.ToInt64() + arg.ifc_len;
			ifreq ifreq = (ifreq)Marshal.PtrToStructure(ifc_buf, typeof(ifreq));
			while (ifc_buf.ToInt64() < num2)
			{
				ifreq = (ifreq)Marshal.PtrToStructure(ifc_buf, typeof(ifreq));
				if (Enum.IsDefined(typeof(AixAddressFamily), (int)ifreq.ifru_addr.sa_family) && ifreq.ifru_addr.sa_family == 2)
				{
					IPAddress obj = new IPAddress(((System.Net.NetworkInformation.AixStructs.sockaddr_in)Marshal.PtrToStructure(ifc_buf + 16, typeof(System.Net.NetworkInformation.AixStructs.sockaddr_in))).sin_addr);
					if (address.Equals(obj))
					{
						ifreq_addrin arg2 = default(ifreq_addrin);
						ByteArrayCopy(arg2.ifr_name, ifreq.ifr_name, 16);
						if (ioctl(num, AixIoctlRequest.SIOCGIFNETMASK, ref arg2) < 0)
						{
							return new IPAddress(arg2.ifru_addr.sin_addr);
						}
						throw new SystemException("ioctl for SIOCGIFNETMASK failed");
					}
				}
				ifc_buf += 16 + ifreq.ifru_addr.sa_len;
			}
		}
		finally
		{
			if (arg.ifc_buf != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(arg.ifc_buf);
			}
			if (num != -1)
			{
				close(num);
			}
		}
		return null;
	}
}
