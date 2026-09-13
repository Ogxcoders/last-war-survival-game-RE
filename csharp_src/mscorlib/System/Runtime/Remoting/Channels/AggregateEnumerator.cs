using System.Collections;

namespace System.Runtime.Remoting.Channels;

internal class AggregateEnumerator : IDictionaryEnumerator, IEnumerator
{
	private IDictionary[] dictionaries;

	private int pos;

	private IDictionaryEnumerator currente;

	public DictionaryEntry Entry => currente.Entry;

	public object Key => currente.Key;

	public object Value => currente.Value;

	public object Current => currente.Current;

	public AggregateEnumerator(IDictionary[] dics)
	{
		dictionaries = dics;
		Reset();
	}

	public bool MoveNext()
	{
		if (pos >= dictionaries.Length)
		{
			return false;
		}
		if (!currente.MoveNext())
		{
			pos++;
			if (pos >= dictionaries.Length)
			{
				return false;
			}
			currente = dictionaries[pos].GetEnumerator();
			return MoveNext();
		}
		return true;
	}

	public void Reset()
	{
		pos = 0;
		if (dictionaries.Length != 0)
		{
			currente = dictionaries[0].GetEnumerator();
		}
	}
}
