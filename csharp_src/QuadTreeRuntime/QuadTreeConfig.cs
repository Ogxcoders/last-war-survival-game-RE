public sealed class QuadTreeConfig
{
	public bool isDynamic = true;

	public int maxObjectsPerNode = 32;

	public float minmumQuad = 4f;

	public int initObjectsCount = 128;

	public float maxDataRadius = float.MaxValue;

	public float circleRadiusExtend;

	public int minmumAddDataDepthToRoot;

	public static readonly QuadTreeConfig @default = new QuadTreeConfig();
}
