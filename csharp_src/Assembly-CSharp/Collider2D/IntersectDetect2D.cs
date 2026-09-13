using System;
using UnityEngine;

namespace Collider2D;

public static class IntersectDetect2D
{
	public static bool OBB2DIntersectOBB2D(ref OBB2D a, ref OBB2D b)
	{
		if (!IsNotOBB2DIntersectInAxis(ref a, ref b, a._axisX) && !IsNotOBB2DIntersectInAxis(ref a, ref b, a._axisY) && !IsNotOBB2DIntersectInAxis(ref a, ref b, b._axisX))
		{
			return !IsNotOBB2DIntersectInAxis(ref a, ref b, b._axisY);
		}
		return false;
	}

	private static bool IsNotOBB2DIntersectInAxis(ref OBB2D a, ref OBB2D b, Vector2 axis)
	{
		OBB2DVertexProject(ref a, axis, out var minValue, out var maxValue);
		OBB2DVertexProject(ref b, axis, out var minValue2, out var maxValue2);
		if (!(minValue > maxValue2))
		{
			return minValue2 > maxValue;
		}
		return true;
	}

	private static void OBB2DVertexProject(ref OBB2D obb, Vector2 axis, out float minValue, out float maxValue)
	{
		minValue = float.MinValue;
		maxValue = float.MaxValue;
		float b = Vector2.Dot(obb._verLB, axis);
		minValue = Mathf.Min(minValue, b);
		maxValue = Mathf.Max(maxValue, b);
		b = Vector2.Dot(obb._verLT, axis);
		minValue = Mathf.Min(minValue, b);
		maxValue = Mathf.Max(maxValue, b);
		b = Vector2.Dot(obb._verRT, axis);
		minValue = Mathf.Min(minValue, b);
		maxValue = Mathf.Max(maxValue, b);
		b = Vector2.Dot(obb._verRB, axis);
		minValue = Mathf.Min(minValue, b);
		maxValue = Mathf.Max(maxValue, b);
	}

	public static bool AABBIntersectSegment2D(float aabbMinX, float aabbMinY, float aabbMaxX, float aabbMaxY, Vector2 segmentStartPoint, Vector2 segmentEndPoint)
	{
		float num = segmentEndPoint.x - segmentStartPoint.x;
		float num2 = segmentEndPoint.y - segmentStartPoint.y;
		float num3 = Math.Abs(num);
		float num4 = Math.Abs(num2);
		if (num3 < float.Epsilon)
		{
			if (segmentStartPoint.x < aabbMinX || segmentStartPoint.x > aabbMaxX)
			{
				return false;
			}
		}
		else
		{
			float num5 = 1f / num;
			float num6 = (aabbMinX - segmentStartPoint.x) * num5;
			float num7 = (aabbMaxX - segmentStartPoint.x) * num5;
			if (num6 > num7)
			{
				float num8 = num6;
				num6 = num7;
				num7 = num8;
			}
			float num9 = ((num6 < 0f) ? 0f : num6);
			float num10 = ((num7 > 1f) ? 1f : num7);
			if (num9 > num10)
			{
				return false;
			}
		}
		if (num4 < float.Epsilon)
		{
			if (segmentStartPoint.y < aabbMinY || segmentStartPoint.y > aabbMaxY)
			{
				return false;
			}
		}
		else
		{
			float num11 = 1f / num2;
			float num12 = (aabbMinY - segmentStartPoint.y) * num11;
			float num13 = (aabbMaxY - segmentStartPoint.y) * num11;
			if (num12 > num13)
			{
				float num14 = num12;
				num12 = num13;
				num13 = num14;
			}
			float num15 = ((num12 < 0f) ? 0f : num12);
			float num16 = ((num13 > 1f) ? 1f : num13);
			if (num15 > num16)
			{
				return false;
			}
		}
		return true;
	}

	public static bool AABBIntersectCircle2D(float aabbMinX, float aabbMinY, float aabbMaxX, float aabbMaxY, float circleCenterX, float circleCenterY, float circleRadius)
	{
		float num = circleCenterX;
		if (aabbMaxX < num)
		{
			num = aabbMaxX;
		}
		if (aabbMinX > num)
		{
			num = aabbMinX;
		}
		float num2 = circleCenterY;
		if (aabbMaxY < num2)
		{
			num2 = aabbMaxY;
		}
		if (aabbMinY > num2)
		{
			num2 = aabbMinY;
		}
		float num3 = circleCenterX - num;
		float num4 = circleCenterY - num2;
		return num3 * num3 + num4 * num4 < circleRadius * circleRadius;
	}

	public static bool AABBIntersectAABB2D(float aAABBMinX, float aAABBMinY, float aAABBMaxX, float aAABBMaxY, float bAABBMinX, float bAABBMinY, float bAABBMaxX, float bAABBMaxY)
	{
		if (aAABBMaxX < bAABBMinX || aAABBMinX > bAABBMaxX)
		{
			return false;
		}
		if (aAABBMaxY < bAABBMinY || aAABBMinY > bAABBMaxY)
		{
			return false;
		}
		return true;
	}

	public static bool FastCapsule2DIntersectCircle2D(float capsuleEndPointX, float capsuleEndPointY, float capsuleStartPointX, float capsuleStartPointY, float capsuleRadius, float capsuleSegmentSqrLength, float capsuleSegmentX, float capsuleSegmentY, float circleCenterX, float circleCenterY, float radius)
	{
		float num = circleCenterX - capsuleEndPointX;
		float num2 = circleCenterY - capsuleEndPointY;
		float num3 = radius + capsuleRadius;
		float num4 = num3 * num3;
		if (capsuleSegmentSqrLength <= float.Epsilon)
		{
			return num * num + num2 * num2 < num4;
		}
		float num5 = 0f;
		float num6 = num * capsuleSegmentX + num2 * capsuleSegmentY;
		if (num6 > 0f)
		{
			num5 = num * num + num2 * num2;
		}
		else if (num6 + capsuleSegmentSqrLength < 0f)
		{
			float num7 = circleCenterX - capsuleStartPointX;
			float num8 = circleCenterY - capsuleStartPointY;
			num5 = num7 * num7 + num8 * num8;
		}
		else
		{
			float num9 = num6 / capsuleSegmentSqrLength;
			float num10 = capsuleEndPointX + capsuleSegmentX * num9;
			float num11 = capsuleEndPointY + capsuleSegmentY * num9;
			float num12 = circleCenterX - num10;
			float num13 = circleCenterY - num11;
			num5 = num12 * num12 + num13 * num13;
		}
		return num5 < num4;
	}

	public static bool Circle2DIntersectCircle2D(float circleACenterX, float circleACenterY, float circleARadius, float circleBCenterX, float circleBCenterY, float circleBRadius)
	{
		float num = circleARadius + circleBRadius;
		float num2 = circleACenterX - circleBCenterX;
		float num3 = circleACenterY - circleBCenterY;
		return num2 * num2 + num3 * num3 <= num * num;
	}
}
