namespace System.IO.Compression;

internal sealed class CheckSumAndSizeWriteStream : Stream
{
	private readonly Stream _baseStream;

	private readonly Stream _baseBaseStream;

	private long _position;

	private uint _checksum;

	private readonly bool _leaveOpenOnClose;

	private bool _canWrite;

	private bool _isDisposed;

	private bool _everWritten;

	private long _initialPosition;

	private readonly ZipArchiveEntry _zipArchiveEntry;

	private readonly EventHandler _onClose;

	private readonly Action<long, long, uint, Stream, ZipArchiveEntry, EventHandler> _saveCrcAndSizes;

	public override long Length
	{
		get
		{
			ThrowIfDisposed();
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
		}
	}

	public override long Position
	{
		get
		{
			ThrowIfDisposed();
			return _position;
		}
		set
		{
			ThrowIfDisposed();
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
		}
	}

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => _canWrite;

	public CheckSumAndSizeWriteStream(Stream baseStream, Stream baseBaseStream, bool leaveOpenOnClose, ZipArchiveEntry entry, EventHandler onClose, Action<long, long, uint, Stream, ZipArchiveEntry, EventHandler> saveCrcAndSizes)
	{
		_baseStream = baseStream;
		_baseBaseStream = baseBaseStream;
		_position = 0L;
		_checksum = 0u;
		_leaveOpenOnClose = leaveOpenOnClose;
		_canWrite = true;
		_isDisposed = false;
		_initialPosition = 0L;
		_zipArchiveEntry = entry;
		_onClose = onClose;
		_saveCrcAndSizes = saveCrcAndSizes;
	}

	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw new ObjectDisposedException(GetType().ToString(), "A stream from ZipArchiveEntry has been disposed.");
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		ThrowIfDisposed();
		throw new NotSupportedException("This stream from ZipArchiveEntry does not support reading.");
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		ThrowIfDisposed();
		throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
	}

	public override void SetLength(long value)
	{
		ThrowIfDisposed();
		throw new NotSupportedException("SetLength requires a stream that supports seeking and writing.");
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset", "The argument must be non-negative.");
		}
		if (count < 0)
		{
			throw new ArgumentOutOfRangeException("count", "The argument must be non-negative.");
		}
		if (buffer.Length - offset < count)
		{
			throw new ArgumentException("The offset and length parameters are not valid for the array that was given.");
		}
		ThrowIfDisposed();
		if (count != 0)
		{
			if (!_everWritten)
			{
				_initialPosition = _baseBaseStream.Position;
				_everWritten = true;
			}
			_checksum = Crc32Helper.UpdateCrc32(_checksum, buffer, offset, count);
			_baseStream.Write(buffer, offset, count);
			_position += count;
		}
	}

	public override void Flush()
	{
		ThrowIfDisposed();
		_baseStream.Flush();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_isDisposed)
		{
			if (!_everWritten)
			{
				_initialPosition = _baseBaseStream.Position;
			}
			if (!_leaveOpenOnClose)
			{
				_baseStream.Dispose();
			}
			_saveCrcAndSizes?.Invoke(_initialPosition, Position, _checksum, _baseBaseStream, _zipArchiveEntry, _onClose);
			_isDisposed = true;
		}
		base.Dispose(disposing);
	}
}
