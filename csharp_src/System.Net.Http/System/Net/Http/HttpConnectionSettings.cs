using System.Collections.Generic;
using System.Net.Security;

namespace System.Net.Http;

internal sealed class HttpConnectionSettings
{
	internal DecompressionMethods _automaticDecompression;

	internal bool _useCookies = true;

	internal CookieContainer _cookieContainer;

	internal bool _useProxy = true;

	internal IWebProxy _proxy;

	internal ICredentials _defaultProxyCredentials;

	internal bool _preAuthenticate;

	internal ICredentials _credentials;

	internal bool _allowAutoRedirect = true;

	internal int _maxAutomaticRedirections = 50;

	internal int _maxConnectionsPerServer = int.MaxValue;

	internal int _maxResponseDrainSize = 1048576;

	internal TimeSpan _maxResponseDrainTime = HttpHandlerDefaults.DefaultResponseDrainTimeout;

	internal int _maxResponseHeadersLength = 64;

	internal TimeSpan _pooledConnectionLifetime = HttpHandlerDefaults.DefaultPooledConnectionLifetime;

	internal TimeSpan _pooledConnectionIdleTimeout = HttpHandlerDefaults.DefaultPooledConnectionIdleTimeout;

	internal TimeSpan _expect100ContinueTimeout = HttpHandlerDefaults.DefaultExpect100ContinueTimeout;

	internal TimeSpan _connectTimeout = HttpHandlerDefaults.DefaultConnectTimeout;

	internal SslClientAuthenticationOptions _sslOptions;

	internal IDictionary<string, object> _properties;

	public HttpConnectionSettings Clone()
	{
		if (_useCookies && _cookieContainer == null)
		{
			_cookieContainer = new CookieContainer();
		}
		return new HttpConnectionSettings
		{
			_allowAutoRedirect = _allowAutoRedirect,
			_automaticDecompression = _automaticDecompression,
			_cookieContainer = _cookieContainer,
			_connectTimeout = _connectTimeout,
			_credentials = _credentials,
			_defaultProxyCredentials = _defaultProxyCredentials,
			_expect100ContinueTimeout = _expect100ContinueTimeout,
			_maxAutomaticRedirections = _maxAutomaticRedirections,
			_maxConnectionsPerServer = _maxConnectionsPerServer,
			_maxResponseDrainSize = _maxResponseDrainSize,
			_maxResponseDrainTime = _maxResponseDrainTime,
			_maxResponseHeadersLength = _maxResponseHeadersLength,
			_pooledConnectionLifetime = _pooledConnectionLifetime,
			_pooledConnectionIdleTimeout = _pooledConnectionIdleTimeout,
			_preAuthenticate = _preAuthenticate,
			_properties = _properties,
			_proxy = _proxy,
			_sslOptions = _sslOptions?.ShallowClone(),
			_useCookies = _useCookies,
			_useProxy = _useProxy
		};
	}
}
