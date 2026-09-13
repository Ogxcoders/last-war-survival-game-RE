using System;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public struct FixedArray3<T> where T : unmanaged
{
	public T Value0;

	public T Value1;

	public T Value2;

	public const int Length = 3;

	public unsafe ref T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (index > -1 && index < 3)
			{
				return ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref Value0), index));
			}
			throw new IndexOutOfRangeException();
		}
	}
}
