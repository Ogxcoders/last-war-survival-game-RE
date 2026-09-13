using UnityEngine;

namespace Tayx.Graphy.Utils.NumString;

public static class G_IntString
{
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

	public static int MinValue => -(negativeBuffer.Length - 1);

	public static int MaxValue => positiveBuffer.Length - 1;

	public static void Init(int minNegativeValue, int maxPositiveValue)
	{
		if (minNegativeValue <= 0)
		{
			int num = Mathf.Abs(minNegativeValue);
			negativeBuffer = new string[num];
			for (int i = 0; i < num; i++)
			{
				negativeBuffer[i] = (-i).ToString();
			}
		}
		if (maxPositiveValue >= 0)
		{
			positiveBuffer = new string[maxPositiveValue];
			for (int j = 0; j < maxPositiveValue; j++)
			{
				positiveBuffer[j] = j.ToString();
			}
		}
	}

	public static string ToStringNonAlloc(this int value)
	{
		if (value < 0 && -value < negativeBuffer.Length)
		{
			return negativeBuffer[-value];
		}
		if (value >= 0 && value < positiveBuffer.Length)
		{
			return positiveBuffer[value];
		}
		return value.ToString();
	}
}
