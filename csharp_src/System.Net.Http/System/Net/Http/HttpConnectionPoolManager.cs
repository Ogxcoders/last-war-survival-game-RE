using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal sealed class HttpConnectionPoolManager : IDisposable
{
	internal readonly struct HttpConnectionKey : IEquatable<HttpConnectionKey>
	{
		public readonly HttpConnectionKind Kind;

		public readonly string Host;

		public readonly int Port;

		public readonly string SslHostName;

		public readonly Uri ProxyUri;

		public HttpConnectionKey(HttpConnectionKind kind, string host, int port, string sslHostName, Uri proxyUri)
		{
			Kind = kind;
			Host = host;
			Port = port;
			SslHostName = sslHostName;
			ProxyUri = proxyUri;
		}

		public override int GetHashCode()
		{
			if (!(SslHostName == Host))
			{
				return HashCode.Combine(Kind, Host, Port, SslHostName, ProxyUri);
			}
			return HashCode.Combine(Kind, Host, Port, ProxyUri);
		}

		public override bool Equals(object obj)
		{
			if (obj != null && obj is HttpConnectionKey)
			{
				return Equals((HttpConnectionKey)obj);
			}
			return false;
		}

		public bool Equals(HttpConnectionKey other)
		{
			if (Kind == other.Kind && Host == other.Host && Port == other.Port && ProxyUri == other.ProxyUri)
			{
				return SslHostName == other.SslHostName;
			}
			return false;
		}

		public static bool operator ==(HttpConnectionKey key1, HttpConnectionKey key2)
		{
			return key1.Equals(key2);
		}

		public static bool operator !=(HttpConnectionKey key1, HttpConnectionKey key2)
		{
			return !key1.Equals(key2);
		}
	}

	private readonly TimeSpan _cleanPoolTimeout;

	private readonly ConcurrentDictionary<HttpConnectionKey, HttpConnectionPool> _pools;

	private readonly Timer _cleaningTimer;

	private readonly int _maxConnectionsPerServer;

	private readonly bool _avoidStoringConnections;

	private readonly HttpConnectionSettings _settings;

	private readonly IWebProxy _proxy;

	private readonly ICredentials _proxyCredentials;

	private bool _timerIsRunning;

	private object SyncObj => _pools;

	public HttpConnectionSettings Settings => _settings;

	public ICredentials ProxyCredentials => _proxyCredentials;

	public bool AvoidStoringConnections => _avoidStoringConnections;

	public HttpConnectionPoolManager(HttpConnectionSettings settings)
	{
		_settings = settings;
		_maxConnectionsPerServer = settings._maxConnectionsPerServer;
		_avoidStoringConnections = settings._pooledConnectionIdleTimeout == TimeSpan.Zero || settings._pooledConnectionLifetime == TimeSpan.Zero;
		_pools = new ConcurrentDictionary<HttpConnectionKey, HttpConnectionPool>();
		if (!_avoidStoringConnections)
		{
			if (settings._pooledConnectionIdleTimeout == Timeout.InfiniteTimeSpan)
			{
				_cleanPoolTimeout = TimeSpan.FromSeconds(30.0);
			}
			else
			{
				TimeSpan timeSpan = settings._pooledConnectionIdleTimeout / 4.0;
				_cleanPoolTimeout = ((timeSpan.TotalSeconds >= 1.0) ? timeSpan : TimeSpan.FromSeconds(1.0));
			}
			bool flag = false;
			try
			{
				if (!ExecutionContext.IsFlowSuppressed())
				{
					ExecutionContext.SuppressFlow();
					flag = true;
				}
				_cleaningTimer = new Timer(delegate(object s)
				{
					if (((WeakReference<HttpConnectionPoolManager>)s).TryGetTarget(out var target))
					{
						target.RemoveStalePools();
					}
				}, new WeakReference<HttpConnectionPoolManager>(this), -1, -1);
			}
			finally
			{
				if (flag)
				{
					ExecutionContext.RestoreFlow();
				}
			}
		}
		if (settings._useProxy)
		{
			_proxy = settings._proxy ?? SystemProxyInfo.ConstructSystemProxy();
			if (_proxy != null)
			{
				_proxyCredentials = _proxy.Credentials ?? settings._defaultProxyCredentials;
			}
		}
	}

	private static string ParseHostNameFromHeader(string hostHeader)
	{
		int num = hostHeader.IndexOf(':');
		if (num >= 0)
		{
			int num2 = hostHeader.IndexOf(']');
			if (num2 == -1)
			{
				return hostHeader.Substring(0, num);
			}
			num = hostHeader.LastIndexOf(':');
			if (num > num2)
			{
				return hostHeader.Substring(0, num);
			}
		}
		return hostHeader;
	}

	private static HttpConnectionKey GetConnectionKey(HttpRequestMessage request, Uri proxyUri, bool isProxyConnect)
	{
		Uri requestUri = request.RequestUri;
		if (isProxyConnect)
		{
			return new HttpConnectionKey(HttpConnectionKind.ProxyConnect, requestUri.IdnHost, requestUri.Port, null, proxyUri);
		}
		string text = null;
		if (HttpUtilities.IsSupportedSecureScheme(requestUri.Scheme))
		{
			string host = request.Headers.Host;
			text = ((host == null) ? requestUri.IdnHost : ParseHostNameFromHeader(host));
		}
		if (proxyUri != null)
		{
			if (text == null)
			{
				if (HttpUtilities.IsNonSecureWebSocketScheme(requestUri.Scheme))
				{
					return new HttpConnectionKey(HttpConnectionKind.ProxyTunnel, requestUri.IdnHost, requestUri.Port, null, proxyUri);
				}
				return new HttpConnectionKey(HttpConnectionKind.Proxy, null, 0, null, proxyUri);
			}
			return new HttpConnectionKey(HttpConnectionKind.SslProxyTunnel, requestUri.IdnHost, requestUri.Port, text, proxyUri);
		}
		if (text != null)
		{
			return new HttpConnectionKey(HttpConnectionKind.Https, requestUri.IdnHost, requestUri.Port, text, null);
		}
		return new HttpConnectionKey(HttpConnectionKind.Http, requestUri.IdnHost, requestUri.Port, null, null);
	}

	public Task<HttpResponseMessage> SendAsyncCore(HttpRequestMessage request, Uri proxyUri, bool doRequestAuth, bool isProxyConnect, CancellationToken cancellationToken)
	{
		HttpConnectionKey connectionKey = GetConnectionKey(request, proxyUri, isProxyConnect);
		HttpConnectionPool value;
		while (!_pools.TryGetValue(connectionKey, out value))
		{
			bool flag = connectionKey.Host != null && request.RequestUri.HostNameType == UriHostNameType.IPv6;
			value = new HttpConnectionPool(this, connectionKey.Kind, flag ? ("[" + connectionKey.Host + "]") : connectionKey.Host, connectionKey.Port, connectionKey.SslHostName, connectionKey.ProxyUri, _maxConnectionsPerServer);
			if (_cleaningTimer == null)
			{
				break;
			}
			if (!_pools.TryAdd(connectionKey, value))
			{
				continue;
			}
			lock (SyncObj)
			{
				if (!_timerIsRunning)
				{
					SetCleaningTimer(_cleanPoolTimeout);
				}
			}
			break;
		}
		return value.SendAsync(request, doRequestAuth, cancellationToken);
	}

	public Task<HttpResponseMessage> SendProxyConnectAsync(HttpRequestMessage request, Uri proxyUri, CancellationToken cancellationToken)
	{
		return SendAsyncCore(request, proxyUri, doRequestAuth: false, isProxyConnect: true, cancellationToken);
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, bool doRequestAuth, CancellationToken cancellationToken)
	{
		if (_proxy == null)
		{
			return SendAsyncCore(request, null, doRequestAuth, isProxyConnect: false, cancellationToken);
		}
		Uri uri = null;
		try
		{
			if (!_proxy.IsBypassed(request.RequestUri))
			{
				uri = _proxy.GetProxy(request.RequestUri);
			}
		}
		catch (Exception arg)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, $"Exception from IWebProxy.GetProxy({request.RequestUri}): {arg}");
			}
		}
		if (uri != null && uri.Scheme != "http")
		{
			throw new NotSupportedException("Only the 'http' scheme is allowed for proxies.");
		}
		return SendAsyncCore(request, uri, doRequestAuth, isProxyConnect: false, cancellationToken);
	}

	public void Dispose()
	{
		_cleaningTimer?.Dispose();
		foreach (KeyValuePair<HttpConnectionKey, HttpConnectionPool> pool in _pools)
		{
			pool.Value.Dispose();
		}
		if (_proxy is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	private void SetCleaningTimer(TimeSpan timeout)
	{
		try
		{
			_cleaningTimer.Change(timeout, timeout);
			_timerIsRunning = timeout != Timeout.InfiniteTimeSpan;
		}
		catch (ObjectDisposedException)
		{
		}
	}

	private void RemoveStalePools()
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this);
		}
		foreach (KeyValuePair<HttpConnectionKey, HttpConnectionPool> pool in _pools)
		{
			if (pool.Value.CleanCacheAndDisposeIfUnused())
			{
				_pools.TryRemove(pool.Key, out var _);
			}
		}
		lock (SyncObj)
		{
			if (_pools.IsEmpty)
			{
				SetCleaningTimer(Timeout.InfiniteTimeSpan);
			}
		}
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this);
		}
	}
}
