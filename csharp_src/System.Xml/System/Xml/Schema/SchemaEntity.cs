namespace System.Xml.Schema;

internal sealed class SchemaEntity : IDtdEntityInfo
{
	private XmlQualifiedName qname;

	private string url;

	private string pubid;

	private string text;

	private XmlQualifiedName ndata = XmlQualifiedName.Empty;

	private int lineNumber;

	private int linePosition;

	private bool isParameter;

	private bool isExternal;

	private bool parsingInProgress;

	private bool isDeclaredInExternal;

	private string baseURI;

	private string declaredURI;

	string IDtdEntityInfo.Name => Name.Name;

	bool IDtdEntityInfo.IsExternal => IsExternal;

	bool IDtdEntityInfo.IsDeclaredInExternal => DeclaredInExternal;

	bool IDtdEntityInfo.IsUnparsedEntity => !NData.IsEmpty;

	bool IDtdEntityInfo.IsParameterEntity => isParameter;

	string IDtdEntityInfo.BaseUriString => BaseURI;

	string IDtdEntityInfo.DeclaredUriString => DeclaredURI;

	string IDtdEntityInfo.SystemId => Url;

	string IDtdEntityInfo.PublicId => Pubid;

	string IDtdEntityInfo.Text => Text;

	int IDtdEntityInfo.LineNumber => Line;

	int IDtdEntityInfo.LinePosition => Pos;

	internal XmlQualifiedName Name => qname;

	internal string Url
	{
		get
		{
			return url;
		}
		set
		{
			url = value;
			isExternal = true;
		}
	}

	internal string Pubid
	{
		get
		{
			return pubid;
		}
		set
		{
			pubid = value;
		}
	}

	internal bool IsExternal
	{
		get
		{
			return isExternal;
		}
		set
		{
			isExternal = value;
		}
	}

	internal bool DeclaredInExternal
	{
		get
		{
			return isDeclaredInExternal;
		}
		set
		{
			isDeclaredInExternal = value;
		}
	}

	internal XmlQualifiedName NData
	{
		get
		{
			return ndata;
		}
		set
		{
			ndata = value;
		}
	}

	internal string Text
	{
		get
		{
			return text;
		}
		set
		{
			text = value;
			isExternal = false;
		}
	}

	internal int Line
	{
		get
		{
			return lineNumber;
		}
		set
		{
			lineNumber = value;
		}
	}

	internal int Pos
	{
		get
		{
			return linePosition;
		}
		set
		{
			linePosition = value;
		}
	}

	internal string BaseURI
	{
		get
		{
			if (baseURI != null)
			{
				return baseURI;
			}
			return string.Empty;
		}
		set
		{
			baseURI = value;
		}
	}

	internal bool ParsingInProgress
	{
		get
		{
			return parsingInProgress;
		}
		set
		{
			parsingInProgress = value;
		}
	}

	internal string DeclaredURI
	{
		get
		{
			if (declaredURI != null)
			{
				return declaredURI;
			}
			return string.Empty;
		}
		set
		{
			declaredURI = value;
		}
	}

	internal SchemaEntity(XmlQualifiedName qname, bool isParameter)
	{
		this.qname = qname;
		this.isParameter = isParameter;
	}

	internal static bool IsPredefinedEntity(string n)
	{
		switch (n)
		{
		default:
			return n == "quot";
		case "lt":
		case "gt":
		case "amp":
		case "apos":
			return true;
		}
	}
}
