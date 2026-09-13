namespace System.Xml.Xsl.XsltOld;

internal class TextEvent : Event
{
	private string text;

	protected TextEvent()
	{
	}

	public TextEvent(string text)
	{
		this.text = text;
	}

	public TextEvent(Compiler compiler)
	{
		NavigatorInput input = compiler.Input;
		text = input.Value;
	}

	public override bool Output(Processor processor, ActionFrame frame)
	{
		return processor.TextEvent(text);
	}

	public virtual string Evaluate(Processor processor, ActionFrame frame)
	{
		return text;
	}
}
