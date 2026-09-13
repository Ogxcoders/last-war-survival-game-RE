namespace Joker;

public class ConfigService : Singleton<ConfigService>, IConfigGroup, IService
{
	private readonly ConfigGroup _group = new ConfigGroup();

	public void Awake()
	{
	}

	public void Shutdown()
	{
	}

	public void Startup()
	{
	}

	public void Destroy()
	{
	}

	public void SetInt(string key, int value)
	{
		_group.SetInt(key, value);
	}

	public int GetInt(string key, int defaultValue)
	{
		return _group.GetInt(key, defaultValue);
	}

	public bool TryGetInt(string key, out int value)
	{
		return _group.TryGetInt(key, out value);
	}

	public void SetFloat(string key, float value)
	{
		_group.SetFloat(key, value);
	}

	public float GetFloat(string key, float defaultValue)
	{
		return _group.GetFloat(key, defaultValue);
	}

	public bool TryGetFloat(string key, out float value)
	{
		return _group.TryGetFloat(key, out value);
	}

	public void SetString(string key, string value)
	{
		_group.SetString(key, value);
	}

	public string GetString(string key, string defaultValue)
	{
		return _group.GetString(key, defaultValue);
	}

	public bool TryGetString(string key, out string value)
	{
		return _group.TryGetString(key, out value);
	}

	public void SetGroup(string key, IConfigGroup value)
	{
		_group.SetGroup(key, value);
	}

	public IConfigGroup GetGroup(string key, IConfigGroup defaultGroup)
	{
		return _group.GetGroup(key, defaultGroup);
	}

	public bool TryGetGroup(string key, out IConfigGroup group)
	{
		return _group.TryGetGroup(key, out group);
	}

	public bool HasKey(string key)
	{
		return _group.HasKey(key);
	}

	public void DeleteKey(string key)
	{
		_group.DeleteKey(key);
	}

	public void DeleteAll()
	{
		_group.DeleteAll();
	}
}
