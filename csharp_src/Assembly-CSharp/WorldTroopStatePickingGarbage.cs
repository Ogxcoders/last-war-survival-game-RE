using UnityEngine;

public class WorldTroopStatePickingGarbage : WorldTroopStateBase
{
	public WorldTroopStatePickingGarbage(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
			if (worldTroop.IsGolloesExplore())
			{
				worldTroop.PlayAnim("idle");
			}
			else
			{
				worldTroop.PlayAnim("idle");
				worldTroop.TroopUnitsBirthThenPickGarbage(appear: false);
			}
			GameEntry.Event.Fire(EventId.GarbageCollectStart, worldTroop.GetMarchInfo().targetUuid);
		}
	}

	public override void OnStateLeave()
	{
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop != null && worldTroop.GetMarchStatus() != MarchStatus.PICKING && worldTroop.GetMarchStatus() != MarchStatus.SAMPLING && worldTroop.GetMarchStatus() != MarchStatus.GOLLOES_EXPLORING && worldTroop.GetMarchStatus() != MarchStatus.GOLLOES_EXPLORING)
		{
			if (worldTroop.IsGolloesExplore())
			{
				ChangeState(WorldTroopState.Move);
				return;
			}
			worldTroop.ShowJunkMan();
			worldTroop.TroopUnitPickBack();
			ChangeState(WorldTroopState.Move);
		}
	}

	public override void OnLodChanged(int lod)
	{
	}
}
