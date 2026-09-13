namespace System.Runtime.Collections;

internal abstract class ObjectCacheItem<T> where T : class
{
	public abstract T Value { get; }

	public abstract bool TryAddReference();

	public abstract void ReleaseReference();
}
