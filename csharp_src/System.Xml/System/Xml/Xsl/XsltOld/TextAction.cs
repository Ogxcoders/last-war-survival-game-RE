using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class TextAction : CompiledAction
{
	private bool disableOutputEscaping;

	private string text;

	internal override void Compile(Compiler compiler)
	{
		CompileAttributes(compiler);
		CompileContent(compiler);
	}

	internal override bool CompileAttribute(Compiler compiler)
	{
		string localName = compiler.Input.LocalName;
		string value = compiler.Input.Value;
		if (Ref.Equal(localName, compiler.Atoms.DisableOutputEscaping))
		{
			disableOutputEscaping = compiler.GetYesNo(value);
			return true;
		}
		return false;
	}

	private void CompileContent(Compiler compiler)
	{
		if (!compiler.Recurse())
		{
			return;
		}
		NavigatorInput input = compiler.Input;
		text = string.Empty;
		do
		{
			switch (input.NodeType)
			{
			case XPathNodeType.Text:
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				text += input.Value;
				break;
			default:
				throw compiler.UnexpectedKeyword();
			case XPathNodeType.ProcessingInstruction:
			case XPathNodeType.Comment:
				break;
			}
		}
		while (compiler.Advance());
		compiler.ToParent();
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		if (frame.State == 0 && processor.TextEvent(text, disableOutputEscaping))
		{
			frame.Finished();
		}
	}
}
