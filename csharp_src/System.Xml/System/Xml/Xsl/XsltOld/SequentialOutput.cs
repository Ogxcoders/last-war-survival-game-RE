using System.Collections;
using System.Globalization;
using System.Text;

namespace System.Xml.Xsl.XsltOld;

internal abstract class SequentialOutput : RecordOutput
{
	private const char s_Colon = ':';

	private const char s_GreaterThan = '>';

	private const char s_LessThan = '<';

	private const char s_Space = ' ';

	private const char s_Quote = '"';

	private const char s_Semicolon = ';';

	private const char s_NewLine = '\n';

	private const char s_Return = '\r';

	private const char s_Ampersand = '&';

	private const string s_LessThanQuestion = "<?";

	private const string s_QuestionGreaterThan = "?>";

	private const string s_LessThanSlash = "</";

	private const string s_SlashGreaterThan = " />";

	private const string s_EqualQuote = "=\"";

	private const string s_DocType = "<!DOCTYPE ";

	private const string s_CommentBegin = "<!--";

	private const string s_CommentEnd = "-->";

	private const string s_CDataBegin = "<![CDATA[";

	private const string s_CDataEnd = "]]>";

	private const string s_VersionAll = " version=\"1.0\"";

	private const string s_Standalone = " standalone=\"";

	private const string s_EncodingStart = " encoding=\"";

	private const string s_Public = "PUBLIC ";

	private const string s_System = "SYSTEM ";

	private const string s_Html = "html";

	private const string s_QuoteSpace = "\" ";

	private const string s_CDataSplit = "]]]]><![CDATA[>";

	private const string s_EnLessThan = "&lt;";

	private const string s_EnGreaterThan = "&gt;";

	private const string s_EnAmpersand = "&amp;";

	private const string s_EnQuote = "&quot;";

	private const string s_EnNewLine = "&#xA;";

	private const string s_EnReturn = "&#xD;";

	private const string s_EndOfLine = "\r\n";

	private static char[] s_TextValueFind = new char[3] { '&', '>', '<' };

	private static string[] s_TextValueReplace = new string[3] { "&amp;", "&gt;", "&lt;" };

	private static char[] s_XmlAttributeValueFind = new char[6] { '&', '>', '<', '"', '\n', '\r' };

	private static string[] s_XmlAttributeValueReplace = new string[6] { "&amp;", "&gt;", "&lt;", "&quot;", "&#xA;", "&#xD;" };

	private Processor processor;

	protected Encoding encoding;

	private ArrayList outputCache;

	private bool firstLine = true;

	private bool secondRoot;

	private XsltOutput output;

	private bool isHtmlOutput;

	private bool isXmlOutput;

	private Hashtable cdataElements;

	private bool indentOutput;

	private bool outputDoctype;

	private bool outputXmlDecl;

	private bool omitXmlDeclCalled;

	private byte[] byteBuffer;

	private Encoding utf8Encoding;

	private XmlCharType xmlCharType = XmlCharType.Instance;

	private void CacheOuptutProps(XsltOutput output)
	{
		this.output = output;
		isXmlOutput = this.output.Method == XsltOutput.OutputMethod.Xml;
		isHtmlOutput = this.output.Method == XsltOutput.OutputMethod.Html;
		cdataElements = this.output.CDataElements;
		indentOutput = this.output.Indent;
		outputDoctype = this.output.DoctypeSystem != null || (isHtmlOutput && this.output.DoctypePublic != null);
		outputXmlDecl = isXmlOutput && !this.output.OmitXmlDeclaration && !omitXmlDeclCalled;
	}

	internal SequentialOutput(Processor processor)
	{
		this.processor = processor;
		CacheOuptutProps(processor.Output);
	}

	public void OmitXmlDecl()
	{
		omitXmlDeclCalled = true;
		outputXmlDecl = false;
	}

