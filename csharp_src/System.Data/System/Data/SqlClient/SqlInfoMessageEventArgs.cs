namespace System.Data.SqlClient;

public sealed class SqlInfoMessageEventArgs : EventArgs
{
	private SqlException _exception;

	public SqlErrorCollection Errors => _exception.Errors;

	public string Message => _exception.Message;

	public string Source => _exception.Source;

	internal SqlInfoMessageEventArgs(SqlException exception)
	{
		_exception = exception;
	}

	private bool ShouldSerializeErrors()
	{
		if (_exception != null)
		{
			return 0 < _exception.Errors.Count;
		}
		return false;
	}

	public override string ToString()
	{
		return Message;
	}
}
