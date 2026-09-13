public class CitySpaceManIdle : CitySpaceManState
{
	public CitySpaceManIdle(CitySpaceMan spaceMan)
		: base(spaceMan)
	{
	}

	public override void OnEnter()
	{
		_spaceMan.PlayAnim("dig_idle");
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate(float deltaTime)
	{
	}
}
