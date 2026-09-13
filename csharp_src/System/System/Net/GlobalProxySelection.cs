namespace System.Net;

[Obsolete("This class has been deprecated. Please use WebRequest.DefaultWebProxy instead to access and set the global default proxy. Use 'null' instead of GetEmptyWebProxy. https://go.microsoft.com/fwlink/?linkid=14202")]
public class GlobalProxySelection
{
	public static IWebProxy Select
	{
		get
		{
			IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
			if (defaultWebProxy == null)
			{
				return GetEmptyWebProxy();
			}
			if (defaultWebProxy is WebRequest.WebProxyWrapper webProxyWrapper)
			{
				return webProxyWrapper.WebProxy;
			}
			return defaultWebProxy;
		}
		set
		{
			WebRequest.DefaultWebProxy = value;
		}
	}

	public static IWebProxy GetEmptyWebProxy()
	{
		return new EmptyWebProxy();
	}
}
