using System.Collections.Generic;
using UnityEngine;

public class WorldTroopStateMove : WorldTroopStateBase
{
	public class WayInfo
	{
		public WorldTroopPathSegment[] pathList;

		public int targetPos;
	}

	private List<WayInfo> wayList = new List<WayInfo>();

	private WorldTroopPathSegment[] pathList;

	private int curPathIndex;

	private float curPathLen;

	private float moveSpeed;

	private Vector3 moveDir;

	private Vector3 position;

	private int targetPos;

	private int realTargetPos;

	private long endTime;

	private bool scoutTroopArrived;

	private const int CITY_RADIUS = 6;

	private const int FAST_RETREAT_SPEED = 2;

	private const int FAST_RETREAT_TIME = 3;

	public WorldTroopStateMove(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		base.OnStateEnter();
		worldTroop.SetIsBattle(value: false);
		StartMove();
	}

	public override void RefreshPosition()
	{
		StartMove();
	}

	public override void OnStateLeave()
	{
		base.OnStateLeave();
		pathList = null;
		worldTroop?.OnMoveStateEnd();
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
			if (worldTroop.GetMarchStatus() == MarchStatus.DESTROY_WAIT)
			{
				ChangeState(WorldTroopState.AttackBuild);
			}
			else if (worldTroop.GetMarchStatus() == MarchStatus.STATION)
			{
				ChangeState(WorldTroopState.Idle);
			}
			else if (worldTroop.GetMarchStatus() == MarchStatus.TRANSPORT_BACK_HOME)
			{
				ChangeState(WorldTroopState.TransPortBackHome);
			}
			else if (worldTroop.GetMarchStatus() == MarchStatus.WAITING && worldTroop.GetMarchInfo().type == NewMarchType.BLOODY_QUEEN)
			{
				ChangeState(WorldTroopState.BloodyQueenMonsterWait);
			}
			else if (targetPos != worldTroop.GetMarchTargetPos())
			{
				StartMove();
			}
		}
		else
		{
			ChangeState(WorldTroopState.Attack);
		}
		UpdateMovement(deltaTime);
	}

	public override void OnLodChanged(int lod)
	{
		if (worldTroop == null)
		{
			return;
		}
		if (lod < 4 && pathList != null && pathList.Length > 1)
		{
			StartMove();
		}
		if (lod > 1)
		{
			return;
		}
		if (!worldTroop.GetMarchInfo().IsWanderBoss())
		{
			WorldMarch marchInfo = worldTroop.GetMarchInfo();
			if (marchInfo == null || marchInfo.GetMarchType() != 26)
			{
				worldTroop.PlayAnim("run");
				return;
			}
		}
		worldTroop.PlayAnim("walk");
	}

	private void StartMove()
	{
		if (worldTroop == null)
		{
			return;
		}
		scoutTroopArrived = false;
		targetPos = worldTroop.GetMarchTargetPos();
		pathList = worldTroop.CreatePathSegment();
		moveSpeed = worldTroop.GetSpeed();
		curPathLen = worldTroop.GetMarchInfo().GetPassedLen();
		position = worldTroop.GetMarchInfo().position;
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
			if (!worldTroop.GetMarchInfo().IsWanderBoss())
			{
				WorldMarch marchInfo = worldTroop.GetMarchInfo();
				if (marchInfo == null || marchInfo.GetMarchType() != 26)
				{
					worldTroop.PlayAnim("run");
					goto IL_014c;
				}
			}
			worldTroop.PlayAnim("walk");
			goto IL_014c;
		}
		FinishMove();
		return;
		IL_014c:
		if (worldTroop.GetMarchInfo().type == NewMarchType.DARKNESS_MONSTER)
		{
			worldTroop.SetRotation(Quaternion.LookRotation(moveDir), includeAnims: true);
		}
		else
		{
			worldTroop.SetRotation(Quaternion.LookRotation(moveDir));
		}
		if (worldTroop.GetMarchInfo().type == NewMarchType.ZOMBIE_RETREAT && (pathList[curPathIndex].dist - curPathLen) / moveSpeed > 3f)
		{
			position += moveDir * 6f;
		}
	}

	private void UpdateMovement(float deltaTime)
	{
		if (worldTroop == null || pathList == null || curPathIndex >= pathList.Length - 1)
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
		WorldMarch marchInfo = worldTroop.GetMarchInfo();
		if (marchInfo != null && marchInfo.type == NewMarchType.ZOMBIE_RETREAT)
		{
			if ((pathList[curPathIndex].dist - curPathLen) / moveSpeed < 3f)
			{
				position += moveDir * 2f * deltaTime;
			}
			else
			{
				position += moveDir * moveSpeed * deltaTime;
			}
		}
		else
		{
			position += moveDir * moveSpeed * deltaTime;
		}
		worldTroop.SetPosition(position);
		worldTroop.SetRotation(Quaternion.LookRotation(moveDir));
		curPathLen -= moveSpeed * deltaTime;
		if (SceneManager.World.GetCurSeasonType() == SeasonType.Darkness)
		{
			float lightLength = worldTroop.GetMarchInfo().LightLength;
			if (lightLength > 0f && curPathLen < lightLength)
			{
				worldTroop.TriggerDefenderToAfraidLight();
			}
		}
		if (!(curPathLen >= 10f))
		{
			if (curPathLen < 10f && marchInfo != null && marchInfo.target == MarchTargetType.SEASON_FARMER_SEND_RES)
			{
				FinishMove();
			}
			else if (worldTroop.CanAttack(curPathLen))
			{
				if (worldTroop.GetMarchTargetType() == MarchTargetType.WHISTLE_MONSTER)
				{
					worldTroop.DelayApply = true;
					worldTroop.DelayApplyTime = curPathLen / moveSpeed;
					ChangeState(WorldTroopState.Whistle);
					worldTroop.TriggerDefenderToBeWhistled(curPathLen / moveSpeed);
					return;
				}
				worldTroop.SetIsBattle(value: true);
				worldTroop.DelayApply = true;
				worldTroop.DelayApplyTime = curPathLen / moveSpeed;
				pathList = null;
				worldTroop.TriggerDefenderToDefend(curPathLen / moveSpeed, moveDir);
				if (worldTroop.IsAttackRadarPlayer())
				{
					GameEntry.Event.Fire(EventId.OnActRadarPlayer, worldTroop.GetMarchUUID());
				}
				if (worldTroop.GetMarchTargetType() == MarchTargetType.ATTACK_ALLIANCE_CITY || worldTroop.GetMarchTargetType() == MarchTargetType.ATTACK_CITY_STRONGHOLD)
				{
					GameEntry.Event.Fire(EventId.OnActAllianceCity, worldTroop.GetMarchUUID());
				}
				if (worldTroop.IsMummyMarch() && SceneManager.World != null && SceneManager.IsInWorld())
				{
					string prefabPath = "Assets/Main/SeasonRes/Shared/Prefabs/Effect/Eff_S3_SandHand.prefab";
					Vector3 v3Pos = SceneManager.World.TileIndexToWorld(targetPos, worldTroop.GetMarchTargetServer());
					SceneManager.World.CreateBattleVFX(prefabPath, 3.5f, delegate(GameObject go)
					{
						if (SceneManager.World != null && SceneManager.World.DynamicObjNode != null)
						{
							go.transform.SetParent(SceneManager.World.DynamicObjNode);
							go.transform.localScale = Vector3.one;
							go.transform.localPosition = v3Pos;
						}
					});
				}
			}
			else if (curPathLen <= 0f)
			{
				FinishMove();
			}
		}
		if (curPathLen < 1f && marchInfo != null && !scoutTroopArrived && (marchInfo.target == MarchTargetType.SEASON_MUMMY_CONVERT || marchInfo.target == MarchTargetType.SCOUT_TREAT || marchInfo.target == MarchTargetType.POWER_WORK_HELPER_CHARGE || marchInfo.target == MarchTargetType.CHARGE_SUPPLIES))
		{
			scoutTroopArrived = true;
			GameEntry.Event.Fire(EventId.OnScoutTroopArrived, worldTroop.GetMarchUUID());
		}
	}

	private void FinishMove()
	{
		if (wayList.Count > 0 || worldTroop == null)
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
		if (worldTroop.IsPickGarbageTroop())
		{
			ChangeState(WorldTroopState.PickGarbageMovetoGarbage);
		}
		else if (worldTroop.IsGolloesExplore())
		{
			ChangeState(WorldTroopState.PickingGarbage);
		}
		else if (worldTroop.IsDetectRescue())
		{
			ChangeState(WorldTroopState.DetectRescueStart);
		}
		else
		{
			ChangeState(WorldTroopState.Idle);
		}
		WorldTroop targetTroop = worldTroop.GetTargetTroop();
		if (targetTroop != null && targetTroop.GetMarchInfo().IsBoss())
		{
			worldTroop.TriggerDefenderToDefend(0.01f, moveDir);
		}
	}
}
