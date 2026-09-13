using System;
using UnityEngine;

namespace MoreMountains.NiceVibrations;

public class MMSignal : MonoBehaviour
{
	public enum SignalType
	{
		DigitalNoise,
		Pulse,
		Sawtooth,
		Sine,
		Square,
		Triangle,
		WhiteNoise
	}

	public static float GetValue(float time, SignalType signalType, float phase, float amplitude, float frequency, float offset, bool Invert = false)
	{
		float num = 0f;
		float num2 = ((!Invert) ? 1 : (-1));
		float num3 = frequency * time + phase;
		switch (signalType)
		{
		case SignalType.Sine:
			num = Mathf.Sin(MathF.PI * 2f * num3);
			break;
		case SignalType.Square:
			num = Mathf.Sign(Mathf.Sin(MathF.PI * 2f * num3));
			break;
		case SignalType.Triangle:
			num = 1f - 4f * Mathf.Abs(Mathf.Round(num3 - 0.25f) - (num3 - 0.25f));
			break;
		case SignalType.Sawtooth:
			num = 2f * (num3 - Mathf.Floor(num3 + 0.5f));
			break;
		case SignalType.Pulse:
			num = ((!((double)Mathf.Abs(Mathf.Sin(MathF.PI * 2f * num3)) < 0.99)) ? 1 : 0);
			break;
		case SignalType.WhiteNoise:
			num = 2f * (float)UnityEngine.Random.Range(0, int.MaxValue) / 2.1474836E+09f - 1f;
			break;
		case SignalType.DigitalNoise:
			num = UnityEngine.Random.Range(0, 2);
			break;
		}
		return num2 * amplitude * num + offset;
	}
}
