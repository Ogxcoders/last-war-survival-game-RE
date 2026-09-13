using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace System.Net;

public sealed class HttpListenerRequest
{
	private class Context : TransportContext
	{
		public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
		{
			throw new NotImplementedException();
		}
	}

	private delegate X509Certificate2 GCCDelegate();

	private string[] accept_types;

	private Encoding content_encoding;

	private long content_length;

	private bool cl_set;

	private CookieCollection cookies;

	private WebHeaderCollection headers;

	private string method;

	private Stream input_stream;

	private Version version;

	private NameValueCollection query_string;

	private string raw_url;

	private Uri url;

	private Uri referrer;

	private string[] user_languages;

	private HttpListenerContext context;

	private bool is_chunked;

	private bool ka_set;

	private bool keep_alive;

	private GCCDelegate gcc_delegate;

	private static byte[] _100continue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");

	private static char[] separators = new char[1] { ' ' };

	public string[] AcceptTypes => accept_types;

	public int ClientCertificateError
	{
		get
		{
			HttpConnection connection = context.Connection;
			if (connection.ClientCertificate == null)
			{
				throw new InvalidOperationException("No client certificate");
			}
			int[] clientCertificateErrors = connection.ClientCertificateErrors;
			if (clientCertificateErrors != null && clientCertificateErrors.Length != 0)
			{
				return clientCertificateErrors[0];
			}
			return 0;
		}
	}

	public Encoding ContentEncoding
	{
		get
		{
			if (content_encoding == null)
			{
				content_encoding = Encoding.Default;
			}
			return content_encoding;
		}
	}

	public long ContentLength64
	{
		get
		{
			if (!is_chunked)
			{
				return content_length;
			}
			return -1L;
		}
	}

	public string ContentType => headers["content-type"];

	public CookieCollection Cookies
	{
		get
		{
			if (cookies == null)
			{
				cookies = new CookieCollection();
			}
			return cookies;
		}
	}

	public bool HasEntityBody
	{
		get
		{
			if (content_length <= 0)
			{
				return is_chunked;
			}
			return true;
		}
	}

	public NameValueCollection Headers => headers;

	public string HttpMethod => method;

	public Stream InputStream
	{
		get
		{
			if (input_stream == null)
			{
				if (is_chunked || content_length > 0)
				{
					input_stream = context.Connection.GetRequestStream(is_chunked, content_length);
				}
				else
				{
					input_stream = Stream.Null;
				}
			}
			return input_stream;
		}
	}

	[System.MonoTODO("Always returns false")]
	public bool IsAuthenticated => false;

	public bool IsLocal => LocalEndPoint.Address.Equals(RemoteEndPoint.Address);

	public bool IsSecureConnection => context.Connection.IsSecure;

	public bool KeepAlive
	{
		get
		{
			if (ka_set)
			{
				return keep_alive;
			}
			ka_set = true;
			string text = headers["Connection"];
			if (!string.IsNullOrEmpty(text))
			{
				keep_alive = string.Compare(text, "keep-alive", StringComparison.OrdinalIgnoreCase) == 0;
			}
			else if (version == HttpVersion.Version11)
			{
				keep_alive = true;
			}
			else
			{
				text = headers["keep-alive"];
				if (!string.IsNullOrEmpty(text))
				{
					keep_alive = string.Compare(text, "closed", StringComparison.OrdinalIgnoreCase) != 0;
				}
			}
			return keep_alive;
		}
	}

	public IPEndPoint LocalEndPoint => context.Connection.LocalEndPoint;

	public Version ProtocolVersion => version;

	public NameValueCollection QueryString => query_string;

	public string RawUrl => raw_url;

	public IPEndPoint RemoteEndPoint => context.Connection.RemoteEndPoint;

	[System.MonoTODO("Always returns Guid.Empty")]
	public Guid RequestTraceIdentifier => Guid.Empty;

	public Uri Url => url;

	public Uri UrlReferrer => referrer;

	public string UserAgent => headers["user-agent"];

	public string UserHostAddress => LocalEndPoint.ToString();

	public string UserHostName => headers["host"];

	public string[] UserLanguages => user_languages;

	[System.MonoTODO]
	public string ServiceName => null;

	public TransportContext TransportContext => new Context();

	[System.MonoTODO]
	public bool IsWebSocketRequest => false;

	internal HttpListenerRequest(HttpListenerContext context)
	{
		this.context = context;
		headers = new WebHeaderCollection();
		version = HttpVersion.Version10;
	}

