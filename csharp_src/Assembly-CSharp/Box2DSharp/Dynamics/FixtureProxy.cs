using Box2DSharp.Collision;

namespace Box2DSharp.Dynamics;

public class FixtureProxy
{
	public Box2DSharp.Collision.AABB AABB;

	public int ChildIndex;

	public Fixture Fixture;

	public int ProxyId = -1;
}
