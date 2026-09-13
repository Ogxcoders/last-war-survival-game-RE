namespace System;

internal class TimeType
{
	public readonly int Offset;

	public readonly bool IsDst;

	public string Name;

	public TimeType(int offset, bool is_dst, string abbrev)
	{
		Offset = offset;
		IsDst = is_dst;
		Name = abbrev;
	}

	public override string ToString()
	{
		return "offset: " + Offset + "s, is_dst: " + IsDst + ", zone name: " + Name;
	}
}
