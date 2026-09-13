using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation.AixStructs;

[StructLayout(LayoutKind.Explicit, Size = 16)]
internal struct ifconf
{
	[FieldOffset(0)]
	public int ifc_len;

	[FieldOffset(8)]
	public IntPtr ifc_buf;
}
