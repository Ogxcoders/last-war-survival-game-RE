using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct AndroidTzDataEntry
{
	public unsafe fixed byte id[40];

	public int byteOffset;

	public int length;

	public int rawUtcOffset;
}
