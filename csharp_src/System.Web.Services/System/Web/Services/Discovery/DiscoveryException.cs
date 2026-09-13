namespace System.Web.Services.Discovery;

internal class DiscoveryException : Exception
{
	public string Url;

	public Exception Exception;

	public DiscoveryException(string url, Exception origin)
	{
		Url = url;
		Exception = origin;
	}
}
