using System.IO;

namespace System.Xml.Xsl.XsltOld;

internal class TextOnlyOutput : RecordOutput
{
	private Processor processor;

	private TextWriter writer;

	internal XsltOutput Output => processor.Output;

	public TextWriter Writer => writer;

	internal TextOnlyOutput(Processor processor, Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		this.processor = processor;
		writer = new StreamWriter(stream, Output.Encoding);
	}

	internal TextOnlyOutput(Processor processor, TextWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		this.processor = processor;
		this.writer = writer;
	}

	public Processor.OutputResult RecordDone(RecordBuilder record)
	{
		BuilderInfo mainNode = record.MainNode;
		XmlNodeType nodeType = mainNode.NodeType;
		if (nodeType == XmlNodeType.Text || (uint)(nodeType - 13) <= 1u)
		{
			writer.Write(mainNode.Value);
		}
		record.Reset();
		return Processor.OutputResult.Continue;
	}

	public void TheEnd()
	{
		writer.Flush();
	}
}
