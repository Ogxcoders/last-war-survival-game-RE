using System.Diagnostics;

namespace System.Collections.Concurrent;

internal sealed class BlockingCollectionDebugView<T>
{
	private readonly BlockingCollection<T> _blockingCollection;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public T[] Items => _blockingCollection.ToArray();

	public BlockingCollectionDebugView(BlockingCollection<T> collection)
	{
		if (collection == null)
		{
			throw new ArgumentNullException("collection");
		}
		_blockingCollection = collection;
	}
}
