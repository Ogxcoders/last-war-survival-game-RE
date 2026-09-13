using System.Text;

namespace System.Xml.Xsl.XsltOld;

internal class BuilderInfo
{
	private string name;

	private string localName;

	private string namespaceURI;

	private string prefix;

	private XmlNodeType nodeType;

	private int depth;

	private bool isEmptyTag;

	internal string[] TextInfo = new string[4];

	internal int TextInfoCount;

	internal bool search;

	internal HtmlElementProps htmlProps;

	internal HtmlAttributeProps htmlAttrProps;

	internal string Name
	{
		get
		{
			if (name == null)
			{
				string text = Prefix;
				string text2 = LocalName;
				if (text != null && 0 < text.Length)
				{
					if (text2.Length > 0)
					{
						name = text + ":" + text2;
					}
					else
					{
						name = text;
					}
				}
				else
				{
					name = text2;
				}
			}
			return name;
		}
	}

	internal string LocalName
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

	internal string NamespaceURI
	{
		get
		{
			return namespaceURI;
		}
		set
		{
			namespaceURI = value;
		}
	}

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

	internal string Value
	{
		get
		{
			switch (TextInfoCount)
			{
			case 0:
				return string.Empty;
			case 1:
				return TextInfo[0];
			default:
			{
				int num = 0;
				for (int i = 0; i < TextInfoCount; i++)
				{
					string text = TextInfo[i];
					if (text != null)
					{
						num += text.Length;
					}
				}
				StringBuilder stringBuilder = new StringBuilder(num);
				for (int j = 0; j < TextInfoCount; j++)
				{
					string text2 = TextInfo[j];
					if (text2 != null)
					{
						stringBuilder.Append(text2);
					}
				}
				return stringBuilder.ToString();
			}
			}
		}
		set
		{
			TextInfoCount = 0;
			ValueAppend(value, disableEscaping: false);
		}
	}

	internal XmlNodeType NodeType
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

	internal int Depth
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

	internal bool IsEmptyTag
	{
		get
		{
			return isEmptyTag;
		}
		set
		{
			isEmptyTag = value;
		}
	}

	internal BuilderInfo()
	{
		Initialize(string.Empty, string.Empty, string.Empty);
	}

	internal void Initialize(string prefix, string name, string nspace)
	{
		this.prefix = prefix;
		localName = name;
		namespaceURI = nspace;
		this.name = null;
		htmlProps = null;
		htmlAttrProps = null;
		TextInfoCount = 0;
	}

	internal void Initialize(BuilderInfo src)
	{
		prefix = src.Prefix;
		localName = src.LocalName;
		namespaceURI = src.NamespaceURI;
		name = null;
		depth = src.Depth;
		nodeType = src.NodeType;
		htmlProps = src.htmlProps;
		htmlAttrProps = src.htmlAttrProps;
		TextInfoCount = 0;
		EnsureTextInfoSize(src.TextInfoCount);
		src.TextInfo.CopyTo(TextInfo, 0);
		TextInfoCount = src.TextInfoCount;
	}

	private void EnsureTextInfoSize(int newSize)
	{
		if (TextInfo.Length < newSize)
		{
			string[] array = new string[newSize * 2];
			Array.Copy(TextInfo, array, TextInfoCount);
			TextInfo = array;
		}
	}

	internal BuilderInfo Clone()
	{
		BuilderInfo builderInfo = new BuilderInfo();
		builderInfo.Initialize(this);
		return builderInfo;
	}

	internal void ValueAppend(string s, bool disableEscaping)
	{
		if (s != null && s.Length != 0)
		{
			EnsureTextInfoSize(TextInfoCount + ((!disableEscaping) ? 1 : 2));
			if (disableEscaping)
			{
				TextInfo[TextInfoCount++] = null;
			}
			TextInfo[TextInfoCount++] = s;
		}
	}
}
