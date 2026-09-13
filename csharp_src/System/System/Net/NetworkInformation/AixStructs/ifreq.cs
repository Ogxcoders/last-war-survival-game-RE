using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation.AixStructs;

[StructLayout(LayoutKind.Explicit, Size = 18)]
internal struct ifreq
{
	[FieldOffset(0)]
	public unsafe fixed byte ifr_name[16];

	[FieldOffset(16)]
	public sockaddr ifru_addr;
}
