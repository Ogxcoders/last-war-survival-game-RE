using System;

namespace Box2DSharp.Common;

public sealed class FMath
{
	public static FP Pi = FP.Pi;

	public static FP PiOver2 = FP.PiOver2;

	public static FP Epsilon = FP.Epsilon;

	public static FP Deg2Rad = FP.Deg2Rad;

	public static FP Rad2Deg = FP.Rad2Deg;

	public static FP Infinity = FP.MaxValue;

	public static FP Sqrt(FP number)
	{
		return FP.Sqrt(number);
	}

	public static FP Max(FP val1, FP val2)
	{
		if (!(val1 > val2))
		{
			return val2;
		}
		return val1;
	}

	public static FP Min(FP val1, FP val2)
	{
		if (!(val1 < val2))
		{
			return val2;
		}
		return val1;
	}

	public static FP Max(FP val1, FP val2, FP val3)
	{
		FP fP = ((val1 > val2) ? val1 : val2);
		if (!(fP > val3))
		{
			return val3;
		}
		return fP;
	}

	public static FP Clamp(FP value, FP min, FP max)
	{
		if (!(value < min))
		{
			if (!(value > max))
			{
				return value;
			}
			return max;
		}
		return min;
	}

	public static FP Clamp01(FP value)
	{
		if (value < FP.Zero)
		{
			return FP.Zero;
		}
		if (value > FP.One)
		{
			return FP.One;
		}
		return value;
	}

	public static FP Sin(FP value)
	{
		return FP.Sin(value);
	}

	public static FP Cos(FP value)
	{
		return FP.Cos(value);
	}

	public static FP Tan(FP value)
	{
		return FP.Tan(value);
	}

	public static FP Asin(FP value)
	{
		return FP.Asin(value);
	}

	public static FP Acos(FP value)
	{
		return FP.Acos(value);
	}

	public static FP Atan(FP value)
	{
		return FP.Atan(value);
	}

	public static FP Atan2(FP y, FP x)
	{
		return FP.Atan2(y, x);
	}

	public static FP Floor(FP value)
	{
		return FP.Floor(value);
	}

	public static FP Ceiling(FP value)
	{
		return value;
	}

	public static FP Round(FP value)
	{
		return FP.Round(value);
	}

	public static int Sign(FP value)
	{
		return FP.Sign(value);
	}

	public static FP Abs(FP value)
	{
		return FP.Abs(value);
	}

	public static FP Barycentric(FP value1, FP value2, FP value3, FP amount1, FP amount2)
	{
		return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
	}

	public static FP CatmullRom(FP value1, FP value2, FP value3, FP value4, FP amount)
	{
		FP x = amount * amount;
		FP y = x * amount;
		return (FP)0.5 * ((FP)2.0 * value2 + (value3 - value1) * amount + ((FP)2.0 * value1 - (FP)5.0 * value2 + (FP)4.0 * value3 - value4) * x + ((FP)3.0 * value2 - value1 - (FP)3.0 * value3 + value4) * y);
	}

	public static FP Distance(FP value1, FP value2)
	{
		return FP.Abs(value1 - value2);
	}

	public static FP Hermite(FP value1, FP tangent1, FP value2, FP tangent2, FP amount)
	{
		FP y = value1;
		FP y2 = value2;
		FP y3 = tangent1;
		FP y4 = tangent2;
		FP x = amount;
		FP y5 = x * x * x;
		FP y6 = x * x;
		if (amount == 0f)
		{
			return value1;
		}
		if (amount == 1f)
		{
			return value2;
		}
		return ((FP)2 * y - (FP)2 * y2 + y4 + y3) * y5 + ((FP)3 * y2 - (FP)3 * y - (FP)2 * y3 - y4) * y6 + y3 * x + y;
	}

	public static FP Lerp(FP value1, FP value2, FP amount)
	{
		return value1 + (value2 - value1) * Clamp01(amount);
	}

	public static FP InverseLerp(FP value1, FP value2, FP amount)
	{
		if (value1 != value2)
		{
			return Clamp01((amount - value1) / (value2 - value1));
		}
		return FP.Zero;
	}

	public static FP SmoothStep(FP value1, FP value2, FP amount)
	{
		FP amount2 = Clamp(amount, 0f, 1f);
		return Hermite(value1, 0f, value2, 0f, amount2);
	}

	internal static FP Pow2(FP x)
	{
		if (x.RawValue == 0L)
		{
			return FP.One;
		}
		bool flag = x.RawValue < 0;
		if (flag)
		{
			x = -x;
		}
		if (x == FP.One)
		{
			if (!flag)
			{
				return 2;
			}
			return FP.One / 2;
		}
		if (x >= FP.Log2Max)
		{
			if (!flag)
			{
				return FP.MaxValue;
			}
			return FP.One / FP.MaxValue;
		}
		if (x <= FP.Log2Min)
		{
			if (!flag)
			{
				return FP.Zero;
			}
			return FP.MaxValue;
		}
		int num = (int)(long)Floor(x);
		x = FP.FromRaw(x.RawValue & 0xFFFFFFFFu);
		FP x2 = FP.One;
		FP y = FP.One;
		int num2 = 1;
		while (y.RawValue != 0L)
		{
			y = FP.FastMul(FP.FastMul(in x, in y), in FP.Ln2) / num2;
			x2 += y;
			num2++;
		}
		x2 = FP.FromRaw(x2.RawValue << num);
		if (flag)
		{
			x2 = FP.One / x2;
		}
		return x2;
	}

