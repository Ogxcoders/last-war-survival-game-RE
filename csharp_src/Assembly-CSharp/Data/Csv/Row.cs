using System.Text;

namespace Data.Csv;

public class Row
{
	public readonly CSVCacheder _csv;

	private readonly string[] _array;

	public int Count
	{
		get
		{
			if (_array == null)
			{
				return 0;
			}
			return _array.Length;
		}
	}

	public string this[string key] => GetData(key);

	public Row(CSVCacheder csv, string[] value)
	{
		_csv = csv;
		_array = value;
	}

	public bool HasKey(string key)
	{
		return _csv.hasKey(key);
	}

	public string GetData(string key, bool log = true)
	{
		if (_csv == null || _array == null)
		{
			return string.Empty;
		}
		int columnIndex = _csv.GetColumnIndex(key);
		if (columnIndex >= 0 && columnIndex < _array.Length)
		{
			return _array[columnIndex];
		}
		return string.Empty;
	}

	public string GetString(string key)
	{
		return GetData(key);
	}

	public int GetInt(string key, int defalut = 0)
	{
		return TryGetInt(key);
	}

	public int TryGetInt(string key, int defalut = 0)
	{
		if (!int.TryParse(GetData(key), out var result))
		{
			return defalut;
		}
		return result;
	}

	public float GetFloat(string key)
	{
		if (!float.TryParse(GetData(key), out var result))
		{
			return 0f;
		}
		return result;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (_array == null)
		{
			return stringBuilder.ToString();
		}
		string[] array = _array;
		foreach (string text in array)
		{
			stringBuilder.Append(text + ",");
		}
		return stringBuilder.ToString();
	}

	public string[] GetStringArray(string key, char spliter, bool copy = false)
	{
		return GetString(key)?.Split(new char[1] { spliter });
	}

	public long GetLong(string key)
	{
		if (!long.TryParse(GetData(key), out var result))
		{
			return 0L;
		}
		return result;
	}
}
