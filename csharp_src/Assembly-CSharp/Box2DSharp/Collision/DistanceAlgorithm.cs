using System;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public static class DistanceAlgorithm
{
	public static void Distance(out DistanceOutput output, ref SimplexCache cache, in DistanceInput input, in GJkProfile gJkProfile = null)
	{
		if (gJkProfile != null)
		{
			gJkProfile.GjkCalls++;
		}
		output = default(DistanceOutput);
		ref readonly DistanceProxy proxyA = ref input.ProxyA;
		ref readonly DistanceProxy proxyB = ref input.ProxyB;
		Transform transformA = input.TransformA;
		Transform transformB = input.TransformB;
		Simplex simplex = default(Simplex);
		simplex.ReadCache(ref cache, in proxyA, in transformA, in proxyB, in transformB);
		ref FixedArray3<SimplexVertex> vertices = ref simplex.Vertices;
		Span<int> span = stackalloc int[3];
		Span<int> span2 = stackalloc int[3];
		int num = 0;
		while (num < 20)
		{
			int count = simplex.Count;
			for (int i = 0; i < simplex.Count; i++)
			{
				span[i] = vertices[i].IndexA;
				span2[i] = vertices[i].IndexB;
			}
			switch (simplex.Count)
			{
			case 2:
				simplex.Solve2();
				break;
			case 3:
				simplex.Solve3();
				break;
			default:
				throw new ArgumentOutOfRangeException("Count");
			case 1:
				break;
			}
			if (simplex.Count == 3)
			{
				break;
			}
			FVector2 v = simplex.GetSearchDirection();
			if (v.LengthSquared() < Settings.Epsilon * Settings.Epsilon)
			{
				break;
			}
			ref SimplexVertex reference = ref vertices[simplex.Count];
			reference.IndexA = proxyA.GetSupport(MathUtils.MulT(in transformA.Rotation, -v));
			reference.Wa = MathUtils.Mul(in transformA, in proxyA.GetVertex(reference.IndexA));
			reference.IndexB = proxyB.GetSupport(MathUtils.MulT(in transformB.Rotation, in v));
			reference.Wb = MathUtils.Mul(in transformB, in proxyB.GetVertex(reference.IndexB));
			reference.W = reference.Wb - reference.Wa;
			num++;
			if (gJkProfile != null)
			{
				gJkProfile.GjkIters++;
			}
			bool flag = false;
			for (int j = 0; j < count; j++)
			{
				if (reference.IndexA == span[j] && reference.IndexB == span2[j])
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			simplex.Count++;
		}
		if (gJkProfile != null)
		{
			gJkProfile.GjkMaxIters = Math.Max(gJkProfile.GjkMaxIters, num);
		}
		simplex.GetWitnessPoints(out output.PointA, out output.PointB);
		output.Distance = FVector2.Distance(output.PointA, output.PointB);
		output.Iterations = num;
		simplex.WriteCache(ref cache);
		if (input.UseRadii)
		{
			if (output.Distance < Settings.Epsilon)
			{
				output.PointB = (output.PointA = (output.PointA + output.PointB) / 2);
				output.Distance = FP.Zero;
				return;
			}
			FP y = proxyA.Radius;
			FP y2 = proxyB.Radius;
			FVector2 fVector = output.PointB - output.PointA;
			fVector.Normalize();
			output.Distance = FMath.Max(FP.Zero, output.Distance - y - y2);
			output.PointA += y * fVector;
			output.PointB -= y2 * fVector;
		}
	}

	public static bool ShapeCast(out ShapeCastOutput output, in ShapeCastInput input)
	{
		output = new ShapeCastOutput
		{
			Iterations = 0,
			Lambda = FP.One,
			Normal = FVector2.Zero,
			Point = FVector2.Zero
		};
		ref readonly DistanceProxy proxyA = ref input.ProxyA;
		ref readonly DistanceProxy proxyB = ref input.ProxyB;
		FP x = FMath.Max(proxyA.Radius, Settings.PolygonRadius);
		FP x2 = x + FMath.Max(proxyB.Radius, Settings.PolygonRadius);
		Transform T = input.TransformA;
		Transform T2 = input.TransformB;
		FVector2 v = input.TranslationB;
		FVector2 fVector = FVector2.Zero;
		FP x3 = FP.Zero;
		Simplex simplex = default(Simplex);
		int support = proxyA.GetSupport(MathUtils.MulT(in T.Rotation, -v));
		FVector2 fVector2 = MathUtils.Mul(in T, in proxyA.GetVertex(support));
		int support2 = proxyB.GetSupport(MathUtils.MulT(in T2.Rotation, in v));
		FVector2 fVector3 = MathUtils.Mul(in T2, in proxyB.GetVertex(support2));
		FVector2 v2 = fVector2 - fVector3;
		FP y = FMath.Max(Settings.PolygonRadius, x2 - Settings.PolygonRadius);
		FP fP = Settings.LinearSlop / 2;
		int i;
		for (i = 0; i < 20; i++)
		{
			if (!(v2.Length() - y > fP))
			{
				break;
			}
			output.Iterations++;
			support = proxyA.GetSupport(MathUtils.MulT(in T.Rotation, -v2));
			fVector2 = MathUtils.Mul(in T, in proxyA.GetVertex(support));
			support2 = proxyB.GetSupport(MathUtils.MulT(in T2.Rotation, in v2));
			fVector3 = MathUtils.Mul(in T2, in proxyB.GetVertex(support2));
			FVector2 value = fVector2 - fVector3;
			v2.Normalize();
			FP x4 = FVector2.Dot(v2, value);
			FP y2 = FVector2.Dot(v2, v);
			if (x4 - y > x3 * y2)
			{
				if (y2 <= 0)
				{
					return false;
				}
				x3 = (x4 - y) / y2;
				if (x3 > 1)
				{
					return false;
				}
				fVector = -v2;
				simplex.Count = 0;
			}
			ref SimplexVertex reference = ref simplex.Vertices[simplex.Count];
			reference.IndexA = support2;
			reference.Wa = fVector3 + x3 * v;
			reference.IndexB = support;
			reference.Wb = fVector2;
			reference.W = reference.Wb - reference.Wa;
			reference.A = FP.One;
			simplex.Count++;
			switch (simplex.Count)
			{
			case 2:
				simplex.Solve2();
				break;
			case 3:
				simplex.Solve3();
				break;
			}
			if (simplex.Count == 3)
			{
				return false;
			}
			v2 = simplex.GetClosestPoint();
		}
		if (i == 0)
		{
			return false;
		}
		simplex.GetWitnessPoints(out var _, out var pB);
		if (v2.LengthSquared() > FP.Zero)
		{
			fVector = -v2;
			fVector.Normalize();
		}
		output.Point = pB + x * fVector;
		output.Normal = fVector;
		output.Lambda = x3;
		output.Iterations = i;
		return true;
	}
}
