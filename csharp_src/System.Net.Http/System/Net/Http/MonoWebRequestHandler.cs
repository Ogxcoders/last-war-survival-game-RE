using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal class MonoWebRequestHandler : IMonoHttpClientHandler, IDisposable
{
	private static long groupCounter;

	private bool allowAutoRedirect;

	private DecompressionMethods automaticDecompression;

	private CookieContainer cookieContainer;

	private ICredentials credentials;

	private int maxAutomaticRedirections;

	private long maxRequestContentBufferSize;

	private bool preAuthenticate;

	private IWebProxy proxy;

	private bool useCookies;

	private bool useProxy;

	private SslClientAuthenticationOptions sslOptions;

	private bool allowPipelining;

	private RequestCachePolicy cachePolicy;

	private AuthenticationLevel authenticationLevel;

	private TimeSpan continueTimeout;

	private TokenImpersonationLevel impersonationLevel;

	private int maxResponseHeadersLength;

	private int readWriteTimeout;

	private RemoteCertificateValidationCallback serverCertificateValidationCallback;

	private bool unsafeAuthenticatedConnectionSharing;

	private bool sentRequest;

	private string connectionGroupName;

	private TimeSpan? timeout;

	private bool disposed;

	public bool AllowAutoRedirect
	{
		get
		{
			return allowAutoRedirect;
		}
		set
		{
			EnsureModifiability();
			allowAutoRedirect = value;
		}
	}

	public DecompressionMethods AutomaticDecompression
	{
		get
		{
			return automaticDecompression;
		}
		set
		{
			EnsureModifiability();
			automaticDecompression = value;
		}
	}

	public CookieContainer CookieContainer
	{
		get
		{
			return cookieContainer ?? (cookieContainer = new CookieContainer());
		}
		set
		{
			EnsureModifiability();
			cookieContainer = value;
		}
	}

	public ICredentials Credentials
	{
		get
		{
			return credentials;
		}
		set
		{
			EnsureModifiability();
			credentials = value;
		}
	}

	public int MaxAutomaticRedirections
	{
		get
		{
			return maxAutomaticRedirections;
		}
		set
		{
			EnsureModifiability();
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			maxAutomaticRedirections = value;
		}
	}

	public long MaxRequestContentBufferSize
	{
		get
		{
			return maxRequestContentBufferSize;
		}
		set
		{
			EnsureModifiability();
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			maxRequestContentBufferSize = value;
		}
	}

	public bool PreAuthenticate
	{
		get
		{
			return preAuthenticate;
		}
		set
		{
			EnsureModifiability();
			preAuthenticate = value;
		}
	}

	public IWebProxy Proxy
	{
		get
		{
			return proxy;
		}
		set
		{
			EnsureModifiability();
			if (!UseProxy)
			{
				throw new InvalidOperationException();
			}
			proxy = value;
		}
	}

	public virtual bool SupportsAutomaticDecompression => true;

	public virtual bool SupportsProxy => true;

	public virtual bool SupportsRedirectConfiguration => true;

	public bool UseCookies
	{
		get
		{
			return useCookies;
		}
		set
		{
			EnsureModifiability();
			useCookies = value;
		}
	}

	public bool UseProxy
	{
		get
		{
			return useProxy;
		}
		set
		{
			EnsureModifiability();
			useProxy = value;
		}
	}

	public bool AllowPipelining
	{
		get
		{
			return allowPipelining;
		}
		set
		{
			EnsureModifiability();
			allowPipelining = value;
		}
	}

	public RequestCachePolicy CachePolicy
	{
		get
		{
			return cachePolicy;
		}
		set
		{
			EnsureModifiability();
			cachePolicy = value;
		}
	}

	public AuthenticationLevel AuthenticationLevel
	{
		get
		{
			return authenticationLevel;
		}
		set
		{
			EnsureModifiability();
			authenticationLevel = value;
		}
	}

	[System.MonoTODO]
	public TimeSpan ContinueTimeout
	{
		get
		{
			return continueTimeout;
		}
		set
		{
			EnsureModifiability();
			continueTimeout = value;
		}
	}

	public TokenImpersonationLevel ImpersonationLevel
	{
		get
		{
			return impersonationLevel;
		}
		set
		{
			EnsureModifiability();
			impersonationLevel = value;
		}
	}

	public int MaxResponseHeadersLength
	{
		get
		{
			return maxResponseHeadersLength;
		}
		set
		{
			EnsureModifiability();
			maxResponseHeadersLength = value;
		}
	}

	public int ReadWriteTimeout
	{
		get
		{
			return readWriteTimeout;
		}
		set
		{
			EnsureModifiability();
			readWriteTimeout = value;
		}
	}

	public RemoteCertificateValidationCallback ServerCertificateValidationCallback
	{
		get
		{
			return serverCertificateValidationCallback;
		}
		set
		{
			EnsureModifiability();
			serverCertificateValidationCallback = value;
		}
	}

	public bool UnsafeAuthenticatedConnectionSharing
	{
		get
		{
			return unsafeAuthenticatedConnectionSharing;
		}
		set
		{
			EnsureModifiability();
			unsafeAuthenticatedConnectionSharing = value;
		}
	}

	public SslClientAuthenticationOptions SslOptions
	{
		get
		{
			return sslOptions ?? (sslOptions = new SslClientAuthenticationOptions());
		}
		set
		{
			EnsureModifiability();
			sslOptions = value;
		}
	}

	public ICredentials DefaultProxyCredentials
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public int MaxConnectionsPerServer
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public IDictionary<string, object> Properties
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public MonoWebRequestHandler()
	{
		allowAutoRedirect = true;
		maxAutomaticRedirections = 50;
		maxRequestContentBufferSize = 2147483647L;
		useCookies = true;
		useProxy = true;
		allowPipelining = true;
		authenticationLevel = AuthenticationLevel.MutualAuthRequested;
		cachePolicy = WebRequest.DefaultCachePolicy;
		continueTimeout = TimeSpan.FromMilliseconds(350.0);
		impersonationLevel = TokenImpersonationLevel.Delegation;
		maxResponseHeadersLength = HttpWebRequest.DefaultMaximumResponseHeadersLength;
		readWriteTimeout = 300000;
		serverCertificateValidationCallback = null;
		unsafeAuthenticatedConnectionSharing = false;
		connectionGroupName = "HttpClientHandler" + Interlocked.Increment(ref groupCounter);
	}

	internal void EnsureModifiability()
	{
		if (sentRequest)
		{
			throw new InvalidOperationException("This instance has already started one or more requests. Properties can only be modified before sending the first request.");
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && !disposed)
		{
			Volatile.Write(ref disposed, value: true);
			ServicePointManager.CloseConnectionGroup(connectionGroupName);
		}
	}

	private bool GetConnectionKeepAlive(HttpRequestHeaders headers)
	{
		return headers.Connection.Any((string l) => string.Equals(l, "Keep-Alive", StringComparison.OrdinalIgnoreCase));
	}

	internal virtual HttpWebRequest CreateWebRequest(HttpRequestMessage request)
	{
		HttpWebRequest httpWebRequest = new HttpWebRequest(request.RequestUri);
		httpWebRequest.ThrowOnError = false;
		httpWebRequest.AllowWriteStreamBuffering = false;
		if (request.Version == HttpVersion.Version20)
		{
			httpWebRequest.ProtocolVersion = HttpVersion.Version11;
		}
		else
		{
			httpWebRequest.ProtocolVersion = request.Version;
		}
		httpWebRequest.ConnectionGroupName = connectionGroupName;
		httpWebRequest.Method = request.Method.Method;
		if (httpWebRequest.ProtocolVersion == HttpVersion.Version10)
		{
			httpWebRequest.KeepAlive = GetConnectionKeepAlive(request.Headers);
		}
		else
		{
			httpWebRequest.KeepAlive = request.Headers.ConnectionClose != true;
		}
		if (allowAutoRedirect)
		{
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.MaximumAutomaticRedirections = maxAutomaticRedirections;
		}
		else
		{
			httpWebRequest.AllowAutoRedirect = false;
		}
		httpWebRequest.AutomaticDecompression = automaticDecompression;
		httpWebRequest.PreAuthenticate = preAuthenticate;
		if (useCookies)
		{
			httpWebRequest.CookieContainer = CookieContainer;
		}
		httpWebRequest.Credentials = credentials;
		if (useProxy)
		{
			httpWebRequest.Proxy = proxy;
		}
		else
		{
			httpWebRequest.Proxy = null;
		}
		httpWebRequest.ServicePoint.Expect100Continue = request.Headers.ExpectContinue == true;
		if (timeout.HasValue)
		{
			httpWebRequest.Timeout = (int)timeout.Value.TotalMilliseconds;
		}
		WebHeaderCollection headers = httpWebRequest.Headers;
		foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers)
		{
			IEnumerable<string> enumerable = header.Value;
			if (header.Key == "Host")
			{
				httpWebRequest.Host = request.Headers.Host;
				continue;
			}
			if (header.Key == "Transfer-Encoding")
			{
				enumerable = enumerable.Where((string l) => l != "chunked");
			}
			string singleHeaderString = PlatformHelper.GetSingleHeaderString(header.Key, enumerable);
			if (singleHeaderString != null)
			{
				headers.AddInternal(header.Key, singleHeaderString);
			}
		}
		return httpWebRequest;
	}

	private HttpResponseMessage CreateResponseMessage(HttpWebResponse wr, HttpRequestMessage requestMessage, CancellationToken cancellationToken)
	{
		HttpResponseMessage httpResponseMessage = new HttpResponseMessage(wr.StatusCode);
		httpResponseMessage.RequestMessage = requestMessage;
		httpResponseMessage.ReasonPhrase = wr.StatusDescription;
		httpResponseMessage.Content = PlatformHelper.CreateStreamContent(wr.GetResponseStream(), cancellationToken);
		WebHeaderCollection headers = wr.Headers;
		for (int i = 0; i < headers.Count; i++)
		{
			string key = headers.GetKey(i);
			string[] values = headers.GetValues(i);
			HttpHeaders httpHeaders = ((!PlatformHelper.IsContentHeader(key)) ? ((HttpHeaders)httpResponseMessage.Headers) : ((HttpHeaders)httpResponseMessage.Content.Headers));
			httpHeaders.TryAddWithoutValidation(key, values);
		}
		requestMessage.RequestUri = wr.ResponseUri;
		return httpResponseMessage;
	}

	private static bool MethodHasBody(HttpMethod method)
	{
		switch (method.Method)
		{
		case "HEAD":
		case "GET":
		case "MKCOL":
		case "CONNECT":
		case "TRACE":
			return false;
		default:
			return true;
		}
	}

	public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
		Volatile.Write(ref sentRequest, value: true);
		HttpWebRequest wrequest = CreateWebRequest(request);
		HttpWebResponse wresponse = null;
		try
		{
			using (cancellationToken.Register(delegate(object l)
			{
				((HttpWebRequest)l).Abort();
			}, wrequest))
			{
				HttpContent content = request.Content;
				if (content != null)
				{
					WebHeaderCollection headers = wrequest.Headers;
					foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
					{
						foreach (string item in header.Value)
						{
							headers.AddInternal(header.Key, item);
						}
					}
					if (request.Headers.TransferEncodingChunked == true)
					{
						wrequest.SendChunked = true;
					}
					else
					{
						long? contentLength = content.Headers.ContentLength;
						if (contentLength.HasValue)
						{
							wrequest.ContentLength = contentLength.Value;
						}
						else
						{
							if (MaxRequestContentBufferSize == 0L)
							{
								throw new InvalidOperationException("The content length of the request content can't be determined. Either set TransferEncodingChunked to true, load content into buffer, or set MaxRequestContentBufferSize.");
							}
							await content.LoadIntoBufferAsync(MaxRequestContentBufferSize).ConfigureAwait(continueOnCapturedContext: false);
							wrequest.ContentLength = content.Headers.ContentLength.Value;
						}
					}
					wrequest.ResendContentFactory = content.CopyToAsync;
					using Stream stream = await wrequest.GetRequestStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
					await request.Content.CopyToAsync(stream).ConfigureAwait(continueOnCapturedContext: false);
				}
				else if (MethodHasBody(request.Method))
				{
					wrequest.ContentLength = 0L;
				}
				wresponse = (HttpWebResponse)(await wrequest.GetResponseAsync().ConfigureAwait(continueOnCapturedContext: false));
			}
		}
		catch (WebException ex)
		{
			if (ex.Status != WebExceptionStatus.RequestCanceled)
			{
				throw new HttpRequestException("An error occurred while sending the request", ex);
			}
		}
		catch (IOException inner)
		{
			throw new HttpRequestException("An error occurred while sending the request", inner);
		}
		if (cancellationToken.IsCancellationRequested)
		{
			TaskCompletionSource<HttpResponseMessage> taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
			taskCompletionSource.SetCanceled();
			return await taskCompletionSource.Task;
		}
		return CreateResponseMessage(wresponse, request, cancellationToken);
	}

	void IMonoHttpClientHandler.SetWebRequestTimeout(TimeSpan timeout)
	{
		this.timeout = timeout;
	}
}
