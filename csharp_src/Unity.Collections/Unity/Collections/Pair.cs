namespace Unity.Collections;

internal struct Pair<Key, Value>
{
	public Key key;

	public Value value;

	public Pair(Key k, Value v)
	{
		key = k;
		value = v;
	}

	public override string ToString()
	{
		return $"{key} = {value}";
	}
}
