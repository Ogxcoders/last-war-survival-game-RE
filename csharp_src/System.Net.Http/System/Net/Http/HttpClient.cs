using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class HttpClient : HttpMessageInvoker
{
	private static readonly TimeSpan s_defaultTimeout = TimeSpan.FromSeconds(100.0);

	private static readonly TimeSpan s_maxTimeout = TimeSpan.FromMilliseconds(2147483647.0);

	private static readonly TimeSpan s_infiniteTimeout = System.Threading.Timeout.InfiniteTimeSpan;

	private const HttpCompletionOption defaultCompletionOption = HttpCompletionOption.ResponseContentRead;

	private volatile bool _operationStarted;

	private volatile bool _disposed;

	private CancellationTokenSource _pendingRequestsCts;

	private HttpRequestHeaders _defaultRequestHeaders;

	private Uri _baseAddress;

	private TimeSpan _timeout;

	private int _maxResponseContentBufferSize;

	public HttpRequestHeaders DefaultRequestHeaders
	{
		get
		{
			if (_defaultRequestHeaders == null)
			{
				_defaultRequestHeaders = new HttpRequestHeaders();
			}
			return _defaultRequestHeaders;
		}
	}

	public Uri BaseAddress
	{
		get
		{
			return _baseAddress;
		}
		set
		{
			CheckBaseAddress(value, "value");
			CheckDisposedOrStarted();
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.UriBaseAddress(this, value);
			}
			_baseAddress = value;
		}
	}

	public TimeSpan Timeout
	{
		get
		{
			return _timeout;
		}
		set
		{
			if (value != s_infiniteTimeout && (value <= TimeSpan.Zero || value > s_maxTimeout))
			{
				throw new ArgumentOutOfRangeException("value");
			}
			CheckDisposedOrStarted();
			_timeout = value;
		}
	}

	public long MaxResponseContentBufferSize
	{
		get
		{
			return _maxResponseContentBufferSize;
		}
		set
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			if (value > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("value", value, string.Format(CultureInfo.InvariantCulture, "Buffering more than {0} bytes is not supported.", int.MaxValue));
			}
			CheckDisposedOrStarted();
			_maxResponseContentBufferSize = (int)value;
		}
	}

	public HttpClient()
		: this(CreateDefaultHandler())
	{
	}

	public HttpClient(HttpMessageHandler handler)
		: this(handler, disposeHandler: true)
	{
	}

	public HttpClient(HttpMessageHandler handler, bool disposeHandler)
		: base(handler, disposeHandler)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, handler);
		}
		_timeout = s_defaultTimeout;
		_maxResponseContentBufferSize = int.MaxValue;
		_pendingRequestsCts = new CancellationTokenSource();
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this);
		}
	}

	public Task<string> GetStringAsync(string requestUri)
	{
		return GetStringAsync(CreateUri(requestUri));
	}

	public Task<string> GetStringAsync(Uri requestUri)
	{
		return GetStringAsyncCore(GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead));
	}

	private async Task<string> GetStringAsyncCore(Task<HttpResponseMessage> getTask)
	{
		using HttpResponseMessage responseMessage = await getTask.ConfigureAwait(continueOnCapturedContext: false);
		responseMessage.EnsureSuccessStatusCode();
		HttpContent content = responseMessage.Content;
		if (content != null)
		{
			HttpContentHeaders headers = content.Headers;
			Stream stream = content.TryReadAsStream();
			if (stream == null)
			{
				stream = await content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			using Stream responseStream = stream;
			using HttpContent.LimitArrayPoolWriteStream buffer = new HttpContent.LimitArrayPoolWriteStream(_maxResponseContentBufferSize, (int)headers.ContentLength.GetValueOrDefault());
			await responseStream.CopyToAsync(buffer).ConfigureAwait(continueOnCapturedContext: false);
			if (buffer.Length > 0)
			{
				return HttpContent.ReadBufferAsString(buffer.GetBuffer(), headers);
			}
		}
		return string.Empty;
	}

	public Task<byte[]> GetByteArrayAsync(string requestUri)
	{
		return GetByteArrayAsync(CreateUri(requestUri));
	}

	public Task<byte[]> GetByteArrayAsync(Uri requestUri)
	{
		return GetByteArrayAsyncCore(GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead));
	}

	private async Task<byte[]> GetByteArrayAsyncCore(Task<HttpResponseMessage> getTask)
	{
		using HttpResponseMessage responseMessage = await getTask.ConfigureAwait(continueOnCapturedContext: false);
		responseMessage.EnsureSuccessStatusCode();
		HttpContent content = responseMessage.Content;
		if (content != null)
		{
			HttpContentHeaders headers = content.Headers;
			Stream stream = content.TryReadAsStream();
			if (stream == null)
			{
				stream = await content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			using Stream responseStream = stream;
			long? contentLength = headers.ContentLength;
			if (contentLength.HasValue)
			{
				Stream buffer = new HttpContent.LimitMemoryStream(_maxResponseContentBufferSize, (int)contentLength.GetValueOrDefault());
				await responseStream.CopyToAsync(buffer).ConfigureAwait(continueOnCapturedContext: false);
				if (buffer.Length > 0)
				{
					return ((HttpContent.LimitMemoryStream)buffer).GetSizedBuffer();
				}
			}
			else
			{
				Stream buffer = new HttpContent.LimitArrayPoolWriteStream(_maxResponseContentBufferSize);
				try
				{
					await responseStream.CopyToAsync(buffer).ConfigureAwait(continueOnCapturedContext: false);
					if (buffer.Length > 0)
					{
						return ((HttpContent.LimitArrayPoolWriteStream)buffer).ToArray();
					}
				}
				finally
				{
					buffer.Dispose();
				}
			}
		}
		return Array.Empty<byte>();
	}

	public Task<Stream> GetStreamAsync(string requestUri)
	{
		return GetStreamAsync(CreateUri(requestUri));
	}

	public Task<Stream> GetStreamAsync(Uri requestUri)
	{
		return FinishGetStreamAsync(GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead));
	}

	private async Task<Stream> FinishGetStreamAsync(Task<HttpResponseMessage> getTask)
	{
		HttpResponseMessage obj = await getTask.ConfigureAwait(continueOnCapturedContext: false);
		obj.EnsureSuccessStatusCode();
		HttpContent content = obj.Content;
		Stream result;
		if (content != null)
		{
			Stream stream = content.TryReadAsStream();
			if (stream == null)
			{
				stream = await content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			result = stream;
		}
		else
		{
			result = Stream.Null;
		}
		return result;
	}

	public Task<HttpResponseMessage> GetAsync(string requestUri)
	{
		return GetAsync(CreateUri(requestUri));
	}

	public Task<HttpResponseMessage> GetAsync(Uri requestUri)
	{
		return GetAsync(requestUri, HttpCompletionOption.ResponseContentRead);
	}

	public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption)
	{
		return GetAsync(CreateUri(requestUri), completionOption);
	}

	public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption)
	{
		return GetAsync(requestUri, completionOption, CancellationToken.None);
	}

	public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken)
	{
		return GetAsync(CreateUri(requestUri), cancellationToken);
	}

	public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
	{
		return GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken);
	}

	public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		return GetAsync(CreateUri(requestUri), completionOption, cancellationToken);
	}

	public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		return SendAsync(new HttpRequestMessage(HttpMethod.Get, requestUri), completionOption, cancellationToken);
	}

	public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
	{
		return PostAsync(CreateUri(requestUri), content);
	}

	public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
	{
		return PostAsync(requestUri, content, CancellationToken.None);
	}

	public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
	{
		return PostAsync(CreateUri(requestUri), content, cancellationToken);
	}

	public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
	{
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
		httpRequestMessage.Content = content;
		return SendAsync(httpRequestMessage, cancellationToken);
	}

	public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
	{
		return PutAsync(CreateUri(requestUri), content);
	}

	public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
	{
		return PutAsync(requestUri, content, CancellationToken.None);
	}

	public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
	{
		return PutAsync(CreateUri(requestUri), content, cancellationToken);
	}

	public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
	{
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, requestUri);
		httpRequestMessage.Content = content;
		return SendAsync(httpRequestMessage, cancellationToken);
	}

	public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content)
	{
		return PatchAsync(CreateUri(requestUri), content);
	}

	public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content)
	{
		return PatchAsync(requestUri, content, CancellationToken.None);
	}

	public Task<HttpResponseMessage> PatchAsync(string requestUri, HttpContent content, CancellationToken cancellationToken)
	{
		return PatchAsync(CreateUri(requestUri), content, cancellationToken);
	}

	public Task<HttpResponseMessage> PatchAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
	{
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, requestUri);
		httpRequestMessage.Content = content;
		return SendAsync(httpRequestMessage, cancellationToken);
	}

	public Task<HttpResponseMessage> DeleteAsync(string requestUri)
	{
		return DeleteAsync(CreateUri(requestUri));
	}

	public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
	{
		return DeleteAsync(requestUri, CancellationToken.None);
	}

	public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken)
	{
		return DeleteAsync(CreateUri(requestUri), cancellationToken);
	}

	public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken)
	{
		return SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri), cancellationToken);
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
	{
		return SendAsync(request, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
	}

	public override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption)
	{
		return SendAsync(request, completionOption, CancellationToken.None);
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken)
	{
		if (request == null)
		{
			throw new ArgumentNullException("request");
		}
		CheckDisposed();
		CheckRequestMessage(request);
		SetOperationStarted();
		PrepareRequestMessage(request);
		bool flag = _timeout != s_infiniteTimeout;
		bool disposeCts;
		CancellationTokenSource cancellationTokenSource;
		if (flag || cancellationToken.CanBeCanceled)
		{
			disposeCts = true;
			cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _pendingRequestsCts.Token);
			if (flag)
			{
				cancellationTokenSource.CancelAfter(_timeout);
			}
		}
		else
		{
			disposeCts = false;
			cancellationTokenSource = _pendingRequestsCts;
		}
		Task<HttpResponseMessage> sendTask;
		try
		{
			sendTask = base.SendAsync(request, cancellationTokenSource.Token);
		}
		catch
		{
			HandleFinishSendAsyncCleanup(cancellationTokenSource, disposeCts);
			throw;
		}
		if (completionOption != HttpCompletionOption.ResponseContentRead || string.Equals(request.Method.Method, "HEAD", StringComparison.OrdinalIgnoreCase))
		{
			return FinishSendAsyncUnbuffered(sendTask, request, cancellationTokenSource, disposeCts);
		}
		return FinishSendAsyncBuffered(sendTask, request, cancellationTokenSource, disposeCts);
	}

	private async Task<HttpResponseMessage> FinishSendAsyncBuffered(Task<HttpResponseMessage> sendTask, HttpRequestMessage request, CancellationTokenSource cts, bool disposeCts)
	{
		HttpResponseMessage response = null;
		try
		{
			response = await sendTask.ConfigureAwait(continueOnCapturedContext: false);
			if (response == null)
			{
				throw new InvalidOperationException("Handler did not return a response message.");
			}
			if (response.Content != null)
			{
				await response.Content.LoadIntoBufferAsync(_maxResponseContentBufferSize, cts.Token).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.ClientSendCompleted(this, response, request);
			}
			return response;
		}
		catch (Exception e)
		{
			response?.Dispose();
			HandleFinishSendAsyncError(e, cts);
			throw;
		}
		finally
		{
			HandleFinishSendAsyncCleanup(cts, disposeCts);
		}
	}

	private async Task<HttpResponseMessage> FinishSendAsyncUnbuffered(Task<HttpResponseMessage> sendTask, HttpRequestMessage request, CancellationTokenSource cts, bool disposeCts)
	{
		try
		{
			HttpResponseMessage httpResponseMessage = await sendTask.ConfigureAwait(continueOnCapturedContext: false);
			if (httpResponseMessage == null)
			{
				throw new InvalidOperationException("Handler did not return a response message.");
			}
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.ClientSendCompleted(this, httpResponseMessage, request);
			}
			return httpResponseMessage;
		}
		catch (Exception e)
		{
			HandleFinishSendAsyncError(e, cts);
			throw;
		}
		finally
		{
			HandleFinishSendAsyncCleanup(cts, disposeCts);
		}
	}

	private void HandleFinishSendAsyncError(Exception e, CancellationTokenSource cts)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Error(this, e);
		}
		if (cts.IsCancellationRequested && e is HttpRequestException)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, "Canceled");
			}
			throw new OperationCanceledException(cts.Token);
		}
	}

	private void HandleFinishSendAsyncCleanup(CancellationTokenSource cts, bool disposeCts)
	{
		if (disposeCts)
		{
			cts.Dispose();
		}
	}

	public void CancelPendingRequests()
	{
		CheckDisposed();
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this);
		}
		CancellationTokenSource cancellationTokenSource = Interlocked.Exchange(ref _pendingRequestsCts, new CancellationTokenSource());
		cancellationTokenSource.Cancel();
		cancellationTokenSource.Dispose();
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_disposed)
		{
			_disposed = true;
			_pendingRequestsCts.Cancel();
			_pendingRequestsCts.Dispose();
		}
		base.Dispose(disposing);
	}

	private void SetOperationStarted()
	{
		if (!_operationStarted)
		{
			_operationStarted = true;
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

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
	}

	private static void CheckRequestMessage(HttpRequestMessage request)
	{
		if (!request.MarkAsSent())
		{
			throw new InvalidOperationException("The request message was already sent. Cannot send the same request message multiple times.");
		}
	}

	private void PrepareRequestMessage(HttpRequestMessage request)
	{
		Uri uri = null;
		if (request.RequestUri == null && _baseAddress == null)
		{
			throw new InvalidOperationException("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.");
		}
		if (request.RequestUri == null)
		{
			uri = _baseAddress;
		}
		else if (!request.RequestUri.IsAbsoluteUri || (request.RequestUri.Scheme == Uri.UriSchemeFile && request.RequestUri.OriginalString.StartsWith("/", StringComparison.Ordinal)))
		{
			if (_baseAddress == null)
			{
				throw new InvalidOperationException("An invalid request URI was provided. The request URI must either be an absolute URI or BaseAddress must be set.");
			}
			uri = new Uri(_baseAddress, request.RequestUri);
		}
		if (uri != null)
		{
			request.RequestUri = uri;
		}
		if (_defaultRequestHeaders != null)
		{
			request.Headers.AddHeaders(_defaultRequestHeaders);
		}
	}

	private static void CheckBaseAddress(Uri baseAddress, string parameterName)
	{
		if (!(baseAddress == null))
		{
			if (!baseAddress.IsAbsoluteUri)
			{
				throw new ArgumentException("The base address must be an absolute URI.", parameterName);
			}
			if (!HttpUtilities.IsHttpUri(baseAddress))
			{
				throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", parameterName);
			}
		}
	}

	private Uri CreateUri(string uri)
	{
		if (string.IsNullOrEmpty(uri))
		{
			return null;
		}
		return new Uri(uri, UriKind.RelativeOrAbsolute);
	}

	private static HttpMessageHandler CreateDefaultHandler()
	{
		string text = Environment.GetEnvironmentVariable("XA_HTTP_CLIENT_HANDLER_TYPE")?.Trim();
		if (string.IsNullOrEmpty(text))
		{
			return new HttpClientHandler();
		}
		if (text != null && text.StartsWith("System.Net.Http.MonoWebRequestHandler", StringComparison.InvariantCulture))
		{
			Type type = Type.GetType(text, throwOnError: false);
			if (type != null)
			{
				return new HttpClientHandler((IMonoHttpClientHandler)Activator.CreateInstance(type));
			}
			return new HttpClientHandler();
		}
		Type type2 = Type.GetType(text, throwOnError: false);
		if (type2 == null && !text.Contains(", "))
		{
			type2 = Type.GetType(text + ", Mono.Android", throwOnError: false);
		}
		if (type2 == null)
		{
			return new HttpClientHandler();
		}
		if (Activator.CreateInstance(type2) is HttpMessageHandler result)
		{
			return result;
		}
		return new HttpClientHandler();
	}
}
