using System.ComponentModel;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public struct PositionSolverManifold
{
	public FVector2 Normal;

	public FVector2 Point;

	public FP Separation;

	public void Initialize(in ContactPositionConstraint pc, in Transform xfA, in Transform xfB, int index)
	{
		switch (pc.Type)
		{
		case ManifoldType.Circles:
		{
			FP x3 = xfA.Rotation.Cos * pc.LocalPoint.X - xfA.Rotation.Sin * pc.LocalPoint.Y + xfA.Position.X;
			FP y3 = xfA.Rotation.Sin * pc.LocalPoint.X + xfA.Rotation.Cos * pc.LocalPoint.Y + xfA.Position.Y;
			FVector2 fVector5 = new FVector2(x3, y3);
			x3 = xfB.Rotation.Cos * pc.LocalPoints.Value0.X - xfB.Rotation.Sin * pc.LocalPoints.Value0.Y + xfB.Position.X;
			y3 = xfB.Rotation.Sin * pc.LocalPoints.Value0.X + xfB.Rotation.Cos * pc.LocalPoints.Value0.Y + xfB.Position.Y;
			FVector2 fVector6 = new FVector2(x3, y3);
			Normal = fVector6 - fVector5;
			Normal.Normalize();
			Point = 0.5f * (fVector5 + fVector6);
			Separation = FVector2.Dot(fVector6 - fVector5, Normal) - pc.RadiusA - pc.RadiusB;
			break;
		}
		case ManifoldType.FaceA:
		{
			Normal = new FVector2(xfA.Rotation.Cos * pc.LocalNormal.X - xfA.Rotation.Sin * pc.LocalNormal.Y, xfA.Rotation.Sin * pc.LocalNormal.X + xfA.Rotation.Cos * pc.LocalNormal.Y);
			FP x2 = xfA.Rotation.Cos * pc.LocalPoint.X - xfA.Rotation.Sin * pc.LocalPoint.Y + xfA.Position.X;
			FP y2 = xfA.Rotation.Sin * pc.LocalPoint.X + xfA.Rotation.Cos * pc.LocalPoint.Y + xfA.Position.Y;
			FVector2 fVector3 = new FVector2(x2, y2);
			if (index == 0)
			{
				x2 = xfB.Rotation.Cos * pc.LocalPoints.Value0.X - xfB.Rotation.Sin * pc.LocalPoints.Value0.Y + xfB.Position.X;
				y2 = xfB.Rotation.Sin * pc.LocalPoints.Value0.X + xfB.Rotation.Cos * pc.LocalPoints.Value0.Y + xfB.Position.Y;
			}
			else
			{
				x2 = xfB.Rotation.Cos * pc.LocalPoints.Value1.X - xfB.Rotation.Sin * pc.LocalPoints.Value1.Y + xfB.Position.X;
				y2 = xfB.Rotation.Sin * pc.LocalPoints.Value1.X + xfB.Rotation.Cos * pc.LocalPoints.Value1.Y + xfB.Position.Y;
			}
			FVector2 fVector4 = new FVector2(x2, y2);
			Separation = FVector2.Dot(fVector4 - fVector3, Normal) - pc.RadiusA - pc.RadiusB;
			Point = fVector4;
			break;
		}
		case ManifoldType.FaceB:
		{
			Normal = new FVector2(xfB.Rotation.Cos * pc.LocalNormal.X - xfB.Rotation.Sin * pc.LocalNormal.Y, xfB.Rotation.Sin * pc.LocalNormal.X + xfB.Rotation.Cos * pc.LocalNormal.Y);
			FP x = xfB.Rotation.Cos * pc.LocalPoint.X - xfB.Rotation.Sin * pc.LocalPoint.Y + xfB.Position.X;
			FP y = xfB.Rotation.Sin * pc.LocalPoint.X + xfB.Rotation.Cos * pc.LocalPoint.Y + xfB.Position.Y;
			FVector2 fVector = new FVector2(x, y);
			if (index == 0)
			{
				x = xfA.Rotation.Cos * pc.LocalPoints.Value0.X - xfA.Rotation.Sin * pc.LocalPoints.Value0.Y + xfA.Position.X;
				y = xfA.Rotation.Sin * pc.LocalPoints.Value0.X + xfA.Rotation.Cos * pc.LocalPoints.Value0.Y + xfA.Position.Y;
			}
			else
			{
				x = xfA.Rotation.Cos * pc.LocalPoints.Value1.X - xfA.Rotation.Sin * pc.LocalPoints.Value1.Y + xfA.Position.X;
				y = xfA.Rotation.Sin * pc.LocalPoints.Value1.X + xfA.Rotation.Cos * pc.LocalPoints.Value1.Y + xfA.Position.Y;
			}
			FVector2 fVector2 = new FVector2(x, y);
			Separation = FVector2.Dot(fVector2 - fVector, Normal) - pc.RadiusA - pc.RadiusB;
			Point = fVector2;
			Normal = -Normal;
			break;
		}
		default:
			throw new InvalidEnumArgumentException($"Invalid ManifoldType: {pc.Type}");
		}
	}
}
