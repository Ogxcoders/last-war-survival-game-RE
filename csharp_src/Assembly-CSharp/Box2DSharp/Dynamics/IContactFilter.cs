namespace Box2DSharp.Dynamics;

public interface IContactFilter
{
	bool ShouldCollide(Fixture fixtureA, Fixture fixtureB);
}
