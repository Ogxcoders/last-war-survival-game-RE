using System.ComponentModel.Composition.Primitives;
using Microsoft.Internal;

namespace System.ComponentModel.Composition;

internal class CompositionErrorDebuggerProxy
{
	private readonly CompositionError _error;

	public string Description => _error.Description;

	public Exception Exception => _error.Exception;

	public ICompositionElement Element => _error.Element;

	public CompositionErrorDebuggerProxy(CompositionError error)
	{
		Requires.NotNull(error, "error");
		_error = error;
	}
}
