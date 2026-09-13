using System.Diagnostics;

namespace System.Collections.Generic;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
internal readonly struct CopyPosition
{
	public static System.Collections.Generic.CopyPosition Start => default(System.Collections.Generic.CopyPosition);

	internal int Row { get; }

	internal int Column { get; }

	private string DebuggerDisplay => $"[{Row}, {Column}]";

	internal CopyPosition(int row, int column)
	{
		Row = row;
		Column = column;
	}

	public System.Collections.Generic.CopyPosition Normalize(int endColumn)
	{
		if (Column != endColumn)
		{
			return this;
		}
		return new System.Collections.Generic.CopyPosition(Row + 1, 0);
	}
}
