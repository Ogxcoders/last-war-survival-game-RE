using System.IO;
using System.Net.Sockets;

namespace System.Net.Http;

internal sealed class HttpConnectionWithFinalizer : HttpConnection
{
	public HttpConnectionWithFinalizer(HttpConnectionPool pool, Socket socket, Stream stream, TransportContext transportContext)
		: base(pool, socket, stream, transportContext)
	{
	}

	~HttpConnectionWithFinalizer()
	{
		Dispose(disposing: false);
	}
}