	internal void SetRequestLine(string req)
	{
		string[] array = req.Split(separators, 3);
		if (array.Length != 3)
		{
			context.ErrorMessage = "Invalid request line (parts).";
			return;
		}
		method = array[0];
		string text = method;
		foreach (char c in text)
		{
			int num = c;
			if ((num < 65 || num > 90) && (num <= 32 || c >= '\u007f' || c == '(' || c == ')' || c == '<' || c == '<' || c == '>' || c == '@' || c == ',' || c == ';' || c == ':' || c == '\\' || c == '"' || c == '/' || c == '[' || c == ']' || c == '?' || c == '=' || c == '{' || c == '}'))
			{
				context.ErrorMessage = "(Invalid verb)";
				return;
			}
		}
		raw_url = array[1];
		if (array[2].Length != 8 || !array[2].StartsWith("HTTP/"))
		{
			context.ErrorMessage = "Invalid request line (version).";
			return;
		}
		try
		{
			version = new Version(array[2].Substring(5));
			if (version.Major < 1)
			{
				throw new Exception();
			}
		}
		catch
		{
			context.ErrorMessage = "Invalid request line (version).";
		}
	}

	private void CreateQueryString(string query)
	{
		if (query == null || query.Length == 0)
		{
			query_string = new NameValueCollection(1);
			return;
		}
		query_string = new NameValueCollection();
		if (query[0] == '?')
		{
			query = query.Substring(1);
		}
		string[] array = query.Split('&');
		foreach (string text in array)
		{
			int num = text.IndexOf('=');
			if (num == -1)
			{
				query_string.Add(null, WebUtility.UrlDecode(text));
				continue;
			}
			string name = WebUtility.UrlDecode(text.Substring(0, num));
			string value = WebUtility.UrlDecode(text.Substring(num + 1));
			query_string.Add(name, value);
		}
	}

	private static bool MaybeUri(string s)
	{
		int num = s.IndexOf(':');
		if (num == -1)
		{
			return false;
		}
		if (num >= 10)
		{
			return false;
		}
		return IsPredefinedScheme(s.Substring(0, num));
	}

	private static bool IsPredefinedScheme(string scheme)
	{
		if (scheme == null || scheme.Length < 3)
		{
			return false;
		}
		char c = scheme[0];
		if (c == 'h')
		{
			if (!(scheme == "http"))
			{
				return scheme == "https";
			}
			return true;
		}
		if (c == 'f')
		{
			if (!(scheme == "file"))
			{
				return scheme == "ftp";
			}
			return true;
		}
		if (c == 'n')
		{
			c = scheme[1];
			if (c == 'e')
			{
				if (!(scheme == "news") && !(scheme == "net.pipe"))
				{
					return scheme == "net.tcp";
				}
				return true;
			}
			if (scheme == "nntp")
			{
				return true;
			}
			return false;
		}
		if ((c == 'g' && scheme == "gopher") || (c == 'm' && scheme == "mailto"))
		{
			return true;
		}
		return false;
	}

