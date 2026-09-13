using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using UnityEngine.Internal;

namespace Unity.Collections;

[DebuggerTypeProxy(typeof(NativeArrayDebugView<>))]
[NativeContainer]
[NativeContainerSupportsMinMaxWriteRestriction]
[NativeContainerSupportsDeallocateOnJobCompletion]
[NativeContainerSupportsDeferredConvertListToArray]
[DebuggerDisplay("Length = {Length}")]
public struct NativeArray<T> : IDisposable, IEnumerable<T>, IEnumerable, IEquatable<NativeArray<T>> where T : struct
{
	private struct DisposeJob : IJob
	{
		public NativeArray<T> Container;

		public void Execute()
		{
			Container.Deallocate();
		}
	}

	[ExcludeFromDocs]
	public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private NativeArray<T> m_Array;

		private int m_Index;

		public T Current => m_Array[m_Index];

		object IEnumerator.Current => Current;

		public Enumerator(ref NativeArray<T> array)
		{
			m_Array = array;
			m_Index = -1;
		}

		public void Dispose()
		{
		}

		public bool MoveNext()
		{
			m_Index++;
			return m_Index < m_Array.Length;
		}

		public void Reset()
		{
			m_Index = -1;
		}
	}

	[NativeDisableUnsafePtrRestriction]
	internal unsafe void* m_Buffer;

	internal int m_Length;

	internal Allocator m_AllocatorLabel;

	public int Length => m_Length;

	public unsafe T this[int index]
	{
		get
		{
			return UnsafeUtility.ReadArrayElement<T>(m_Buffer, index);
		}
		[WriteAccessRequired]
		set
		{
			UnsafeUtility.WriteArrayElement(m_Buffer, index, value);
		}
	}

	public unsafe bool IsCreated => m_Buffer != null;

	public unsafe NativeArray(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory)
	{
		Allocate(length, allocator, out this);
		if ((options & NativeArrayOptions.ClearMemory) == NativeArrayOptions.ClearMemory)
		{
			UnsafeUtility.MemClear(m_Buffer, (long)Length * (long)UnsafeUtility.SizeOf<T>());
		}
	}

	public NativeArray(T[] array, Allocator allocator)
	{
		Allocate(array.Length, allocator, out this);
		Copy(array, this);
	}

	public NativeArray(NativeArray<T> array, Allocator allocator)
	{
		Allocate(array.Length, allocator, out this);
		Copy(array, this);
	}

	private unsafe static void Allocate(int length, Allocator allocator, out NativeArray<T> array)
	{
		long size = (long)UnsafeUtility.SizeOf<T>() * (long)length;
		array = default(NativeArray<T>);
		array.m_Buffer = UnsafeUtility.Malloc(size, UnsafeUtility.AlignOf<T>(), allocator);
		array.m_Length = length;
		array.m_AllocatorLabel = allocator;
	}

	[BurstDiscard]
	internal static void IsUnmanagedAndThrow()
	{
		if (!UnsafeUtility.IsValidNativeContainerElementType<T>())
		{
			throw new InvalidOperationException($"{typeof(T)} used in NativeArray<{typeof(T)}> must be unmanaged (contain no managed types) and cannot itself be a native container type.");
		}
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckElementReadAccess(int index)
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckElementWriteAccess(int index)
	{
	}

	private unsafe void Deallocate()
	{
		UnsafeUtility.Free(m_Buffer, m_AllocatorLabel);
		m_Buffer = null;
		m_Length = 0;
	}

	[WriteAccessRequired]
	public void Dispose()
	{
		Deallocate();
	}

	public unsafe JobHandle Dispose(JobHandle inputDeps)
	{
		JobHandle result = new DisposeJob
		{
			Container = this
		}.Schedule(inputDeps);
		m_Buffer = null;
		m_Length = 0;
		return result;
	}

	[WriteAccessRequired]
	public void CopyFrom(T[] array)
	{
		Copy(array, this);
	}

	[WriteAccessRequired]
	public void CopyFrom(NativeArray<T> array)
	{
		Copy(array, this);
	}

	public void CopyTo(T[] array)
	{
		Copy(this, array);
	}

	public void CopyTo(NativeArray<T> array)
	{
		Copy(this, array);
	}

	public T[] ToArray()
	{
		T[] array = new T[Length];
		Copy(this, array, Length);
		return array;
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(ref this);
	}

	IEnumerator<T> IEnumerable<T>.GetEnumerator()
	{
		return new Enumerator(ref this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public unsafe bool Equals(NativeArray<T> other)
	{
		return m_Buffer == other.m_Buffer && m_Length == other.m_Length;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is NativeArray<T> && Equals((NativeArray<T>)obj);
	}

	public unsafe override int GetHashCode()
	{
		return ((int)m_Buffer * 397) ^ m_Length;
	}

	public static bool operator ==(NativeArray<T> left, NativeArray<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(NativeArray<T> left, NativeArray<T> right)
	{
		return !left.Equals(right);
	}

	public static void Copy(NativeArray<T> src, NativeArray<T> dst)
	{
		Copy(src, 0, dst, 0, src.Length);
	}

	public static void Copy(T[] src, NativeArray<T> dst)
	{
		Copy(src, 0, dst, 0, src.Length);
	}

	public static void Copy(NativeArray<T> src, T[] dst)
	{
		Copy(src, 0, dst, 0, src.Length);
	}

	public static void Copy(NativeArray<T> src, NativeArray<T> dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public static void Copy(T[] src, NativeArray<T> dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public static void Copy(NativeArray<T> src, T[] dst, int length)
	{
		Copy(src, 0, dst, 0, length);
	}

	public unsafe static void Copy(NativeArray<T> src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
	{
		UnsafeUtility.MemCpy((byte*)dst.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>(), (byte*)src.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>(), length * UnsafeUtility.SizeOf<T>());
	}

	public unsafe static void Copy(T[] src, int srcIndex, NativeArray<T> dst, int dstIndex, int length)
	{
		GCHandle gCHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
		IntPtr intPtr = gCHandle.AddrOfPinnedObject();
		UnsafeUtility.MemCpy((byte*)dst.m_Buffer + dstIndex * UnsafeUtility.SizeOf<T>(), (byte*)(void*)intPtr + srcIndex * UnsafeUtility.SizeOf<T>(), length * UnsafeUtility.SizeOf<T>());
		gCHandle.Free();
	}

	public unsafe static void Copy(NativeArray<T> src, int srcIndex, T[] dst, int dstIndex, int length)
	{
		GCHandle gCHandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
		IntPtr intPtr = gCHandle.AddrOfPinnedObject();
		UnsafeUtility.MemCpy((byte*)(void*)intPtr + dstIndex * UnsafeUtility.SizeOf<T>(), (byte*)src.m_Buffer + srcIndex * UnsafeUtility.SizeOf<T>(), length * UnsafeUtility.SizeOf<T>());
		gCHandle.Free();
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckReinterpretLoadRange<U>(int sourceIndex) where U : struct
	{
	}

	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	private void CheckReinterpretStoreRange<U>(int destIndex) where U : struct
	{
	}

	public unsafe U ReinterpretLoad<U>(int sourceIndex) where U : struct
	{
		byte* source = (byte*)m_Buffer + (long)UnsafeUtility.SizeOf<T>() * (long)sourceIndex;
		return UnsafeUtility.ReadArrayElement<U>(source, 0);
	}

	public unsafe void ReinterpretStore<U>(int destIndex, U data) where U : struct
	{
		byte* destination = (byte*)m_Buffer + (long)UnsafeUtility.SizeOf<T>() * (long)destIndex;
		UnsafeUtility.WriteArrayElement(destination, 0, data);
	}

	private unsafe NativeArray<U> InternalReinterpret<U>(int length) where U : struct
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<U>(m_Buffer, length, m_AllocatorLabel);
	}

	public NativeArray<U> Reinterpret<U>() where U : struct
	{
		return InternalReinterpret<U>(Length);
	}

	public NativeArray<U> Reinterpret<U>(int expectedTypeSize) where U : struct
	{
		long num = UnsafeUtility.SizeOf<T>();
		long num2 = UnsafeUtility.SizeOf<U>();
		long num3 = Length * num;
		long num4 = num3 / num2;
		return InternalReinterpret<U>((int)num4);
	}

	public unsafe NativeArray<T> GetSubArray(int start, int length)
	{
		return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((byte*)m_Buffer + (long)UnsafeUtility.SizeOf<T>() * (long)start, length, Allocator.Invalid);
	}
}
