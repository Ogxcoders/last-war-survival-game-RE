using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;

namespace System.Data.SqlTypes;

internal sealed class StreamOnSqlChars : SqlStreamChars
{
	private SqlChars _sqlchars;

	private long _lPosition;

	public override bool IsNull
	{
		get
		{
			if (_sqlchars != null)
			{
				return _sqlchars.IsNull;
			}
			return true;
		}
	}

	public override long Length
	{
		get
		{
			CheckIfStreamClosed("get_Length");
			return _sqlchars.Length;
		}
	}

	public override long Position
	{
		get
		{
			CheckIfStreamClosed("get_Position");
			return _lPosition;
		}
		set
		{
			CheckIfStreamClosed("set_Position");
			if (value < 0 || value > _sqlchars.Length)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_lPosition = value;
		}
	}

	internal StreamOnSqlChars(SqlChars s)
	{
		_sqlchars = s;
		_lPosition = 0L;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		CheckIfStreamClosed("Seek");
		long num = 0L;
		switch (origin)
		{
		case SeekOrigin.Begin:
			if (offset < 0 || offset > _sqlchars.Length)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			_lPosition = offset;
			break;
		case SeekOrigin.Current:
			num = _lPosition + offset;
			if (num < 0 || num > _sqlchars.Length)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			_lPosition = num;
			break;
		case SeekOrigin.End:
			num = _sqlchars.Length + offset;
			if (num < 0 || num > _sqlchars.Length)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			_lPosition = num;
			break;
		default:
			throw ADP.ArgumentOutOfRange("offset");
		}
		return _lPosition;
	}

	public override int Read(char[] buffer, int offset, int count)
	{
		CheckIfStreamClosed("Read");
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		int num = (int)_sqlchars.Read(_lPosition, buffer, offset, count);
		_lPosition += num;
		return num;
	}

	public override void Write(char[] buffer, int offset, int count)
	{
		CheckIfStreamClosed("Write");
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0 || offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		_sqlchars.Write(_lPosition, buffer, offset, count);
		_lPosition += count;
	}

	public override void SetLength(long value)
	{
		CheckIfStreamClosed("SetLength");
		_sqlchars.SetLength(value);
		if (_lPosition > value)
		{
			_lPosition = value;
		}
	}

	protected override void Dispose(bool disposing)
	{
		_sqlchars = null;
	}

	private bool FClosed()
	{
		return _sqlchars == null;
	}

	private void CheckIfStreamClosed([CallerMemberName] string methodname = "")
	{
		if (FClosed())
		{
			throw ADP.StreamClosed(methodname);
		}
	}
}
