namespace Box2DSharp.Dynamics;

public sealed class DefaultContactFilter : IContactFilter
{
	public bool ShouldCollide(Fixture fixtureA, Fixture fixtureB)
	{
		return DoShouldCollide(fixtureA, fixtureB);
	}

	public static bool DoShouldCollide(Fixture fixtureA, Fixture fixtureB)
	{
		Filter filter = fixtureA.Filter;
		Filter filter2 = fixtureB.Filter;
		if (filter.GroupIndex == filter2.GroupIndex && filter.GroupIndex != 0)
		{
			return filter.GroupIndex > 0;
		}
		if ((filter.MaskBits & filter2.CategoryBits) != 0)
		{
			return (filter.CategoryBits & filter2.MaskBits) != 0;
		}
		return false;
	}
}
