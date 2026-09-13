using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld;

internal class DbgData
{
	private XPathNavigator styleSheet;

	private VariableAction[] variables;

	private static DbgData s_nullDbgData = new DbgData();

	public XPathNavigator StyleSheet => styleSheet;

	public VariableAction[] Variables => variables;

	public static DbgData Empty => s_nullDbgData;

	public DbgData(Compiler compiler)
	{
		DbgCompiler dbgCompiler = (DbgCompiler)compiler;
		styleSheet = dbgCompiler.Input.Navigator.Clone();
		variables = dbgCompiler.LocalVariables;
		dbgCompiler.Debugger.OnInstructionCompile(StyleSheet);
	}

	internal void ReplaceVariables(VariableAction[] vars)
	{
		variables = vars;
	}

	private DbgData()
	{
		styleSheet = null;
		variables = new VariableAction[0];
	}
}