	internal static FP Log2(FP x)
	{
		if (x.RawValue <= 0)
		{
			throw new ArgumentOutOfRangeException("Non-positive value passed to Ln", "x");
		}
		long num = 2147483648L;
		long num2 = 0L;
		long num3 = x.RawValue;
		while (num3 < 4294967296L)
		{
			num3 <<= 1;
			num2 -= 4294967296L;
		}
		while (num3 >= 8589934592L)
		{
			num3 >>= 1;
			num2 += 4294967296L;
		}
		FP x2 = FP.FromRaw(num3);
		for (int i = 0; i < 32; i++)
		{
			x2 = FP.FastMul(in x2, in x2);
			if (x2.RawValue >= 8589934592L)
			{
				x2 = FP.FromRaw(x2.RawValue >> 1);
				num2 += num;
			}
			num >>= 1;
		}
		return FP.FromRaw(num2);
	}

	public static FP Ln(FP x)
	{
		return FP.FastMul(Log2(x), in FP.Ln2);
	}

	public static FP Pow(FP b, FP exp)
	{
		if (b == FP.One)
		{
			return FP.One;
		}
		if (exp.RawValue == 0L)
		{
			return FP.One;
		}
		if (b.RawValue == 0L)
		{
			if (exp.RawValue < 0)
			{
				return FP.MaxValue;
			}
			return FP.Zero;
		}
		return Pow2(exp * Log2(b));
	}

	public static FP MoveTowards(FP current, FP target, FP maxDelta)
	{
		if (Abs(target - current) <= maxDelta)
		{
			return target;
		}
		return current + (FP)Sign(target - current) * maxDelta;
	}

	public static FP Repeat(FP t, FP length)
	{
		return t - Floor(t / length) * length;
	}

	public static FP DeltaAngle(FP current, FP target)
	{
		FP x = Repeat(target - current, 360f);
		if (x > 180f)
		{
			x -= (FP)360f;
		}
		return x;
	}

	public static FP MoveTowardsAngle(FP current, FP target, FP maxDelta)
	{
		target = current + DeltaAngle(current, target);
		return MoveTowards(current, target, maxDelta);
	}

	public static FP SmoothDamp(FP current, FP target, ref FP currentVelocity, FP smoothTime, FP maxSpeed)
	{
		FP eN = FP.EN2;
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, eN);
	}

	public static FP SmoothDamp(FP current, FP target, ref FP currentVelocity, FP smoothTime)
	{
		FP eN = FP.EN2;
		FP maxSpeed = -FP.MaxValue;
		return SmoothDamp(current, target, ref currentVelocity, smoothTime, maxSpeed, eN);
	}

	public static FP SmoothDamp(FP current, FP target, ref FP currentVelocity, FP smoothTime, FP maxSpeed, FP deltaTime)
	{
		smoothTime = Max(FP.EN4, smoothTime);
		FP x = 2f / smoothTime;
		FP y = x * deltaTime;
		FP y2 = FP.One / (FP.One + y + (FP)0.48f * y * y + (FP)0.235f * y * y * y);
		FP value = current - target;
		FP x2 = target;
		FP fP = maxSpeed * smoothTime;
		value = Clamp(value, -fP, fP);
		target = current - value;
		FP y3 = (currentVelocity + x * value) * deltaTime;
		currentVelocity = (currentVelocity - x * y3) * y2;
		FP x3 = target + (value + y3) * y2;
		if (x2 - current > FP.Zero == x3 > x2)
		{
			x3 = x2;
			currentVelocity = (x3 - x2) / deltaTime;
		}
		return x3;
	}

	public static FP Exp(FP num)
	{
		if (num == FP.Zero)
		{
			return FP.One;
		}
		if (num == FP.One)
		{
			return FP.e;
		}
		if (num.RawValue >= 137438953472L)
		{
			return FP.MaxValue;
		}
		if (num.RawValue <= -51539607552L)
		{
			return FP.MinValue;
		}
		bool flag = num.m_rawValue < 0;
		if (flag)
		{
			num = -num;
		}
		FP x = num + FP.One;
		FP x2 = num;
		for (int i = 2; i < 30; i++)
		{
			x2 *= num / i;
			x += x2;
			if (x2.m_rawValue < 500 && (i > 15 || x2.m_rawValue < 20))
			{
				break;
			}
		}
		if (flag)
		{
			x = FP.One / x;
		}
		return x;
	}
}
