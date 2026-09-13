using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct SeparationFunction
{
	public enum FunctionType
	{
		Points,
		FaceA,
		FaceB
	}

	public FVector2 Axis;

	public FVector2 LocalPoint;

	public DistanceProxy ProxyA;

	public DistanceProxy ProxyB;

	public Sweep SweepA;

	public Sweep SweepB;

	public FunctionType Type;

	public FP Initialize(ref SimplexCache cache, DistanceProxy proxyA, in Sweep sweepA, DistanceProxy proxyB, in Sweep sweepB, FP t1)
	{
		ProxyA = proxyA;
		ProxyB = proxyB;
		ushort count = cache.Count;
		byte value = cache.IndexA.Value0;
		byte value2 = cache.IndexA.Value1;
		byte value3 = cache.IndexB.Value0;
		byte value4 = cache.IndexB.Value1;
		SweepA = sweepA;
		SweepB = sweepB;
		SweepA.GetTransform(out var xf, t1);
		SweepB.GetTransform(out var xf2, t1);
		if (count == 1)
		{
			Type = FunctionType.Points;
			FVector2 v = ProxyA.GetVertex(value);
			FVector2 v2 = ProxyB.GetVertex(value3);
			FVector2 fVector = MathUtils.Mul(in xf, in v);
			FVector2 fVector2 = MathUtils.Mul(in xf2, in v2);
			Axis = fVector2 - fVector;
			return Axis.Normalize();
		}
		if (value == value2)
		{
			Type = FunctionType.FaceB;
			FVector2 vertex = proxyB.GetVertex(value3);
			FVector2 vertex2 = proxyB.GetVertex(value4);
			Axis = MathUtils.Cross(vertex2 - vertex, 1f);
			Axis.Normalize();
			FVector2 value5 = MathUtils.Mul(in xf2.Rotation, in Axis);
			LocalPoint = 0.5f * (vertex + vertex2);
			FVector2 fVector3 = MathUtils.Mul(in xf2, in LocalPoint);
			FVector2 v3 = proxyA.GetVertex(value);
			FP fP = FVector2.Dot(MathUtils.Mul(in xf, in v3) - fVector3, value5);
			if (fP < 0f)
			{
				Axis = -Axis;
				fP = -fP;
			}
			return fP;
		}
		Type = FunctionType.FaceA;
		FVector2 vertex3 = ProxyA.GetVertex(value);
		FVector2 vertex4 = ProxyA.GetVertex(value2);
		Axis = MathUtils.Cross(vertex4 - vertex3, 1f);
		Axis.Normalize();
		FVector2 value6 = MathUtils.Mul(in xf.Rotation, in Axis);
		LocalPoint = 0.5f * (vertex3 + vertex4);
		FVector2 fVector4 = MathUtils.Mul(in xf, in LocalPoint);
		FVector2 v4 = ProxyB.GetVertex(value3);
		FP fP2 = FVector2.Dot(MathUtils.Mul(in xf2, in v4) - fVector4, value6);
		if (fP2 < 0f)
		{
			Axis = -Axis;
			fP2 = -fP2;
		}
		return fP2;
	}

	public FP FindMinSeparation(out int indexA, out int indexB, FP t)
	{
		SweepA.GetTransform(out var xf, t);
		SweepB.GetTransform(out var xf2, t);
		switch (Type)
		{
		case FunctionType.Points:
		{
			FVector2 d3 = MathUtils.MulT(in xf.Rotation, in Axis);
			FVector2 d4 = MathUtils.MulT(in xf2.Rotation, -Axis);
			indexA = ProxyA.GetSupport(in d3);
			indexB = ProxyB.GetSupport(in d4);
			FVector2 v3 = ProxyA.GetVertex(indexA);
			FVector2 v4 = ProxyB.GetVertex(indexB);
			FVector2 fVector5 = MathUtils.Mul(in xf, in v3);
			return FVector2.Dot(MathUtils.Mul(in xf2, in v4) - fVector5, Axis);
		}
		case FunctionType.FaceA:
		{
			FVector2 fVector3 = MathUtils.Mul(in xf.Rotation, in Axis);
			FVector2 fVector4 = MathUtils.Mul(in xf, in LocalPoint);
			FVector2 d2 = MathUtils.MulT(in xf2.Rotation, -fVector3);
			indexA = -1;
			indexB = ProxyB.GetSupport(in d2);
			FVector2 v2 = ProxyB.GetVertex(indexB);
			return FVector2.Dot(MathUtils.Mul(in xf2, in v2) - fVector4, fVector3);
		}
		case FunctionType.FaceB:
		{
			FVector2 fVector = MathUtils.Mul(in xf2.Rotation, in Axis);
			FVector2 fVector2 = MathUtils.Mul(in xf2, in LocalPoint);
			FVector2 d = MathUtils.MulT(in xf.Rotation, -fVector);
			indexB = -1;
			indexA = ProxyA.GetSupport(in d);
			FVector2 v = ProxyA.GetVertex(indexA);
			return FVector2.Dot(MathUtils.Mul(in xf, in v) - fVector2, fVector);
		}
		default:
			indexA = -1;
			indexB = -1;
			return 0f;
		}
	}

	public FP Evaluate(int indexA, int indexB, FP t)
	{
		SweepA.GetTransform(out var xf, t);
		SweepB.GetTransform(out var xf2, t);
		switch (Type)
		{
		case FunctionType.Points:
		{
			FVector2 v3 = ProxyA.GetVertex(indexA);
			FVector2 v4 = ProxyB.GetVertex(indexB);
			FVector2 fVector3 = MathUtils.Mul(in xf, in v3);
			return FVector2.Dot(MathUtils.Mul(in xf2, in v4) - fVector3, Axis);
		}
		case FunctionType.FaceA:
		{
			FVector2 value2 = MathUtils.Mul(in xf.Rotation, in Axis);
			FVector2 fVector2 = MathUtils.Mul(in xf, in LocalPoint);
			FVector2 v2 = ProxyB.GetVertex(indexB);
			return FVector2.Dot(MathUtils.Mul(in xf2, in v2) - fVector2, value2);
		}
		case FunctionType.FaceB:
		{
			FVector2 value = MathUtils.Mul(in xf2.Rotation, in Axis);
			FVector2 fVector = MathUtils.Mul(in xf2, in LocalPoint);
			FVector2 v = ProxyA.GetVertex(indexA);
			return FVector2.Dot(MathUtils.Mul(in xf, in v) - fVector, value);
		}
		default:
			return 0f;
		}
	}
}
