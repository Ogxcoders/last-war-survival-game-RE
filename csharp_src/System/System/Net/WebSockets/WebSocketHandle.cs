using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

internal sealed class WebSocketHandle
{
	[ThreadStatic]
	private static StringBuilder t_cachedStringBuilder;

	private static readonly Encoding s_defaultHttpEncoding = Encoding.GetEncoding(28591);

	private const int DefaultReceiveBufferSize = 4096;

	private const string WSServerGuid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

	private readonly CancellationTokenSource _abortSource = new CancellationTokenSource();

	private WebSocketState _state = WebSocketState.Connecting;

	private WebSocket _webSocket;

	public WebSocketCloseStatus? CloseStatus => _webSocket?.CloseStatus;

	public string CloseStatusDescription => _webSocket?.CloseStatusDescription;

	public WebSocketState State => _webSocket?.State ?? _state;

	public string SubProtocol => _webSocket?.SubProtocol;

	public static WebSocketHandle Create()
	{
		return new WebSocketHandle();
	}

	public static bool IsValid(WebSocketHandle handle)
	{
		return handle != null;
	}

	public static void CheckPlatformSupport()
	{
	}

	public void Dispose()
	{
		_state = WebSocketState.Closed;
		_webSocket?.Dispose();
	}

	public void Abort()
	{
		_abortSource.Cancel();
		_webSocket?.Abort();
	}

