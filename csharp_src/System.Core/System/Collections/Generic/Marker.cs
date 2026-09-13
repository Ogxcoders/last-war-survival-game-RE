using System.Diagnostics;

namespace System.Collections.Generic;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
internal readonly struct Marker
{
	public int Count { get; }

	public int Index { get; }

	private string DebuggerDisplay => string.Format("{0}: {1}, {2}: {3}", "Index", Index, "Count", Count);

	public Marker(int count, int index)
	{
		Count = count;
		Index = index;
	}
}
