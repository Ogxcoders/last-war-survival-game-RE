using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Globalization;

namespace System.ComponentModel.Composition;

[Serializable]
[DebuggerTypeProxy(typeof(CompositionErrorDebuggerProxy))]
public class CompositionError
{
	private readonly CompositionErrorId _id;

	private readonly string _description;

	private readonly Exception _exception;

	private readonly ICompositionElement _element;

	public ICompositionElement Element => _element;

	public string Description => _description;

	public Exception Exception => _exception;

	internal CompositionErrorId Id => _id;

	internal Exception InnerException => Exception;

	public CompositionError(string message)
		: this(CompositionErrorId.Unknown, message, null, null)
	{
	}

	public CompositionError(string message, ICompositionElement element)
		: this(CompositionErrorId.Unknown, message, element, null)
	{
	}

	public CompositionError(string message, Exception exception)
		: this(CompositionErrorId.Unknown, message, null, exception)
	{
	}

	public CompositionError(string message, ICompositionElement element, Exception exception)
		: this(CompositionErrorId.Unknown, message, element, exception)
	{
	}

	internal CompositionError(CompositionErrorId id, string description, ICompositionElement element, Exception exception)
	{
		_id = id;
		_description = description ?? string.Empty;
		_element = element;
		_exception = exception;
	}

	public override string ToString()
	{
		return Description;
	}

	internal static CompositionError Create(CompositionErrorId id, string format, params object[] parameters)
	{
		return Create(id, null, null, format, parameters);
	}

	internal static CompositionError Create(CompositionErrorId id, ICompositionElement element, string format, params object[] parameters)
	{
		return Create(id, element, null, format, parameters);
	}

	internal static CompositionError Create(CompositionErrorId id, ICompositionElement element, Exception exception, string format, params object[] parameters)
	{
		return new CompositionError(id, string.Format(CultureInfo.CurrentCulture, format, parameters), element, exception);
	}
}
