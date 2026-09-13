using System.Collections.Generic;

public class AutoDoMovePosMachine
{
	private Dictionary<AutoDoMovePosState, BaseAutoDoMovePosState> _allState;

	private AutoDoMovePosState _curState;

	private AutoDoMovePos _autoDo;

	public AutoDoMovePosMachine(AutoDoMovePos autoDo)
	{
		_autoDo = autoDo;
		InitAllState();
		_curState = AutoDoMovePosState.DownAnim;
		GetCurState()?.OnEnter();
	}

	public void UnInit()
	{
		GetCurState()?.OnLeave();
	}

	public void ChangeState(AutoDoMovePosState state)
	{
		GetCurState()?.OnLeave();
		_curState = state;
		GetCurState()?.OnEnter();
	}

	private void InitAllState()
	{
		_allState = new Dictionary<AutoDoMovePosState, BaseAutoDoMovePosState>();
		_allState.Add(AutoDoMovePosState.DownAnim, new AutoDoMovePosDownAnimState(_autoDo, this));
		_allState.Add(AutoDoMovePosState.Move, new AutoDoMovePoMoveState(_autoDo, this));
		_allState.Add(AutoDoMovePosState.UpAnim, new AutoDoMovePosUpAnimState(_autoDo, this));
	}

	public BaseAutoDoMovePosState GetCurState()
	{
		if (_allState.ContainsKey(_curState))
		{
			return _allState[_curState];
		}
		return null;
	}

	public void OnUpdate(float deltaTime)
	{
		GetCurState()?.OnUpdate(deltaTime);
	}
}
