namespace System.Net.Http;

internal static class SystemProxyInfo
{
	public static IWebProxy ConstructSystemProxy()
	{
		if (!HttpEnvironmentProxy.TryCreate(out var proxy))
		{
			return null;
		}
		return proxy;
	}
}
