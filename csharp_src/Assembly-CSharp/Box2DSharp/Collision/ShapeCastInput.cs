using Box2DSharp.Common;

namespace Box2DSharp.Collision;

public struct ShapeCastInput
{
	public DistanceProxy ProxyA;

	public DistanceProxy ProxyB;

	public Transform TransformA;

	public Transform TransformB;

	public FVector2 TranslationB;
}
