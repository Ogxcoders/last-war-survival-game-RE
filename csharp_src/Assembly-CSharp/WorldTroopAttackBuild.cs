using UnityEngine;

public class WorldTroopAttackBuild : WorldTroopStateBase
{
	private long startTimeSecond;

	private long endTimeSecond;

	private long targetUuid;

	private long lastAttackSecond;

	public WorldTroopAttackBuild(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		base.OnStateEnter();
		if (worldTroop != null)
		{
			WorldMarch marchInfo = worldTroop.GetMarchInfo();
			if (marchInfo != null)
			{
				startTimeSecond = marchInfo.startTime / 1000;
				endTimeSecond = marchInfo.endTime / 1000;
				targetUuid = marchInfo.targetUuid;
			}
			worldTroop.SetRotationRoot();
			worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
			GameEntry.Event.Fire(EventId.ShowTroopAtkBuildIcon, worldTroop.GetMarchUUID());
		}
	}

	public override void OnStateLeave()
	{
		if (worldTroop != null)
		{
			worldTroop.RemoveAttack();
			GameEntry.Event.Fire(EventId.HideTroopAtkBuildIcon, worldTroop.GetMarchUUID());
			base.OnStateLeave();
		}
	}

	public override void OnStateUpdate(float deltaTime)
	{
		base.OnStateUpdate(deltaTime);
		if (worldTroop == null)
		{
			return;
		}
		CheckAttack();
		if (worldTroop.IsBattle())
		{
			ChangeState(WorldTroopState.AttackBegin);
		}
		else if (worldTroop.GetMarchStatus() != MarchStatus.DESTROY_WAIT)
		{
			if (worldTroop.GetMarchStatus() == MarchStatus.CHASING || worldTroop.GetMarchStatus() == MarchStatus.MOVING)
			{
				ChangeState(WorldTroopState.Move);
			}
			else
			{
				ChangeState(WorldTroopState.Idle);
			}
		}
	}

	private void CheckAttack()
	{
		if (worldTroop != null)
		{
			int serverTimeSeconds = GameEntry.Timer.GetServerTimeSeconds();
			if (serverTimeSeconds < endTimeSecond && serverTimeSeconds - lastAttackSecond >= 1)
			{
				lastAttackSecond = serverTimeSeconds;
				worldTroop.ShowAttack();
			}
		}
	}
}
