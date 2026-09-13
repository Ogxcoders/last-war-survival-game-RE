namespace System.Net;

internal class DirectProxy : ProxyChain
{
	private bool m_ProxyRetrieved;

	internal DirectProxy(Uri destination)
		: base(destination)
	{
	}

	protected override bool GetNextProxy(out Uri proxy)
	{
		proxy = null;
		if (m_ProxyRetrieved)
		{
			return false;
		}
		m_ProxyRetrieved = true;
		return true;
	}
}
