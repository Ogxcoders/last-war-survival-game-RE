public abstract class CitySpaceManState
{
	protected CitySpaceMan _spaceMan;

	public CitySpaceManState(CitySpaceMan spaceMan)
	{
		_spaceMan = spaceMan;
	}

	public abstract void OnEnter();

	public abstract void OnExit();

	public abstract void OnUpdate(float deltaTime);
}
