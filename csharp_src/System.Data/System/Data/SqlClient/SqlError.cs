namespace System.Data.SqlClient;

[Serializable]
public sealed class SqlError
{
	private string _source = "Core .Net SqlClient Data Provider";

	private int _number;

	private byte _state;

	private byte _errorClass;

	private string _server;

	private string _message;

	private string _procedure;

	private int _lineNumber;

	private int _win32ErrorCode;

	private Exception _exception;

	public string Source => _source;

	public int Number => _number;

	public byte State => _state;

	public byte Class => _errorClass;

	public string Server => _server;

	public string Message => _message;

	public string Procedure => _procedure;

	public int LineNumber => _lineNumber;

	internal int Win32ErrorCode => _win32ErrorCode;

	internal Exception Exception => _exception;

	internal SqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber, uint win32ErrorCode, Exception exception = null)
		: this(infoNumber, errorState, errorClass, server, errorMessage, procedure, lineNumber, exception)
	{
		_win32ErrorCode = (int)win32ErrorCode;
	}

	internal SqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber, Exception exception = null)
	{
		_number = infoNumber;
		_state = errorState;
		_errorClass = errorClass;
		_server = server;
		_message = errorMessage;
		_procedure = procedure;
		_lineNumber = lineNumber;
		_win32ErrorCode = 0;
		_exception = exception;
	}

	public override string ToString()
	{
		return typeof(SqlError).ToString() + ": " + _message;
	}
}
