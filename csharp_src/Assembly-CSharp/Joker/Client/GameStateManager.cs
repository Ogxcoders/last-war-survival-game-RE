using System;
using System.Collections.Generic;

namespace Joker.Client;

public class GameStateManager : IGameManager, IUpdate, ILateUpdate, IFixedUpdate
{
	private Dictionary<int, IGameState> _idToStates = new Dictionary<int, IGameState>();

	private Dictionary<Type, IGameState> _typeToStates = new Dictionary<Type, IGameState>();

	private IGameState _currentState;

	private IUpdate _currentStateUpdater;

	private ILateUpdate _currentStateLateUpdater;

	private IFixedUpdate _currentStateFixedUpdater;

	private IGameState _targetState;

	private bool _targetFading;

	private object _exitParam;

	private object _enterParam;

	public IGameState CurrentState => _currentState;

	public GameStateManager()
	{
	}

	public GameStateManager(IGameState[] states)
	{
		foreach (IGameState state in states)
		{
			AddState(state);
		}
		ChangeState(states[0].Id);
	}

	public void AddState(IGameState state)
	{
		_idToStates.Add(state.Id, state);
		_typeToStates.Add(state.GetType(), state);
		if (_currentState == null && _targetState == null)
		{
			ChangeState(state.Id);
		}
	}

	public IGameState GetState(int id)
	{
		if (_idToStates.TryGetValue(id, out var value))
		{
			return value;
		}
		return null;
	}

	public void ChangeState(int state, object enterParam = null, object exitParam = null)
	{
		_targetState = _idToStates[state];
		_enterParam = enterParam;
		_exitParam = exitParam;
	}

	public void ChangeState<T>(object enterParam = null, object exitParam = null)
	{
		_targetState = _typeToStates[typeof(T)];
		_enterParam = enterParam;
		_exitParam = exitParam;
	}

	private void _ChangeState()
	{
		_currentState?.Exit(_exitParam, _targetState);
		_targetState?.Enter(_enterParam, _currentState);
		_currentState = _targetState;
		_targetState = null;
		_enterParam = null;
		_exitParam = null;
		_currentStateUpdater = _currentState as IUpdate;
		_currentStateLateUpdater = _currentState as ILateUpdate;
		_currentStateFixedUpdater = _currentState as IFixedUpdate;
	}

	public void Update()
	{
		_currentStateUpdater?.Update();
		if (_targetState != null && (_currentState == null || _currentState.IsEndLoading))
		{
			_ChangeState();
		}
	}

	public void LateUpdate()
	{
		_currentStateLateUpdater?.LateUpdate();
	}

	public void FixedUpdate()
	{
		_currentStateFixedUpdater?.FixedUpdate();
	}
}
