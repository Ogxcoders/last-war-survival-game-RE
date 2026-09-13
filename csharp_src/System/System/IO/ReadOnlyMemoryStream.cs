using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

internal sealed class ReadOnlyMemoryStream : Stream
{
	private readonly ReadOnlyMemory<byte> _content;

	private int _position;

	public override bool CanRead => true;

	public override bool CanSeek => true;

	public override bool CanWrite => false;

	public override long Length => _content.Length;

	public override long Position
	{
		get
		{
			return _position;
		}
		set
		{
			if (value < 0 || value > int.MaxValue)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			_position = (int)value;
		}
	}

	public ReadOnlyMemoryStream(ReadOnlyMemory<byte> content)
	{
		_content = content;
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		long num = origin switch
		{
			SeekOrigin.End => _content.Length + offset, 
			SeekOrigin.Current => _position + offset, 
			SeekOrigin.Begin => offset, 
			_ => throw new ArgumentOutOfRangeException("origin"), 
		};
		if (num > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (num < 0)
		{
			throw new IOException("An attempt was made to move the position before the beginning of the stream.");
		}
		_position = (int)num;
		return _position;
	}

	public override int ReadByte()
	{
		ReadOnlySpan<byte> span = _content.Span;
		if (_position >= span.Length)
		{
			return -1;
		}
		return span[_position++];
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		ValidateReadArrayArguments(buffer, offset, count);
		return Read(new Span<byte>(buffer, offset, count));
	}

	public override int Read(Span<byte> buffer)
	{
		int num = _content.Length - _position;
		if (num <= 0 || buffer.Length == 0)
		{
			return 0;
		}
		ReadOnlySpan<byte> readOnlySpan;
		if (num <= buffer.Length)
		{
			readOnlySpan = _content.Span;
			readOnlySpan = readOnlySpan.Slice(_position);
			readOnlySpan.CopyTo(buffer);
			_position = _content.Length;
			return num;
		}
		readOnlySpan = _content.Span;
		readOnlySpan = readOnlySpan.Slice(_position, buffer.Length);
		readOnlySpan.CopyTo(buffer);
		_position += buffer.Length;
		return buffer.Length;
	}

	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		ValidateReadArrayArguments(buffer, offset, count);
		if (!cancellationToken.IsCancellationRequested)
		{
			return Task.FromResult(Read(new Span<byte>(buffer, offset, count)));
		}
		return Task.FromCanceled<int>(cancellationToken);
	}

	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (!cancellationToken.IsCancellationRequested)
		{
			return new ValueTask<int>(Read(buffer.Span));
		}
		return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
	}

	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(ReadAsync(buffer, offset, count), callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		return TaskToApm.End<int>(asyncResult);
	}

	public override void CopyTo(Stream destination, int bufferSize)
	{
		StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
		if (_content.Length > _position)
		{
			destination.Write(_content.Span.Slice(_position));
		}
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
	{
		StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
		if (_content.Length <= _position)
		{
			return Task.CompletedTask;
		}
		return destination.WriteAsync(_content.Slice(_position), cancellationToken).AsTask();
	}

	public override void Flush()
	{
	}

	public override Task FlushAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	public override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		throw new NotSupportedException();
	}

	private static void ValidateReadArrayArguments(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || buffer.Length - offset < count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
	}
}
