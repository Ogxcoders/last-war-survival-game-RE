using System.Collections.Generic;

public class CityTroopMachine
{
	private Dictionary<TroopState, BaseCityTroopState> _allState;

	private TroopState _curState;

	private CityTroop _cityTroop;

	public CityTroopMachine(CityTroop cityTroop)
	{
		_cityTroop = cityTroop;
		InitAllCityTroopState();
		SetInitState();
		GetCurState()?.OnEnter();
	}

	public void UnInit()
	{
		GetCurState()?.OnLeave();
	}

	public void ChangeState(TroopState state)
	{
		GetCurState()?.OnLeave();
		_curState = state;
		GetCurState()?.OnEnter();
	}

	private void InitAllCityTroopState()
	{
		_allState = new Dictionary<TroopState, BaseCityTroopState>();
		_allState.Add(TroopState.Idle, new CityTroopIdleState(_cityTroop, this));
		_allState.Add(TroopState.Move, new CityTroopMoveState(_cityTroop, this));
		_allState.Add(TroopState.WorkGarbage, new CityTroopWorkGarbageState(_cityTroop, this));
		_allState.Add(TroopState.GarbageResult, new CityTroopGarbageResultState(_cityTroop, this));
		_allState.Add(TroopState.OpenFog, new CityTroopOpenFogState(_cityTroop, this));
		_allState.Add(TroopState.Fight, new CityTroopAttackMonsterState(_cityTroop, this));
		_allState.Add(TroopState.FightResult, new CityTroopFightResultState(_cityTroop, this));
		_allState.Add(TroopState.CityTruckPickGarbageMovetoGarbage, new CityTroopStatePickGarbageMovetoGarbage(_cityTroop, this));
	}

	public BaseCityTroopState GetCurState()
	{
		if (_allState.ContainsKey(_curState))
		{
			return _allState[_curState];
		}
		return null;
	}

	private void SetInitState()
	{
		_cityTroop.EndPos = _cityTroop.transform.position;
		int num = SceneManager.World.WorldToTileIndex(_cityTroop.transform.position);
		if (!GameEntry.Data.Fog.IsUnlock(num))
		{
			_cityTroop.OpenFogPointIndex = num;
			_curState = TroopState.OpenFog;
			return;
		}
		_curState = TroopState.Idle;
		switch (SceneManager.World.GetPointType(num))
		{
		case 1:
			_curState = TroopState.WorkGarbage;
			break;
		case 2:
			_curState = TroopState.Fight;
			break;
		}
	}
}
