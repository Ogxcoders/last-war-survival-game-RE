using System.Collections;

namespace Unity.Collections;

internal struct ListPair<Key, Value> where Value : IList
{
	public Key key;

	public Value value;

	public ListPair(Key k, Value v)
	{
		key = k;
		value = v;
	}

	public override string ToString()
	{
		string text = $"{key} = [";
		for (int i = 0; i < value.Count; i++)
		{
			text += value[i];
			if (i < value.Count - 1)
			{
				text += ", ";
			}
		}
		return text + "]";
	}
}
