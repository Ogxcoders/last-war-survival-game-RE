using UnityEngine;

namespace Tayx.Graphy.Utils.NumString;

public static class G_FloatString
{
	private const string floatFormat = "0.0";

	private static float decimalMultiplier = 1f;

	private static string[] negativeBuffer = new string[0];

	private static string[] positiveBuffer = new string[0];

	public static bool Inited
	{
		get
		{
			if (negativeBuffer.Length == 0)
			{
				return positiveBuffer.Length != 0;
			}
			return true;
		}
	}

	public static float MinValue => 0f - (negativeBuffer.Length - 1).FromIndex();

	public static float MaxValue => (positiveBuffer.Length - 1).FromIndex();

	public static void Init(float minNegativeValue, float maxPositiveValue, int decimals = 1)
	{
		decimalMultiplier = Pow(10, Mathf.Clamp(decimals, 1, 5));
		int num = minNegativeValue.ToIndex();
		int num2 = maxPositiveValue.ToIndex();
		if (num >= 0)
		{
			negativeBuffer = new string[num];
			for (int i = 0; i < num; i++)
			{
				negativeBuffer[i] = (-i).FromIndex().ToString("0.0");
			}
		}
		if (num2 >= 0)
		{
			positiveBuffer = new string[num2];
			for (int j = 0; j < num2; j++)
			{
				positiveBuffer[j] = j.FromIndex().ToString("0.0");
			}
		}
	}

	public static string ToStringNonAlloc(this float value)
	{
		int num = value.ToIndex();
		if (value < 0f && num < negativeBuffer.Length)
		{
			return negativeBuffer[num];
		}
		if (value >= 0f && num < positiveBuffer.Length)
		{
			return positiveBuffer[num];
		}
		return value.ToString();
	}

	public static string ToStringNonAlloc(this float value, string format)
	{
		int num = value.ToIndex();
		if (value < 0f && num < negativeBuffer.Length)
		{
			return negativeBuffer[num];
		}
		if (value >= 0f && num < positiveBuffer.Length)
		{
			return positiveBuffer[num];
		}
		return value.ToString(format);
	}

	public static int ToInt(this float f)
	{
		return (int)f;
	}

	public static float ToFloat(this int i)
	{
		return i;
	}

	private static int Pow(int f, int p)
	{
		for (int i = 1; i < p; i++)
		{
			f *= f;
		}
		return f;
	}

	private static int ToIndex(this float f)
	{
		return Mathf.Abs((f * decimalMultiplier).ToInt());
	}

	private static float FromIndex(this int i)
	{
		return i.ToFloat() / decimalMultiplier;
	}
}
