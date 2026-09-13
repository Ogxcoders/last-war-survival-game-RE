using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Compression;

public sealed class BrotliStream : Stream
{
	private const int DefaultInternalBufferSize = 65520;

	private Stream _stream;

	private readonly byte[] _buffer;

	private readonly bool _leaveOpen;

	private readonly CompressionMode _mode;

	private int _activeAsyncOperation;

	private BrotliDecoder _decoder;

	private int _bufferOffset;

	private int _bufferCount;

	private BrotliEncoder _encoder;

	public Stream BaseStream => _stream;

	public override bool CanRead
	{
		get
		{
			if (_mode == CompressionMode.Decompress && _stream != null)
			{
				return _stream.CanRead;
			}
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			if (_mode == CompressionMode.Compress && _stream != null)
			{
				return _stream.CanWrite;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override long Position
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

	private bool AsyncOperationIsActive => _activeAsyncOperation != 0;

	public BrotliStream(Stream stream, CompressionMode mode)
		: this(stream, mode, leaveOpen: false)
	{
	}

	public BrotliStream(Stream stream, CompressionMode mode, bool leaveOpen)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		switch (mode)
		{
		case CompressionMode.Compress:
			if (!stream.CanWrite)
			{
				throw new ArgumentException("Stream does not support writing.", "stream");
			}
			break;
		case CompressionMode.Decompress:
			if (!stream.CanRead)
			{
				throw new ArgumentException("Stream does not support reading.", "stream");
			}
			break;
		default:
			throw new ArgumentException("Enum value was out of legal range.", "mode");
		}
		_mode = mode;
		_stream = stream;
		_leaveOpen = leaveOpen;
		_buffer = new byte[65520];
	}

	private void EnsureNotDisposed()
	{
		if (_stream == null)
		{
			throw new ObjectDisposedException("stream", "Can not access a closed Stream.");
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && _stream != null)
			{
				if (_mode == CompressionMode.Compress)
				{
					WriteCore(ReadOnlySpan<byte>.Empty, isFinalBlock: true);
				}
				if (!_leaveOpen)
				{
					_stream.Dispose();
				}
			}
		}
		finally
		{
			_stream = null;
			_encoder.Dispose();
			_decoder.Dispose();
			base.Dispose(disposing);
		}
	}

	private static void ValidateParameters(byte[] array, int offset, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Positive number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Positive number required.");
		}
		if (array.Length - offset < count)
		{
			throw new ArgumentException("Offset plus count is larger than the length of target array.");
		}
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	private void EnsureNoActiveAsyncOperation()
	{
		if (AsyncOperationIsActive)
		{
			ThrowInvalidBeginCall();
		}
	}

	private void AsyncOperationStarting()
	{
		if (Interlocked.CompareExchange(ref _activeAsyncOperation, 1, 0) != 0)
		{
			ThrowInvalidBeginCall();
		}
	}

	private void AsyncOperationCompleting()
	{
		Interlocked.CompareExchange(ref _activeAsyncOperation, 0, 1);
	}

	private static void ThrowInvalidBeginCall()
	{
		throw new InvalidOperationException("Only one asynchronous reader or writer is allowed time at one time.");
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		ValidateParameters(buffer, offset, count);
		return Read(new Span<byte>(buffer, offset, count));
	}

	public override int Read(Span<byte> buffer)
	{
		if (_mode != CompressionMode.Decompress)
		{
			throw new InvalidOperationException("Can not perform Read operations on a BrotliStream constructed with CompressionMode.Compress.");
		}
		EnsureNotDisposed();
		int num = 0;
		OperationStatus operationStatus = OperationStatus.DestinationTooSmall;
		while (buffer.Length > 0 && operationStatus != OperationStatus.Done)
		{
			if (operationStatus == OperationStatus.NeedMoreData)
			{
				if (_bufferCount > 0 && _bufferOffset != 0)
				{
					_buffer.AsSpan(_bufferOffset, _bufferCount).CopyTo(_buffer);
				}
				_bufferOffset = 0;
				int num2 = 0;
				while (_bufferCount < _buffer.Length && (num2 = _stream.Read(_buffer, _bufferCount, _buffer.Length - _bufferCount)) > 0)
				{
					_bufferCount += num2;
					if (_bufferCount > _buffer.Length)
					{
						throw new InvalidDataException("BrotliStream.BaseStream returned more bytes than requested in Read.");
					}
				}
				if (_bufferCount <= 0)
				{
					break;
				}
			}
			operationStatus = _decoder.Decompress(_buffer.AsSpan(_bufferOffset, _bufferCount), buffer, out var bytesConsumed, out var bytesWritten);
			if (operationStatus == OperationStatus.InvalidData)
			{
				throw new InvalidOperationException("Decoder ran into invalid data.");
			}
			if (bytesConsumed > 0)
			{
				_bufferOffset += bytesConsumed;
				_bufferCount -= bytesConsumed;
			}
			if (bytesWritten > 0)
			{
				num += bytesWritten;
				buffer = buffer.Slice(bytesWritten);
			}
		}
		return num;
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
	{
		return System.Threading.Tasks.TaskToApm.Begin(ReadAsync(buffer, offset, count, CancellationToken.None), asyncCallback, asyncState);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return System.Threading.Tasks.TaskToApm.End<int>(asyncResult);
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		ValidateParameters(buffer, offset, count);
		return ReadAsync(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_mode != CompressionMode.Decompress)
		{
			throw new InvalidOperationException("Can not perform Read operations on a BrotliStream constructed with CompressionMode.Compress.");
		}
		EnsureNoActiveAsyncOperation();
		EnsureNotDisposed();
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
		}
		return FinishReadAsyncMemory(buffer, cancellationToken);
	}

	private async ValueTask<int> FinishReadAsyncMemory(Memory<byte> buffer, CancellationToken cancellationToken)
	{
		AsyncOperationStarting();
		try
		{
			int totalWritten = 0;
			_ = Memory<byte>.Empty;
			OperationStatus operationStatus = OperationStatus.DestinationTooSmall;
			while (buffer.Length > 0 && operationStatus != OperationStatus.Done)
			{
				if (operationStatus == OperationStatus.NeedMoreData)
				{
					if (_bufferCount > 0 && _bufferOffset != 0)
					{
						_buffer.AsSpan(_bufferOffset, _bufferCount).CopyTo(_buffer);
					}
					_bufferOffset = 0;
					int num = 0;
					while (true)
					{
						bool flag = _bufferCount < _buffer.Length;
						if (flag)
						{
							flag = (num = await _stream.ReadAsync(new Memory<byte>(_buffer, _bufferCount, _buffer.Length - _bufferCount)).ConfigureAwait(continueOnCapturedContext: false)) > 0;
						}
						if (!flag)
						{
							break;
						}
						_bufferCount += num;
						if (_bufferCount > _buffer.Length)
						{
							throw new InvalidDataException("BrotliStream.BaseStream returned more bytes than requested in Read.");
						}
					}
					if (_bufferCount <= 0)
					{
						break;
					}
				}
				cancellationToken.ThrowIfCancellationRequested();
				operationStatus = _decoder.Decompress(_buffer.AsSpan(_bufferOffset, _bufferCount), buffer.Span, out var bytesConsumed, out var bytesWritten);
				if (operationStatus == OperationStatus.InvalidData)
				{
					throw new InvalidOperationException("Decoder ran into invalid data.");
				}
				if (bytesConsumed > 0)
				{
					_bufferOffset += bytesConsumed;
					_bufferCount -= bytesConsumed;
				}
				if (bytesWritten > 0)
				{
					totalWritten += bytesWritten;
					buffer = buffer.Slice(bytesWritten);
				}
			}
			return totalWritten;
		}
		finally
		{
			AsyncOperationCompleting();
		}
	}

	public BrotliStream(Stream stream, CompressionLevel compressionLevel)
		: this(stream, compressionLevel, leaveOpen: false)
	{
	}

	public BrotliStream(Stream stream, CompressionLevel compressionLevel, bool leaveOpen)
		: this(stream, CompressionMode.Compress, leaveOpen)
	{
		_encoder.SetQuality(BrotliUtils.GetQualityFromCompressionLevel(compressionLevel));
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		ValidateParameters(buffer, offset, count);
		WriteCore(new ReadOnlySpan<byte>(buffer, offset, count));
	}

	public override void Write(ReadOnlySpan<byte> buffer)
	{
		WriteCore(buffer);
	}

	internal void WriteCore(ReadOnlySpan<byte> buffer, bool isFinalBlock = false)
	{
		if (_mode != CompressionMode.Compress)
		{
			throw new InvalidOperationException("Can not perform Write operations on a BrotliStream constructed with CompressionMode.Decompress.");
		}
		EnsureNotDisposed();
		OperationStatus operationStatus = OperationStatus.DestinationTooSmall;
		Span<byte> destination = new Span<byte>(_buffer);
		while (operationStatus == OperationStatus.DestinationTooSmall)
		{
			int bytesConsumed = 0;
			int bytesWritten = 0;
			operationStatus = _encoder.Compress(buffer, destination, out bytesConsumed, out bytesWritten, isFinalBlock);
			if (operationStatus == OperationStatus.InvalidData)
			{
				throw new InvalidOperationException("Encoder ran into invalid data.");
			}
			if (bytesWritten > 0)
			{
				_stream.Write(destination.Slice(0, bytesWritten));
			}
			if (bytesConsumed > 0)
			{
				buffer = buffer.Slice(bytesConsumed);
			}
		}
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback asyncCallback, object asyncState)
	{
		return System.Threading.Tasks.TaskToApm.Begin(WriteAsync(buffer, offset, count, CancellationToken.None), asyncCallback, asyncState);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		System.Threading.Tasks.TaskToApm.End(asyncResult);
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		ValidateParameters(buffer, offset, count);
		return WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (_mode != CompressionMode.Compress)
		{
			throw new InvalidOperationException("Can not perform Write operations on a BrotliStream constructed with CompressionMode.Decompress.");
		}
		EnsureNoActiveAsyncOperation();
		EnsureNotDisposed();
		return new ValueTask(cancellationToken.IsCancellationRequested ? Task.FromCanceled<int>(cancellationToken) : WriteAsyncMemoryCore(buffer, cancellationToken));
	}

	private async Task WriteAsyncMemoryCore(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
	{
		AsyncOperationStarting();
		try
		{
			OperationStatus lastResult = OperationStatus.DestinationTooSmall;
			while (lastResult == OperationStatus.DestinationTooSmall)
			{
				Memory<byte> destination = new Memory<byte>(_buffer);
				int bytesConsumed = 0;
				int bytesWritten = 0;
				lastResult = _encoder.Compress(buffer, destination, out bytesConsumed, out bytesWritten, isFinalBlock: false);
				if (lastResult == OperationStatus.InvalidData)
				{
					throw new InvalidOperationException("Encoder ran into invalid data.");
				}
				if (bytesConsumed > 0)
				{
					buffer = buffer.Slice(bytesConsumed);
				}
				if (bytesWritten > 0)
				{
					await _stream.WriteAsync(new ReadOnlyMemory<byte>(_buffer, 0, bytesWritten), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
		finally
		{
			AsyncOperationCompleting();
		}
	}

	public override void Flush()
	{
		EnsureNotDisposed();
		if (_mode != CompressionMode.Compress || _encoder._state == null || _encoder._state.IsClosed)
		{
			return;
		}
		OperationStatus operationStatus = OperationStatus.DestinationTooSmall;
		Span<byte> destination = new Span<byte>(_buffer);
		while (operationStatus == OperationStatus.DestinationTooSmall)
		{
			int bytesWritten = 0;
			operationStatus = _encoder.Flush(destination, out bytesWritten);
			if (operationStatus == OperationStatus.InvalidData)
			{
				throw new InvalidDataException("Encoder ran into invalid data.");
			}
			if (bytesWritten > 0)
			{
				_stream.Write(destination.Slice(0, bytesWritten));
			}
		}
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		EnsureNoActiveAsyncOperation();
		EnsureNotDisposed();
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		if (_mode == CompressionMode.Compress)
		{
			return FlushAsyncCore(cancellationToken);
		}
		return Task.CompletedTask;
	}

	private async Task FlushAsyncCore(CancellationToken cancellationToken)
	{
		AsyncOperationStarting();
		try
		{
			if (_encoder._state == null || _encoder._state.IsClosed)
			{
				return;
			}
			OperationStatus lastResult = OperationStatus.DestinationTooSmall;
			while (lastResult == OperationStatus.DestinationTooSmall)
			{
				Memory<byte> destination = new Memory<byte>(_buffer);
				int bytesWritten = 0;
				lastResult = _encoder.Flush(destination, out bytesWritten);
				if (lastResult == OperationStatus.InvalidData)
				{
					throw new InvalidDataException("Encoder ran into invalid data.");
				}
				if (bytesWritten > 0)
				{
					await _stream.WriteAsync(destination.Slice(0, bytesWritten), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
		finally
		{
			AsyncOperationCompleting();
		}
	}
}
