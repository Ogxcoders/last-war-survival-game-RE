namespace System.Xml.Xsl.XsltOld;

internal class ApplyImportsAction : CompiledAction
{
	private XmlQualifiedName mode;

	private Stylesheet stylesheet;

	private const int TemplateProcessed = 2;

	internal override void Compile(Compiler compiler)
	{
		CheckEmpty(compiler);
		if (!compiler.CanHaveApplyImports)
		{
			throw XsltException.Create("The 'xsl:apply-imports' instruction cannot be included within the content of an 'xsl:for-each' instruction or within an 'xsl:template' instruction without the 'match' attribute.");
		}
		mode = compiler.CurrentMode;
		stylesheet = compiler.CompiledStylesheet;
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		case 0:
			processor.PushTemplateLookup(frame.NodeSet, mode, stylesheet);
			frame.State = 2;
			break;
		case 2:
			frame.Finished();
			break;
		}
	}
}
