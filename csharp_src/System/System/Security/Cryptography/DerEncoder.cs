using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace System.Security.Cryptography;

internal static class DerEncoder
{
	private class AsnSetValueComparer : IComparer<byte[][]>, IComparer
	{
		public static AsnSetValueComparer Instance { get; } = new AsnSetValueComparer();

		public int Compare(byte[][] x, byte[][] y)
		{
			int num = x[0][0] - y[0][0];
			if (num != 0)
			{
				return num;
			}
			num = x[2].Length - y[2].Length;
			if (num != 0)
			{
				return num;
			}
			for (int i = 0; i < x[2].Length; i++)
			{
				num = x[2][i] - y[2][i];
				if (num != 0)
				{
					return num;
				}
			}
			return 0;
		}

		public int Compare(object x, object y)
		{
			return Compare(x as byte[][], y as byte[][]);
		}
	}

	private const byte ConstructedFlag = 32;

	private const byte ConstructedSequenceTag = 48;

	private const byte ConstructedSetTag = 49;

	private static readonly byte[][] s_nullTlv = new byte[3][]
	{
		new byte[1] { 5 },
		new byte[1],
		Array.Empty<byte>()
	};

	private static byte[] EncodeLength(int length)
	{
		byte b = (byte)length;
		if (length >= 128)
		{
			if (length > 255)
			{
				int num = length >> 8;
				byte b2 = (byte)num;
				if (length > 65535)
				{
					num >>= 8;
					byte b3 = (byte)num;
					if (length > 16777215)
					{
						num >>= 8;
						byte b4 = (byte)num;
						return new byte[5] { 132, b4, b3, b2, b };
					}
					return new byte[4] { 131, b3, b2, b };
				}
				return new byte[3] { 130, b2, b };
			}
			return new byte[2] { 129, b };
		}
		return new byte[1] { b };
	}

	internal static byte[][] SegmentedEncodeBoolean(bool value)
	{
		byte[] array = new byte[1] { (byte)(value ? 255u : 0u) };
		return new byte[3][]
		{
			new byte[1] { 1 },
			new byte[1] { 1 },
			array
		};
	}

