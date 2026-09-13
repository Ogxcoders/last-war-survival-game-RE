using UnityEngine;

public class WorldTroopBerserkBossAttack : WorldTroopStateBase
{
	private bool _isSetAttackingRotation;

	public WorldTroopBerserkBossAttack(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			base.OnStateEnter();
			SetAttackingModelRotation();
		}
	}

	public override void OnStateLeave()
	{
		base.OnStateLeave();
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop == null)
		{
			return;
		}
		base.OnStateUpdate(deltaTime);
		if (worldTroop.GetMarchInfo().status == MarchStatus.BERSERK_BOSS_WAITING)
		{
			worldTroop.PlayAnim("idle");
		}
		else if (worldTroop.GetMarchInfo().status == MarchStatus.ATTACKING)
		{
			if (!_isSetAttackingRotation)
			{
				SetAttackingModelRotation();
			}
			if (worldTroop.GetMarchInfo().curHp <= 0)
			{
				worldTroop.PlayAnim("stun");
			}
			else
			{
				worldTroop.PlayLoopAttackAnim();
			}
		}
	}

	private void SetAttackingModelRotation()
	{
		if (worldTroop != null && worldTroop.GetMarchInfo().status == MarchStatus.ATTACKING)
		{
			_isSetAttackingRotation = true;
			worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
		}
	}
}