	private void WriteStartElement(RecordBuilder record)
	{
		BuilderInfo mainNode = record.MainNode;
		HtmlElementProps htmlElementProps = null;
		if (isHtmlOutput)
		{
			if (mainNode.Prefix.Length == 0)
			{
				htmlElementProps = mainNode.htmlProps;
				if (htmlElementProps == null && mainNode.search)
				{
					htmlElementProps = HtmlElementProps.GetProps(mainNode.LocalName);
				}
				record.Manager.CurrentElementScope.HtmlElementProps = htmlElementProps;
				mainNode.IsEmptyTag = false;
			}
		}
		else if (isXmlOutput && mainNode.Depth == 0)
		{
			if (secondRoot && (output.DoctypeSystem != null || output.Standalone))
			{
				throw XsltException.Create("There are multiple root elements in the output XML.");
			}
			secondRoot = true;
		}
		if (outputDoctype)
		{
			WriteDoctype(mainNode);
			outputDoctype = false;
		}
		if (cdataElements != null && cdataElements.Contains(new XmlQualifiedName(mainNode.LocalName, mainNode.NamespaceURI)) && isXmlOutput)
		{
			record.Manager.CurrentElementScope.ToCData = true;
		}
		Indent(record);
		Write('<');
		WriteName(mainNode.Prefix, mainNode.LocalName);
		WriteAttributes(record.AttributeList, record.AttributeCount, htmlElementProps);
		if (mainNode.IsEmptyTag)
		{
			Write(" />");
		}
		else
		{
			Write('>');
		}
		if (htmlElementProps != null && htmlElementProps.Head)
		{
			mainNode.Depth++;
			Indent(record);
			mainNode.Depth--;
			Write("<META http-equiv=\"Content-Type\" content=\"");
			Write(output.MediaType);
			Write("; charset=");
			Write(encoding.WebName);
			Write("\">");
		}
	}

	private void WriteTextNode(RecordBuilder record)
	{
		BuilderInfo mainNode = record.MainNode;
		OutputScope currentElementScope = record.Manager.CurrentElementScope;
		currentElementScope.Mixed = true;
		if (currentElementScope.HtmlElementProps != null && currentElementScope.HtmlElementProps.NoEntities)
		{
			Write(mainNode.Value);
		}
		else if (currentElementScope.ToCData)
		{
			WriteCDataSection(mainNode.Value);
		}
		else
		{
			WriteTextNode(mainNode);
		}
	}

	private void WriteTextNode(BuilderInfo node)
	{
		for (int i = 0; i < node.TextInfoCount; i++)
		{
			string text = node.TextInfo[i];
			if (text == null)
			{
				i++;
				Write(node.TextInfo[i]);
			}
			else
			{
				WriteWithReplace(text, s_TextValueFind, s_TextValueReplace);
			}
		}
	}

	private void WriteCDataSection(string value)
	{
		Write("<![CDATA[");
		WriteCData(value);
		Write("]]>");
	}

	private void WriteDoctype(BuilderInfo mainNode)
	{
		Indent(0);
		Write("<!DOCTYPE ");
		if (isXmlOutput)
		{
			WriteName(mainNode.Prefix, mainNode.LocalName);
		}
		else
		{
			WriteName(string.Empty, "html");
		}
		Write(' ');
		if (output.DoctypePublic != null)
		{
			Write("PUBLIC ");
			Write('"');
			Write(output.DoctypePublic);
			Write("\" ");
		}
		else
		{
			Write("SYSTEM ");
		}
		if (output.DoctypeSystem != null)
		{
			Write('"');
			Write(output.DoctypeSystem);
			Write('"');
		}
		Write('>');
	}

	private void WriteXmlDeclaration()
	{
		outputXmlDecl = false;
		Indent(0);
		Write("<?");
		WriteName(string.Empty, "xml");
		Write(" version=\"1.0\"");
		if (encoding != null)
		{
			Write(" encoding=\"");
			Write(encoding.WebName);
			Write('"');
		}
		if (output.HasStandalone)
		{
			Write(" standalone=\"");
			Write(output.Standalone ? "yes" : "no");
			Write('"');
		}
		Write("?>");
	}

	private void WriteProcessingInstruction(RecordBuilder record)
	{
		Indent(record);
		WriteProcessingInstruction(record.MainNode);
	}

	private void WriteProcessingInstruction(BuilderInfo node)
	{
		Write("<?");
		WriteName(node.Prefix, node.LocalName);
		Write(' ');
		Write(node.Value);
		if (isHtmlOutput)
		{
			Write('>');
		}
		else
		{
			Write("?>");
		}
	}

	private void WriteEndElement(RecordBuilder record)
	{
		_ = record.MainNode;
		HtmlElementProps htmlElementProps = record.Manager.CurrentElementScope.HtmlElementProps;
		if (htmlElementProps == null || !htmlElementProps.Empty)
		{
			Indent(record);
			Write("</");
			WriteName(record.MainNode.Prefix, record.MainNode.LocalName);
			Write('>');
		}
	}

