using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public class EdgeAndCircleContact : Contact
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal override void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB)
	{
		CollisionUtils.CollideEdgeAndCircle(ref manifold, (EdgeShape)FixtureA.Shape, in xfA, (CircleShape)FixtureB.Shape, in xfB);
	}
}
