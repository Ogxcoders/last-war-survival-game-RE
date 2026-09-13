using System;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common;

[Serializable]
public struct FVector2 : IEquatable<FVector2>
{
	private static FVector2 _zeroVector = new FVector2(0, 0);

	private static FVector2 _oneVector = new FVector2(1, 1);

	private static FVector2 _rightVector = new FVector2(1, 0);

	private static FVector2 _leftVector = new FVector2(-1, 0);

	private static FVector2 _upVector = new FVector2(0, 1);

	private static FVector2 _downVector = new FVector2(0, -1);

	public FP X;

	public FP Y;

	public static FVector2 Zero => _zeroVector;

	public static FVector2 One => _oneVector;

	public static FVector2 Right => _rightVector;

	public static FVector2 Left => _leftVector;

	public static FVector2 Up => _upVector;

	public static FVector2 Down => _downVector;

	public FP this[int index]
	{
		get
		{
			return index switch
			{
				0 => X, 
				1 => Y, 
				_ => throw new ArgumentOutOfRangeException("index", "The index is out of range. Allowed values are 0 or 1."), 
			};
		}
		set
		{
			switch (index)
			{
			case 0:
				X = value;
				break;
			case 1:
				Y = value;
				break;
			default:
				throw new ArgumentOutOfRangeException("index", "The index is out of range. Allowed values are 0 or 1");
			}
		}
	}

	public FP magnitude
	{
		get
		{
			DistanceSquared(ref this, ref _zeroVector, out var result);
			return FP.Sqrt(result);
		}
	}

	public FVector2 normalized
	{
		get
		{
			Normalize(ref this, out var result);
			return result;
		}
	}

	public FVector2(FP x, FP y)
	{
		X = x;
		Y = y;
	}

	public FVector2(FP value)
	{
		X = value;
		Y = value;
	}

	public void Set(FP x, FP y)
	{
		X = x;
		Y = y;
	}

	public static void Reflect(ref FVector2 vector, ref FVector2 normal, out FVector2 result)
	{
		FP y = Dot(vector, normal);
		result.X = vector.X - (FP)2f * y * normal.X;
		result.Y = vector.Y - (FP)2f * y * normal.Y;
	}

	public static FVector2 Reflect(FVector2 vector, FVector2 normal)
	{
		Reflect(ref vector, ref normal, out var result);
		return result;
	}

	public static FVector2 Add(FVector2 value1, FVector2 value2)
	{
		ref FP x = ref value1.X;
		x += value2.X;
		ref FP y = ref value1.Y;
		y += value2.Y;
		return value1;
	}

	public static void Add(ref FVector2 value1, ref FVector2 value2, out FVector2 result)
	{
		result.X = value1.X + value2.X;
		result.Y = value1.Y + value2.Y;
	}

	public static FVector2 Clamp(FVector2 value1, FVector2 min, FVector2 max)
	{
		return new FVector2(FMath.Clamp(value1.X, min.X, max.X), FMath.Clamp(value1.Y, min.Y, max.Y));
	}

	public static void Clamp(ref FVector2 value1, ref FVector2 min, ref FVector2 max, out FVector2 result)
	{
		result = new FVector2(FMath.Clamp(value1.X, min.X, max.X), FMath.Clamp(value1.Y, min.Y, max.Y));
	}

	public static FP Distance(FVector2 value1, FVector2 value2)
	{
		DistanceSquared(ref value1, ref value2, out var result);
		return FP.Sqrt(result);
	}

	public static void Distance(ref FVector2 value1, ref FVector2 value2, out FP result)
	{
		DistanceSquared(ref value1, ref value2, out result);
		result = FP.Sqrt(result);
	}

	public static FP DistanceSquared(FVector2 value1, FVector2 value2)
	{
		DistanceSquared(ref value1, ref value2, out var result);
		return result;
	}

	public static void DistanceSquared(ref FVector2 value1, ref FVector2 value2, out FP result)
	{
		result = (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y);
	}

	public static FVector2 Divide(FVector2 value1, FVector2 value2)
	{
		value1.X /= value2.X;
		value1.Y /= value2.Y;
		return value1;
	}

	public static void Divide(ref FVector2 value1, ref FVector2 value2, out FVector2 result)
	{
		result.X = value1.X / value2.X;
		result.Y = value1.Y / value2.Y;
	}

	public static FVector2 Divide(FVector2 value1, FP divider)
	{
		FP y = 1 / divider;
		ref FP x = ref value1.X;
		x *= y;
		ref FP y2 = ref value1.Y;
		y2 *= y;
		return value1;
	}

	public static void Divide(ref FVector2 value1, FP divider, out FVector2 result)
	{
		FP y = 1 / divider;
		result.X = value1.X * y;
		result.Y = value1.Y * y;
	}

