using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Size = 32)]
public struct LuaJitNode
{
	[FieldOffset(0)]
	public LuaJitTValue val;

	[FieldOffset(8)]
	public LuaJitTValue key;

	[FieldOffset(16)]
	public ulong next64;

	[FieldOffset(16)]
	public uint next32;

	[FieldOffset(20)]
	public uint freetop;
}
