using System.Collections.Generic;
using UnityEngine;

public class WorldTroopStateAttack : WorldTroopStateBase
{
	public class WayInfo
	{
		public WorldTroopPathSegment[] pathList;

		public int targetPos;
	}

	private enum DummyAttack
	{
		None,
		Idle,
		Attack
	}

	private int targetPos;

	private float waitSkillFinishTime = 1.5f;

	private float startTick;

	private const float maxBattleTime = 3f;

	protected WorldTroop defenderTroop;

	private List<WayInfo> wayList = new List<WayInfo>();

	private WorldTroopPathSegment[] pathList;

	private int curPathIndex;

	private float curPathLen;

	private float moveSpeed;

	private Vector3 moveDir;

	private Vector3 position;

	private int realTargetPos;

	private long endTime;

	private DummyAttack statusDummyAttack;

	private double tickTimeDummyAttack;

	private int attackDummyTimesCounter;

	public WorldTroopStateAttack(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		if (worldTroop != null)
		{
			base.OnStateEnter();
			defenderTroop = worldTroop.GetTargetTroop();
			startTick = Time.realtimeSinceStartup;
			targetPos = worldTroop.GetMarchTargetPos();
			worldTroop.SetRotationRoot();
			worldTroop.SetRotation(Quaternion.LookRotation(worldTroop.GetDefenderPosition() - worldTroop.GetPosition()));
			GameEntry.Event.Fire(EventId.HideTroopName, worldTroop.GetMarchUUID());
			attackDummyTimesCounter = 0;
			StartMove();
			WorldMarch marchInfo = worldTroop.GetMarchInfo();
			if (marchInfo.IsBoss() && !GameEntry.ConfigCache.GetTemplateData("lw_world_monster", marchInfo.monsterId, "attack_plot").IsNullOrEmpty())
			{
				GameEntry.Lua.Call("CSharpCallLuaInterface.PlayBossAttackPlotBubble", marchInfo.monsterId, worldTroop.GetTransform(), 1, 3);
			}
		}
	}

	public override void OnStateLeave()
	{
		if (worldTroop != null && worldTroop.DelayApply && defenderTroop != null && defenderTroop.IsCanPlayAni() && !defenderTroop.IsDelayDestroy)
		{
			WorldMarch marchInfo = defenderTroop.GetMarchInfo();
			if (marchInfo.IsSandWorm() && marchInfo.sandWormData.IsStunning())
			{
				defenderTroop.TryPlay("stun", worldTroop.GetPosition());
			}
			else
			{
				defenderTroop.PlayIdleQueued();
			}
		}
		defenderTroop = null;
		base.OnStateLeave();
		if (worldTroop != null && statusDummyAttack == DummyAttack.Attack)
		{
			statusDummyAttack = DummyAttack.None;
			if (worldTroop.GetMarchInfo().IsWanderBoss())
			{
				worldTroop.PlayAnim("walk");
			}
			else
			{
				worldTroop.PlayAnim("run");
			}
		}
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (worldTroop == null)
		{
			return;
		}
		RecoverWorldTroop();
		if (targetPos != worldTroop.GetMarchTargetPos())
		{
			StartMove();
			if (!worldTroop.CanAttack(curPathLen))
			{
				worldTroop.SetIsBattle(value: false);
			}
		}
		if (worldTroop.DelayApply && worldTroop.CanAttack(curPathLen) && worldTroop.IsBattle())
		{
			if (statusDummyAttack == DummyAttack.Attack)
			{
				tickTimeDummyAttack += deltaTime;
				if (tickTimeDummyAttack >= 1.0 && worldTroop.IsBattle())
				{
					AttackOnce(worldTroop.GetDefenderPositionList());
					tickTimeDummyAttack -= 1.0;
					attackDummyTimesCounter++;
					if (worldTroop.IsFakeAttackMonsterMarch() && attackDummyTimesCounter >= 5)
					{
						worldTroop.UpdateFakeAttackMonsterMarch();
					}
				}
			}
			else if (statusDummyAttack == DummyAttack.Idle)
			{
				AttackOnce(worldTroop.GetDefenderPositionList());
				statusDummyAttack = DummyAttack.Attack;
			}
			else
			{
				statusDummyAttack = DummyAttack.Idle;
			}
			return;
		}
		if (worldTroop.IsBattle())
		{
			if (targetPos != worldTroop.GetMarchTargetPos())
			{
				worldTroop.RemoveAttack();
				StartMove();
				worldTroop.SetIsBattle(value: false);
				ChangeState(WorldTroopState.AttackEnd);
			}
		}
		else
		{
			ChangeState(WorldTroopState.AttackEnd);
		}
		UpdateMovement(deltaTime);
	}

