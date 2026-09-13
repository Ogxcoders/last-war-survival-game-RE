using UnityEngine;

public class AutoDoMovePoMoveState : BaseAutoDoMovePosState
{
	private float _time;

	private float _allTime;

	private int _useIndex;

	private float _speed;

	private int _allCount;

	public AutoDoMovePoMoveState(AutoDoMovePos autoDo, AutoDoMovePosMachine autoMachine)
		: base(autoDo, autoMachine)
	{
	}

	public override void OnEnter()
	{
		_time = 0f;
		_useIndex = 0;
		_speed = autoDoMovePos.GetMoveSpeed();
		_allCount = autoDoMovePos.GetMovePosListCount();
		_allTime = GetAllTime();
	}

	public override void OnUpdate(float deltaTime)
	{
		_time += Time.deltaTime;
		autoDoMovePos.transform.position = Vector3.Lerp(GetStartScreenPos(), GetEndScreenPos(), _time / _allTime);
		if (_time > _allTime)
		{
			_time = 0f;
			_useIndex++;
			if (_useIndex >= _allCount - 1)
			{
				machine.ChangeState(AutoDoMovePosState.UpAnim);
			}
			else
			{
				_allTime = GetAllTime();
			}
		}
	}

	public override void OnLeave()
	{
	}

	private Vector3 GetStartScreenPos()
	{
		return autoDoMovePos.GetScreenPos(_useIndex);
	}

	private Vector3 GetEndScreenPos()
	{
		return autoDoMovePos.GetScreenPos(_useIndex + 1);
	}

	private float GetAllTime()
	{
		return Vector3.Distance(GetStartScreenPos(), GetEndScreenPos()) / _speed;
	}
}