	internal static byte[][] SegmentedEncodeUnsignedInteger(uint value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		if (BitConverter.IsLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return SegmentedEncodeUnsignedInteger(bytes);
	}

	internal static byte[][] SegmentedEncodeUnsignedInteger(ReadOnlySpan<byte> bigEndianBytes)
	{
		int i = 0;
		int num;
		for (num = i + bigEndianBytes.Length; i < num && bigEndianBytes[i] == 0; i++)
		{
		}
		if (i == num)
		{
			i--;
		}
		int num2 = num - i;
		int num3 = ((bigEndianBytes[i] > 127) ? 1 : 0);
		byte[] array = new byte[num2 + num3];
		bigEndianBytes.Slice(i, num2).CopyTo(new Span<byte>(array).Slice(num3));
		return new byte[3][]
		{
			new byte[1] { 2 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] SegmentedEncodeBitString(params byte[][][] childSegments)
	{
		return SegmentedEncodeBitString(ConcatenateArrays(childSegments));
	}

	internal static byte[][] SegmentedEncodeBitString(byte[] data)
	{
		return SegmentedEncodeBitString(0, data);
	}

	internal static byte[][] SegmentedEncodeBitString(int unusedBits, byte[] data)
	{
		byte[] array = new byte[data.Length + 1];
		Buffer.BlockCopy(data, 0, array, 1, data.Length);
		array[0] = (byte)unusedBits;
		byte b = (byte)(-1 << unusedBits);
		array[data.Length] &= b;
		return new byte[3][]
		{
			new byte[1] { 3 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] SegmentedEncodeNamedBitList(byte[] bigEndianBytes, int namedBitsCount)
	{
		int num = -1;
		for (int num2 = Math.Min(bigEndianBytes.Length * 8 - 1, namedBitsCount - 1); num2 >= 0; num2--)
		{
			int num3 = num2 / 8;
			int num4 = 7 - num2 % 8;
			int num5 = 1 << num4;
			if ((bigEndianBytes[num3] & num5) == num5)
			{
				num = num2;
				break;
			}
		}
		byte[] array;
		if (num >= 0)
		{
			int num6 = num + 1;
			int num7 = (7 + num6) / 8;
			int num8 = 7 - num % 8;
			byte b = (byte)(-1 << num8);
			array = new byte[num7 + 1];
			array[0] = (byte)num8;
			Buffer.BlockCopy(bigEndianBytes, 0, array, 1, num7);
			array[num7] &= b;
		}
		else
		{
			array = new byte[1];
		}
		return new byte[3][]
		{
			new byte[1] { 3 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] SegmentedEncodeOctetString(byte[] data)
	{
		return new byte[3][]
		{
			new byte[1] { 4 },
			EncodeLength(data.Length),
			data
		};
	}

	internal static byte[][] SegmentedEncodeNull()
	{
		return s_nullTlv;
	}

	internal static byte[] EncodeOid(string oidValue)
	{
		return ConcatenateArrays(SegmentedEncodeOid(oidValue));
	}

	internal static byte[][] SegmentedEncodeOid(Oid oid)
	{
		return SegmentedEncodeOid(oid.Value);
	}

	internal static byte[][] SegmentedEncodeOid(string oidValue)
	{
		if (string.IsNullOrEmpty(oidValue))
		{
			throw new CryptographicException("The OID value was invalid.");
		}
		if (oidValue.Length < 3)
		{
			throw new CryptographicException("The OID value was invalid.");
		}
		if (oidValue[1] != '.')
		{
			throw new CryptographicException("The OID value was invalid.");
		}
		int num = oidValue[0] switch
		{
			'0' => 0, 
			'1' => 1, 
			'2' => 2, 
			_ => throw new CryptographicException("The OID value was invalid."), 
		};
		int startIndex = 2;
		BigInteger rid = ParseOidRid(oidValue, ref startIndex);
		rid += (BigInteger)(40 * num);
		List<byte> list = new List<byte>(oidValue.Length / 2);
		EncodeRid(list, ref rid);
		while (startIndex < oidValue.Length)
		{
			rid = ParseOidRid(oidValue, ref startIndex);
			EncodeRid(list, ref rid);
		}
		return new byte[3][]
		{
			new byte[1] { 6 },
			EncodeLength(list.Count),
			list.ToArray()
		};
	}

	internal static byte[][] SegmentedEncodeUtf8String(char[] chars)
	{
		return SegmentedEncodeUtf8String(chars, 0, chars.Length);
	}

	internal static byte[][] SegmentedEncodeUtf8String(char[] chars, int offset, int count)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(chars, offset, count);
		return new byte[3][]
		{
			new byte[1] { 12 },
			EncodeLength(bytes.Length),
			bytes
		};
	}

	internal static byte[][] ConstructSegmentedSequence(params byte[][][] items)
	{
		return ConstructSegmentedSequence((IEnumerable<byte[][]>)items);
	}

	internal static byte[][] ConstructSegmentedSequence(IEnumerable<byte[][]> items)
	{
		byte[] array = ConcatenateArrays(items);
		return new byte[3][]
		{
			new byte[1] { 48 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] ConstructSegmentedContextSpecificValue(int contextId, params byte[][][] items)
	{
		byte[] array = ConcatenateArrays(items);
		byte b = (byte)(0xA0 | contextId);
		return new byte[3][]
		{
			new byte[1] { b },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] ConstructSegmentedSet(params byte[][][] items)
	{
		byte[][][] obj = (byte[][][])items.Clone();
		Array.Sort(obj, AsnSetValueComparer.Instance);
		byte[] array = ConcatenateArrays(obj);
		return new byte[3][]
		{
			new byte[1] { 49 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] ConstructSegmentedPresortedSet(params byte[][][] items)
	{
		byte[] array = ConcatenateArrays(items);
		return new byte[3][]
		{
			new byte[1] { 49 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static bool IsValidPrintableString(char[] chars)
	{
		return IsValidPrintableString(chars, 0, chars.Length);
	}

	internal static bool IsValidPrintableString(char[] chars, int offset, int count)
	{
		int num = count + offset;
		for (int i = offset; i < num; i++)
		{
			if (!IsPrintableStringCharacter(chars[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal static byte[][] SegmentedEncodePrintableString(char[] chars)
	{
		return SegmentedEncodePrintableString(chars, 0, chars.Length);
	}

	internal static byte[][] SegmentedEncodePrintableString(char[] chars, int offset, int count)
	{
		byte[] array = new byte[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = (byte)chars[i + offset];
		}
		return new byte[3][]
		{
			new byte[1] { 19 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] SegmentedEncodeIA5String(char[] chars)
	{
		return SegmentedEncodeIA5String(chars, 0, chars.Length);
	}

	internal static byte[][] SegmentedEncodeIA5String(char[] chars, int offset, int count)
	{
		byte[] array = new byte[count];
		for (int i = 0; i < count; i++)
		{
			char c = chars[i + offset];
			if (c > '\u007f')
			{
				throw new CryptographicException("The string contains a character not in the 7 bit ASCII character set.");
			}
			array[i] = (byte)c;
		}
		return new byte[3][]
		{
			new byte[1] { 22 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] SegmentedEncodeUtcTime(DateTime utcTime)
	{
		byte[] array = new byte[13];
		int year = utcTime.Year;
		int month = utcTime.Month;
		int day = utcTime.Day;
		int hour = utcTime.Hour;
		int minute = utcTime.Minute;
		int second = utcTime.Second;
		array[1] = (byte)(48 + year % 10);
		year /= 10;
		array[0] = (byte)(48 + year % 10);
		array[3] = (byte)(48 + month % 10);
		month /= 10;
		array[2] = (byte)(48 + month % 10);
		array[5] = (byte)(48 + day % 10);
		day /= 10;
		array[4] = (byte)(48 + day % 10);
		array[7] = (byte)(48 + hour % 10);
		hour /= 10;
		array[6] = (byte)(48 + hour % 10);
		array[9] = (byte)(48 + minute % 10);
		minute /= 10;
		array[8] = (byte)(48 + minute % 10);
		array[11] = (byte)(48 + second % 10);
		second /= 10;
		array[10] = (byte)(48 + second % 10);
		array[12] = 90;
		return new byte[3][]
		{
			new byte[1] { 23 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[][] SegmentedEncodeGeneralizedTime(DateTime utcTime)
	{
		byte[] array = new byte[15];
		int year = utcTime.Year;
		int month = utcTime.Month;
		int day = utcTime.Day;
		int hour = utcTime.Hour;
		int minute = utcTime.Minute;
		int second = utcTime.Second;
		array[3] = (byte)(48 + year % 10);
		year /= 10;
		array[2] = (byte)(48 + year % 10);
		year /= 10;
		array[1] = (byte)(48 + year % 10);
		year /= 10;
		array[0] = (byte)(48 + year % 10);
		array[5] = (byte)(48 + month % 10);
		month /= 10;
		array[4] = (byte)(48 + month % 10);
		array[7] = (byte)(48 + day % 10);
		day /= 10;
		array[6] = (byte)(48 + day % 10);
		array[9] = (byte)(48 + hour % 10);
		hour /= 10;
		array[8] = (byte)(48 + hour % 10);
		array[11] = (byte)(48 + minute % 10);
		minute /= 10;
		array[10] = (byte)(48 + minute % 10);
		array[13] = (byte)(48 + second % 10);
		second /= 10;
		array[12] = (byte)(48 + second % 10);
		array[14] = 90;
		return new byte[3][]
		{
			new byte[1] { 24 },
			EncodeLength(array.Length),
			array
		};
	}

	internal static byte[] ConstructSequence(params byte[][][] items)
	{
		return ConstructSequence((IEnumerable<byte[][]>)items);
	}

	internal static byte[] ConstructSequence(IEnumerable<byte[][]> items)
	{
		int num = 0;
		foreach (byte[][] item in items)
		{
			foreach (byte[] array in item)
			{
				num += array.Length;
			}
		}
		byte[] array2 = EncodeLength(num);
		byte[] array3 = new byte[1 + array2.Length + num];
		array3[0] = 48;
		int num2 = 1;
		Buffer.BlockCopy(array2, 0, array3, num2, array2.Length);
		num2 += array2.Length;
		foreach (byte[][] item2 in items)
		{
			foreach (byte[] array4 in item2)
			{
				Buffer.BlockCopy(array4, 0, array3, num2, array4.Length);
				num2 += array4.Length;
			}
		}
		return array3;
	}

	private static BigInteger ParseOidRid(string oidValue, ref int startIndex)
	{
		int num = oidValue.IndexOf('.', startIndex);
		if (num == -1)
		{
			num = oidValue.Length;
		}
		BigInteger zero = BigInteger.Zero;
		for (int i = startIndex; i < num; i++)
		{
			zero *= (BigInteger)10;
			zero += (BigInteger)AtoI(oidValue[i]);
		}
		startIndex = num + 1;
		return zero;
	}

	private static int AtoI(char c)
	{
		if (c >= '0' && c <= '9')
		{
			return c - 48;
		}
		throw new CryptographicException("The OID value was invalid.");
	}

	private static void EncodeRid(List<byte> encodedData, ref BigInteger rid)
	{
		BigInteger divisor = new BigInteger(128);
		BigInteger bigInteger = rid;
		Stack<byte> stack = new Stack<byte>();
		byte b = 0;
		do
		{
			bigInteger = BigInteger.DivRem(bigInteger, divisor, out var remainder);
			byte b2 = (byte)remainder;
			b2 |= b;
			b = 128;
			stack.Push(b2);
		}
		while (bigInteger != BigInteger.Zero);
		encodedData.AddRange(stack);
	}

	private static bool IsPrintableStringCharacter(char c)
	{
		if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
		{
			return true;
		}
		switch (c)
		{
		case ' ':
		case '\'':
		case '(':
		case ')':
		case '+':
		case ',':
		case '-':
		case '.':
		case '/':
		case ':':
		case '=':
		case '?':
			return true;
		default:
			return false;
		}
	}

	private static byte[] ConcatenateArrays(params byte[][][] segments)
	{
		return ConcatenateArrays((IEnumerable<byte[][]>)segments);
	}

	private static byte[] ConcatenateArrays(IEnumerable<byte[][]> segments)
	{
		int num = 0;
		foreach (byte[][] segment in segments)
		{
			foreach (byte[] array in segment)
			{
				num += array.Length;
			}
		}
		byte[] array2 = new byte[num];
		int num2 = 0;
		foreach (byte[][] segment2 in segments)
		{
			foreach (byte[] array3 in segment2)
			{
				Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
				num2 += array3.Length;
			}
		}
		return array2;
	}
}
