using System;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct Simplex
{
	public FixedArray3<SimplexVertex> Vertices;

	public int Count;

	public void ReadCache(ref SimplexCache cache, in DistanceProxy proxyA, in Transform transformA, in DistanceProxy proxyB, in Transform transformB)
	{
		Count = cache.Count;
		for (int i = 0; i < Count; i++)
		{
			ref SimplexVertex reference = ref Vertices[i];
			reference.IndexA = cache.IndexA[i];
			reference.IndexB = cache.IndexB[i];
			FVector2 v = proxyA.GetVertex(reference.IndexA);
			FVector2 v2 = proxyB.GetVertex(reference.IndexB);
			reference.Wa = MathUtils.Mul(in transformA, in v);
			reference.Wb = MathUtils.Mul(in transformB, in v2);
			reference.W = reference.Wb - reference.Wa;
			reference.A = 0f;
		}
		if (Count > 1)
		{
			FP y = cache.Metric;
			FP metric = GetMetric();
			if (metric < (FP)0.5f * y || (FP)2f * y < metric || metric < Settings.Epsilon)
			{
				Count = 0;
			}
		}
		if (Count == 0)
		{
			ref SimplexVertex reference2 = ref Vertices[0];
			reference2.IndexA = 0;
			reference2.IndexB = 0;
			FVector2 v3 = proxyA.GetVertex(0);
			FVector2 v4 = proxyB.GetVertex(0);
			reference2.Wa = MathUtils.Mul(in transformA, in v3);
			reference2.Wb = MathUtils.Mul(in transformB, in v4);
			reference2.W = reference2.Wb - reference2.Wa;
			reference2.A = 1f;
			Count = 1;
		}
	}

	public void WriteCache(ref SimplexCache cache)
	{
		cache.Metric = GetMetric();
		cache.Count = (ushort)Count;
		for (int i = 0; i < Count; i++)
		{
			cache.IndexA[i] = (byte)Vertices[i].IndexA;
			cache.IndexB[i] = (byte)Vertices[i].IndexB;
		}
	}

	public FVector2 GetSearchDirection()
	{
		switch (Count)
		{
		case 1:
			return -Vertices.Value0.W;
		case 2:
		{
			FVector2 a = Vertices.Value1.W - Vertices.Value0.W;
			if (MathUtils.Cross(in a, -Vertices.Value0.W) > 0f)
			{
				return MathUtils.Cross(1f, in a);
			}
			return MathUtils.Cross(in a, 1f);
		}
		default:
			throw new ArgumentOutOfRangeException("Count");
		}
	}

	public FVector2 GetClosestPoint()
	{
		return Count switch
		{
			1 => Vertices.Value0.W, 
			2 => Vertices.Value0.A * Vertices.Value0.W + Vertices.Value1.A * Vertices.Value1.W, 
			3 => FVector2.Zero, 
			_ => throw new ArgumentOutOfRangeException("Count"), 
		};
	}

	public void GetWitnessPoints(out FVector2 pA, out FVector2 pB)
	{
		switch (Count)
		{
		case 1:
			pA = Vertices.Value0.Wa;
			pB = Vertices.Value0.Wb;
			break;
		case 2:
			pA = Vertices.Value0.A * Vertices.Value0.Wa + Vertices.Value1.A * Vertices.Value1.Wa;
			pB = Vertices.Value0.A * Vertices.Value0.Wb + Vertices.Value1.A * Vertices.Value1.Wb;
			break;
		case 3:
			pA = Vertices.Value0.A * Vertices.Value0.Wa + Vertices.Value1.A * Vertices.Value1.Wa + Vertices.Value2.A * Vertices.Value2.Wa;
			pB = pA;
			break;
		default:
			throw new ArgumentOutOfRangeException("Count");
		}
	}

	public FP GetMetric()
	{
		return Count switch
		{
			0 => FP.Zero, 
			1 => FP.Zero, 
			2 => FVector2.Distance(Vertices.Value0.W, Vertices.Value1.W), 
			3 => MathUtils.Cross(Vertices.Value1.W - Vertices.Value0.W, Vertices.Value2.W - Vertices.Value0.W), 
			_ => throw new ArgumentOutOfRangeException("Count"), 
		};
	}

	public void Solve2()
	{
		ref SimplexVertex value = ref Vertices.Value0;
		ref SimplexVertex value2 = ref Vertices.Value1;
		FVector2 w = value.W;
		FVector2 w2 = value2.W;
		FVector2 value3 = w2 - w;
		FP y = -FVector2.Dot(w, value3);
		if (y <= FP.Zero)
		{
			value.A = FP.One;
			Count = 1;
			return;
		}
		FP x = FVector2.Dot(w2, value3);
		if (x <= FP.Zero)
		{
			value2.A = FP.One;
			Count = 1;
			Vertices.Value0 = Vertices.Value1;
		}
		else
		{
			FP y2 = FP.One / (x + y);
			value.A = x * y2;
			value2.A = y * y2;
			Count = 2;
		}
	}

	public void Solve3()
	{
		ref SimplexVertex value = ref Vertices.Value0;
		ref SimplexVertex value2 = ref Vertices.Value1;
		ref SimplexVertex value3 = ref Vertices.Value2;
		FVector2 b = value.W;
		FVector2 a = value2.W;
		FVector2 b2 = value3.W;
		FVector2 a2 = a - b;
		FP fP = FVector2.Dot(b, a2);
		FP x = FVector2.Dot(a, a2);
		FP y = -fP;
		FVector2 b3 = b2 - b;
		FP fP2 = FVector2.Dot(b, b3);
		FP x2 = FVector2.Dot(b2, b3);
		FP y2 = -fP2;
		FVector2 value4 = b2 - a;
		FP fP3 = FVector2.Dot(a, value4);
		FP x3 = FVector2.Dot(b2, value4);
		FP y3 = -fP3;
		FP x4 = MathUtils.Cross(in a2, in b3);
		FP x5 = x4 * MathUtils.Cross(in a, in b2);
		FP y4 = x4 * MathUtils.Cross(in b2, in b);
		FP y5 = x4 * MathUtils.Cross(in b, in a);
		if (y <= FP.Zero && y2 <= FP.Zero)
		{
			value.A = FP.One;
			Count = 1;
		}
		else if (x > FP.Zero && y > FP.Zero && y5 <= FP.Zero)
		{
			FP y6 = FP.One / (x + y);
			value.A = x * y6;
			value2.A = y * y6;
			Count = 2;
		}
		else if (x2 > FP.Zero && y2 > FP.Zero && y4 <= FP.Zero)
		{
			FP y7 = FP.One / (x2 + y2);
			value.A = x2 * y7;
			value3.A = y2 * y7;
			Count = 2;
			value2 = value3;
		}
		else if (x <= FP.Zero && y3 <= FP.Zero)
		{
			value2.A = FP.One;
			Count = 1;
			value = value2;
		}
		else if (x2 <= FP.Zero && x3 <= FP.Zero)
		{
			value3.A = FP.One;
			Count = 1;
			value = value3;
		}
		else if (x3 > 0f && y3 > 0f && x5 <= 0f)
		{
			FP y8 = 1f / (x3 + y3);
			value2.A = x3 * y8;
			value3.A = y3 * y8;
			Count = 2;
			value = value3;
		}
		else
		{
			FP y9 = 1f / (x5 + y4 + y5);
			value.A = x5 * y9;
			value2.A = y4 * y9;
			value3.A = y5 * y9;
			Count = 3;
		}
	}
}
