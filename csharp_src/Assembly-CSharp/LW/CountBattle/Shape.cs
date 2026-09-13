using UnityEngine;

namespace LW.CountBattle;

public abstract class Shape
{
	public Vector2 pos;

	public float angle;

	public abstract Vector2 World2Local(Vector2 worldPoint);

	public abstract Vector2 Local2World(Vector2 worldPoint);

	public abstract bool Contains(Vector2 point);

	public abstract bool Overlap(Shape other);

	public Shape(Vector2 pos, float angle)
	{
		this.pos = pos;
		this.angle = angle;
	}

	public void SetPos(float x, float y)
	{
		pos.x = x;
		pos.y = y;
	}

	public void Get_pos(out float x, out float y)
	{
		x = pos.x;
		y = pos.y;
	}

	public void Set_pos(float x, float y)
	{
		pos.x = x;
		pos.y = y;
	}
}
