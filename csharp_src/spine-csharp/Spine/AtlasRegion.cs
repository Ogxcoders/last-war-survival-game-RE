namespace Spine;

public class AtlasRegion
{
	public AtlasPage page;

	public string name;

	public int x;

	public int y;

	public int width;

	public int height;

	public float u;

	public float v;

	public float u2;

	public float v2;

	public float offsetX;

	public float offsetY;

	public int originalWidth;

	public int originalHeight;

	public int degrees;

	public bool rotate;

	public int index;

	public string[] names;

	public int[][] values;

	public AtlasRegion Clone()
	{
		return MemberwiseClone() as AtlasRegion;
	}
}
