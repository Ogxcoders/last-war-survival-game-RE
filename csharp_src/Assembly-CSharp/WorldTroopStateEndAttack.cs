public class WorldTroopStateEndAttack : WorldTroopStateBase
{
	public WorldTroopStateEndAttack(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		GameEntry.Event.Fire(EventId.HideTroopHead, worldTroop.GetMarchUUID());
		GameEntry.Event.Fire(EventId.ShowTroopName, worldTroop.GetMarchUUID());
	}

	public override void OnStateLeave()
	{
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop == null)
		{
			return;
		}
		MarchStatus marchStatus = worldTroop.GetMarchStatus();
		MarchTargetType marchTargetType = worldTroop.GetMarchTargetType();
		if (marchStatus == MarchStatus.CHASING || marchStatus == MarchStatus.MOVING || marchTargetType == MarchTargetType.BACK_HOME)
		{
			ChangeState(WorldTroopState.Move);
			if (worldTroop.IsExplore())
			{
				GameEntry.Event.Fire(EventId.AttackExploreEnd, worldTroop.GetMarchUUID());
			}
			return;
		}
		switch (marchStatus)
		{
		case MarchStatus.DESTROY_WAIT:
			ChangeState(WorldTroopState.AttackBuild);
			return;
		case MarchStatus.TRANSPORT_BACK_HOME:
			ChangeState(WorldTroopState.TransPortBackHome);
			return;
		case MarchStatus.WAITING:
			if (worldTroop.GetMarchInfo().type == NewMarchType.BLOODY_QUEEN)
			{
				ChangeState(WorldTroopState.BloodyQueenMonsterWait);
				return;
			}
			break;
		}
		ChangeState(WorldTroopState.Idle);
		if (worldTroop.IsExplore())
		{
			GameEntry.Event.Fire(EventId.AttackExploreEnd, worldTroop.GetMarchUUID());
		}
	}
}
