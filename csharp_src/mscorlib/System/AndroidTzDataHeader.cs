using System.Runtime.InteropServices;

namespace System;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct AndroidTzDataHeader
{
	public unsafe fixed byte signature[12];

	public int indexOffset;

	public int dataOffset;

	public int zoneTabOffset;
}
