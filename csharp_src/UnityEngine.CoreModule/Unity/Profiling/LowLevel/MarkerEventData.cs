using System.Runtime.InteropServices;

namespace Unity.Profiling.LowLevel;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct MarkerEventData
{
	[FieldOffset(0)]
	public byte type;

	[FieldOffset(1)]
	public byte reserved0;

	[FieldOffset(2)]
	public ushort reserved1;

	[FieldOffset(4)]
	public uint size;

	[FieldOffset(8)]
	public unsafe void* ptr;
}
