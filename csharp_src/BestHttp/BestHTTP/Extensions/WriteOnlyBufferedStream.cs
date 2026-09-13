using System;
using System.IO;
using BestHTTP.Logger;

namespace BestHTTP.Extensions;

public sealed class WriteOnlyBufferedStream : Stream
{
	private int _position;

	private byte[] buffer;

	private Stream stream;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length => buffer.Length;

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			throw new NotImplementedException("Position set");
		}
	}

	public WriteOnlyBufferedStream(Stream stream, int bufferSize)
	{
		this.stream = stream;
		buffer = VariableSizedBufferPool.Get(bufferSize, canBeLarger: true);
		_position = 0;
	}

	public override void Flush()
	{
		if (_position > 0)
		{
			if (HTTPManager.Logger.Level == Loglevels.All)
			{
				HTTPManager.Logger.Information("BufferStream", $"Flushing {_position:N0} bytes");
			}
			stream.Write(buffer, 0, _position);
			_position = 0;
		}
	}

	public override void Write(byte[] bufferFrom, int offset, int count)
	{
		while (count > 0)
		{
			int num = Math.Min(count, buffer.Length - _position);
			Array.Copy(bufferFrom, offset, buffer, _position, num);
			_position += num;
			offset += num;
			count -= num;
			if (_position == buffer.Length)
			{
				Flush();
			}
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return 0;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return 0L;
	}

	public override void SetLength(long value)
	{
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (buffer != null)
		{
			VariableSizedBufferPool.Release(buffer);
		}
		buffer = null;
	}
}
