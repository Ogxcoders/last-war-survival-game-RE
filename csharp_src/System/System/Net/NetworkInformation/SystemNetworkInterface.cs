namespace System.Net.NetworkInformation;

internal static class SystemNetworkInterface
{
	private static readonly NetworkInterfaceFactory nif = NetworkInterfaceFactory.Create();

	public static int InternalLoopbackInterfaceIndex => nif.GetLoopbackInterfaceIndex();

	public static int InternalIPv6LoopbackInterfaceIndex
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public static NetworkInterface[] GetNetworkInterfaces()
	{
		try
		{
			return nif.GetAllNetworkInterfaces();
		}
		catch
		{
			return new NetworkInterface[0];
		}
	}

	public static bool InternalGetIsNetworkAvailable()
	{
		return true;
	}

	public static IPAddress GetNetMask(IPAddress address)
	{
		return nif.GetNetMask(address);
	}
}
