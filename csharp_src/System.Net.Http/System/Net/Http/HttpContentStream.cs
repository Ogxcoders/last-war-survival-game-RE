using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal abstract class HttpContentStream : Stream
{
	protected HttpConnection _connection;

	public sealed override bool CanSeek => false;

	public sealed override long Length
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public sealed override long Position
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

	public HttpContentStream(HttpConnection connection)
	{
		_connection = connection;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && _connection != null)
		{
			_connection.Dispose();
			_connection = null;
		}
		base.Dispose(disposing);
	}

	public sealed override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(ReadAsync(buffer, offset, count, default(CancellationToken)), callback, state);
	}

	public sealed override int EndRead(IAsyncResult asyncResult)
	{
		return TaskToApm.End<int>(asyncResult);
	}

	public sealed override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		return TaskToApm.Begin(WriteAsync(buffer, offset, count, default(CancellationToken)), callback, state);
	}

	public sealed override void EndWrite(IAsyncResult asyncResult)
	{
		TaskToApm.End(asyncResult);
	}

	public sealed override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotSupportedException();
	}

	public sealed override void SetLength(long value)
	{
		throw new NotSupportedException();
	}

	protected static void ValidateBufferArgs(byte[] buffer, int offset, int count)
	{
		if (buffer == null)
		{
			throw new ArgumentNullException("buffer");
		}
		if ((uint)offset > buffer.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if ((uint)count > buffer.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
	}

	protected static void ValidateCopyToArgs(Stream source, Stream destination, int bufferSize)
	{
		if (destination == null)
		{
			throw new ArgumentNullException("destination");
		}
		if (bufferSize <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize", bufferSize, "Positive number required.");
		}
		if (!destination.CanWrite)
		{
			throw destination.CanRead ? ((SystemException)new NotSupportedException("Stream does not support writing.")) : ((SystemException)new ObjectDisposedException("destination", "Cannot access a closed Stream."));
		}
	}
}
