using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.WebSockets;

internal sealed class ManagedWebSocket : WebSocket
{
	private sealed class Utf8MessageState
	{
		internal bool SequenceInProgress;

		internal int AdditionalBytesExpected;

		internal int ExpectedValueMin;

		internal int CurrentDecodeBits;
	}

	private enum MessageOpcode : byte
	{
		Continuation = 0,
		Text = 1,
		Binary = 2,
		Close = 8,
		Ping = 9,
		Pong = 10
	}

	[StructLayout(LayoutKind.Auto)]
	private struct MessageHeader
	{
		internal MessageOpcode Opcode;

		internal bool Fin;

		internal long PayloadLength;

		internal int Mask;
	}

	private interface IWebSocketReceiveResultGetter<TResult>
	{
		TResult GetResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeDescription);
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private readonly struct WebSocketReceiveResultGetter : IWebSocketReceiveResultGetter<WebSocketReceiveResult>
	{
		public WebSocketReceiveResult GetResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeDescription)
		{
			return new WebSocketReceiveResult(count, messageType, endOfMessage, closeStatus, closeDescription);
		}
	}

	private static readonly RandomNumberGenerator s_random = RandomNumberGenerator.Create();

	private static readonly UTF8Encoding s_textEncoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

	private static readonly WebSocketState[] s_validSendStates = new WebSocketState[2]
	{
		WebSocketState.Open,
		WebSocketState.CloseReceived
	};

	private static readonly WebSocketState[] s_validReceiveStates = new WebSocketState[2]
	{
		WebSocketState.Open,
		WebSocketState.CloseSent
	};

	private static readonly WebSocketState[] s_validCloseOutputStates = new WebSocketState[2]
	{
		WebSocketState.Open,
		WebSocketState.CloseReceived
	};

	private static readonly WebSocketState[] s_validCloseStates = new WebSocketState[3]
	{
		WebSocketState.Open,
		WebSocketState.CloseReceived,
		WebSocketState.CloseSent
	};

	private static readonly Task<WebSocketReceiveResult> s_cachedCloseTask = Task.FromResult(new WebSocketReceiveResult(0, WebSocketMessageType.Close, endOfMessage: true));

	internal const int MaxMessageHeaderLength = 14;

	private const int MaxControlPayloadLength = 125;

	private const int MaskLength = 4;

	private readonly Stream _stream;

	private readonly bool _isServer;

	private readonly string _subprotocol;

	private readonly Timer _keepAliveTimer;

	private readonly CancellationTokenSource _abortSource = new CancellationTokenSource();

	private Memory<byte> _receiveBuffer;

	private readonly Utf8MessageState _utf8TextState = new Utf8MessageState();

	private readonly SemaphoreSlim _sendFrameAsyncLock = new SemaphoreSlim(1, 1);

	private WebSocketState _state = WebSocketState.Open;

	private bool _disposed;

	private bool _sentCloseFrame;

	private bool _receivedCloseFrame;

	private WebSocketCloseStatus? _closeStatus;

	private string _closeStatusDescription;

	private MessageHeader _lastReceiveHeader = new MessageHeader
	{
		Opcode = MessageOpcode.Text,
		Fin = true
	};

	private int _receiveBufferOffset;

	private int _receiveBufferCount;

	private int _receivedMaskOffsetOffset;

	private byte[] _sendBuffer;

	private bool _lastSendWasFragment;

	private Task _lastReceiveAsync = Task.CompletedTask;

	private object StateUpdateLock => _abortSource;

	private object ReceiveAsyncLock => _utf8TextState;

	public override WebSocketCloseStatus? CloseStatus => _closeStatus;

	public override string CloseStatusDescription => _closeStatusDescription;

	public override WebSocketState State => _state;

	public override string SubProtocol => _subprotocol;

	public static ManagedWebSocket CreateFromConnectedStream(Stream stream, bool isServer, string subprotocol, TimeSpan keepAliveInterval)
	{
		return new ManagedWebSocket(stream, isServer, subprotocol, keepAliveInterval);
	}

	private ManagedWebSocket(Stream stream, bool isServer, string subprotocol, TimeSpan keepAliveInterval)
	{
		_stream = stream;
		_isServer = isServer;
		_subprotocol = subprotocol;
		_receiveBuffer = new byte[125];
		_abortSource.Token.Register(delegate(object s)
		{
			ManagedWebSocket managedWebSocket = (ManagedWebSocket)s;
			lock (managedWebSocket.StateUpdateLock)
			{
				WebSocketState state = managedWebSocket._state;
				if (state != WebSocketState.Closed && state != WebSocketState.Aborted)
				{
					managedWebSocket._state = ((state != WebSocketState.None && state != WebSocketState.Connecting) ? WebSocketState.Aborted : WebSocketState.Closed);
				}
			}
		}, this);
		if (keepAliveInterval > TimeSpan.Zero)
		{
			_keepAliveTimer = new Timer(delegate(object s)
			{
				((ManagedWebSocket)s).SendKeepAliveFrameAsync();
			}, this, keepAliveInterval, keepAliveInterval);
		}
	}

	public override void Dispose()
	{
		lock (StateUpdateLock)
		{
			DisposeCore();
		}
	}

	private void DisposeCore()
	{
		if (!_disposed)
		{
			_disposed = true;
			_keepAliveTimer?.Dispose();
			_stream?.Dispose();
			if (_state < WebSocketState.Aborted)
			{
				_state = WebSocketState.Closed;
			}
		}
	}

	public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		if (messageType != WebSocketMessageType.Text && messageType != WebSocketMessageType.Binary)
		{
			throw new ArgumentException(global::SR.Format("The message type '{0}' is not allowed for the '{1}' operation. Valid message types are: '{2}, {3}'. To close the WebSocket, use the '{4}' operation instead. ", "Close", "SendAsync", "Binary", "Text", "CloseOutputAsync"), "messageType");
		}
		WebSocketValidate.ValidateArraySegment(buffer, "buffer");
		return SendPrivateAsync(buffer, messageType, endOfMessage, cancellationToken).AsTask();
	}

	private ValueTask SendPrivateAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
	{
		if (messageType != WebSocketMessageType.Text && messageType != WebSocketMessageType.Binary)
		{
			throw new ArgumentException(global::SR.Format("The message type '{0}' is not allowed for the '{1}' operation. Valid message types are: '{2}, {3}'. To close the WebSocket, use the '{4}' operation instead. ", "Close", "SendAsync", "Binary", "Text", "CloseOutputAsync"), "messageType");
		}
		try
		{
			WebSocketValidate.ThrowIfInvalidState(_state, _disposed, s_validSendStates);
		}
		catch (Exception exception)
		{
			return new ValueTask(Task.FromException(exception));
		}
		MessageOpcode opcode = ((!_lastSendWasFragment) ? ((messageType != WebSocketMessageType.Binary) ? MessageOpcode.Text : MessageOpcode.Binary) : MessageOpcode.Continuation);
		ValueTask result = SendFrameAsync(opcode, endOfMessage, buffer, cancellationToken);
		_lastSendWasFragment = !endOfMessage;
		return result;
	}

	public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
	{
		WebSocketValidate.ValidateArraySegment(buffer, "buffer");
		try
		{
			WebSocketValidate.ThrowIfInvalidState(_state, _disposed, s_validReceiveStates);
			lock (ReceiveAsyncLock)
			{
				ThrowIfOperationInProgress(_lastReceiveAsync.IsCompleted, "ReceiveAsync");
				return (Task<WebSocketReceiveResult>)(_lastReceiveAsync = ReceiveAsyncPrivate<WebSocketReceiveResultGetter, WebSocketReceiveResult>(buffer, cancellationToken).AsTask());
			}
		}
		catch (Exception exception)
		{
			return Task.FromException<WebSocketReceiveResult>(exception);
		}
	}

	public override Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		WebSocketValidate.ValidateCloseStatus(closeStatus, statusDescription);
		try
		{
			WebSocketValidate.ThrowIfInvalidState(_state, _disposed, s_validCloseStates);
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
		return CloseAsyncPrivate(closeStatus, statusDescription, cancellationToken);
	}

	public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		WebSocketValidate.ValidateCloseStatus(closeStatus, statusDescription);
		try
		{
			WebSocketValidate.ThrowIfInvalidState(_state, _disposed, s_validCloseOutputStates);
		}
		catch (Exception exception)
		{
			return Task.FromException(exception);
		}
		return SendCloseFrameAsync(closeStatus, statusDescription, cancellationToken);
	}

	public override void Abort()
	{
		_abortSource.Cancel();
		Dispose();
	}

	private ValueTask SendFrameAsync(MessageOpcode opcode, bool endOfMessage, ReadOnlyMemory<byte> payloadBuffer, CancellationToken cancellationToken)
	{
		if (!cancellationToken.CanBeCanceled && _sendFrameAsyncLock.Wait(0))
		{
			return SendFrameLockAcquiredNonCancelableAsync(opcode, endOfMessage, payloadBuffer);
		}
		return new ValueTask(SendFrameFallbackAsync(opcode, endOfMessage, payloadBuffer, cancellationToken));
	}

	private ValueTask SendFrameLockAcquiredNonCancelableAsync(MessageOpcode opcode, bool endOfMessage, ReadOnlyMemory<byte> payloadBuffer)
	{
		ValueTask valueTask = default(ValueTask);
		bool flag = true;
		try
		{
			int length = WriteFrameToSendBuffer(opcode, endOfMessage, payloadBuffer.Span);
			valueTask = _stream.WriteAsync(new ReadOnlyMemory<byte>(_sendBuffer, 0, length));
			if (valueTask.IsCompleted)
			{
				return valueTask;
			}
			flag = false;
		}
		catch (Exception ex)
		{
			return new ValueTask(Task.FromException((ex is OperationCanceledException) ? ex : ((_state == WebSocketState.Aborted) ? CreateOperationCanceledException(ex) : new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex))));
		}
		finally
		{
			if (flag)
			{
				_sendFrameAsyncLock.Release();
				ReleaseSendBuffer();
			}
		}
		return new ValueTask(WaitForWriteTaskAsync(valueTask));
	}

	private async Task WaitForWriteTaskAsync(ValueTask writeTask)
	{
		try
		{
			await writeTask.ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex) when (!(ex is OperationCanceledException))
		{
			throw (_state == WebSocketState.Aborted) ? CreateOperationCanceledException(ex) : new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
		}
		finally
		{
			_sendFrameAsyncLock.Release();
			ReleaseSendBuffer();
		}
	}

	private async Task SendFrameFallbackAsync(MessageOpcode opcode, bool endOfMessage, ReadOnlyMemory<byte> payloadBuffer, CancellationToken cancellationToken)
	{
		await _sendFrameAsyncLock.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			int length = WriteFrameToSendBuffer(opcode, endOfMessage, payloadBuffer.Span);
			using (cancellationToken.Register(delegate(object s)
			{
				((ManagedWebSocket)s).Abort();
			}, this))
			{
				await _stream.WriteAsync(new ReadOnlyMemory<byte>(_sendBuffer, 0, length), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		catch (Exception ex) when (!(ex is OperationCanceledException))
		{
			throw (_state == WebSocketState.Aborted) ? CreateOperationCanceledException(ex, cancellationToken) : new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
		}
		finally
		{
			_sendFrameAsyncLock.Release();
			ReleaseSendBuffer();
		}
	}

	private int WriteFrameToSendBuffer(MessageOpcode opcode, bool endOfMessage, ReadOnlySpan<byte> payloadBuffer)
	{
		AllocateSendBuffer(payloadBuffer.Length + 14);
		int? num = null;
		int num2;
		if (_isServer)
		{
			num2 = WriteHeader(opcode, _sendBuffer, payloadBuffer, endOfMessage, useMask: false);
		}
		else
		{
			num = WriteHeader(opcode, _sendBuffer, payloadBuffer, endOfMessage, useMask: true);
			num2 = num.GetValueOrDefault() + 4;
		}
		if (payloadBuffer.Length > 0)
		{
			payloadBuffer.CopyTo(new Span<byte>(_sendBuffer, num2, payloadBuffer.Length));
			if (num.HasValue)
			{
				ApplyMask(new Span<byte>(_sendBuffer, num2, payloadBuffer.Length), _sendBuffer, num.Value, 0);
			}
		}
		return num2 + payloadBuffer.Length;
	}

	private void SendKeepAliveFrameAsync()
	{
		if (!_sendFrameAsyncLock.Wait(0))
		{
			return;
		}
		ValueTask valueTask = SendFrameLockAcquiredNonCancelableAsync(MessageOpcode.Ping, endOfMessage: true, Memory<byte>.Empty);
		if (valueTask.IsCompletedSuccessfully)
		{
			valueTask.GetAwaiter().GetResult();
			return;
		}
		valueTask.AsTask().ContinueWith(delegate(Task p)
		{
			_ = p.Exception;
		}, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
	}

	private static int WriteHeader(MessageOpcode opcode, byte[] sendBuffer, ReadOnlySpan<byte> payload, bool endOfMessage, bool useMask)
	{
		sendBuffer[0] = (byte)opcode;
		if (endOfMessage)
		{
			sendBuffer[0] |= 128;
		}
		int num;
		if (payload.Length <= 125)
		{
			sendBuffer[1] = (byte)payload.Length;
			num = 2;
		}
		else if (payload.Length <= 65535)
		{
			sendBuffer[1] = 126;
			sendBuffer[2] = (byte)(payload.Length / 256);
			sendBuffer[3] = (byte)payload.Length;
			num = 4;
		}
		else
		{
			sendBuffer[1] = 127;
			int num2 = payload.Length;
			for (int num3 = 9; num3 >= 2; num3--)
			{
				sendBuffer[num3] = (byte)num2;
				num2 /= 256;
			}
			num = 10;
		}
		if (useMask)
		{
			sendBuffer[1] |= 128;
			WriteRandomMask(sendBuffer, num);
		}
		return num;
	}

	private static void WriteRandomMask(byte[] buffer, int offset)
	{
		s_random.GetBytes(buffer, offset, 4);
	}

	private async ValueTask<TWebSocketReceiveResult> ReceiveAsyncPrivate<TWebSocketReceiveResultGetter, TWebSocketReceiveResult>(Memory<byte> payloadBuffer, CancellationToken cancellationToken, TWebSocketReceiveResultGetter resultGetter = default(TWebSocketReceiveResultGetter)) where TWebSocketReceiveResultGetter : struct, IWebSocketReceiveResultGetter<TWebSocketReceiveResult>
	{
		CancellationTokenRegistration registration = cancellationToken.Register(delegate(object s)
		{
			((ManagedWebSocket)s).Abort();
		}, this);
		try
		{
			MessageHeader header;
			Span<byte> span;
			while (true)
			{
				header = _lastReceiveHeader;
				if (header.PayloadLength == 0L)
				{
					if (_receiveBufferCount < (_isServer ? 14 : 10))
					{
						if (_receiveBufferCount < 2)
						{
							await EnsureBufferContainsAsync(2, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						}
						span = _receiveBuffer.Span;
						long num = span[_receiveBufferOffset + 1] & 0x7F;
						if (_isServer || num > 125)
						{
							int minimumRequiredBytes = 2 + (_isServer ? 4 : 0) + ((num > 125) ? ((num == 126) ? 2 : 8) : 0);
							await EnsureBufferContainsAsync(minimumRequiredBytes, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						}
					}
					if (!TryParseMessageHeaderFromReceiveBuffer(out header))
					{
						await CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted).ConfigureAwait(continueOnCapturedContext: false);
					}
					_receivedMaskOffsetOffset = 0;
				}
				if (header.Opcode != MessageOpcode.Ping && header.Opcode != MessageOpcode.Pong)
				{
					break;
				}
				await HandleReceivedPingPongAsync(header, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (header.Opcode == MessageOpcode.Close)
			{
				await HandleReceivedCloseAsync(header, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				return resultGetter.GetResult(0, WebSocketMessageType.Close, endOfMessage: true, _closeStatus, _closeStatusDescription);
			}
			if (header.Opcode == MessageOpcode.Continuation)
			{
				header.Opcode = _lastReceiveHeader.Opcode;
			}
			if (header.PayloadLength == 0L || payloadBuffer.Length == 0)
			{
				_lastReceiveHeader = header;
				return resultGetter.GetResult(0, (header.Opcode != MessageOpcode.Text) ? WebSocketMessageType.Binary : WebSocketMessageType.Text, header.Fin && header.PayloadLength == 0, null, null);
			}
			int totalBytesReceived = 0;
			if (_receiveBufferCount > 0)
			{
				int num2 = Math.Min(payloadBuffer.Length, (int)Math.Min(header.PayloadLength, _receiveBufferCount));
				span = _receiveBuffer.Span;
				span = span.Slice(_receiveBufferOffset, num2);
				span.CopyTo(payloadBuffer.Span);
				ConsumeFromBuffer(num2);
				totalBytesReceived += num2;
			}
			int num3;
			for (; totalBytesReceived < payloadBuffer.Length && totalBytesReceived < header.PayloadLength; totalBytesReceived += num3)
			{
				num3 = await _stream.ReadAsync(payloadBuffer.Slice(totalBytesReceived, (int)Math.Min(payloadBuffer.Length, header.PayloadLength) - totalBytesReceived), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				if (num3 <= 0)
				{
					ThrowIfEOFUnexpected(throwOnPrematureClosure: true);
					break;
				}
			}
			if (_isServer)
			{
				ManagedWebSocket managedWebSocket = this;
				span = payloadBuffer.Span;
				managedWebSocket._receivedMaskOffsetOffset = ApplyMask(span.Slice(0, totalBytesReceived), header.Mask, _receivedMaskOffsetOffset);
			}
			header.PayloadLength -= totalBytesReceived;
			if (header.Opcode == MessageOpcode.Text)
			{
				span = payloadBuffer.Span;
				if (!TryValidateUtf8(span.Slice(0, totalBytesReceived), header.Fin && header.PayloadLength == 0, _utf8TextState))
				{
					await CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.InvalidPayloadData, WebSocketError.Faulted).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			_lastReceiveHeader = header;
			return resultGetter.GetResult(totalBytesReceived, (header.Opcode != MessageOpcode.Text) ? WebSocketMessageType.Binary : WebSocketMessageType.Text, header.Fin && header.PayloadLength == 0, null, null);
		}
		catch (Exception ex) when (!(ex is OperationCanceledException))
		{
			if (_state == WebSocketState.Aborted)
			{
				throw new OperationCanceledException("Aborted", ex);
			}
			_abortSource.Cancel();
			throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, ex);
		}
		finally
		{
			registration.Dispose();
		}
	}

	private async Task HandleReceivedCloseAsync(MessageHeader header, CancellationToken cancellationToken)
	{
		lock (StateUpdateLock)
		{
			_receivedCloseFrame = true;
			if (_state < WebSocketState.CloseReceived)
			{
				_state = WebSocketState.CloseReceived;
			}
		}
		WebSocketCloseStatus closeStatus = WebSocketCloseStatus.NormalClosure;
		string closeStatusDescription = string.Empty;
		if (header.PayloadLength == 1)
		{
			await CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted).ConfigureAwait(continueOnCapturedContext: false);
		}
		else if (header.PayloadLength >= 2)
		{
			if (_receiveBufferCount < header.PayloadLength)
			{
				await EnsureBufferContainsAsync((int)header.PayloadLength, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (_isServer)
			{
				ApplyMask(_receiveBuffer.Span.Slice(_receiveBufferOffset, (int)header.PayloadLength), header.Mask, 0);
			}
			closeStatus = (WebSocketCloseStatus)((_receiveBuffer.Span[_receiveBufferOffset] << 8) | _receiveBuffer.Span[_receiveBufferOffset + 1]);
			if (!IsValidCloseStatus(closeStatus))
			{
				await CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (header.PayloadLength > 2)
			{
				try
				{
					closeStatusDescription = s_textEncoding.GetString(_receiveBuffer.Span.Slice(_receiveBufferOffset + 2, (int)header.PayloadLength - 2));
				}
				catch (DecoderFallbackException innerException)
				{
					await CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus.ProtocolError, WebSocketError.Faulted, innerException).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			ConsumeFromBuffer((int)header.PayloadLength);
		}
		_closeStatus = closeStatus;
		_closeStatusDescription = closeStatusDescription;
		if (!_isServer && _sentCloseFrame)
		{
			await WaitForServerToCloseConnectionAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task WaitForServerToCloseConnectionAsync(CancellationToken cancellationToken)
	{
		ValueTask<int> valueTask = _stream.ReadAsync(_receiveBuffer, cancellationToken);
		if (valueTask.IsCompletedSuccessfully)
		{
			return;
		}
		using CancellationTokenSource finalCts = new CancellationTokenSource(1000);
		using (finalCts.Token.Register(delegate(object s)
		{
			((ManagedWebSocket)s).Abort();
		}, this))
		{
			try
			{
				await valueTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch
			{
			}
		}
	}

	private async Task HandleReceivedPingPongAsync(MessageHeader header, CancellationToken cancellationToken)
	{
		if (header.PayloadLength > 0 && _receiveBufferCount < header.PayloadLength)
		{
			await EnsureBufferContainsAsync((int)header.PayloadLength, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (header.Opcode == MessageOpcode.Ping)
		{
			if (_isServer)
			{
				ApplyMask(_receiveBuffer.Span.Slice(_receiveBufferOffset, (int)header.PayloadLength), header.Mask, 0);
			}
			await SendFrameAsync(MessageOpcode.Pong, endOfMessage: true, _receiveBuffer.Slice(_receiveBufferOffset, (int)header.PayloadLength), default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (header.PayloadLength > 0)
		{
			ConsumeFromBuffer((int)header.PayloadLength);
		}
	}

	private static bool IsValidCloseStatus(WebSocketCloseStatus closeStatus)
	{
		if (closeStatus < WebSocketCloseStatus.NormalClosure || closeStatus >= (WebSocketCloseStatus)5000)
		{
			return false;
		}
		if (closeStatus >= (WebSocketCloseStatus)3000)
		{
			return true;
		}
		if ((uint)(closeStatus - 1000) <= 3u || (uint)(closeStatus - 1007) <= 4u)
		{
			return true;
		}
		return false;
	}

	private async Task CloseWithReceiveErrorAndThrowAsync(WebSocketCloseStatus closeStatus, WebSocketError error, Exception innerException = null)
	{
		if (!_sentCloseFrame)
		{
			await CloseOutputAsync(closeStatus, string.Empty, default(CancellationToken)).ConfigureAwait(continueOnCapturedContext: false);
		}
		_receiveBufferCount = 0;
		throw new WebSocketException(error, innerException);
	}

	private bool TryParseMessageHeaderFromReceiveBuffer(out MessageHeader resultHeader)
	{
		MessageHeader messageHeader = default(MessageHeader);
		Span<byte> span = _receiveBuffer.Span;
		messageHeader.Fin = (span[_receiveBufferOffset] & 0x80) != 0;
		bool flag = (span[_receiveBufferOffset] & 0x70) != 0;
		messageHeader.Opcode = (MessageOpcode)(span[_receiveBufferOffset] & 0xF);
		bool flag2 = (span[_receiveBufferOffset + 1] & 0x80) != 0;
		messageHeader.PayloadLength = span[_receiveBufferOffset + 1] & 0x7F;
		ConsumeFromBuffer(2);
		if (messageHeader.PayloadLength == 126)
		{
			messageHeader.PayloadLength = (span[_receiveBufferOffset] << 8) | span[_receiveBufferOffset + 1];
			ConsumeFromBuffer(2);
		}
		else if (messageHeader.PayloadLength == 127)
		{
			messageHeader.PayloadLength = 0L;
			for (int i = 0; i < 8; i++)
			{
				messageHeader.PayloadLength = (messageHeader.PayloadLength << 8) | span[_receiveBufferOffset + i];
			}
			ConsumeFromBuffer(8);
		}
		bool flag3 = flag;
		if (flag2)
		{
			if (!_isServer)
			{
				flag3 = true;
			}
			messageHeader.Mask = CombineMaskBytes(span, _receiveBufferOffset);
			ConsumeFromBuffer(4);
		}
		switch (messageHeader.Opcode)
		{
		case MessageOpcode.Continuation:
			if (_lastReceiveHeader.Fin)
			{
				flag3 = true;
			}
			break;
		case MessageOpcode.Text:
		case MessageOpcode.Binary:
			if (!_lastReceiveHeader.Fin)
			{
				flag3 = true;
			}
			break;
		case MessageOpcode.Close:
		case MessageOpcode.Ping:
		case MessageOpcode.Pong:
			if (messageHeader.PayloadLength > 125 || !messageHeader.Fin)
			{
				flag3 = true;
			}
			break;
		default:
			flag3 = true;
			break;
		}
		resultHeader = messageHeader;
		return !flag3;
	}

	private async Task CloseAsyncPrivate(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
	{
		if (!_sentCloseFrame)
		{
			await SendCloseFrameAsync(closeStatus, statusDescription, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		byte[] closeBuffer = ArrayPool<byte>.Shared.Rent(139);
		try
		{
			while (!_receivedCloseFrame)
			{
				Task task;
				lock (ReceiveAsyncLock)
				{
					if (_receivedCloseFrame)
					{
						break;
					}
					task = _lastReceiveAsync;
					task = (_lastReceiveAsync = ValidateAndReceiveAsync(task, closeBuffer, cancellationToken));
					goto IL_0110;
				}
				IL_0110:
				await task.ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(closeBuffer);
		}
		lock (StateUpdateLock)
		{
			DisposeCore();
			if (_state < WebSocketState.Closed)
			{
				_state = WebSocketState.Closed;
			}
		}
	}

	private async Task SendCloseFrameAsync(WebSocketCloseStatus closeStatus, string closeStatusDescription, CancellationToken cancellationToken)
	{
		byte[] buffer = null;
		try
		{
			int num = 2;
			if (string.IsNullOrEmpty(closeStatusDescription))
			{
				buffer = ArrayPool<byte>.Shared.Rent(num);
			}
			else
			{
				num += s_textEncoding.GetByteCount(closeStatusDescription);
				buffer = ArrayPool<byte>.Shared.Rent(num);
				s_textEncoding.GetBytes(closeStatusDescription, 0, closeStatusDescription.Length, buffer, 2);
			}
			ushort num2 = (ushort)closeStatus;
			buffer[0] = (byte)(num2 >> 8);
			buffer[1] = (byte)(num2 & 0xFF);
			await SendFrameAsync(MessageOpcode.Close, endOfMessage: true, new Memory<byte>(buffer, 0, num), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			if (buffer != null)
			{
				ArrayPool<byte>.Shared.Return(buffer);
			}
		}
		lock (StateUpdateLock)
		{
			_sentCloseFrame = true;
			if (_state <= WebSocketState.CloseReceived)
			{
				_state = WebSocketState.CloseSent;
			}
		}
		if (!_isServer && _receivedCloseFrame)
		{
			await WaitForServerToCloseConnectionAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private void ConsumeFromBuffer(int count)
	{
		_receiveBufferCount -= count;
		_receiveBufferOffset += count;
	}

	private async Task EnsureBufferContainsAsync(int minimumRequiredBytes, CancellationToken cancellationToken, bool throwOnPrematureClosure = true)
	{
		if (_receiveBufferCount >= minimumRequiredBytes)
		{
			return;
		}
		if (_receiveBufferCount > 0)
		{
			Span<byte> span = _receiveBuffer.Span;
			span = span.Slice(_receiveBufferOffset, _receiveBufferCount);
			span.CopyTo(_receiveBuffer.Span);
		}
		_receiveBufferOffset = 0;
		while (_receiveBufferCount < minimumRequiredBytes)
		{
			int num = await _stream.ReadAsync(_receiveBuffer.Slice(_receiveBufferCount, _receiveBuffer.Length - _receiveBufferCount), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (num <= 0)
			{
				ThrowIfEOFUnexpected(throwOnPrematureClosure);
				break;
			}
			_receiveBufferCount += num;
		}
	}

	private void ThrowIfEOFUnexpected(bool throwOnPrematureClosure)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("WebSocket");
		}
		if (throwOnPrematureClosure)
		{
			throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely);
		}
	}

	private void AllocateSendBuffer(int minLength)
	{
		_sendBuffer = ArrayPool<byte>.Shared.Rent(minLength);
	}

	private void ReleaseSendBuffer()
	{
		byte[] sendBuffer = _sendBuffer;
		if (sendBuffer != null)
		{
			_sendBuffer = null;
			ArrayPool<byte>.Shared.Return(sendBuffer);
		}
	}

	private static int CombineMaskBytes(Span<byte> buffer, int maskOffset)
	{
		return BitConverter.ToInt32(buffer.Slice(maskOffset));
	}

	private static int ApplyMask(Span<byte> toMask, byte[] mask, int maskOffset, int maskOffsetIndex)
	{
		return ApplyMask(toMask, CombineMaskBytes(mask, maskOffset), maskOffsetIndex);
	}

	private unsafe static int ApplyMask(Span<byte> toMask, int mask, int maskIndex)
	{
		int num = maskIndex * 8;
		int num2 = (mask >>> num) | (mask << 32 - num);
		int num3 = toMask.Length;
		if (num3 > 0)
		{
			fixed (byte* reference = &MemoryMarshal.GetReference(toMask))
			{
				byte* ptr = reference;
				if ((long)ptr % 4L == 0L)
				{
					while (num3 >= 4)
					{
						num3 -= 4;
						*(int*)ptr ^= num2;
						ptr += 4;
					}
				}
				if (num3 > 0)
				{
					byte* ptr2 = (byte*)(&mask);
					byte* ptr3 = ptr + num3;
					while (ptr < ptr3)
					{
						byte* intPtr = ptr++;
						*intPtr ^= ptr2[maskIndex];
						maskIndex = (maskIndex + 1) & 3;
					}
				}
			}
		}
		return maskIndex;
	}

	private void ThrowIfOperationInProgress(bool operationCompleted, [CallerMemberName] string methodName = null)
	{
		if (!operationCompleted)
		{
			Abort();
			ThrowOperationInProgress(methodName);
		}
	}

	private void ThrowOperationInProgress(string methodName)
	{
		throw new InvalidOperationException(global::SR.Format("There is already one outstanding '{0}' call for this WebSocket instance. ReceiveAsync and SendAsync can be called simultaneously, but at most one outstanding operation for each of them is allowed at the same time.", methodName));
	}

	private static Exception CreateOperationCanceledException(Exception innerException, CancellationToken cancellationToken = default(CancellationToken))
	{
		return new OperationCanceledException(new OperationCanceledException().Message, innerException, cancellationToken);
	}

	private static bool TryValidateUtf8(Span<byte> span, bool endOfMessage, Utf8MessageState state)
	{
		int num = 0;
		while (num < span.Length)
		{
			if (!state.SequenceInProgress)
			{
				state.SequenceInProgress = true;
				byte b = span[num];
				num++;
				if ((b & 0x80) == 0)
				{
					state.AdditionalBytesExpected = 0;
					state.CurrentDecodeBits = b & 0x7F;
					state.ExpectedValueMin = 0;
				}
				else
				{
					if ((b & 0xC0) == 128)
					{
						return false;
					}
					if ((b & 0xE0) == 192)
					{
						state.AdditionalBytesExpected = 1;
						state.CurrentDecodeBits = b & 0x1F;
						state.ExpectedValueMin = 128;
					}
					else if ((b & 0xF0) == 224)
					{
						state.AdditionalBytesExpected = 2;
						state.CurrentDecodeBits = b & 0xF;
						state.ExpectedValueMin = 2048;
					}
					else
					{
						if ((b & 0xF8) != 240)
						{
							return false;
						}
						state.AdditionalBytesExpected = 3;
						state.CurrentDecodeBits = b & 7;
						state.ExpectedValueMin = 65536;
					}
				}
			}
			while (state.AdditionalBytesExpected > 0 && num < span.Length)
			{
				byte b2 = span[num];
				if ((b2 & 0xC0) != 128)
				{
					return false;
				}
				num++;
				state.AdditionalBytesExpected--;
				state.CurrentDecodeBits = (state.CurrentDecodeBits << 6) | (b2 & 0x3F);
				if (state.AdditionalBytesExpected == 1 && state.CurrentDecodeBits >= 864 && state.CurrentDecodeBits <= 895)
				{
					return false;
				}
				if (state.AdditionalBytesExpected == 2 && state.CurrentDecodeBits >= 272)
				{
					return false;
				}
			}
			if (state.AdditionalBytesExpected == 0)
			{
				state.SequenceInProgress = false;
				if (state.CurrentDecodeBits < state.ExpectedValueMin)
				{
					return false;
				}
			}
		}
		if (endOfMessage && state.SequenceInProgress)
		{
			return false;
		}
		return true;
	}

	private Task ValidateAndReceiveAsync(Task receiveTask, byte[] buffer, CancellationToken cancellationToken)
	{
		if (receiveTask == null || (receiveTask.Status == TaskStatus.RanToCompletion && (!(receiveTask is Task<WebSocketReceiveResult> task) || task.Result.MessageType != WebSocketMessageType.Close)))
		{
			receiveTask = ReceiveAsyncPrivate<WebSocketReceiveResultGetter, WebSocketReceiveResult>(new ArraySegment<byte>(buffer), cancellationToken).AsTask();
		}
		return receiveTask;
	}
}
