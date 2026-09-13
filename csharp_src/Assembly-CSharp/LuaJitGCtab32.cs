using System;

public struct LuaJitGCtab32
{
	public IntPtr nextgc;

	public uint masks;

	public unsafe LuaJitTValue* array;

	public IntPtr gclist;

	public IntPtr metatable;

	public IntPtr node;

	public uint asize;

	public uint hmask;
}
