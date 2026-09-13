using System.Runtime.CompilerServices;

namespace System.Runtime;

internal static class RuntimeImports
{
	internal unsafe static void RhZeroMemory(ref byte b, ulong byteLength)
	{
		fixed (byte* p = &b)
		{
			ZeroMemory(p, (uint)byteLength);
		}
	}

	internal unsafe static void RhZeroMemory(IntPtr p, UIntPtr byteLength)
	{
		ZeroMemory((void*)p, (uint)byteLength);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void ZeroMemory(void* p, uint byteLength);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal unsafe static extern void Memmove(byte* dest, byte* src, uint len);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal unsafe static extern void Memmove_wbarrier(byte* dest, byte* src, uint len, IntPtr type_handle);
}
