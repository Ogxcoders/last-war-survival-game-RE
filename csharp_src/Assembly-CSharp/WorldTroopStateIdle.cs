public class WorldTroopStateIdle : WorldTroopStateBase
{
	private int curPos;

	public WorldTroopStateIdle(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop == null)
		{
			return;
		}
		base.OnStateEnter();
		GameEntry.Event.Fire(EventId.CheckTroopStateIcon, worldTroop.GetMarchUUID());
		curPos = ((worldTroop.GetMarchInfo().type == NewMarchType.RUNNING_MUMMY) ? worldTroop.GetMarchInfo().startPos : worldTroop.GetMarchTargetPos());
		if (worldTroop.GetMarchStatus() == MarchStatus.STATION)
		{
			worldTroop.SetRotation(worldTroop.GetStationRotation());
		}
		if (worldTroop.idlePlayAttack)
		{
			worldTroop.PlayAnim("attack");
			YieldUtils.DelayActionWithOutContext(delegate
			{
				if (!worldTroop.isDestroy)
				{
					if (worldTroop.GetMarchInfo().IsFrozen())
					{
						worldTroop.SetStateSpeed("idle", 0f);
					}
					else
					{
						worldTroop.SetStateSpeed("idle", 1f);
					}
					PlayIdleAnim();
				}
			}, 1.5f);
		}
		else
		{
			if (worldTroop.GetMarchInfo().IsFrozen())
			{
				worldTroop.SetStateSpeed("idle", 0f);
			}
			else
			{
				worldTroop.SetStateSpeed("idle", 1f);
			}
			PlayIdleAnim();
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
		RecoverWorldTroop();
		if (!worldTroop.IsBattle())
		{
			if (worldTroop.GetMarchStatus() == MarchStatus.CHASING || worldTroop.GetMarchStatus() == MarchStatus.MOVING || worldTroop.GetMarchStatus() == MarchStatus.IN_WORM_HOLE)
			{
				if (worldTroop.GetMovePathCount() > 1 && curPos != worldTroop.GetMarchTargetPos())
				{
					ChangeState(WorldTroopState.Move);
				}
				else if (worldTroop.IsS1SeasonPreBoss())
				{
					ChangeState(WorldTroopState.S1SeasonPreActBossAttackEachOther);
				}
			}
			else if (worldTroop.GetMarchStatus() == MarchStatus.DESTROY_WAIT)
			{
				ChangeState(WorldTroopState.AttackBuild);
			}
			else if (worldTroop.GetMarchStatus() == MarchStatus.TRANSPORT_BACK_HOME)
			{
				ChangeState(WorldTroopState.TransPortBackHome);
			}
			else if (worldTroop.GetMarchStatus() == MarchStatus.PICKING || worldTroop.GetMarchStatus() == MarchStatus.SAMPLING)
			{
				ChangeState(WorldTroopState.PickGarbageMovetoGarbage);
			}
		}
		else
		{
			ChangeState(WorldTroopState.Attack);
		}
	}

	public override void OnLodChanged(int lod)
	{
		if (lod <= 1)
		{
			PlayIdleAnim();
		}
	}

	public override void OnMonsterIceBroken()
	{
		worldTroop.SetStateSpeed("idle", 1f);
	}

	private void PlayIdleAnim()
	{
		if (SceneManager.World != null && SceneManager.World is WorldScene worldScene && worldScene.GetCurSeasonType() == SeasonType.Darkness)
		{
			if (worldTroop.GetMarchInfo().IsS4BNMonsterOrBoss())
			{
				worldTroop.PlayAnim("idle01");
			}
			else if (worldTroop.GetMarchInfo().IsS4WNMonster() && (worldScene.GetBloodyNightState() == BloodyNightState.None || LightDataManager.GetInstance().IsLightUpInPointId(worldTroop.GetMarchInfo().startPos)))
			{
				worldTroop.PlayAnim("idle02");
			}
			else
			{
				worldTroop.PlayAnim("idle");
			}
		}
		else
		{
			worldTroop.PlayAnim("idle");
		}
	}
}
