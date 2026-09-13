namespace System.Net.NetworkInformation;

internal abstract class NetworkInterfaceFactory
{
	public abstract NetworkInterface[] GetAllNetworkInterfaces();

	public abstract int GetLoopbackInterfaceIndex();

	public abstract IPAddress GetNetMask(IPAddress address);

	public static NetworkInterfaceFactory Create()
	{
		return NetworkInterfaceFactoryPal.Create();
	}
}
