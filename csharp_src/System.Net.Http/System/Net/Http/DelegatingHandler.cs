using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public abstract class DelegatingHandler : HttpMessageHandler
{
	private HttpMessageHandler _innerHandler;

	private volatile bool _operationStarted;

	private volatile bool _disposed;

	public HttpMessageHandler InnerHandler
	{
		get
		{
			return _innerHandler;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CheckDisposedOrStarted();
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Associate(this, value);
			}
			_innerHandler = value;
		}
	}

	protected DelegatingHandler()
	{
	}

	protected DelegatingHandler(HttpMessageHandler innerHandler)
	{
		InnerHandler = innerHandler;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request", "A request message must be provided. It cannot be null.");
		}
		SetOperationStarted();
		return _innerHandler.SendAsync(request, cancellationToken);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_disposed)
		{
			_disposed = true;
			if (_innerHandler != null)
			{
				_innerHandler.Dispose();
			}
		}
		base.Dispose(disposing);
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
	}

	private void CheckDisposedOrStarted()
	{
		CheckDisposed();
		if (_operationStarted)
		{
			throw new InvalidOperationException("This instance has already started one or more requests. Properties can only be modified before sending the first request.");
		}
	}

	private void SetOperationStarted()
	{
		CheckDisposed();
		if (_innerHandler == null)
		{
			throw new InvalidOperationException("The inner handler has not been assigned.");
		}
		if (!_operationStarted)
		{
			_operationStarted = true;
		}
	}
}
