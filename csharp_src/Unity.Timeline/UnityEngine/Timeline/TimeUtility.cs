using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace UnityEngine.Timeline;

internal static class TimeUtility
{
	public static readonly double kTimeEpsilon = 1E-14;

	public static readonly double kFrameRateEpsilon = 1E-06;

	public static readonly double k_MaxTimelineDurationInSeconds = 9000000.0;

	public static readonly double kFrameRateRounding = 0.01;

	private static void ValidateFrameRate(double frameRate)
	{
		if (frameRate <= kTimeEpsilon)
		{
			throw new ArgumentException("frame rate cannot be 0 or negative");
		}
	}

	public static int ToFrames(double time, double frameRate)
	{
		ValidateFrameRate(frameRate);
		time = Math.Min(Math.Max(time, 0.0 - k_MaxTimelineDurationInSeconds), k_MaxTimelineDurationInSeconds);
		double epsilon = GetEpsilon(time, frameRate);
		if (time < 0.0)
		{
			return (int)Math.Ceiling(time * frameRate - epsilon);
		}
		return (int)Math.Floor(time * frameRate + epsilon);
	}

	public static double ToExactFrames(double time, double frameRate)
	{
		ValidateFrameRate(frameRate);
		return time * frameRate;
	}

	public static double FromFrames(int frames, double frameRate)
	{
		ValidateFrameRate(frameRate);
		return (double)frames / frameRate;
	}

	public static double FromFrames(double frames, double frameRate)
	{
		ValidateFrameRate(frameRate);
		return frames / frameRate;
	}

	public static bool OnFrameBoundary(double time, double frameRate)
	{
		return OnFrameBoundary(time, frameRate, GetEpsilon(time, frameRate));
	}

	public static double GetEpsilon(double time, double frameRate)
	{
		return Math.Max(Math.Abs(time), 1.0) * frameRate * kTimeEpsilon;
	}

	public static int PreviousFrame(double time, double frameRate)
	{
		return Math.Max(0, ToFrames(time, frameRate) - 1);
	}

	public static int NextFrame(double time, double frameRate)
	{
		return ToFrames(time, frameRate) + 1;
	}

	public static double PreviousFrameTime(double time, double frameRate)
	{
		return FromFrames(PreviousFrame(time, frameRate), frameRate);
	}

	public static double NextFrameTime(double time, double frameRate)
	{
		return FromFrames(NextFrame(time, frameRate), frameRate);
	}

	public static bool OnFrameBoundary(double time, double frameRate, double epsilon)
	{
		ValidateFrameRate(frameRate);
		double num = ToExactFrames(time, frameRate);
		double num2 = Math.Round(num);
		return Math.Abs(num - num2) < epsilon;
	}

	public static double RoundToFrame(double time, double frameRate)
	{
		ValidateFrameRate(frameRate);
		double num = (double)(int)Math.Floor(time * frameRate) / frameRate;
		double num2 = (double)(int)Math.Ceiling(time * frameRate) / frameRate;
		if (!(Math.Abs(time - num) < Math.Abs(time - num2)))
		{
			return num2;
		}
		return num;
	}

	public static string TimeAsFrames(double timeValue, double frameRate, string format = "F2")
	{
		if (OnFrameBoundary(timeValue, frameRate))
		{
			return ToFrames(timeValue, frameRate).ToString();
		}
		return ToExactFrames(timeValue, frameRate).ToString(format);
	}

	public static string TimeAsTimeCode(double timeValue, double frameRate, string format = "F2")
	{
		ValidateFrameRate(frameRate);
		int num = (int)Math.Abs(timeValue);
		int num2 = num / 3600;
		int num3 = num % 3600 / 60;
		int num4 = num % 60;
		string text = ((timeValue < 0.0) ? "-" : string.Empty);
		string text2 = ((num2 > 0) ? (num2 + ":" + num3.ToString("D2") + ":" + num4.ToString("D2")) : ((num3 <= 0) ? num4.ToString() : (num3 + ":" + num4.ToString("D2"))));
		int totalWidth = (int)Math.Floor(Math.Log10(frameRate) + 1.0);
		string text3 = (ToFrames(timeValue, frameRate) - ToFrames(num, frameRate)).ToString().PadLeft(totalWidth, '0');
		if (!OnFrameBoundary(timeValue, frameRate))
		{
			string text4 = ToExactFrames(timeValue, frameRate).ToString(format);
			int num5 = text4.IndexOf('.');
			if (num5 >= 0)
			{
				text3 = text3 + " [" + text4.Substring(num5) + "]";
			}
		}
		return text + text2 + ":" + text3;
	}

