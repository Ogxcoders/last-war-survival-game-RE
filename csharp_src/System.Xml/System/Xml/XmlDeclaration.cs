using System.Text;

namespace System.Xml;

public class XmlDeclaration : XmlLinkedNode
{
	private const string YES = "yes";

	private const string NO = "no";

	private string version;

	private string encoding;

	private string standalone;

	public string Version
	{
		get
		{
			return version;
		}
		internal set
		{
			version = value;
		}
	}

	public string Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			encoding = ((value == null) ? string.Empty : value);
		}
	}

	public string Standalone
	{
		get
		{
			return standalone;
		}
		set
		{
			if (value == null)
			{
				standalone = string.Empty;
				return;
			}
			if (value.Length == 0 || value == "yes" || value == "no")
			{
				standalone = value;
				return;
			}
			throw new ArgumentException(Res.GetString("Wrong value for the XML declaration standalone attribute of '{0}'.", value));
		}
	}

	public override string Value
	{
		get
		{
			return InnerText;
		}
		set
		{
			InnerText = value;
		}
	}

	public override string InnerText
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder("version=\"" + Version + "\"");
			if (Encoding.Length > 0)
			{
				stringBuilder.Append(" encoding=\"");
				stringBuilder.Append(Encoding);
				stringBuilder.Append("\"");
			}
			if (Standalone.Length > 0)
			{
				stringBuilder.Append(" standalone=\"");
				stringBuilder.Append(Standalone);
				stringBuilder.Append("\"");
			}
			return stringBuilder.ToString();
		}
		set
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = Encoding;
			string text5 = Standalone;
			string text6 = Version;
			XmlLoader.ParseXmlDeclarationValue(value, out text, out text2, out text3);
			try
			{
				if (text != null && !IsValidXmlVersion(text))
				{
					throw new ArgumentException(Res.GetString("Wrong XML version information. The XML must match production \"VersionNum ::= '1.' [0-9]+\"."));
				}
				Version = text;
				if (text2 != null)
				{
					Encoding = text2;
				}
				if (text3 != null)
				{
					Standalone = text3;
				}
			}
			catch
			{
				Encoding = text4;
				Standalone = text5;
				Version = text6;
				throw;
			}
		}
	}

	public override string Name => "xml";

	public override string LocalName => Name;

	public override XmlNodeType NodeType => XmlNodeType.XmlDeclaration;

	protected internal XmlDeclaration(string version, string encoding, string standalone, XmlDocument doc)
		: base(doc)
	{
		if (!IsValidXmlVersion(version))
		{
			throw new ArgumentException(Res.GetString("Wrong XML version information. The XML must match production \"VersionNum ::= '1.' [0-9]+\"."));
		}
		if (standalone != null && standalone.Length > 0 && standalone != "yes" && standalone != "no")
		{
			throw new ArgumentException(Res.GetString("Wrong value for the XML declaration standalone attribute of '{0}'.", standalone));
		}
		Encoding = encoding;
		Standalone = standalone;
		Version = version;
	}

	public override XmlNode CloneNode(bool deep)
	{
		return OwnerDocument.CreateXmlDeclaration(Version, Encoding, Standalone);
	}

	public override void WriteTo(XmlWriter w)
	{
		w.WriteProcessingInstruction(Name, InnerText);
	}

	public override void WriteContentTo(XmlWriter w)
	{
	}

	private bool IsValidXmlVersion(string ver)
	{
		if (ver.Length >= 3 && ver[0] == '1' && ver[1] == '.')
		{
			return XmlCharType.IsOnlyDigits(ver, 2, ver.Length - 2);
		}
		return false;
	}
}
