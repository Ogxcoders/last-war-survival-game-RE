using Box2DSharp.Common;

namespace Box2DSharp.Dynamics;

public interface IRayCastCallback
{
	FP RayCastCallback(Fixture fixture, in FVector2 point, in FVector2 normal, FP fraction);
}
