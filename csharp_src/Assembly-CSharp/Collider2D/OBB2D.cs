using UnityEngine;

namespace Collider2D;

public struct OBB2D
{
	public Vector2 _axisX;

	public Vector2 _axisY;

	public Vector2 _center;

	public Vector2 _extend;

	public Vector2 _verLB;

	public Vector2 _verLT;

	public Vector2 _verRT;

	public Vector2 _verRB;

	public void SetAxis(Vector2 axisX, Vector2 axisY)
	{
		_axisX = axisX;
		_axisY = axisY;
		_verRT = _center + (axisX * _extend.x + axisY * _extend.y);
		_verRB = _center + (axisX * _extend.x - axisY * _extend.y);
		_verLB = _center + (-axisX * _extend.x - axisY * _extend.y);
		_verLT = _center + (-axisX * _extend.x + axisY * _extend.y);
	}

	public void SetBound(Vector2 center, Vector2 extend)
	{
		_center = center;
		_extend = extend;
	}

	public bool IsIntersect2D(OBB2D b)
	{
		return IntersectDetect2D.OBB2DIntersectOBB2D(ref this, ref b);
	}

	public Vector2 GetClosePoint(Vector2 point)
	{
		return Vector2.zero;
	}
}
