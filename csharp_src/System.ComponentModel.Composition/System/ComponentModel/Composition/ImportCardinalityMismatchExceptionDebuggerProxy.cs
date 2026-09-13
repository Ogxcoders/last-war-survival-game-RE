using Microsoft.Internal;

namespace System.ComponentModel.Composition;

internal class ImportCardinalityMismatchExceptionDebuggerProxy
{
	private readonly ImportCardinalityMismatchException _exception;

	public Exception InnerException => _exception.InnerException;

	public string Message => _exception.Message;

	public ImportCardinalityMismatchExceptionDebuggerProxy(ImportCardinalityMismatchException exception)
	{
		Requires.NotNull(exception, "exception");
		_exception = exception;
	}
}
