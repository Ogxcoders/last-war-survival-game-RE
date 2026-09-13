using System;

namespace UnityEngine.Timeline;

internal readonly struct FrameRate : IEquatable<FrameRate>
{
	public readonly double rate;

	public static readonly FrameRate k_23_976Fps = new FrameRate(23.976023976024);

	public static readonly FrameRate k_24Fps = new FrameRate(24.0);

	public static readonly FrameRate k_25Fps = new FrameRate(25.0);

	public static readonly FrameRate k_30Fps = new FrameRate(30.0);

	public static readonly FrameRate k_29_97Fps = new FrameRate(29.97002997003);

	public static readonly FrameRate k_50Fps = new FrameRate(50.0);

	public static readonly FrameRate k_59_94Fps = new FrameRate(59.9400599400599);

	public static readonly FrameRate k_60Fps = new FrameRate(60.0);

	private FrameRate(double framerate)
	{
		rate = framerate;
	}

	public bool IsValid()
	{
		return rate > TimeUtility.kTimeEpsilon;
	}

	public bool Equals(FrameRate other)
	{
		return Math.Abs(rate - other.rate) < TimeUtility.kFrameRateEpsilon;
	}

	public override bool Equals(object obj)
	{
		if (obj is FrameRate other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		double num = rate;
		return num.GetHashCode();
	}

	public static bool operator ==(FrameRate a, FrameRate b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(FrameRate a, FrameRate b)
	{
		return !a.Equals(b);
	}

	public static FrameRate DoubleToFrameRate(double rate)
	{
		return new FrameRate((Math.Ceiling(rate) - rate < TimeUtility.kFrameRateEpsilon) ? rate : (Math.Ceiling(rate) * 1000.0 / 1001.0));
	}
}
