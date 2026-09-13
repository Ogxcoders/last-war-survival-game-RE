using System;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public struct FixedArray2<T> where T : unmanaged
{
	public T Value0;

	public T Value1;

	public const int Length = 2;

	public unsafe ref T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index > -1 && index < 2)
			{
				return ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref Value0), index));
			}
			throw new IndexOutOfRangeException();
		}
	}
}
