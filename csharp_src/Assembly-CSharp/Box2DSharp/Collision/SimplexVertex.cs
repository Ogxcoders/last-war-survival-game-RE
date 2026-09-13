using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct SimplexVertex
{
	public FVector2 Wa;

	public FVector2 Wb;

	public FVector2 W;

	public FP A;

	public int IndexA;

	public int IndexB;
}
