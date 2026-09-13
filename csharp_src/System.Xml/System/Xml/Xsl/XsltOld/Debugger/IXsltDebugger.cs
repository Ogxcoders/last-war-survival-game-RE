using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld.Debugger;

internal interface IXsltDebugger
{
	string GetBuiltInTemplatesUri();

	void OnInstructionCompile(XPathNavigator styleSheetNavigator);

	void OnInstructionExecute(IXsltProcessor xsltProcessor);
}
