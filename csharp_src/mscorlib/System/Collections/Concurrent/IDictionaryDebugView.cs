using System.Collections.Generic;
using System.Diagnostics;

namespace System.Collections.Concurrent;

internal sealed class IDictionaryDebugView<K, V>
{
	private readonly IDictionary<K, V> _dictionary;

	[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
	public KeyValuePair<K, V>[] Items
	{
		get
		{
			KeyValuePair<K, V>[] array = new KeyValuePair<K, V>[_dictionary.Count];
			_dictionary.CopyTo(array, 0);
			return array;
		}
	}

	public IDictionaryDebugView(IDictionary<K, V> dictionary)
	{
		if (dictionary == null)
		{
			throw new ArgumentNullException("dictionary");
		}
		_dictionary = dictionary;
	}
}
