using System.Collections;

namespace System.Xml.Xsl.XsltOld;

internal class CopyCodeAction : Action
{
	private const int Outputting = 2;

	private ArrayList copyEvents;

	internal CopyCodeAction()
	{
		copyEvents = new ArrayList();
	}

	internal void AddEvent(Event copyEvent)
	{
		copyEvents.Add(copyEvent);
	}

	internal void AddEvents(ArrayList copyEvents)
	{
		this.copyEvents.AddRange(copyEvents);
	}

	internal override void ReplaceNamespaceAlias(Compiler compiler)
	{
		int count = copyEvents.Count;
		for (int i = 0; i < count; i++)
		{
			((Event)copyEvents[i]).ReplaceNamespaceAlias(compiler);
		}
	}

	internal override void Execute(Processor processor, ActionFrame frame)
	{
		switch (frame.State)
		{
		default:
			return;
		case 0:
			frame.Counter = 0;
			frame.State = 2;
			break;
		case 2:
			break;
		}
		while (processor.CanContinue && ((Event)copyEvents[frame.Counter]).Output(processor, frame))
		{
			if (frame.IncrementCounter() >= copyEvents.Count)
			{
				frame.Finished();
				break;
			}
		}
	}

	internal override DbgData GetDbgData(ActionFrame frame)
	{
		return ((Event)copyEvents[frame.Counter]).DbgData;
	}
}