	public static FVector2 Abs(FVector2 value)
	{
		return new FVector2(FP.Abs(value.X), FP.Abs(value.Y));
	}

	public static FP Dot(FVector2 value1, FVector2 value2)
	{
		return value1.X * value2.X + value1.Y * value2.Y;
	}

	public static void Dot(ref FVector2 value1, ref FVector2 value2, out FP result)
	{
		result = value1.X * value2.X + value1.Y * value2.Y;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is FVector2))
		{
			return false;
		}
		return this == (FVector2)obj;
	}

	public bool Equals(FVector2 other)
	{
		return this == other;
	}

	public override int GetHashCode()
	{
		return (int)(long)(X + Y);
	}

	public static FVector2 Hermite(FVector2 value1, FVector2 tangent1, FVector2 value2, FVector2 tangent2, FP amount)
	{
		FVector2 result = default(FVector2);
		Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
		return result;
	}

	public static void Hermite(ref FVector2 value1, ref FVector2 tangent1, ref FVector2 value2, ref FVector2 tangent2, FP amount, out FVector2 result)
	{
		result.X = FMath.Hermite(value1.X, tangent1.X, value2.X, tangent2.X, amount);
		result.Y = FMath.Hermite(value1.Y, tangent1.Y, value2.Y, tangent2.Y, amount);
	}

	public static FVector2 ClampMagnitude(FVector2 vector, FP maxLength)
	{
		if (vector.LengthSquared() > maxLength * maxLength)
		{
			return vector.normalized * maxLength;
		}
		return vector;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP Length()
	{
		return FP.Sqrt(Dot(this, this));
	}

	public FP LengthSquared()
	{
		DistanceSquared(ref this, ref _zeroVector, out var result);
		return result;
	}

	public static FVector2 Lerp(FVector2 value1, FVector2 value2, FP amount)
	{
		amount = FMath.Clamp(amount, 0, 1);
		return new FVector2(FMath.Lerp(value1.X, value2.X, amount), FMath.Lerp(value1.Y, value2.Y, amount));
	}

	public static FVector2 LerpUnclamped(FVector2 value1, FVector2 value2, FP amount)
	{
		return new FVector2(FMath.Lerp(value1.X, value2.X, amount), FMath.Lerp(value1.Y, value2.Y, amount));
	}

	public static void LerpUnclamped(ref FVector2 value1, ref FVector2 value2, FP amount, out FVector2 result)
	{
		result = new FVector2(FMath.Lerp(value1.X, value2.X, amount), FMath.Lerp(value1.Y, value2.Y, amount));
	}

	public static FVector2 Max(FVector2 value1, FVector2 value2)
	{
		return new FVector2(FMath.Max(value1.X, value2.X), FMath.Max(value1.Y, value2.Y));
	}

	public static void Max(ref FVector2 value1, ref FVector2 value2, out FVector2 result)
	{
		result.X = FMath.Max(value1.X, value2.X);
		result.Y = FMath.Max(value1.Y, value2.Y);
	}

	public static FVector2 Min(FVector2 value1, FVector2 value2)
	{
		return new FVector2(FMath.Min(value1.X, value2.X), FMath.Min(value1.Y, value2.Y));
	}

	public static void Min(ref FVector2 value1, ref FVector2 value2, out FVector2 result)
	{
		result.X = FMath.Min(value1.X, value2.X);
		result.Y = FMath.Min(value1.Y, value2.Y);
	}

	public void Scale(FVector2 other)
	{
		X *= other.X;
		Y *= other.Y;
	}

	public static FVector2 Scale(FVector2 value1, FVector2 value2)
	{
		FVector2 result = default(FVector2);
		result.X = value1.X * value2.X;
		result.Y = value1.Y * value2.Y;
		return result;
	}

	public static FVector2 Multiply(FVector2 value1, FVector2 value2)
	{
		ref FP x = ref value1.X;
		x *= value2.X;
		ref FP y = ref value1.Y;
		y *= value2.Y;
		return value1;
	}

	public static FVector2 Multiply(FVector2 value1, FP scaleFactor)
	{
		ref FP x = ref value1.X;
		x *= scaleFactor;
		ref FP y = ref value1.Y;
		y *= scaleFactor;
		return value1;
	}

	public static void Multiply(ref FVector2 value1, FP scaleFactor, out FVector2 result)
	{
		result.X = value1.X * scaleFactor;
		result.Y = value1.Y * scaleFactor;
	}

	public static void Multiply(ref FVector2 value1, ref FVector2 value2, out FVector2 result)
	{
		result.X = value1.X * value2.X;
		result.Y = value1.Y * value2.Y;
	}

	public static FVector2 Negate(FVector2 value)
	{
		value.X = -value.X;
		value.Y = -value.Y;
		return value;
	}

	public static void Negate(ref FVector2 value, out FVector2 result)
	{
		result.X = -value.X;
		result.Y = -value.Y;
	}

	public static FVector2 Normalize(FVector2 value)
	{
		Normalize(ref value, out value);
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP Normalize()
	{
		return Normalize(ref this, out this);
	}

	public static FP Normalize(ref FVector2 value, out FVector2 result)
	{
		DistanceSquared(ref value, ref _zeroVector, out var result2);
		FP fP = FP.Sqrt(result2);
		if (fP < Settings.Epsilon)
		{
			result = value;
			return FP.Zero;
		}
		result2 = 1f / fP;
		result.X = value.X * result2;
		result.Y = value.Y * result2;
		return fP;
	}

	public static FP Normalize(ref FVector2 value, out FVector2 result, out FP length)
	{
		DistanceSquared(ref value, ref _zeroVector, out var result2);
		length = FP.Sqrt(result2);
		if (length < Settings.Epsilon)
		{
			result = value;
			return FP.Zero;
		}
		result2 = 1f / length;
		result.X = value.X * result2;
		result.Y = value.Y * result2;
		return length;
	}

	public static FVector2 SmoothStep(FVector2 value1, FVector2 value2, FP amount)
	{
		return new FVector2(FMath.SmoothStep(value1.X, value2.X, amount), FMath.SmoothStep(value1.Y, value2.Y, amount));
	}

	public static void SmoothStep(ref FVector2 value1, ref FVector2 value2, FP amount, out FVector2 result)
	{
		result = new FVector2(FMath.SmoothStep(value1.X, value2.X, amount), FMath.SmoothStep(value1.Y, value2.Y, amount));
	}

	public static FVector2 Subtract(FVector2 value1, FVector2 value2)
	{
		ref FP x = ref value1.X;
		x -= value2.X;
		ref FP y = ref value1.Y;
		y -= value2.Y;
		return value1;
	}

	public static void Subtract(ref FVector2 value1, ref FVector2 value2, out FVector2 result)
	{
		result.X = value1.X - value2.X;
		result.Y = value1.Y - value2.Y;
	}

	public static FP Angle(FVector2 a, FVector2 b)
	{
		return FP.Acos(FMath.Clamp(a.normalized * b.normalized, -FP.One, FP.One)) * FP.Rad2Deg;
	}

	public FVector3 ToTSVector()
	{
		return new FVector3(X, Y, 0);
	}

	public override string ToString()
	{
		return $"({X.AsFloat}, {Y.AsFloat}";
	}

	public static FVector2 operator -(FVector2 value)
	{
		value.X = -value.X;
		value.Y = -value.Y;
		return value;
	}

	public static bool operator ==(FVector2 value1, FVector2 value2)
	{
		if (value1.X == value2.X)
		{
			return value1.Y == value2.Y;
		}
		return false;
	}

	public static bool operator !=(FVector2 value1, FVector2 value2)
	{
		if (!(value1.X != value2.X))
		{
			return value1.Y != value2.Y;
		}
		return true;
	}

	public static FVector2 operator +(FVector2 value1, FVector2 value2)
	{
		ref FP x = ref value1.X;
		x += value2.X;
		ref FP y = ref value1.Y;
		y += value2.Y;
		return value1;
	}

	public static FVector2 operator -(FVector2 value1, FVector2 value2)
	{
		ref FP x = ref value1.X;
		x -= value2.X;
		ref FP y = ref value1.Y;
		y -= value2.Y;
		return value1;
	}

	public static FP operator *(FVector2 value1, FVector2 value2)
	{
		return Dot(value1, value2);
	}

	public static FVector2 operator *(FVector2 value, FP scaleFactor)
	{
		ref FP x = ref value.X;
		x *= scaleFactor;
		ref FP y = ref value.Y;
		y *= scaleFactor;
		return value;
	}

	public static FVector2 operator *(FP scaleFactor, FVector2 value)
	{
		ref FP x = ref value.X;
		x *= scaleFactor;
		ref FP y = ref value.Y;
		y *= scaleFactor;
		return value;
	}

	public static FVector2 operator /(FVector2 value1, FVector2 value2)
	{
		value1.X /= value2.X;
		value1.Y /= value2.Y;
		return value1;
	}

	public static FVector2 operator /(FVector2 value1, FP divider)
	{
		FP y = 1 / divider;
		ref FP x = ref value1.X;
		x *= y;
		ref FP y2 = ref value1.Y;
		y2 *= y;
		return value1;
	}
}
