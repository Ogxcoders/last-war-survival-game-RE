using System.IO;

namespace System.Xml.Xsl.XsltOld;

internal class TextOutput : SequentialOutput
{
	private TextWriter writer;

	internal TextOutput(Processor processor, Stream stream)
		: base(processor)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		encoding = processor.Output.Encoding;
		writer = new StreamWriter(stream, encoding);
	}

	internal TextOutput(Processor processor, TextWriter writer)
		: base(processor)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		encoding = writer.Encoding;
		this.writer = writer;
	}

	internal override void Write(char outputChar)
	{
		writer.Write(outputChar);
	}

	internal override void Write(string outputText)
	{
		writer.Write(outputText);
	}

	internal override void Close()
	{
		writer.Flush();
		writer = null;
	}
}
