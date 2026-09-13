using System.Diagnostics;

namespace System.Xml.Xsl;

[DebuggerDisplay("({Line},{Pos})")]
internal struct Location
{
	private ulong value;

	public int Line => (int)(value >> 32);

	public int Pos => (int)value;

	public Location(int line, int pos)
	{
		value = (ulong)(((long)line << 32) | (uint)pos);
	}

	public Location(Location that)
	{
		value = that.value;
	}

	public bool LessOrEqual(Location that)
	{
		return value <= that.value;
	}
}
