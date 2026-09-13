using System.Collections.Generic;

namespace Joker;

public class ConfigGroup : IConfigGroup
{
	private readonly Dictionary<string, object> _dic = new Dictionary<string, object>();

	public void SetInt(string key, int value)
	{
		_dic[key] = value;
	}

	public int GetInt(string key, int defaultValue)
	{
		if (!TryGetInt(key, out var value))
		{
			return defaultValue;
		}
		return value;
	}

	public bool TryGetInt(string key, out int value)
	{
		if (_dic.TryGetValue(key, out var value2))
		{
			value = (int)value2;
			return true;
		}
		value = 0;
		return false;
	}

	public void SetFloat(string key, float value)
	{
		_dic[key] = value;
	}

	public float GetFloat(string key, float defaultValue)
	{
		if (!TryGetFloat(key, out var value))
		{
			return defaultValue;
		}
		return value;
	}

	public bool TryGetFloat(string key, out float value)
	{
		if (_dic.TryGetValue(key, out var value2))
		{
			value = (float)value2;
			return true;
		}
		value = 0f;
		return false;
	}

	public void SetString(string key, string value)
	{
		_dic[key] = value;
	}

	public string GetString(string key, string defaultValue)
	{
		if (!TryGetString(key, out var value))
		{
			return defaultValue;
		}
		return value;
	}

	public bool TryGetString(string key, out string value)
	{
		if (_dic.TryGetValue(key, out var value2))
		{
			value = (string)value2;
			return true;
		}
		value = null;
		return false;
	}

	public void SetGroup(string key, IConfigGroup value)
	{
		_dic[key] = value;
	}

	public IConfigGroup GetGroup(string key, IConfigGroup defaultGroup)
	{
		if (_dic.TryGetValue(key, out var value))
		{
			return (IConfigGroup)value;
		}
		return defaultGroup;
	}

	public bool TryGetGroup(string key, out IConfigGroup group)
	{
		if (_dic.TryGetValue(key, out var value))
		{
			group = (IConfigGroup)value;
			return true;
		}
		group = null;
		return false;
	}

	public bool HasKey(string key)
	{
		return _dic.ContainsKey(key);
	}

	public void DeleteKey(string key)
	{
		_dic.Remove(key);
	}

	public void DeleteAll()
	{
		_dic.Clear();
	}
}
