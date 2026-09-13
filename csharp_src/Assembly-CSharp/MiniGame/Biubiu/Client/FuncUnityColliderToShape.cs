using System.Collections.Generic;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class FuncUnityColliderToShape
{
	public static Shape ColliderToShape(BoxCollider2D boxCollider2D)
	{
		float x = boxCollider2D.transform.lossyScale.x;
		FVector2 center = boxCollider2D.offset.ToFVector2() * x;
		FVector2 fVector = (boxCollider2D.size * 0.5f).ToFVector2() * x;
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.SetAsBox(fVector.X, fVector.Y, in center, 0);
		return polygonShape;
	}

	public static Shape ColliderToShape(EdgeCollider2D edgeCollider2D)
	{
		Vector2[] points = edgeCollider2D.points;
		List<FVector2> list = new List<FVector2>();
		Vector2[] array = points;
		for (int i = 0; i < array.Length; i++)
		{
			Vector2 vector = array[i];
			list.Add(vector.ToFVector2() * edgeCollider2D.transform.lossyScale.x);
		}
		PolygonShape polygonShape = new PolygonShape();
		polygonShape.Set(list.ToArray(), list.Count);
		return polygonShape;
	}

	public static Shape ColliderToShape(CircleCollider2D circleCollider2D)
	{
		return new CircleShape
		{
			Radius = circleCollider2D.radius,
			Position = circleCollider2D.offset.ToFVector2()
		};
	}
}
