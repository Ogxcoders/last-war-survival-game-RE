using UnityEngine;

namespace MoreMountains.NiceVibrations;

public static class NiceVibrationsDemoHelpers
{
	public static float Round(float value, int digits)
	{
		float num = Mathf.Pow(10f, digits);
		return Mathf.Round(value * num) / num;
	}

	public static float Remap(float x, float A, float B, float C, float D)
	{
		return C + (x - A) / (B - A) * (D - C);
	}
}
