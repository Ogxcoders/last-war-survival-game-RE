using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Box2DSharp.Collision;
using Box2DSharp.Common;
using Box2DSharp.Testbed.Unity.Inspection;
using MiniGame;
using MiniGame.Core;
using UnityEngine;

namespace Box2DSharp;

public class DebugDraw : IDebugDraw, IDraw
{
	public UnityDraw Draw;

	public bool ShowUI = true;

	public readonly ConcurrentQueue<(FloatVector2 Position, string Text)> Texts = new ConcurrentQueue<(FloatVector2, string)>();

	public DrawFlag Flags { get; set; }

	public void DrawPolygon(Span<FVector2> vertices, int vertexCount, in Box2DSharp.Common.Color color)
	{
		Span<Vector2> vertices2 = new Vector2[vertexCount];
		for (int i = 0; i < vertexCount; i++)
		{
			vertices2[i] = vertices[i].ToUnityVector2();
		}
		DrawPolygon(vertices2, vertexCount, in color);
	}

	public void DrawSolidPolygon(Span<FVector2> vertices, int vertexCount, in Box2DSharp.Common.Color color)
	{
		Span<Vector2> vertices2 = new Vector2[vertexCount];
		for (int i = 0; i < vertexCount; i++)
		{
			vertices2[i] = vertices[i].ToUnityVector2();
		}
		DrawSolidPolygon(vertices2, vertexCount, in color);
	}

	public void DrawCircle(in FVector2 center, FP radius, in Box2DSharp.Common.Color color)
	{
		DrawCircle(center.ToCSharpVector2(), (float)radius, in color);
	}

	public void DrawSolidCircle(in FVector2 center, FP radius, in FVector2 axis, in Box2DSharp.Common.Color color)
	{
		DrawSolidCircle(center.ToUnityVector2(), (float)radius, axis.ToUnityVector2(), in color);
	}

	public void DrawSegment(in FVector2 p1, in FVector2 p2, in Box2DSharp.Common.Color color)
	{
		DrawSegment(p1.ToUnityVector2(), p2.ToUnityVector2(), in color);
	}

	public void DrawPoint(in FVector2 p, FP size, in Box2DSharp.Common.Color color)
	{
		DrawPoint(p.ToUnityVector2(), size.AsFloat, in color);
	}

	public void DrawPolygon(Span<Vector2> vertices, int vertexCount, in Box2DSharp.Common.Color color)
	{
		List<(Vector3, Vector3)> list = new List<(Vector3, Vector3)>();
		for (int i = 0; i < vertexCount; i++)
		{
			if (i < vertexCount - 1)
			{
				list.Add((vertices[i].ToUnityVector3(), vertices[i + 1].ToUnityVector3()));
			}
			else
			{
				list.Add((vertices[i].ToUnityVector3(), vertices[0].ToUnityVector3()));
			}
		}
		Draw.PostLines(list, color.ToUnityColor());
	}

	public void DrawSolidPolygon(Span<Vector2> vertices, int vertexCount, in Box2DSharp.Common.Color color)
	{
		List<(Vector3, Vector3)> list = new List<(Vector3, Vector3)>();
		for (int i = 0; i < vertexCount; i++)
		{
			if (i < vertexCount - 1)
			{
				list.Add((vertices[i].ToUnityVector3(), vertices[i + 1].ToUnityVector3()));
			}
			else
			{
				list.Add((vertices[i].ToUnityVector3(), vertices[0].ToUnityVector3()));
			}
		}
		Draw.PostLines(list, color.ToUnityColor());
	}

	public void DrawCircle(in FloatVector2 center, float radius, in Box2DSharp.Common.Color color)
	{
		List<(Vector3, Vector3)> list = new List<(Vector3, Vector3)>();
		for (int i = 0; i <= 100; i++)
		{
			list.Add((new Vector2(center.X + radius * (float)Math.Cos(MathF.PI / 50f * (float)i), center.Y + radius * (float)Math.Sin(MathF.PI / 50f * (float)i)), new Vector2(center.X + radius * (float)Math.Cos(MathF.PI / 50f * (float)(i + 1)), center.Y + radius * (float)Math.Sin(MathF.PI / 50f * (float)(i + 1)))));
		}
		Draw.PostLines(list, color.ToUnityColor());
	}

