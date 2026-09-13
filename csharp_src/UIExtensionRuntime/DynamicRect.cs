using UnityEngine;

public class DynamicRect
{
	private Rect mRect;

	public int Index;

	public DynamicRect(float x, float y, float width, float height, int index)
	{
		Index = index;
		mRect = new Rect(x, y, width, height);
	}

	public bool Overlaps(DynamicRect otherRect)
	{
		return mRect.Overlaps(otherRect.mRect);
	}

	public bool Overlaps(Rect otherRect)
	{
		return mRect.Overlaps(otherRect);
	}

	public override string ToString()
	{
		return $"index:{Index},x:{mRect.x},y:{mRect.y},w:{mRect.width},h:{mRect.height}";
	}
}
