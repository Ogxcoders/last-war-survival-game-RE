using System;

public class Strtod_CSharp
{
	private static int maxExponent = 511;

	private static double[] powersOf10 = new double[9] { 10.0, 100.0, 10000.0, 100000000.0, 10000000000000000.0, 1E+32, 1E+64, 1E+128, 1E+256 };

	public static int errno = 0;

	public static char CH(ReadOnlySpan<char> buffer, int p)
	{
		if (p >= 0 && p < buffer.Length)
		{
			return buffer[p];
		}
		return '\0';
	}

	public static double strtod(string str)
	{
		return strtod(str.AsSpan());
	}

	public static double strtod(ReadOnlySpan<char> str)
	{
		int value = 0;
		int num = 0;
		int num2 = 0;
		int i;
		for (i = 0; char.IsWhiteSpace(CH(str, i)); i++)
		{
		}
		int value2;
		if (CH(str, i) == '-')
		{
			value2 = 1;
			i++;
		}
		else
		{
			if (CH(str, i) == '+')
			{
				i++;
			}
			value2 = 0;
		}
		int num3 = -1;
		int num4 = 0;
		while (true)
		{
			char c = CH(str, i);
			if (!char.IsDigit(c))
			{
				if (c != '.' || num3 >= 0)
				{
					break;
				}
				num3 = num4;
			}
			i++;
			num4++;
		}
		int num5 = i;
		i -= num4;
		if (num3 < 0)
		{
			num3 = num4;
		}
		else
		{
			num4--;
		}
		if (num4 > 18)
		{
			num2 = num3 - 18;
			num4 = 18;
		}
		else
		{
			num2 = num3 - num4;
		}
		double num6;
		if (num4 == 0)
		{
			num6 = 0.0;
			i = 0;
		}
		else
		{
			int num7 = 0;
			while (num4 > 9)
			{
				char c = CH(str, i);
				i++;
				if (c == '.')
				{
					c = CH(str, i);
					i++;
				}
				num7 = 10 * num7 + (c - 48);
				num4--;
			}
			int num8 = 0;
			while (num4 > 0)
			{
				char c = CH(str, i);
				i++;
				if (c == '.')
				{
					c = CH(str, i);
					i++;
				}
				num8 = 10 * num8 + (c - 48);
				num4--;
			}
			num6 = 1000000000.0 * (double)num7 + (double)num8;
			i = num5;
			if (CH(str, i) == 'E' || CH(str, i) == 'e')
			{
				i++;
				if (CH(str, i) == '-')
				{
					value = 1;
					i++;
				}
				else
				{
					if (CH(str, i) == '+')
					{
						i++;
					}
					value = 0;
				}
				if (!char.IsDigit(CH(str, i)))
				{
					i = num5;
					goto IL_028c;
				}
				for (; char.IsDigit(CH(str, i)); i++)
				{
					num = num * 10 + (CH(str, i) - 48);
				}
			}
			num = ((!Convert.ToBoolean(value)) ? (num2 + num) : (num2 - num));
			if (num < 0)
			{
				value = 1;
				num = -num;
			}
			else
			{
				value = 0;
			}
			if (num > maxExponent)
			{
				num = maxExponent;
				errno = -1;
			}
			double num9 = 1.0;
			int num10 = 0;
			while (num != 0)
			{
				if (Convert.ToBoolean(num & 1))
				{
					num9 *= powersOf10[num10];
				}
				if (num10 >= powersOf10.Length)
				{
					errno = -2;
					break;
				}
				num >>= 1;
				num10++;
			}
			num6 = ((!Convert.ToBoolean(value)) ? (num6 * num9) : (num6 / num9));
		}
		goto IL_028c;
		IL_028c:
		if (Convert.ToBoolean(value2))
		{
			return 0.0 - num6;
		}
		return num6;
	}
}
