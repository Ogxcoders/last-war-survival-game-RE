using UnityEngine;

public class AutoDoMovePosDownAnimState : BaseAutoDoMovePosState
{
	private float _time;

	private float _allTime;

	private float _doAnimTime;

	public AutoDoMovePosDownAnimState(AutoDoMovePos autoDo, AutoDoMovePosMachine autoMachine)
		: base(autoDo, autoMachine)
	{
	}

	public override void OnEnter()
	{
		_time = 0f;
		_allTime = autoDoMovePos.GetDownTime();
		_doAnimTime = _allTime / 3f;
		autoDoMovePos.ChangeStartPos();
	}

	public override void OnUpdate(float deltaTime)
	{
		_time += Time.deltaTime;
		if (_time > _allTime)
		{
			machine.ChangeState(AutoDoMovePosState.Move);
		}
		else if (_time > _doAnimTime)
		{
			GameEntry.Event.Fire(EventId.GuideMoveArrowPlayAnim, 1);
			_doAnimTime = _allTime;
		}
	}

	public override void OnLeave()
	{
	}
}
