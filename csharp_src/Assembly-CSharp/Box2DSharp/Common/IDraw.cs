using System;

namespace Box2DSharp.Common;

public interface IDraw
{
	DrawFlag Flags { get; set; }

	void DrawPolygon(Span<FVector2> vertices, int vertexCount, in Color color);

	void DrawSolidPolygon(Span<FVector2> vertices, int vertexCount, in Color color);

	void DrawCircle(in FVector2 center, FP radius, in Color color);

	void DrawSolidCircle(in FVector2 center, FP radius, in FVector2 axis, in Color color);

	void DrawSegment(in FVector2 p1, in FVector2 p2, in Color color);

	void DrawTransform(in Transform xf);

	void DrawPoint(in FVector2 p, FP size, in Color color);
}
