using UnityEngine;

public class CityTroopOpenFogState : BaseCityTroopState
{
	private Vector3 _pos;

	private float _curTime;

	private float _allTime;

	private const float DefaultTime = 3.5f;

	private const string ShowAnimationName = "Default";

	private const string HideAnimationName = "CollectGarbageUI_hide";

	private SuperTextMesh _timeText;

	private long _showTime;

	public CityTroopOpenFogState(CityTroop troop, CityTroopMachine troopMachine)
		: base(troop, troopMachine)
	{
	}

	public override void OnEnter()
	{
		_pos = Vector3.zero;
		_curTime = 0f;
		_allTime = 3.5f;
		_showTime = 0L;
		cityTroop.EnterWorkOpenFog();
		GameEntry.Event.Fire(EventId.UnlockFogAnim, cityTroop.OpenFogPointIndex.ToString());
	}

	public override void OnUpdate(float deltaTime)
	{
		_curTime += deltaTime;
		if (_curTime - _allTime > 0f && GameEntry.Data.Fog.IsUnlock(cityTroop.OpenFogPointIndex))
		{
			machine.ChangeState(TroopState.Move);
		}
	}

	public override void OnLeave()
	{
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
}
