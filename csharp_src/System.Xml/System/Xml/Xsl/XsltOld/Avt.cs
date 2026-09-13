using System.Collections;
using System.Text;

namespace System.Xml.Xsl.XsltOld;

internal sealed class Avt
{
	private string constAvt;

	private TextEvent[] events;

	public bool IsConstant => events == null;

	private Avt(string constAvt)
	{
		this.constAvt = constAvt;
	}

	private Avt(ArrayList eventList)
	{
		events = new TextEvent[eventList.Count];
		for (int i = 0; i < eventList.Count; i++)
		{
			events[i] = (TextEvent)eventList[i];
		}
	}

	internal string Evaluate(Processor processor, ActionFrame frame)
	{
		if (IsConstant)
		{
			return constAvt;
		}
		StringBuilder sharedStringBuilder = processor.GetSharedStringBuilder();
		for (int i = 0; i < events.Length; i++)
		{
			sharedStringBuilder.Append(events[i].Evaluate(processor, frame));
		}
		processor.ReleaseSharedStringBuilder();
		return sharedStringBuilder.ToString();
	}

	internal static Avt CompileAvt(Compiler compiler, string avtText)
	{
		bool constant;
		ArrayList eventList = compiler.CompileAvt(avtText, out constant);
		if (!constant)
		{
			return new Avt(eventList);
		}
		return new Avt(avtText);
	}
}
