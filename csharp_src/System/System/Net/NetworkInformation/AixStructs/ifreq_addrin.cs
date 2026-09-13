using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation.AixStructs;

[StructLayout(LayoutKind.Explicit, Size = 24)]
internal struct ifreq_addrin
{
	[FieldOffset(0)]
	public unsafe fixed byte ifr_name[16];

	[FieldOffset(16)]
	public sockaddr_in ifru_addr;
}
