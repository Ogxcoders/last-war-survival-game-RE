namespace System.Net;

internal class StaticProxy : ProxyChain
{
	private Uri m_Proxy;

	internal StaticProxy(Uri destination, Uri proxy)
		: base(destination)
	{
		if (proxy == null)
		{
			throw new ArgumentNullException("proxy");
		}
		m_Proxy = proxy;
	}

	protected override bool GetNextProxy(out Uri proxy)
	{
		proxy = m_Proxy;
		if (proxy == null)
		{
			return false;
		}
		m_Proxy = null;
		return true;
	}
}
