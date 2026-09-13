using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct LuaJitTValue
{
	[FieldOffset(0)]
	public ulong u64;

	[FieldOffset(0)]
	public double n;

	[FieldOffset(0)]
	public int i;

	[FieldOffset(0)]
	public long it64;

	[FieldOffset(4)]
	public uint it;
}
