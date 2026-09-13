namespace System.Xml.Xsl.XsltOld.Debugger;

internal interface IXsltProcessor
{
	int StackDepth { get; }

	IStackFrame GetStackFrame(int depth);
}
