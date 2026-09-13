using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace System.Web.Services.Protocols;

[ComVisible(true)]
public abstract class HttpWebClientProtocol : WebClientProtocol
{
	private bool allowAutoRedirect;

	private bool enableDecompression;

	private X509CertificateCollection clientCertificates;

	private CookieContainer cookieContainer;

	private IWebProxy proxy;

	private string userAgent;

	private bool _unsafeAuthenticated;

	private Hashtable mappings = new Hashtable();

	[DefaultValue(false)]
	[WebServicesDescription("Enable automatic handling of server redirects.")]
	public bool AllowAutoRedirect
	{
		get
		{
			return allowAutoRedirect;
		}
		set
		{
			allowAutoRedirect = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[WebServicesDescription("The client certificates that will be sent to the server, if the server requests them.")]
	public X509CertificateCollection ClientCertificates
	{
		get
		{
			if (clientCertificates == null)
			{
				clientCertificates = new X509CertificateCollection();
			}
			return clientCertificates;
		}
	}

	[DefaultValue(null)]
	[WebServicesDescription("A container for all cookies received from servers in the current session.")]
	public CookieContainer CookieContainer
	{
		get
		{
			return cookieContainer;
		}
		set
		{
			cookieContainer = value;
		}
	}

	[DefaultValue(false)]
	public bool EnableDecompression
	{
		get
		{
			return enableDecompression;
		}
		set
		{
			enableDecompression = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public IWebProxy Proxy
	{
		get
		{
			return proxy;
		}
		set
		{
			proxy = value;
		}
	}

	[WebServicesDescription("Sets the user agent http header for the request.")]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string UserAgent
	{
		get
		{
			return userAgent;
		}
		set
		{
			userAgent = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool UnsafeAuthenticatedConnectionSharing
	{
		get
		{
			return _unsafeAuthenticated;
		}
		set
		{
			_unsafeAuthenticated = value;
		}
	}

	protected HttpWebClientProtocol()
	{
		allowAutoRedirect = false;
		clientCertificates = null;
		cookieContainer = null;
		proxy = null;
		userAgent = $"Mono Web Services Client Protocol {Environment.Version}";
	}

	internal virtual void CheckForCookies(HttpWebResponse response)
	{
		CookieCollection cookies = response.Cookies;
		if (cookieContainer == null || cookies.Count == 0)
		{
			return;
		}
		CookieCollection cookies2 = cookieContainer.GetCookies(uri);
		foreach (Cookie item in cookies)
		{
			bool flag = true;
			foreach (Cookie item2 in cookies2)
			{
				if (item.Equals(item2))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				cookieContainer.Add(item);
			}
		}
	}

	protected override WebRequest GetWebRequest(Uri uri)
	{
		WebRequest webRequest = base.GetWebRequest(uri);
		if (!(webRequest is HttpWebRequest httpWebRequest))
		{
			return webRequest;
		}
		if (enableDecompression)
		{
			httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
		}
		httpWebRequest.AllowAutoRedirect = allowAutoRedirect;
		if (clientCertificates != null)
		{
			httpWebRequest.ClientCertificates.AddRange(clientCertificates);
		}
		httpWebRequest.CookieContainer = cookieContainer;
		if (proxy != null)
		{
			httpWebRequest.Proxy = proxy;
		}
		httpWebRequest.UserAgent = userAgent;
		return httpWebRequest;
	}

	protected override WebResponse GetWebResponse(WebRequest request)
	{
		WebResponse webResponse = base.GetWebResponse(request);
		if (webResponse is HttpWebResponse response)
		{
			CheckForCookies(response);
		}
		return webResponse;
	}

	protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
	{
		WebResponse webResponse = base.GetWebResponse(request, result);
		if (webResponse is HttpWebResponse response)
		{
			CheckForCookies(response);
		}
		return webResponse;
	}

	internal void RegisterMapping(object userState, WebClientAsyncResult result)
	{
		if (userState == null)
		{
			userState = typeof(string);
		}
		mappings[userState] = result;
	}

	internal void UnregisterMapping(object userState)
	{
		if (userState == null)
		{
			userState = typeof(string);
		}
		mappings.Remove(userState);
	}

	protected void CancelAsync(object userState)
	{
		WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)mappings[userState];
		if (webClientAsyncResult != null)
		{
			mappings.Remove(userState);
			webClientAsyncResult.Abort();
		}
	}

	[System.MonoTODO]
	public static bool GenerateXmlMappings(Type type, ArrayList mapping)
	{
		throw new NotImplementedException();
	}

	[System.MonoTODO]
	public static Hashtable GenerateXmlMappings(Type[] types, ArrayList mapping)
	{
		throw new NotImplementedException();
	}
}
