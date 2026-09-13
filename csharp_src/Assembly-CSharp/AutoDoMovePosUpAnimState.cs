using UnityEngine;

public class AutoDoMovePosUpAnimState : BaseAutoDoMovePosState
{
	private float _time;

	private float _allTime;

	private float _doAnimTime;

	public AutoDoMovePosUpAnimState(AutoDoMovePos autoDo, AutoDoMovePosMachine autoMachine)
		: base(autoDo, autoMachine)
	{
	}

	public override void OnEnter()
	{
		_time = 0f;
		_allTime = autoDoMovePos.GetUpTime();
		_doAnimTime = _allTime / 3f;
		autoDoMovePos.ChangeEndPos();
	}

	public override void OnUpdate(float deltaTime)
	{
		_time += Time.deltaTime;
		if (_time > _allTime)
		{
			machine.ChangeState(AutoDoMovePosState.DownAnim);
		}
		else if (_time > _doAnimTime)
		{
			GameEntry.Event.Fire(EventId.GuideMoveArrowPlayAnim, 2);
			_doAnimTime = _allTime;
		}
	}

	public override void OnLeave()
	{
	}
}
