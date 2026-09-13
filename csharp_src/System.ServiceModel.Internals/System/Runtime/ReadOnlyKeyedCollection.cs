using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Runtime;

internal class ReadOnlyKeyedCollection<TKey, TValue> : ReadOnlyCollection<TValue>
{
	private KeyedCollection<TKey, TValue> innerCollection;

	public TValue this[TKey key] => innerCollection[key];

	public ReadOnlyKeyedCollection(KeyedCollection<TKey, TValue> innerCollection)
		: base((IList<TValue>)innerCollection)
	{
		this.innerCollection = innerCollection;
	}
}
