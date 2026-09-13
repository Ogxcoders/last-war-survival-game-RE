using UnityEngine;

public class WorldTroopStatePickGarbageLeaveGarbage : WorldTroopStateBase
{
	private float startTime;

	private const float totalTime = 2f;

	public WorldTroopStatePickGarbageLeaveGarbage(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			worldTroop.PlayAnim("back");
			worldTroop.TroopUnitPickBack();
			startTime = Time.realtimeSinceStartup;
		}
	}

	public override void OnStateLeave()
	{
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (Time.realtimeSinceStartup - startTime > 2f)
		{
			ChangeState(WorldTroopState.Move);
		}
	}

	public override void OnLodChanged(int lod)
	{
	}
}
