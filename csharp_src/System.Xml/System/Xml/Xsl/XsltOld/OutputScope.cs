namespace System.Xml.Xsl.XsltOld;

internal class OutputScope : DocumentScope
{
	private string name;

	private string nsUri;

	private string prefix;

	private XmlSpace space;

	private string lang;

	private bool mixed;

	private bool toCData;

	private HtmlElementProps htmlElementProps;

	internal string Name => name;

	internal string Namespace => nsUri;

	internal string Prefix
	{
		get
		{
			return prefix;
		}
		set
		{
			prefix = value;
		}
	}

	internal XmlSpace Space
	{
		get
		{
			return space;
		}
		set
		{
			space = value;
		}
	}

	internal string Lang
	{
		get
		{
			return lang;
		}
		set
		{
			lang = value;
		}
	}

	internal bool Mixed
	{
		get
		{
			return mixed;
		}
		set
		{
			mixed = value;
		}
	}

	internal bool ToCData
	{
		get
		{
			return toCData;
		}
		set
		{
			toCData = value;
		}
	}

	internal HtmlElementProps HtmlElementProps
	{
		get
		{
			return htmlElementProps;
		}
		set
		{
			htmlElementProps = value;
		}
	}

	internal OutputScope()
	{
		Init(string.Empty, string.Empty, string.Empty, XmlSpace.None, string.Empty, mixed: false);
	}

	internal void Init(string name, string nspace, string prefix, XmlSpace space, string lang, bool mixed)
	{
		scopes = null;
		this.name = name;
		nsUri = nspace;
		this.prefix = prefix;
		this.space = space;
		this.lang = lang;
		this.mixed = mixed;
		toCData = false;
		htmlElementProps = null;
	}

	internal bool FindPrefix(string urn, out string prefix)
	{
		for (NamespaceDecl next = scopes; next != null; next = next.Next)
		{
			if (Ref.Equal(next.Uri, urn) && next.Prefix != null && next.Prefix.Length > 0)
			{
				prefix = next.Prefix;
				return true;
			}
		}
		prefix = string.Empty;
		return false;
	}
}
