using UnityEngine;

public class InfinityRect
{
	private Rect rect;

	private int index;

	public int Index => index;

	public float RectX
	{
		get
		{
			return rect.x;
		}
		set
		{
			rect.x = value;
		}
	}

	public float RectY
	{
		get
		{
			return rect.y;
		}
		set
		{
			rect.y = value;
		}
	}

	public float RectWidth
	{
		get
		{
			return rect.width;
		}
		set
		{
			rect.width = value;
		}
	}

	public float RectHeight
	{
		get
		{
			return rect.height;
		}
		set
		{
			rect.height = value;
		}
	}

	public InfinityRect(float x, float y, float width, float height, int index)
	{
		this.index = index;
		rect = new Rect(x, y, width, height);
	}

	public bool Overlaps(Rect otherRect)
	{
		return rect.Overlaps(otherRect);
	}
}
