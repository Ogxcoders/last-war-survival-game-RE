using UnityEngine;

public class DynamicAtlasIntegerRectangle
{
	public int x;

	public int y;

	public int width;

	public int height;

	public int right => x + width;

	public int top => y + height;

	public int size => width * height;

	public Rect rect => new Rect(x, y, width, height);

	public DynamicAtlasIntegerRectangle(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	public override string ToString()
	{
		return $"x{x}_y:{y}_width:{width}_height{height}_top:{top}_right{right}";
	}
}
