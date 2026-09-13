namespace System.Net;

[Serializable]
internal sealed class EmptyWebProxy : IAutoWebProxy, IWebProxy
{
	[NonSerialized]
	private ICredentials m_credentials;

	public ICredentials Credentials
	{
		get
		{
			return m_credentials;
		}
		set
		{
			m_credentials = value;
		}
	}

	public Uri GetProxy(Uri uri)
	{
		return uri;
	}

	public bool IsBypassed(Uri uri)
	{
		return true;
	}

	ProxyChain IAutoWebProxy.GetProxies(Uri destination)
	{
		return new DirectProxy(destination);
	}
}
