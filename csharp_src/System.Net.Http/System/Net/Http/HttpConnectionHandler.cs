using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal sealed class HttpConnectionHandler : HttpMessageHandler
{
	private readonly HttpConnectionPoolManager _poolManager;

	public HttpConnectionHandler(HttpConnectionPoolManager poolManager)
	{
		_poolManager = poolManager;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return _poolManager.SendAsync(request, doRequestAuth: false, cancellationToken);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_poolManager.Dispose();
		}
		base.Dispose(disposing);
	}
}
