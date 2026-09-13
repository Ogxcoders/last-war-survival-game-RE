using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal sealed class HttpConnectionPool : IDisposable
{
	[StructLayout(LayoutKind.Auto)]
	private readonly struct CachedConnection : IEquatable<CachedConnection>
	{
		internal readonly HttpConnection _connection;

		internal readonly DateTimeOffset _returnedTime;

		public CachedConnection(HttpConnection connection)
		{
			_connection = connection;
			_returnedTime = DateTimeOffset.UtcNow;
		}

		public bool IsUsable(DateTimeOffset now, TimeSpan pooledConnectionLifetime, TimeSpan pooledConnectionIdleTimeout, bool poll = false)
		{
			if (pooledConnectionIdleTimeout != Timeout.InfiniteTimeSpan && now - _returnedTime > pooledConnectionIdleTimeout)
			{
				if (NetEventSource.IsEnabled)
				{
					_connection.Trace($"Connection no longer usable. Idle {now - _returnedTime} > {pooledConnectionIdleTimeout}.", "IsUsable");
				}
				return false;
			}
			if (pooledConnectionLifetime != Timeout.InfiniteTimeSpan && now - _connection.CreationTime > pooledConnectionLifetime)
			{
				if (NetEventSource.IsEnabled)
				{
					_connection.Trace($"Connection no longer usable. Alive {now - _connection.CreationTime} > {pooledConnectionLifetime}.", "IsUsable");
				}
				return false;
			}
			if (poll && _connection.PollRead())
			{
				if (NetEventSource.IsEnabled)
				{
					_connection.Trace("Connection no longer usable. Unexpected data received.", "IsUsable");
				}
				return false;
			}
			return true;
		}

		public bool Equals(CachedConnection other)
		{
			return other._connection == _connection;
		}

		public override bool Equals(object obj)
		{
			if (obj is CachedConnection)
			{
				return Equals((CachedConnection)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _connection?.GetHashCode() ?? 0;
		}
	}

	private class ConnectionWaiter : TaskCompletionSource<(HttpConnection, HttpResponseMessage)>
	{
		internal readonly HttpConnectionPool _pool;

		private readonly HttpRequestMessage _request;

		internal readonly CancellationToken _cancellationToken;

		internal CancellationTokenRegistration _cancellationTokenRegistration;

		internal ConnectionWaiter _next;

		internal ConnectionWaiter _prev;

		public ConnectionWaiter(HttpConnectionPool pool, HttpRequestMessage request, CancellationToken cancellationToken)
			: base(TaskCreationOptions.RunContinuationsAsynchronously)
		{
			_pool = pool;
			_request = request;
			_cancellationToken = cancellationToken;
		}

		public ValueTask<(HttpConnection, HttpResponseMessage)> CreateConnectionAsync()
		{
			return _pool.CreateConnectionAsync(_request, _cancellationToken);
		}
	}

	private static readonly bool s_isWindows7Or2008R2 = GetIsWindows7Or2008R2();

	private readonly HttpConnectionPoolManager _poolManager;

	private readonly HttpConnectionKind _kind;

	private readonly string _host;

	private readonly int _port;

	private readonly Uri _proxyUri;

	private readonly List<CachedConnection> _idleConnections = new List<CachedConnection>();

	private readonly int _maxConnections;

	private readonly byte[] _hostHeaderValueBytes;

	private readonly SslClientAuthenticationOptions _sslOptions;

	private ConnectionWaiter _waitersHead;

	private ConnectionWaiter _waitersTail;

	private int _associatedConnectionCount;

	private bool _usedSinceLastCleanup = true;

	private bool _disposed;

	private const int DefaultHttpPort = 80;

	private const int DefaultHttpsPort = 443;

	public HttpConnectionSettings Settings => _poolManager.Settings;

	public bool IsSecure => _sslOptions != null;

	public HttpConnectionKind Kind => _kind;

	public bool AnyProxyKind => _proxyUri != null;

	public Uri ProxyUri => _proxyUri;

	public ICredentials ProxyCredentials => _poolManager.ProxyCredentials;

	public byte[] HostHeaderValueBytes => _hostHeaderValueBytes;

	public CredentialCache PreAuthCredentials { get; }

	private object SyncObj => _idleConnections;

	public HttpConnectionPool(HttpConnectionPoolManager poolManager, HttpConnectionKind kind, string host, int port, string sslHostName, Uri proxyUri, int maxConnections)
	{
		_poolManager = poolManager;
		_kind = kind;
		_host = host;
		_port = port;
		_proxyUri = proxyUri;
		_maxConnections = maxConnections;
		switch (kind)
		{
		case HttpConnectionKind.Https:
			_sslOptions = ConstructSslOptions(poolManager, sslHostName);
			break;
		case HttpConnectionKind.SslProxyTunnel:
			_sslOptions = ConstructSslOptions(poolManager, sslHostName);
			break;
		}
		if (_host != null)
		{
			string s = ((_port != ((_sslOptions == null) ? 80 : 443)) ? $"{_host}:{_port}" : _host);
			_hostHeaderValueBytes = Encoding.ASCII.GetBytes(s);
		}
		if (_poolManager.Settings._preAuthenticate)
		{
			PreAuthCredentials = new CredentialCache();
		}
	}

	private static SslClientAuthenticationOptions ConstructSslOptions(HttpConnectionPoolManager poolManager, string sslHostName)
	{
		SslClientAuthenticationOptions sslClientAuthenticationOptions = poolManager.Settings._sslOptions?.ShallowClone() ?? new SslClientAuthenticationOptions();
		sslClientAuthenticationOptions.ApplicationProtocols = null;
		sslClientAuthenticationOptions.TargetHost = sslHostName;
		if (s_isWindows7Or2008R2 && sslClientAuthenticationOptions.EnabledSslProtocols == SslProtocols.None)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Info(poolManager, $"Win7OrWin2K8R2 platform, Changing default TLS protocols to {SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12}");
			}
			sslClientAuthenticationOptions.EnabledSslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;
		}
		return sslClientAuthenticationOptions;
	}

	private ValueTask<(HttpConnection, HttpResponseMessage)> GetConnectionAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<(HttpConnection, HttpResponseMessage)>(Task.FromCanceled<(HttpConnection, HttpResponseMessage)>(cancellationToken));
		}
		TimeSpan pooledConnectionLifetime = _poolManager.Settings._pooledConnectionLifetime;
		TimeSpan pooledConnectionIdleTimeout = _poolManager.Settings._pooledConnectionIdleTimeout;
		DateTimeOffset utcNow = DateTimeOffset.UtcNow;
		List<CachedConnection> idleConnections = _idleConnections;
		HttpConnection connection;
		while (true)
		{
			CachedConnection cachedConnection;
			lock (SyncObj)
			{
				if (idleConnections.Count <= 0)
				{
					if (_associatedConnectionCount < _maxConnections)
					{
						if (NetEventSource.IsEnabled)
						{
							Trace("Creating new connection for pool.", "GetConnectionAsync");
						}
						IncrementConnectionCountNoLock();
						return WaitForCreatedConnectionAsync(CreateConnectionAsync(request, cancellationToken));
					}
					if (NetEventSource.IsEnabled)
					{
						Trace("Limit reached.  Waiting to create new connection.", "GetConnectionAsync");
					}
					ConnectionWaiter connectionWaiter = new ConnectionWaiter(this, request, cancellationToken);
					EnqueueWaiter(connectionWaiter);
					if (cancellationToken.CanBeCanceled)
					{
						connectionWaiter._cancellationTokenRegistration = cancellationToken.Register(delegate(object s)
						{
							ConnectionWaiter connectionWaiter2 = (ConnectionWaiter)s;
							lock (connectionWaiter2._pool.SyncObj)
							{
								if (connectionWaiter2._pool.RemoveWaiterForCancellation(connectionWaiter2))
								{
									connectionWaiter2.TrySetCanceled(connectionWaiter2._cancellationToken);
								}
							}
						}, connectionWaiter);
					}
					return new ValueTask<(HttpConnection, HttpResponseMessage)>(connectionWaiter.Task);
				}
				cachedConnection = idleConnections[idleConnections.Count - 1];
				idleConnections.RemoveAt(idleConnections.Count - 1);
			}
			connection = cachedConnection._connection;
			if (cachedConnection.IsUsable(utcNow, pooledConnectionLifetime, pooledConnectionIdleTimeout) && !connection.EnsureReadAheadAndPollRead())
			{
				break;
			}
			if (NetEventSource.IsEnabled)
			{
				connection.Trace("Found invalid connection in pool.", "GetConnectionAsync");
			}
			connection.Dispose();
		}
		if (NetEventSource.IsEnabled)
		{
			connection.Trace("Found usable connection in pool.", "GetConnectionAsync");
		}
		return new ValueTask<(HttpConnection, HttpResponseMessage)>((connection, null));
	}

	public async Task<HttpResponseMessage> SendWithRetryAsync(HttpRequestMessage request, bool doRequestAuth, CancellationToken cancellationToken)
	{
		HttpResponseMessage httpResponseMessage;
		while (true)
		{
			HttpConnection connection;
			(connection, httpResponseMessage) = await GetConnectionAsync(request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (httpResponseMessage != null)
			{
				break;
			}
			bool isNewConnection = connection.IsNewConnection;
			connection.Acquire();
			try
			{
				return await SendWithNtConnectionAuthAsync(connection, request, doRequestAuth, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (HttpRequestException ex) when (!isNewConnection && ex.InnerException is IOException && connection.CanRetry)
			{
			}
			finally
			{
				connection.Release();
			}
		}
		return httpResponseMessage;
	}

	private async Task<HttpResponseMessage> SendWithNtConnectionAuthAsync(HttpConnection connection, HttpRequestMessage request, bool doRequestAuth, CancellationToken cancellationToken)
	{
		if (doRequestAuth && Settings._credentials != null)
		{
			return await AuthenticationHelper.SendWithNtConnectionAuthAsync(request, Settings._credentials, connection, this, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		return await SendWithNtProxyAuthAsync(connection, request, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public Task<HttpResponseMessage> SendWithNtProxyAuthAsync(HttpConnection connection, HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (AnyProxyKind && ProxyCredentials != null)
		{
			return AuthenticationHelper.SendWithNtProxyAuthAsync(request, ProxyUri, ProxyCredentials, connection, this, cancellationToken);
		}
		return connection.SendAsync(request, cancellationToken);
	}

	public Task<HttpResponseMessage> SendWithProxyAuthAsync(HttpRequestMessage request, bool doRequestAuth, CancellationToken cancellationToken)
	{
		if ((_kind == HttpConnectionKind.Proxy || _kind == HttpConnectionKind.ProxyConnect) && _poolManager.ProxyCredentials != null)
		{
			return AuthenticationHelper.SendWithProxyAuthAsync(request, _proxyUri, _poolManager.ProxyCredentials, doRequestAuth, this, cancellationToken);
		}
		return SendWithRetryAsync(request, doRequestAuth, cancellationToken);
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, bool doRequestAuth, CancellationToken cancellationToken)
	{
		if (doRequestAuth && Settings._credentials != null)
		{
			return AuthenticationHelper.SendWithRequestAuthAsync(request, Settings._credentials, Settings._preAuthenticate, this, cancellationToken);
		}
		return SendWithProxyAuthAsync(request, doRequestAuth, cancellationToken);
	}

	internal async ValueTask<(HttpConnection, HttpResponseMessage)> CreateConnectionAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		CancellationTokenSource cancellationWithConnectTimeout = null;
		if (Settings._connectTimeout != Timeout.InfiniteTimeSpan)
		{
			cancellationWithConnectTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, default(CancellationToken));
			cancellationWithConnectTimeout.CancelAfter(Settings._connectTimeout);
			cancellationToken = cancellationWithConnectTimeout.Token;
		}
		try
		{
			Socket socket = null;
			Stream stream = null;
			switch (_kind)
			{
			case HttpConnectionKind.Http:
			case HttpConnectionKind.Https:
			case HttpConnectionKind.ProxyConnect:
				(socket, stream) = await ConnectHelper.ConnectAsync(_host, _port, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				break;
			case HttpConnectionKind.Proxy:
				(socket, stream) = await ConnectHelper.ConnectAsync(_proxyUri.IdnHost, _proxyUri.Port, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				break;
			case HttpConnectionKind.ProxyTunnel:
			case HttpConnectionKind.SslProxyTunnel:
			{
				HttpResponseMessage httpResponseMessage;
				(stream, httpResponseMessage) = await EstablishProxyTunnel(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (httpResponseMessage != null)
				{
					httpResponseMessage.RequestMessage = request;
					return (null, httpResponseMessage);
				}
				break;
			}
			}
			TransportContext transportContext = null;
			if (_sslOptions != null)
			{
				transportContext = ((SslStream)(stream = await ConnectHelper.EstablishSslConnectionAsync(_sslOptions, request, stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false))).TransportContext;
			}
			HttpConnection item = ((_maxConnections == int.MaxValue) ? new HttpConnection(this, socket, stream, transportContext) : new HttpConnectionWithFinalizer(this, socket, stream, transportContext));
			return (item, null);
		}
		finally
		{
			cancellationWithConnectTimeout?.Dispose();
		}
	}

	private async ValueTask<(Stream, HttpResponseMessage)> EstablishProxyTunnel(CancellationToken cancellationToken)
	{
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Connect, _proxyUri);
		httpRequestMessage.Headers.Host = $"{_host}:{_port}";
		HttpResponseMessage httpResponseMessage = await _poolManager.SendProxyConnectAsync(httpRequestMessage, _proxyUri, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
		{
			return (null, httpResponseMessage);
		}
		return (await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false), null);
	}

	private void EnqueueWaiter(ConnectionWaiter waiter)
	{
		waiter._next = _waitersHead;
		if (_waitersHead != null)
		{
			_waitersHead._prev = waiter;
		}
		else
		{
			_waitersTail = waiter;
		}
		_waitersHead = waiter;
	}

	private ConnectionWaiter DequeueWaiter()
	{
		ConnectionWaiter waitersTail = _waitersTail;
		_waitersTail = waitersTail._prev;
		if (_waitersTail != null)
		{
			_waitersTail._next = null;
		}
		else
		{
			_waitersHead = null;
		}
		waitersTail._next = null;
		waitersTail._prev = null;
		return waitersTail;
	}

	private bool RemoveWaiterForCancellation(ConnectionWaiter waiter)
	{
		bool result = waiter._next != null || waiter._prev != null || _waitersHead == waiter || _waitersTail == waiter;
		if (waiter._next != null)
		{
			waiter._next._prev = waiter._prev;
		}
		if (waiter._prev != null)
		{
			waiter._prev._next = waiter._next;
		}
		if (_waitersHead == waiter && _waitersTail == waiter)
		{
			_waitersHead = (_waitersTail = null);
		}
		else if (_waitersHead == waiter)
		{
			_waitersHead = waiter._next;
		}
		else if (_waitersTail == waiter)
		{
			_waitersTail = waiter._prev;
		}
		waiter._next = null;
		waiter._prev = null;
		return result;
	}

	private async ValueTask<(HttpConnection, HttpResponseMessage)> WaitForCreatedConnectionAsync(ValueTask<(HttpConnection, HttpResponseMessage)> creationTask)
	{
		try
		{
			var (httpConnection, item) = await creationTask.ConfigureAwait(continueOnCapturedContext: false);
			if (httpConnection == null)
			{
				DecrementConnectionCount();
			}
			return (httpConnection, item);
		}
		catch
		{
			DecrementConnectionCount();
			throw;
		}
	}

	public void IncrementConnectionCount()
	{
		lock (SyncObj)
		{
			IncrementConnectionCountNoLock();
		}
	}

	private void IncrementConnectionCountNoLock()
	{
		if (NetEventSource.IsEnabled)
		{
			Trace(null, "IncrementConnectionCountNoLock");
		}
		_usedSinceLastCleanup = true;
		_associatedConnectionCount++;
	}

	public void DecrementConnectionCount()
	{
		if (NetEventSource.IsEnabled)
		{
			Trace(null, "DecrementConnectionCount");
		}
		lock (SyncObj)
		{
			_usedSinceLastCleanup = true;
			if (_waitersHead == null)
			{
				_associatedConnectionCount--;
				return;
			}
			ConnectionWaiter connectionWaiter = DequeueWaiter();
			connectionWaiter._cancellationTokenRegistration.Dispose();
			ValueTask<(HttpConnection, HttpResponseMessage)> valueTask = connectionWaiter.CreateConnectionAsync();
			if (valueTask.IsCompletedSuccessfully)
			{
				connectionWaiter.SetResult(valueTask.Result);
				return;
			}
			valueTask.AsTask().ContinueWith(delegate(Task<(HttpConnection, HttpResponseMessage)> innerConnectionTask, object state)
			{
				ConnectionWaiter connectionWaiter2 = (ConnectionWaiter)state;
				try
				{
					if (innerConnectionTask.GetAwaiter().GetResult().Item2 != null)
					{
						connectionWaiter2._pool.DecrementConnectionCount();
					}
					connectionWaiter2.SetResult(innerConnectionTask.Result);
				}
				catch (Exception exception)
				{
					connectionWaiter2.SetException(exception);
					connectionWaiter2._pool.DecrementConnectionCount();
				}
			}, connectionWaiter, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}
	}

	public void ReturnConnection(HttpConnection connection)
	{
		List<CachedConnection> idleConnections = _idleConnections;
		lock (SyncObj)
		{
			_usedSinceLastCleanup = true;
			if (_waitersTail != null && !connection.EnsureReadAheadAndPollRead())
			{
				ConnectionWaiter connectionWaiter = DequeueWaiter();
				connectionWaiter._cancellationTokenRegistration.Dispose();
				if (NetEventSource.IsEnabled)
				{
					connection.Trace("Transferring connection returned to pool.", "ReturnConnection");
				}
				connectionWaiter.SetResult((connection, null));
			}
			else if (_disposed || _poolManager.AvoidStoringConnections)
			{
				if (NetEventSource.IsEnabled)
				{
					connection.Trace("Disposing connection returned to disposed pool.", "ReturnConnection");
				}
				connection.Dispose();
			}
			else
			{
				idleConnections.Add(new CachedConnection(connection));
				if (NetEventSource.IsEnabled)
				{
					connection.Trace("Stored connection in pool.", "ReturnConnection");
				}
			}
		}
	}

	public void Dispose()
	{
		List<CachedConnection> idleConnections = _idleConnections;
		lock (SyncObj)
		{
			if (!_disposed)
			{
				if (NetEventSource.IsEnabled)
				{
					Trace("Disposing pool.", "Dispose");
				}
				_disposed = true;
				idleConnections.ForEach(delegate(CachedConnection c)
				{
					c._connection.Dispose();
				});
				idleConnections.Clear();
			}
		}
	}

	public bool CleanCacheAndDisposeIfUnused()
	{
		TimeSpan pooledConnectionLifetime = _poolManager.Settings._pooledConnectionLifetime;
		TimeSpan pooledConnectionIdleTimeout = _poolManager.Settings._pooledConnectionIdleTimeout;
		List<CachedConnection> idleConnections = _idleConnections;
		List<HttpConnection> list = null;
		bool lockTaken = false;
		try
		{
			if (NetEventSource.IsEnabled)
			{
				Trace("Cleaning pool.", "CleanCacheAndDisposeIfUnused");
			}
			Monitor.Enter(SyncObj, ref lockTaken);
			DateTimeOffset utcNow = DateTimeOffset.UtcNow;
			int i;
			for (i = 0; i < idleConnections.Count && idleConnections[i].IsUsable(utcNow, pooledConnectionLifetime, pooledConnectionIdleTimeout, poll: true); i++)
			{
			}
			if (i < idleConnections.Count)
			{
				list = new List<HttpConnection> { idleConnections[i]._connection };
				int j = i + 1;
				while (j < idleConnections.Count)
				{
					for (; j < idleConnections.Count && !idleConnections[j].IsUsable(utcNow, pooledConnectionLifetime, pooledConnectionIdleTimeout, poll: true); j++)
					{
						list.Add(idleConnections[j]._connection);
					}
					if (j < idleConnections.Count)
					{
						idleConnections[i++] = idleConnections[j++];
					}
				}
				idleConnections.RemoveRange(i, idleConnections.Count - i);
				if (_associatedConnectionCount == 0 && !_usedSinceLastCleanup)
				{
					_disposed = true;
					return true;
				}
			}
			_usedSinceLastCleanup = false;
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(SyncObj);
			}
			list?.ForEach(delegate(HttpConnection c)
			{
				c.Dispose();
			});
		}
		return false;
	}

	private static bool GetIsWindows7Or2008R2()
	{
		OperatingSystem oSVersion = Environment.OSVersion;
		if (oSVersion.Platform == PlatformID.Win32NT)
		{
			Version version = oSVersion.Version;
			if (version.Major == 6)
			{
				return version.Minor == 1;
			}
			return false;
		}
		return false;
	}

	public override string ToString()
	{
		return "HttpConnectionPool" + ((!(_proxyUri == null)) ? ((_sslOptions == null) ? $"Proxy {_proxyUri}" : ($"https://{_host}:{_port}/ tunnelled via Proxy {_proxyUri}" + ((_sslOptions.TargetHost != _host) ? (", SSL TargetHost=" + _sslOptions.TargetHost) : null))) : ((_sslOptions == null) ? $"http://{_host}:{_port}" : ($"https://{_host}:{_port}" + ((_sslOptions.TargetHost != _host) ? (", SSL TargetHost=" + _sslOptions.TargetHost) : null))));
	}

	private void Trace(string message, [CallerMemberName] string memberName = null)
	{
		NetEventSource.Log.HandlerMessage(GetHashCode(), 0, 0, memberName, ToString() + ":" + message);
	}
}
