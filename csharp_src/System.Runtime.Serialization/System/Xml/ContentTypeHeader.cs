using System.Collections.Generic;
using System.Runtime.Serialization;

namespace System.Xml;

internal class ContentTypeHeader : MimeHeader
{
	public static readonly ContentTypeHeader Default = new ContentTypeHeader("application/octet-stream");

	private string mediaType;

	private string subType;

	private Dictionary<string, string> parameters;

	public string MediaType
	{
		get
		{
			if (mediaType == null && base.Value != null)
			{
				ParseValue();
			}
			return mediaType;
		}
	}

	public string MediaSubtype
	{
		get
		{
			if (subType == null && base.Value != null)
			{
				ParseValue();
			}
			return subType;
		}
	}

	public Dictionary<string, string> Parameters
	{
		get
		{
			if (parameters == null)
			{
				if (base.Value != null)
				{
					ParseValue();
				}
				else
				{
					parameters = new Dictionary<string, string>();
				}
			}
			return parameters;
		}
	}

	public ContentTypeHeader(string value)
		: base("content-type", value)
	{
	}

	private void ParseValue()
	{
		if (parameters != null)
		{
			return;
		}
		int offset = 0;
		parameters = new Dictionary<string, string>();
		mediaType = MailBnfHelper.ReadToken(base.Value, ref offset, null);
		if (offset >= base.Value.Length || base.Value[offset++] != '/')
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME content type header is invalid.")));
		}
		subType = MailBnfHelper.ReadToken(base.Value, ref offset, null);
		while (MailBnfHelper.SkipCFWS(base.Value, ref offset))
		{
			if (offset >= base.Value.Length || base.Value[offset++] != ';')
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME content type header is invalid.")));
			}
			if (!MailBnfHelper.SkipCFWS(base.Value, ref offset))
			{
				break;
			}
			string text = MailBnfHelper.ReadParameterAttribute(base.Value, ref offset, null);
			if (text == null || offset >= base.Value.Length || base.Value[offset++] != '=')
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME content type header is invalid.")));
			}
			string text2 = MailBnfHelper.ReadParameterValue(base.Value, ref offset, null);
			parameters.Add(text.ToLowerInvariant(), text2);
		}
		if (!parameters.ContainsKey(MtomGlobals.StartInfoParam))
		{
			return;
		}
		string text3 = parameters[MtomGlobals.StartInfoParam];
		int offset2 = text3.IndexOf(';');
		if (offset2 <= -1)
		{
			return;
		}
		while (MailBnfHelper.SkipCFWS(text3, ref offset2))
		{
			if (text3[offset2] == ';')
			{
				offset2++;
				string text4 = MailBnfHelper.ReadParameterAttribute(text3, ref offset2, null);
				if (text4 == null || offset2 >= text3.Length || text3[offset2++] != '=')
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME content type header is invalid.")));
				}
				string text5 = MailBnfHelper.ReadParameterValue(text3, ref offset2, null);
				if (text4 == MtomGlobals.ActionParam)
				{
					parameters[MtomGlobals.ActionParam] = text5;
				}
				continue;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME content type header is invalid.")));
		}
	}
}
