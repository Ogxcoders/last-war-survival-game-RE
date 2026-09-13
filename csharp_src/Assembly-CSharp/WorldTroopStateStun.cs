public class WorldTroopStateStun : WorldTroopStateBase
{
	private long stateEndTime;

	private float countdown;

	public WorldTroopStateStun(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			base.OnStateEnter();
			worldTroop.PlayAnim("stun");
			worldTroop.SetStunVFX(active: true);
			stateEndTime = 0L;
			if (worldTroop.GetMarchInfo().IsSandWorm())
			{
				stateEndTime = worldTroop.GetMarchInfo().sandWormData.stateEndTime;
			}
		}
	}

	public override void OnStateLeave()
	{
		stateEndTime = 0L;
		base.OnStateLeave();
		worldTroop.SetStunVFX(active: false);
	}

	public override void OnStateDispose()
	{
		stateEndTime = 0L;
		worldTroop.SetStunVFX(active: false);
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop == null)
		{
			return;
		}
		if (stateEndTime > 0)
		{
			countdown += deltaTime;
			if (countdown > 1f)
			{
				countdown = 0f;
				long serverTime = GameEntry.Timer.GetServerTime();
				if (stateEndTime < serverTime)
				{
					worldTroop.oldMarchState = 0;
					ChangeState(WorldTroopState.Idle);
				}
			}
		}
		base.OnStateUpdate(deltaTime);
	}

	public override void OnLodChanged(int lod)
	{
		if (lod <= 1)
		{
			worldTroop.PlayAnim("stun");
		}
	}
}
