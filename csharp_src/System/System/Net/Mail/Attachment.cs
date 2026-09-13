using System.IO;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail;

public class Attachment : AttachmentBase
{
	private ContentDisposition contentDisposition = new ContentDisposition();

	private Encoding nameEncoding;

	public ContentDisposition ContentDisposition => contentDisposition;

	public string Name
	{
		get
		{
			return base.ContentType.Name;
		}
		set
		{
			base.ContentType.Name = value;
		}
	}

	public Encoding NameEncoding
	{
		get
		{
			return nameEncoding;
		}
		set
		{
			nameEncoding = value;
		}
	}

	public Attachment(string fileName)
		: base(fileName)
	{
		InitName(fileName);
	}

	public Attachment(string fileName, string mediaType)
		: base(fileName, mediaType)
	{
		InitName(fileName);
	}

	public Attachment(string fileName, ContentType contentType)
		: base(fileName, contentType)
	{
		InitName(fileName);
	}

	public Attachment(Stream contentStream, ContentType contentType)
		: base(contentStream, contentType)
	{
	}

	public Attachment(Stream contentStream, string name)
		: base(contentStream)
	{
		Name = name;
	}

	public Attachment(Stream contentStream, string name, string mediaType)
		: base(contentStream, mediaType)
	{
		Name = name;
	}

	public static Attachment CreateAttachmentFromString(string content, ContentType contentType)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream);
		streamWriter.Write(content);
		streamWriter.Flush();
		memoryStream.Position = 0L;
		return new Attachment(memoryStream, contentType)
		{
			TransferEncoding = TransferEncoding.QuotedPrintable
		};
	}

	public static Attachment CreateAttachmentFromString(string content, string name)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream);
		streamWriter.Write(content);
		streamWriter.Flush();
		memoryStream.Position = 0L;
		return new Attachment(memoryStream, new ContentType("text/plain"))
		{
			TransferEncoding = TransferEncoding.QuotedPrintable,
			Name = name
		};
	}

	public static Attachment CreateAttachmentFromString(string content, string name, Encoding contentEncoding, string mediaType)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		MemoryStream memoryStream = new MemoryStream();
		StreamWriter streamWriter = new StreamWriter(memoryStream, contentEncoding);
		streamWriter.Write(content);
		streamWriter.Flush();
		memoryStream.Position = 0L;
		return new Attachment(memoryStream, name, mediaType)
		{
			TransferEncoding = MailMessage.GuessTransferEncoding(contentEncoding),
			ContentType = 
			{
				CharSet = streamWriter.Encoding.BodyName
			}
		};
	}

	private void InitName(string fileName)
	{
		if (fileName == null)
		{
			throw new ArgumentNullException("fileName");
		}
		Name = Path.GetFileName(fileName);
	}
}
