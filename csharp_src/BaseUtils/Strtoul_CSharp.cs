using System;

public class Strtoul_CSharp
{
	public static int errno;

	public static char CH(ReadOnlySpan<char> buffer, int p)
	{
		if (p >= 0 && p < buffer.Length)
		{
			return buffer[p];
		}
		return '\0';
	}

	public static ulong strtoul(string str)
	{
		return strtoul(str.AsSpan());
	}

	public static ulong strtoul(ReadOnlySpan<char> str)
	{
		int num = 0;
		int num2 = 0;
		char c;
		do
		{
			c = CH(str, num2++);
		}
		while (char.IsWhiteSpace(c));
		switch (c)
		{
		case '-':
			c = CH(str, num2++);
			break;
		case '+':
			c = CH(str, num2++);
			break;
		}
		if ((num == 0 || num == 16) && c == '0' && (CH(str, num2) == 'x' || CH(str, num2) == 'X'))
		{
			c = CH(str, num2 + 1);
			num2 += 2;
			num = 16;
		}
		if (num == 0)
		{
			num = ((c == '0') ? 8 : 10);
		}
		ulong num3 = ulong.MaxValue / (ulong)num;
		int num4 = (int)(ulong.MaxValue % (ulong)num);
		ulong num5 = 0uL;
		int num6 = 0;
		while (true)
		{
			if (char.IsDigit(c))
			{
				c = (char)(c - 48);
			}
			else
			{
				if (!char.IsLetter(c))
				{
					break;
				}
				c = (char)(c - (char.IsUpper(c) ? 55 : 87));
			}
			if (c >= num)
			{
				break;
			}
			if (num6 < 0 || num5 > num3 || (num5 == num3 && c > num4))
			{
				num6 = -1;
			}
			else
			{
				num6 = 1;
				num5 *= (ulong)num;
				num5 += c;
			}
			c = CH(str, num2++);
		}
		if (num6 < 0)
		{
			num5 = ulong.MaxValue;
			errno = -2;
		}
		return num5;
	}
}
