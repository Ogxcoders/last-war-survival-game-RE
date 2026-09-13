using System.Runtime.CompilerServices;
using Box2DSharp.Collision;
using Box2DSharp.Collision.Collider;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;

namespace Box2DSharp.Dynamics.Contacts;

public class ChainAndCircleContact : Contact
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal override void Evaluate(ref Manifold manifold, in Transform xfA, Transform xfB)
	{
		((ChainShape)FixtureA.Shape).GetChildEdge(out var edge, ChildIndexA);
		CollisionUtils.CollideEdgeAndCircle(ref manifold, edge, in xfA, (CircleShape)FixtureB.Shape, in xfB);
	}
}
