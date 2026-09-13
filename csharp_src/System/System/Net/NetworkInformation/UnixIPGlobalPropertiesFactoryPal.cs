namespace System.Net.NetworkInformation;

internal static class UnixIPGlobalPropertiesFactoryPal
{
	public static IPGlobalProperties Create()
	{
		return new AndroidIPGlobalProperties();
	}
}
