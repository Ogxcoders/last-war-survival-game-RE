using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public sealed class ReadOnlyMemoryContent : HttpContent
{
	private readonly ReadOnlyMemory<byte> _content;

	public ReadOnlyMemoryContent(ReadOnlyMemory<byte> content)
	{
		_content = content;
		if (MemoryMarshal.TryGetArray(content, out var segment))
		{
			SetBuffer(segment.Array, segment.Offset, segment.Count);
		}
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		return stream.WriteAsync(_content).AsTask();
	}

	internal override Task SerializeToStreamAsync(Stream stream, TransportContext context, CancellationToken cancellationToken)
	{
		return stream.WriteAsync(_content, cancellationToken).AsTask();
	}

	protected internal override bool TryComputeLength(out long length)
	{
		length = _content.Length;
		return true;
	}

	protected override Task<Stream> CreateContentReadStreamAsync()
	{
		return Task.FromResult((Stream)new ReadOnlyMemoryStream(_content));
	}

	internal override Stream TryCreateContentReadStream()
	{
		return new ReadOnlyMemoryStream(_content);
	}
}