	public static double ParseTimeCode(string timeCode, double frameRate, double defaultValue)
	{
		timeCode = RemoveChar(timeCode, (char c) => char.IsWhiteSpace(c));
		string[] array = timeCode.Split(new char[1] { ':' });
		if (array.Length == 0 || array.Length > 4)
		{
			return defaultValue;
		}
		int num = 0;
		int num2 = 0;
		double num3 = 0.0;
		double num4 = 0.0;
		try
		{
			string text = array[^1];
			if (Regex.Match(text, "^\\d+\\.\\d+$").Success)
			{
				num3 = double.Parse(text);
				if (array.Length > 3)
				{
					return defaultValue;
				}
				if (array.Length > 1)
				{
					num2 = int.Parse(array[^2]);
				}
				if (array.Length > 2)
				{
					num = int.Parse(array[^3]);
				}
			}
			else
			{
				if (Regex.Match(text, "^\\d+\\[\\.\\d+\\]$").Success)
				{
					num4 = double.Parse(RemoveChar(text, (char c) => c == '[' || c == ']'));
				}
				else
				{
					if (!Regex.Match(text, "^\\d*$").Success)
					{
						return defaultValue;
					}
					num4 = int.Parse(text);
				}
				if (array.Length > 1)
				{
					num3 = int.Parse(array[^2]);
				}
				if (array.Length > 2)
				{
					num2 = int.Parse(array[^3]);
				}
				if (array.Length > 3)
				{
					num = int.Parse(array[^4]);
				}
			}
		}
		catch (FormatException)
		{
			return defaultValue;
		}
		return num4 / frameRate + num3 + (double)(num2 * 60) + (double)(num * 3600);
	}

	public static double ParseTimeSeconds(string timeCode, double frameRate, double defaultValue)
	{
		timeCode = RemoveChar(timeCode, (char c) => char.IsWhiteSpace(c));
		string[] array = timeCode.Split(new char[1] { ':' });
		if (array.Length == 0 || array.Length > 4)
		{
			return defaultValue;
		}
		int num = 0;
		int num2 = 0;
		double result = 0.0;
		try
		{
			string text = array[^1];
			if (!double.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
			{
				if (!Regex.Match(text, "^\\d+\\.\\d+$").Success)
				{
					return defaultValue;
				}
				result = double.Parse(text);
			}
			if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
			{
				return defaultValue;
			}
			if (array.Length > 3)
			{
				return defaultValue;
			}
			if (array.Length > 1)
			{
				num2 = int.Parse(array[^2]);
			}
			if (array.Length > 2)
			{
				num = int.Parse(array[^3]);
			}
		}
		catch (FormatException)
		{
			return defaultValue;
		}
		return result + (double)(num2 * 60) + (double)(num * 3600);
	}

	public static double GetAnimationClipLength(AnimationClip clip)
	{
		if (clip == null || clip.empty)
		{
			return 0.0;
		}
		double result = clip.length;
		if (clip.frameRate > 0f)
		{
			result = (double)Mathf.Round(clip.length * clip.frameRate) / (double)clip.frameRate;
		}
		return result;
	}

	private static string RemoveChar(string str, Func<char, bool> charToRemoveFunc)
	{
		int length = str.Length;
		char[] array = str.ToCharArray();
		int length2 = 0;
		for (int i = 0; i < length; i++)
		{
			if (!charToRemoveFunc(array[i]))
			{
				array[length2++] = array[i];
			}
		}
		return new string(array, 0, length2);
	}

	public static FrameRate GetClosestFrameRate(double frameRate)
	{
		ValidateFrameRate(frameRate);
		FrameRate result = FrameRate.DoubleToFrameRate(frameRate);
		if (!(Math.Abs(frameRate - result.rate) < kFrameRateRounding))
		{
			return default(FrameRate);
		}
		return result;
	}

	public static FrameRate ToFrameRate(StandardFrameRates enumValue)
	{
		return enumValue switch
		{
			StandardFrameRates.Fps23_97 => FrameRate.k_23_976Fps, 
			StandardFrameRates.Fps24 => FrameRate.k_24Fps, 
			StandardFrameRates.Fps25 => FrameRate.k_25Fps, 
			StandardFrameRates.Fps29_97 => FrameRate.k_29_97Fps, 
			StandardFrameRates.Fps30 => FrameRate.k_30Fps, 
			StandardFrameRates.Fps50 => FrameRate.k_50Fps, 
			StandardFrameRates.Fps59_94 => FrameRate.k_59_94Fps, 
			StandardFrameRates.Fps60 => FrameRate.k_60Fps, 
			_ => default(FrameRate), 
		};
	}

	internal static bool ToStandardFrameRate(FrameRate rate, out StandardFrameRates standard)
	{
		if (rate == FrameRate.k_23_976Fps)
		{
			standard = StandardFrameRates.Fps23_97;
		}
		else if (rate == FrameRate.k_24Fps)
		{
			standard = StandardFrameRates.Fps24;
		}
		else if (rate == FrameRate.k_25Fps)
		{
			standard = StandardFrameRates.Fps25;
		}
		else if (rate == FrameRate.k_30Fps)
		{
			standard = StandardFrameRates.Fps30;
		}
		else if (rate == FrameRate.k_29_97Fps)
		{
			standard = StandardFrameRates.Fps29_97;
		}
		else if (rate == FrameRate.k_50Fps)
		{
			standard = StandardFrameRates.Fps50;
		}
		else if (rate == FrameRate.k_59_94Fps)
		{
			standard = StandardFrameRates.Fps59_94;
		}
		else
		{
			if (!(rate == FrameRate.k_60Fps))
			{
				standard = (StandardFrameRates)Enum.GetValues(typeof(StandardFrameRates)).Length;
				return false;
			}
			standard = StandardFrameRates.Fps60;
		}
		return true;
	}
}
