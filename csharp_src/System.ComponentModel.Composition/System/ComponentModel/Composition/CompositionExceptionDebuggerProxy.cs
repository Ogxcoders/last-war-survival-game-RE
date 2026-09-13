using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition;

internal class CompositionExceptionDebuggerProxy
{
	private readonly CompositionException _exception;

	public ReadOnlyCollection<Exception> Exceptions
	{
		get
		{
			List<Exception> list = new List<Exception>();
			foreach (CompositionError error in _exception.Errors)
			{
				if (error.Exception != null)
				{
					list.Add(error.Exception);
				}
			}
			return list.ToReadOnlyCollection();
		}
	}

	public string Message => _exception.Message;

	public ReadOnlyCollection<Exception> RootCauses
	{
		get
		{
			List<Exception> list = new List<Exception>();
			foreach (CompositionError error in _exception.Errors)
			{
				if (error.Exception == null)
				{
					continue;
				}
				if (error.Exception is CompositionException exception)
				{
					CompositionExceptionDebuggerProxy compositionExceptionDebuggerProxy = new CompositionExceptionDebuggerProxy(exception);
					if (compositionExceptionDebuggerProxy.RootCauses.Count > 0)
					{
						list.AddRange(compositionExceptionDebuggerProxy.RootCauses);
						continue;
					}
				}
				list.Add(error.Exception);
			}
			return list.ToReadOnlyCollection();
		}
	}

	public CompositionExceptionDebuggerProxy(CompositionException exception)
	{
		Requires.NotNull(exception, "exception");
		_exception = exception;
	}
}
