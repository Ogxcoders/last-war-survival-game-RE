using System;

namespace FibMatrix;

public static class Ascii85
{
	private const int BlockSize = 4;

	private const int EncodedBlockSize = 5;

	public static string Encode(byte[] bytes)
	{
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		int num = bytes.Length;
		if (num == 0)
		{
			return string.Empty;
		}
		int num2 = num % 4;
		int num3 = (4 - num2) % 4;
		int num4 = (num + num3) / 4;
		char[] array = new char[num4 * 5];
		for (int i = 0; i < num4; i++)
		{
			int num5 = i * 4;
			uint num6 = 0u;
			for (int j = 0; j < 4; j++)
			{
				int num7 = num5 + j;
				byte b = (byte)((num7 < num) ? bytes[num7] : 0);
				num6 |= (uint)(b << 24 - j * 8);
			}
			if (num6 == 0 && i == num4 - 1 && num3 > 0)
			{
				array[i * 5] = 'z';
				continue;
			}
			for (int num8 = 4; num8 >= 0; num8--)
			{
				array[i * 5 + num8] = (char)(num6 % 85 + 33);
				num6 /= 85;
			}
		}
		return new string(array, 0, array.Length - num3);
	}

	public static byte[] Decode(string encoded)
	{
		if (encoded == null)
		{
			throw new ArgumentNullException("encoded");
		}
		if (encoded.Length == 0)
		{
			return Array.Empty<byte>();
		}
		int length = encoded.Length;
		int num = length / 5;
		if (length % 5 != 0)
		{
			num++;
		}
		byte[] array = new byte[num * 4];
		int num2 = 0;
		uint num3 = 0u;
		int num4 = 0;
		foreach (char c in encoded)
		{
			if (c == 'z' && num4 == 0)
			{
				Array.Clear(array, num2, 4);
				num2 += 4;
				continue;
			}
			if (c < '!' || c > 'u')
			{
				throw new ArgumentException("Invalid character in Ascii85 string");
			}
			num3 = num3 * 85 + (uint)(c - 33);
			num4++;
			if (num4 == 5)
			{
				for (int num5 = 3; num5 >= 0; num5--)
				{
					array[num2 + num5] = (byte)(num3 & 0xFF);
					num3 >>= 8;
				}
				num2 += 4;
				num4 = 0;
				num3 = 0u;
			}
		}
		if (num4 > 0)
		{
			for (int j = num4; j < 5; j++)
			{
				num3 = num3 * 85 + 84;
			}
			for (int num6 = 3; num6 >= 0; num6--)
			{
				if (num2 + num6 < array.Length)
				{
					array[num2 + num6] = (byte)(num3 & 0xFF);
				}
				num3 >>= 8;
			}
			num2 += 4;
		}
		Array.Resize(ref array, num2);
		return array;
	}
}
