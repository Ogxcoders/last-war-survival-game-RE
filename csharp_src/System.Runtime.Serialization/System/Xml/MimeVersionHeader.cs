using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal class MimeVersionHeader : MimeHeader
{
	public static readonly MimeVersionHeader Default = new MimeVersionHeader("1.0");

	private string version;

	public string Version
	{
		get
		{
			if (version == null && base.Value != null)
			{
				ParseValue();
			}
			return version;
		}
	}

	public MimeVersionHeader(string value)
		: base("mime-version", value)
	{
	}

	private void ParseValue()
	{
		if (base.Value == "1.0")
		{
			version = "1.0";
			return;
		}
		int offset = 0;
		if (!MailBnfHelper.SkipCFWS(base.Value, ref offset))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME version header is invalid.")));
		}
		StringBuilder stringBuilder = new StringBuilder();
		MailBnfHelper.ReadDigits(base.Value, ref offset, stringBuilder);
		if (!MailBnfHelper.SkipCFWS(base.Value, ref offset) || offset >= base.Value.Length || base.Value[offset++] != '.' || !MailBnfHelper.SkipCFWS(base.Value, ref offset))
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME version header is invalid.")));
		}
		stringBuilder.Append('.');
		MailBnfHelper.ReadDigits(base.Value, ref offset, stringBuilder);
		version = stringBuilder.ToString();
	}
}
