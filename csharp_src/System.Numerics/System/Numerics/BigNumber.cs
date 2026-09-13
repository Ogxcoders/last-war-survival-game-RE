using System.Buffers;
using System.Globalization;
using System.Text;

namespace System.Numerics;

internal static class BigNumber
{
	private struct BigNumberBuffer
	{
		public StringBuilder digits;

		public int precision;

		public int scale;

		public bool sign;

		public static BigNumberBuffer Create()
		{
			return new BigNumberBuffer
			{
				digits = new StringBuilder()
			};
		}
	}

	private const NumberStyles InvalidNumberStyles = ~(NumberStyles.Any | NumberStyles.AllowHexSpecifier);

	internal static bool TryValidateParseStyleInteger(NumberStyles style, out ArgumentException e)
	{
		if ((style & ~(NumberStyles.Any | NumberStyles.AllowHexSpecifier)) != NumberStyles.None)
		{
			e = new ArgumentException(global::SR.Format("An undefined NumberStyles value is being used.", "style"));
			return false;
		}
		if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None && (style & ~NumberStyles.HexNumber) != NumberStyles.None)
		{
			e = new ArgumentException("With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of those in HexNumber.");
			return false;
		}
		e = null;
		return true;
	}

	internal static bool TryParseBigInteger(string value, NumberStyles style, NumberFormatInfo info, out BigInteger result)
	{
		if (value == null)
		{
			result = default(BigInteger);
			return false;
		}
		return TryParseBigInteger(value.AsSpan(), style, info, out result);
	}

	internal static bool TryParseBigInteger(ReadOnlySpan<char> value, NumberStyles style, NumberFormatInfo info, out BigInteger result)
	{
		result = BigInteger.Zero;
		if (!TryValidateParseStyleInteger(style, out var e))
		{
			throw e;
		}
		BigNumberBuffer number = BigNumberBuffer.Create();
		if (!FormatProvider.TryStringToBigInteger(value, style, info, number.digits, out number.precision, out number.scale, out number.sign))
		{
			return false;
		}
		if ((style & NumberStyles.AllowHexSpecifier) != NumberStyles.None)
		{
			if (!HexNumberToBigInteger(ref number, ref result))
			{
				return false;
			}
		}
		else if (!NumberToBigInteger(ref number, ref result))
		{
			return false;
		}
		return true;
	}

	internal static BigInteger ParseBigInteger(string value, NumberStyles style, NumberFormatInfo info)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		return ParseBigInteger(value.AsSpan(), style, info);
	}

	internal static BigInteger ParseBigInteger(ReadOnlySpan<char> value, NumberStyles style, NumberFormatInfo info)
	{
		if (!TryValidateParseStyleInteger(style, out var e))
		{
			throw e;
		}
		BigInteger result = BigInteger.Zero;
		if (!TryParseBigInteger(value, style, info, out result))
		{
			throw new FormatException("The value could not be parsed.");
		}
		return result;
	}

	private static bool HexNumberToBigInteger(ref BigNumberBuffer number, ref BigInteger value)
	{
		if (number.digits == null || number.digits.Length == 0)
		{
			return false;
		}
		int num = number.digits.Length - 1;
		byte[] array = new byte[num / 2 + num % 2];
		bool flag = false;
		bool flag2 = false;
		int num2 = 0;
		for (int num3 = num - 1; num3 > -1; num3--)
		{
			char c = number.digits[num3];
			byte b = ((c >= '0' && c <= '9') ? ((byte)(c - 48)) : ((c < 'A' || c > 'F') ? ((byte)(c - 97 + 10)) : ((byte)(c - 65 + 10))));
			if (num3 == 0 && (b & 8) == 8)
			{
				flag2 = true;
			}
			if (flag)
			{
				array[num2] = (byte)(array[num2] | (b << 4));
				num2++;
			}
			else
			{
				array[num2] = (flag2 ? ((byte)(b | 0xF0)) : b);
			}
			flag = !flag;
		}
		value = new BigInteger(array);
		return true;
	}

	private static bool NumberToBigInteger(ref BigNumberBuffer number, ref BigInteger value)
	{
		int num = number.scale;
		int index = 0;
		BigInteger bigInteger = 10;
		value = 0;
		while (--num >= 0)
		{
			value *= bigInteger;
			if (number.digits[index] != 0)
			{
				value += (BigInteger)(number.digits[index++] - 48);
			}
		}
		while (number.digits[index] != 0)
		{
			if (number.digits[index++] != '0')
			{
				return false;
			}
		}
		if (number.sign)
		{
			value = -value;
		}
		return true;
	}

	internal static char ParseFormatSpecifier(ReadOnlySpan<char> format, out int digits)
	{
		digits = -1;
		if (format.Length == 0)
		{
			return 'R';
		}
		int num = 0;
		char c = format[num];
		if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
		{
			num++;
			int num2 = -1;
			if (num < format.Length && format[num] >= '0' && format[num] <= '9')
			{
				num2 = format[num++] - 48;
				while (num < format.Length && format[num] >= '0' && format[num] <= '9')
				{
					num2 = num2 * 10 + (format[num++] - 48);
					if (num2 >= 10)
					{
						break;
					}
				}
			}
			if (num >= format.Length || format[num] == '\0')
			{
				digits = num2;
				return c;
			}
		}
		return '\0';
	}

	private static string FormatBigIntegerToHex(bool targetSpan, BigInteger value, char format, int digits, NumberFormatInfo info, Span<char> destination, out int charsWritten, out bool spanSuccess)
	{
		byte[] array = null;
		Span<byte> destination2 = stackalloc byte[64];
		if (!value.TryWriteOrCountBytes(destination2, out var bytesWritten))
		{
			destination2 = (array = ArrayPool<byte>.Shared.Rent(bytesWritten));
			value.TryWriteBytes(destination2, out bytesWritten);
		}
		destination2 = destination2.Slice(0, bytesWritten);
		Span<char> initialBuffer = stackalloc char[128];
		System.Text.ValueStringBuilder valueStringBuilder = new System.Text.ValueStringBuilder(initialBuffer);
		int num = destination2.Length - 1;
		if (num > -1)
		{
			bool flag = false;
			byte b = destination2[num];
			if (b > 247)
			{
				b -= 240;
				flag = true;
			}
			if (b < 8 || flag)
			{
				valueStringBuilder.Append((b < 10) ? ((char)(b + 48)) : ((format == 'X') ? ((char)((b & 0xF) - 10 + 65)) : ((char)((b & 0xF) - 10 + 97))));
				num--;
			}
		}
		if (num > -1)
		{
			Span<char> span = valueStringBuilder.AppendSpan((num + 1) * 2);
			int num2 = 0;
			string text = ((format == 'x') ? "0123456789abcdef" : "0123456789ABCDEF");
			while (num > -1)
			{
				byte b2 = destination2[num--];
				span[num2++] = text[b2 >> 4];
				span[num2++] = text[b2 & 0xF];
			}
		}
		if (digits > valueStringBuilder.Length)
		{
			valueStringBuilder.Insert(0, (value._sign >= 0) ? '0' : ((format == 'x') ? 'f' : 'F'), digits - valueStringBuilder.Length);
		}
		if (array != null)
		{
			ArrayPool<byte>.Shared.Return(array);
		}
		if (targetSpan)
		{
			spanSuccess = valueStringBuilder.TryCopyTo(destination, out charsWritten);
			return null;
		}
		charsWritten = 0;
		spanSuccess = false;
		return valueStringBuilder.ToString();
	}

	internal static string FormatBigInteger(BigInteger value, string format, NumberFormatInfo info)
	{
		int charsWritten;
		bool spanSuccess;
		return FormatBigInteger(targetSpan: false, value, format, format, info, default(Span<char>), out charsWritten, out spanSuccess);
	}

	internal static bool TryFormatBigInteger(BigInteger value, ReadOnlySpan<char> format, NumberFormatInfo info, Span<char> destination, out int charsWritten)
	{
		FormatBigInteger(targetSpan: true, value, null, format, info, destination, out charsWritten, out var spanSuccess);
		return spanSuccess;
	}

	private static string FormatBigInteger(bool targetSpan, BigInteger value, string formatString, ReadOnlySpan<char> formatSpan, NumberFormatInfo info, Span<char> destination, out int charsWritten, out bool spanSuccess)
	{
		int digits = 0;
		char c = ParseFormatSpecifier(formatSpan, out digits);
		if (c == 'x' || c == 'X')
		{
			return FormatBigIntegerToHex(targetSpan, value, c, digits, info, destination, out charsWritten, out spanSuccess);
		}
		if (value._bits == null)
		{
			if (c == 'g' || c == 'G' || c == 'r' || c == 'R')
			{
				formatSpan = (formatString = ((digits > 0) ? $"D{digits}" : "D"));
			}
			if (targetSpan)
			{
				spanSuccess = value._sign.TryFormat(destination, out charsWritten, formatSpan, info);
				return null;
			}
			charsWritten = 0;
			spanSuccess = false;
			return value._sign.ToString(formatString, info);
		}
		int num = value._bits.Length;
		uint[] array;
		int num3;
		int num4;
		checked
		{
			int num2;
			try
			{
				num2 = unchecked(checked(num * 10) / 9) + 2;
			}
			catch (OverflowException innerException)
			{
				throw new FormatException("The value is too large to be represented by this format specifier.", innerException);
			}
			array = new uint[num2];
			num3 = 0;
			num4 = num;
		}
		while (--num4 >= 0)
		{
			uint num5 = value._bits[num4];
			for (int i = 0; i < num3; i++)
			{
				ulong num6 = NumericsHelpers.MakeUlong(array[i], num5);
				array[i] = (uint)(num6 % 1000000000);
				num5 = (uint)(num6 / 1000000000);
			}
			if (num5 != 0)
			{
				array[num3++] = num5 % 1000000000;
				num5 /= 1000000000;
				if (num5 != 0)
				{
					array[num3++] = num5;
				}
			}
		}
		int num7;
		bool flag;
		char[] array2;
		int num9;
		checked
		{
			try
			{
				num7 = num3 * 9;
			}
			catch (OverflowException innerException2)
			{
				throw new FormatException("The value is too large to be represented by this format specifier.", innerException2);
			}
			flag = c == 'g' || c == 'G' || c == 'd' || c == 'D' || c == 'r' || c == 'R';
			if (flag)
			{
				if (digits > 0 && digits > num7)
				{
					num7 = digits;
				}
				if (value._sign < 0)
				{
					try
					{
						num7 += info.NegativeSign.Length;
					}
					catch (OverflowException innerException3)
					{
						throw new FormatException("The value is too large to be represented by this format specifier.", innerException3);
					}
				}
			}
			int num8;
			try
			{
				num8 = num7 + 1;
			}
			catch (OverflowException innerException4)
			{
				throw new FormatException("The value is too large to be represented by this format specifier.", innerException4);
			}
			array2 = new char[num8];
			num9 = num7;
		}
		for (int j = 0; j < num3 - 1; j++)
		{
			uint num10 = array[j];
			int num11 = 9;
			while (--num11 >= 0)
			{
				array2[--num9] = (char)(48 + num10 % 10);
				num10 /= 10;
			}
		}
		for (uint num12 = array[num3 - 1]; num12 != 0; num12 /= 10)
		{
			array2[--num9] = (char)(48 + num12 % 10);
		}
		if (!flag)
		{
			bool sign = value._sign < 0;
			int precision = 29;
			int scale = num7 - num9;
			Span<char> initialBuffer = stackalloc char[128];
			System.Text.ValueStringBuilder sb = new System.Text.ValueStringBuilder(initialBuffer);
			FormatProvider.FormatBigInteger(ref sb, precision, scale, sign, formatSpan, info, array2, num9);
			if (targetSpan)
			{
				spanSuccess = sb.TryCopyTo(destination, out charsWritten);
				return null;
			}
			charsWritten = 0;
			spanSuccess = false;
			return sb.ToString();
		}
		int num13 = num7 - num9;
		while (digits > 0 && digits > num13)
		{
			array2[--num9] = '0';
			digits--;
		}
		if (value._sign < 0)
		{
			_ = info.NegativeSign;
			for (int num14 = info.NegativeSign.Length - 1; num14 > -1; num14--)
			{
				array2[--num9] = info.NegativeSign[num14];
			}
		}
		int num15 = num7 - num9;
		if (!targetSpan)
		{
			charsWritten = 0;
			spanSuccess = false;
			return new string(array2, num9, num7 - num9);
		}
		if (new ReadOnlySpan<char>(array2, num9, num7 - num9).TryCopyTo(destination))
		{
			charsWritten = num15;
			spanSuccess = true;
			return null;
		}
		charsWritten = 0;
		spanSuccess = false;
		return null;
	}
}
