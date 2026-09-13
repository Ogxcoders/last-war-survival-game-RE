using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http;

public class MultipartContent : HttpContent, IEnumerable<HttpContent>, IEnumerable
{
	private sealed class ContentReadStream : Stream
	{
		private readonly Stream[] _streams;

		private readonly long _length;

		private int _next;

		private Stream _current;

		private long _position;

		public override bool CanRead => true;

		public override bool CanSeek => true;

		public override bool CanWrite => false;

		public override long Position
		{
			get
			{
				return _position;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				long num = 0L;
				for (int i = 0; i < _streams.Length; i++)
				{
					Stream stream = _streams[i];
					long length = stream.Length;
					if (value < num + length)
					{
						_current = stream;
						i = (_next = i + 1);
						stream.Position = value - num;
						for (; i < _streams.Length; i++)
						{
							_streams[i].Position = 0L;
						}
						_position = value;
						return;
					}
					num += length;
				}
				_current = null;
				_next = _streams.Length;
				_position = value;
			}
		}

		public override long Length => _length;

		internal ContentReadStream(Stream[] streams)
		{
			_streams = streams;
			foreach (Stream stream in streams)
			{
				_length += stream.Length;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Stream[] streams = _streams;
				for (int i = 0; i < streams.Length; i++)
				{
					streams[i].Dispose();
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			ValidateReadArgs(buffer, offset, count);
			if (count == 0)
			{
				return 0;
			}
			while (true)
			{
				if (_current != null)
				{
					int num = _current.Read(buffer, offset, count);
					if (num != 0)
					{
						_position += num;
						return num;
					}
					_current = null;
				}
				if (_next >= _streams.Length)
				{
					break;
				}
				_current = _streams[_next++];
			}
			return 0;
		}

		public override int Read(Span<byte> buffer)
		{
			if (buffer.Length == 0)
			{
				return 0;
			}
			while (true)
			{
				if (_current != null)
				{
					int num = _current.Read(buffer);
					if (num != 0)
					{
						_position += num;
						return num;
					}
					_current = null;
				}
				if (_next >= _streams.Length)
				{
					break;
				}
				_current = _streams[_next++];
			}
			return 0;
		}

		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			ValidateReadArgs(buffer, offset, count);
			return ReadAsyncPrivate(new Memory<byte>(buffer, offset, count), cancellationToken).AsTask();
		}

		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			return ReadAsyncPrivate(buffer, cancellationToken);
		}

		public override IAsyncResult BeginRead(byte[] array, int offset, int count, AsyncCallback asyncCallback, object asyncState)
		{
			return TaskToApm.Begin(ReadAsync(array, offset, count, CancellationToken.None), asyncCallback, asyncState);
		}

		public override int EndRead(IAsyncResult asyncResult)
		{
			return TaskToApm.End<int>(asyncResult);
		}

		public async ValueTask<int> ReadAsyncPrivate(Memory<byte> buffer, CancellationToken cancellationToken)
		{
			if (buffer.Length == 0)
			{
				return 0;
			}
			while (true)
			{
				if (_current != null)
				{
					int num = await _current.ReadAsync(buffer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					if (num != 0)
					{
						_position += num;
						return num;
					}
					_current = null;
				}
				if (_next >= _streams.Length)
				{
					break;
				}
				_current = _streams[_next++];
			}
			return 0;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
			case SeekOrigin.Begin:
				Position = offset;
				break;
			case SeekOrigin.Current:
				Position += offset;
				break;
			case SeekOrigin.End:
				Position = _length + offset;
				break;
			default:
				throw new ArgumentOutOfRangeException("origin");
			}
			return Position;
		}

		private static void ValidateReadArgs(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset > buffer.Length - count)
			{
				throw new ArgumentException("The buffer was not long enough.", "buffer");
			}
		}

