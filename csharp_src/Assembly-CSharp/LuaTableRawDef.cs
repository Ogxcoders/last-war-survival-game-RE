using System;

public struct LuaTableRawDef
{
	public IntPtr next;

	public uint bytes;

	public uint sizearray;

	public IntPtr alimit;

	public IntPtr node;

	public IntPtr lastfree;

	public IntPtr metatable;

	public IntPtr gclist;
}
