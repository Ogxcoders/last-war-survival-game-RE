using UnityEngine;

public class CitySpaceManWalk : CitySpaceManState
{
	private Quaternion _targetQuaternion;

	public CitySpaceManWalk(CitySpaceMan spaceMan)
		: base(spaceMan)
	{
	}

	public override void OnEnter()
	{
		_spaceMan.PlayAnim("dig_run");
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate(float deltaTime)
	{
		if (_spaceMan.Velocity != Vector3.zero)
		{
			Debug.Log($">>speed: {_spaceMan.Velocity}");
			Quaternion quaternion = Quaternion.LookRotation(_spaceMan.Velocity, Vector3.up);
			if (quaternion != _spaceMan.Rotation)
			{
				_spaceMan.Rotation = Quaternion.Lerp(_spaceMan._tranform.rotation, quaternion, Time.fixedDeltaTime * 10f);
			}
		}
	}
}
