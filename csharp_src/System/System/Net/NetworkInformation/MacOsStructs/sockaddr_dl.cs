using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation.MacOsStructs;

internal struct sockaddr_dl
{
	public byte sdl_len;

	public byte sdl_family;

	public ushort sdl_index;

	public byte sdl_type;

	public byte sdl_nlen;

	public byte sdl_alen;

	public byte sdl_slen;

	public byte[] sdl_data;

	internal void Read(IntPtr ptr)
	{
		sdl_len = Marshal.ReadByte(ptr, 0);
		sdl_family = Marshal.ReadByte(ptr, 1);
		sdl_index = (ushort)Marshal.ReadInt16(ptr, 2);
		sdl_type = Marshal.ReadByte(ptr, 4);
		sdl_nlen = Marshal.ReadByte(ptr, 5);
		sdl_alen = Marshal.ReadByte(ptr, 6);
		sdl_slen = Marshal.ReadByte(ptr, 7);
		sdl_data = new byte[Math.Max(12, sdl_len - 8)];
		Marshal.Copy(new IntPtr(ptr.ToInt64() + 8), sdl_data, 0, sdl_data.Length);
	}
}
