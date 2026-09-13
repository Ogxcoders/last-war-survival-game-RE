using Microsoft.Internal;

namespace System.ComponentModel.Composition.Primitives;

internal class ComposablePartExceptionDebuggerProxy
{
	private readonly ComposablePartException _exception;

	public ICompositionElement Element => _exception.Element;

	public Exception InnerException => _exception.InnerException;

	public string Message => _exception.Message;

	public ComposablePartExceptionDebuggerProxy(ComposablePartException exception)
	{
		Requires.NotNull(exception, "exception");
		_exception = exception;
	}
}