	public Processor.OutputResult RecordDone(RecordBuilder record)
	{
		if (output.Method == XsltOutput.OutputMethod.Unknown)
		{
			if (!DecideDefaultOutput(record.MainNode))
			{
				CacheRecord(record);
			}
			else
			{
				OutputCachedRecords();
				OutputRecord(record);
			}
		}
		else
		{
			OutputRecord(record);
		}
		record.Reset();
		return Processor.OutputResult.Continue;
	}

	public void TheEnd()
	{
		OutputCachedRecords();
		Close();
	}

	private bool DecideDefaultOutput(BuilderInfo node)
	{
		XsltOutput.OutputMethod defaultOutput = XsltOutput.OutputMethod.Xml;
		switch (node.NodeType)
		{
		case XmlNodeType.Element:
			if (node.NamespaceURI.Length == 0 && string.Compare("html", node.LocalName, StringComparison.OrdinalIgnoreCase) == 0)
			{
				defaultOutput = XsltOutput.OutputMethod.Html;
			}
			break;
		case XmlNodeType.Text:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			if (xmlCharType.IsOnlyWhitespace(node.Value))
			{
				return false;
			}
			defaultOutput = XsltOutput.OutputMethod.Xml;
			break;
		default:
			return false;
		}
		if (processor.SetDefaultOutput(defaultOutput))
		{
			CacheOuptutProps(processor.Output);
		}
		return true;
	}

	private void CacheRecord(RecordBuilder record)
	{
		if (outputCache == null)
		{
			outputCache = new ArrayList();
		}
		outputCache.Add(record.MainNode.Clone());
	}

	private void OutputCachedRecords()
	{
		if (outputCache != null)
		{
			for (int i = 0; i < outputCache.Count; i++)
			{
				BuilderInfo node = (BuilderInfo)outputCache[i];
				OutputRecord(node);
			}
			outputCache = null;
		}
	}

	private void OutputRecord(RecordBuilder record)
	{
		BuilderInfo mainNode = record.MainNode;
		if (outputXmlDecl)
		{
			WriteXmlDeclaration();
		}
		switch (mainNode.NodeType)
		{
		case XmlNodeType.Element:
			WriteStartElement(record);
			break;
		case XmlNodeType.Text:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			WriteTextNode(record);
			break;
		case XmlNodeType.EntityReference:
			Write('&');
			WriteName(mainNode.Prefix, mainNode.LocalName);
			Write(';');
			break;
		case XmlNodeType.ProcessingInstruction:
			WriteProcessingInstruction(record);
			break;
		case XmlNodeType.Comment:
			Indent(record);
			Write("<!--");
			Write(mainNode.Value);
			Write("-->");
			break;
		case XmlNodeType.DocumentType:
			Write(mainNode.Value);
			break;
		case XmlNodeType.EndElement:
			WriteEndElement(record);
			break;
		case XmlNodeType.Attribute:
		case XmlNodeType.CDATA:
		case XmlNodeType.Entity:
		case XmlNodeType.Document:
		case XmlNodeType.DocumentFragment:
		case XmlNodeType.Notation:
			break;
		}
	}

	private void OutputRecord(BuilderInfo node)
	{
		if (outputXmlDecl)
		{
			WriteXmlDeclaration();
		}
		Indent(0);
		switch (node.NodeType)
		{
		case XmlNodeType.Text:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			WriteTextNode(node);
			break;
		case XmlNodeType.EntityReference:
			Write('&');
			WriteName(node.Prefix, node.LocalName);
			Write(';');
			break;
		case XmlNodeType.ProcessingInstruction:
			WriteProcessingInstruction(node);
			break;
		case XmlNodeType.Comment:
			Write("<!--");
			Write(node.Value);
			Write("-->");
			break;
		case XmlNodeType.DocumentType:
			Write(node.Value);
			break;
		case XmlNodeType.Element:
		case XmlNodeType.Attribute:
		case XmlNodeType.CDATA:
		case XmlNodeType.Entity:
		case XmlNodeType.Document:
		case XmlNodeType.DocumentFragment:
		case XmlNodeType.Notation:
		case XmlNodeType.EndElement:
			break;
		}
	}

	private void WriteName(string prefix, string name)
	{
		if (prefix != null && prefix.Length > 0)
		{
			Write(prefix);
			if (name == null || name.Length <= 0)
			{
				return;
			}
			Write(':');
		}
		Write(name);
	}

	private void WriteXmlAttributeValue(string value)
	{
		WriteWithReplace(value, s_XmlAttributeValueFind, s_XmlAttributeValueReplace);
	}

