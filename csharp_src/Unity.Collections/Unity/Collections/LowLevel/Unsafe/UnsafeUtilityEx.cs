using System.Runtime.CompilerServices;

namespace Unity.Collections.LowLevel.Unsafe;

public static class UnsafeUtilityEx
{
	public unsafe static ref T AsRef<T>(void* ptr) where T : struct
	{
		return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>(ptr);
	}

	public unsafe static ref T ArrayElementAsRef<T>(void* ptr, int index) where T : struct
	{
		return ref System.Runtime.CompilerServices.Unsafe.AsRef<T>((byte*)ptr + index * UnsafeUtility.SizeOf<T>());
	}

	public unsafe static void* RestrictNoAlias(void* ptr)
	{
		return ptr;
	}

	public unsafe static void MemSet(void* destination, byte value, int count)
	{
		for (int i = 0; i < count; i++)
		{
			((sbyte*)destination)[i] = (sbyte)value;
		}
	}
}
