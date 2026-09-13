using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Web.Services.Protocols;

[ComVisible(true)]
public abstract class WebClientProtocol : Component
{
	private string connectionGroupName;

	private ICredentials credentials;

	private bool preAuthenticate;

	private Encoding requestEncoding;

	private int timeout;

	internal Uri uri;

	private WebRequest current_request;

	private static HybridDictionary cache;

	[DefaultValue("")]
	public string ConnectionGroupName
	{
		get
		{
			return connectionGroupName;
		}
		set
		{
			connectionGroupName = value;
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
			credentials = value;
		}
	}

	[DefaultValue(false)]
	[WebServicesDescription("Enables pre authentication of the request.")]
	public bool PreAuthenticate
	{
		get
		{
			return preAuthenticate;
		}
		set
		{
			preAuthenticate = value;
		}
	}

	[DefaultValue(null)]
	[RecommendedAsConfigurable(true)]
	[WebServicesDescription("The encoding to use for requests.")]
	public Encoding RequestEncoding
	{
		get
		{
			return requestEncoding;
		}
		set
		{
			requestEncoding = value;
		}
	}

	[DefaultValue(100000)]
	[RecommendedAsConfigurable(true)]
	[WebServicesDescription("Sets the timeout in milliseconds to be used for synchronous calls.  The default of -1 means infinite.")]
	public int Timeout
	{
		get
		{
			return timeout;
		}
		set
		{
			timeout = value;
		}
	}

	[DefaultValue("")]
	[RecommendedAsConfigurable(true)]
	[WebServicesDescription("The base URL to the server to use for requests.")]
	public string Url
	{
		get
		{
			if (!(uri == null))
			{
				return uri.AbsoluteUri;
			}
			return string.Empty;
		}
		set
		{
			uri = new Uri(value);
		}
	}

	public bool UseDefaultCredentials
	{
		get
		{
			return CredentialCache.DefaultCredentials == Credentials;
		}
		set
		{
			Credentials = (value ? CredentialCache.DefaultCredentials : null);
		}
	}

	static WebClientProtocol()
	{
		cache = new HybridDictionary();
	}

	protected WebClientProtocol()
	{
		connectionGroupName = string.Empty;
		credentials = null;
		preAuthenticate = false;
		requestEncoding = null;
		timeout = 100000;
	}

	public virtual void Abort()
	{
		WebRequest webRequest = current_request;
		current_request = null;
		webRequest?.Abort();
	}

	protected static void AddToCache(Type type, object value)
	{
		cache[type] = value;
	}

	protected static object GetFromCache(Type type)
	{
		return cache[type];
	}

	protected virtual WebRequest GetWebRequest(Uri uri)
	{
		if (uri == null)
		{
			throw new InvalidOperationException("uri is null");
		}
		WebRequest webRequest = WebRequest.Create(uri);
		webRequest.Timeout = timeout;
		webRequest.PreAuthenticate = preAuthenticate;
		webRequest.ConnectionGroupName = connectionGroupName;
		if (credentials != null)
		{
			webRequest.Credentials = credentials;
		}
		current_request = webRequest;
		return webRequest;
	}

	protected virtual WebResponse GetWebResponse(WebRequest request)
	{
		WebResponse webResponse = null;
		try
		{
			request.Timeout = timeout;
			webResponse = request.GetResponse();
		}
		catch (WebException ex)
		{
			webResponse = ex.Response;
			if (webResponse == null)
			{
				throw;
			}
		}
		return webResponse;
	}

	protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
	{
		return request.EndGetResponse(result);
	}
}
