namespace System.Xml.Xsl.XsltOld;

internal abstract class Action
{
	internal const int Initialized = 0;

	internal const int Finished = -1;

	internal abstract void Execute(Processor processor, ActionFrame frame);

	internal virtual void ReplaceNamespaceAlias(Compiler compiler)
	{
	}

	internal virtual DbgData GetDbgData(ActionFrame frame)
	{
		return DbgData.Empty;
	}
}
