namespace System.Net.NetworkInformation;

internal static class NetworkInterfaceFactoryPal
{
	public static NetworkInterfaceFactory Create()
	{
		return UnixNetworkInterfaceFactoryPal.Create() ?? throw new NotImplementedException();
	}
}
