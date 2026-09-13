using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 16)]
public struct LuaTValue64
{
	[FieldOffset(0)]
	public ulong u64;

	[FieldOffset(0)]
	public double n;

	[FieldOffset(0)]
	public long i;

	[FieldOffset(8)]
	public byte tt_;
}
