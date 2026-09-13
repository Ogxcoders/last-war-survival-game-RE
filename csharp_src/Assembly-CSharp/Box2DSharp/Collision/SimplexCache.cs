using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct SimplexCache
{
	public FP Metric;

	public ushort Count;

	public FixedArray3<byte> IndexA;

	public FixedArray3<byte> IndexB;
}
