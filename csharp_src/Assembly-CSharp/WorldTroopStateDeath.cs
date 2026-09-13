public class WorldTroopStateDeath : WorldTroopStateBase
{
	private int targetPos;

	public WorldTroopStateDeath(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			targetPos = worldTroop.GetMarchTargetPos();
			worldTroop.PlayAnim("death");
		}
	}

	public override void OnStateUpdate(float deltaTime)
	{
		_ = worldTroop;
	}

	public override void OnLodChanged(int lod)
	{
	}
}
