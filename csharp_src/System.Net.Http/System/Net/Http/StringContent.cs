using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace System.Net.Http;

public class StringContent : ByteArrayContent
{
	private const string DefaultMediaType = "text/plain";

	public StringContent(string content)
		: this(content, null, null)
	{
	}

	public StringContent(string content, Encoding encoding)
		: this(content, encoding, null)
	{
	}

	public StringContent(string content, Encoding encoding, string mediaType)
		: base(GetContentByteArray(content, encoding))
	{
		MediaTypeHeaderValue contentType = new MediaTypeHeaderValue((mediaType == null) ? "text/plain" : mediaType)
		{
			CharSet = ((encoding == null) ? HttpContent.DefaultStringEncoding.WebName : encoding.WebName)
		};
		base.Headers.ContentType = contentType;
	}

	private static byte[] GetContentByteArray(string content, Encoding encoding)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		if (encoding == null)
		{
			encoding = HttpContent.DefaultStringEncoding;
		}
		return encoding.GetBytes(content);
	}

	internal override Stream TryCreateContentReadStream()
	{
		if (!(GetType() == typeof(StringContent)))
		{
			return null;
		}
		return CreateMemoryStreamForByteArray();
	}
}
