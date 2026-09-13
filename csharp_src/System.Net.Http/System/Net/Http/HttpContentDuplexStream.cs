using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

internal abstract class HttpContentDuplexStream : HttpContentStream
{
	public sealed override bool CanRead => true;

	public sealed override bool CanWrite => true;

	public HttpContentDuplexStream(HttpConnection connection)
		: base(connection)
	{
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Info(this);
		}
	}

	public sealed override void Flush()
	{
		FlushAsync().GetAwaiter().GetResult();
	}

	public sealed override int Read(byte[] buffer, int offset, int count)
	{
		HttpContentStream.ValidateBufferArgs(buffer, offset, count);
		return ReadAsync(new Memory<byte>(buffer, offset, count), CancellationToken.None).GetAwaiter().GetResult();
	}

	public sealed override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		HttpContentStream.ValidateBufferArgs(buffer, offset, count);
		return ReadAsync(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
	}

	public sealed override void Write(byte[] buffer, int offset, int count)
	{
		HttpContentStream.ValidateBufferArgs(buffer, offset, count);
		WriteAsync(new Memory<byte>(buffer, offset, count), CancellationToken.None).GetAwaiter().GetResult();
	}

	public sealed override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		HttpContentStream.ValidateBufferArgs(buffer, offset, count);
		return WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
	}

	public sealed override void CopyTo(Stream destination, int bufferSize)
	{
		CopyToAsync(destination, bufferSize, CancellationToken.None).GetAwaiter().GetResult();
	}
}
