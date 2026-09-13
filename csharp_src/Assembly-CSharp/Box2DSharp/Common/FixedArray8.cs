using System;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public struct FixedArray8<T> where T : unmanaged
{
	public T Value0;

	public T Value1;

	public T Value2;

	public T Value3;

	public T Value4;

	public T Value5;

	public T Value6;

	public T Value7;

	public const int Length = 8;

	public unsafe ref T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index > -1 && index < 8)
			{
				return ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref Value0), index));
			}
			throw new IndexOutOfRangeException();
		}
	}
}
