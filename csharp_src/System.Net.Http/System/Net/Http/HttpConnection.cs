using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal class HttpConnection : IDisposable
{
	private sealed class ChunkedEncodingReadStream : HttpContentReadStream
	{
		private enum ParsingState : byte
		{
			ExpectChunkHeader,
			ExpectChunkData,
			ExpectChunkTerminator,
			ConsumeTrailers,
			Done
		}

		private const int MaxChunkBytesAllowed = 16384;

		private ulong _chunkBytesRemaining;

		private ParsingState _state;

		public override bool NeedsDrain => _connection != null;

		public ChunkedEncodingReadStream(HttpConnection connection)
			: base(connection)
		{
		}

		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			if (_connection == null || buffer.Length == 0)
			{
				return new ValueTask<int>(0);
			}
			int num = ReadChunksFromConnectionBuffer(buffer.Span, default(CancellationTokenRegistration));
			if (num > 0)
			{
				return new ValueTask<int>(num);
			}
			if (_connection == null)
			{
				return new ValueTask<int>(0);
			}
			return ReadAsyncCore(buffer, cancellationToken);
		}

		private async ValueTask<int> ReadAsyncCore(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
			try
			{
				int num2;
				do
				{
					if (_connection == null)
					{
						return 0;
					}
					if (_state == ParsingState.ExpectChunkData && buffer.Length >= _connection.ReadBufferSize && _chunkBytesRemaining >= (ulong)_connection.ReadBufferSize)
					{
						int num = await _connection.ReadAsync(buffer.Slice(0, (int)Math.Min((ulong)buffer.Length, _chunkBytesRemaining))).ConfigureAwait(continueOnCapturedContext: false);
						if (num == 0)
						{
							throw new IOException("The server returned an invalid or unrecognized response.");
						}
						_chunkBytesRemaining -= (ulong)num;
						if (_chunkBytesRemaining == 0L)
						{
							_state = ParsingState.ExpectChunkTerminator;
						}
						return num;
					}
					await _connection.FillAsync().ConfigureAwait(continueOnCapturedContext: false);
					num2 = ReadChunksFromConnectionBuffer(buffer.Span, ctr);
				}
				while (num2 <= 0);
				return num2;
			}
			catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			finally
			{
				ctr.Dispose();
			}
		}

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			HttpContentStream.ValidateCopyToArgs(this, destination, bufferSize);
			if (!cancellationToken.IsCancellationRequested)
			{
				if (_connection != null)
				{
					return CopyToAsyncCore(destination, cancellationToken);
				}
				return Task.CompletedTask;
			}
			return Task.FromCanceled(cancellationToken);
		}

		private async Task CopyToAsyncCore(Stream destination, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
			try
			{
				while (true)
				{
					ReadOnlyMemory<byte> buffer = ReadChunkFromConnectionBuffer(int.MaxValue, ctr);
					if (buffer.Length != 0)
					{
						await destination.WriteAsync(buffer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						continue;
					}
					if (_connection == null)
					{
						break;
					}
					await _connection.FillAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			finally
			{
				ctr.Dispose();
			}
		}

		private int ReadChunksFromConnectionBuffer(Span<byte> buffer, CancellationTokenRegistration cancellationRegistration)
		{
			int num = 0;
			while (buffer.Length > 0)
			{
				ReadOnlyMemory<byte> readOnlyMemory = ReadChunkFromConnectionBuffer(buffer.Length, cancellationRegistration);
				if (readOnlyMemory.Length == 0)
				{
					break;
				}
				num += readOnlyMemory.Length;
				readOnlyMemory.Span.CopyTo(buffer);
				buffer = buffer.Slice(readOnlyMemory.Length);
			}
			return num;
		}

		private ReadOnlyMemory<byte> ReadChunkFromConnectionBuffer(int maxBytesToRead, CancellationTokenRegistration cancellationRegistration)
		{
			try
			{
				ReadOnlySpan<byte> line;
				switch (_state)
				{
				case ParsingState.ExpectChunkHeader:
				{
					_connection._allowedReadLineBytes = 16384;
					if (!_connection.TryReadNextLine(out line))
					{
						return default(ReadOnlyMemory<byte>);
					}
					if (!Utf8Parser.TryParse(line, out ulong value, out int bytesConsumed, 'X'))
					{
						throw new IOException("The server returned an invalid or unrecognized response.");
					}
					_chunkBytesRemaining = value;
					if (bytesConsumed != line.Length)
					{
						ValidateChunkExtension(line.Slice(bytesConsumed));
					}
					if (value != 0)
					{
						_state = ParsingState.ExpectChunkData;
						goto case ParsingState.ExpectChunkData;
					}
					_state = ParsingState.ConsumeTrailers;
					_connection._allowedReadLineBytes = _connection.MaxResponseHeadersLength;
					goto case ParsingState.ConsumeTrailers;
				}
				case ParsingState.ExpectChunkData:
				{
					ReadOnlyMemory<byte> remainingBuffer = _connection.RemainingBuffer;
					if (remainingBuffer.Length == 0)
					{
						return default(ReadOnlyMemory<byte>);
					}
					int num = Math.Min(maxBytesToRead, (int)Math.Min((ulong)remainingBuffer.Length, _chunkBytesRemaining));
					_connection.ConsumeFromRemainingBuffer(num);
					_chunkBytesRemaining -= (ulong)num;
					if (_chunkBytesRemaining == 0L)
					{
						_state = ParsingState.ExpectChunkTerminator;
					}
					return remainingBuffer.Slice(0, num);
				}
				case ParsingState.ExpectChunkTerminator:
					_connection._allowedReadLineBytes = 16384;
					if (!_connection.TryReadNextLine(out line))
					{
						return default(ReadOnlyMemory<byte>);
					}
					if (line.Length != 0)
					{
						ThrowInvalidHttpResponse();
					}
					_state = ParsingState.ExpectChunkHeader;
					goto case ParsingState.ExpectChunkHeader;
				case ParsingState.ConsumeTrailers:
					while (_connection.TryReadNextLine(out line))
					{
						if (line.IsEmpty)
						{
							cancellationRegistration.Dispose();
							CancellationHelper.ThrowIfCancellationRequested(cancellationRegistration.Token);
							_state = ParsingState.Done;
							_connection.CompleteResponse();
							_connection = null;
							break;
						}
					}
					return default(ReadOnlyMemory<byte>);
				default:
					if (NetEventSource.IsEnabled)
					{
						NetEventSource.Error(this, $"Unexpected state: {_state}");
					}
					return default(ReadOnlyMemory<byte>);
				}
			}
			catch (Exception)
			{
				_connection.Dispose();
				_connection = null;
				throw;
			}
		}

		private static void ValidateChunkExtension(ReadOnlySpan<byte> lineAfterChunkSize)
		{
			for (int i = 0; i < lineAfterChunkSize.Length; i++)
			{
				switch (lineAfterChunkSize[i])
				{
				case 9:
				case 32:
					continue;
				case 59:
					return;
				}
				throw new IOException("The server returned an invalid or unrecognized response.");
			}
		}

		public override async Task<bool> DrainAsync(int maxDrainBytes)
		{
			CancellationTokenSource cts = null;
			CancellationTokenRegistration ctr = default(CancellationTokenRegistration);
			try
			{
				int drainedBytes = 0;
				while (true)
				{
					drainedBytes += _connection.RemainingBuffer.Length;
					while (ReadChunkFromConnectionBuffer(int.MaxValue, ctr).Length != 0)
					{
					}
					if (_connection == null)
					{
						return true;
					}
					if (drainedBytes >= maxDrainBytes)
					{
						break;
					}
					if (cts == null)
					{
						TimeSpan maxResponseDrainTime = _connection._pool.Settings._maxResponseDrainTime;
						if (maxResponseDrainTime != Timeout.InfiniteTimeSpan)
						{
							cts = new CancellationTokenSource((int)maxResponseDrainTime.TotalMilliseconds);
							ctr = cts.Token.Register(delegate(object s)
							{
								((HttpConnection)s).Dispose();
							}, _connection);
						}
					}
					await _connection.FillAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				return false;
			}
			finally
			{
				ctr.Dispose();
				cts?.Dispose();
			}
		}
	}

	private sealed class ChunkedEncodingWriteStream : HttpContentWriteStream
	{
		private static readonly byte[] s_finalChunkBytes = new byte[5] { 48, 13, 10, 13, 10 };

		public ChunkedEncodingWriteStream(HttpConnection connection)
			: base(connection)
		{
		}

		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken ignored)
		{
			if (buffer.Length != 0)
			{
				return new ValueTask(WriteChunkAsync(buffer));
			}
			return _connection.FlushAsync();
		}

		private async Task WriteChunkAsync(ReadOnlyMemory<byte> buffer)
		{
			await _connection.WriteHexInt32Async(buffer.Length).ConfigureAwait(continueOnCapturedContext: false);
			await _connection.WriteTwoBytesAsync(13, 10).ConfigureAwait(continueOnCapturedContext: false);
			await _connection.WriteAsync(buffer).ConfigureAwait(continueOnCapturedContext: false);
			await _connection.WriteTwoBytesAsync(13, 10).ConfigureAwait(continueOnCapturedContext: false);
		}

		public override async Task FinishAsync()
		{
			await _connection.WriteBytesAsync(s_finalChunkBytes).ConfigureAwait(continueOnCapturedContext: false);
			_connection = null;
		}
	}

	private sealed class ConnectionCloseReadStream : HttpContentReadStream
	{
		public ConnectionCloseReadStream(HttpConnection connection)
			: base(connection)
		{
		}

		public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
			if (_connection == null || buffer.Length == 0)
			{
				return 0;
			}
			ValueTask<int> valueTask = _connection.ReadAsync(buffer);
			int num;
			if (valueTask.IsCompletedSuccessfully)
			{
				num = valueTask.Result;
			}
			else
			{
				CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
				try
				{
					num = await valueTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
				{
					throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
				}
				finally
				{
					ctr.Dispose();
				}
			}
			if (num == 0)
			{
				CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
				_connection.Dispose();
				_connection = null;
				return 0;
			}
			return num;
		}

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			HttpContentStream.ValidateCopyToArgs(this, destination, bufferSize);
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (_connection == null)
			{
				return Task.CompletedTask;
			}
			Task task = _connection.CopyToUntilEofAsync(destination, bufferSize, cancellationToken);
			if (task.IsCompletedSuccessfully)
			{
				Finish();
				return Task.CompletedTask;
			}
			return CompleteCopyToAsync(task, cancellationToken);
		}

		private async Task CompleteCopyToAsync(Task copyTask, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
			try
			{
				await copyTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			finally
			{
				ctr.Dispose();
			}
			CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
			Finish();
		}

		private void Finish()
		{
			_connection.Dispose();
			_connection = null;
		}
	}

	private sealed class ContentLengthReadStream : HttpContentReadStream
	{
		private ulong _contentBytesRemaining;

		public override bool NeedsDrain => _connection != null;

		public ContentLengthReadStream(HttpConnection connection, ulong contentLength)
			: base(connection)
		{
			_contentBytesRemaining = contentLength;
		}

		public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
			if (_connection == null || buffer.Length == 0)
			{
				return 0;
			}
			if ((ulong)buffer.Length > _contentBytesRemaining)
			{
				buffer = buffer.Slice(0, (int)_contentBytesRemaining);
			}
			ValueTask<int> valueTask = _connection.ReadAsync(buffer);
			int num;
			if (valueTask.IsCompletedSuccessfully)
			{
				num = valueTask.Result;
			}
			else
			{
				CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
				try
				{
					num = await valueTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
				{
					throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
				}
				finally
				{
					ctr.Dispose();
				}
			}
			if (num <= 0)
			{
				CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
				throw new IOException("The server returned an invalid or unrecognized response.");
			}
			_contentBytesRemaining -= (ulong)num;
			if (_contentBytesRemaining == 0L)
			{
				_connection.CompleteResponse();
				_connection = null;
			}
			return num;
		}

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			HttpContentStream.ValidateCopyToArgs(this, destination, bufferSize);
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (_connection == null)
			{
				return Task.CompletedTask;
			}
			Task task = _connection.CopyToExactLengthAsync(destination, _contentBytesRemaining, cancellationToken);
			if (task.IsCompletedSuccessfully)
			{
				Finish();
				return Task.CompletedTask;
			}
			return CompleteCopyToAsync(task, cancellationToken);
		}

		private async Task CompleteCopyToAsync(Task copyTask, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
			try
			{
				await copyTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			finally
			{
				ctr.Dispose();
			}
			Finish();
		}

		private void Finish()
		{
			_contentBytesRemaining = 0uL;
			_connection.CompleteResponse();
			_connection = null;
		}

		private ReadOnlyMemory<byte> ReadFromConnectionBuffer(int maxBytesToRead)
		{
			ReadOnlyMemory<byte> remainingBuffer = _connection.RemainingBuffer;
			if (remainingBuffer.Length == 0)
			{
				return default(ReadOnlyMemory<byte>);
			}
			int num = Math.Min(maxBytesToRead, (int)Math.Min((ulong)remainingBuffer.Length, _contentBytesRemaining));
			_connection.ConsumeFromRemainingBuffer(num);
			_contentBytesRemaining -= (ulong)num;
			return remainingBuffer.Slice(0, num);
		}

		public override async Task<bool> DrainAsync(int maxDrainBytes)
		{
			ReadFromConnectionBuffer(int.MaxValue);
			if (_contentBytesRemaining == 0L)
			{
				Finish();
				return true;
			}
			if (_contentBytesRemaining > (ulong)maxDrainBytes)
			{
				return false;
			}
			CancellationTokenSource cts = null;
			CancellationTokenRegistration ctr = default(CancellationTokenRegistration);
			TimeSpan maxResponseDrainTime = _connection._pool.Settings._maxResponseDrainTime;
			if (maxResponseDrainTime != Timeout.InfiniteTimeSpan)
			{
				cts = new CancellationTokenSource((int)maxResponseDrainTime.TotalMilliseconds);
				ctr = cts.Token.Register(delegate(object s)
				{
					((HttpConnection)s).Dispose();
				}, _connection);
			}
			try
			{
				do
				{
					await _connection.FillAsync().ConfigureAwait(continueOnCapturedContext: false);
					ReadFromConnectionBuffer(int.MaxValue);
				}
				while (_contentBytesRemaining != 0L);
				ctr.Dispose();
				CancellationHelper.ThrowIfCancellationRequested(ctr.Token);
				Finish();
				return true;
			}
			finally
			{
				ctr.Dispose();
				cts?.Dispose();
			}
		}
	}

	private sealed class ContentLengthWriteStream : HttpContentWriteStream
	{
		public ContentLengthWriteStream(HttpConnection connection)
			: base(connection)
		{
		}

		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken ignored)
		{
			return new ValueTask(_connection.WriteAsync(buffer));
		}

		public override Task FinishAsync()
		{
			_connection = null;
			return Task.CompletedTask;
		}
	}

	private sealed class EmptyReadStream : HttpContentReadStream
	{
		private static readonly Task<int> s_zeroTask = Task.FromResult(0);

		internal static EmptyReadStream Instance { get; } = new EmptyReadStream();

		private EmptyReadStream()
			: base(null)
		{
		}

		protected override void Dispose(bool disposing)
		{
		}

		public override void Close()
		{
		}

		public override int ReadByte()
		{
			return -1;
		}

		public override int Read(Span<byte> buffer)
		{
			return 0;
		}

		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(0);
			}
			return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
		}
	}

	private sealed class HttpConnectionResponseContent : HttpContent
	{
		private HttpContentStream _stream;

		private bool _consumedStream;

		internal bool IsEmpty => _stream == EmptyReadStream.Instance;

		public void SetStream(HttpContentStream stream)
		{
			_stream = stream;
		}

		private HttpContentStream ConsumeStream()
		{
			if (_consumedStream || _stream == null)
			{
				throw new InvalidOperationException("The stream was already consumed. It cannot be read again.");
			}
			_consumedStream = true;
			return _stream;
		}

		protected sealed override Task SerializeToStreamAsync(Stream stream, TransportContext context)
		{
			return SerializeToStreamAsync(stream, context, CancellationToken.None);
		}

		internal sealed override async Task SerializeToStreamAsync(Stream stream, TransportContext context, CancellationToken cancellationToken)
		{
			using HttpContentStream contentStream = ConsumeStream();
			await contentStream.CopyToAsync(stream, 8192, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}

		protected internal sealed override bool TryComputeLength(out long length)
		{
			length = 0L;
			return false;
		}

		protected sealed override Task<Stream> CreateContentReadStreamAsync()
		{
			return Task.FromResult((Stream)ConsumeStream());
		}

		internal sealed override Stream TryCreateContentReadStream()
		{
			return ConsumeStream();
		}

		protected sealed override void Dispose(bool disposing)
		{
			if (disposing && _stream != null)
			{
				_stream.Dispose();
				_stream = null;
			}
			base.Dispose(disposing);
		}
	}

	internal abstract class HttpContentReadStream : HttpContentStream
	{
		private int _disposed;

		public sealed override bool CanRead => true;

		public sealed override bool CanWrite => false;

		public virtual bool NeedsDrain => false;

		public HttpContentReadStream(HttpConnection connection)
			: base(connection)
		{
		}

		public sealed override void Flush()
		{
		}

		public sealed override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (!cancellationToken.IsCancellationRequested)
			{
				return Task.CompletedTask;
			}
			return Task.FromCanceled(cancellationToken);
		}

		public sealed override void WriteByte(byte value)
		{
			throw new NotSupportedException();
		}

		public sealed override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public sealed override void Write(ReadOnlySpan<byte> source)
		{
			throw new NotSupportedException();
		}

		public sealed override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		public sealed override ValueTask WriteAsync(ReadOnlyMemory<byte> destination, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			HttpContentStream.ValidateBufferArgs(buffer, offset, count);
			return ReadAsync(new Memory<byte>(buffer, offset, count), CancellationToken.None).GetAwaiter().GetResult();
		}

		public sealed override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			HttpContentStream.ValidateBufferArgs(buffer, offset, count);
			return ReadAsync(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
		}

		public sealed override void CopyTo(Stream destination, int bufferSize)
		{
			CopyToAsync(destination, bufferSize, CancellationToken.None).GetAwaiter().GetResult();
		}

		public virtual Task<bool> DrainAsync(int maxDrainBytes)
		{
			return Task.FromResult(result: false);
		}

		protected override void Dispose(bool disposing)
		{
			if (Interlocked.Exchange(ref _disposed, 1) == 0)
			{
				if (disposing && NeedsDrain)
				{
					DrainOnDisposeAsync();
				}
				else
				{
					base.Dispose(disposing);
				}
			}
		}

		private async Task DrainOnDisposeAsync()
		{
			HttpConnection connection = _connection;
			try
			{
				bool flag = await DrainAsync(connection._pool.Settings._maxResponseDrainSize).ConfigureAwait(continueOnCapturedContext: false);
				if (NetEventSource.IsEnabled)
				{
					connection.Trace(flag ? "Connection drain succeeded" : $"Connection drain failed because MaxResponseDrainSize of {connection._pool.Settings._maxResponseDrainSize} bytes was exceeded", "DrainOnDisposeAsync");
				}
			}
			catch (Exception arg)
			{
				if (NetEventSource.IsEnabled)
				{
					connection.Trace($"Connection drain failed due to exception: {arg}", "DrainOnDisposeAsync");
				}
			}
			base.Dispose(disposing: true);
		}
	}

	private abstract class HttpContentWriteStream : HttpContentStream
	{
		public sealed override bool CanRead => false;

		public sealed override bool CanWrite => true;

		public HttpContentWriteStream(HttpConnection connection)
			: base(connection)
		{
		}

		public sealed override void Flush()
		{
			FlushAsync().GetAwaiter().GetResult();
		}

		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public sealed override void Write(byte[] buffer, int offset, int count)
		{
			WriteAsync(buffer, offset, count, CancellationToken.None).GetAwaiter().GetResult();
		}

		public sealed override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken ignored)
		{
			HttpContentStream.ValidateBufferArgs(buffer, offset, count);
			return WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), ignored).AsTask();
		}

		public sealed override Task FlushAsync(CancellationToken ignored)
		{
			return _connection.FlushAsync().AsTask();
		}

		public abstract Task FinishAsync();
	}

	private sealed class RawConnectionStream : HttpContentDuplexStream
	{
		public RawConnectionStream(HttpConnection connection)
			: base(connection)
		{
		}

		public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
			if (_connection == null || buffer.Length == 0)
			{
				return 0;
			}
			ValueTask<int> valueTask = _connection.ReadBufferedAsync(buffer);
			int num;
			if (valueTask.IsCompletedSuccessfully)
			{
				num = valueTask.Result;
			}
			else
			{
				CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
				try
				{
					num = await valueTask.ConfigureAwait(continueOnCapturedContext: false);
				}
				catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
				{
					throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
				}
				finally
				{
					ctr.Dispose();
				}
			}
			if (num == 0)
			{
				CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
				_connection.Dispose();
				_connection = null;
				return 0;
			}
			return num;
		}

		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			HttpContentStream.ValidateCopyToArgs(this, destination, bufferSize);
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (_connection == null)
			{
				return Task.CompletedTask;
			}
			Task task = _connection.CopyToUntilEofAsync(destination, bufferSize, cancellationToken);
			if (task.IsCompletedSuccessfully)
			{
				Finish();
				return Task.CompletedTask;
			}
			return CompleteCopyToAsync(task, cancellationToken);
		}

		private async Task CompleteCopyToAsync(Task copyTask, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
			try
			{
				await copyTask.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			finally
			{
				ctr.Dispose();
			}
			CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
			Finish();
		}

		private void Finish()
		{
			_connection.Dispose();
			_connection = null;
		}

		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask(Task.FromCanceled(cancellationToken));
			}
			if (_connection == null)
			{
				return new ValueTask(Task.FromException(new IOException("The write operation failed, see inner exception.")));
			}
			if (buffer.Length == 0)
			{
				return default(ValueTask);
			}
			ValueTask valueTask = _connection.WriteWithoutBufferingAsync(buffer);
			if (!valueTask.IsCompleted)
			{
				return new ValueTask(WaitWithConnectionCancellationAsync(valueTask, cancellationToken));
			}
			return valueTask;
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled(cancellationToken);
			}
			if (_connection == null)
			{
				return Task.CompletedTask;
			}
			ValueTask task = _connection.FlushAsync();
			if (!task.IsCompleted)
			{
				return WaitWithConnectionCancellationAsync(task, cancellationToken);
			}
			return task.AsTask();
		}

		private async Task WaitWithConnectionCancellationAsync(ValueTask task, CancellationToken cancellationToken)
		{
			CancellationTokenRegistration ctr = _connection.RegisterCancellation(cancellationToken);
			try
			{
				await task.ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (Exception ex) when (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			finally
			{
				ctr.Dispose();
			}
		}
	}

	private const int InitialReadBufferSize = 4096;

	private const int InitialWriteBufferSize = 4096;

	private const int Expect100ErrorSendThreshold = 1024;

	private static readonly byte[] s_contentLength0NewlineAsciiBytes = Encoding.ASCII.GetBytes("Content-Length: 0\r\n");

	private static readonly byte[] s_spaceHttp10NewlineAsciiBytes = Encoding.ASCII.GetBytes(" HTTP/1.0\r\n");

	private static readonly byte[] s_spaceHttp11NewlineAsciiBytes = Encoding.ASCII.GetBytes(" HTTP/1.1\r\n");

	private static readonly byte[] s_httpSchemeAndDelimiter = Encoding.ASCII.GetBytes(Uri.UriSchemeHttp + Uri.SchemeDelimiter);

	private static readonly byte[] s_http1DotBytes = Encoding.ASCII.GetBytes("HTTP/1.");

	private static readonly ulong s_http10Bytes = BitConverter.ToUInt64(Encoding.ASCII.GetBytes("HTTP/1.0"));

	private static readonly ulong s_http11Bytes = BitConverter.ToUInt64(Encoding.ASCII.GetBytes("HTTP/1.1"));

	private readonly HttpConnectionPool _pool;

	private readonly Socket _socket;

	private readonly Stream _stream;

	private readonly TransportContext _transportContext;

	private readonly WeakReference<HttpConnection> _weakThisRef;

	private HttpRequestMessage _currentRequest;

	private readonly byte[] _writeBuffer;

	private int _writeOffset;

	private int _allowedReadLineBytes;

	private ValueTask<int>? _readAheadTask;

	private int _readAheadTaskLock;

	private byte[] _readBuffer;

	private int _readOffset;

	private int _readLength;

	private bool _inUse;

	private bool _canRetry;

	private bool _connectionClose;

	private int _disposed;

	public bool IsNewConnection => !_readAheadTask.HasValue;

	public bool CanRetry => _canRetry;

	public DateTimeOffset CreationTime { get; } = DateTimeOffset.UtcNow;

	public TransportContext TransportContext => _transportContext;

	public HttpConnectionKind Kind => _pool.Kind;

	private int MaxResponseHeadersLength => (int)Math.Min(2147483647L, (long)_pool.Settings._maxResponseHeadersLength * 1024L);

	private int ReadBufferSize => _readBuffer.Length;

	private ReadOnlyMemory<byte> RemainingBuffer => new ReadOnlyMemory<byte>(_readBuffer, _readOffset, _readLength - _readOffset);

	public HttpConnection(HttpConnectionPool pool, Socket socket, Stream stream, TransportContext transportContext)
	{
		_pool = pool;
		_socket = socket;
		_stream = stream;
		_transportContext = transportContext;
		_writeBuffer = new byte[4096];
		_readBuffer = new byte[4096];
		_weakThisRef = new WeakReference<HttpConnection>(this);
		if (NetEventSource.IsEnabled)
		{
			if (pool.IsSecure)
			{
				SslStream sslStream = (SslStream)_stream;
				Trace($"Secure connection created to {pool}. " + $"SslProtocol:{sslStream.SslProtocol}, " + $"CipherAlgorithm:{sslStream.CipherAlgorithm}, CipherStrength:{sslStream.CipherStrength}, " + $"HashAlgorithm:{sslStream.HashAlgorithm}, HashStrength:{sslStream.HashStrength}, " + $"KeyExchangeAlgorithm:{sslStream.KeyExchangeAlgorithm}, KeyExchangeStrength:{sslStream.KeyExchangeStrength}, " + $"LocalCert:{sslStream.LocalCertificate}, RemoteCert:{sslStream.RemoteCertificate}", ".ctor");
			}
			else
			{
				Trace($"Connection created to {pool}.", ".ctor");
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected void Dispose(bool disposing)
	{
		if (Interlocked.Exchange(ref _disposed, 1) != 0)
		{
			return;
		}
		if (NetEventSource.IsEnabled)
		{
			Trace("Connection closing.", "Dispose");
		}
		_pool.DecrementConnectionCount();
		if (disposing)
		{
			GC.SuppressFinalize(this);
			_stream.Dispose();
			ValueTask<int>? valueTask = ConsumeReadAheadTask();
			if (valueTask.HasValue)
			{
				IgnoreExceptionsAsync(valueTask.GetValueOrDefault());
			}
		}
	}

	private static async Task IgnoreExceptionsAsync(ValueTask<int> task)
	{
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
		}
	}

	private async Task LogExceptionsAsync(Task task)
	{
		try
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception arg)
		{
			if (NetEventSource.IsEnabled)
			{
				Trace($"Exception from asynchronous processing: {arg}", "LogExceptionsAsync");
			}
		}
	}

	public bool PollRead()
	{
		if (_socket != null)
		{
			try
			{
				return _socket.Poll(0, SelectMode.SelectRead);
			}
			catch (Exception ex) when (ex is SocketException || ex is ObjectDisposedException)
			{
				return true;
			}
		}
		return EnsureReadAheadAndPollRead();
	}

	public bool EnsureReadAheadAndPollRead()
	{
		try
		{
			if (!_readAheadTask.HasValue)
			{
				_readAheadTask = _stream.ReadAsync(new Memory<byte>(_readBuffer));
			}
		}
		catch (Exception arg)
		{
			if (NetEventSource.IsEnabled)
			{
				Trace($"Error performing read ahead: {arg}", "EnsureReadAheadAndPollRead");
			}
			Dispose();
			_readAheadTask = new ValueTask<int>(0);
		}
		if (!_readAheadTask.Value.IsCompleted && _socket != null)
		{
			try
			{
				return _socket.Poll(0, SelectMode.SelectRead);
			}
			catch
			{
				return false;
			}
		}
		return _readAheadTask.Value.IsCompleted;
	}

	private ValueTask<int>? ConsumeReadAheadTask()
	{
		if (Interlocked.CompareExchange(ref _readAheadTaskLock, 1, 0) == 0)
		{
			ValueTask<int>? readAheadTask = _readAheadTask;
			_readAheadTask = null;
			Volatile.Write(ref _readAheadTaskLock, 0);
			return readAheadTask;
		}
		return null;
	}

	private void ConsumeFromRemainingBuffer(int bytesToConsume)
	{
		_readOffset += bytesToConsume;
	}

	private async Task WriteHeadersAsync(HttpHeaders headers, string cookiesFromContainer)
	{
		foreach (KeyValuePair<HeaderDescriptor, string[]> header in headers.GetHeaderDescriptorsAndValues())
		{
			if (header.Key.KnownHeader == null)
			{
				await WriteAsciiStringAsync(header.Key.Name).ConfigureAwait(continueOnCapturedContext: false);
				await WriteTwoBytesAsync(58, 32).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await WriteBytesAsync(header.Key.KnownHeader.AsciiBytesWithColonSpace).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (header.Value.Length != 0)
			{
				await WriteStringAsync(header.Value[0]).ConfigureAwait(continueOnCapturedContext: false);
				if (cookiesFromContainer != null && header.Key.KnownHeader == KnownHeaders.Cookie)
				{
					await WriteTwoBytesAsync(59, 32).ConfigureAwait(continueOnCapturedContext: false);
					await WriteStringAsync(cookiesFromContainer).ConfigureAwait(continueOnCapturedContext: false);
					cookiesFromContainer = null;
				}
				if (header.Value.Length > 1)
				{
					HttpHeaderParser parser = header.Key.Parser;
					string separator = ", ";
					if (parser != null && parser.SupportsMultipleValues)
					{
						separator = parser.Separator;
					}
					for (int i = 1; i < header.Value.Length; i++)
					{
						await WriteAsciiStringAsync(separator).ConfigureAwait(continueOnCapturedContext: false);
						await WriteStringAsync(header.Value[i]).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
			}
			await WriteTwoBytesAsync(13, 10).ConfigureAwait(continueOnCapturedContext: false);
		}
		if (cookiesFromContainer != null)
		{
			await WriteAsciiStringAsync("Cookie").ConfigureAwait(continueOnCapturedContext: false);
			await WriteTwoBytesAsync(58, 32).ConfigureAwait(continueOnCapturedContext: false);
			await WriteStringAsync(cookiesFromContainer).ConfigureAwait(continueOnCapturedContext: false);
			await WriteTwoBytesAsync(13, 10).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task WriteHostHeaderAsync(Uri uri)
	{
		await WriteBytesAsync(KnownHeaders.Host.AsciiBytesWithColonSpace).ConfigureAwait(continueOnCapturedContext: false);
		if (_pool.HostHeaderValueBytes != null)
		{
			await WriteBytesAsync(_pool.HostHeaderValueBytes).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			if (uri.HostNameType != UriHostNameType.IPv6)
			{
				await WriteAsciiStringAsync(uri.IdnHost).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await WriteByteAsync(91).ConfigureAwait(continueOnCapturedContext: false);
				await WriteAsciiStringAsync(uri.IdnHost).ConfigureAwait(continueOnCapturedContext: false);
				await WriteByteAsync(93).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (!uri.IsDefaultPort)
			{
				await WriteByteAsync(58).ConfigureAwait(continueOnCapturedContext: false);
				await WriteDecimalInt32Async(uri.Port).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		await WriteTwoBytesAsync(13, 10).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task WriteDecimalInt32Async(int value)
	{
		if (Utf8Formatter.TryFormat(value, new Span<byte>(_writeBuffer, _writeOffset, _writeBuffer.Length - _writeOffset), out var bytesWritten))
		{
			_writeOffset += bytesWritten;
			return Task.CompletedTask;
		}
		return WriteAsciiStringAsync(value.ToString());
	}

	private Task WriteHexInt32Async(int value)
	{
		if (Utf8Formatter.TryFormat(value, new Span<byte>(_writeBuffer, _writeOffset, _writeBuffer.Length - _writeOffset), out var bytesWritten, 'X'))
		{
			_writeOffset += bytesWritten;
			return Task.CompletedTask;
		}
		return WriteAsciiStringAsync(value.ToString("X", CultureInfo.InvariantCulture));
	}

	public async Task<HttpResponseMessage> SendAsyncCore(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		TaskCompletionSource<bool> allowExpect100ToContinue = null;
		Task sendRequestContentTask = null;
		_currentRequest = request;
		HttpMethod normalizedMethod = HttpMethod.Normalize(request.Method);
		bool hasExpectContinueHeader = request.HasHeaders && request.Headers.ExpectContinue == true;
		_canRetry = true;
		if (NetEventSource.IsEnabled)
		{
			Trace($"Sending request: {request}", "SendAsyncCore");
		}
		CancellationTokenRegistration cancellationRegistration = RegisterCancellation(cancellationToken);
		try
		{
			await WriteStringAsync(normalizedMethod.Method).ConfigureAwait(continueOnCapturedContext: false);
			await WriteByteAsync(32).ConfigureAwait(continueOnCapturedContext: false);
			if ((object)normalizedMethod == HttpMethod.Connect)
			{
				if (!request.HasHeaders || request.Headers.Host == null)
				{
					throw new HttpRequestException("CONNECT request must contain Host header.");
				}
				await WriteAsciiStringAsync(request.Headers.Host).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				if (Kind == HttpConnectionKind.Proxy)
				{
					await WriteBytesAsync(s_httpSchemeAndDelimiter).ConfigureAwait(continueOnCapturedContext: false);
					if (request.RequestUri.HostNameType != UriHostNameType.IPv6)
					{
						await WriteAsciiStringAsync(request.RequestUri.IdnHost).ConfigureAwait(continueOnCapturedContext: false);
					}
					else
					{
						await WriteByteAsync(91).ConfigureAwait(continueOnCapturedContext: false);
						await WriteAsciiStringAsync(request.RequestUri.IdnHost).ConfigureAwait(continueOnCapturedContext: false);
						await WriteByteAsync(93).ConfigureAwait(continueOnCapturedContext: false);
					}
					if (!request.RequestUri.IsDefaultPort)
					{
						await WriteByteAsync(58).ConfigureAwait(continueOnCapturedContext: false);
						await WriteDecimalInt32Async(request.RequestUri.Port).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				await WriteStringAsync(request.RequestUri.GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment, UriFormat.UriEscaped)).ConfigureAwait(continueOnCapturedContext: false);
			}
			bool flag = request.Version.Minor == 0 && request.Version.Major == 1;
			await WriteBytesAsync(flag ? s_spaceHttp10NewlineAsciiBytes : s_spaceHttp11NewlineAsciiBytes).ConfigureAwait(continueOnCapturedContext: false);
			string text = null;
			if (_pool.Settings._useCookies)
			{
				text = _pool.Settings._cookieContainer.GetCookieHeader(request.RequestUri);
				if (text == "")
				{
					text = null;
				}
			}
			if (request.HasHeaders || text != null)
			{
				await WriteHeadersAsync(request.Headers, text).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (request.Content != null)
			{
				await WriteHeadersAsync(request.Content.Headers, null).ConfigureAwait(continueOnCapturedContext: false);
			}
			else if ((object)normalizedMethod != HttpMethod.Get && (object)normalizedMethod != HttpMethod.Head && (object)normalizedMethod != HttpMethod.Connect)
			{
				await WriteBytesAsync(s_contentLength0NewlineAsciiBytes).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (!request.HasHeaders || request.Headers.Host == null)
			{
				await WriteHostHeaderAsync(request.RequestUri).ConfigureAwait(continueOnCapturedContext: false);
			}
			await WriteTwoBytesAsync(13, 10).ConfigureAwait(continueOnCapturedContext: false);
			if (request.Content == null)
			{
				await FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
			else if (!hasExpectContinueHeader)
			{
				await SendRequestContentAsync(request, CreateRequestContentStream(request), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				await FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
				allowExpect100ToContinue = new TaskCompletionSource<bool>();
				Timer expect100Timer = new Timer(delegate(object s)
				{
					((TaskCompletionSource<bool>)s).TrySetResult(result: true);
				}, allowExpect100ToContinue, _pool.Settings._expect100ContinueTimeout, Timeout.InfiniteTimeSpan);
				sendRequestContentTask = SendRequestContentWithExpect100ContinueAsync(request, allowExpect100ToContinue.Task, CreateRequestContentStream(request), expect100Timer, cancellationToken);
			}
			_allowedReadLineBytes = MaxResponseHeadersLength;
			ValueTask<int>? valueTask = ConsumeReadAheadTask();
			if (valueTask.HasValue)
			{
				int num = await valueTask.GetValueOrDefault().ConfigureAwait(continueOnCapturedContext: false);
				if (NetEventSource.IsEnabled)
				{
					Trace($"Received {num} bytes.", "SendAsyncCore");
				}
				if (num == 0)
				{
					throw new IOException("The server returned an invalid or unrecognized response.");
				}
				_readOffset = 0;
				_readLength = num;
			}
			_canRetry = false;
			HttpResponseMessage response = new HttpResponseMessage
			{
				RequestMessage = request,
				Content = new HttpConnectionResponseContent()
			};
			ParseStatusLine(await ReadNextResponseHeaderLineAsync().ConfigureAwait(continueOnCapturedContext: false), response);
			if (hasExpectContinueHeader)
			{
				if (response.StatusCode >= HttpStatusCode.MultipleChoices && request.Content != null && (!request.Content.Headers.ContentLength.HasValue || request.Content.Headers.ContentLength.GetValueOrDefault() > 1024))
				{
					allowExpect100ToContinue.TrySetResult(result: false);
					if (!allowExpect100ToContinue.Task.Result)
					{
						_connectionClose = true;
					}
				}
				else
				{
					allowExpect100ToContinue?.TrySetResult(result: true);
					if (response.StatusCode == HttpStatusCode.Continue)
					{
						if (!LineIsEmpty(await ReadNextResponseHeaderLineAsync().ConfigureAwait(continueOnCapturedContext: false)))
						{
							ThrowInvalidHttpResponse();
						}
						ParseStatusLine(await ReadNextResponseHeaderLineAsync().ConfigureAwait(continueOnCapturedContext: false), response);
					}
				}
			}
			if (sendRequestContentTask != null)
			{
				Task task = sendRequestContentTask;
				sendRequestContentTask = null;
				await task.ConfigureAwait(continueOnCapturedContext: false);
			}
			while (true)
			{
				ArraySegment<byte> line = await ReadNextResponseHeaderLineAsync(foldedHeadersAllowed: true).ConfigureAwait(continueOnCapturedContext: false);
				if (LineIsEmpty(line))
				{
					break;
				}
				ParseHeaderNameValue(line, response);
			}
			if (response.Headers.ConnectionClose == true)
			{
				_connectionClose = true;
			}
			cancellationRegistration.Dispose();
			CancellationHelper.ThrowIfCancellationRequested(cancellationToken);
			HttpContentStream stream;
			if ((object)normalizedMethod == HttpMethod.Head || response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotModified)
			{
				stream = EmptyReadStream.Instance;
				CompleteResponse();
			}
			else if ((object)normalizedMethod == HttpMethod.Connect && response.StatusCode == HttpStatusCode.OK)
			{
				stream = new RawConnectionStream(this);
				_connectionClose = true;
			}
			else if (response.StatusCode == HttpStatusCode.SwitchingProtocols)
			{
				stream = new RawConnectionStream(this);
			}
			else if (!response.Content.Headers.ContentLength.HasValue)
			{
				stream = ((response.Headers.TransferEncodingChunked != true) ? ((HttpContentReadStream)new ConnectionCloseReadStream(this)) : ((HttpContentReadStream)new ChunkedEncodingReadStream(this)));
			}
			else
			{
				long valueOrDefault = response.Content.Headers.ContentLength.GetValueOrDefault();
				if (valueOrDefault <= 0)
				{
					stream = EmptyReadStream.Instance;
					CompleteResponse();
				}
				else
				{
					stream = new ContentLengthReadStream(this, (ulong)valueOrDefault);
				}
			}
			((HttpConnectionResponseContent)response.Content).SetStream(stream);
			if (NetEventSource.IsEnabled)
			{
				Trace($"Received response: {response}", "SendAsyncCore");
			}
			if (_pool.Settings._useCookies)
			{
				CookieHelper.ProcessReceivedCookies(response, _pool.Settings._cookieContainer);
			}
			return response;
		}
		catch (Exception ex)
		{
			cancellationRegistration.Dispose();
			allowExpect100ToContinue?.TrySetResult(result: false);
			if (NetEventSource.IsEnabled)
			{
				Trace($"Error sending request: {ex}", "SendAsyncCore");
			}
			if (sendRequestContentTask != null && !sendRequestContentTask.IsCompletedSuccessfully)
			{
				LogExceptionsAsync(sendRequestContentTask);
			}
			Dispose();
			if (CancellationHelper.ShouldWrapInOperationCanceledException(ex, cancellationToken))
			{
				throw CancellationHelper.CreateOperationCanceledException(ex, cancellationToken);
			}
			if (ex is InvalidOperationException || ex is IOException)
			{
				throw new HttpRequestException("An error occurred while sending the request.", ex);
			}
			throw;
		}
	}

	public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return SendAsyncCore(request, cancellationToken);
	}

	private HttpContentWriteStream CreateRequestContentStream(HttpRequestMessage request)
	{
		if (!request.HasHeaders || request.Headers.TransferEncodingChunked != true)
		{
			return new ContentLengthWriteStream(this);
		}
		return new ChunkedEncodingWriteStream(this);
	}

	private CancellationTokenRegistration RegisterCancellation(CancellationToken cancellationToken)
	{
		return cancellationToken.Register(delegate(object s)
		{
			if (((WeakReference<HttpConnection>)s).TryGetTarget(out var target))
			{
				if (NetEventSource.IsEnabled)
				{
					target.Trace("Cancellation requested. Disposing of the connection.", "RegisterCancellation");
				}
				target.Dispose();
			}
		}, _weakThisRef);
	}

	private static bool LineIsEmpty(ArraySegment<byte> line)
	{
		return line.Count == 0;
	}

	private async Task SendRequestContentAsync(HttpRequestMessage request, HttpContentWriteStream stream, CancellationToken cancellationToken)
	{
		_canRetry = false;
		await request.Content.CopyToAsync(stream, _transportContext, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		await stream.FinishAsync().ConfigureAwait(continueOnCapturedContext: false);
		await FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task SendRequestContentWithExpect100ContinueAsync(HttpRequestMessage request, Task<bool> allowExpect100ToContinueTask, HttpContentWriteStream stream, Timer expect100Timer, CancellationToken cancellationToken)
	{
		bool num = await allowExpect100ToContinueTask.ConfigureAwait(continueOnCapturedContext: false);
		expect100Timer.Dispose();
		if (num)
		{
			if (NetEventSource.IsEnabled)
			{
				Trace("Sending request content for Expect: 100-continue.", "SendRequestContentWithExpect100ContinueAsync");
			}
			await SendRequestContentAsync(request, stream, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		else if (NetEventSource.IsEnabled)
		{
			Trace("Canceling request content for Expect: 100-continue.", "SendRequestContentWithExpect100ContinueAsync");
		}
	}

	private static void ParseStatusLine(ArraySegment<byte> line, HttpResponseMessage response)
	{
		ParseStatusLine((Span<byte>)line, response);
	}

	private static void ParseStatusLine(Span<byte> line, HttpResponseMessage response)
	{
		if (line.Length < 12 || line[8] != 32)
		{
			ThrowInvalidHttpResponse();
		}
		ulong num = BitConverter.ToUInt64(line);
		if (num == s_http11Bytes)
		{
			response.SetVersionWithoutValidation(HttpVersion.Version11);
		}
		else if (num == s_http10Bytes)
		{
			response.SetVersionWithoutValidation(HttpVersion.Version10);
		}
		else
		{
			byte b = line[7];
			if (IsDigit(b) && line.Slice(0, 7).SequenceEqual(s_http1DotBytes))
			{
				response.SetVersionWithoutValidation(new Version(1, b - 48));
			}
			else
			{
				ThrowInvalidHttpResponse();
			}
		}
		byte b2 = line[9];
		byte b3 = line[10];
		byte b4 = line[11];
		if (!IsDigit(b2) || !IsDigit(b3) || !IsDigit(b4))
		{
			ThrowInvalidHttpResponse();
		}
		response.SetStatusCodeWithoutValidation((HttpStatusCode)(100 * (b2 - 48) + 10 * (b3 - 48) + (b4 - 48)));
		if (line.Length == 12)
		{
			response.SetReasonPhraseWithoutValidation(string.Empty);
		}
		else if (line[12] == 32)
		{
			Span<byte> span = line.Slice(13);
			string text = HttpStatusDescription.Get(response.StatusCode);
			if (text == null || !EqualsOrdinal(text, span))
			{
				try
				{
					response.ReasonPhrase = HttpRuleParser.DefaultHttpEncoding.GetString(span);
					return;
				}
				catch (FormatException innerException)
				{
					ThrowInvalidHttpResponse(innerException);
					return;
				}
			}
			response.SetReasonPhraseWithoutValidation(text);
		}
		else
		{
			ThrowInvalidHttpResponse();
		}
	}

	private static void ParseHeaderNameValue(ArraySegment<byte> line, HttpResponseMessage response)
	{
		ParseHeaderNameValue((Span<byte>)line, response);
	}

	private static void ParseHeaderNameValue(Span<byte> line, HttpResponseMessage response)
	{
		int i = 0;
		while (line[i] != 58 && line[i] != 32)
		{
			i++;
			if (i == line.Length)
			{
				ThrowInvalidHttpResponse();
			}
		}
		if (i == 0)
		{
			ThrowInvalidHttpResponse();
		}
		if (!HeaderDescriptor.TryGet(line.Slice(0, i), out var descriptor))
		{
			ThrowInvalidHttpResponse();
		}
		while (line[i] == 32)
		{
			i++;
			if (i == line.Length)
			{
				ThrowInvalidHttpResponse();
			}
		}
		if (line[i++] != 58)
		{
			ThrowInvalidHttpResponse();
		}
		for (; i < line.Length && (line[i] == 32 || line[i] == 9); i++)
		{
		}
		string headerValue = descriptor.GetHeaderValue(line.Slice(i));
		if (descriptor.HeaderType == HttpHeaderType.Content)
		{
			response.Content.Headers.TryAddWithoutValidation(descriptor, headerValue);
		}
		else
		{
			response.Headers.TryAddWithoutValidation((descriptor.HeaderType == HttpHeaderType.Request) ? descriptor.AsCustomHeader() : descriptor, headerValue);
		}
	}

	private static bool IsDigit(byte c)
	{
		return (uint)(c - 48) <= 9u;
	}

	private void WriteToBuffer(ReadOnlyMemory<byte> source)
	{
		source.Span.CopyTo(new Span<byte>(_writeBuffer, _writeOffset, source.Length));
		_writeOffset += source.Length;
	}

	private async Task WriteAsync(ReadOnlyMemory<byte> source)
	{
		int num = _writeBuffer.Length - _writeOffset;
		if (source.Length <= num)
		{
			WriteToBuffer(source);
			return;
		}
		if (_writeOffset != 0)
		{
			WriteToBuffer(source.Slice(0, num));
			source = source.Slice(num);
			await FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		if (source.Length >= _writeBuffer.Length)
		{
			await WriteToStreamAsync(source).ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			WriteToBuffer(source);
		}
	}

	private ValueTask WriteWithoutBufferingAsync(ReadOnlyMemory<byte> source)
	{
		if (_writeOffset == 0)
		{
			return WriteToStreamAsync(source);
		}
		int num = _writeBuffer.Length - _writeOffset;
		if (source.Length <= num)
		{
			WriteToBuffer(source);
			return FlushAsync();
		}
		return new ValueTask(FlushThenWriteWithoutBufferingAsync(source));
	}

	private async Task FlushThenWriteWithoutBufferingAsync(ReadOnlyMemory<byte> source)
	{
		await FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		await WriteToStreamAsync(source).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task WriteByteAsync(byte b)
	{
		if (_writeOffset < _writeBuffer.Length)
		{
			_writeBuffer[_writeOffset++] = b;
			return Task.CompletedTask;
		}
		return WriteByteSlowAsync(b);
	}

	private async Task WriteByteSlowAsync(byte b)
	{
		await WriteToStreamAsync(_writeBuffer).ConfigureAwait(continueOnCapturedContext: false);
		_writeBuffer[0] = b;
		_writeOffset = 1;
	}

	private Task WriteTwoBytesAsync(byte b1, byte b2)
	{
		if (_writeOffset <= _writeBuffer.Length - 2)
		{
			byte[] writeBuffer = _writeBuffer;
			writeBuffer[_writeOffset++] = b1;
			writeBuffer[_writeOffset++] = b2;
			return Task.CompletedTask;
		}
		return WriteTwoBytesSlowAsync(b1, b2);
	}

	private async Task WriteTwoBytesSlowAsync(byte b1, byte b2)
	{
		await WriteByteAsync(b1).ConfigureAwait(continueOnCapturedContext: false);
		await WriteByteAsync(b2).ConfigureAwait(continueOnCapturedContext: false);
	}

	private Task WriteBytesAsync(byte[] bytes)
	{
		if (_writeOffset <= _writeBuffer.Length - bytes.Length)
		{
			Buffer.BlockCopy(bytes, 0, _writeBuffer, _writeOffset, bytes.Length);
			_writeOffset += bytes.Length;
			return Task.CompletedTask;
		}
		return WriteBytesSlowAsync(bytes);
	}

	private async Task WriteBytesSlowAsync(byte[] bytes)
	{
		int offset = 0;
		while (true)
		{
			int num = Math.Min(bytes.Length - offset, _writeBuffer.Length - _writeOffset);
			Buffer.BlockCopy(bytes, offset, _writeBuffer, _writeOffset, num);
			_writeOffset += num;
			offset += num;
			if (offset != bytes.Length)
			{
				if (_writeOffset == _writeBuffer.Length)
				{
					await WriteToStreamAsync(_writeBuffer).ConfigureAwait(continueOnCapturedContext: false);
					_writeOffset = 0;
				}
				continue;
			}
			break;
		}
	}

	private Task WriteStringAsync(string s)
	{
		int writeOffset = _writeOffset;
		if (s.Length <= _writeBuffer.Length - writeOffset)
		{
			byte[] writeBuffer = _writeBuffer;
			foreach (char c in s)
			{
				if ((c & 0xFF80) != 0)
				{
					throw new HttpRequestException("Request headers must contain only ASCII characters.");
				}
				writeBuffer[writeOffset++] = (byte)c;
			}
			_writeOffset = writeOffset;
			return Task.CompletedTask;
		}
		return WriteStringAsyncSlow(s);
	}

	private Task WriteAsciiStringAsync(string s)
	{
		int writeOffset = _writeOffset;
		if (s.Length <= _writeBuffer.Length - writeOffset)
		{
			byte[] writeBuffer = _writeBuffer;
			foreach (char c in s)
			{
				writeBuffer[writeOffset++] = (byte)c;
			}
			_writeOffset = writeOffset;
			return Task.CompletedTask;
		}
		return WriteStringAsyncSlow(s);
	}

	private async Task WriteStringAsyncSlow(string s)
	{
		foreach (char c in s)
		{
			if ((c & 0xFF80) != 0)
			{
				throw new HttpRequestException("Request headers must contain only ASCII characters.");
			}
			await WriteByteAsync((byte)c).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private ValueTask FlushAsync()
	{
		if (_writeOffset > 0)
		{
			ValueTask result = WriteToStreamAsync(new ReadOnlyMemory<byte>(_writeBuffer, 0, _writeOffset));
			_writeOffset = 0;
			return result;
		}
		return default(ValueTask);
	}

	private ValueTask WriteToStreamAsync(ReadOnlyMemory<byte> source)
	{
		if (NetEventSource.IsEnabled)
		{
			Trace($"Writing {source.Length} bytes.", "WriteToStreamAsync");
		}
		return _stream.WriteAsync(source);
	}

	private bool TryReadNextLine(out ReadOnlySpan<byte> line)
	{
		ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(_readBuffer, _readOffset, _readLength - _readOffset);
		int num = span.IndexOf<byte>(10);
		if (num < 0)
		{
			if (_allowedReadLineBytes < span.Length)
			{
				ThrowInvalidHttpResponse();
			}
			line = default(ReadOnlySpan<byte>);
			return false;
		}
		int num2 = num + 1;
		_readOffset += num2;
		_allowedReadLineBytes -= num2;
		ThrowIfExceededAllowedReadLineBytes();
		line = span.Slice(0, (num > 0 && span[num - 1] == 13) ? (num - 1) : num);
		return true;
	}

	private async ValueTask<ArraySegment<byte>> ReadNextResponseHeaderLineAsync(bool foldedHeadersAllowed = false)
	{
		int previouslyScannedBytes = 0;
		int num;
		int num2;
		int readOffset;
		int num3;
		while (true)
		{
			num = _readOffset + previouslyScannedBytes;
			num2 = Array.IndexOf(_readBuffer, (byte)10, num, _readLength - num);
			if (num2 >= 0)
			{
				readOffset = _readOffset;
				num3 = num2 - readOffset;
				if (num2 > 0 && _readBuffer[num2 - 1] == 13)
				{
					num3--;
				}
				if (!foldedHeadersAllowed || num3 <= 0)
				{
					break;
				}
				if (num2 + 1 == _readLength)
				{
					int num4 = ((_readBuffer[num2 - 1] == 13) ? (num2 - 2) : (num2 - 1));
					previouslyScannedBytes = num4 - _readOffset;
					_allowedReadLineBytes -= num4 - num;
					ThrowIfExceededAllowedReadLineBytes();
					await FillAsync().ConfigureAwait(continueOnCapturedContext: false);
					continue;
				}
				char c = (char)_readBuffer[num2 + 1];
				if (c != ' ' && c != '\t')
				{
					break;
				}
				if (Array.IndexOf(_readBuffer, (byte)58, _readOffset, num2 - _readOffset) == -1)
				{
					ThrowInvalidHttpResponse();
				}
				_readBuffer[num2] = 32;
				if (_readBuffer[num2 - 1] == 13)
				{
					_readBuffer[num2 - 1] = 32;
				}
				previouslyScannedBytes = num2 + 1 - _readOffset;
				_allowedReadLineBytes -= num2 + 1 - num;
				ThrowIfExceededAllowedReadLineBytes();
			}
			else
			{
				previouslyScannedBytes = _readLength - _readOffset;
				_allowedReadLineBytes -= _readLength - num;
				ThrowIfExceededAllowedReadLineBytes();
				await FillAsync().ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		_allowedReadLineBytes -= num2 + 1 - num;
		ThrowIfExceededAllowedReadLineBytes();
		_readOffset = num2 + 1;
		return new ArraySegment<byte>(_readBuffer, readOffset, num3);
	}

	private void ThrowIfExceededAllowedReadLineBytes()
	{
		if (_allowedReadLineBytes < 0)
		{
			ThrowInvalidHttpResponse();
		}
	}

	private async Task FillAsync()
	{
		int num = _readLength - _readOffset;
		if (num == 0)
		{
			_readOffset = (_readLength = 0);
		}
		else if (_readOffset > 0)
		{
			Buffer.BlockCopy(_readBuffer, _readOffset, _readBuffer, 0, num);
			_readOffset = 0;
			_readLength = num;
		}
		else if (num == _readBuffer.Length)
		{
			byte[] array = new byte[_readBuffer.Length * 2];
			Buffer.BlockCopy(_readBuffer, 0, array, 0, num);
			_readBuffer = array;
			_readOffset = 0;
			_readLength = num;
		}
		int num2 = await _stream.ReadAsync(new Memory<byte>(_readBuffer, _readLength, _readBuffer.Length - _readLength)).ConfigureAwait(continueOnCapturedContext: false);
		if (NetEventSource.IsEnabled)
		{
			Trace($"Received {num2} bytes.", "FillAsync");
		}
		if (num2 == 0)
		{
			throw new IOException("The server returned an invalid or unrecognized response.");
		}
		_readLength += num2;
	}

	private void ReadFromBuffer(Span<byte> buffer)
	{
		new Span<byte>(_readBuffer, _readOffset, buffer.Length).CopyTo(buffer);
		_readOffset += buffer.Length;
	}

	private async ValueTask<int> ReadAsync(Memory<byte> destination)
	{
		int num = _readLength - _readOffset;
		if (num > 0)
		{
			if (destination.Length <= num)
			{
				ReadFromBuffer(destination.Span);
				return destination.Length;
			}
			ReadFromBuffer(destination.Span.Slice(0, num));
			return num;
		}
		int num2 = await _stream.ReadAsync(destination).ConfigureAwait(continueOnCapturedContext: false);
		if (NetEventSource.IsEnabled)
		{
			Trace($"Received {num2} bytes.", "ReadAsync");
		}
		return num2;
	}

	private ValueTask<int> ReadBufferedAsync(Memory<byte> destination)
	{
		if (destination.Length < _readBuffer.Length)
		{
			return ReadBufferedAsyncCore(destination);
		}
		return ReadAsync(destination);
	}

	private async ValueTask<int> ReadBufferedAsyncCore(Memory<byte> destination)
	{
		int num = _readLength - _readOffset;
		if (num > 0)
		{
			if (destination.Length <= num)
			{
				ReadFromBuffer(destination.Span);
				return destination.Length;
			}
			ReadFromBuffer(destination.Span.Slice(0, num));
			return num;
		}
		_readOffset = (_readLength = 0);
		int num2 = await _stream.ReadAsync(_readBuffer.AsMemory()).ConfigureAwait(continueOnCapturedContext: false);
		if (NetEventSource.IsEnabled)
		{
			Trace($"Received {num2} bytes.", "ReadBufferedAsyncCore");
		}
		_readLength = num2;
		int num3 = Math.Min(num2, destination.Length);
		_readBuffer.AsSpan(0, num3).CopyTo(destination.Span);
		_readOffset = num3;
		return num3;
	}

	private async Task CopyFromBufferAsync(Stream destination, int count, CancellationToken cancellationToken)
	{
		if (NetEventSource.IsEnabled)
		{
			Trace($"Copying {count} bytes to stream.", "CopyFromBufferAsync");
		}
		await destination.WriteAsync(new ReadOnlyMemory<byte>(_readBuffer, _readOffset, count), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_readOffset += count;
	}

	private Task CopyToUntilEofAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		if (_readLength - _readOffset <= 0)
		{
			return _stream.CopyToAsync(destination, bufferSize, cancellationToken);
		}
		return CopyToUntilEofWithExistingBufferedDataAsync(destination, cancellationToken);
	}

	private async Task CopyToUntilEofWithExistingBufferedDataAsync(Stream destination, CancellationToken cancellationToken)
	{
		int count = _readLength - _readOffset;
		await CopyFromBufferAsync(destination, count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_readLength = (_readOffset = 0);
		await _stream.CopyToAsync(destination).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task CopyToExactLengthAsync(Stream destination, ulong length, CancellationToken cancellationToken)
	{
		int remaining = _readLength - _readOffset;
		if (remaining > 0)
		{
			if ((ulong)remaining > length)
			{
				remaining = (int)length;
			}
			await CopyFromBufferAsync(destination, remaining, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			length -= (ulong)remaining;
			if (length == 0L)
			{
				return;
			}
		}
		do
		{
			await FillAsync().ConfigureAwait(continueOnCapturedContext: false);
			remaining = (((ulong)_readLength < length) ? _readLength : ((int)length));
			await CopyFromBufferAsync(destination, remaining, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			length -= (ulong)remaining;
		}
		while (length != 0L);
	}

	public void Acquire()
	{
		_inUse = true;
	}

	public void Release()
	{
		_inUse = false;
		if (_currentRequest == null)
		{
			ReturnConnectionToPool();
		}
	}

	private void CompleteResponse()
	{
		_currentRequest = null;
		if (RemainingBuffer.Length != 0)
		{
			if (NetEventSource.IsEnabled)
			{
				Trace("Unexpected data on connection after response read.", "CompleteResponse");
			}
			ConsumeFromRemainingBuffer(RemainingBuffer.Length);
			_connectionClose = true;
		}
		if (!_inUse)
		{
			ReturnConnectionToPool();
		}
	}

	public async Task DrainResponseAsync(HttpResponseMessage response)
	{
		if (_connectionClose)
		{
			throw new HttpRequestException("Authentication failed because the connection could not be reused.");
		}
		HttpContentReadStream httpContentReadStream = (HttpContentReadStream)(await response.Content.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false));
		if (httpContentReadStream.NeedsDrain && (!(await httpContentReadStream.DrainAsync(_pool.Settings._maxResponseDrainSize).ConfigureAwait(continueOnCapturedContext: false)) || _connectionClose))
		{
			throw new HttpRequestException("Authentication failed because the connection could not be reused.");
		}
		response.Dispose();
	}

	private void ReturnConnectionToPool()
	{
		if (_connectionClose)
		{
			if (NetEventSource.IsEnabled)
			{
				Trace("Connection will not be reused.", "ReturnConnectionToPool");
			}
			Dispose();
		}
		else
		{
			_pool.ReturnConnection(this);
		}
	}

	private static bool EqualsOrdinal(string left, Span<byte> right)
	{
		if (left.Length != right.Length)
		{
			return false;
		}
		for (int i = 0; i < left.Length; i++)
		{
			if (left[i] != right[i])
			{
				return false;
			}
		}
		return true;
	}

	public sealed override string ToString()
	{
		return string.Format("{0}({1})", "HttpConnection", _pool);
	}

	private static void ThrowInvalidHttpResponse()
	{
		throw new HttpRequestException("The server returned an invalid or unrecognized response.");
	}

	private static void ThrowInvalidHttpResponse(Exception innerException)
	{
		throw new HttpRequestException("The server returned an invalid or unrecognized response.", innerException);
	}

	internal void Trace(string message, [CallerMemberName] string memberName = null)
	{
		NetEventSource.Log.HandlerMessage(_pool?.GetHashCode() ?? 0, GetHashCode(), _currentRequest?.GetHashCode() ?? 0, memberName, ToString() + ": " + message);
	}
}
