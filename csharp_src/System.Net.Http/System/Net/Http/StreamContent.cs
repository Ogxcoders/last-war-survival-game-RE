using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class StreamContent : HttpContent
{
	private sealed class ReadOnlyStream : DelegatingStream
	{
		public override bool CanWrite => false;

		public override int WriteTimeout
		{
			get
			{
				throw new NotSupportedException("The stream does not support writing.");
			}
			set
			{
				throw new NotSupportedException("The stream does not support writing.");
			}
		}

		public ReadOnlyStream(Stream innerStream)
			: base(innerStream)
		{
		}

		public override void Flush()
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override void Write(ReadOnlySpan<byte> buffer)
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override void WriteByte(byte value)
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			throw new NotSupportedException("The stream does not support writing.");
		}

		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException("The stream does not support writing.");
		}
	}

	private Stream _content;

	private int _bufferSize;

	private bool _contentConsumed;

	private long _start;

	public StreamContent(Stream content)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		InitializeContent(content, 0);
	}

	public StreamContent(Stream content, int bufferSize)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		if (bufferSize <= 0)
		{
			throw new ArgumentOutOfRangeException("bufferSize");
		}
		InitializeContent(content, bufferSize);
	}

	private void InitializeContent(Stream content, int bufferSize)
	{
		_content = content;
		_bufferSize = bufferSize;
		if (content.CanSeek)
		{
			_start = content.Position;
		}
		if (NetEventSource.IsEnabled)
		{
			NetEventSource.Associate(this, content);
		}
	}

	protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		PrepareContent();
		return StreamToStreamCopy.CopyAsync(_content, stream, _bufferSize, !_content.CanSeek);
	}

	protected internal override bool TryComputeLength(out long length)
	{
		if (_content.CanSeek)
		{
			length = _content.Length - _start;
			return true;
		}
		length = 0L;
		return false;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_content.Dispose();
		}
		base.Dispose(disposing);
	}

	protected override Task<Stream> CreateContentReadStreamAsync()
	{
		return Task.FromResult((Stream)new ReadOnlyStream(_content));
	}

	internal override Stream TryCreateContentReadStream()
	{
		if (!(GetType() == typeof(StreamContent)))
		{
			return null;
		}
		return new ReadOnlyStream(_content);
	}

	private void PrepareContent()
	{
		if (_contentConsumed)
		{
			if (!_content.CanSeek)
			{
				throw new InvalidOperationException("The stream was already consumed. It cannot be read again.");
			}
			_content.Position = _start;
		}
		_contentConsumed = true;
	}
}
