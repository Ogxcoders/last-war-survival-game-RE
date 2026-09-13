using System.Collections;

namespace System.Xml.Xsl.XsltOld;

internal class WriterOutput : RecordOutput
{
	private XmlWriter writer;

	private Processor processor;

	internal WriterOutput(Processor processor, XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		this.writer = writer;
		this.processor = processor;
	}

	public Processor.OutputResult RecordDone(RecordBuilder record)
	{
		BuilderInfo mainNode = record.MainNode;
		switch (mainNode.NodeType)
		{
		case XmlNodeType.Element:
			writer.WriteStartElement(mainNode.Prefix, mainNode.LocalName, mainNode.NamespaceURI);
			WriteAttributes(record.AttributeList, record.AttributeCount);
			if (mainNode.IsEmptyTag)
			{
				writer.WriteEndElement();
			}
			break;
		case XmlNodeType.Text:
		case XmlNodeType.Whitespace:
		case XmlNodeType.SignificantWhitespace:
			writer.WriteString(mainNode.Value);
			break;
		case XmlNodeType.CDATA:
			writer.WriteCData(mainNode.Value);
			break;
		case XmlNodeType.EntityReference:
			writer.WriteEntityRef(mainNode.LocalName);
			break;
		case XmlNodeType.ProcessingInstruction:
			writer.WriteProcessingInstruction(mainNode.LocalName, mainNode.Value);
			break;
		case XmlNodeType.Comment:
			writer.WriteComment(mainNode.Value);
			break;
		case XmlNodeType.DocumentType:
			writer.WriteRaw(mainNode.Value);
			break;
		case XmlNodeType.EndElement:
			writer.WriteFullEndElement();
			break;
		}
		record.Reset();
		return Processor.OutputResult.Continue;
	}

	public void TheEnd()
	{
		writer.Flush();
		writer = null;
	}

	private void WriteAttributes(ArrayList list, int count)
	{
		for (int i = 0; i < count; i++)
		{
			BuilderInfo builderInfo = (BuilderInfo)list[i];
			writer.WriteAttributeString(builderInfo.Prefix, builderInfo.LocalName, builderInfo.NamespaceURI, builderInfo.Value);
		}
	}
}
