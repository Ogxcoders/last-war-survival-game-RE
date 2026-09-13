using System.Collections.Generic;

namespace System.Net;

internal interface IWebProxyFinder : IDisposable
{
	bool IsValid { get; }

	bool GetProxies(Uri destination, out IList<string> proxyList);

	void Abort();

	void Reset();
}
