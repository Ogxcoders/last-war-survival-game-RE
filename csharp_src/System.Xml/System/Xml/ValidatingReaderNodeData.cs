namespace System.Xml;

internal class ValidatingReaderNodeData
{
	private string localName;

	private string namespaceUri;

	private string prefix;

	private string nameWPrefix;

	private string rawValue;

	private string originalStringValue;

	private int depth;

	private AttributePSVIInfo attributePSVIInfo;

	private XmlNodeType nodeType;

	private int lineNo;

	private int linePos;

	public string LocalName
	{
		get
		{
			return localName;
		}
		set
		{
			localName = value;
		}
	}

	public string Namespace
	{
		get
		{
			return namespaceUri;
		}
		set
		{
			namespaceUri = value;
		}
	}

	public string Prefix
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

	public int Depth
	{
		get
		{
			return depth;
		}
		set
		{
			depth = value;
		}
	}

	public string RawValue
	{
		get
		{
			return rawValue;
		}
		set
		{
			rawValue = value;
		}
	}

	public string OriginalStringValue
	{
		get
		{
			return originalStringValue;
		}
		set
		{
			originalStringValue = value;
		}
	}

	public XmlNodeType NodeType
	{
		get
		{
			return nodeType;
		}
		set
		{
			nodeType = value;
		}
	}

	public AttributePSVIInfo AttInfo
	{
		get
		{
			return attributePSVIInfo;
		}
		set
		{
			attributePSVIInfo = value;
		}
	}

	public int LineNumber => lineNo;

	public int LinePosition => linePos;

	public ValidatingReaderNodeData()
	{
		Clear(XmlNodeType.None);
	}

	public ValidatingReaderNodeData(XmlNodeType nodeType)
	{
		Clear(nodeType);
	}

	public string GetAtomizedNameWPrefix(XmlNameTable nameTable)
	{
		if (nameWPrefix == null)
		{
			if (prefix.Length == 0)
			{
				nameWPrefix = localName;
			}
			else
			{
				nameWPrefix = nameTable.Add(prefix + ":" + localName);
			}
		}
		return nameWPrefix;
	}

	internal void Clear(XmlNodeType nodeType)
	{
		this.nodeType = nodeType;
		localName = string.Empty;
		prefix = string.Empty;
		namespaceUri = string.Empty;
		rawValue = string.Empty;
		if (attributePSVIInfo != null)
		{
			attributePSVIInfo.Reset();
		}
		nameWPrefix = null;
		lineNo = 0;
		linePos = 0;
	}

	internal void ClearName()
	{
		localName = string.Empty;
		prefix = string.Empty;
		namespaceUri = string.Empty;
	}

	internal void SetLineInfo(int lineNo, int linePos)
	{
		this.lineNo = lineNo;
		this.linePos = linePos;
	}

	internal void SetLineInfo(IXmlLineInfo lineInfo)
	{
		if (lineInfo != null)
		{
			lineNo = lineInfo.LineNumber;
			linePos = lineInfo.LinePosition;
		}
	}

	internal void SetItemData(string localName, string prefix, string ns, string value)
	{
		this.localName = localName;
		this.prefix = prefix;
		namespaceUri = ns;
		rawValue = value;
	}

	internal void SetItemData(string localName, string prefix, string ns, int depth)
	{
		this.localName = localName;
		this.prefix = prefix;
		namespaceUri = ns;
		this.depth = depth;
		rawValue = string.Empty;
	}

	internal void SetItemData(string value)
	{
		SetItemData(value, value);
	}

	internal void SetItemData(string value, string originalStringValue)
	{
		rawValue = value;
		this.originalStringValue = originalStringValue;
	}
}