	private void WriteHtmlAttributeValue(string value)
	{
		int length = value.Length;
		int num = 0;
		while (num < length)
		{
			char c = value[num];
			num++;
			switch (c)
			{
			case '&':
				if (num != length && value[num] == '{')
				{
					Write(c);
				}
				else
				{
					Write("&amp;");
				}
				break;
			case '"':
				Write("&quot;");
				break;
			default:
				Write(c);
				break;
			}
		}
	}

	private void WriteHtmlUri(string value)
	{
		int length = value.Length;
		int num = 0;
		while (num < length)
		{
			char c = value[num];
			num++;
			switch (c)
			{
			case '&':
				if (num != length && value[num] == '{')
				{
					Write(c);
				}
				else
				{
					Write("&amp;");
				}
				continue;
			case '"':
				Write("&quot;");
				continue;
			case '\n':
				Write("&#xA;");
				continue;
			case '\r':
				Write("&#xD;");
				continue;
			}
			if ('\u007f' < c)
			{
				if (utf8Encoding == null)
				{
					utf8Encoding = Encoding.UTF8;
					byteBuffer = new byte[utf8Encoding.GetMaxByteCount(1)];
				}
				int bytes = utf8Encoding.GetBytes(value, num - 1, 1, byteBuffer, 0);
				for (int i = 0; i < bytes; i++)
				{
					Write("%");
					uint num2 = byteBuffer[i];
					Write(num2.ToString("X2", CultureInfo.InvariantCulture));
				}
			}
			else
			{
				Write(c);
			}
		}
	}

	private void WriteWithReplace(string value, char[] find, string[] replace)
	{
		int length = value.Length;
		int i;
		for (i = 0; i < length; i++)
		{
			int num = value.IndexOfAny(find, i);
			if (num == -1)
			{
				break;
			}
			for (; i < num; i++)
			{
				Write(value[i]);
			}
			char c = value[i];
			int num2 = find.Length - 1;
			while (0 <= num2)
			{
				if (find[num2] == c)
				{
					Write(replace[num2]);
					break;
				}
				num2--;
			}
		}
		if (i == 0)
		{
			Write(value);
			return;
		}
		for (; i < length; i++)
		{
			Write(value[i]);
		}
	}

	private void WriteCData(string value)
	{
		Write(value.Replace("]]>", "]]]]><![CDATA[>"));
	}

	private void WriteAttributes(ArrayList list, int count, HtmlElementProps htmlElementsProps)
	{
		for (int i = 0; i < count; i++)
		{
			BuilderInfo builderInfo = (BuilderInfo)list[i];
			string value = builderInfo.Value;
			bool flag = false;
			bool flag2 = false;
			if (htmlElementsProps != null && builderInfo.Prefix.Length == 0)
			{
				HtmlAttributeProps htmlAttributeProps = builderInfo.htmlAttrProps;
				if (htmlAttributeProps == null && builderInfo.search)
				{
					htmlAttributeProps = HtmlAttributeProps.GetProps(builderInfo.LocalName);
				}
				if (htmlAttributeProps != null)
				{
					flag = htmlElementsProps.AbrParent && htmlAttributeProps.Abr;
					flag2 = htmlElementsProps.UriParent && (htmlAttributeProps.Uri || (htmlElementsProps.NameParent && htmlAttributeProps.Name));
				}
			}
			Write(' ');
			WriteName(builderInfo.Prefix, builderInfo.LocalName);
			if (!flag || string.Compare(builderInfo.LocalName, value, StringComparison.OrdinalIgnoreCase) != 0)
			{
				Write("=\"");
				if (flag2)
				{
					WriteHtmlUri(value);
				}
				else if (isHtmlOutput)
				{
					WriteHtmlAttributeValue(value);
				}
				else
				{
					WriteXmlAttributeValue(value);
				}
				Write('"');
			}
		}
	}

	private void Indent(RecordBuilder record)
	{
		if (!record.Manager.CurrentElementScope.Mixed)
		{
			Indent(record.MainNode.Depth);
		}
	}

	private void Indent(int depth)
	{
		if (firstLine)
		{
			if (indentOutput)
			{
				firstLine = false;
			}
			return;
		}
		Write("\r\n");
		int num = 2 * depth;
		while (0 < num)
		{
			Write(" ");
			num--;
		}
	}

	internal abstract void Write(char outputChar);

	internal abstract void Write(string outputText);

	internal abstract void Close();
}
