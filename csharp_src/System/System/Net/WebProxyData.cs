using System.Collections;

namespace System.Net;

internal class WebProxyData
{
	internal bool bypassOnLocal;

	internal bool automaticallyDetectSettings;

	internal Uri proxyAddress;

	internal Hashtable proxyHostAddresses;

	internal Uri scriptLocation;

	internal ArrayList bypassList;
}
