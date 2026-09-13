namespace System.IO.Compression;

internal sealed class SubReadStream : Stream
{
	private readonly long _startInSuperStream;

	private long _positionInSuperStream;

	private readonly long _endInSuperStream;

	private readonly Stream _superStream;

	private bool _canRead;

	private bool _isDisposed;

	public override long Length
	{
		get
		{
			ThrowIfDisposed();
			return _endInSuperStream - _startInSuperStream;
		}
	}

	public override long Position
	{
		get
		{
			ThrowIfDisposed();
			return _positionInSuperStream - _startInSuperStream;
		}
		set
		{
			ThrowIfDisposed();
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
		}
	}

	public override bool CanRead
	{
		get
		{
			if (_superStream.CanRead)
			{
				return _canRead;
			}
			return false;
		}
	}

	public override bool CanSeek => false;

	public override bool CanWrite => false;

	public SubReadStream(Stream superStream, long startPosition, long maxLength)
	{
		_startInSuperStream = startPosition;
		_positionInSuperStream = startPosition;
		_endInSuperStream = startPosition + maxLength;
		_superStream = superStream;
		_canRead = true;
		_isDisposed = false;
	}

	private void ThrowIfDisposed()
	{
		if (_isDisposed)
		{
			throw new ObjectDisposedException(GetType().ToString(), "A stream from ZipArchiveEntry has been disposed.");
		}
	}

	private void ThrowIfCantRead()
	{
		if (!CanRead)
		{
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support reading.");
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		ThrowIfDisposed();
		ThrowIfCantRead();
		if (_superStream.Position != _positionInSuperStream)
		{
			_superStream.Seek(_positionInSuperStream, SeekOrigin.Begin);
		}
		if (_positionInSuperStream + count > _endInSuperStream)
		{
			count = (int)(_endInSuperStream - _positionInSuperStream);
		}
		int num = _superStream.Read(buffer, offset, count);
		_positionInSuperStream += num;
		return num;
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
		ThrowIfDisposed();
		throw new NotSupportedException("This stream from ZipArchiveEntry does not support writing.");
	}

	public override void Flush()
	{
		ThrowIfDisposed();
		throw new NotSupportedException("This stream from ZipArchiveEntry does not support writing.");
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_isDisposed)
		{
			_canRead = false;
			_isDisposed = true;
		}
		base.Dispose(disposing);
	}
}
