using System.Threading;

namespace System.ComponentModel.Composition.ReflectionModel;

internal sealed class DisposableReflectionComposablePart : ReflectionComposablePart, IDisposable
{
	private volatile int _isDisposed;

	public DisposableReflectionComposablePart(ReflectionComposablePartDefinition definition)
		: base(definition)
	{
	}

	protected override void ReleaseInstanceIfNecessary(object instance)
	{
		if (instance is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	protected override void EnsureRunning()
	{
		base.EnsureRunning();
		if (_isDisposed == 1)
		{
			throw ExceptionBuilder.CreateObjectDisposed(this);
		}
	}

	void IDisposable.Dispose()
	{
		if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) == 0)
		{
			ReleaseInstanceIfNecessary(base.CachedInstance);
		}
	}
}
