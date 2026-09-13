namespace System.Xml;

internal class ContentTransferEncodingHeader : MimeHeader
{
	private ContentTransferEncoding contentTransferEncoding;

	private string contentTransferEncodingValue;

	public static readonly ContentTransferEncodingHeader Binary = new ContentTransferEncodingHeader(ContentTransferEncoding.Binary, "binary");

	public static readonly ContentTransferEncodingHeader EightBit = new ContentTransferEncodingHeader(ContentTransferEncoding.EightBit, "8bit");

	public static readonly ContentTransferEncodingHeader SevenBit = new ContentTransferEncodingHeader(ContentTransferEncoding.SevenBit, "7bit");

	public ContentTransferEncoding ContentTransferEncoding
	{
		get
		{
			ParseValue();
			return contentTransferEncoding;
		}
	}

	public string ContentTransferEncodingValue
	{
		get
		{
			ParseValue();
			return contentTransferEncodingValue;
		}
	}

	public ContentTransferEncodingHeader(string value)
		: base("content-transfer-encoding", value.ToLowerInvariant())
	{
	}

	public ContentTransferEncodingHeader(ContentTransferEncoding contentTransferEncoding, string value)
		: base("content-transfer-encoding", null)
	{
		this.contentTransferEncoding = contentTransferEncoding;
		contentTransferEncodingValue = value;
	}

	private void ParseValue()
	{
		if (contentTransferEncodingValue == null)
		{
			int offset = 0;
			contentTransferEncodingValue = ((base.Value.Length == 0) ? base.Value : ((base.Value[0] == '"') ? MailBnfHelper.ReadQuotedString(base.Value, ref offset, null) : MailBnfHelper.ReadToken(base.Value, ref offset, null)));
			switch (contentTransferEncodingValue)
			{
			case "7bit":
				contentTransferEncoding = ContentTransferEncoding.SevenBit;
				break;
			case "8bit":
				contentTransferEncoding = ContentTransferEncoding.EightBit;
				break;
			case "binary":
				contentTransferEncoding = ContentTransferEncoding.Binary;
				break;
			default:
				contentTransferEncoding = ContentTransferEncoding.Other;
				break;
			}
		}
	}
}
