using System;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;
using Box2DSharp.Foreign;

namespace Box2DSharp.Collision.Shapes;

public class ChainShape : Shape
{
	public int Count;

	public FVector2 PrevVertex;

	public FVector2 NextVertex;

	public FVector2[] Vertices;

	public ChainShape()
	{
		base.ShapeType = ShapeType.Chain;
		base.Radius = Settings.PolygonRadius;
		Vertices = null;
		Count = 0;
	}

	public override Shape Clone()
	{
		ChainShape chainShape = new ChainShape
		{
			Vertices = new FVector2[Vertices.Length]
		};
		Array.Copy(Vertices, chainShape.Vertices, Vertices.Length);
		chainShape.Count = Count;
		chainShape.PrevVertex = PrevVertex;
		chainShape.NextVertex = NextVertex;
		return chainShape;
	}

	public void Clear()
	{
		Vertices = null;
		Count = 0;
	}

	public void CreateLoop(FVector2[] vertices, int count = -1)
	{
		if (count == -1)
		{
			count = vertices.Length;
		}
		if (count >= 3)
		{
			for (int i = 1; i < count; i++)
			{
				_ = ref vertices[i - 1];
				_ = ref vertices[i];
			}
			Count = count + 1;
			Vertices = new FVector2[Count];
			Array.Copy(vertices, Vertices, count);
			Vertices[count] = Vertices[0];
			PrevVertex = Vertices[Count - 2];
			NextVertex = Vertices[1];
		}
	}

	public void CreateChain(FVector2[] vertices, int count, FVector2 prevVertex, FVector2 nextVertex)
	{
		for (int i = 1; i < count; i++)
		{
		}
		Count = count;
		Vertices = new FVector2[count];
		Array.Copy(vertices, Vertices, count);
		PrevVertex = prevVertex;
		NextVertex = nextVertex;
	}

	public override int GetChildCount()
	{
		return Count - 1;
	}

	public void GetChildEdge(out EdgeShape edge, int index)
	{
		edge = new EdgeShape
		{
			ShapeType = ShapeType.Edge,
			Radius = base.Radius,
			Vertex1 = Vertices[index],
			Vertex2 = Vertices[index + 1],
			OneSided = true,
			Vertex0 = ((index > 0) ? Vertices[index - 1] : PrevVertex),
			Vertex3 = ((index < Count - 2) ? Vertices[index + 2] : NextVertex)
		};
	}

	public override bool TestPoint(in Transform transform, in FVector2 p)
	{
		return false;
	}

	public override bool RayCast(out RayCastOutput output, in RayCastInput input, in Transform transform, int childIndex)
	{
		EdgeShape edgeShape = new EdgeShape();
		int num = childIndex + 1;
		if (num == Count)
		{
			num = 0;
		}
		edgeShape.Vertex1 = Vertices[childIndex];
		edgeShape.Vertex2 = Vertices[num];
		return edgeShape.RayCast(out output, in input, in transform, 0);
	}

	public override void ComputeAABB(out AABB aabb, in Transform transform, int childIndex)
	{
		int num = childIndex + 1;
		if (num == Count)
		{
			num = 0;
		}
		FVector2 value = MathUtils.Mul(in transform, in Vertices[childIndex]);
		FVector2 value2 = MathUtils.Mul(in transform, in Vertices[num]);
		FVector2 fVector = FVector2.Min(value, value2);
		FVector2 fVector2 = FVector2.Max(value, value2);
		FVector2 fVector3 = new FVector2(base.Radius, base.Radius);
		aabb = new AABB(fVector - fVector3, fVector2 + fVector3);
	}

	public override void ComputeMass(out MassData massData, FP density)
	{
		massData = default(MassData);
		massData.Mass = 0f;
		massData.Center.SetZero();
		massData.RotationInertia = 0f;
	}

	public override PhysicsSnapShot.ComponentPhysicsShapeData TakeSnapShot()
	{
		return new PhysicsSnapShot.ComponentPhysicsChainShapeData
		{
			Vertices = (Vertices.Clone() as FVector2[]),
			Count = Count,
			NextVertex = NextVertex,
			PrevVertex = PrevVertex
		};
	}

	public override void RestoreSnapshot(PhysicsSnapShot.ComponentPhysicsShapeData shapeData)
	{
		if (shapeData is PhysicsSnapShot.ComponentPhysicsChainShapeData componentPhysicsChainShapeData)
		{
			Vertices = componentPhysicsChainShapeData.Vertices;
			NextVertex = componentPhysicsChainShapeData.NextVertex;
			PrevVertex = componentPhysicsChainShapeData.PrevVertex;
			Count = componentPhysicsChainShapeData.Count;
		}
	}
}
