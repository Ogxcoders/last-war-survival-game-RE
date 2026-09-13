using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Foreign;

namespace Box2DSharp.Collision.Shapes;

public class CircleShape : Shape
{
	public FVector2 Position;

	public new FP Radius
	{
		get
		{
			return base.Radius;
		}
		set
		{
			base.Radius = value;
		}
	}

	public CircleShape()
	{
		base.ShapeType = ShapeType.Circle;
		Radius = 0;
		Position.SetZero();
	}

	public override Shape Clone()
	{
		return new CircleShape
		{
			Position = Position,
			Radius = Radius
		};
	}

	public override int GetChildCount()
	{
		return 1;
	}

	public override bool TestPoint(in Transform transform, in FVector2 p)
	{
		FVector2 fVector = transform.Position + MathUtils.Mul(in transform.Rotation, in Position);
		FVector2 fVector2 = p - fVector;
		return FVector2.Dot(fVector2, fVector2) <= Radius * Radius;
	}

	public override bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex)
	{
		output = default(RayCastOutput);
		FVector2 fVector = transform.Position + MathUtils.Mul(in transform.Rotation, in Position);
		FVector2 fVector2 = input.P1 - fVector;
		FP y = FVector2.Dot(fVector2, fVector2) - Radius * Radius;
		FVector2 fVector3 = input.P2 - input.P1;
		FP x = FVector2.Dot(fVector2, fVector3);
		FP x2 = FVector2.Dot(fVector3, fVector3);
		FP fP = x * x - x2 * y;
		if (fP < FP.Zero || x2 < Settings.Epsilon)
		{
			return false;
		}
		FP fP2 = -(x + FP.Sqrt(fP));
		if (FP.Zero <= fP2 && fP2 <= input.MaxFraction * x2)
		{
			fP2 /= x2;
			output = new RayCastOutput
			{
				Fraction = fP2,
				Normal = fVector2 + fP2 * fVector3
			};
			output.Normal.Normalize();
			return true;
		}
		return false;
	}

	public override void ComputeAABB(out AABB aabb, in Transform transform, int childIndex)
	{
		FVector2 fVector = transform.Position + MathUtils.Mul(in transform.Rotation, in Position);
		aabb = default(AABB);
		aabb.LowerBound.Set(fVector.X - Radius, fVector.Y - Radius);
		aabb.UpperBound.Set(fVector.X + Radius, fVector.Y + Radius);
	}

	public override void ComputeMass(out MassData massData, FP density)
	{
		massData = new MassData
		{
			Mass = density * Settings.Pi * Radius * Radius,
			Center = Position
		};
		massData.RotationInertia = massData.Mass * ((FP)0.5f * Radius * Radius + FVector2.Dot(Position, Position));
	}

	public override PhysicsSnapShot.ComponentPhysicsShapeData TakeSnapShot()
	{
		return new PhysicsSnapShot.ComponentPhysicsCircleData
		{
			Position = Position,
			Radius = Radius
		};
	}

	public override void RestoreSnapshot(PhysicsSnapShot.ComponentPhysicsShapeData shapeData)
	{
		if (shapeData is PhysicsSnapShot.ComponentPhysicsCircleData componentPhysicsCircleData)
		{
			Radius = componentPhysicsCircleData.Radius;
			Position = componentPhysicsCircleData.Position;
		}
	}
}