	public override void OnLodChanged(int lod)
	{
		if (lod < 4 && pathList != null && pathList.Length > 1)
		{
			StartMove();
		}
		if (lod <= 1)
		{
			worldTroop.PlayAnim("attack", rewind: true);
			Vector3 defenderPosition = worldTroop.GetDefenderPosition();
			worldTroop.AttackOnce(defenderPosition);
		}
	}

	private void StartMove()
	{
		GameEntry.Timer.GetServerTime();
		targetPos = worldTroop.GetMarchTargetPos();
		pathList = worldTroop.CreatePathSegment();
		if (worldTroop.NeedGetRealTargetPos())
		{
			realTargetPos = worldTroop.GetRealMarchTargetPos();
		}
		else
		{
			realTargetPos = 0;
		}
		moveSpeed = worldTroop.GetSpeed();
		curPathLen = worldTroop.GetMarchInfo().GetPassedLen();
		position = worldTroop.GetPosition();
		if (pathList != null && pathList.Length > 1)
		{
			WorldTroop.CalcMoveOnPath(pathList, 0, curPathLen, out curPathIndex, out var _, out var _);
			if (curPathIndex >= pathList.Length - 1)
			{
				FinishMove();
				return;
			}
			Vector3 vector = pathList[curPathIndex + 1].pos - position;
			moveDir = vector.normalized;
			curPathLen = vector.magnitude;
			if (worldTroop.DelayApply && worldTroop.CanAttack(curPathLen))
			{
				statusDummyAttack = DummyAttack.Idle;
			}
			else
			{
				worldTroop.PlayAnim("idle");
			}
			worldTroop.SetRotation(Quaternion.LookRotation(moveDir));
		}
		else
		{
			FinishMove();
		}
	}

	private void UpdateMovement(float deltaTime)
	{
		if (pathList == null || curPathIndex >= pathList.Length - 1)
		{
			return;
		}
		moveSpeed = worldTroop.GetSpeed();
		long marchBlackEndTime = worldTroop.GetMarchBlackEndTime();
		long marchBlackStartTime = worldTroop.GetMarchBlackStartTime();
		if (marchBlackEndTime > 0 && marchBlackStartTime > 0)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			if (marchBlackStartTime <= serverTime && serverTime <= marchBlackEndTime)
			{
				moveSpeed = worldTroop.GetBlackSpeed();
			}
		}
		position += moveDir * moveSpeed * deltaTime;
		worldTroop.SetPosition(position);
		worldTroop.SetRotation(Quaternion.LookRotation(moveDir));
		curPathLen -= moveSpeed * deltaTime;
		if (curPathLen <= 0f)
		{
			FinishMove();
		}
	}

	private void FinishMove()
	{
		if (wayList.Count > 0)
		{
			return;
		}
		if (worldTroop.IsFakeAttackMonsterMarch())
		{
			worldTroop.UpdateFakeAttackMonsterMarch();
			return;
		}
		if (pathList != null && pathList.Length != 0)
		{
			worldTroop.SetPosition(pathList[pathList.Length - 1].pos);
		}
		else
		{
			worldTroop.SetPosition(worldTroop.GetMarchInfo().position);
		}
		pathList = null;
		Vector3 defenderPosition = worldTroop.GetDefenderPosition();
		AttackOnce(defenderPosition);
	}

	private void AttackOnce(List<Vector3> targetList)
	{
		Vector3 defenderPosition = worldTroop.GetDefenderPosition();
		if (targetList != null && targetList.Count > 1)
		{
			worldTroop.SetRotation(Quaternion.LookRotation(defenderPosition - worldTroop.GetPosition()));
			for (int i = 1; i < 6; i++)
			{
				int index = Random.Range(0, targetList.Count);
				worldTroop.PlayLoopAttackAnim();
				worldTroop.AttackOnceWithIndex(targetList[index], i);
			}
		}
		else
		{
			if (worldTroop.GetMarchTargetType() == MarchTargetType.ATTACK_ALLIANCE_CITY || worldTroop.GetMarchTargetType() == MarchTargetType.ATTACK_CITY_STRONGHOLD || worldTroop.GetMarchTargetType() == MarchTargetType.ATTACK_THRONE)
			{
				defenderPosition += new Vector3(7f, 0f, 5f);
			}
			AttackOnce(defenderPosition);
		}
	}

	private void AttackOnce(Vector3 defPos)
	{
		worldTroop.PlayLoopAttackAnim();
		worldTroop.SetRotation(Quaternion.LookRotation(defPos - worldTroop.GetPosition()));
		worldTroop.AttackOnce(defPos);
	}
}
