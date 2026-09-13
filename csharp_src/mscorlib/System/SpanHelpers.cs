using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System;

internal static class SpanHelpers
{
	internal struct ComparerComparable<T, TComparer> : IComparable<T> where TComparer : IComparer<T>
	{
		private readonly T _value;

		private readonly TComparer _comparer;

		public ComparerComparable(T value, TComparer comparer)
		{
			_value = value;
			_comparer = comparer;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CompareTo(T other)
		{
			return _comparer.Compare(_value, other);
		}
	}

	[StructLayout(LayoutKind.Sequential, Size = 64)]
	private struct Reg64
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 32)]
	private struct Reg32
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 16)]
	private struct Reg16
	{
	}

	public static class PerTypeValues<T>
	{
		public static readonly bool IsReferenceOrContainsReferences = IsReferenceOrContainsReferencesCore(typeof(T));

		public static readonly T[] EmptyArray = new T[0];

		public static readonly IntPtr ArrayAdjustment = MeasureArrayAdjustment();

		private static IntPtr MeasureArrayAdjustment()
		{
			T[] array = new T[1];
			return Unsafe.ByteOffset(ref Unsafe.As<Pinnable<T>>(array).Data, ref array[0]);
		}
	}

	private const ulong XorPowerOfTwoToHighChar = 4295098372uL;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int BinarySearch<T, TComparable>(this ReadOnlySpan<T> span, TComparable comparable) where TComparable : IComparable<T>
	{
		if (comparable == null)
		{
			ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparable);
		}
		return BinarySearch(ref MemoryMarshal.GetReference(span), span.Length, comparable);
	}

	public static int BinarySearch<T, TComparable>(ref T spanStart, int length, TComparable comparable) where TComparable : IComparable<T>
	{
		int num = 0;
		int num2 = length - 1;
		while (num <= num2)
		{
			int num3 = num2 + num >>> 1;
			int num4 = comparable.CompareTo(Unsafe.Add(ref spanStart, num3));
			if (num4 == 0)
			{
				return num3;
			}
			if (num4 > 0)
			{
				num = num3 + 1;
			}
			else
			{
				num2 = num3 - 1;
			}
		}
		return ~num;
	}

	public static int IndexOf(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
	{
		if (valueLength == 0)
		{
			return 0;
		}
		byte value2 = value;
		ref byte second = ref Unsafe.Add(ref value, 1);
		int num = valueLength - 1;
		int num2 = 0;
		while (true)
		{
			int num3 = searchSpaceLength - num2 - num;
			if (num3 <= 0)
			{
				break;
			}
			int num4 = IndexOf(ref Unsafe.Add(ref searchSpace, num2), value2, num3);
			if (num4 == -1)
			{
				break;
			}
			num2 += num4;
			if (SequenceEqual(ref Unsafe.Add(ref searchSpace, num2 + 1), ref second, num))
			{
				return num2;
			}
			num2++;
		}
		return -1;
	}

	public static int IndexOfAny(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
	{
		if (valueLength == 0)
		{
			return 0;
		}
		int num = -1;
		for (int i = 0; i < valueLength; i++)
		{
			int num2 = IndexOf(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
			if ((uint)num2 < (uint)num)
			{
				num = num2;
				searchSpaceLength = num2;
				if (num == 0)
				{
					break;
				}
			}
		}
		return num;
	}

	public static int LastIndexOfAny(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
	{
		if (valueLength == 0)
		{
			return 0;
		}
		int num = -1;
		for (int i = 0; i < valueLength; i++)
		{
			int num2 = LastIndexOf(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
			if (num2 > num)
			{
				num = num2;
			}
		}
		return num;
	}

	public unsafe static int IndexOf(ref byte searchSpace, byte value, int length)
	{
		IntPtr intPtr = (IntPtr)0;
		IntPtr intPtr2 = (IntPtr)length;
		while (true)
		{
			if ((nuint)(void*)intPtr2 >= (nuint)8u)
			{
				intPtr2 -= 8;
				if (value != Unsafe.AddByteOffset(ref searchSpace, intPtr))
				{
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 1))
					{
						goto IL_0155;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 2))
					{
						goto IL_0163;
					}
					if (value != Unsafe.AddByteOffset(ref searchSpace, intPtr + 3))
					{
						if (value != Unsafe.AddByteOffset(ref searchSpace, intPtr + 4))
						{
							if (value != Unsafe.AddByteOffset(ref searchSpace, intPtr + 5))
							{
								if (value != Unsafe.AddByteOffset(ref searchSpace, intPtr + 6))
								{
									if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 7))
									{
										break;
									}
									intPtr += 8;
									continue;
								}
								return (int)(void*)(intPtr + 6);
							}
							return (int)(void*)(intPtr + 5);
						}
						return (int)(void*)(intPtr + 4);
					}
					goto IL_0171;
				}
			}
			else
			{
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr))
					{
						goto IL_014d;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 1))
					{
						goto IL_0155;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 2))
					{
						goto IL_0163;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 3))
					{
						goto IL_0171;
					}
					intPtr += 4;
				}
				while (true)
				{
					if ((void*)intPtr2 != null)
					{
						intPtr2 -= 1;
						if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr))
						{
							break;
						}
						intPtr += 1;
						continue;
					}
					return -1;
				}
			}
			goto IL_014d;
			IL_0163:
			return (int)(void*)(intPtr + 2);
			IL_014d:
			return (int)(void*)intPtr;
			IL_0155:
			return (int)(void*)(intPtr + 1);
			IL_0171:
			return (int)(void*)(intPtr + 3);
		}
		return (int)(void*)(intPtr + 7);
	}

	public static int LastIndexOf(ref byte searchSpace, int searchSpaceLength, ref byte value, int valueLength)
	{
		if (valueLength == 0)
		{
			return 0;
		}
		byte value2 = value;
		ref byte second = ref Unsafe.Add(ref value, 1);
		int num = valueLength - 1;
		int num2 = 0;
		while (true)
		{
			int num3 = searchSpaceLength - num2 - num;
			if (num3 <= 0)
			{
				break;
			}
			int num4 = LastIndexOf(ref searchSpace, value2, num3);
			if (num4 == -1)
			{
				break;
			}
			if (SequenceEqual(ref Unsafe.Add(ref searchSpace, num4 + 1), ref second, num))
			{
				return num4;
			}
			num2 += num3 - num4;
		}
		return -1;
	}

	public unsafe static int LastIndexOf(ref byte searchSpace, byte value, int length)
	{
		IntPtr intPtr = (IntPtr)length;
		IntPtr intPtr2 = (IntPtr)length;
		while (true)
		{
			if ((nuint)(void*)intPtr2 >= (nuint)8u)
			{
				intPtr2 -= 8;
				intPtr -= 8;
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 7))
				{
					break;
				}
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 6))
				{
					return (int)(void*)(intPtr + 6);
				}
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 5))
				{
					return (int)(void*)(intPtr + 5);
				}
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 4))
				{
					return (int)(void*)(intPtr + 4);
				}
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 3))
				{
					goto IL_0171;
				}
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 2))
				{
					goto IL_0163;
				}
				if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 1))
				{
					goto IL_0155;
				}
				if (value != Unsafe.AddByteOffset(ref searchSpace, intPtr))
				{
					continue;
				}
			}
			else
			{
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					intPtr -= 4;
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 3))
					{
						goto IL_0171;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 2))
					{
						goto IL_0163;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr + 1))
					{
						goto IL_0155;
					}
					if (value == Unsafe.AddByteOffset(ref searchSpace, intPtr))
					{
						goto IL_014d;
					}
				}
				do
				{
					if ((void*)intPtr2 != null)
					{
						intPtr2 -= 1;
						intPtr -= 1;
						continue;
					}
					return -1;
				}
				while (value != Unsafe.AddByteOffset(ref searchSpace, intPtr));
			}
			goto IL_014d;
			IL_0163:
			return (int)(void*)(intPtr + 2);
			IL_0171:
			return (int)(void*)(intPtr + 3);
			IL_0155:
			return (int)(void*)(intPtr + 1);
			IL_014d:
			return (int)(void*)intPtr;
		}
		return (int)(void*)(intPtr + 7);
	}

	public unsafe static int IndexOfAny(ref byte searchSpace, byte value0, byte value1, int length)
	{
		IntPtr intPtr = (IntPtr)0;
		IntPtr intPtr2 = (IntPtr)length;
		while (true)
		{
			if ((nuint)(void*)intPtr2 >= (nuint)8u)
			{
				intPtr2 -= 8;
				uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
				if (value0 != num && value1 != num)
				{
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
					if (value0 == num || value1 == num)
					{
						goto IL_01ed;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
					if (value0 == num || value1 == num)
					{
						goto IL_01fb;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
					if (value0 != num && value1 != num)
					{
						num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 4);
						if (value0 != num && value1 != num)
						{
							num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 5);
							if (value0 != num && value1 != num)
							{
								num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 6);
								if (value0 != num && value1 != num)
								{
									num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 7);
									if (value0 == num || value1 == num)
									{
										break;
									}
									intPtr += 8;
									continue;
								}
								return (int)(void*)(intPtr + 6);
							}
							return (int)(void*)(intPtr + 5);
						}
						return (int)(void*)(intPtr + 4);
					}
					goto IL_0209;
				}
			}
			else
			{
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
					if (value0 == num || value1 == num)
					{
						goto IL_01e5;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
					if (value0 == num || value1 == num)
					{
						goto IL_01ed;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
					if (value0 == num || value1 == num)
					{
						goto IL_01fb;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
					if (value0 == num || value1 == num)
					{
						goto IL_0209;
					}
					intPtr += 4;
				}
				while (true)
				{
					if ((void*)intPtr2 != null)
					{
						intPtr2 -= 1;
						uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
						if (value0 == num || value1 == num)
						{
							break;
						}
						intPtr += 1;
						continue;
					}
					return -1;
				}
			}
			goto IL_01e5;
			IL_01fb:
			return (int)(void*)(intPtr + 2);
			IL_01ed:
			return (int)(void*)(intPtr + 1);
			IL_0209:
			return (int)(void*)(intPtr + 3);
			IL_01e5:
			return (int)(void*)intPtr;
		}
		return (int)(void*)(intPtr + 7);
	}

	public unsafe static int IndexOfAny(ref byte searchSpace, byte value0, byte value1, byte value2, int length)
	{
		IntPtr intPtr = (IntPtr)0;
		IntPtr intPtr2 = (IntPtr)length;
		while (true)
		{
			if ((nuint)(void*)intPtr2 >= (nuint)8u)
			{
				intPtr2 -= 8;
				uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
				if (value0 != num && value1 != num && value2 != num)
				{
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_0262;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_0270;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
					if (value0 != num && value1 != num && value2 != num)
					{
						num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 4);
						if (value0 != num && value1 != num && value2 != num)
						{
							num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 5);
							if (value0 != num && value1 != num && value2 != num)
							{
								num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 6);
								if (value0 != num && value1 != num && value2 != num)
								{
									num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 7);
									if (value0 == num || value1 == num || value2 == num)
									{
										break;
									}
									intPtr += 8;
									continue;
								}
								return (int)(void*)(intPtr + 6);
							}
							return (int)(void*)(intPtr + 5);
						}
						return (int)(void*)(intPtr + 4);
					}
					goto IL_027e;
				}
			}
			else
			{
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_025a;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_0262;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_0270;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_027e;
					}
					intPtr += 4;
				}
				while (true)
				{
					if ((void*)intPtr2 != null)
					{
						intPtr2 -= 1;
						uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
						if (value0 == num || value1 == num || value2 == num)
						{
							break;
						}
						intPtr += 1;
						continue;
					}
					return -1;
				}
			}
			goto IL_025a;
			IL_025a:
			return (int)(void*)intPtr;
			IL_0270:
			return (int)(void*)(intPtr + 2);
			IL_0262:
			return (int)(void*)(intPtr + 1);
			IL_027e:
			return (int)(void*)(intPtr + 3);
		}
		return (int)(void*)(intPtr + 7);
	}

	public unsafe static int LastIndexOfAny(ref byte searchSpace, byte value0, byte value1, int length)
	{
		IntPtr intPtr = (IntPtr)length;
		IntPtr intPtr2 = (IntPtr)length;
		while (true)
		{
			if ((nuint)(void*)intPtr2 >= (nuint)8u)
			{
				intPtr2 -= 8;
				intPtr -= 8;
				uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 7);
				if (value0 == num || value1 == num)
				{
					break;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 6);
				if (value0 == num || value1 == num)
				{
					return (int)(void*)(intPtr + 6);
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 5);
				if (value0 == num || value1 == num)
				{
					return (int)(void*)(intPtr + 5);
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 4);
				if (value0 == num || value1 == num)
				{
					return (int)(void*)(intPtr + 4);
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
				if (value0 == num || value1 == num)
				{
					goto IL_0209;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
				if (value0 == num || value1 == num)
				{
					goto IL_01fb;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
				if (value0 == num || value1 == num)
				{
					goto IL_01ed;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
				if (value0 != num && value1 != num)
				{
					continue;
				}
			}
			else
			{
				uint num;
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					intPtr -= 4;
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
					if (value0 == num || value1 == num)
					{
						goto IL_0209;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
					if (value0 == num || value1 == num)
					{
						goto IL_01fb;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
					if (value0 == num || value1 == num)
					{
						goto IL_01ed;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
					if (value0 == num || value1 == num)
					{
						goto IL_01e5;
					}
				}
				do
				{
					if ((void*)intPtr2 != null)
					{
						intPtr2 -= 1;
						intPtr -= 1;
						num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
						continue;
					}
					return -1;
				}
				while (value0 != num && value1 != num);
			}
			goto IL_01e5;
			IL_01fb:
			return (int)(void*)(intPtr + 2);
			IL_01ed:
			return (int)(void*)(intPtr + 1);
			IL_01e5:
			return (int)(void*)intPtr;
			IL_0209:
			return (int)(void*)(intPtr + 3);
		}
		return (int)(void*)(intPtr + 7);
	}

	public unsafe static int LastIndexOfAny(ref byte searchSpace, byte value0, byte value1, byte value2, int length)
	{
		IntPtr intPtr = (IntPtr)length;
		IntPtr intPtr2 = (IntPtr)length;
		while (true)
		{
			if ((nuint)(void*)intPtr2 >= (nuint)8u)
			{
				intPtr2 -= 8;
				intPtr -= 8;
				uint num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 7);
				if (value0 == num || value1 == num || value2 == num)
				{
					break;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 6);
				if (value0 == num || value1 == num || value2 == num)
				{
					return (int)(void*)(intPtr + 6);
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 5);
				if (value0 == num || value1 == num || value2 == num)
				{
					return (int)(void*)(intPtr + 5);
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 4);
				if (value0 == num || value1 == num || value2 == num)
				{
					return (int)(void*)(intPtr + 4);
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
				if (value0 == num || value1 == num || value2 == num)
				{
					goto IL_027c;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
				if (value0 == num || value1 == num || value2 == num)
				{
					goto IL_026e;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
				if (value0 == num || value1 == num || value2 == num)
				{
					goto IL_0260;
				}
				num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
				if (value0 != num && value1 != num && value2 != num)
				{
					continue;
				}
			}
			else
			{
				uint num;
				if ((nuint)(void*)intPtr2 >= (nuint)4u)
				{
					intPtr2 -= 4;
					intPtr -= 4;
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 3);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_027c;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 2);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_026e;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr + 1);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_0260;
					}
					num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
					if (value0 == num || value1 == num || value2 == num)
					{
						goto IL_0258;
					}
				}
				do
				{
					if ((void*)intPtr2 != null)
					{
						intPtr2 -= 1;
						intPtr -= 1;
						num = Unsafe.AddByteOffset(ref searchSpace, intPtr);
						continue;
					}
					return -1;
				}
				while (value0 != num && value1 != num && value2 != num);
			}
			goto IL_0258;
			IL_0260:
			return (int)(void*)(intPtr + 1);
			IL_026e:
			return (int)(void*)(intPtr + 2);
			IL_027c:
			return (int)(void*)(intPtr + 3);
			IL_0258:
			return (int)(void*)intPtr;
		}
		return (int)(void*)(intPtr + 7);
	}

	public unsafe static bool SequenceEqual(ref byte first, ref byte second, ulong length)
	{
		if (!Unsafe.AreSame(ref first, ref second))
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)(void*)length;
			if ((nuint)(void*)intPtr2 >= (nuint)sizeof(UIntPtr))
			{
				intPtr2 -= sizeof(UIntPtr);
				while (true)
				{
					if ((void*)intPtr2 > (void*)intPtr)
					{
						if (Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.AddByteOffset(ref first, intPtr)) != Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.AddByteOffset(ref second, intPtr)))
						{
							break;
						}
						intPtr += sizeof(UIntPtr);
						continue;
					}
					return Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.AddByteOffset(ref first, intPtr2)) == Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.AddByteOffset(ref second, intPtr2));
				}
				goto IL_00be;
			}
			while ((void*)intPtr2 > (void*)intPtr)
			{
				if (Unsafe.AddByteOffset(ref first, intPtr) == Unsafe.AddByteOffset(ref second, intPtr))
				{
					intPtr += 1;
					continue;
				}
				goto IL_00be;
			}
		}
		return true;
		IL_00be:
		return false;
	}

	public unsafe static int SequenceCompareTo(ref byte first, int firstLength, ref byte second, int secondLength)
	{
		if (!Unsafe.AreSame(ref first, ref second))
		{
			IntPtr intPtr = (IntPtr)((firstLength < secondLength) ? firstLength : secondLength);
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)(void*)intPtr;
			if ((nuint)(void*)intPtr3 > (nuint)sizeof(UIntPtr))
			{
				intPtr3 -= sizeof(UIntPtr);
				for (; (void*)intPtr3 > (void*)intPtr2 && !(Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.AddByteOffset(ref first, intPtr2)) != Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.AddByteOffset(ref second, intPtr2))); intPtr2 += sizeof(UIntPtr))
				{
				}
			}
			for (; (void*)intPtr > (void*)intPtr2; intPtr2 += 1)
			{
				int num = Unsafe.AddByteOffset(ref first, intPtr2).CompareTo(Unsafe.AddByteOffset(ref second, intPtr2));
				if (num != 0)
				{
					return num;
				}
			}
		}
		return firstLength - secondLength;
	}

	public unsafe static int SequenceCompareTo(ref char first, int firstLength, ref char second, int secondLength)
	{
		int result = firstLength - secondLength;
		if (!Unsafe.AreSame(ref first, ref second))
		{
			IntPtr intPtr = (IntPtr)((firstLength < secondLength) ? firstLength : secondLength);
			IntPtr intPtr2 = (IntPtr)0;
			if ((nuint)(void*)intPtr >= (nuint)(sizeof(UIntPtr) / 2))
			{
				if (Vector.IsHardwareAccelerated && (nuint)(void*)intPtr >= (nuint)Vector<ushort>.Count)
				{
					IntPtr intPtr3 = intPtr - Vector<ushort>.Count;
					while (!(Unsafe.ReadUnaligned<Vector<ushort>>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref first, intPtr2))) != Unsafe.ReadUnaligned<Vector<ushort>>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref second, intPtr2)))))
					{
						intPtr2 += Vector<ushort>.Count;
						if ((void*)intPtr3 < (void*)intPtr2)
						{
							break;
						}
					}
				}
				for (; (void*)intPtr >= (void*)(intPtr2 + sizeof(UIntPtr) / 2) && !(Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref first, intPtr2))) != Unsafe.ReadUnaligned<UIntPtr>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref second, intPtr2)))); intPtr2 += sizeof(UIntPtr) / 2)
				{
				}
			}
			if (sizeof(UIntPtr) > 4 && (void*)intPtr >= (void*)(intPtr2 + 2) && Unsafe.ReadUnaligned<int>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref first, intPtr2))) == Unsafe.ReadUnaligned<int>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref second, intPtr2))))
			{
				intPtr2 += 2;
			}
			for (; (void*)intPtr2 < (void*)intPtr; intPtr2 += 1)
			{
				int num = Unsafe.Add(ref first, intPtr2).CompareTo(Unsafe.Add(ref second, intPtr2));
				if (num != 0)
				{
					return num;
				}
			}
		}
		return result;
	}

	public unsafe static int IndexOf(ref char searchSpace, char value, int length)
	{
		fixed (char* ptr = &searchSpace)
		{
			char* ptr2 = ptr;
			char* ptr3 = ptr2 + length;
			if (Vector.IsHardwareAccelerated && length >= Vector<ushort>.Count * 2)
			{
				int num = ((int)ptr2 & (Unsafe.SizeOf<Vector<ushort>>() - 1)) / 2;
				length = (Vector<ushort>.Count - num) & (Vector<ushort>.Count - 1);
			}
			while (true)
			{
				if (length >= 4)
				{
					length -= 4;
					if (*ptr2 == value)
					{
						break;
					}
					if (ptr2[1] != value)
					{
						if (ptr2[2] != value)
						{
							if (ptr2[3] != value)
							{
								ptr2 += 4;
								continue;
							}
							ptr2++;
						}
						ptr2++;
					}
					ptr2++;
					break;
				}
				while (length > 0)
				{
					length--;
					if (*ptr2 == value)
					{
						goto end_IL_0079;
					}
					ptr2++;
				}
				if (Vector.IsHardwareAccelerated && ptr2 < ptr3)
				{
					length = (int)((ptr3 - ptr2) & ~(Vector<ushort>.Count - 1));
					Vector<ushort> left = new Vector<ushort>(value);
					while (length > 0)
					{
						Vector<ushort> vector = Vector.Equals(left, Unsafe.Read<Vector<ushort>>(ptr2));
						if (Vector<ushort>.Zero.Equals(vector))
						{
							ptr2 += Vector<ushort>.Count;
							length -= Vector<ushort>.Count;
							continue;
						}
						return (int)(ptr2 - ptr) + LocateFirstFoundChar(vector);
					}
					if (ptr2 < ptr3)
					{
						length = (int)(ptr3 - ptr2);
						continue;
					}
				}
				return -1;
				continue;
				end_IL_0079:
				break;
			}
			return (int)(ptr2 - ptr);
		}
	}

	public unsafe static int LastIndexOf(ref char searchSpace, char value, int length)
	{
		fixed (char* ptr = &searchSpace)
		{
			char* ptr2 = ptr + length;
			char* ptr3 = ptr;
			if (Vector.IsHardwareAccelerated && length >= Vector<ushort>.Count * 2)
			{
				length = ((int)ptr2 & (Unsafe.SizeOf<Vector<ushort>>() - 1)) / 2;
			}
			while (true)
			{
				if (length >= 4)
				{
					length -= 4;
					ptr2 -= 4;
					if (ptr2[3] == value)
					{
						break;
					}
					if (ptr2[2] != value)
					{
						if (ptr2[1] != value)
						{
							if (*ptr2 != value)
							{
								continue;
							}
							goto IL_011a;
						}
						return (int)(ptr2 - ptr3) + 1;
					}
					return (int)(ptr2 - ptr3) + 2;
				}
				while (length > 0)
				{
					length--;
					ptr2--;
					if (*ptr2 != value)
					{
						continue;
					}
					goto IL_011a;
				}
				if (Vector.IsHardwareAccelerated && ptr2 > ptr3)
				{
					length = (int)((ptr2 - ptr3) & ~(Vector<ushort>.Count - 1));
					Vector<ushort> left = new Vector<ushort>(value);
					while (length > 0)
					{
						char* ptr4 = ptr2 - Vector<ushort>.Count;
						Vector<ushort> vector = Vector.Equals(left, Unsafe.Read<Vector<ushort>>(ptr4));
						if (Vector<ushort>.Zero.Equals(vector))
						{
							ptr2 -= Vector<ushort>.Count;
							length -= Vector<ushort>.Count;
							continue;
						}
						return (int)(ptr4 - ptr3) + LocateLastFoundChar(vector);
					}
					if (ptr2 > ptr3)
					{
						length = (int)(ptr2 - ptr3);
						continue;
					}
				}
				return -1;
				IL_011a:
				return (int)(ptr2 - ptr3);
			}
			return (int)(ptr2 - ptr3) + 3;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int LocateFirstFoundChar(Vector<ushort> match)
	{
		Vector<ulong> vector = Vector.AsVectorUInt64(match);
		ulong num = 0uL;
		int i;
		for (i = 0; i < Vector<ulong>.Count; i++)
		{
			num = vector[i];
			if (num != 0L)
			{
				break;
			}
		}
		return i * 4 + LocateFirstFoundChar(num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int LocateFirstFoundChar(ulong match)
	{
		return (int)((match ^ (match - 1)) * 4295098372L >> 49);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int LocateLastFoundChar(Vector<ushort> match)
	{
		Vector<ulong> vector = Vector.AsVectorUInt64(match);
		ulong num = 0uL;
		int num2;
		for (num2 = Vector<ulong>.Count - 1; num2 >= 0; num2--)
		{
			num = vector[num2];
			if (num != 0L)
			{
				break;
			}
		}
		return num2 * 4 + LocateLastFoundChar(num);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int LocateLastFoundChar(ulong match)
	{
		int num = 3;
		while ((long)match > 0L)
		{
			match <<= 16;
			num--;
		}
		return num;
	}

	public static int IndexOf<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
	{
		if (valueLength == 0)
		{
			return 0;
		}
		T value2 = value;
		ref T second = ref Unsafe.Add(ref value, 1);
		int num = valueLength - 1;
		int num2 = 0;
		while (true)
		{
			int num3 = searchSpaceLength - num2 - num;
			if (num3 <= 0)
			{
				break;
			}
			int num4 = IndexOf(ref Unsafe.Add(ref searchSpace, num2), value2, num3);
			if (num4 == -1)
			{
				break;
			}
			num2 += num4;
			if (SequenceEqual(ref Unsafe.Add(ref searchSpace, num2 + 1), ref second, num))
			{
				return num2;
			}
			num2++;
		}
		return -1;
	}

	public unsafe static int IndexOf<T>(ref T searchSpace, T value, int length) where T : IEquatable<T>
	{
		IntPtr intPtr = (IntPtr)0;
		while (true)
		{
			if (length >= 8)
			{
				length -= 8;
				T other = Unsafe.Add(ref searchSpace, intPtr);
				if (!value.Equals(other))
				{
					T other2 = Unsafe.Add(ref searchSpace, intPtr + 1);
					if (value.Equals(other2))
					{
						goto IL_020a;
					}
					T other3 = Unsafe.Add(ref searchSpace, intPtr + 2);
					if (value.Equals(other3))
					{
						goto IL_0218;
					}
					T other4 = Unsafe.Add(ref searchSpace, intPtr + 3);
					if (!value.Equals(other4))
					{
						T other5 = Unsafe.Add(ref searchSpace, intPtr + 4);
						if (!value.Equals(other5))
						{
							T other6 = Unsafe.Add(ref searchSpace, intPtr + 5);
							if (!value.Equals(other6))
							{
								T other7 = Unsafe.Add(ref searchSpace, intPtr + 6);
								if (!value.Equals(other7))
								{
									T other8 = Unsafe.Add(ref searchSpace, intPtr + 7);
									if (value.Equals(other8))
									{
										break;
									}
									intPtr += 8;
									continue;
								}
								return (int)(void*)(intPtr + 6);
							}
							return (int)(void*)(intPtr + 5);
						}
						return (int)(void*)(intPtr + 4);
					}
					goto IL_0226;
				}
			}
			else
			{
				if (length >= 4)
				{
					length -= 4;
					T other9 = Unsafe.Add(ref searchSpace, intPtr);
					if (value.Equals(other9))
					{
						goto IL_0202;
					}
					T other10 = Unsafe.Add(ref searchSpace, intPtr + 1);
					if (value.Equals(other10))
					{
						goto IL_020a;
					}
					T other11 = Unsafe.Add(ref searchSpace, intPtr + 2);
					if (value.Equals(other11))
					{
						goto IL_0218;
					}
					T other12 = Unsafe.Add(ref searchSpace, intPtr + 3);
					if (value.Equals(other12))
					{
						goto IL_0226;
					}
					intPtr += 4;
				}
				while (true)
				{
					if (length > 0)
					{
						T other13 = Unsafe.Add(ref searchSpace, intPtr);
						if (value.Equals(other13))
						{
							break;
						}
						intPtr += 1;
						length--;
						continue;
					}
					return -1;
				}
			}
			goto IL_0202;
			IL_0218:
			return (int)(void*)(intPtr + 2);
			IL_0202:
			return (int)(void*)intPtr;
			IL_020a:
			return (int)(void*)(intPtr + 1);
			IL_0226:
			return (int)(void*)(intPtr + 3);
		}
		return (int)(void*)(intPtr + 7);
	}

	public static int IndexOfAny<T>(ref T searchSpace, T value0, T value1, int length) where T : IEquatable<T>
	{
		int num = 0;
		while (true)
		{
			if (length - num >= 8)
			{
				T other = Unsafe.Add(ref searchSpace, num);
				if (!value0.Equals(other) && !value1.Equals(other))
				{
					other = Unsafe.Add(ref searchSpace, num + 1);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02cb;
					}
					other = Unsafe.Add(ref searchSpace, num + 2);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02cf;
					}
					other = Unsafe.Add(ref searchSpace, num + 3);
					if (!value0.Equals(other) && !value1.Equals(other))
					{
						other = Unsafe.Add(ref searchSpace, num + 4);
						if (!value0.Equals(other) && !value1.Equals(other))
						{
							other = Unsafe.Add(ref searchSpace, num + 5);
							if (!value0.Equals(other) && !value1.Equals(other))
							{
								other = Unsafe.Add(ref searchSpace, num + 6);
								if (!value0.Equals(other) && !value1.Equals(other))
								{
									other = Unsafe.Add(ref searchSpace, num + 7);
									if (value0.Equals(other) || value1.Equals(other))
									{
										break;
									}
									num += 8;
									continue;
								}
								return num + 6;
							}
							return num + 5;
						}
						return num + 4;
					}
					goto IL_02d3;
				}
			}
			else
			{
				if (length - num >= 4)
				{
					T other = Unsafe.Add(ref searchSpace, num);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02c9;
					}
					other = Unsafe.Add(ref searchSpace, num + 1);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02cb;
					}
					other = Unsafe.Add(ref searchSpace, num + 2);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02cf;
					}
					other = Unsafe.Add(ref searchSpace, num + 3);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02d3;
					}
					num += 4;
				}
				while (true)
				{
					if (num < length)
					{
						T other = Unsafe.Add(ref searchSpace, num);
						if (value0.Equals(other) || value1.Equals(other))
						{
							break;
						}
						num++;
						continue;
					}
					return -1;
				}
			}
			goto IL_02c9;
			IL_02cf:
			return num + 2;
			IL_02cb:
			return num + 1;
			IL_02d3:
			return num + 3;
			IL_02c9:
			return num;
		}
		return num + 7;
	}

	public static int IndexOfAny<T>(ref T searchSpace, T value0, T value1, T value2, int length) where T : IEquatable<T>
	{
		int num = 0;
		while (true)
		{
			if (length - num >= 8)
			{
				T other = Unsafe.Add(ref searchSpace, num);
				if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
				{
					other = Unsafe.Add(ref searchSpace, num + 1);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03c2;
					}
					other = Unsafe.Add(ref searchSpace, num + 2);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03c6;
					}
					other = Unsafe.Add(ref searchSpace, num + 3);
					if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
					{
						other = Unsafe.Add(ref searchSpace, num + 4);
						if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
						{
							other = Unsafe.Add(ref searchSpace, num + 5);
							if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
							{
								other = Unsafe.Add(ref searchSpace, num + 6);
								if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
								{
									other = Unsafe.Add(ref searchSpace, num + 7);
									if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
									{
										break;
									}
									num += 8;
									continue;
								}
								return num + 6;
							}
							return num + 5;
						}
						return num + 4;
					}
					goto IL_03ca;
				}
			}
			else
			{
				if (length - num >= 4)
				{
					T other = Unsafe.Add(ref searchSpace, num);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03c0;
					}
					other = Unsafe.Add(ref searchSpace, num + 1);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03c2;
					}
					other = Unsafe.Add(ref searchSpace, num + 2);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03c6;
					}
					other = Unsafe.Add(ref searchSpace, num + 3);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03ca;
					}
					num += 4;
				}
				while (true)
				{
					if (num < length)
					{
						T other = Unsafe.Add(ref searchSpace, num);
						if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
						{
							break;
						}
						num++;
						continue;
					}
					return -1;
				}
			}
			goto IL_03c0;
			IL_03c0:
			return num;
			IL_03c6:
			return num + 2;
			IL_03c2:
			return num + 1;
			IL_03ca:
			return num + 3;
		}
		return num + 7;
	}

	public static int IndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
	{
		if (valueLength == 0)
		{
			return 0;
		}
		int num = -1;
		for (int i = 0; i < valueLength; i++)
		{
			int num2 = IndexOf(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
			if ((uint)num2 < (uint)num)
			{
				num = num2;
				searchSpaceLength = num2;
				if (num == 0)
				{
					break;
				}
			}
		}
		return num;
	}

	public static int LastIndexOf<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
	{
		if (valueLength == 0)
		{
			return 0;
		}
		T value2 = value;
		ref T second = ref Unsafe.Add(ref value, 1);
		int num = valueLength - 1;
		int num2 = 0;
		while (true)
		{
			int num3 = searchSpaceLength - num2 - num;
			if (num3 <= 0)
			{
				break;
			}
			int num4 = LastIndexOf(ref searchSpace, value2, num3);
			if (num4 == -1)
			{
				break;
			}
			if (SequenceEqual(ref Unsafe.Add(ref searchSpace, num4 + 1), ref second, num))
			{
				return num4;
			}
			num2 += num3 - num4;
		}
		return -1;
	}

	public static int LastIndexOf<T>(ref T searchSpace, T value, int length) where T : IEquatable<T>
	{
		while (true)
		{
			if (length >= 8)
			{
				length -= 8;
				T other = Unsafe.Add(ref searchSpace, length + 7);
				if (value.Equals(other))
				{
					break;
				}
				T other2 = Unsafe.Add(ref searchSpace, length + 6);
				if (value.Equals(other2))
				{
					return length + 6;
				}
				T other3 = Unsafe.Add(ref searchSpace, length + 5);
				if (value.Equals(other3))
				{
					return length + 5;
				}
				T other4 = Unsafe.Add(ref searchSpace, length + 4);
				if (value.Equals(other4))
				{
					return length + 4;
				}
				T other5 = Unsafe.Add(ref searchSpace, length + 3);
				if (value.Equals(other5))
				{
					goto IL_01c2;
				}
				T other6 = Unsafe.Add(ref searchSpace, length + 2);
				if (value.Equals(other6))
				{
					goto IL_01be;
				}
				T other7 = Unsafe.Add(ref searchSpace, length + 1);
				if (value.Equals(other7))
				{
					goto IL_01ba;
				}
				T other8 = Unsafe.Add(ref searchSpace, length);
				if (!value.Equals(other8))
				{
					continue;
				}
			}
			else
			{
				if (length >= 4)
				{
					length -= 4;
					T other9 = Unsafe.Add(ref searchSpace, length + 3);
					if (value.Equals(other9))
					{
						goto IL_01c2;
					}
					T other10 = Unsafe.Add(ref searchSpace, length + 2);
					if (value.Equals(other10))
					{
						goto IL_01be;
					}
					T other11 = Unsafe.Add(ref searchSpace, length + 1);
					if (value.Equals(other11))
					{
						goto IL_01ba;
					}
					T other12 = Unsafe.Add(ref searchSpace, length);
					if (value.Equals(other12))
					{
						goto IL_01b8;
					}
				}
				T other13;
				do
				{
					if (length > 0)
					{
						length--;
						other13 = Unsafe.Add(ref searchSpace, length);
						continue;
					}
					return -1;
				}
				while (!value.Equals(other13));
			}
			goto IL_01b8;
			IL_01be:
			return length + 2;
			IL_01c2:
			return length + 3;
			IL_01ba:
			return length + 1;
			IL_01b8:
			return length;
		}
		return length + 7;
	}

	public static int LastIndexOfAny<T>(ref T searchSpace, T value0, T value1, int length) where T : IEquatable<T>
	{
		while (true)
		{
			if (length >= 8)
			{
				length -= 8;
				T other = Unsafe.Add(ref searchSpace, length + 7);
				if (value0.Equals(other) || value1.Equals(other))
				{
					break;
				}
				other = Unsafe.Add(ref searchSpace, length + 6);
				if (value0.Equals(other) || value1.Equals(other))
				{
					return length + 6;
				}
				other = Unsafe.Add(ref searchSpace, length + 5);
				if (value0.Equals(other) || value1.Equals(other))
				{
					return length + 5;
				}
				other = Unsafe.Add(ref searchSpace, length + 4);
				if (value0.Equals(other) || value1.Equals(other))
				{
					return length + 4;
				}
				other = Unsafe.Add(ref searchSpace, length + 3);
				if (value0.Equals(other) || value1.Equals(other))
				{
					goto IL_02cd;
				}
				other = Unsafe.Add(ref searchSpace, length + 2);
				if (value0.Equals(other) || value1.Equals(other))
				{
					goto IL_02c9;
				}
				other = Unsafe.Add(ref searchSpace, length + 1);
				if (value0.Equals(other) || value1.Equals(other))
				{
					goto IL_02c5;
				}
				other = Unsafe.Add(ref searchSpace, length);
				if (!value0.Equals(other) && !value1.Equals(other))
				{
					continue;
				}
			}
			else
			{
				T other;
				if (length >= 4)
				{
					length -= 4;
					other = Unsafe.Add(ref searchSpace, length + 3);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02cd;
					}
					other = Unsafe.Add(ref searchSpace, length + 2);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02c9;
					}
					other = Unsafe.Add(ref searchSpace, length + 1);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02c5;
					}
					other = Unsafe.Add(ref searchSpace, length);
					if (value0.Equals(other) || value1.Equals(other))
					{
						goto IL_02c3;
					}
				}
				do
				{
					if (length > 0)
					{
						length--;
						other = Unsafe.Add(ref searchSpace, length);
						continue;
					}
					return -1;
				}
				while (!value0.Equals(other) && !value1.Equals(other));
			}
			goto IL_02c3;
			IL_02c9:
			return length + 2;
			IL_02c5:
			return length + 1;
			IL_02c3:
			return length;
			IL_02cd:
			return length + 3;
		}
		return length + 7;
	}

	public static int LastIndexOfAny<T>(ref T searchSpace, T value0, T value1, T value2, int length) where T : IEquatable<T>
	{
		while (true)
		{
			if (length >= 8)
			{
				length -= 8;
				T other = Unsafe.Add(ref searchSpace, length + 7);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					break;
				}
				other = Unsafe.Add(ref searchSpace, length + 6);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					return length + 6;
				}
				other = Unsafe.Add(ref searchSpace, length + 5);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					return length + 5;
				}
				other = Unsafe.Add(ref searchSpace, length + 4);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					return length + 4;
				}
				other = Unsafe.Add(ref searchSpace, length + 3);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					goto IL_03da;
				}
				other = Unsafe.Add(ref searchSpace, length + 2);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					goto IL_03d5;
				}
				other = Unsafe.Add(ref searchSpace, length + 1);
				if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
				{
					goto IL_03d0;
				}
				other = Unsafe.Add(ref searchSpace, length);
				if (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other))
				{
					continue;
				}
			}
			else
			{
				T other;
				if (length >= 4)
				{
					length -= 4;
					other = Unsafe.Add(ref searchSpace, length + 3);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03da;
					}
					other = Unsafe.Add(ref searchSpace, length + 2);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03d5;
					}
					other = Unsafe.Add(ref searchSpace, length + 1);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03d0;
					}
					other = Unsafe.Add(ref searchSpace, length);
					if (value0.Equals(other) || value1.Equals(other) || value2.Equals(other))
					{
						goto IL_03cd;
					}
				}
				do
				{
					if (length > 0)
					{
						length--;
						other = Unsafe.Add(ref searchSpace, length);
						continue;
					}
					return -1;
				}
				while (!value0.Equals(other) && !value1.Equals(other) && !value2.Equals(other));
			}
			goto IL_03cd;
			IL_03d0:
			return length + 1;
			IL_03d5:
			return length + 2;
			IL_03da:
			return length + 3;
			IL_03cd:
			return length;
		}
		return length + 7;
	}

	public static int LastIndexOfAny<T>(ref T searchSpace, int searchSpaceLength, ref T value, int valueLength) where T : IEquatable<T>
	{
		if (valueLength == 0)
		{
			return 0;
		}
		int num = -1;
		for (int i = 0; i < valueLength; i++)
		{
			int num2 = LastIndexOf(ref searchSpace, Unsafe.Add(ref value, i), searchSpaceLength);
			if (num2 > num)
			{
				num = num2;
			}
		}
		return num;
	}

	public static bool SequenceEqual<T>(ref T first, ref T second, int length) where T : IEquatable<T>
	{
		if (!Unsafe.AreSame(ref first, ref second))
		{
			IntPtr intPtr = (IntPtr)0;
			while (true)
			{
				if (length >= 8)
				{
					length -= 8;
					ref T reference = ref Unsafe.Add(ref first, intPtr);
					T other = Unsafe.Add(ref second, intPtr);
					if (reference.Equals(other))
					{
						ref T reference2 = ref Unsafe.Add(ref first, intPtr + 1);
						T other2 = Unsafe.Add(ref second, intPtr + 1);
						if (reference2.Equals(other2))
						{
							ref T reference3 = ref Unsafe.Add(ref first, intPtr + 2);
							T other3 = Unsafe.Add(ref second, intPtr + 2);
							if (reference3.Equals(other3))
							{
								ref T reference4 = ref Unsafe.Add(ref first, intPtr + 3);
								T other4 = Unsafe.Add(ref second, intPtr + 3);
								if (reference4.Equals(other4))
								{
									ref T reference5 = ref Unsafe.Add(ref first, intPtr + 4);
									T other5 = Unsafe.Add(ref second, intPtr + 4);
									if (reference5.Equals(other5))
									{
										ref T reference6 = ref Unsafe.Add(ref first, intPtr + 5);
										T other6 = Unsafe.Add(ref second, intPtr + 5);
										if (reference6.Equals(other6))
										{
											ref T reference7 = ref Unsafe.Add(ref first, intPtr + 6);
											T other7 = Unsafe.Add(ref second, intPtr + 6);
											if (reference7.Equals(other7))
											{
												ref T reference8 = ref Unsafe.Add(ref first, intPtr + 7);
												T other8 = Unsafe.Add(ref second, intPtr + 7);
												if (reference8.Equals(other8))
												{
													intPtr += 8;
													continue;
												}
											}
										}
									}
								}
							}
						}
					}
				}
				else
				{
					if (length < 4)
					{
						goto IL_0285;
					}
					length -= 4;
					ref T reference9 = ref Unsafe.Add(ref first, intPtr);
					T other9 = Unsafe.Add(ref second, intPtr);
					if (reference9.Equals(other9))
					{
						ref T reference10 = ref Unsafe.Add(ref first, intPtr + 1);
						T other10 = Unsafe.Add(ref second, intPtr + 1);
						if (reference10.Equals(other10))
						{
							ref T reference11 = ref Unsafe.Add(ref first, intPtr + 2);
							T other11 = Unsafe.Add(ref second, intPtr + 2);
							if (reference11.Equals(other11))
							{
								ref T reference12 = ref Unsafe.Add(ref first, intPtr + 3);
								T other12 = Unsafe.Add(ref second, intPtr + 3);
								if (reference12.Equals(other12))
								{
									intPtr += 4;
									goto IL_0285;
								}
							}
						}
					}
				}
				goto IL_028b;
				IL_028b:
				return false;
				IL_0285:
				while (length > 0)
				{
					ref T reference13 = ref Unsafe.Add(ref first, intPtr);
					T other13 = Unsafe.Add(ref second, intPtr);
					if (reference13.Equals(other13))
					{
						intPtr += 1;
						length--;
						continue;
					}
					goto IL_028b;
				}
				break;
			}
		}
		return true;
	}

	public static int SequenceCompareTo<T>(ref T first, int firstLength, ref T second, int secondLength) where T : IComparable<T>
	{
		int num = firstLength;
		if (num > secondLength)
		{
			num = secondLength;
		}
		for (int i = 0; i < num; i++)
		{
			ref T reference = ref Unsafe.Add(ref first, i);
			T other = Unsafe.Add(ref second, i);
			int num2 = reference.CompareTo(other);
			if (num2 != 0)
			{
				return num2;
			}
		}
		return firstLength.CompareTo(secondLength);
	}

	public static int IndexOfCultureHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, CompareInfo compareInfo)
	{
		if (GlobalizationMode.Invariant)
		{
			return CompareInfo.InvariantIndexOf(span, value, ignoreCase: false);
		}
		return compareInfo.IndexOf(span, value, CompareOptions.None);
	}

	public static int IndexOfCultureIgnoreCaseHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, CompareInfo compareInfo)
	{
		if (GlobalizationMode.Invariant)
		{
			return CompareInfo.InvariantIndexOf(span, value, ignoreCase: true);
		}
		return compareInfo.IndexOf(span, value, CompareOptions.IgnoreCase);
	}

	public static int IndexOfOrdinalHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, bool ignoreCase)
	{
		if (GlobalizationMode.Invariant)
		{
			return CompareInfo.InvariantIndexOf(span, value, ignoreCase);
		}
		return CompareInfo.Invariant.IndexOfOrdinal(span, value, ignoreCase);
	}

	public static bool StartsWithCultureHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, CompareInfo compareInfo)
	{
		if (GlobalizationMode.Invariant)
		{
			return span.StartsWith(value);
		}
		if (span.Length == 0)
		{
			return false;
		}
		return compareInfo.IsPrefix(span, value, CompareOptions.None);
	}

	public static bool StartsWithCultureIgnoreCaseHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, CompareInfo compareInfo)
	{
		if (GlobalizationMode.Invariant)
		{
			return StartsWithOrdinalIgnoreCaseHelper(span, value);
		}
		if (span.Length == 0)
		{
			return false;
		}
		return compareInfo.IsPrefix(span, value, CompareOptions.IgnoreCase);
	}

	public static bool StartsWithOrdinalIgnoreCaseHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value)
	{
		if (span.Length < value.Length)
		{
			return false;
		}
		return CompareInfo.CompareOrdinalIgnoreCase(span.Slice(0, value.Length), value) == 0;
	}

	public static bool EndsWithCultureHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, CompareInfo compareInfo)
	{
		if (GlobalizationMode.Invariant)
		{
			return span.EndsWith(value);
		}
		if (span.Length == 0)
		{
			return false;
		}
		return compareInfo.IsSuffix(span, value, CompareOptions.None);
	}

	public static bool EndsWithCultureIgnoreCaseHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value, CompareInfo compareInfo)
	{
		if (GlobalizationMode.Invariant)
		{
			return EndsWithOrdinalIgnoreCaseHelper(span, value);
		}
		if (span.Length == 0)
		{
			return false;
		}
		return compareInfo.IsSuffix(span, value, CompareOptions.IgnoreCase);
	}

	public static bool EndsWithOrdinalIgnoreCaseHelper(ReadOnlySpan<char> span, ReadOnlySpan<char> value)
	{
		if (span.Length < value.Length)
		{
			return false;
		}
		return CompareInfo.CompareOrdinalIgnoreCase(span.Slice(span.Length - value.Length), value) == 0;
	}

	public static void ClearWithoutReferences(ref byte b, ulong byteLength)
	{
		if (byteLength == 0L)
		{
			return;
		}
		ulong num = byteLength - 1;
		if (num <= 21)
		{
			switch (num)
			{
			case 0uL:
				b = 0;
				return;
			case 1uL:
				Unsafe.As<byte, short>(ref b) = 0;
				return;
			case 2uL:
				Unsafe.As<byte, short>(ref b) = 0;
				Unsafe.Add(ref b, 2) = 0;
				return;
			case 3uL:
				Unsafe.As<byte, int>(ref b) = 0;
				return;
			case 4uL:
				Unsafe.As<byte, int>(ref b) = 0;
				Unsafe.Add(ref b, 4) = 0;
				return;
			case 5uL:
				Unsafe.As<byte, int>(ref b) = 0;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 4)) = 0;
				return;
			case 6uL:
				Unsafe.As<byte, int>(ref b) = 0;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 4)) = 0;
				Unsafe.Add(ref b, 6) = 0;
				return;
			case 7uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				return;
			case 8uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.Add(ref b, 8) = 0;
				return;
			case 9uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 8)) = 0;
				return;
			case 10uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 8)) = 0;
				Unsafe.Add(ref b, 10) = 0;
				return;
			case 11uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
				return;
			case 12uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
				Unsafe.Add(ref b, 12) = 0;
				return;
			case 13uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 12)) = 0;
				return;
			case 14uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 8)) = 0;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 12)) = 0;
				Unsafe.Add(ref b, 14) = 0;
				return;
			case 15uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				return;
			case 16uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				Unsafe.Add(ref b, 16) = 0;
				return;
			case 17uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 16)) = 0;
				return;
			case 18uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 16)) = 0;
				Unsafe.Add(ref b, 18) = 0;
				return;
			case 19uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
				return;
			case 20uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
				Unsafe.Add(ref b, 20) = 0;
				return;
			case 21uL:
				Unsafe.As<byte, long>(ref b) = 0L;
				Unsafe.As<byte, long>(ref Unsafe.Add(ref b, 8)) = 0L;
				Unsafe.As<byte, int>(ref Unsafe.Add(ref b, 16)) = 0;
				Unsafe.As<byte, short>(ref Unsafe.Add(ref b, 20)) = 0;
				return;
			}
		}
		ulong num2;
		if (byteLength < 512)
		{
			num2 = 0uL;
			if ((Unsafe.As<byte, int>(ref b) & 3) != 0)
			{
				if ((Unsafe.As<byte, int>(ref b) & 1) != 0)
				{
					Unsafe.AddByteOffset(ref b, num2) = 0;
					num2++;
					if ((Unsafe.As<byte, int>(ref b) & 2) != 0)
					{
						goto IL_0349;
					}
				}
				Unsafe.As<byte, short>(ref Unsafe.AddByteOffset(ref b, num2)) = 0;
				num2 += 2;
			}
			goto IL_0349;
		}
		RuntimeImports.RhZeroMemory(ref b, byteLength);
		return;
		IL_0349:
		if (((Unsafe.As<byte, int>(ref b) - 1) & 4) == 0)
		{
			Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, num2)) = 0;
			num2 += 4;
		}
		ulong num3 = byteLength - 16;
		byteLength -= num2;
		ulong num4;
		do
		{
			num4 = num2 + 16;
			Unsafe.As<byte, long>(ref Unsafe.AddByteOffset(ref b, num2)) = 0L;
			Unsafe.As<byte, long>(ref Unsafe.AddByteOffset(ref b, num2 + 8)) = 0L;
			num2 = num4;
		}
		while (num4 <= num3);
		if ((byteLength & 8) != 0L)
		{
			Unsafe.As<byte, long>(ref Unsafe.AddByteOffset(ref b, num2)) = 0L;
			num2 += 8;
		}
		if ((byteLength & 4) != 0L)
		{
			Unsafe.As<byte, int>(ref Unsafe.AddByteOffset(ref b, num2)) = 0;
			num2 += 4;
		}
		if ((byteLength & 2) != 0L)
		{
			Unsafe.As<byte, short>(ref Unsafe.AddByteOffset(ref b, num2)) = 0;
			num2 += 2;
		}
		if ((byteLength & 1) != 0L)
		{
			Unsafe.AddByteOffset(ref b, num2) = 0;
		}
	}

	public static void ClearWithReferences(ref IntPtr ip, ulong pointerSizeLength)
	{
		while (pointerSizeLength >= 8)
		{
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -1) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -2) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -3) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -4) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -5) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -6) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -7) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -8) = default(IntPtr);
			pointerSizeLength -= 8;
		}
		if (pointerSizeLength < 4)
		{
			if (pointerSizeLength < 2)
			{
				if (pointerSizeLength == 0)
				{
					return;
				}
				goto IL_015b;
			}
		}
		else
		{
			Unsafe.Add(ref ip, 2) = default(IntPtr);
			Unsafe.Add(ref ip, 3) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -3) = default(IntPtr);
			Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -2) = default(IntPtr);
		}
		Unsafe.Add(ref ip, 1) = default(IntPtr);
		Unsafe.Add(ref Unsafe.Add(ref ip, (IntPtr)(long)pointerSizeLength), -1) = default(IntPtr);
		goto IL_015b;
		IL_015b:
		ip = default(IntPtr);
	}

	public unsafe static void CopyTo<T>(ref T dst, int dstLength, ref T src, int srcLength)
	{
		IntPtr intPtr = Unsafe.ByteOffset(ref src, ref Unsafe.Add(ref src, srcLength));
		IntPtr intPtr2 = Unsafe.ByteOffset(ref dst, ref Unsafe.Add(ref dst, dstLength));
		IntPtr intPtr3 = Unsafe.ByteOffset(ref src, ref dst);
		bool num;
		if (sizeof(IntPtr) != 4)
		{
			if ((ulong)(long)intPtr3 >= (ulong)(long)intPtr)
			{
				num = (ulong)(long)intPtr3 > (ulong)(-(long)intPtr2);
				goto IL_006f;
			}
		}
		else if ((uint)(int)intPtr3 >= (uint)(int)intPtr)
		{
			num = (uint)(int)intPtr3 > (uint)(-(int)intPtr2);
			goto IL_006f;
		}
		goto IL_00da;
		IL_006f:
		if (!num && !IsReferenceOrContainsReferences<T>())
		{
			ref byte source = ref Unsafe.As<T, byte>(ref dst);
			ref byte source2 = ref Unsafe.As<T, byte>(ref src);
			ulong num2 = (ulong)(long)intPtr;
			uint num4;
			for (ulong num3 = 0uL; num3 < num2; num3 += num4)
			{
				num4 = (uint)((num2 - num3 > uint.MaxValue) ? uint.MaxValue : (num2 - num3));
				Unsafe.CopyBlock(ref Unsafe.Add(ref source, (IntPtr)(long)num3), ref Unsafe.Add(ref source2, (IntPtr)(long)num3), num4);
			}
			return;
		}
		goto IL_00da;
		IL_00da:
		bool num5 = ((sizeof(IntPtr) == 4) ? ((uint)(int)intPtr3 > (uint)(-(int)intPtr2)) : ((ulong)(long)intPtr3 > (ulong)(-(long)intPtr2)));
		int num6 = (num5 ? 1 : (-1));
		int num7 = ((!num5) ? (srcLength - 1) : 0);
		int i;
		for (i = 0; i < (srcLength & -8); i += 8)
		{
			Unsafe.Add(ref dst, num7) = Unsafe.Add(ref src, num7);
			Unsafe.Add(ref dst, num7 + num6) = Unsafe.Add(ref src, num7 + num6);
			Unsafe.Add(ref dst, num7 + num6 * 2) = Unsafe.Add(ref src, num7 + num6 * 2);
			Unsafe.Add(ref dst, num7 + num6 * 3) = Unsafe.Add(ref src, num7 + num6 * 3);
			Unsafe.Add(ref dst, num7 + num6 * 4) = Unsafe.Add(ref src, num7 + num6 * 4);
			Unsafe.Add(ref dst, num7 + num6 * 5) = Unsafe.Add(ref src, num7 + num6 * 5);
			Unsafe.Add(ref dst, num7 + num6 * 6) = Unsafe.Add(ref src, num7 + num6 * 6);
			Unsafe.Add(ref dst, num7 + num6 * 7) = Unsafe.Add(ref src, num7 + num6 * 7);
			num7 += num6 * 8;
		}
		if (i < (srcLength & -4))
		{
			Unsafe.Add(ref dst, num7) = Unsafe.Add(ref src, num7);
			Unsafe.Add(ref dst, num7 + num6) = Unsafe.Add(ref src, num7 + num6);
			Unsafe.Add(ref dst, num7 + num6 * 2) = Unsafe.Add(ref src, num7 + num6 * 2);
			Unsafe.Add(ref dst, num7 + num6 * 3) = Unsafe.Add(ref src, num7 + num6 * 3);
			num7 += num6 * 4;
			i += 4;
		}
		for (; i < srcLength; i++)
		{
			Unsafe.Add(ref dst, num7) = Unsafe.Add(ref src, num7);
			num7 += num6;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public unsafe static IntPtr Add<T>(this IntPtr start, int index)
	{
		if (sizeof(IntPtr) == 4)
		{
			uint num = (uint)(index * Unsafe.SizeOf<T>());
			return (IntPtr)((byte*)(void*)start + num);
		}
		ulong num2 = (ulong)index * (ulong)Unsafe.SizeOf<T>();
		return (IntPtr)((byte*)(void*)start + num2);
	}

	public static bool IsReferenceOrContainsReferences<T>()
	{
		return PerTypeValues<T>.IsReferenceOrContainsReferences;
	}

	private static bool IsReferenceOrContainsReferencesCore(Type type)
	{
		if (type.GetTypeInfo().IsPrimitive)
		{
			return false;
		}
		if (!type.GetTypeInfo().IsValueType)
		{
			return true;
		}
		Type underlyingType = Nullable.GetUnderlyingType(type);
		if (underlyingType != null)
		{
			type = underlyingType;
		}
		if (type.GetTypeInfo().IsEnum)
		{
			return false;
		}
		foreach (FieldInfo declaredField in type.GetTypeInfo().DeclaredFields)
		{
			if (!declaredField.IsStatic && IsReferenceOrContainsReferencesCore(declaredField.FieldType))
			{
				return true;
			}
		}
		return false;
	}

	public unsafe static void ClearLessThanPointerSized(byte* ptr, UIntPtr byteLength)
	{
		if (sizeof(UIntPtr) == 4)
		{
			Unsafe.InitBlockUnaligned(ptr, 0, (uint)byteLength);
			return;
		}
		ulong num = (ulong)byteLength;
		uint num2 = (uint)(num & 0xFFFFFFFFu);
		Unsafe.InitBlockUnaligned(ptr, 0, num2);
		num -= num2;
		ptr += num2;
		while (num != 0)
		{
			num2 = (uint)((num >= uint.MaxValue) ? uint.MaxValue : num);
			Unsafe.InitBlockUnaligned(ptr, 0, num2);
			ptr += num2;
			num -= num2;
		}
	}

	public unsafe static void ClearLessThanPointerSized(ref byte b, UIntPtr byteLength)
	{
		if (sizeof(UIntPtr) == 4)
		{
			Unsafe.InitBlockUnaligned(ref b, 0, (uint)byteLength);
			return;
		}
		ulong num = (ulong)byteLength;
		uint num2 = (uint)(num & 0xFFFFFFFFu);
		Unsafe.InitBlockUnaligned(ref b, 0, num2);
		num -= num2;
		long num3 = num2;
		while (num != 0)
		{
			num2 = (uint)((num >= uint.MaxValue) ? uint.MaxValue : num);
			Unsafe.InitBlockUnaligned(ref Unsafe.Add(ref b, (IntPtr)num3), 0, num2);
			num3 += num2;
			num -= num2;
		}
	}

	public unsafe static void ClearPointerSizedWithoutReferences(ref byte b, UIntPtr byteLength)
	{
		IntPtr zero;
		for (zero = IntPtr.Zero; zero.LessThanEqual(byteLength - sizeof(Reg64)); zero += sizeof(Reg64))
		{
			Unsafe.As<byte, Reg64>(ref Unsafe.Add(ref b, zero)) = default(Reg64);
		}
		if (zero.LessThanEqual(byteLength - sizeof(Reg32)))
		{
			Unsafe.As<byte, Reg32>(ref Unsafe.Add(ref b, zero)) = default(Reg32);
			zero += sizeof(Reg32);
		}
		if (zero.LessThanEqual(byteLength - sizeof(Reg16)))
		{
			Unsafe.As<byte, Reg16>(ref Unsafe.Add(ref b, zero)) = default(Reg16);
			zero += sizeof(Reg16);
		}
		if (zero.LessThanEqual(byteLength - 8))
		{
			Unsafe.As<byte, long>(ref Unsafe.Add(ref b, zero)) = 0L;
			zero += 8;
		}
		if (sizeof(IntPtr) == 4 && zero.LessThanEqual(byteLength - 4))
		{
			Unsafe.As<byte, int>(ref Unsafe.Add(ref b, zero)) = 0;
			zero += 4;
		}
	}

	public static void ClearPointerSizedWithReferences(ref IntPtr ip, UIntPtr pointerSizeLength)
	{
		IntPtr intPtr = IntPtr.Zero;
		IntPtr zero = IntPtr.Zero;
		while ((zero = intPtr + 8).LessThanEqual(pointerSizeLength))
		{
			Unsafe.Add(ref ip, intPtr + 0) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 1) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 2) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 3) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 4) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 5) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 6) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 7) = default(IntPtr);
			intPtr = zero;
		}
		if ((zero = intPtr + 4).LessThanEqual(pointerSizeLength))
		{
			Unsafe.Add(ref ip, intPtr + 0) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 1) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 2) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 3) = default(IntPtr);
			intPtr = zero;
		}
		if ((zero = intPtr + 2).LessThanEqual(pointerSizeLength))
		{
			Unsafe.Add(ref ip, intPtr + 0) = default(IntPtr);
			Unsafe.Add(ref ip, intPtr + 1) = default(IntPtr);
			intPtr = zero;
		}
		if ((intPtr + 1).LessThanEqual(pointerSizeLength))
		{
			Unsafe.Add(ref ip, intPtr) = default(IntPtr);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static bool LessThanEqual(this IntPtr index, UIntPtr length)
	{
		if (sizeof(UIntPtr) != 4)
		{
			return (long)index <= (long)(ulong)length;
		}
		return (int)index <= (int)(uint)length;
	}
}
