using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal sealed class HttpAuthenticatedConnectionHandler : HttpMessageHandler
{
	private readonly HttpConnectionPoolManager _poolManager;

	public HttpAuthenticatedConnectionHandler(HttpConnectionPoolManager poolManager)
	{
		_poolManager = poolManager;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return _poolManager.SendAsync(request, doRequestAuth: true, cancellationToken);
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
