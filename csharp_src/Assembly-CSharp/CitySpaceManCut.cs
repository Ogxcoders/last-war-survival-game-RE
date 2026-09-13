using UnityEngine;

public class CitySpaceManCut : CitySpaceManState
{
	public CitySpaceManCut(CitySpaceMan spaceMan)
		: base(spaceMan)
	{
	}

	public override void OnEnter()
	{
		_spaceMan.ShowWeapon(show: true);
		_spaceMan.PlayAnim("dig");
	}

	public override void OnExit()
	{
		_spaceMan.ShowWeapon(show: false);
	}

	public override void OnUpdate(float deltaTime)
	{
		if (_spaceMan.Velocity != Vector3.zero)
		{
			_spaceMan.Rotation = Quaternion.LookRotation(_spaceMan.Velocity);
		}
	}
}
