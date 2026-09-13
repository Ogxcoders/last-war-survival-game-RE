using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct ShapeCastOutput
{
	public FVector2 Point;

	public FVector2 Normal;

	public FP Lambda;

	public int Iterations;
}
