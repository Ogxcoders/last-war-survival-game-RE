using UnityEngine;

public class WorldTroopStateBase
{
	protected WorldTroop worldTroop;

	private WorldTroopStateMachine stateMachine;

	private bool isBirth;

	public WorldTroopStateBase(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
	{
		this.worldTroop = worldTroop;
		this.stateMachine = stateMachine;
	}

	public void ChangeState(WorldTroopState stateType)
	{
		stateMachine.ChangeState(stateType);
		worldTroop.UpdatePerformance();
	}

	public virtual void OnStateEnter()
	{
	}

	public virtual void OnStateLeave()
	{
	}

	public virtual void OnStateUpdate(float deltaTime)
	{
	}

	public virtual void OnLodChanged(int lod)
	{
	}

	protected virtual void RecoverWorldTroop()
	{
	}

	protected virtual void BirthWorldTroop()
	{
		isBirth = true;
	}

	public virtual void RefreshPosition()
	{
	}

	protected void Log(string txt)
	{
		Debug.Log("<color=green>" + txt + "</color>");
	}

	public virtual void OnMonsterIceBroken()
	{
	}

	public virtual void OnStateDispose()
	{
	}
}
