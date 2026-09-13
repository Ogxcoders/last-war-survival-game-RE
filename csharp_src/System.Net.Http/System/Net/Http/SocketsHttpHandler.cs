using System.Collections.Generic;
using System.Globalization;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public sealed class SocketsHttpHandler : HttpMessageHandler, IMonoHttpClientHandler, IDisposable
{
	private readonly HttpConnectionSettings _settings = new HttpConnectionSettings();

	private HttpMessageHandler _handler;

	private bool _disposed;

	public bool UseCookies
	{
		get
		{
			return _settings._useCookies;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._useCookies = value;
		}
	}

	public CookieContainer CookieContainer
	{
		get
		{
			return _settings._cookieContainer ?? (_settings._cookieContainer = new CookieContainer());
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._cookieContainer = value;
		}
	}

	public DecompressionMethods AutomaticDecompression
	{
		get
		{
			return _settings._automaticDecompression;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._automaticDecompression = value;
		}
	}

	public bool UseProxy
	{
		get
		{
			return _settings._useProxy;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._useProxy = value;
		}
	}

	public IWebProxy Proxy
	{
		get
		{
			return _settings._proxy;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._proxy = value;
		}
	}

	public ICredentials DefaultProxyCredentials
	{
		get
		{
			return _settings._defaultProxyCredentials;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._defaultProxyCredentials = value;
		}
	}

	public bool PreAuthenticate
	{
		get
		{
			return _settings._preAuthenticate;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._preAuthenticate = value;
		}
	}

	public ICredentials Credentials
	{
		get
		{
			return _settings._credentials;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._credentials = value;
		}
	}

	public bool AllowAutoRedirect
	{
		get
		{
			return _settings._allowAutoRedirect;
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._allowAutoRedirect = value;
		}
	}

	public int MaxAutomaticRedirections
	{
		get
		{
			return _settings._maxAutomaticRedirections;
		}
		set
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("value", value, global::SR.Format("The specified value must be greater than {0}.", 0));
			}
			CheckDisposedOrStarted();
			_settings._maxAutomaticRedirections = value;
		}
	}

	public int MaxConnectionsPerServer
	{
		get
		{
			return _settings._maxConnectionsPerServer;
		}
		set
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("value", value, global::SR.Format("The specified value must be greater than {0}.", 0));
			}
			CheckDisposedOrStarted();
			_settings._maxConnectionsPerServer = value;
		}
	}

	public int MaxResponseDrainSize
	{
		get
		{
			return _settings._maxResponseDrainSize;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", value, "Non-negative number required.");
			}
			CheckDisposedOrStarted();
			_settings._maxResponseDrainSize = value;
		}
	}

	public TimeSpan ResponseDrainTimeout
	{
		get
		{
			return _settings._maxResponseDrainTime;
		}
		set
		{
			if ((value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan) || value.TotalMilliseconds > 2147483647.0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			CheckDisposedOrStarted();
			_settings._maxResponseDrainTime = value;
		}
	}

	public int MaxResponseHeadersLength
	{
		get
		{
			return _settings._maxResponseHeadersLength;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value", value, global::SR.Format("The specified value must be greater than {0}.", 0));
			}
			CheckDisposedOrStarted();
			_settings._maxResponseHeadersLength = value;
		}
	}

	public SslClientAuthenticationOptions SslOptions
	{
		get
		{
			return _settings._sslOptions ?? (_settings._sslOptions = new SslClientAuthenticationOptions());
		}
		set
		{
			CheckDisposedOrStarted();
			_settings._sslOptions = value;
		}
	}

	public TimeSpan PooledConnectionLifetime
	{
		get
		{
			return _settings._pooledConnectionLifetime;
		}
		set
		{
			if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			CheckDisposedOrStarted();
			_settings._pooledConnectionLifetime = value;
		}
	}

	public TimeSpan PooledConnectionIdleTimeout
	{
		get
		{
			return _settings._pooledConnectionIdleTimeout;
		}
		set
		{
			if (value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			CheckDisposedOrStarted();
			_settings._pooledConnectionIdleTimeout = value;
		}
	}

	public TimeSpan ConnectTimeout
	{
		get
		{
			return _settings._connectTimeout;
		}
		set
		{
			if ((value <= TimeSpan.Zero && value != Timeout.InfiniteTimeSpan) || value.TotalMilliseconds > 2147483647.0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			CheckDisposedOrStarted();
			_settings._connectTimeout = value;
		}
	}

	public TimeSpan Expect100ContinueTimeout
	{
		get
		{
			return _settings._expect100ContinueTimeout;
		}
		set
		{
			if ((value < TimeSpan.Zero && value != Timeout.InfiniteTimeSpan) || value.TotalMilliseconds > 2147483647.0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			CheckDisposedOrStarted();
			_settings._expect100ContinueTimeout = value;
		}
	}

	public IDictionary<string, object> Properties => _settings._properties ?? (_settings._properties = new Dictionary<string, object>());

	bool IMonoHttpClientHandler.SupportsAutomaticDecompression => true;

	long IMonoHttpClientHandler.MaxRequestContentBufferSize
	{
		get
		{
			return 0L;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("value", value, string.Format(CultureInfo.InvariantCulture, "Buffering more than {0} bytes is not supported.", int.MaxValue));
			}
			CheckDisposedOrStarted();
		}
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("SocketsHttpHandler");
		}
	}

	private void CheckDisposedOrStarted()
	{
		CheckDisposed();
		if (_handler != null)
		{
			throw new InvalidOperationException("This instance has already started one or more requests. Properties can only be modified before sending the first request.");
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_disposed)
		{
			_disposed = true;
			_handler?.Dispose();
		}
		base.Dispose(disposing);
	}

	private HttpMessageHandler SetupHandlerChain()
	{
		HttpConnectionSettings httpConnectionSettings = _settings.Clone();
		HttpConnectionPoolManager poolManager = new HttpConnectionPoolManager(httpConnectionSettings);
		HttpMessageHandler httpMessageHandler = ((httpConnectionSettings._credentials != null) ? ((HttpMessageHandler)new HttpAuthenticatedConnectionHandler(poolManager)) : ((HttpMessageHandler)new HttpConnectionHandler(poolManager)));
		if (httpConnectionSettings._allowAutoRedirect)
		{
			HttpMessageHandler redirectInnerHandler = ((httpConnectionSettings._credentials == null || httpConnectionSettings._credentials is CredentialCache) ? httpMessageHandler : new HttpConnectionHandler(poolManager));
			httpMessageHandler = new RedirectHandler(httpConnectionSettings._maxAutomaticRedirections, httpMessageHandler, redirectInnerHandler);
		}
		if (httpConnectionSettings._automaticDecompression != DecompressionMethods.None)
		{
			httpMessageHandler = new DecompressionHandler(httpConnectionSettings._automaticDecompression, httpMessageHandler);
		}
		if (Interlocked.CompareExchange(ref _handler, httpMessageHandler, null) != null)
		{
			httpMessageHandler.Dispose();
		}
		return _handler;
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		CheckDisposed();
		HttpMessageHandler httpMessageHandler = _handler ?? SetupHandlerChain();
		Exception ex = ValidateAndNormalizeRequest(request);
		if (ex != null)
		{
			return Task.FromException<HttpResponseMessage>(ex);
		}
		return httpMessageHandler.SendAsync(request, cancellationToken);
	}

	private Exception ValidateAndNormalizeRequest(HttpRequestMessage request)
	{
		if (request.Version.Major == 0)
		{
			return new NotSupportedException("Request HttpVersion 0.X is not supported.  Use 1.0 or above.");
		}
		if (request.HasHeaders && request.Headers.TransferEncodingChunked == true)
		{
			if (request.Content == null)
			{
				return new HttpRequestException("An error occurred while sending the request.", new InvalidOperationException("'Transfer-Encoding: chunked' header can not be used when content object is not specified."));
			}
			request.Content.Headers.ContentLength = null;
		}
		else if (request.Content != null && !request.Content.Headers.ContentLength.HasValue)
		{
			request.Headers.TransferEncodingChunked = true;
		}
		if (request.Version.Minor == 0 && request.Version.Major == 1 && request.HasHeaders)
		{
			if (request.Headers.TransferEncodingChunked == true)
			{
				return new NotSupportedException("HTTP 1.0 does not support chunking.");
			}
			if (request.Headers.ExpectContinue == true)
			{
				request.Headers.ExpectContinue = false;
			}
		}
		return null;
	}

	void IMonoHttpClientHandler.SetWebRequestTimeout(TimeSpan timeout)
	{
	}

	Task<HttpResponseMessage> IMonoHttpClientHandler.SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return SendAsync(request, cancellationToken);
	}
}