	internal bool FinishInitialization()
	{
		string text = UserHostName;
		if (version > HttpVersion.Version10 && (text == null || text.Length == 0))
		{
			context.ErrorMessage = "Invalid host name";
			return true;
		}
		Uri result = null;
		string text2 = ((!MaybeUri(raw_url.ToLowerInvariant()) || !Uri.TryCreate(raw_url, UriKind.Absolute, out result)) ? raw_url : result.PathAndQuery);
		if (text == null || text.Length == 0)
		{
			text = UserHostAddress;
		}
		if (result != null)
		{
			text = result.Host;
		}
		int num = text.IndexOf(':');
		if (num >= 0)
		{
			text = text.Substring(0, num);
		}
		string text3 = string.Format("{0}://{1}:{2}", IsSecureConnection ? "https" : "http", text, LocalEndPoint.Port);
		if (!Uri.TryCreate(text3 + text2, UriKind.Absolute, out url))
		{
			context.ErrorMessage = WebUtility.HtmlEncode("Invalid url: " + text3 + text2);
			return true;
		}
		CreateQueryString(url.Query);
		url = HttpListenerRequestUriBuilder.GetRequestUri(raw_url, url.Scheme, url.Authority, url.LocalPath, url.Query);
		if (version >= HttpVersion.Version11)
		{
			string text4 = Headers["Transfer-Encoding"];
			is_chunked = text4 != null && string.Compare(text4, "chunked", StringComparison.OrdinalIgnoreCase) == 0;
			if (text4 != null && !is_chunked)
			{
				context.Connection.SendError(null, 501);
				return false;
			}
		}
		if (!is_chunked && !cl_set && (string.Compare(method, "POST", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(method, "PUT", StringComparison.OrdinalIgnoreCase) == 0))
		{
			context.Connection.SendError(null, 411);
			return false;
		}
		if (string.Compare(Headers["Expect"], "100-continue", StringComparison.OrdinalIgnoreCase) == 0)
		{
			context.Connection.GetResponseStream().InternalWrite(_100continue, 0, _100continue.Length);
		}
		return true;
	}

	internal static string Unquote(string str)
	{
		int num = str.IndexOf('"');
		int num2 = str.LastIndexOf('"');
		if (num >= 0 && num2 >= 0)
		{
			str = str.Substring(num + 1, num2 - 1);
		}
		return str.Trim();
	}

	internal void AddHeader(string header)
	{
		int num = header.IndexOf(':');
		if (num == -1 || num == 0)
		{
			context.ErrorMessage = "Bad Request";
			context.ErrorStatus = 400;
			return;
		}
		string text = header.Substring(0, num).Trim();
		string text2 = header.Substring(num + 1).Trim();
		string text3 = text.ToLower(CultureInfo.InvariantCulture);
		headers.SetInternal(text, text2);
		switch (text3)
		{
		case "accept-language":
			user_languages = text2.Split(',');
			break;
		case "accept":
			accept_types = text2.Split(',');
			break;
		case "content-length":
			try
			{
				content_length = long.Parse(text2.Trim());
				if (content_length < 0)
				{
					context.ErrorMessage = "Invalid Content-Length.";
				}
				cl_set = true;
				break;
			}
			catch
			{
				context.ErrorMessage = "Invalid Content-Length.";
				break;
			}
		case "referer":
			try
			{
				referrer = new Uri(text2);
				break;
			}
			catch
			{
				referrer = new Uri("http://someone.is.screwing.with.the.headers.com/");
				break;
			}
		case "cookie":
		{
			if (cookies == null)
			{
				cookies = new CookieCollection();
			}
			string[] array = text2.Split(',', ';');
			Cookie cookie = null;
			int num2 = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text4 = array2[i].Trim();
				if (text4.Length == 0)
				{
					continue;
				}
				if (text4.StartsWith("$Version"))
				{
					num2 = int.Parse(Unquote(text4.Substring(text4.IndexOf('=') + 1)));
					continue;
				}
				if (text4.StartsWith("$Path"))
				{
					if (cookie != null)
					{
						cookie.Path = text4.Substring(text4.IndexOf('=') + 1).Trim();
					}
					continue;
				}
				if (text4.StartsWith("$Domain"))
				{
					if (cookie != null)
					{
						cookie.Domain = text4.Substring(text4.IndexOf('=') + 1).Trim();
					}
					continue;
				}
				if (text4.StartsWith("$Port"))
				{
					if (cookie != null)
					{
						cookie.Port = text4.Substring(text4.IndexOf('=') + 1).Trim();
					}
					continue;
				}
				if (cookie != null)
				{
					cookies.Add(cookie);
				}
				try
				{
					cookie = new Cookie();
					int num3 = text4.IndexOf('=');
					if (num3 > 0)
					{
						cookie.Name = text4.Substring(0, num3).Trim();
						cookie.Value = text4.Substring(num3 + 1).Trim();
					}
					else
					{
						cookie.Name = text4.Trim();
						cookie.Value = string.Empty;
					}
					cookie.Version = num2;
				}
				catch (CookieException)
				{
					cookie = null;
				}
			}
			if (cookie != null)
			{
				cookies.Add(cookie);
			}
			break;
		}
		}
	}

	internal bool FlushInput()
	{
		if (!HasEntityBody)
		{
			return true;
		}
		int num = 2048;
		if (content_length > 0)
		{
			num = (int)Math.Min(content_length, num);
		}
		byte[] buffer = new byte[num];
		while (true)
		{
			try
			{
				IAsyncResult asyncResult = InputStream.BeginRead(buffer, 0, num, null, null);
				if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(1000))
				{
					return false;
				}
				if (InputStream.EndRead(asyncResult) <= 0)
				{
					return true;
				}
			}
			catch (ObjectDisposedException)
			{
				input_stream = null;
				return true;
			}
			catch
			{
				return false;
			}
		}
	}

	public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
	{
		if (gcc_delegate == null)
		{
			gcc_delegate = GetClientCertificate;
		}
		return gcc_delegate.BeginInvoke(requestCallback, state);
	}

	public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
	{
		if (asyncResult == null)
		{
			throw new ArgumentNullException("asyncResult");
		}
		if (gcc_delegate == null)
		{
			throw new InvalidOperationException();
		}
		return gcc_delegate.EndInvoke(asyncResult);
	}

	public X509Certificate2 GetClientCertificate()
	{
		return context.Connection.ClientCertificate;
	}

	public Task<X509Certificate2> GetClientCertificateAsync()
	{
		return Task<X509Certificate2>.Factory.FromAsync(BeginGetClientCertificate, EndGetClientCertificate, null);
	}
}
