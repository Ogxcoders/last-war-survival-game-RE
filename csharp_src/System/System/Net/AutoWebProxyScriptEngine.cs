using System.Collections.Generic;

namespace System.Net;

internal class AutoWebProxyScriptEngine
{
	public Uri AutomaticConfigurationScript { get; set; }

	public bool AutomaticallyDetectSettings { get; set; }

	public AutoWebProxyScriptEngine(WebProxy proxy, bool useRegistry)
	{
	}

	public bool GetProxies(Uri destination, out IList<string> proxyList)
	{
		int syncStatus = 0;
		return GetProxies(destination, out proxyList, ref syncStatus);
	}

	public bool GetProxies(Uri destination, out IList<string> proxyList, ref int syncStatus)
	{
		proxyList = null;
		return false;
	}

	public void Close()
	{
	}

	public void Abort(ref int syncStatus)
	{
	}

	public void CheckForChanges()
	{
	}
}
