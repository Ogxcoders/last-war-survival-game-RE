namespace System.Data.SqlClient.SNI;

internal class SNIError
{
	public readonly SNIProviders provider;

	public readonly string errorMessage;

	public readonly uint nativeError;

	public readonly uint sniError;

	public readonly string function;

	public readonly uint lineNumber;

	public readonly Exception exception;

	public SNIError(SNIProviders provider, uint nativeError, uint sniErrorCode, string errorMessage)
	{
		lineNumber = 0u;
		function = string.Empty;
		this.provider = provider;
		this.nativeError = nativeError;
		sniError = sniErrorCode;
		this.errorMessage = errorMessage;
		exception = null;
	}

	public SNIError(SNIProviders provider, uint sniErrorCode, Exception sniException)
	{
		lineNumber = 0u;
		function = string.Empty;
		this.provider = provider;
		nativeError = 0u;
		sniError = sniErrorCode;
		errorMessage = string.Empty;
		exception = sniException;
	}
}
