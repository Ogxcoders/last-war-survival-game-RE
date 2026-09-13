using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation.AixStructs;

[StructLayout(LayoutKind.Explicit, Size = 20)]
internal struct ifreq_flags
{
	[FieldOffset(0)]
	public unsafe fixed byte ifr_name[16];

	[FieldOffset(16)]
	public uint ifru_flags;
}
