using System;
using System.IO;

namespace Microsoft.SqlServer.Server;

internal sealed class DummyStream : Stream
{
	private long _size;

	public override bool CanRead => false;

	public override bool CanWrite => true;

	public override bool CanSeek => false;

	public override long Position
	{
		get
		{
			return _size;
		}
		set
		{
			_size = value;
		}
	}

	public override long Length => _size;

	private void DontDoIt()
	{
		throw new Exception(global::SR.GetString("Internal Error"));
	}

	public override void SetLength(long value)
	{
		_size = value;
	}

	public override long Seek(long value, SeekOrigin loc)
	{
		DontDoIt();
		return -1L;
	}

	public override void Flush()
	{
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		DontDoIt();
		return -1;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		_size += count;
	}
}