	public Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		return _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
	}

	public ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		return _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
	}

	public Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
	{
		return _webSocket.ReceiveAsync(buffer, cancellationToken);
	}

	public ValueTask<ValueWebSocketReceiveResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken)
	{
		return _webSocket.ReceiveAsync(buffer, cancellationToken);
	}

	public Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		return _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
	}

	public Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		return _webSocket.CloseOutputAsync(closeStatus, statusDescription, cancellationToken);
	}

	public async Task ConnectAsyncCore(Uri uri, CancellationToken cancellationToken, ClientWebSocketOptions options)
	{
		CancellationTokenRegistration registration = cancellationToken.Register(delegate(object s)
		{
			((WebSocketHandle)s).Abort();
		}, this);
		try
		{
			Stream stream = new NetworkStream(await ConnectSocketAsync(uri.Host, uri.Port, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), ownsSocket: true);
			if (uri.Scheme == "wss")
			{
				SslStream sslStream = new SslStream(stream);
				await sslStream.AuthenticateAsClientAsync(uri.Host, options.ClientCertificates, SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12, checkCertificateRevocation: false).ConfigureAwait(continueOnCapturedContext: false);
				stream = sslStream;
			}
			KeyValuePair<string, string> secKeyAndSecWebSocketAccept = CreateSecKeyAndSecWebSocketAccept();
			byte[] array = BuildRequestHeader(uri, options, secKeyAndSecWebSocketAccept.Key);
			await stream.WriteAsync(array, 0, array.Length, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			_webSocket = WebSocket.CreateClientWebSocket(stream, await ParseAndValidateConnectResponseAsync(stream, options, secKeyAndSecWebSocketAccept.Value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false), options.ReceiveBufferSize, options.SendBufferSize, options.KeepAliveInterval, useZeroMaskingKey: false, options.Buffer.GetValueOrDefault());
			if (_state == WebSocketState.Aborted)
			{
				_webSocket.Abort();
			}
			else if (_state == WebSocketState.Closed)
			{
				_webSocket.Dispose();
			}
		}
		catch (Exception ex)
		{
			if (_state < WebSocketState.Closed)
			{
				_state = WebSocketState.Closed;
			}
			Abort();
			if (ex is WebSocketException)
			{
				throw;
			}
			throw new WebSocketException("Unable to connect to the remote server", ex);
		}
		finally
		{
			registration.Dispose();
		}
	}

	private async Task<Socket> ConnectSocketAsync(string host, int port, CancellationToken cancellationToken)
	{
		IPAddress[] array = await Dns.GetHostAddressesAsync(host).ConfigureAwait(continueOnCapturedContext: false);
		ExceptionDispatchInfo exceptionDispatchInfo = null;
		IPAddress[] array2 = array;
		foreach (IPAddress iPAddress in array2)
		{
			Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			try
			{
				using (cancellationToken.Register(delegate(object s)
				{
					((Socket)s).Dispose();
				}, socket))
				{
					using (_abortSource.Token.Register(delegate(object s)
					{
						((Socket)s).Dispose();
					}, socket))
					{
						_ = 1;
						try
						{
							await socket.ConnectAsync(iPAddress, port).ConfigureAwait(continueOnCapturedContext: false);
						}
						catch (ObjectDisposedException innerException)
						{
							CancellationToken token = (cancellationToken.IsCancellationRequested ? cancellationToken : _abortSource.Token);
							if (token.IsCancellationRequested)
							{
								throw new OperationCanceledException(new OperationCanceledException().Message, innerException, token);
							}
						}
					}
				}
				cancellationToken.ThrowIfCancellationRequested();
				_abortSource.Token.ThrowIfCancellationRequested();
				return socket;
			}
			catch (Exception source)
			{
				socket.Dispose();
				exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source);
			}
		}
		exceptionDispatchInfo?.Throw();
		throw new WebSocketException("Unable to connect to the remote server");
	}

	private static byte[] BuildRequestHeader(Uri uri, ClientWebSocketOptions options, string secKey)
	{
		StringBuilder stringBuilder = t_cachedStringBuilder ?? (t_cachedStringBuilder = new StringBuilder());
		try
		{
			stringBuilder.Append("GET ").Append(uri.PathAndQuery).Append(" HTTP/1.1\r\n");
			string value = options.RequestHeaders["Host"];
			stringBuilder.Append("Host: ");
			if (string.IsNullOrEmpty(value))
			{
				stringBuilder.Append(uri.IdnHost).Append(':').Append(uri.Port)
					.Append("\r\n");
			}
			else
			{
				stringBuilder.Append(value).Append("\r\n");
			}
			stringBuilder.Append("Connection: Upgrade\r\n");
			stringBuilder.Append("Upgrade: websocket\r\n");
			stringBuilder.Append("Sec-WebSocket-Version: 13\r\n");
			stringBuilder.Append("Sec-WebSocket-Key: ").Append(secKey).Append("\r\n");
			string[] allKeys = options.RequestHeaders.AllKeys;
			foreach (string text in allKeys)
			{
				if (!string.Equals(text, "Host", StringComparison.OrdinalIgnoreCase))
				{
					stringBuilder.Append(text).Append(": ").Append(options.RequestHeaders[text])
						.Append("\r\n");
				}
			}
			if (options.RequestedSubProtocols.Count > 0)
			{
				stringBuilder.Append("Sec-WebSocket-Protocol").Append(": ");
				stringBuilder.Append(options.RequestedSubProtocols[0]);
				for (int j = 1; j < options.RequestedSubProtocols.Count; j++)
				{
					stringBuilder.Append(", ").Append(options.RequestedSubProtocols[j]);
				}
				stringBuilder.Append("\r\n");
			}
			if (options.Cookies != null)
			{
				string cookieHeader = options.Cookies.GetCookieHeader(uri);
				if (!string.IsNullOrWhiteSpace(cookieHeader))
				{
					stringBuilder.Append("Cookie").Append(": ").Append(cookieHeader)
						.Append("\r\n");
				}
			}
			stringBuilder.Append("\r\n");
			return s_defaultHttpEncoding.GetBytes(stringBuilder.ToString());
		}
		finally
		{
			stringBuilder.Clear();
		}
	}

	private static KeyValuePair<string, string> CreateSecKeyAndSecWebSocketAccept()
	{
		string text = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
		using SHA1 sHA = SHA1.Create();
		return new KeyValuePair<string, string>(text, Convert.ToBase64String(sHA.ComputeHash(Encoding.ASCII.GetBytes(text + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11"))));
	}

	private async Task<string> ParseAndValidateConnectResponseAsync(Stream stream, ClientWebSocketOptions options, string expectedSecWebSocketAccept, CancellationToken cancellationToken)
	{
		string text = await ReadResponseHeaderLineAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		if (string.IsNullOrEmpty(text))
		{
			throw new WebSocketException(global::SR.Format("Unable to connect to the remote server"));
		}
		if (!text.StartsWith("HTTP/1.1 ", StringComparison.Ordinal) || text.Length < "HTTP/1.1 101".Length)
		{
			throw new WebSocketException(WebSocketError.HeaderError);
		}
		if (!text.StartsWith("HTTP/1.1 101", StringComparison.Ordinal) || (text.Length > "HTTP/1.1 101".Length && !char.IsWhiteSpace(text["HTTP/1.1 101".Length])))
		{
			throw new WebSocketException("Unable to connect to the remote server");
		}
		bool foundUpgrade = false;
		bool foundConnection = false;
		bool foundSecWebSocketAccept = false;
		string subprotocol = null;
		string text2;
		while (!string.IsNullOrEmpty(text2 = await ReadResponseHeaderLineAsync(stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false)))
		{
			int num = text2.IndexOf(':');
			if (num == -1)
			{
				throw new WebSocketException(WebSocketError.HeaderError);
			}
			string text3 = text2.SubstringTrim(0, num);
			string headerValue = text2.SubstringTrim(num + 1);
			ValidateAndTrackHeader("Connection", "Upgrade", text3, headerValue, ref foundConnection);
			ValidateAndTrackHeader("Upgrade", "websocket", text3, headerValue, ref foundUpgrade);
			ValidateAndTrackHeader("Sec-WebSocket-Accept", expectedSecWebSocketAccept, text3, headerValue, ref foundSecWebSocketAccept);
			if (string.Equals("Sec-WebSocket-Protocol", text3, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(headerValue))
			{
				string text4 = options.RequestedSubProtocols.Find((string requested) => string.Equals(requested, headerValue, StringComparison.OrdinalIgnoreCase));
				if (text4 == null || subprotocol != null)
				{
					throw new WebSocketException(WebSocketError.UnsupportedProtocol, global::SR.Format("The WebSocket client request requested '{0}' protocol(s), but server is only accepting '{1}' protocol(s).", string.Join(", ", options.RequestedSubProtocols), subprotocol));
				}
				subprotocol = text4;
			}
		}
		if (!foundUpgrade || !foundConnection || !foundSecWebSocketAccept)
		{
			throw new WebSocketException("Unable to connect to the remote server");
		}
		return subprotocol;
	}

	private static void ValidateAndTrackHeader(string targetHeaderName, string targetHeaderValue, string foundHeaderName, string foundHeaderValue, ref bool foundHeader)
	{
		bool flag = string.Equals(targetHeaderName, foundHeaderName, StringComparison.OrdinalIgnoreCase);
		if (!foundHeader)
		{
			if (flag)
			{
				if (!string.Equals(targetHeaderValue, foundHeaderValue, StringComparison.OrdinalIgnoreCase))
				{
					throw new WebSocketException(global::SR.Format("The '{0}' header value '{1}' is invalid.", targetHeaderName, foundHeaderValue));
				}
				foundHeader = true;
			}
		}
		else if (flag)
		{
			throw new WebSocketException(global::SR.Format("Unable to connect to the remote server"));
		}
	}

	private static async Task<string> ReadResponseHeaderLineAsync(Stream stream, CancellationToken cancellationToken)
	{
		StringBuilder sb = t_cachedStringBuilder;
		if (sb != null)
		{
			t_cachedStringBuilder = null;
		}
		else
		{
			sb = new StringBuilder();
		}
		byte[] arr = new byte[1];
		char prevChar = '\0';
		try
		{
			while (await stream.ReadAsync(arr, 0, 1, cancellationToken).ConfigureAwait(continueOnCapturedContext: false) == 1)
			{
				char c = (char)arr[0];
				if (prevChar == '\r' && c == '\n')
				{
					break;
				}
				sb.Append(c);
				prevChar = c;
			}
			if (sb.Length > 0 && sb[sb.Length - 1] == '\r')
			{
				sb.Length--;
			}
			return sb.ToString();
		}
		finally
		{
			sb.Clear();
			t_cachedStringBuilder = sb;
		}
	}
}
