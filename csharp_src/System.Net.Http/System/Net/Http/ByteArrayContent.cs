using System.IO;
using System.Threading.Tasks;

namespace System.Net.Http;

public class ByteArrayContent : HttpContent
{
	private readonly byte[] _content;

	private readonly int _offset;

	private readonly int _count;

	public ByteArrayContent(byte[] content)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		_content = content;
		_offset = 0;
		_count = content.Length;
		SetBuffer(_content, _offset, _count);
	}

	public ByteArrayContent(byte[] content, int offset, int count)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		if (offset < 0 || offset > content.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || count > content.Length - offset)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		_content = content;
		_offset = offset;
		_count = count;
		SetBuffer(_content, _offset, _count);
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		return stream.WriteAsync(_content, _offset, _count);
	}

	protected internal override bool TryComputeLength(out long length)
	{
		length = _count;
		return true;
	}

	protected override Task<Stream> CreateContentReadStreamAsync()
	{
		return Task.FromResult((Stream)CreateMemoryStreamForByteArray());
	}

	internal override Stream TryCreateContentReadStream()
	{
		if (!(GetType() == typeof(ByteArrayContent)))
		{
			return null;
		}
		return CreateMemoryStreamForByteArray();
	}

	internal MemoryStream CreateMemoryStreamForByteArray()
	{
		return new MemoryStream(_content, _offset, _count, writable: false);
	}
}
