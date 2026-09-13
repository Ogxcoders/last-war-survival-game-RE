using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Collision.Shapes;

public class EdgeShape : Shape
{
	public FVector2 Vertex1;

	public FVector2 Vertex2;

	public FVector2 Vertex0;

	public FVector2 Vertex3;

	public bool OneSided;

	public EdgeShape()
	{
		base.ShapeType = ShapeType.Edge;
		base.Radius = Settings.PolygonRadius;
	}

	public void SetOneSided(in FVector2 v0, in FVector2 v1, in FVector2 v2, in FVector2 v3)
	{
		Vertex0 = v0;
		Vertex1 = v1;
		Vertex2 = v2;
		Vertex3 = v3;
		OneSided = true;
	}

	public void SetTwoSided(in FVector2 v1, in FVector2 v2)
	{
		Vertex1 = v1;
		Vertex2 = v2;
		OneSided = false;
	}

	public override Shape Clone()
	{
		return new EdgeShape
		{
			Vertex0 = Vertex0,
			Vertex1 = Vertex1,
			Vertex2 = Vertex2,
			Vertex3 = Vertex3,
			OneSided = OneSided
		};
	}

	public override int GetChildCount()
	{
		return 1;
	}

	public override bool TestPoint(in Transform transform, in FVector2 point)
	{
		return false;
	}

	public override bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex)
	{
		output = default(RayCastOutput);
		FVector2 fVector = MathUtils.MulT(in transform.Rotation, input.P1 - transform.Position);
		FVector2 fVector2 = MathUtils.MulT(in transform.Rotation, input.P2 - transform.Position) - fVector;
		FVector2 vertex = Vertex1;
		FVector2 vertex2 = Vertex2;
		FVector2 fVector3 = vertex2 - vertex;
		FVector2 v = new FVector2(fVector3.Y, -fVector3.X);
		v.Normalize();
		FP fP = FVector2.Dot(v, vertex - fVector);
		if (OneSided && fP > FP.Zero)
		{
			return false;
		}
		FP fP2 = FVector2.Dot(v, fVector2);
		if (FP.Abs(fP2) < Settings.Epsilon)
		{
			return false;
		}
		FP fP3 = fP / fP2;
		if (fP3 < FP.Zero || input.MaxFraction < fP3)
		{
			return false;
		}
		FVector2 fVector4 = fVector + fP3 * fVector2;
		FVector2 fVector5 = vertex2 - vertex;
		FP fP4 = FVector2.Dot(fVector5, fVector5);
		if (FP.Abs(fP4) < Settings.Epsilon)
		{
			return false;
		}
		FP fP5 = FVector2.Dot(fVector4 - vertex, fVector5) / fP4;
		if (fP5 < FP.Zero || FP.One < fP5)
		{
			return false;
		}
		output = new RayCastOutput
		{
			Fraction = fP3,
			Normal = ((fP > FP.Zero) ? (-MathUtils.Mul(in transform.Rotation, in v)) : MathUtils.Mul(in transform.Rotation, in v))
		};
		return true;
	}

	public override void ComputeAABB(out AABB aabb, in Transform xf, int childIndex)
	{
		FVector2 value = MathUtils.Mul(in xf, in Vertex1);
		FVector2 value2 = MathUtils.Mul(in xf, in Vertex2);
		FVector2 fVector = FVector2.Min(value, value2);
		FVector2 fVector2 = FVector2.Max(value, value2);
		FVector2 fVector3 = new FVector2(base.Radius, base.Radius);
		aabb = new AABB(fVector - fVector3, fVector2 + fVector3);
	}

	public override void ComputeMass(out MassData massData, FP density)
	{
		massData = new MassData
		{
			Mass = 0,
			Center = 0.5 * (Vertex1 + Vertex2),
			RotationInertia = 0
		};
	}
}
