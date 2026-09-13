using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Foreign;

namespace Box2DSharp.Collision.Shapes;

public abstract class Shape
{
	public FP Radius { get; internal set; }

	public ShapeType ShapeType { get; internal set; }

	public abstract Shape Clone();

	public abstract int GetChildCount();

	public abstract bool TestPoint(in Transform transform, in FVector2 point);

	public abstract bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex);

	public abstract void ComputeAABB(out AABB aabb, in Transform xf, int childIndex);

	public abstract void ComputeMass(out MassData massData, FP density);

	public virtual PhysicsSnapShot.ComponentPhysicsShapeData TakeSnapShot()
	{
		return null;
	}

	public virtual void RestoreSnapshot(PhysicsSnapShot.ComponentPhysicsShapeData shapeData)
	{
	}
}
