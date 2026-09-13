using UnityEngine;

namespace Collider2D;

public struct FastCapsule2D
{
	public Vector2 StartPoint;

	public Vector2 EndPoint;

	public Vector2 Segment;

	public float SegmentSqrMagnitude;

	public float Radius;

	public Vector2 Center;

	public void Set(Vector2 startPoint, Vector2 endPoint, float radius)
	{
		StartPoint = startPoint;
		EndPoint = endPoint;
		Segment = startPoint - endPoint;
		Center = StartPoint * 0.5f + EndPoint * 0.5f;
		SegmentSqrMagnitude = Segment.x * Segment.x + Segment.y * Segment.y;
		Radius = radius;
	}
}
