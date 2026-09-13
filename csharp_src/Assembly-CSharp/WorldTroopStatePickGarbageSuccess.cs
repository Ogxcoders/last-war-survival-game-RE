using UnityEngine;

public class WorldTroopStatePickGarbageSuccess : WorldTroopStateBase
{
	private float startTime;

	private const float totalTime = 0.6f;

	public WorldTroopStatePickGarbageSuccess(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			worldTroop.PlayAnim("hit");
			startTime = Time.realtimeSinceStartup;
			worldTroop.TroopUnitPickSuccess();
		}
	}

	public override void OnStateLeave()
	{
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (Time.realtimeSinceStartup - startTime > 0.6f)
		{
			ChangeState(WorldTroopState.PickGarbageLeaveGarbage);
		}
	}

	public override void OnLodChanged(int lod)
	{
	}
}