		public override void Flush()
		{
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override void Write(ReadOnlySpan<byte> buffer)
		{
			throw new NotSupportedException();
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			throw new NotSupportedException();
		}

		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException();
		}
	}

	private const string CrLf = "\r\n";

	private static readonly int s_crlfLength = GetEncodedLength("\r\n");

	private static readonly int s_dashDashLength = GetEncodedLength("--");

	private static readonly int s_colonSpaceLength = GetEncodedLength(": ");

	private static readonly int s_commaSpaceLength = GetEncodedLength(", ");

	private readonly List<HttpContent> _nestedContent;

	private readonly string _boundary;

	public MultipartContent()
		: this("mixed", GetDefaultBoundary())
	{
	}

	public MultipartContent(string subtype)
		: this(subtype, GetDefaultBoundary())
	{
	}

	public MultipartContent(string subtype, string boundary)
	{
		if (string.IsNullOrWhiteSpace(subtype))
		{
			throw new ArgumentException("The value cannot be null or empty.", "subtype");
		}
		ValidateBoundary(boundary);
		_boundary = boundary;
		string text = boundary;
		if (!text.StartsWith("\"", StringComparison.Ordinal))
		{
			text = "\"" + text + "\"";
		}
		MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("multipart/" + subtype)
		{
			Parameters = 
			{
				new NameValueHeaderValue("boundary", text)
			}
		};
		base.Headers.ContentType = contentType;
		_nestedContent = new List<HttpContent>();
	}

	private static void ValidateBoundary(string boundary)
	{
		if (string.IsNullOrWhiteSpace(boundary))
		{
			throw new ArgumentException("The value cannot be null or empty.", "boundary");
		}
		if (boundary.Length > 70)
		{
			throw new ArgumentOutOfRangeException("boundary", boundary, string.Format(CultureInfo.InvariantCulture, "The field cannot be longer than {0} characters.", 70));
		}
		if (boundary.EndsWith(" ", StringComparison.Ordinal))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", boundary), "boundary");
		}
		foreach (char c in boundary)
		{
			if (('0' > c || c > '9') && ('a' > c || c > 'z') && ('A' > c || c > 'Z') && "'()+_,-./:=? ".IndexOf(c) < 0)
			{
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", boundary), "boundary");
			}
		}
	}

	private static string GetDefaultBoundary()
	{
		return Guid.NewGuid().ToString();
	}

	public virtual void Add(HttpContent content)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		_nestedContent.Add(content);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (HttpContent item in _nestedContent)
			{
				item.Dispose();
			}
			_nestedContent.Clear();
		}
		base.Dispose(disposing);
	}

	public IEnumerator<HttpContent> GetEnumerator()
	{
		return _nestedContent.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _nestedContent.GetEnumerator();
	}

	protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
	{
		_ = 3;
		try
		{
			await EncodeStringToStreamAsync(stream, "--" + _boundary + "\r\n").ConfigureAwait(continueOnCapturedContext: false);
			StringBuilder output = new StringBuilder();
			for (int contentIndex = 0; contentIndex < _nestedContent.Count; contentIndex++)
			{
				HttpContent content = _nestedContent[contentIndex];
				await EncodeStringToStreamAsync(stream, SerializeHeadersToString(output, contentIndex, content)).ConfigureAwait(continueOnCapturedContext: false);
				await content.CopyToAsync(stream).ConfigureAwait(continueOnCapturedContext: false);
			}
			await EncodeStringToStreamAsync(stream, "\r\n--" + _boundary + "--\r\n").ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (Exception ex)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, ex);
			}
			throw;
		}
	}

	protected override async Task<Stream> CreateContentReadStreamAsync()
	{
		_ = 1;
		try
		{
			Stream[] streams = new Stream[2 + _nestedContent.Count * 2];
			StringBuilder scratch = new StringBuilder();
			int streamIndex = 0;
			streams[streamIndex++] = EncodeStringToNewStream("--" + _boundary + "\r\n");
			for (int contentIndex = 0; contentIndex < _nestedContent.Count; contentIndex++)
			{
				HttpContent httpContent = _nestedContent[contentIndex];
				streams[streamIndex++] = EncodeStringToNewStream(SerializeHeadersToString(scratch, contentIndex, httpContent));
				Stream stream = (await httpContent.ReadAsStreamAsync().ConfigureAwait(continueOnCapturedContext: false)) ?? new MemoryStream();
				if (!stream.CanSeek)
				{
					return await base.CreateContentReadStreamAsync().ConfigureAwait(continueOnCapturedContext: false);
				}
				streams[streamIndex++] = stream;
			}
			streams[streamIndex] = EncodeStringToNewStream("\r\n--" + _boundary + "--\r\n");
			return new ContentReadStream(streams);
		}
		catch (Exception ex)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, ex);
			}
			throw;
		}
	}

	private string SerializeHeadersToString(StringBuilder scratch, int contentIndex, HttpContent content)
	{
		scratch.Clear();
		if (contentIndex != 0)
		{
			scratch.Append("\r\n--");
			scratch.Append(_boundary);
			scratch.Append("\r\n");
		}
		foreach (KeyValuePair<string, IEnumerable<string>> header in content.Headers)
		{
			scratch.Append(header.Key);
			scratch.Append(": ");
			string value = string.Empty;
			foreach (string item in header.Value)
			{
				scratch.Append(value);
				scratch.Append(item);
				value = ", ";
			}
			scratch.Append("\r\n");
		}
		scratch.Append("\r\n");
		return scratch.ToString();
	}

	private static ValueTask EncodeStringToStreamAsync(Stream stream, string input)
	{
		byte[] bytes = HttpRuleParser.DefaultHttpEncoding.GetBytes(input);
		return stream.WriteAsync(new ReadOnlyMemory<byte>(bytes));
	}

	private static Stream EncodeStringToNewStream(string input)
	{
		return new MemoryStream(HttpRuleParser.DefaultHttpEncoding.GetBytes(input), writable: false);
	}

	protected internal override bool TryComputeLength(out long length)
	{
		int encodedLength = GetEncodedLength(_boundary);
		long num = 0L;
		long num2 = s_crlfLength + s_dashDashLength + encodedLength + s_crlfLength;
		num += s_dashDashLength + encodedLength + s_crlfLength;
		bool flag = true;
		foreach (HttpContent item in _nestedContent)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				num += num2;
			}
			foreach (KeyValuePair<string, IEnumerable<string>> header in item.Headers)
			{
				num += GetEncodedLength(header.Key) + s_colonSpaceLength;
				int num3 = 0;
				foreach (string item2 in header.Value)
				{
					num += GetEncodedLength(item2);
					num3++;
				}
				if (num3 > 1)
				{
					num += (num3 - 1) * s_commaSpaceLength;
				}
				num += s_crlfLength;
			}
			num += s_crlfLength;
			long length2 = 0L;
			if (!item.TryComputeLength(out length2))
			{
				length = 0L;
				return false;
			}
			num += length2;
		}
		num += s_crlfLength + s_dashDashLength + encodedLength + s_dashDashLength + s_crlfLength;
		length = num;
		return true;
	}

	private static int GetEncodedLength(string input)
	{
		return HttpRuleParser.DefaultHttpEncoding.GetByteCount(input);
	}
}
