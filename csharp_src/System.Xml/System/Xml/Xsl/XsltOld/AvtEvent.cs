namespace System.Xml.Xsl.XsltOld;

internal sealed class AvtEvent : TextEvent
{
	private int key;

	public AvtEvent(int key)
	{
		this.key = key;
	}

	public override bool Output(Processor processor, ActionFrame frame)
	{
		return processor.TextEvent(processor.EvaluateString(frame, key));
	}

	public override string Evaluate(Processor processor, ActionFrame frame)
	{
		return processor.EvaluateString(frame, key);
	}
}
