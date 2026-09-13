using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml;

internal class MimeHeaders
{
	private static class Constants
	{
		public const string ContentTransferEncoding = "content-transfer-encoding";

		public const string ContentID = "content-id";

		public const string ContentType = "content-type";

		public const string MimeVersion = "mime-version";
	}

	private Dictionary<string, MimeHeader> headers = new Dictionary<string, MimeHeader>();

	public ContentTypeHeader ContentType
	{
		get
		{
			if (headers.TryGetValue("content-type", out var value))
			{
				return value as ContentTypeHeader;
			}
			return null;
		}
	}

	public ContentIDHeader ContentID
	{
		get
		{
			if (headers.TryGetValue("content-id", out var value))
			{
				return value as ContentIDHeader;
			}
			return null;
		}
	}

	public ContentTransferEncodingHeader ContentTransferEncoding
	{
		get
		{
			if (headers.TryGetValue("content-transfer-encoding", out var value))
			{
				return value as ContentTransferEncodingHeader;
			}
			return null;
		}
	}

	public MimeVersionHeader MimeVersion
	{
		get
		{
			if (headers.TryGetValue("mime-version", out var value))
			{
				return value as MimeVersionHeader;
			}
			return null;
		}
	}

	public void Add(string name, string value, ref int remaining)
	{
		if (name == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
		}
		if (value == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
		}
		switch (name)
		{
		case "content-type":
			Add(new ContentTypeHeader(value));
			break;
		case "content-id":
			Add(new ContentIDHeader(name, value));
			break;
		case "content-transfer-encoding":
			Add(new ContentTransferEncodingHeader(value));
			break;
		case "mime-version":
			Add(new MimeVersionHeader(value));
			break;
		default:
			remaining += value.Length * 2;
			break;
		}
		remaining += name.Length * 2;
	}

	public void Add(MimeHeader header)
	{
		if (header == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("header");
		}
		if (headers.TryGetValue(header.Name, out var _))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME header '{0}' already exists.", header.Name)));
		}
		headers.Add(header.Name, header);
	}

	public void Release(ref int remaining)
	{
		foreach (MimeHeader value in headers.Values)
		{
			remaining += value.Value.Length * 2;
		}
	}
}
