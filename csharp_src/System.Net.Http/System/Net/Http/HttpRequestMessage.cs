using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace System.Net.Http;

public class HttpRequestMessage : IDisposable
{
	private const int MessageNotYetSent = 0;

	private const int MessageAlreadySent = 1;

	private int _sendStatus;

	private HttpMethod _method;

	private Uri _requestUri;

	private HttpRequestHeaders _headers;

	private Version _version;

	private HttpContent _content;

	private bool _disposed;

	private IDictionary<string, object> _properties;

	public Version Version
	{
		get
		{
			return _version;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CheckDisposed();
			_version = value;
		}
	}

	public HttpContent Content
	{
		get
		{
			return _content;
		}
		set
		{
			CheckDisposed();
			if (NetEventSource.IsEnabled)
			{
				if (value == null)
				{
					NetEventSource.ContentNull(this);
				}
				else
				{
					NetEventSource.Associate(this, value);
				}
			}
			_content = value;
		}
	}

	public HttpMethod Method
	{
		get
		{
			return _method;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			CheckDisposed();
			_method = value;
		}
	}

	public Uri RequestUri
	{
		get
		{
			return _requestUri;
		}
		set
		{
			if (value != null && !IsAllowedAbsoluteUri(value))
			{
				throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", "value");
			}
			CheckDisposed();
			_requestUri = value;
		}
	}

	public HttpRequestHeaders Headers
	{
		get
		{
			if (_headers == null)
			{
				_headers = new HttpRequestHeaders();
			}
			return _headers;
		}
	}

	internal bool HasHeaders => _headers != null;

	public IDictionary<string, object> Properties
	{
		get
		{
			if (_properties == null)
			{
				_properties = new Dictionary<string, object>();
			}
			return _properties;
		}
	}

	public HttpRequestMessage()
		: this(HttpMethod.Get, (Uri)null)
	{
	}

	public HttpRequestMessage(HttpMethod method, Uri requestUri)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, method, requestUri);
		}
		InitializeValues(method, requestUri);
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this);
		}
	}

	public HttpRequestMessage(HttpMethod method, string requestUri)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, method, requestUri);
		}
		if (string.IsNullOrEmpty(requestUri))
		{
			InitializeValues(method, null);
		}
		else
		{
			InitializeValues(method, new Uri(requestUri, UriKind.RelativeOrAbsolute));
		}
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Method: ");
		stringBuilder.Append(_method);
		stringBuilder.Append(", RequestUri: '");
		stringBuilder.Append((_requestUri == null) ? "<null>" : _requestUri.ToString());
		stringBuilder.Append("', Version: ");
		stringBuilder.Append(_version);
		stringBuilder.Append(", Content: ");
		stringBuilder.Append((_content == null) ? "<null>" : _content.GetType().ToString());
		stringBuilder.Append(", Headers:\r\n");
		stringBuilder.Append(HeaderUtilities.DumpHeaders(_headers, (_content == null) ? null : _content.Headers));
		return stringBuilder.ToString();
	}

	private void InitializeValues(HttpMethod method, Uri requestUri)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		if (requestUri != null && !IsAllowedAbsoluteUri(requestUri))
		{
			throw new ArgumentException("Only 'http' and 'https' schemes are allowed.", "requestUri");
		}
		_method = method;
		_requestUri = requestUri;
		_version = HttpUtilities.DefaultRequestVersion;
	}

	internal bool MarkAsSent()
	{
		return Interlocked.Exchange(ref _sendStatus, 1) == 0;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && !_disposed)
		{
			_disposed = true;
			if (_content != null)
			{
				_content.Dispose();
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().ToString());
		}
	}

	private static bool IsAllowedAbsoluteUri(Uri uri)
	{
		if (!uri.IsAbsoluteUri)
		{
			return true;
		}
		if (uri.Scheme == Uri.UriSchemeFile && uri.OriginalString.StartsWith("/", StringComparison.Ordinal))
		{
			return true;
		}
		return HttpUtilities.IsHttpUri(uri);
	}
}
