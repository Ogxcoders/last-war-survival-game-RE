using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class CityTroopGarbageResultState : BaseCityTroopState
{
	private enum GarbageResultState
	{
		None,
		Reward
	}

	private GarbageResultState _resultState;

	private Vector3 _pos;

	private float _curTime;

	private float _allTime;

	public CityTroopGarbageResultState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		GameEntry.Event.Subscribe(EventId.CityGarbageResult, CityGarbageResultSignal);
		_pos = Vector3.zero;
		_allTime = 0f;
		_resultState = GarbageResultState.None;
	}

	public override void OnUpdate(float deltaTime)
	{
		if (!(_allTime > 0f))
		{
			return;
		}
		_curTime += deltaTime;
		if (_curTime >= _allTime)
		{
			if (cityTroop.DoWhenUseTmpEndPos())
			{
				machine.ChangeState(TroopState.Move);
			}
			else
			{
				machine.ChangeState(TroopState.Idle);
			}
			_resultState = GarbageResultState.None;
		}
	}

	public override void OnLeave()
	{
		GameEntry.Event.Unsubscribe(EventId.CityGarbageResult, CityGarbageResultSignal);
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
		if (vector != Vector3.zero && !cityTroop.DealGarbageQueueDataWhenDragEnd(vector))
		{
			cityTroop.TmpEndPos = vector;
			cityTroop.DoWhenSetTmpEndPos();
		}
	}

	public override void CityTroopMoveSignal(Vector3 pos1)
	{
		Vector3 vector = CheckCanMovePosByPos(pos1);
		if (vector != Vector3.zero && !cityTroop.DealGarbageQueueDataWhenDragEnd(vector))
		{
			cityTroop.TmpEndPos = vector;
			cityTroop.DoWhenSetTmpEndPos();
		}
	}

	private void CityGarbageResultSignal(object userData)
	{
		if (userData is SFSObject sFSObject && sFSObject.ContainsKey("uuid") && sFSObject.ContainsKey("result"))
		{
			_resultState = (GarbageResultState)sFSObject.GetInt("result");
			DoResultAnim();
		}
	}

	private void DoResultAnim()
	{
		_curTime = 0f;
		CheckSurroundAndOpenFog();
		GameEntry.Lua.Call("CSharpCallLuaInterface.RemoveFromGarbageQueue", 1);
		int num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCurrentGarbageQueue");
		if (num > 0)
		{
			cityTroop.EndPos = SceneManager.World.TileIndexToWorld(num);
			machine.ChangeState(TroopState.Move);
		}
		else if (_resultState == GarbageResultState.None)
		{
			_allTime = 1f;
			cityTroop.PlayGarbageFail();
		}
		else if (_resultState == GarbageResultState.Reward)
		{
			_allTime = 2.8f;
			cityTroop.PlayGarbageSuccess();
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
}
