using UnityEngine;

public class CityTroopStatePickGarbageMovetoGarbage : BaseCityTroopState
{
	private float startTime;

	private const float totalTime = 2f;

	public CityTroopStatePickGarbageMovetoGarbage(CityTroop cityTroop, CityTroopMachine stateMachine)
		: base(cityTroop, stateMachine)
	{
	}

	public override void OnEnter()
	{
		cityTroop.PlayAnim("idle");
		cityTroop.TroopUnitsBirthThenPickGarbage();
		startTime = Time.realtimeSinceStartup;
	}

	public override void OnLeave()
	{
	}

	public override void OnUpdate(float deltaTime)
	{
		if (Time.realtimeSinceStartup - startTime > 2f)
		{
			machine.ChangeState(TroopState.WorkGarbage);
		}
	}
}
