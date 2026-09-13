using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

public sealed class ClientWebSocket : WebSocket
{
	private enum InternalState
	{
		Created,
		Connecting,
		Connected,
		Disposed
	}

	internal sealed class DefaultWebProxy : IWebProxy
	{
		public static DefaultWebProxy Instance { get; } = new DefaultWebProxy();

		public ICredentials Credentials
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public Uri GetProxy(Uri destination)
		{
			throw new NotSupportedException();
		}

		public bool IsBypassed(Uri host)
		{
			throw new NotSupportedException();
		}
	}

	private readonly ClientWebSocketOptions _options;

	private WebSocketHandle _innerWebSocket;

	private int _state;

	public ClientWebSocketOptions Options => _options;

	public override WebSocketCloseStatus? CloseStatus
	{
		get
		{
			if (WebSocketHandle.IsValid(_innerWebSocket))
			{
				return _innerWebSocket.CloseStatus;
			}
			return null;
		}
	}

	public override string CloseStatusDescription
	{
		get
		{
			if (WebSocketHandle.IsValid(_innerWebSocket))
			{
				return _innerWebSocket.CloseStatusDescription;
			}
			return null;
		}
	}

	public override string SubProtocol
	{
		get
		{
			if (WebSocketHandle.IsValid(_innerWebSocket))
			{
				return _innerWebSocket.SubProtocol;
			}
			return null;
		}
	}

	public override WebSocketState State
	{
		get
		{
			if (WebSocketHandle.IsValid(_innerWebSocket))
			{
				return _innerWebSocket.State;
			}
			return (InternalState)_state switch
			{
				InternalState.Created => WebSocketState.None, 
				InternalState.Connecting => WebSocketState.Connecting, 
				_ => WebSocketState.Closed, 
			};
		}
	}

	public ClientWebSocket()
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Enter(this, null, ".ctor");
		}
		WebSocketHandle.CheckPlatformSupport();
		_state = 0;
		_options = new ClientWebSocketOptions
		{
			Proxy = DefaultWebProxy.Instance
		};
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Exit(this, null, ".ctor");
		}
	}

	public Task ConnectAsync(Uri uri, CancellationToken cancellationToken)
	{
		if (uri == null)
		{
			throw new ArgumentNullException("uri");
		}
		if (!uri.IsAbsoluteUri)
		{
			throw new ArgumentException("This operation is not supported for a relative URI.", "uri");
		}
		if (uri.Scheme != "ws" && uri.Scheme != "wss")
		{
			throw new ArgumentException("Only Uris starting with 'ws://' or 'wss://' are supported.", "uri");
		}
		switch ((InternalState)Interlocked.CompareExchange(ref _state, 1, 0))
		{
		case InternalState.Disposed:
			throw new ObjectDisposedException(GetType().FullName);
		default:
			throw new InvalidOperationException("The WebSocket has already been started.");
		case InternalState.Created:
			_options.SetToReadOnly();
			return ConnectAsyncCore(uri, cancellationToken);
		}
	}

	private async Task ConnectAsyncCore(Uri uri, CancellationToken cancellationToken)
	{
		_innerWebSocket = WebSocketHandle.Create();
		try
		{
			if (Interlocked.CompareExchange(ref _state, 2, 1) != 1)
			{
				throw new ObjectDisposedException(GetType().FullName);
			}
			await _innerWebSocket.ConnectAsyncCore(uri, cancellationToken, _options).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception message)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, message, "ConnectAsyncCore");
			}
			throw;
		}
	}

	public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		ThrowIfNotConnected();
		return _innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
	}

	public override ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		ThrowIfNotConnected();
		return _innerWebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
	}

	public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
	{
		ThrowIfNotConnected();
		return _innerWebSocket.ReceiveAsync(buffer, cancellationToken);
	}

	public override ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
	{
		ThrowIfNotConnected();
		return _innerWebSocket.ReceiveAsync(buffer, cancellationToken);
	}

	public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		ThrowIfNotConnected();
		return _innerWebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
	}

	public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		ThrowIfNotConnected();
		return _innerWebSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
	}

	public override void Abort()
	{
		if (_state != 3)
		{
			if (WebSocketHandle.IsValid(_innerWebSocket))
			{
				_innerWebSocket.Abort();
			}
			Dispose();
		}
	}

	public override void Dispose()
	{
		if (Interlocked.Exchange(ref _state, 3) != 3 && WebSocketHandle.IsValid(_innerWebSocket))
		{
			_innerWebSocket.Dispose();
		}
	}

	private void ThrowIfNotConnected()
	{
		if (_state == 3)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (_state != 2)
		{
			throw new InvalidOperationException("The WebSocket is not connected.");
		}
	}
}
