using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public struct LuaTValue32
{
	[FieldOffset(0)]
	public IntPtr gc;

	[FieldOffset(0)]
	public int b;

	[FieldOffset(0)]
	public IntPtr f;

	[FieldOffset(0)]
	public float n;

	[FieldOffset(0)]
	public int i;

	[FieldOffset(4)]
	public byte tt_;
}
