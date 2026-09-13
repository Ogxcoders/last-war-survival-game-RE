namespace System.Collections.Specialized;

public class StringEnumerator
{
	private IEnumerator _baseEnumerator;

	private IEnumerable _temp;

	public string Current => (string)_baseEnumerator.Current;

	internal StringEnumerator(StringCollection mappings)
	{
		_temp = mappings;
		_baseEnumerator = _temp.GetEnumerator();
	}

	public bool MoveNext()
	{
		return _baseEnumerator.MoveNext();
	}

	public void Reset()
	{
		_baseEnumerator.Reset();
	}
}
