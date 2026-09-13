namespace System.Diagnostics;

public class SourceFilter : TraceFilter
{
	private string src;

	public string Source
	{
		get
		{
			return src;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("source");
			}
			src = value;
		}
	}

	public SourceFilter(string source)
	{
		Source = source;
	}

	public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
	{
		if (source == null)
		{
			throw new ArgumentNullException("source");
		}
		return string.Equals(src, source);
	}
}
