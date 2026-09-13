using Box2DSharp.Collision.Collider;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Internal;

public interface ITreeRayCastCallback
{
	FP RayCastCallback(in RayCastInput input, int proxyId);
}
