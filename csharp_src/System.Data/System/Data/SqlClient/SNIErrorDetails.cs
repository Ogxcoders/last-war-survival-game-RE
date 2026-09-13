namespace System.Data.SqlClient;

internal struct SNIErrorDetails
{
	public string errorMessage;

	public uint nativeError;

	public uint sniErrorNumber;

	public int provider;

	public uint lineNumber;

	public string function;

	public Exception exception;
}
