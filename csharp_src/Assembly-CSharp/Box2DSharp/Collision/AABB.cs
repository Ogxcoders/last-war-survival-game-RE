using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct AABB
{
	public FVector2 LowerBound;

	public FVector2 UpperBound;

	public AABB(in FVector2 lowerBound, in FVector2 upperBound)
	{
		LowerBound = lowerBound;
		UpperBound = upperBound;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsValid()
	{
		FVector2 fVector = UpperBound - LowerBound;
		if (fVector.X >= 0f)
		{
			return fVector.Y >= 0f;
		}
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector2 GetCenter()
	{
		return 0.5f * (LowerBound + UpperBound);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FVector2 GetExtents()
	{
		return 0.5f * (UpperBound - LowerBound);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FP GetPerimeter()
	{
		FP x = UpperBound.X - LowerBound.X;
		FP y = UpperBound.Y - LowerBound.Y;
		return x + x + y + y;
	}

	public bool RayCast(out RayCastOutput output, in RayCastInput input)
	{
		output = default(RayCastOutput);
		FP fP = -Settings.MaxFloat;
		FP fP2 = Settings.MaxFloat;
		FVector2 p = input.P1;
		FVector2 value = input.P2 - input.P1;
		FVector2 fVector = FVector2.Abs(value);
		FVector2 vector = default(FVector2);
		if (fVector.X < Settings.Epsilon)
		{
			if (p.X < LowerBound.X || UpperBound.X < p.X)
			{
				return false;
			}
		}
		else
		{
			FP y = 1f / value.X;
			FP a = (LowerBound.X - p.X) * y;
			FP b = (UpperBound.X - p.X) * y;
			float num = -1f;
			if (a > b)
			{
				MathUtils.Swap(ref a, ref b);
				num = 1f;
			}
			if (a > fP)
			{
				vector.SetZero();
				vector.X = num;
				fP = a;
			}
			fP2 = FP.Min(fP2, b);
			if (fP > fP2)
			{
				return false;
			}
		}
		if (fVector.Y < Settings.Epsilon)
		{
			if (p.Y < LowerBound.Y || UpperBound.Y < p.Y)
			{
				return false;
			}
		}
		else
		{
			FP y2 = 1f / value.Y;
			FP a2 = (LowerBound.Y - p.Y) * y2;
			FP b2 = (UpperBound.Y - p.Y) * y2;
			float num2 = -1f;
			if (a2 > b2)
			{
				MathUtils.Swap(ref a2, ref b2);
				num2 = 1f;
			}
			if (a2 > fP)
			{
				vector.SetZero();
				vector.Y = num2;
				fP = a2;
			}
			fP2 = FP.Min(fP2, b2);
			if (fP > fP2)
			{
				return false;
			}
		}
		if (fP < 0f || input.MaxFraction < fP)
		{
			return false;
		}
		output = new RayCastOutput
		{
			Fraction = fP,
			Normal = vector
		};
		return true;
	}

	public static void Combine(in AABB left, in AABB right, out AABB aabb)
	{
		aabb = new AABB(FVector2.Min(left.LowerBound, right.LowerBound), FVector2.Max(left.UpperBound, right.UpperBound));
	}

	public void Combine(in AABB aabb)
	{
		LowerBound = FVector2.Min(LowerBound, aabb.LowerBound);
		UpperBound = FVector2.Max(UpperBound, aabb.UpperBound);
	}

	public void Combine(in AABB aabb1, in AABB aabb2)
	{
		LowerBound = FVector2.Min(aabb1.LowerBound, aabb2.LowerBound);
		UpperBound = FVector2.Max(aabb1.UpperBound, aabb2.UpperBound);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(in AABB aabb)
	{
		if (LowerBound.X <= aabb.LowerBound.X && LowerBound.Y <= aabb.LowerBound.Y && aabb.UpperBound.X <= UpperBound.X)
		{
			return aabb.UpperBound.Y <= UpperBound.Y;
		}
		return false;
	}
}
