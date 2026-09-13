namespace System.Net.NetworkInformation;

internal static class UnixNetworkInterfaceFactoryPal
{
	public static NetworkInterfaceFactory Create()
	{
		if (Environment.OSVersion.Platform == PlatformID.Unix)
		{
			if (Platform.IsMacOS || Platform.IsOpenBSD)
			{
				return new MacOsNetworkInterfaceAPI();
			}
			if (Platform.IsFreeBSD)
			{
				return new FreeBSDNetworkInterfaceAPI();
			}
			if (Platform.IsAix || Platform.IsIBMi)
			{
				return new AixNetworkInterfaceAPI();
			}
			return new LinuxNetworkInterfaceAPI();
		}
		return null;
	}
}
