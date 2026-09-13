using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class AlternateView : AttachmentBase
{
	private Uri baseUri;

	private LinkedResourceCollection linkedResources = new LinkedResourceCollection();

	public Uri BaseUri
	{
		get
		{
			return baseUri;
		}
		set
		{
			baseUri = value;
		}
	}

	public LinkedResourceCollection LinkedResources => linkedResources;

	public AlternateView(string fileName)
		: base(fileName)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException();
		}
	}

	public AlternateView(string fileName, ContentType contentType)
		: base(fileName, contentType)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException();
		}
	}

	public AlternateView(string fileName, string mediaType)
		: base(fileName, mediaType)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException();
		}
	}

	public AlternateView(Stream contentStream)
		: base(contentStream)
	{
	}

	public AlternateView(Stream contentStream, string mediaType)
		: base(contentStream, mediaType)
	{
	}

	public AlternateView(Stream contentStream, ContentType contentType)
		: base(contentStream, contentType)
	{
	}

	public static AlternateView CreateAlternateViewFromString(string content)
	{
		if (content == null)
		{
			throw new ArgumentNullException();
		}
		return new AlternateView(new MemoryStream(Encoding.UTF8.GetBytes(content)))
		{
			TransferEncoding = TransferEncoding.QuotedPrintable
		};
	}

	public static AlternateView CreateAlternateViewFromString(string content, ContentType contentType)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		return new AlternateView(new MemoryStream(((contentType.CharSet != null) ? Encoding.GetEncoding(contentType.CharSet) : Encoding.UTF8).GetBytes(content)), contentType)
		{
			TransferEncoding = TransferEncoding.QuotedPrintable
		};
	}

	public static AlternateView CreateAlternateViewFromString(string content, Encoding contentEncoding, string mediaType)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		if (contentEncoding == null)
		{
			contentEncoding = Encoding.UTF8;
		}
		return new AlternateView(new MemoryStream(contentEncoding.GetBytes(content)), new ContentType
		{
			MediaType = mediaType,
			CharSet = contentEncoding.HeaderName
		})
		{
			TransferEncoding = TransferEncoding.QuotedPrintable
		};
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			foreach (LinkedResource linkedResource in linkedResources)
			{
				linkedResource.Dispose();
			}
		}
		base.Dispose(disposing);
	}
}
