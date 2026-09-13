using UnityEngine;

namespace LW.CountBattle;

public class Circle : Shape
{
	public float radius;

	public Circle(Vector2 pos, float angle, float radius)
		: base(pos, angle)
	{
		this.radius = radius;
	}

	public override Vector2 World2Local(Vector2 worldPoint)
	{
		return worldPoint - pos;
	}

	public override Vector2 Local2World(Vector2 localPoint)
	{
		return localPoint + pos;
	}

	public override bool Contains(Vector2 point)
	{
		return (point - pos).sqrMagnitude < radius * radius;
	}

	public override bool Overlap(Shape other)
	{
		if (other is Circle)
		{
			Circle circle = other as Circle;
			return (circle.pos - pos).sqrMagnitude < (circle.radius + radius) * (circle.radius + radius);
		}
		if (other is Box)
		{
			Box box = other as Box;
			Vector2 vector = box.World2Local(pos);
			float x = vector.x;
			float y = vector.y;
			if (Mathf.Abs(x) - box.size.x * 0.5f >= radius)
			{
				return false;
			}
			if (Mathf.Abs(y) - box.size.y * 0.5f >= radius)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		return string.Concat("Circle: pos=", pos, ", angle=", angle, ", radius=", radius);
	}
}
