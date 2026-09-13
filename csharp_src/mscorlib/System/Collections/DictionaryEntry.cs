namespace System.Collections;

[Serializable]
public struct DictionaryEntry
{
	private object _key;

	private object _value;

	public object Key
	{
		get
		{
			return _key;
		}
		set
		{
			_key = value;
		}
	}

	public object Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	public DictionaryEntry(object key, object value)
	{
		_key = key;
		_value = value;
	}

	public void Deconstruct(out object key, out object value)
	{
		key = Key;
		value = Value;
	}
}
