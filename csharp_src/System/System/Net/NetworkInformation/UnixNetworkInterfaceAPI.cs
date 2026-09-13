using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal abstract class UnixNetworkInterfaceAPI : NetworkInterfaceFactory
{
	[DllImport("libc")]
	public static extern int if_nametoindex(string ifname);

	[DllImport("libc")]
	protected static extern int getifaddrs(out IntPtr ifap);

	[DllImport("libc")]
	protected static extern void freeifaddrs(IntPtr ifap);
}
