using System.Collections.Generic;

public class WorldTroopStateMachine
{
	private Dictionary<WorldTroopState, WorldTroopStateBase> states = new Dictionary<WorldTroopState, WorldTroopStateBase>();

	private WorldTroopStateBase currState;

	private WorldTroop worldTroop;

	private WorldTroopState currStateType;

	public WorldTroopState CurrentStateType => currStateType;

	public WorldTroopStateMachine(WorldTroop worldTroop)
	{
		this.worldTroop = worldTroop;
		currStateType = WorldTroopState.None;
		currState = new WorldTroopStateBase(worldTroop, this);
		states.Add(WorldTroopState.None, currState);
		states.Add(WorldTroopState.Idle, new WorldTroopStateIdle(worldTroop, this));
		states.Add(WorldTroopState.Stun, new WorldTroopStateStun(worldTroop, this));
		states.Add(WorldTroopState.Move, new WorldTroopStateMove(worldTroop, this));
		states.Add(WorldTroopState.Attack, new WorldTroopStateAttack(worldTroop, this));
		states.Add(WorldTroopState.Defend, new WorldTroopStateDefend(worldTroop, this));
		states.Add(WorldTroopState.AttackBegin, new WorldTroopStateStartAttack(worldTroop, this));
		states.Add(WorldTroopState.AttackEnd, new WorldTroopStateEndAttack(worldTroop, this));
		states.Add(WorldTroopState.Death, new WorldTroopStateDeath(worldTroop, this));
		states.Add(WorldTroopState.PickGarbageMovetoGarbage, new WorldTroopStatePickGarbageMovetoGarbage(worldTroop, this));
		states.Add(WorldTroopState.PickingGarbage, new WorldTroopStatePickingGarbage(worldTroop, this));
		states.Add(WorldTroopState.PickGarbageSuccess, new WorldTroopStatePickGarbageSuccess(worldTroop, this));
		states.Add(WorldTroopState.PickGarbageLeaveGarbage, new WorldTroopStatePickGarbageLeaveGarbage(worldTroop, this));
		states.Add(WorldTroopState.AttackBuild, new WorldTroopAttackBuild(worldTroop, this));
		states.Add(WorldTroopState.TransPortBackHome, new WorldTroopTransBack(worldTroop, this));
		states.Add(WorldTroopState.DetectRescueStart, new WorldTroopDetectRescueStart(worldTroop, this));
		states.Add(WorldTroopState.BerserkBossAttack, new WorldTroopBerserkBossAttack(worldTroop, this));
		states.Add(WorldTroopState.WorldTroopPlayAttack, new WorldTroopPlayAttack(worldTroop, this));
		states.Add(WorldTroopState.Whistle, new WorldTroopStateWhistle(worldTroop, this));
		states.Add(WorldTroopState.BloodyQueenMonsterWait, new WorldTroopStateBloodyQueenMonsterWait(worldTroop, this));
		states.Add(WorldTroopState.S1SeasonPreActBossAttackEachOther, new WorldTroopStateS1SeasonPreActBossAttackEachOther(worldTroop, this));
	}

	public WorldTroopState GetCurrentState()
	{
		return currStateType;
	}

	public WorldTroopStateBase GetState(WorldTroopState stateType)
	{
		if (states.TryGetValue(stateType, out var value))
		{
			return value;
		}
		return null;
	}

	public void ChangeState(WorldTroopState stateType)
	{
		if (currStateType != stateType)
		{
			GameEntry.Event.Fire(EventId.CheckTroopStateIcon, worldTroop.GetMarchUUID());
			if (states.TryGetValue(stateType, out var value))
			{
				currState.OnStateLeave();
				currState = value;
				currStateType = stateType;
				currState.OnStateEnter();
			}
		}
	}

	public void OnUpdate(float deltaTime)
	{
		currState.OnStateUpdate(deltaTime);
	}

	public void Dispose()
	{
		currState.OnStateDispose();
	}
}
