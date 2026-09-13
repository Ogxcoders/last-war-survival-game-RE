namespace System.Security.Util;

internal static class Hex
{
	private static char HexDigit(int num)
	{
		return (char)((num < 10) ? (num + 48) : (num + 55));
	}

	public static string EncodeHexString(byte[] sArray)
	{
		string result = null;
		if (sArray != null)
		{
			char[] array = new char[sArray.Length * 2];
			int i = 0;
			int num = 0;
			for (; i < sArray.Length; i++)
			{
				int num2 = (sArray[i] & 0xF0) >> 4;
				array[num++] = HexDigit(num2);
				num2 = sArray[i] & 0xF;
				array[num++] = HexDigit(num2);
			}
			result = new string(array);
		}
		return result;
	}

	internal static string EncodeHexStringFromInt(byte[] sArray)
	{
		string result = null;
		if (sArray != null)
		{
			char[] array = new char[sArray.Length * 2];
			int num = sArray.Length;
			int num2 = 0;
			while (num-- > 0)
			{
				int num3 = (sArray[num] & 0xF0) >> 4;
				array[num2++] = HexDigit(num3);
				num3 = sArray[num] & 0xF;
				array[num2++] = HexDigit(num3);
			}
			result = new string(array);
		}
		return result;
	}

	public static int ConvertHexDigit(char val)
	{
		if (val <= '9' && val >= '0')
		{
			return val - 48;
		}
		if (val >= 'a' && val <= 'f')
		{
			return val - 97 + 10;
		}
		if (val >= 'A' && val <= 'F')
		{
			return val - 65 + 10;
		}
		throw new ArgumentException(Environment.GetResourceString("Index was out of range. Must be non-negative and less than the size of the collection."));
	}

	public static byte[] DecodeHexString(string hexString)
	{
		if (hexString == null)
		{
			throw new ArgumentNullException("hexString");
		}
		bool flag = false;
		int num = 0;
		int num2 = hexString.Length;
		if (num2 >= 2 && hexString[0] == '0' && (hexString[1] == 'x' || hexString[1] == 'X'))
		{
			num2 = hexString.Length - 2;
			num = 2;
		}
		if (num2 % 2 != 0 && num2 % 3 != 2)
		{
			throw new ArgumentException(Environment.GetResourceString("Improperly formatted hex string."));
		}
		byte[] array;
		if (num2 >= 3 && hexString[num + 2] == ' ')
		{
			flag = true;
			array = new byte[num2 / 3 + 1];
		}
		else
		{
			array = new byte[num2 / 2];
		}
		int num3 = 0;
		while (num < hexString.Length)
		{
			int num4 = ConvertHexDigit(hexString[num]);
			int num5 = ConvertHexDigit(hexString[num + 1]);
			array[num3] = (byte)(num5 | (num4 << 4));
			if (flag)
			{
				num++;
			}
			num += 2;
			num3++;
		}
		return array;
	}
}
