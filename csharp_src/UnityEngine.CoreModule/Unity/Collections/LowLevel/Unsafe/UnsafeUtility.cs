using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace Unity.Collections.LowLevel.Unsafe;

[StaticAccessor("UnsafeUtility", StaticAccessorType.DoubleColon)]
[NativeHeader("Runtime/Export/Unsafe/UnsafeUtility.bindings.h")]
public static class UnsafeUtility
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct IsUnmanagedCache<T>
	{
		internal static int value;
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct IsValidNativeContainerElementTypeCache<T>
	{
		internal static int value;
	}

	private struct AlignOfHelper<T> where T : struct
	{
		public byte dummy;

		public T data;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private static extern int GetFieldOffsetInStruct(FieldInfo field);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private static extern int GetFieldOffsetInClass(FieldInfo field);

	public static int GetFieldOffset(FieldInfo field)
	{
		if (field.DeclaringType.IsValueType)
		{
			return GetFieldOffsetInStruct(field);
		}
		if (field.DeclaringType.IsClass)
		{
			return GetFieldOffsetInClass(field);
		}
		return -1;
	}

	public unsafe static void* PinGCObjectAndGetAddress(object target, out ulong gcHandle)
	{
		return PinSystemObjectAndGetAddress(target, out gcHandle);
	}

	public unsafe static void* PinGCArrayAndGetDataAddress(Array target, out ulong gcHandle)
	{
		return PinSystemArrayAndGetAddress(target, out gcHandle);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private unsafe static extern void* PinSystemArrayAndGetAddress(object target, out ulong gcHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	private unsafe static extern void* PinSystemObjectAndGetAddress(object target, out ulong gcHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern void ReleaseGCObject(ulong gcHandle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void CopyObjectAddressToPtr(object target, void* dstPtr);

	public static bool IsBlittable<T>() where T : struct
	{
		return IsBlittable(typeof(T));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void* Malloc(long size, int alignment, Allocator allocator);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void Free(void* memory, Allocator allocator);

	public static bool IsValidAllocator(Allocator allocator)
	{
		return allocator > Allocator.None;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void MemCpy(void* destination, void* source, long size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void MemCpyReplicate(void* destination, void* source, int size, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void MemCpyStride(void* destination, int destinationStride, void* source, int sourceStride, int elementSize, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void MemMove(void* destination, void* source, long size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern void MemSet(void* destination, byte value, long size);

	public unsafe static void MemClear(void* destination, long size)
	{
		MemSet(destination, 0, size);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public unsafe static extern int MemCmp(void* ptr1, void* ptr2, long size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern int SizeOf(Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern bool IsBlittable(Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern bool IsUnmanaged(Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	public static extern bool IsValidNativeContainerElementType(Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal static extern void LogError(string msg, string filename, int linenumber);

	private static bool IsBlittableValueType(Type t)
	{
		return t.IsValueType && IsBlittable(t);
	}

	private static string GetReasonForTypeNonBlittableImpl(Type t, string name)
	{
		if (!t.IsValueType)
		{
			return $"{name} is not blittable because it is not of value type ({t})\n";
		}
		if (t.IsPrimitive)
		{
			return $"{name} is not blittable ({t})\n";
		}
		string text = "";
		FieldInfo[] fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!IsBlittableValueType(fieldInfo.FieldType))
			{
				text += GetReasonForTypeNonBlittableImpl(fieldInfo.FieldType, $"{name}.{fieldInfo.Name}");
			}
		}
		return text;
	}

	internal static bool IsArrayBlittable(Array arr)
	{
		return IsBlittableValueType(arr.GetType().GetElementType());
	}

	internal static bool IsGenericListBlittable<T>() where T : struct
	{
		return IsBlittable<T>();
	}

	internal static string GetReasonForArrayNonBlittable(Array arr)
	{
		Type elementType = arr.GetType().GetElementType();
		return GetReasonForTypeNonBlittableImpl(elementType, elementType.Name);
	}

	internal static string GetReasonForGenericListNonBlittable<T>() where T : struct
	{
		Type typeFromHandle = typeof(T);
		return GetReasonForTypeNonBlittableImpl(typeFromHandle, typeFromHandle.Name);
	}

	internal static string GetReasonForTypeNonBlittable(Type t)
	{
		return GetReasonForTypeNonBlittableImpl(t, t.Name);
	}

	internal static string GetReasonForValueTypeNonBlittable<T>() where T : struct
	{
		Type typeFromHandle = typeof(T);
		return GetReasonForTypeNonBlittableImpl(typeFromHandle, typeFromHandle.Name);
	}

	public static bool IsUnmanaged<T>()
	{
		int num = IsUnmanagedCache<T>.value;
		switch (num)
		{
		case 1:
			return true;
		case 0:
			num = (IsUnmanagedCache<T>.value = (IsUnmanaged(typeof(T)) ? 1 : (-1)));
			break;
		}
		return num == 1;
	}

	public static bool IsValidNativeContainerElementType<T>()
	{
		int num = IsValidNativeContainerElementTypeCache<T>.value;
		switch (num)
		{
		case -1:
			return false;
		case 0:
			num = (IsValidNativeContainerElementTypeCache<T>.value = (IsValidNativeContainerElementType(typeof(T)) ? 1 : (-1)));
			break;
		}
		return num == 1;
	}

	public static int AlignOf<T>() where T : struct
	{
		return SizeOf<AlignOfHelper<T>>() - SizeOf<T>();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void CopyPtrToStructure<T>(void* ptr, out T output) where T : struct
	{
		InternalCopyPtrToStructure<T>(ptr, out output);
	}

	private unsafe static void InternalCopyPtrToStructure<T>(void* ptr, out T output) where T : struct
	{
		output = System.Runtime.CompilerServices.Unsafe.Read<T>(ptr);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static void CopyStructureToPtr<T>(ref T input, void* ptr) where T : struct
	{
		InternalCopyStructureToPtr(ref input, ptr);
	}

	private unsafe static void InternalCopyStructureToPtr<T>(ref T input, void* ptr) where T : struct
	{
		System.Runtime.CompilerServices.Unsafe.Write(ptr, input);
	}

	public unsafe static T ReadArrayElement<T>(void* source, int index)
	{
		return ((T*)source)[index];
	}

	public unsafe static T ReadArrayElementWithStride<T>(void* source, int index, int stride)
	{
		return System.Runtime.CompilerServices.Unsafe.Read<T>((byte*)source + (long)index * (long)stride);
	}

	public unsafe static void WriteArrayElement<T>(void* destination, int index, T value)
	{
		System.Runtime.CompilerServices.Unsafe.Write((byte*)destination + (long)index * (long)System.Runtime.CompilerServices.Unsafe.SizeOf<T>(), value);
	}

	public unsafe static void WriteArrayElementWithStride<T>(void* destination, int index, int stride, T value)
	{
		System.Runtime.CompilerServices.Unsafe.Write((byte*)destination + (long)index * (long)stride, value);
	}

	public unsafe static void* AddressOf<T>(ref T output) where T : struct
	{
		return System.Runtime.CompilerServices.Unsafe.AsPointer(ref output);
	}

	public static int SizeOf<T>() where T : struct
	{
		return System.Runtime.CompilerServices.Unsafe.SizeOf<T>();
	}

	public static int EnumToInt<T>(T enumValue) where T : struct, IConvertible
	{
		int intValue = 0;
		InternalEnumToInt(ref enumValue, ref intValue);
		return intValue;
	}

	private static void InternalEnumToInt<T>(ref T enumValue, ref int intValue)
	{
		intValue = System.Runtime.CompilerServices.Unsafe.As<T, int>(ref enumValue);
	}
}
