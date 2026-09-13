using System;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

public static class FixedArrayExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetRef<T>(this ref FixedArray2<T> array, int index) where T : unmanaged
	{
		return index switch
		{
			0 => ref array.Value0, 
			1 => ref array.Value1, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetRef<T>(this ref FixedArray3<T> array, int index) where T : unmanaged
	{
		return index switch
		{
			0 => ref array.Value0, 
			1 => ref array.Value1, 
			2 => ref array.Value2, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetRef<T>(this ref FixedArray8<T> array, int index) where T : unmanaged
	{
		return index switch
		{
			0 => ref array.Value0, 
			1 => ref array.Value1, 
			2 => ref array.Value2, 
			3 => ref array.Value3, 
			4 => ref array.Value4, 
			5 => ref array.Value5, 
			6 => ref array.Value6, 
			7 => ref array.Value7, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}
}
