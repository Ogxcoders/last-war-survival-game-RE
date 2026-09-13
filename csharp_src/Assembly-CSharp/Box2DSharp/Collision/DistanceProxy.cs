using System;
using System.Runtime.CompilerServices;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct DistanceProxy
{
	public FVector2[] Vertices;

	public int Count;

	public FP Radius;

	public void Set(Shape shape, int index)
	{
		if (shape != null)
		{
			if (shape is CircleShape circleShape)
			{
				CircleShape circleShape2 = circleShape;
				Vertices = new FVector2[1] { circleShape2.Position };
				Count = 1;
				Radius = circleShape2.Radius;
				return;
			}
			if (shape is PolygonShape polygonShape)
			{
				PolygonShape polygonShape2 = polygonShape;
				Vertices = polygonShape2.Vertices;
				Count = polygonShape2.Count;
				Radius = polygonShape2.Radius;
				return;
			}
			if (shape is ChainShape chainShape)
			{
				ChainShape chainShape2 = chainShape;
				Count = 2;
				Vertices = new FVector2[Count];
				Vertices[0] = chainShape2.Vertices[index];
				if (index + 1 < chainShape2.Count)
				{
					Vertices[1] = chainShape2.Vertices[index + 1];
				}
				else
				{
					Vertices[1] = chainShape2.Vertices[0];
				}
				Radius = chainShape2.Radius;
				return;
			}
			if (shape is EdgeShape edgeShape)
			{
				EdgeShape edgeShape2 = edgeShape;
				Vertices = new FVector2[2] { edgeShape2.Vertex1, edgeShape2.Vertex2 };
				Count = 2;
				Radius = edgeShape2.Radius;
				return;
			}
		}
		throw new NotSupportedException();
	}

	public void Set(FVector2[] vertices, int count, FP radius)
	{
		Vertices = new FVector2[vertices.Length];
		Array.Copy(vertices, Vertices, vertices.Length);
		Count = count;
		Radius = radius;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int GetSupport(in FVector2 d)
	{
		int result = 0;
		FP fP = FVector2.Dot(Vertices[0], d);
		for (int i = 1; i < Count; i++)
		{
			FP fP2 = FVector2.Dot(Vertices[i], d);
			if (fP2 > fP)
			{
				result = i;
				fP = fP2;
			}
		}
		return result;
	}

	public ref readonly FVector2 GetSupportVertex(in FVector2 d)
	{
		int num = 0;
		FP fP = FVector2.Dot(Vertices[0], d);
		for (int i = 1; i < Count; i++)
		{
			FP fP2 = FVector2.Dot(Vertices[i], d);
			if (fP2 > fP)
			{
				num = i;
				fP = fP2;
			}
		}
		return ref Vertices[num];
	}

	public int GetVertexCount()
	{
		return Count;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref readonly FVector2 GetVertex(int index)
	{
		return ref Vertices[index];
	}
}
