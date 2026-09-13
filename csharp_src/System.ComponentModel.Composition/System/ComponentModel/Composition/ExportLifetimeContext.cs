namespace System.ComponentModel.Composition;

public sealed class ExportLifetimeContext<T> : IDisposable
{
	private readonly T _value;

	private readonly Action _disposeAction;

	public T Value => _value;

	public ExportLifetimeContext(T value, Action disposeAction)
	{
		_value = value;
		_disposeAction = disposeAction;
	}

	public void Dispose()
	{
		if (_disposeAction != null)
		{
			_disposeAction();
		}
	}
}
