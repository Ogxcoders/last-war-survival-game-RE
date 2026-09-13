using UnityEngine;

public class WorldTroopStatePickGarbageMovetoGarbage : WorldTroopStateBase
{
	private float startTime;

	private const float totalTime = 2f;

	public WorldTroopStatePickGarbageMovetoGarbage(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
			worldTroop.PlayAnim("idle");
			worldTroop.HideJunkMan();
			WorldMarch marchInfo = worldTroop.GetMarchInfo();
			if (marchInfo != null && marchInfo.target == MarchTargetType.SEASON_FARMER_SEND_RES)
			{
				worldTroop.TroopUnitsBirthThenPickGarbage(appear: false);
			}
			else
			{
				worldTroop.TroopUnitsBirthThenPickGarbage();
			}
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
			ChangeState(WorldTroopState.PickingGarbage);
		}
	}

	public override void OnLodChanged(int lod)
	{
	}
}
