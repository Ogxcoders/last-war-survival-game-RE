using UnityEngine;

public class WorldTroopStateBloodyQueenMonsterWait : WorldTroopStateBase
{
	private enum DummyAttack
	{
		None,
		Idle,
		Attack
	}

	private DummyAttack statusDummyAttack;

	private double tickTimeDummyAttack;

	public WorldTroopStateBloodyQueenMonsterWait(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			base.OnStateEnter();
			PlayWaitAnim();
			if (NeedShowLoopAttackAnim())
			{
				worldTroop.SetPosition(worldTroop.GetMarchInfo().bloodyQueenMonster.standWorldPos);
				worldTroop.SetRotationRoot();
				worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
			}
			else if (worldTroop.GetMonsterSpecialType() == 48)
			{
				worldTroop.SetRotationRoot();
				worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetMarchInfo().bloodyQueenMonster.standWorldPos - worldTroop.GetPosition()));
			}
		}
	}

	public override void OnStateLeave()
	{
		base.OnStateLeave();
		statusDummyAttack = DummyAttack.None;
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop == null)
		{
			return;
		}
		if (NeedShowLoopAttackAnim())
		{
			if (statusDummyAttack == DummyAttack.Attack)
			{
				tickTimeDummyAttack += deltaTime;
				if (tickTimeDummyAttack >= 1.0)
				{
					worldTroop.PlayLoopAttackAnim();
					tickTimeDummyAttack -= 1.0;
				}
			}
			else if (statusDummyAttack == DummyAttack.Idle)
			{
				worldTroop.PlayLoopAttackAnim();
				statusDummyAttack = DummyAttack.Attack;
			}
			else
			{
				statusDummyAttack = DummyAttack.Idle;
			}
		}
		base.OnStateUpdate(deltaTime);
	}

	public override void OnLodChanged(int lod)
	{
		if (lod <= 1)
		{
			PlayWaitAnim();
		}
	}

	private void PlayWaitAnim()
	{
		if (NeedShowLoopAttackAnim())
		{
			worldTroop.PlayLoopAttackAnim();
		}
		else
		{
			worldTroop.PlayAnim("idle");
		}
	}

	private bool NeedShowLoopAttackAnim()
	{
		if (worldTroop.GetMonsterSpecialType() != 47)
		{
			return worldTroop.GetMonsterSpecialType() == 49;
		}
		return true;
	}
}
