using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public sealed class BufferedStream : Stream
{
	private const int MaxShadowBufferSize = 81920;

	private const int DefaultBufferSize = 4096;

	private Stream _stream;

	private byte[] _buffer;

	private readonly int _bufferSize;

	private int _readPos;

	private int _readLen;

	private int _writePos;

	private Task<int> _lastSyncCompletedReadTask;

	private SemaphoreSlim _asyncActiveSemaphore;

	public Stream UnderlyingStream => _stream;

	public int BufferSize => _bufferSize;

	public override bool CanRead
	{
		get
		{
			if (_stream != null)
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
			if (_stream != null)
			{
				return _stream.CanWrite;
			}
			return false;
		}
	}

	public override bool CanSeek
	{
		get
		{
			if (_stream != null)
			{
				return _stream.CanSeek;
			}
			return false;
		}
	}

	public override long Length
	{
		get
		{
			EnsureNotClosed();
			if (_writePos > 0)
			{
				FlushWrite();
			}
			return _stream.Length;
		}
	}

	public override long Position
	{
		get
		{
			EnsureNotClosed();
			EnsureCanSeek();
			return _stream.Position + (_readPos - _readLen + _writePos);
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
			}
			EnsureNotClosed();
			EnsureCanSeek();
			if (_writePos > 0)
			{
				FlushWrite();
			}
			_readPos = 0;
			_readLen = 0;
			_stream.Seek(value, SeekOrigin.Begin);
		}
	}

	internal SemaphoreSlim LazyEnsureAsyncActiveSemaphoreInitialized()
	{
		return LazyInitializer.EnsureInitialized(ref _asyncActiveSemaphore, () => new SemaphoreSlim(1, 1));
	}

	public BufferedStream(Stream stream)
		: this(stream, 4096)
	{
	}

	public BufferedStream(Stream stream, int bufferSize)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (bufferSize <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize", SR.Format("'{0}' must be greater than zero.", "bufferSize"));
		}
		_stream = stream;
		_bufferSize = bufferSize;
		if (!_stream.CanRead && !_stream.CanWrite)
		{
			throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
		}
	}

	private void EnsureNotClosed()
	{
		if (_stream == null)
		{
			throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
		}
	}

	private void EnsureCanSeek()
	{
		if (!_stream.CanSeek)
		{
			throw new NotSupportedException("Stream does not support seeking.");
		}
	}

	private void EnsureCanRead()
	{
		if (!_stream.CanRead)
		{
			throw new NotSupportedException("Stream does not support reading.");
		}
	}

	private void EnsureCanWrite()
	{
		if (!_stream.CanWrite)
		{
			throw new NotSupportedException("Stream does not support writing.");
		}
	}

	private void EnsureShadowBufferAllocated()
	{
		if (_buffer.Length == _bufferSize && _bufferSize < 81920)
		{
			byte[] array = new byte[Math.Min(_bufferSize + _bufferSize, 81920)];
			Buffer.BlockCopy(_buffer, 0, array, 0, _writePos);
			_buffer = array;
		}
	}

	private void EnsureBufferAllocated()
	{
		if (_buffer == null)
		{
			_buffer = new byte[_bufferSize];
		}
	}

	public override async ValueTask DisposeAsync()
	{
		_ = 1;
		try
		{
			if (_stream != null)
			{
				try
				{
					await FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				finally
				{
					await _stream.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
			}
		}
		finally
		{
			_stream = null;
			_buffer = null;
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && _stream != null)
			{
				try
				{
					Flush();
					return;
				}
				finally
				{
					_stream.Dispose();
				}
			}
		}
		finally
		{
			_stream = null;
			_buffer = null;
			base.Dispose(disposing);
		}
	}

	public override void Flush()
	{
		EnsureNotClosed();
		if (_writePos > 0)
		{
			FlushWrite();
		}
		else if (_readPos < _readLen)
		{
			if (_stream.CanSeek)
			{
				FlushRead();
			}
			if (_stream.CanWrite)
			{
				_stream.Flush();
			}
		}
		else
		{
			if (_stream.CanWrite)
			{
				_stream.Flush();
			}
			_writePos = (_readPos = (_readLen = 0));
		}
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled<int>(cancellationToken);
		}
		EnsureNotClosed();
		return FlushAsyncInternal(cancellationToken);
	}

	private async Task FlushAsyncInternal(CancellationToken cancellationToken)
	{
		SemaphoreSlim sem = LazyEnsureAsyncActiveSemaphoreInitialized();
		await sem.WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			if (_writePos > 0)
			{
				await FlushWriteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			else if (_readPos < _readLen)
			{
				if (_stream.CanSeek)
				{
					FlushRead();
				}
				if (_stream.CanWrite)
				{
					await _stream.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
			}
			else if (_stream.CanWrite)
			{
				await _stream.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		finally
		{
			sem.Release();
		}
	}

	private void FlushRead()
	{
		if (_readPos - _readLen != 0)
		{
			_stream.Seek(_readPos - _readLen, SeekOrigin.Current);
		}
		_readPos = 0;
		_readLen = 0;
	}

	private void ClearReadBufferBeforeWrite()
	{
		if (_readPos == _readLen)
		{
			_readPos = (_readLen = 0);
			return;
		}
		if (!_stream.CanSeek)
		{
			throw new NotSupportedException("Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.");
		}
		FlushRead();
	}

	private void FlushWrite()
	{
		_stream.Write(_buffer, 0, _writePos);
		_writePos = 0;
		_stream.Flush();
	}

	private async Task FlushWriteAsync(CancellationToken cancellationToken)
	{
		await _stream.WriteAsync(new ReadOnlyMemory<byte>(_buffer, 0, _writePos), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_writePos = 0;
		await _stream.FlushAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	private int ReadFromBuffer(byte[] array, int offset, int count)
	{
		int num = _readLen - _readPos;
		if (num == 0)
		{
			return 0;
		}
		if (num > count)
		{
			num = count;
		}
		Buffer.BlockCopy(_buffer, _readPos, array, offset, num);
		_readPos += num;
		return num;
	}

	private int ReadFromBuffer(Span<byte> destination)
	{
		int num = Math.Min(_readLen - _readPos, destination.Length);
		if (num > 0)
		{
			new ReadOnlySpan<byte>(_buffer, _readPos, num).CopyTo(destination);
			_readPos += num;
		}
		return num;
	}

	private int ReadFromBuffer(byte[] array, int offset, int count, out Exception error)
	{
		try
		{
			error = null;
			return ReadFromBuffer(array, offset, count);
		}
		catch (Exception ex)
		{
			error = ex;
			return 0;
		}
	}

	public override int Read(byte[] array, int offset, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
		}
		if (array.Length - offset < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		EnsureNotClosed();
		EnsureCanRead();
		int num = ReadFromBuffer(array, offset, count);
		if (num == count)
		{
			return num;
		}
		int num2 = num;
		if (num > 0)
		{
			count -= num;
			offset += num;
		}
		_readPos = (_readLen = 0);
		if (_writePos > 0)
		{
			FlushWrite();
		}
		if (count >= _bufferSize)
		{
			return _stream.Read(array, offset, count) + num2;
		}
		EnsureBufferAllocated();
		_readLen = _stream.Read(_buffer, 0, _bufferSize);
		num = ReadFromBuffer(array, offset, count);
		return num + num2;
	}

	public override int Read(Span<byte> destination)
	{
		EnsureNotClosed();
		EnsureCanRead();
		int num = ReadFromBuffer(destination);
		if (num == destination.Length)
		{
			return num;
		}
		if (num > 0)
		{
			destination = destination.Slice(num);
		}
		_readPos = (_readLen = 0);
		if (_writePos > 0)
		{
			FlushWrite();
		}
		if (destination.Length >= _bufferSize)
		{
			return _stream.Read(destination) + num;
		}
		EnsureBufferAllocated();
		_readLen = _stream.Read(_buffer, 0, _bufferSize);
		return ReadFromBuffer(destination) + num;
	}

	private Task<int> LastSyncCompletedReadTask(int val)
	{
		Task<int> lastSyncCompletedReadTask = _lastSyncCompletedReadTask;
		if (lastSyncCompletedReadTask != null && lastSyncCompletedReadTask.Result == val)
		{
			return lastSyncCompletedReadTask;
		}
		return _lastSyncCompletedReadTask = Task.FromResult(val);
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled<int>(cancellationToken);
		}
		EnsureNotClosed();
		EnsureCanRead();
		int num = 0;
		SemaphoreSlim semaphoreSlim = LazyEnsureAsyncActiveSemaphoreInitialized();
		Task task = semaphoreSlim.WaitAsync();
		if (task.IsCompletedSuccessfully)
		{
			bool flag = true;
			try
			{
				num = ReadFromBuffer(buffer, offset, count, out var error);
				flag = num == count || error != null;
				if (flag)
				{
					return (error == null) ? LastSyncCompletedReadTask(num) : Task.FromException<int>(error);
				}
			}
			finally
			{
				if (flag)
				{
					semaphoreSlim.Release();
				}
			}
		}
		return ReadFromUnderlyingStreamAsync(new Memory<byte>(buffer, offset + num, count - num), cancellationToken, num, task).AsTask();
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
		}
		EnsureNotClosed();
		EnsureCanRead();
		int num = 0;
		SemaphoreSlim semaphoreSlim = LazyEnsureAsyncActiveSemaphoreInitialized();
		Task task = semaphoreSlim.WaitAsync();
		if (task.IsCompletedSuccessfully)
		{
			bool flag = true;
			try
			{
				num = ReadFromBuffer(buffer.Span);
				flag = num == buffer.Length;
				if (flag)
				{
					return new ValueTask<int>(num);
				}
			}
			finally
			{
				if (flag)
				{
					semaphoreSlim.Release();
				}
			}
		}
		return ReadFromUnderlyingStreamAsync(buffer.Slice(num), cancellationToken, num, task);
	}

	private async ValueTask<int> ReadFromUnderlyingStreamAsync(Memory<byte> buffer, CancellationToken cancellationToken, int bytesAlreadySatisfied, Task semaphoreLockTask)
	{
		await semaphoreLockTask.ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			int num = ReadFromBuffer(buffer.Span);
			if (num == buffer.Length)
			{
				return bytesAlreadySatisfied + num;
			}
			if (num > 0)
			{
				buffer = buffer.Slice(num);
				bytesAlreadySatisfied += num;
			}
			_readPos = (_readLen = 0);
			if (_writePos > 0)
			{
				await FlushWriteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			if (buffer.Length >= _bufferSize)
			{
				int num2 = bytesAlreadySatisfied;
				return num2 + await _stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			EnsureBufferAllocated();
			_readLen = await _stream.ReadAsync(new Memory<byte>(_buffer, 0, _bufferSize), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			num = ReadFromBuffer(buffer.Span);
			return bytesAlreadySatisfied + num;
		}
		finally
		{
			LazyEnsureAsyncActiveSemaphoreInitialized().Release();
		}
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(ReadAsync(buffer, offset, count, CancellationToken.None), callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return TaskToApm.End<int>(asyncResult);
	}

	public override int ReadByte()
	{
		if (_readPos == _readLen)
		{
			return ReadByteSlow();
		}
		return _buffer[_readPos++];
	}

	private int ReadByteSlow()
	{
		EnsureNotClosed();
		EnsureCanRead();
		if (_writePos > 0)
		{
			FlushWrite();
		}
		EnsureBufferAllocated();
		_readLen = _stream.Read(_buffer, 0, _bufferSize);
		_readPos = 0;
		if (_readLen == 0)
		{
			return -1;
		}
		return _buffer[_readPos++];
	}

	private void WriteToBuffer(byte[] array, ref int offset, ref int count)
	{
		int num = Math.Min(_bufferSize - _writePos, count);
		if (num > 0)
		{
			EnsureBufferAllocated();
			Buffer.BlockCopy(array, offset, _buffer, _writePos, num);
			_writePos += num;
			count -= num;
			offset += num;
		}
	}

	private int WriteToBuffer(ReadOnlySpan<byte> buffer)
	{
		int num = Math.Min(_bufferSize - _writePos, buffer.Length);
		if (num > 0)
		{
			EnsureBufferAllocated();
			buffer.Slice(0, num).CopyTo(new Span<byte>(_buffer, _writePos, num));
			_writePos += num;
		}
		return num;
	}

	private void WriteToBuffer(byte[] array, ref int offset, ref int count, out Exception error)
	{
		try
		{
			error = null;
			WriteToBuffer(array, ref offset, ref count);
		}
		catch (Exception ex)
		{
			error = ex;
		}
	}

	public override void Write(byte[] array, int offset, int count)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
		}
		if (array.Length - offset < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		EnsureNotClosed();
		EnsureCanWrite();
		if (_writePos == 0)
		{
			ClearReadBufferBeforeWrite();
		}
		int num;
		checked
		{
			num = _writePos + count;
			if (num + count < _bufferSize + _bufferSize)
			{
				WriteToBuffer(array, ref offset, ref count);
				if (_writePos >= _bufferSize)
				{
					_stream.Write(_buffer, 0, _writePos);
					_writePos = 0;
					WriteToBuffer(array, ref offset, ref count);
				}
				return;
			}
		}
		if (_writePos > 0)
		{
			if (num <= _bufferSize + _bufferSize && num <= 81920)
			{
				EnsureShadowBufferAllocated();
				Buffer.BlockCopy(array, offset, _buffer, _writePos, count);
				_stream.Write(_buffer, 0, num);
				_writePos = 0;
				return;
			}
			_stream.Write(_buffer, 0, _writePos);
			_writePos = 0;
		}
		_stream.Write(array, offset, count);
	}

	public override void Write(ReadOnlySpan<byte> buffer)
	{
		EnsureNotClosed();
		EnsureCanWrite();
		if (_writePos == 0)
		{
			ClearReadBufferBeforeWrite();
		}
		int num;
		checked
		{
			num = _writePos + buffer.Length;
			if (num + buffer.Length < _bufferSize + _bufferSize)
			{
				int start = WriteToBuffer(buffer);
				if (_writePos >= _bufferSize)
				{
					buffer = buffer.Slice(start);
					_stream.Write(_buffer, 0, _writePos);
					_writePos = 0;
					start = WriteToBuffer(buffer);
				}
				return;
			}
		}
		if (_writePos > 0)
		{
			if (num <= _bufferSize + _bufferSize && num <= 81920)
			{
				EnsureShadowBufferAllocated();
				buffer.CopyTo(new Span<byte>(_buffer, _writePos, buffer.Length));
				_stream.Write(_buffer, 0, num);
				_writePos = 0;
				return;
			}
			_stream.Write(_buffer, 0, _writePos);
			_writePos = 0;
		}
		_stream.Write(buffer);
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer", "Buffer cannot be null.");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
		}
		return WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return new ValueTask(Task.FromCanceled<int>(cancellationToken));
		}
		EnsureNotClosed();
		EnsureCanWrite();
		SemaphoreSlim semaphoreSlim = LazyEnsureAsyncActiveSemaphoreInitialized();
		Task task = semaphoreSlim.WaitAsync();
		if (task.IsCompletedSuccessfully)
		{
			bool flag = true;
			try
			{
				if (_writePos == 0)
				{
					ClearReadBufferBeforeWrite();
				}
				flag = buffer.Length < _bufferSize - _writePos;
				if (flag)
				{
					WriteToBuffer(buffer.Span);
					return default(ValueTask);
				}
			}
			finally
			{
				if (flag)
				{
					semaphoreSlim.Release();
				}
			}
		}
		return new ValueTask(WriteToUnderlyingStreamAsync(buffer, cancellationToken, task));
	}

	private async Task WriteToUnderlyingStreamAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken, Task semaphoreLockTask)
	{
		await semaphoreLockTask.ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			if (_writePos == 0)
			{
				ClearReadBufferBeforeWrite();
			}
			int num;
			checked
			{
				num = _writePos + buffer.Length;
				if (num + buffer.Length < _bufferSize + _bufferSize)
				{
					buffer = buffer.Slice(WriteToBuffer(buffer.Span));
					if (_writePos >= _bufferSize)
					{
						await _stream.WriteAsync(new ReadOnlyMemory<byte>(_buffer, 0, _writePos), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
						_writePos = 0;
						WriteToBuffer(buffer.Span);
					}
					return;
				}
			}
			if (_writePos > 0)
			{
				if (num <= _bufferSize + _bufferSize && num <= 81920)
				{
					EnsureShadowBufferAllocated();
					buffer.Span.CopyTo(new Span<byte>(_buffer, _writePos, buffer.Length));
					await _stream.WriteAsync(new ReadOnlyMemory<byte>(_buffer, 0, num), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					_writePos = 0;
					return;
				}
				await _stream.WriteAsync(new ReadOnlyMemory<byte>(_buffer, 0, _writePos), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_writePos = 0;
			}
			await _stream.WriteAsync(buffer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			LazyEnsureAsyncActiveSemaphoreInitialized().Release();
		}
	}

	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(WriteAsync(buffer, offset, count, CancellationToken.None), callback, state);
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		TaskToApm.End(asyncResult);
	}

	public override void WriteByte(byte value)
	{
		EnsureNotClosed();
		if (_writePos == 0)
		{
			EnsureCanWrite();
			ClearReadBufferBeforeWrite();
			EnsureBufferAllocated();
		}
		if (_writePos >= _bufferSize - 1)
		{
			FlushWrite();
		}
		_buffer[_writePos++] = value;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		EnsureNotClosed();
		EnsureCanSeek();
		if (_writePos > 0)
		{
			FlushWrite();
			return _stream.Seek(offset, origin);
		}
		if (_readLen - _readPos > 0 && origin == SeekOrigin.Current)
		{
			offset -= _readLen - _readPos;
		}
		long position = Position;
		long num = _stream.Seek(offset, origin);
		_readPos = (int)(num - (position - _readPos));
		if (0 <= _readPos && _readPos < _readLen)
		{
			_stream.Seek(_readLen - _readPos, SeekOrigin.Current);
		}
		else
		{
			_readPos = (_readLen = 0);
		}
		return num;
	}

	public override void SetLength(long value)
	{
		if (value < 0)
		{
			throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
		}
		EnsureNotClosed();
		EnsureCanSeek();
		EnsureCanWrite();
		Flush();
		_stream.SetLength(value);
	}

	public override void CopyTo(Stream destination, int bufferSize)
	{
		StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
		int num = _readLen - _readPos;
		if (num > 0)
		{
			destination.Write(_buffer, _readPos, num);
			_readPos = (_readLen = 0);
		}
		else if (_writePos > 0)
		{
			FlushWrite();
		}
		_stream.CopyTo(destination, bufferSize);
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
		if (!cancellationToken.IsCancellationRequested)
		{
			return CopyToAsyncCore(destination, bufferSize, cancellationToken);
		}
		return Task.FromCanceled<int>(cancellationToken);
	}

	private async Task CopyToAsyncCore(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		await LazyEnsureAsyncActiveSemaphoreInitialized().WaitAsync().ConfigureAwait(continueOnCapturedContext: false);
		try
		{
			int num = _readLen - _readPos;
			if (num > 0)
			{
				await destination.WriteAsync(new ReadOnlyMemory<byte>(_buffer, _readPos, num), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				_readPos = (_readLen = 0);
			}
			else if (_writePos > 0)
			{
				await FlushWriteAsync(cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			await _stream.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
		finally
		{
			_asyncActiveSemaphore.Release();
		}
	}
}
