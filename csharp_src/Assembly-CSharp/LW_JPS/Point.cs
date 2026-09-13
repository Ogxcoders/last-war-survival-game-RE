namespace LW_JPS;

public class Point
{
	public int x;

	public int y;

	public float F;

	public float G;

	public float H;

	public Point parent;

	public Point(int _x, int _y)
	{
		x = _x;
		y = _y;
		F = 0f;
		G = 0f;
		H = 0f;
	}
}
