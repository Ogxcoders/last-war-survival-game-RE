using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class CityTroopFightResultState : BaseCityTroopState
{
	private enum FightResultState
	{
		Win = 1,
		Fail
	}

	private FightResultState _resultState;

	private Vector3 _pos;

	private float _curTime;

	private float _allTime;

	private InstanceRequest winInst;

	private InstanceRequest failInst;

	public CityTroopFightResultState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		GameEntry.Event.Subscribe(EventId.CityFightResult, CityFightResultSignal);
		_pos = Vector3.zero;
		_allTime = 0f;
		_resultState = FightResultState.Win;
	}

	public override void OnUpdate(float deltaTime)
	{
		if (_allTime > 0f)
		{
			_curTime += deltaTime;
			if (_curTime >= _allTime)
			{
				machine.ChangeState(TroopState.Idle);
				_resultState = FightResultState.Win;
			}
		}
	}

	public override void OnLeave()
	{
		GameEntry.Event.Unsubscribe(EventId.CityFightResult, CityFightResultSignal);
	}

	public override void OnBeginDrag()
	{
		SceneManager.World.CanMoving = false;
		cityTroop.OnCreateDragLine();
	}

	public override void OnDrag()
	{
		SceneManager.World.CanMoving = false;
		Vector3 touchPoint = SceneManager.World.GetTouchPoint();
		if (_pos != touchPoint)
		{
			_pos = touchPoint;
			cityTroop.OnDragLineUpdate(touchPoint);
		}
	}

	public override void OnEndDrag()
	{
		SceneManager.World.CanMoving = true;
		cityTroop.OnDragLineStop();
		Vector3 vector = CheckCanMovePosByPos(SceneManager.World.GetTouchPoint());
		if (vector != Vector3.zero)
		{
			cityTroop.EndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			machine.ChangeState(TroopState.Move);
		}
	}

	public override void CityTroopMoveSignal(Vector3 pos1)
	{
		Vector3 vector = CheckCanMovePosByPos(pos1);
		if (vector != Vector3.zero)
		{
			cityTroop.EndPos = vector;
			cityTroop.DealGarbageQueueDataWhenDragEnd(vector);
			machine.ChangeState(TroopState.Move);
		}
	}

	private void CityFightResultSignal(object userData)
	{
		if (userData is SFSObject sFSObject)
		{
			if (sFSObject.ContainsKey("result"))
			{
				_resultState = (FightResultState)sFSObject.GetInt("result");
				DoResultAnim();
				machine.ChangeState(TroopState.Idle);
			}
			CheckSurroundAndOpenFog();
		}
	}

	private void CheckSurroundAndOpenFog()
	{
		int pointId = SceneManager.World.WorldToTileIndex(cityTroop.EndPos);
		int fogIndexByPointId = GameEntry.Data.Fog.GetFogIndexByPointId(pointId);
		List<int> canOpenNeighbourFogIds = GameEntry.Data.Fog.GetCanOpenNeighbourFogIds(fogIndexByPointId);
		if (canOpenNeighbourFogIds.Count > 0)
		{
			string userData = string.Join(";", canOpenNeighbourFogIds);
			GameEntry.Event.Fire(EventId.UnlockFogAnim, userData);
		}
	}

	private void DoResultAnim()
	{
		_curTime = 0f;
		if (_resultState == FightResultState.Fail)
		{
			ShowBattleFail();
		}
		else if (_resultState == FightResultState.Win)
		{
			ShowBattleSuccess();
		}
	}

	private void ShowBattleSuccess()
	{
		if (winInst != null)
		{
			return;
		}
		winInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Effect/World/VFX_VictoryNoUI.prefab");
		winInst.completed += delegate
		{
			Transform transform = cityTroop.GetTransform();
			if (transform != null)
			{
				winInst.gameObject.transform.SetParent(transform);
				winInst.gameObject.transform.localScale = Vector3.one * 2f;
				winInst.gameObject.transform.localPosition = new Vector3(0f, 3.55f, 0f);
				YieldUtils.DelayActionWithOutContext(delegate
				{
					if (winInst != null)
					{
						winInst.Destroy();
						winInst = null;
					}
				}, 2.14f);
			}
			else
			{
				RemoveWin();
			}
		};
	}

	private void RemoveWin()
	{
		if (winInst != null)
		{
			winInst.Destroy();
			winInst = null;
		}
	}

	private void ShowBattleFail()
	{
		if (winInst != null)
		{
			return;
		}
		winInst = GameEntry.Resource.InstantiateAsync("Assets/Main/Prefabs/Effect/World/VFX_VictoryNoUI.prefab");
		winInst.completed += delegate
		{
			Transform transform = cityTroop.GetTransform();
			if (transform != null)
			{
				winInst.gameObject.transform.SetParent(transform);
				winInst.gameObject.transform.localScale = Vector3.one * 2f;
				winInst.gameObject.transform.localPosition = new Vector3(0f, 3.55f, 0f);
				YieldUtils.DelayActionWithOutContext(delegate
				{
					if (winInst != null)
					{
						winInst.Destroy();
						winInst = null;
					}
				}, 2.14f);
			}
			else
			{
				RemoveFail();
			}
		};
	}

	private void RemoveFail()
	{
		if (failInst != null)
		{
			failInst.Destroy();
			failInst = null;
		}
	}
}
