using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace System.Data.SqlClient;

internal sealed class SqlCachedStream : Stream
{
	private int _currentPosition;

	private int _currentArrayIndex;

	private List<byte[]> _cachedBytes;

	private long _totalLength;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => TotalLength;

	public override long Position
	{
		get
		{
			long num = 0L;
			if (_currentArrayIndex > 0)
			{
				for (int i = 0; i < _currentArrayIndex; i++)
				{
					num += _cachedBytes[i].Length;
				}
			}
			return num + _currentPosition;
		}
		set
		{
			if (_cachedBytes == null)
			{
				throw ADP.StreamClosed("set_Position");
			}
			SetInternalPosition(value, "set_Position");
		}
	}

	private long TotalLength
	{
		get
		{
			if (_totalLength == 0L && _cachedBytes != null)
			{
				long num = 0L;
				for (int i = 0; i < _cachedBytes.Count; i++)
				{
					num += _cachedBytes[i].Length;
				}
				_totalLength = num;
			}
			return _totalLength;
		}
	}

	internal SqlCachedStream(SqlCachedBuffer sqlBuf)
	{
		_cachedBytes = sqlBuf.CachedBytes;
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing && _cachedBytes != null)
			{
				_cachedBytes.Clear();
			}
			_cachedBytes = null;
			_currentPosition = 0;
			_currentArrayIndex = 0;
			_totalLength = 0L;
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override void Flush()
	{
		throw ADP.NotSupported();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		int num = 0;
		if (_cachedBytes == null)
		{
			throw ADP.StreamClosed("Read");
		}
		if (buffer == null)
		{
			throw ADP.ArgumentNull("buffer");
		}
		if (offset < 0 || count < 0)
		{
			throw ADP.ArgumentOutOfRange(string.Empty, (offset < 0) ? "offset" : "count");
		}
		if (buffer.Length - offset < count)
		{
			throw ADP.ArgumentOutOfRange("count");
		}
		if (_cachedBytes.Count <= _currentArrayIndex)
		{
			return 0;
		}
		while (count > 0)
		{
			if (_cachedBytes[_currentArrayIndex].Length <= _currentPosition)
			{
				_currentArrayIndex++;
				if (_cachedBytes.Count <= _currentArrayIndex)
				{
					break;
				}
				_currentPosition = 0;
			}
			int num2 = _cachedBytes[_currentArrayIndex].Length - _currentPosition;
			if (num2 > count)
			{
				num2 = count;
			}
			Buffer.BlockCopy(_cachedBytes[_currentArrayIndex], _currentPosition, buffer, offset, num2);
			_currentPosition += num2;
			count -= num2;
			offset += num2;
			num += num2;
		}
		return num;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		long num = 0L;
		if (_cachedBytes == null)
		{
			throw ADP.StreamClosed("Seek");
		}
		switch (origin)
		{
		case SeekOrigin.Begin:
			SetInternalPosition(offset, "offset");
			break;
		case SeekOrigin.Current:
			num = offset + Position;
			SetInternalPosition(num, "offset");
			break;
		case SeekOrigin.End:
			num = TotalLength + offset;
			SetInternalPosition(num, "offset");
			break;
		default:
			throw ADP.InvalidSeekOrigin("offset");
		}
		return num;
	}

	public override void SetLength(long value)
	{
		throw ADP.NotSupported();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw ADP.NotSupported();
	}

	private void SetInternalPosition(long lPos, string argumentName)
	{
		long num = lPos;
		if (num < 0)
		{
			throw new ArgumentOutOfRangeException(argumentName);
		}
		for (int i = 0; i < _cachedBytes.Count; i++)
		{
			if (num > _cachedBytes[i].Length)
			{
				num -= _cachedBytes[i].Length;
				continue;
			}
			_currentArrayIndex = i;
			_currentPosition = (int)num;
			return;
		}
		if (num > 0)
		{
			throw new ArgumentOutOfRangeException(argumentName);
		}
	}
}
