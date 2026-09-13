using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld.Debugger;

internal interface IStackFrame
{
	XPathNavigator Instruction { get; }

	XPathNodeIterator NodeSet { get; }

	int GetVariablesCount();

	XPathNavigator GetVariable(int varIndex);

	object GetVariableValue(int varIndex);
}
