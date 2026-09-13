using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class HttpClientHandler : HttpMessageHandler
{
	private const string SocketsHttpHandlerEnvironmentVariableSettingName = "DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER";

	private const string SocketsHttpHandlerAppCtxSettingName = "System.Net.Http.UseSocketsHttpHandler";

	private readonly IMonoHttpClientHandler _delegatingHandler;

	private ClientCertificateOption _clientCertificateOptions;

	private static bool UseSocketsHttpHandler
	{
		get
		{
			if (AppContext.TryGetSwitch("System.Net.Http.UseSocketsHttpHandler", out var isEnabled))
			{
				return isEnabled;
			}
			string environmentVariable = Environment.GetEnvironmentVariable("DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER");
			if (environmentVariable != null && (environmentVariable.Equals("false", StringComparison.OrdinalIgnoreCase) || environmentVariable.Equals("0")))
			{
				return false;
			}
			return true;
		}
	}

	public static Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> DangerousAcceptAnyServerCertificateValidator { get; } = (HttpRequestMessage _003Cp0_003E, X509Certificate2 _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;

	public virtual bool SupportsAutomaticDecompression => _delegatingHandler.SupportsAutomaticDecompression;

	public virtual bool SupportsProxy => true;

	public virtual bool SupportsRedirectConfiguration => true;

	public bool UseCookies
	{
		get
		{
			return _delegatingHandler.UseCookies;
		}
		set
		{
			_delegatingHandler.UseCookies = value;
		}
	}

	public CookieContainer CookieContainer
	{
		get
		{
			return _delegatingHandler.CookieContainer;
		}
		set
		{
			_delegatingHandler.CookieContainer = value;
		}
	}

	public ClientCertificateOption ClientCertificateOptions
	{
		get
		{
			return _clientCertificateOptions;
		}
		set
		{
			switch (value)
			{
			case ClientCertificateOption.Manual:
				ThrowForModifiedManagedSslOptionsIfStarted();
				_clientCertificateOptions = value;
				_delegatingHandler.SslOptions.LocalCertificateSelectionCallback = (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers) => CertificateHelper.GetEligibleClientCertificate(ClientCertificates);
				break;
			case ClientCertificateOption.Automatic:
				ThrowForModifiedManagedSslOptionsIfStarted();
				_clientCertificateOptions = value;
				_delegatingHandler.SslOptions.LocalCertificateSelectionCallback = (object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers) => CertificateHelper.GetEligibleClientCertificate();
				break;
			default:
				throw new ArgumentOutOfRangeException("value");
			}
		}
	}

	public X509CertificateCollection ClientCertificates
	{
		get
		{
			if (ClientCertificateOptions != ClientCertificateOption.Manual)
			{
				throw new InvalidOperationException(global::SR.Format("The {0} property must be set to '{1}' to use this property.", "ClientCertificateOptions", "Manual"));
			}
			return _delegatingHandler.SslOptions.ClientCertificates ?? (_delegatingHandler.SslOptions.ClientCertificates = new X509CertificateCollection());
		}
	}

	public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool> ServerCertificateCustomValidationCallback
	{
		get
		{
			return (_delegatingHandler.SslOptions.RemoteCertificateValidationCallback?.Target as ConnectHelper.CertificateCallbackMapper)?.FromHttpClientHandler;
		}
		set
		{
			ThrowForModifiedManagedSslOptionsIfStarted();
			_delegatingHandler.SslOptions.RemoteCertificateValidationCallback = ((value != null) ? new ConnectHelper.CertificateCallbackMapper(value).ForSocketsHttpHandler : null);
		}
	}

	public bool CheckCertificateRevocationList
	{
		get
		{
			return _delegatingHandler.SslOptions.CertificateRevocationCheckMode == X509RevocationMode.Online;
		}
		set
		{
			ThrowForModifiedManagedSslOptionsIfStarted();
			_delegatingHandler.SslOptions.CertificateRevocationCheckMode = (value ? X509RevocationMode.Online : X509RevocationMode.NoCheck);
		}
	}

	public SslProtocols SslProtocols
	{
		get
		{
			return _delegatingHandler.SslOptions.EnabledSslProtocols;
		}
		set
		{
			ThrowForModifiedManagedSslOptionsIfStarted();
			_delegatingHandler.SslOptions.EnabledSslProtocols = value;
		}
	}

	public DecompressionMethods AutomaticDecompression
	{
		get
		{
			return _delegatingHandler.AutomaticDecompression;
		}
		set
		{
			_delegatingHandler.AutomaticDecompression = value;
		}
	}

	public bool UseProxy
	{
		get
		{
			return _delegatingHandler.UseProxy;
		}
		set
		{
			_delegatingHandler.UseProxy = value;
		}
	}

	public IWebProxy Proxy
	{
		get
		{
			return _delegatingHandler.Proxy;
		}
		set
		{
			_delegatingHandler.Proxy = value;
		}
	}

	public ICredentials DefaultProxyCredentials
	{
		get
		{
			return _delegatingHandler.DefaultProxyCredentials;
		}
		set
		{
			_delegatingHandler.DefaultProxyCredentials = value;
		}
	}

	public bool PreAuthenticate
	{
		get
		{
			return _delegatingHandler.PreAuthenticate;
		}
		set
		{
			_delegatingHandler.PreAuthenticate = value;
		}
	}

	public bool UseDefaultCredentials
	{
		get
		{
			return _delegatingHandler.Credentials == CredentialCache.DefaultCredentials;
		}
		set
		{
			if (value)
			{
				_delegatingHandler.Credentials = CredentialCache.DefaultCredentials;
			}
			else if (_delegatingHandler.Credentials == CredentialCache.DefaultCredentials)
			{
				_delegatingHandler.Credentials = null;
			}
		}
	}

	public ICredentials Credentials
	{
		get
		{
			return _delegatingHandler.Credentials;
		}
		set
		{
			_delegatingHandler.Credentials = value;
		}
	}

	public bool AllowAutoRedirect
	{
		get
		{
			return _delegatingHandler.AllowAutoRedirect;
		}
		set
		{
			_delegatingHandler.AllowAutoRedirect = value;
		}
	}

	public int MaxAutomaticRedirections
	{
		get
		{
			return _delegatingHandler.MaxAutomaticRedirections;
		}
		set
		{
			_delegatingHandler.MaxAutomaticRedirections = value;
		}
	}

	public int MaxConnectionsPerServer
	{
		get
		{
			return _delegatingHandler.MaxConnectionsPerServer;
		}
		set
		{
			_delegatingHandler.MaxConnectionsPerServer = value;
		}
	}

	public int MaxResponseHeadersLength
	{
		get
		{
			return _delegatingHandler.MaxResponseHeadersLength;
		}
		set
		{
			_delegatingHandler.MaxResponseHeadersLength = value;
		}
	}

	public long MaxRequestContentBufferSize
	{
		get
		{
			return _delegatingHandler.MaxRequestContentBufferSize;
		}
		set
		{
			_delegatingHandler.MaxRequestContentBufferSize = value;
		}
	}

	public IDictionary<string, object> Properties => _delegatingHandler.Properties;

	private static IMonoHttpClientHandler CreateDefaultHandler()
	{
		string text = Environment.GetEnvironmentVariable("XA_HTTP_CLIENT_HANDLER_TYPE")?.Trim();
		if (text != null && text.StartsWith("System.Net.Http.MonoWebRequestHandler", StringComparison.InvariantCulture))
		{
			Type type = Type.GetType(text, throwOnError: false);
			if (type != null)
			{
				return (IMonoHttpClientHandler)Activator.CreateInstance(type);
			}
		}
		return new SocketsHttpHandler();
	}

	public HttpClientHandler()
		: this(CreateDefaultHandler())
	{
	}

	internal HttpClientHandler(IMonoHttpClientHandler handler)
	{
		_delegatingHandler = handler;
		ClientCertificateOptions = ClientCertificateOption.Manual;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_delegatingHandler.Dispose();
		}
		base.Dispose(disposing);
	}

	private void ThrowForModifiedManagedSslOptionsIfStarted()
	{
		_delegatingHandler.SslOptions = _delegatingHandler.SslOptions;
	}

	internal void SetWebRequestTimeout(TimeSpan timeout)
	{
		_delegatingHandler.SetWebRequestTimeout(timeout);
	}

	protected internal override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return _delegatingHandler.SendAsync(request, cancellationToken);
	}
}
