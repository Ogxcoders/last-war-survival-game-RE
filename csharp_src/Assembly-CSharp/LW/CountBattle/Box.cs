using UnityEngine;

namespace LW.CountBattle;

public class Box : Shape
{
	public Vector2 size;

	public Box(Vector2 pos, float angle, Vector2 size)
		: base(pos, angle)
	{
		this.size = size;
	}

	public void Get_size(out float x, out float y)
	{
		x = size.x;
		y = size.y;
	}

	public void Set_size(float x, float y)
	{
		size.x = x;
		size.y = y;
	}

	public Vector2[] GetLocalCorners()
	{
		return new Vector2[4]
		{
			new Vector2((0f - size.x) * 0.5f, (0f - size.y) * 0.5f),
			new Vector2(size.x * 0.5f, (0f - size.y) * 0.5f),
			new Vector2(size.x * 0.5f, size.y * 0.5f),
			new Vector2((0f - size.x) * 0.5f, size.y * 0.5f)
		};
	}

	public Vector2[] GetWorldCorners()
	{
		Vector2[] localCorners = GetLocalCorners();
		for (int i = 0; i < localCorners.Length; i++)
		{
			localCorners[i] = Local2World(localCorners[i]);
		}
		return localCorners;
	}

	public override Vector2 World2Local(Vector2 worldPoint)
	{
		Vector3 vector = new Vector3(worldPoint.x, 0f, worldPoint.y);
		Vector3 vector2 = Quaternion.Euler(0f, 0f - angle, 0f) * (vector - new Vector3(pos.x, 0f, pos.y));
		return new Vector2(vector2.x, vector2.z);
	}

	public override Vector2 Local2World(Vector2 localPoint)
	{
		Vector3 vector = new Vector3(localPoint.x, 0f, localPoint.y);
		Vector3 vector2 = Quaternion.Euler(0f, angle, 0f) * vector + new Vector3(pos.x, 0f, pos.y);
		return new Vector2(vector2.x, vector2.z);
	}

	public override bool Contains(Vector2 point)
	{
		Vector2 vector = World2Local(point);
		float x = vector.x;
		float y = vector.y;
		if (Mathf.Abs(x) < size.x * 0.5f)
		{
			return Mathf.Abs(y) < size.y * 0.5f;
		}
		return false;
	}

	public override bool Overlap(Shape other)
	{
		if (other is Circle)
		{
			return (other as Circle).Overlap(this);
		}
		if (other is Box)
		{
			Box obj = other as Box;
			Vector2[] worldCorners = GetWorldCorners();
			Vector2[] worldCorners2 = obj.GetWorldCorners();
			Vector2 axis = worldCorners[1] - worldCorners[0];
			if (!OverlapOnAxis(worldCorners, worldCorners2, axis))
			{
				return false;
			}
			Vector2 axis2 = worldCorners[3] - worldCorners[0];
			if (!OverlapOnAxis(worldCorners, worldCorners2, axis2))
			{
				return false;
			}
			Vector2 axis3 = worldCorners2[1] - worldCorners2[0];
			if (!OverlapOnAxis(worldCorners, worldCorners2, axis3))
			{
				return false;
			}
			Vector2 axis4 = worldCorners2[3] - worldCorners2[0];
			if (!OverlapOnAxis(worldCorners, worldCorners2, axis4))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private bool OverlapOnAxis(Vector2[] worldCornersA, Vector2[] worldCornersB, Vector2 axis)
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		for (int i = 0; i < worldCornersA.Length; i++)
		{
			float num5 = Vector2.Dot(worldCornersA[i], axis);
			if (num5 < num)
			{
				num = num5;
			}
			if (num5 > num2)
			{
				num2 = num5;
			}
		}
		for (int j = 0; j < worldCornersB.Length; j++)
		{
			float num6 = Vector2.Dot(worldCornersB[j], axis);
			if (num6 < num3)
			{
				num3 = num6;
			}
			if (num6 > num4)
			{
				num4 = num6;
			}
		}
		if (num > num4)
		{
			return false;
		}
		if (num2 < num3)
		{
			return false;
		}
		return true;
	}

	public override string ToString()
	{
		return string.Concat("Box: pos=", pos, ", angle=", angle, ", size=", size);
	}
}
