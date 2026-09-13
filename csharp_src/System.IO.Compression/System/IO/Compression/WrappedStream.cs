namespace System.IO.Compression;

internal sealed class WrappedStream : Stream
{
	private readonly Stream _baseStream;

	private readonly bool _closeBaseStream;

	private readonly Action<ZipArchiveEntry> _onClosed;

	private readonly ZipArchiveEntry _zipArchiveEntry;

	private bool _isDisposed;

	public override long Length
	{
		get
		{
			ThrowIfDisposed();
			return _baseStream.Length;
		}
	}

	public override long Position
	{
		get
		{
			ThrowIfDisposed();
			return _baseStream.Position;
		}
		set
		{
			ThrowIfDisposed();
			ThrowIfCantSeek();
			_baseStream.Position = value;
		}
	}

	public override bool CanRead
	{
		get
		{
			if (!_isDisposed)
			{
				return _baseStream.CanRead;
			}
			return false;
		}
	}

	public override bool CanSeek
	{
		get
		{
			if (!_isDisposed)
			{
				return _baseStream.CanSeek;
			}
			return false;
		}
	}

	public override bool CanWrite
	{
		get
		{
			if (!_isDisposed)
			{
				return _baseStream.CanWrite;
			}
			return false;
		}
	}

	internal WrappedStream(Stream baseStream, bool closeBaseStream)
		: this(baseStream, closeBaseStream, null, null)
	{
	}

	private WrappedStream(Stream baseStream, bool closeBaseStream, ZipArchiveEntry entry, Action<ZipArchiveEntry> onClosed)
	{
		_baseStream = baseStream;
		_closeBaseStream = closeBaseStream;
		_onClosed = onClosed;
		_zipArchiveEntry = entry;
		_isDisposed = false;
	}

	internal WrappedStream(Stream baseStream, ZipArchiveEntry entry, Action<ZipArchiveEntry> onClosed)
		: this(baseStream, closeBaseStream: false, entry, onClosed)
	{
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

	private void ThrowIfCantWrite()
	{
		if (!CanWrite)
		{
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support writing.");
		}
	}

	private void ThrowIfCantSeek()
	{
		if (!CanSeek)
		{
			throw new NotSupportedException("This stream from ZipArchiveEntry does not support seeking.");
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		ThrowIfDisposed();
		ThrowIfCantRead();
		return _baseStream.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		ThrowIfDisposed();
		ThrowIfCantSeek();
		return _baseStream.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		ThrowIfDisposed();
		ThrowIfCantSeek();
		ThrowIfCantWrite();
		_baseStream.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		ThrowIfDisposed();
		ThrowIfCantWrite();
		_baseStream.Write(buffer, offset, count);
	}

	public override void Flush()
	{
		ThrowIfDisposed();
		ThrowIfCantWrite();
		_baseStream.Flush();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && !_isDisposed)
		{
			_onClosed?.Invoke(_zipArchiveEntry);
			if (_closeBaseStream)
			{
				_baseStream.Dispose();
			}
			_isDisposed = true;
		}
		base.Dispose(disposing);
	}
}
