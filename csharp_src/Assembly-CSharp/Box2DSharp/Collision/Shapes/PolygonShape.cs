using System;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Foreign;

namespace Box2DSharp.Collision.Shapes;

public class PolygonShape : Shape
{
	public const int MaxPolygonVertices = 8;

	public readonly FVector2[] Normals = new FVector2[8];

	public readonly FVector2[] Vertices = new FVector2[8];

	public FVector2 Centroid;

	public int Count;

	private static FP inv3 = (FP)1 / (FP)3;

	public PolygonShape()
	{
		base.ShapeType = ShapeType.Polygon;
		base.Radius = Settings.PolygonRadius;
	}

	public override Shape Clone()
	{
		PolygonShape polygonShape = new PolygonShape
		{
			Centroid = Centroid,
			Count = Count
		};
		Array.Copy(Vertices, polygonShape.Vertices, Vertices.Length);
		Array.Copy(Normals, polygonShape.Normals, Normals.Length);
		return polygonShape;
	}

	public override int GetChildCount()
	{
		return 1;
	}

	public void Set(FVector2[] vertices, int count = -1)
	{
		if (count == -1)
		{
			count = vertices.Length;
		}
		if (count < 3)
		{
			SetAsBox(1f, 1f);
			return;
		}
		int num = Math.Min(count, 8);
		Span<FVector2> span = stackalloc FVector2[8];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			FVector2 fVector = vertices[i];
			bool flag = true;
			for (int j = 0; j < num2; j++)
			{
				if (FVector2.DistanceSquared(fVector, span[j]) < (FP)0.5f * Settings.LinearSlop * ((FP)0.5f * Settings.LinearSlop))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				span[num2] = fVector;
				num2++;
			}
		}
		num = num2;
		if (num < 3)
		{
			throw new InvalidOperationException("Invalid polygon shape");
		}
		int num3 = 0;
		FP fP = span[0].X;
		for (int k = 1; k < num; k++)
		{
			FP x = span[k].X;
			if (x > fP || (x.Equals(fP) && span[k].Y < span[num3].Y))
			{
				num3 = k;
				fP = x;
			}
		}
		Span<int> span2 = stackalloc int[8];
		int num4 = 0;
		int num5 = num3;
		int num6;
		do
		{
			span2[num4] = num5;
			num6 = 0;
			for (int l = 1; l < num; l++)
			{
				if (num6 == num5)
				{
					num6 = l;
					continue;
				}
				FVector2 a = span[num6] - span[span2[num4]];
				FVector2 b = span[l] - span[span2[num4]];
				FP fP2 = MathUtils.Cross(in a, in b);
				if (fP2 < 0f)
				{
					num6 = l;
				}
				if (fP2 == FP.Zero && b.LengthSquared() > a.LengthSquared())
				{
					num6 = l;
				}
			}
			num4++;
			num5 = num6;
		}
		while (num6 != num3);
		if (num4 < 3)
		{
			throw new InvalidOperationException("Invalid polygon shape");
		}
		Count = num4;
		for (int m = 0; m < num4; m++)
		{
			Vertices[m] = span[span2[m]];
		}
		for (int n = 0; n < num4; n++)
		{
			int num7 = n;
			int num8 = ((n + 1 < num4) ? (n + 1) : 0);
			FVector2 a2 = Vertices[num8] - Vertices[num7];
			Normals[n] = MathUtils.Cross(in a2, FP.One);
			Normals[n].Normalize();
		}
		Centroid = ComputeCentroid(in Vertices, num4);
	}

	public void SetAsBox(FP hx, FP hy)
	{
		Count = 4;
		Vertices[0].Set(-hx, -hy);
		Vertices[1].Set(hx, -hy);
		Vertices[2].Set(hx, hy);
		Vertices[3].Set(-hx, hy);
		Normals[0].Set(0f, -1f);
		Normals[1].Set(1f, 0f);
		Normals[2].Set(0f, 1f);
		Normals[3].Set(-1f, 0f);
		Centroid.SetZero();
	}

	public void SetAsBox(FP hx, FP hy, in FVector2 center, FP angle)
	{
		SetAsBox(hx, hy);
		Centroid = center;
		Transform T = new Transform(in center, angle);
		for (int i = 0; i < Count; i++)
		{
			Vertices[i] = MathUtils.Mul(in T, in Vertices[i]);
			Normals[i] = MathUtils.Mul(in T.Rotation, in Normals[i]);
		}
	}

	public override bool TestPoint(in Transform transform, in FVector2 p)
	{
		FVector2 fVector = MathUtils.MulT(in transform.Rotation, p - transform.Position);
		for (int i = 0; i < Count; i++)
		{
			if (FVector2.Dot(Normals[i], fVector - Vertices[i]) > 0f)
			{
				return false;
			}
		}
		return true;
	}

	public override bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex)
	{
		output = default(RayCastOutput);
		FVector2 fVector = MathUtils.MulT(in transform.Rotation, input.P1 - transform.Position);
		FVector2 value = MathUtils.MulT(in transform.Rotation, input.P2 - transform.Position) - fVector;
		FP x = FP.Zero;
		FP x2 = input.MaxFraction;
		int num = -1;
		for (int i = 0; i < Count; i++)
		{
			FP fP = FVector2.Dot(Normals[i], Vertices[i] - fVector);
			FP y = FVector2.Dot(Normals[i], value);
			if (y == FP.Zero)
			{
				if (fP < 0f)
				{
					return false;
				}
			}
			else if (y < 0f && fP < x * y)
			{
				x = fP / y;
				num = i;
			}
			else if (y > 0f && fP < x2 * y)
			{
				x2 = fP / y;
			}
			if (x2 < x)
			{
				return false;
			}
		}
		if (num >= 0)
		{
			output = new RayCastOutput
			{
				Fraction = x,
				Normal = MathUtils.Mul(in transform.Rotation, in Normals[num])
			};
			return true;
		}
		return false;
	}

	public override void ComputeAABB(out AABB aabb, in Transform transform, int childIndex)
	{
		FVector2 fVector = MathUtils.Mul(in transform, in Vertices[0]);
		FVector2 fVector2 = fVector;
		for (int i = 1; i < Count; i++)
		{
			FVector2 value = MathUtils.Mul(in transform, in Vertices[i]);
			fVector = FVector2.Min(fVector, value);
			fVector2 = FVector2.Max(fVector2, value);
		}
		FVector2 fVector3 = new FVector2(base.Radius, base.Radius);
		aabb = new AABB
		{
			LowerBound = fVector - fVector3,
			UpperBound = fVector2 + fVector3
		};
	}

	public override void ComputeMass(out MassData massData, FP density)
	{
		FVector2 zero = FVector2.Zero;
		FP x = FP.Zero;
		FP x2 = FP.Zero;
		ref FVector2 reference = ref Vertices[0];
		for (int i = 0; i < Count; i++)
		{
			FVector2 a = Vertices[i] - reference;
			FVector2 b = ((i + 1 < Count) ? (Vertices[i + 1] - reference) : (Vertices[0] - reference));
			FP y = MathUtils.Cross(in a, in b);
			FP y2 = (FP)0.5f * y;
			x += y2;
			zero += y2 * inv3 * (a + b);
			FP x3 = a.X;
			FP x4 = a.Y;
			FP x5 = b.X;
			FP x6 = b.Y;
			FP x7 = x3 * x3 + x5 * x3 + x5 * x5;
			FP y3 = x4 * x4 + x6 * x4 + x6 * x6;
			x2 += (FP)0.25f * inv3 * y * (x7 + y3);
		}
		massData = new MassData
		{
			Mass = density * x
		};
		zero *= 1f / x;
		massData.Center = zero + reference;
		massData.RotationInertia = density * x2;
		ref FP rotationInertia = ref massData.RotationInertia;
		rotationInertia += massData.Mass * (FVector2.Dot(massData.Center, massData.Center) - FVector2.Dot(zero, zero));
	}

	public bool Validate()
	{
		for (int i = 0; i < Count; i++)
		{
			int num = i;
			int num2 = ((i < Count - 1) ? (num + 1) : 0);
			FVector2 fVector = Vertices[num];
			FVector2 a = Vertices[num2] - fVector;
			for (int j = 0; j < Count; j++)
			{
				if (j != num && j != num2 && MathUtils.Cross(in a, Vertices[j] - fVector) < 0f)
				{
					return false;
				}
			}
		}
		return true;
	}

	private static FVector2 ComputeCentroid(in FVector2[] vs, int count)
	{
		FVector2 zero = FVector2.Zero;
		FP x = FP.Zero;
		FVector2 fVector = vs[0];
		for (int i = 0; i < count; i++)
		{
			FVector2 fVector2 = vs[0] - fVector;
			FVector2 fVector3 = vs[i] - fVector;
			FVector2 fVector4 = ((i + 1 < count) ? (vs[i + 1] - fVector) : (vs[0] - fVector));
			FP y = MathUtils.Cross(fVector3 - fVector2, fVector4 - fVector2);
			FP y2 = (FP)0.5 * y;
			x += y2;
			zero += y2 * inv3 * (fVector2 + fVector3 + fVector4);
		}
		return FP.One / x * zero + fVector;
	}

	public override PhysicsSnapShot.ComponentPhysicsShapeData TakeSnapShot()
	{
		return new PhysicsSnapShot.ComponentPhysicsPolygonData
		{
			BoxCenter = Centroid,
			Count = Count,
			Vertices = Vertices,
			Normals = Normals
		};
	}

	public override void RestoreSnapshot(PhysicsSnapShot.ComponentPhysicsShapeData shapeData)
	{
		if (shapeData is PhysicsSnapShot.ComponentPhysicsPolygonData componentPhysicsPolygonData)
		{
			Count = componentPhysicsPolygonData.Count;
			Centroid = componentPhysicsPolygonData.BoxCenter;
			for (int i = 0; i < Count; i++)
			{
				Vertices[i] = componentPhysicsPolygonData.Vertices[i];
			}
			for (int j = 0; j < Count; j++)
			{
				Normals[j] = componentPhysicsPolygonData.Normals[j];
			}
		}
	}
}
