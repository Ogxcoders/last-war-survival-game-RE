namespace System.Xml.Xsl.XsltOld;

internal abstract class Event
{
	internal virtual DbgData DbgData => DbgData.Empty;

	public virtual void ReplaceNamespaceAlias(Compiler compiler)
	{
	}

	public abstract bool Output(Processor processor, ActionFrame frame);

	internal void OnInstructionExecute(Processor processor)
	{
		processor.OnInstructionExecute();
	}
}
