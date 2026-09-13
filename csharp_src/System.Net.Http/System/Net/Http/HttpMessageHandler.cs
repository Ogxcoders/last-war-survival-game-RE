using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class HttpMessageHandler : IDisposable
{
	protected HttpMessageHandler()
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Info(this);
		}
	}

	protected internal abstract Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
