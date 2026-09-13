using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

internal sealed class ChunkedMemoryStream : Stream
{
	private sealed class MemoryChunk
	{
		internal readonly byte[] _buffer;

		internal int _freeOffset;

		internal MemoryChunk _next;

		internal MemoryChunk(int bufferSize)
		{
			_buffer = new byte[bufferSize];
		}
	}

	private MemoryChunk _headChunk;

	private MemoryChunk _currentChunk;

	private const int InitialChunkDefaultSize = 1024;

	private const int MaxChunkSize = 1048576;

	private int _totalLength;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length => _totalLength;

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

	internal ChunkedMemoryStream()
	{
	}

	public byte[] ToArray()
	{
		byte[] array = new byte[_totalLength];
		int num = 0;
		for (MemoryChunk memoryChunk = _headChunk; memoryChunk != null; memoryChunk = memoryChunk._next)
		{
			Buffer.BlockCopy(memoryChunk._buffer, 0, array, num, memoryChunk._freeOffset);
			num += memoryChunk._freeOffset;
		}
		return array;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		while (count > 0)
		{
			if (_currentChunk != null)
			{
				int num = _currentChunk._buffer.Length - _currentChunk._freeOffset;
				if (num > 0)
				{
					int num2 = Math.Min(num, count);
					Buffer.BlockCopy(buffer, offset, _currentChunk._buffer, _currentChunk._freeOffset, num2);
					count -= num2;
					offset += num2;
					_totalLength += num2;
					_currentChunk._freeOffset += num2;
					continue;
				}
			}
			AppendChunk(count);
		}
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return Task.FromCanceled(cancellationToken);
		}
		Write(buffer, offset, count);
		return Task.CompletedTask;
	}

	private void AppendChunk(long count)
	{
		int num = ((_currentChunk != null) ? (_currentChunk._buffer.Length * 2) : 1024);
		if (count > num)
		{
			num = (int)Math.Min(count, 1048576L);
		}
		MemoryChunk memoryChunk = new MemoryChunk(num);
		if (_currentChunk == null)
		{
			_headChunk = (_currentChunk = memoryChunk);
			return;
		}
		_currentChunk._next = memoryChunk;
		_currentChunk = memoryChunk;
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public override void SetLength(long value)
	{
		if (_currentChunk != null)
		{
			throw new NotSupportedException();
		}
		AppendChunk(value);
	}
}
