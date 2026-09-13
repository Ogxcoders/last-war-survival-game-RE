using System.Text;

namespace System.Xml.Xsl.XsltOld;

internal class StringOutput : SequentialOutput
{
	private StringBuilder builder;

	private string result;

	internal string Result => result;

	internal StringOutput(Processor processor)
		: base(processor)
	{
		builder = new StringBuilder();
	}

	internal override void Write(char outputChar)
	{
		builder.Append(outputChar);
	}

	internal override void Write(string outputText)
	{
		builder.Append(outputText);
	}

	internal override void Close()
	{
		result = builder.ToString();
	}
}
