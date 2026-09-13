using UnityEngine;

public class WorldTroopStateDefend : WorldTroopStateBase
{
	private Vector3 attackDir;

	private float attackDuration;

	private float defendDuration;

	public WorldTroopStateDefend(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public void SetParam(Vector3 attackDir, float attackDuration)
	{
		if (defendDuration <= 0f)
		{
			this.attackDir = attackDir;
			this.attackDuration = attackDuration;
		}
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			base.OnStateEnter();
			defendDuration = worldTroop.PlayDefendAnim("attack", attackDir, attackDuration);
		}
	}

	public override void OnStateLeave()
	{
		worldTroop.CleanAllQueuedStates();
		defendDuration = 0f;
		base.OnStateLeave();
	}

	public override void OnStateUpdate(float deltaTime)
	{
		defendDuration -= deltaTime;
		if (!(defendDuration > 0f) && worldTroop != null)
		{
			MarchStatus marchStatus = worldTroop.GetMarchStatus();
			MarchTargetType marchTargetType = worldTroop.GetMarchTargetType();
			if (worldTroop.GetMarchInfo().IsSandWorm() && worldTroop.GetMarchInfo().sandWormData.IsStunning())
			{
				ChangeState(WorldTroopState.Stun);
			}
			else if (marchStatus == MarchStatus.CHASING || marchStatus == MarchStatus.MOVING || marchTargetType == MarchTargetType.BACK_HOME)
			{
				ChangeState(WorldTroopState.Move);
			}
			else
			{
				ChangeState(WorldTroopState.Idle);
			}
		}
	}
}