	public void DrawSolidCircle(in Vector2 center, float radius, in Vector2 axis, in Box2DSharp.Common.Color color)
	{
		List<(Vector3, Vector3)> list = new List<(Vector3, Vector3)>();
		for (int i = 0; i <= 100; i++)
		{
			list.Add((new Vector2(center.x + radius * (float)Math.Cos(MathF.PI / 50f * (float)i), center.y + radius * (float)Math.Sin(MathF.PI / 50f * (float)i)), new Vector2(center.x + radius * (float)Math.Cos(MathF.PI / 50f * (float)(i + 1)), center.y + radius * (float)Math.Sin(MathF.PI / 50f * (float)(i + 1)))));
		}
		Draw.PostLines(list, color.ToUnityColor());
		DrawSegment(in center, center + radius * axis, in color);
	}

	public void DrawSegment(in Vector2 p1, in Vector2 p2, in Box2DSharp.Common.Color color)
	{
		Draw.PostLines(new List<(Vector3, Vector3)> { (p1.ToUnityVector3(), p2.ToUnityVector3()) }, color.ToUnityColor());
	}

	public void DrawTransform(in Box2DSharp.Common.Transform xf)
	{
		FVector2 position = xf.Position;
		FVector2 vector = position + 0.4f * xf.Rotation.GetXAxis();
		Draw.PostLines(new List<(Vector3, Vector3)> { (position.ToUnityVector2(), vector.ToUnityVector2()) }, UnityEngine.Color.red);
		vector = position + 0.4f * xf.Rotation.GetYAxis();
		Draw.PostLines(new List<(Vector3, Vector3)> { (position.ToUnityVector2(), vector.ToUnityVector2()) }, UnityEngine.Color.green);
	}

	public void DrawPoint(in Vector2 p, float size, in Box2DSharp.Common.Color color)
	{
		Draw.PostPoint((Center: p, Radius: size / 100f, color: color.ToUnityColor()));
	}

	public void DrawAABB(Box2DSharp.Collision.AABB aabb, Box2DSharp.Common.Color color)
	{
		Vector2 vector = aabb.LowerBound.ToUnityVector2();
		Vector2 vector2 = new Vector2(aabb.UpperBound.X.AsFloat, aabb.LowerBound.Y.AsFloat);
		Vector2 vector3 = aabb.UpperBound.ToUnityVector2();
		Vector2 vector4 = new Vector2(aabb.LowerBound.X.AsFloat, aabb.UpperBound.Y.AsFloat);
		DrawPolygon(new Vector2[4] { vector, vector2, vector3, vector4 }, 4, in color);
	}

	public void DrawString(float x, float y, string strings)
	{
		Texts.Enqueue((new FloatVector2(x, y), strings));
	}

	public void DrawString(int x, int y, string strings)
	{
		Texts.Enqueue((new FloatVector2(x, y), strings));
	}

	public void DrawString(FloatVector2 position, string strings)
	{
		Texts.Enqueue((position, strings));
	}

	void IDraw.DrawPolygon(Span<FVector2> vertices, int vertexCount, in Box2DSharp.Common.Color color)
	{
		DrawPolygon(vertices, vertexCount, in color);
	}

	void IDraw.DrawSolidPolygon(Span<FVector2> vertices, int vertexCount, in Box2DSharp.Common.Color color)
	{
		DrawSolidPolygon(vertices, vertexCount, in color);
	}

	void IDraw.DrawCircle(in FVector2 center, FP radius, in Box2DSharp.Common.Color color)
	{
		DrawCircle(in center, radius, in color);
	}

	void IDraw.DrawSolidCircle(in FVector2 center, FP radius, in FVector2 axis, in Box2DSharp.Common.Color color)
	{
		DrawSolidCircle(in center, radius, in axis, in color);
	}

	void IDraw.DrawSegment(in FVector2 p1, in FVector2 p2, in Box2DSharp.Common.Color color)
	{
		DrawSegment(in p1, in p2, in color);
	}

	void IDraw.DrawTransform(in Box2DSharp.Common.Transform xf)
	{
		DrawTransform(in xf);
	}

	void IDraw.DrawPoint(in FVector2 p, FP size, in Box2DSharp.Common.Color color)
	{
		DrawPoint(in p, size, in color);
	}
}
