using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class NavigatorOutput : RecordOutput
{
	private XPathDocument doc;

	private int documentIndex;

	private XmlRawWriter wr;

	internal XPathNavigator Navigator => ((IXPathNavigable)doc).CreateNavigator();

	internal NavigatorOutput(string baseUri)
	{
		doc = new XPathDocument();
		wr = doc.LoadFromWriter(XPathDocument.LoadFlags.AtomizeNames, baseUri);
	}

	public Processor.OutputResult RecordDone(RecordBuilder record)
	{
		BuilderInfo mainNode = record.MainNode;
		documentIndex++;
		switch (mainNode.NodeType)
		{
		case XmlNodeType.Element:
		{
			wr.WriteStartElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
			for (int i = 0; i < record.AttributeCount; i++)
			{
				documentIndex++;
				BuilderInfo builderInfo = (BuilderInfo)record.AttributeList[i];
				if (builderInfo.NamespaceURI == "http://www.w3.org/2000/xmlns/")
				{
					if (builderInfo.Prefix.Length == 0)
					{
						wr.WriteNamespaceDeclaration(string.Empty, builderInfo.Value);
					}
					else
					{
						wr.WriteNamespaceDeclaration(builderInfo.LocalName, builderInfo.Value);
					}
				}
				else
				{
					wr.WriteAttributeString(builderInfo.Prefix, builderInfo.LocalName, builderInfo.NamespaceURI, builderInfo.Value);
				}
			}
			wr.StartElementContent();
			if (mainNode.IsEmptyTag)
			{
				wr.WriteEndElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
			}
			break;
		}
		case XmlNodeType.Text:
			wr.WriteString(mainNode.Value);
			break;
		case XmlNodeType.SignificantWhitespace:
			wr.WriteString(mainNode.Value);
			break;
		case XmlNodeType.ProcessingInstruction:
			wr.WriteProcessingInstruction(mainNode.LocalName, mainNode.Value);
			break;
		case XmlNodeType.Comment:
			wr.WriteComment(mainNode.Value);
			break;
		case XmlNodeType.EndElement:
			wr.WriteEndElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
			break;
		}
		record.Reset();
		return Processor.OutputResult.Continue;
	}

	public void TheEnd()
	{
		wr.Close();
	}
}
