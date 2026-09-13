namespace System.Net;

internal class ProxyScriptChain : ProxyChain
{
	private WebProxy m_Proxy;

	private Uri[] m_ScriptProxies;

	private int m_CurrentIndex;

	private int m_SyncStatus;

	internal ProxyScriptChain(WebProxy proxy, Uri destination)
		: base(destination)
	{
		m_Proxy = proxy;
	}

	protected override bool GetNextProxy(out Uri proxy)
	{
		if (m_CurrentIndex < 0)
		{
			proxy = null;
			return false;
		}
		if (m_CurrentIndex == 0)
		{
			m_ScriptProxies = m_Proxy.GetProxiesAuto(base.Destination, ref m_SyncStatus);
		}
		if (m_ScriptProxies == null || m_CurrentIndex >= m_ScriptProxies.Length)
		{
			proxy = m_Proxy.GetProxyAutoFailover(base.Destination);
			m_CurrentIndex = -1;
			return true;
		}
		proxy = m_ScriptProxies[m_CurrentIndex++];
		return true;
	}

	internal override void Abort()
	{
		m_Proxy.AbortGetProxiesAuto(ref m_SyncStatus);
	}
}
