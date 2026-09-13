using System;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;

namespace Box2DSharp.Foreign;

public static class PhysicsSnapShot
{
	public interface ComponentPhysicsShapeData
	{
		Type Type { get; }
	}

	public class ComponentPhysicsChainShapeData : ComponentPhysicsShapeData
	{
		public int Count;

		public FVector2[] Vertices;

		public FVector2 PrevVertex;

		public FVector2 NextVertex;

		public Type Type => typeof(ChainShape);
	}

	public class ComponentPhysicsPolygonData : ComponentPhysicsShapeData
	{
		public int Count;

		public FVector2 BoxCenter;

		public FVector2[] Normals;

		public FVector2[] Vertices;

		public Type Type => typeof(PolygonShape);
	}

	public class ComponentPhysicsCircleData : ComponentPhysicsShapeData
	{
		public FP Radius;

		public FVector2 Position;

		public Type Type => typeof(CircleShape);
	}

	public class ComponentPhysicsFixtureDefData
	{
		public FP Density;

		public FP Friction;

		public FP Restitution;

		public FP RestitutionThreshold;

		public short GroupIndex;

		public ushort CategoryBits;

		public ushort MaskBits;

		public object UserData;
	}

	public class ComponentPhysicsBodyDefData
	{
		public BodyType BodyType;

		public FVector2 Position;

		public FP Rotation;

		public FVector2 Velocity;

		public FP GravityScale;

		public BodyFlags Flags;

		public FP AngularDamping;

		public FP Inertia;

		public FP LinearDamping;

		public FP Mass;

		public FP InverseInertia;

		public FP InvMass;

		public FP Torque;

		public FVector2 Force;

		public int IslandIndex;

		public FVector2 LocalCenter;

		public FP Alpha0;

		public object UserData;
	}

	public class ComponentPhysicsSnapshotData
	{
		public ComponentPhysicsBodyDefData BodyDefData;

		public ComponentPhysicsFixtureDefData[] FixtureDef;

		public ComponentPhysicsShapeData[] ShapeData;
	}

	public static Body CreateRestoreSnapshotBody(World world)
	{
		return world.CreateBody(default(BodyDef));
	}
}
