namespace System.Data;

public sealed class StateChangeEventArgs : EventArgs
{
	private ConnectionState _originalState;

	private ConnectionState _currentState;

	public ConnectionState CurrentState => _currentState;

	public ConnectionState OriginalState => _originalState;

	public StateChangeEventArgs(ConnectionState originalState, ConnectionState currentState)
	{
		_originalState = originalState;
		_currentState = currentState;
	}
}
