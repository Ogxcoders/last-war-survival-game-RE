using Box2DSharp.Common;

namespace Box2DSharp.Collision.Collider;

public struct WorldManifold
{
	public FVector2 Normal;

	public FixedArray2<FVector2> Points;

	public FixedArray2<FP> Separations;

	public void Initialize(in Manifold manifold, in Transform xfA, FP radiusA, in Transform xfB, FP radiusB)
	{
		if (manifold.PointCount == 0)
		{
			return;
		}
		switch (manifold.Type)
		{
		case ManifoldType.Circles:
		{
			Normal.Set(1f, 0f);
			FVector2 fVector9 = MathUtils.Mul(in xfA, in manifold.LocalPoint);
			FVector2 fVector10 = MathUtils.Mul(in xfB, in manifold.Points.Value0.LocalPoint);
			if (FVector2.DistanceSquared(fVector9, fVector10) > Settings.Epsilon * Settings.Epsilon)
			{
				Normal = fVector10 - fVector9;
				Normal.Normalize();
			}
			FVector2 fVector11 = fVector9 + radiusA * Normal;
			FVector2 fVector12 = fVector10 - radiusB * Normal;
			Points.Value0 = 0.5f * (fVector11 + fVector12);
			Separations.Value0 = FVector2.Dot(fVector12 - fVector11, Normal);
			break;
		}
		case ManifoldType.FaceA:
		{
			Normal = MathUtils.Mul(in xfA.Rotation, in manifold.LocalNormal);
			FVector2 fVector5 = MathUtils.Mul(in xfA, in manifold.LocalPoint);
			for (int j = 0; j < manifold.PointCount; j++)
			{
				FVector2 fVector6 = MathUtils.Mul(in xfB, in manifold.Points[j].LocalPoint);
				FVector2 fVector7 = fVector6 + (radiusA - FVector2.Dot(fVector6 - fVector5, Normal)) * Normal;
				FVector2 fVector8 = fVector6 - radiusB * Normal;
				Points[j] = 0.5f * (fVector7 + fVector8);
				Separations[j] = FVector2.Dot(fVector8 - fVector7, Normal);
			}
			break;
		}
		case ManifoldType.FaceB:
		{
			Normal = MathUtils.Mul(in xfB.Rotation, in manifold.LocalNormal);
			FVector2 fVector = MathUtils.Mul(in xfB, in manifold.LocalPoint);
			for (int i = 0; i < manifold.PointCount; i++)
			{
				FVector2 fVector2 = MathUtils.Mul(in xfA, in manifold.Points[i].LocalPoint);
				FVector2 fVector3 = fVector2 + (radiusB - FVector2.Dot(fVector2 - fVector, Normal)) * Normal;
				FVector2 fVector4 = fVector2 - radiusA * Normal;
				Points[i] = 0.5f * (fVector4 + fVector3);
				Separations[i] = FVector2.Dot(fVector4 - fVector3, Normal);
			}
			Normal = -Normal;
			break;
		}
		}
	}
}
