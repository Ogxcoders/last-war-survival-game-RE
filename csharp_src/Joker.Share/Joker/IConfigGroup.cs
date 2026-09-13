namespace Joker;

public interface IConfigGroup
{
	void SetInt(string key, int value);

	int GetInt(string key, int defaultValue);

	bool TryGetInt(string key, out int value);

	void SetFloat(string key, float value);

	float GetFloat(string key, float defaultValue);

	bool TryGetFloat(string key, out float value);

	void SetString(string key, string value);

	string GetString(string key, string defaultValue);

	bool TryGetString(string key, out string value);

	void SetGroup(string key, IConfigGroup value);

	IConfigGroup GetGroup(string key, IConfigGroup defaultGroup);

	bool TryGetGroup(string key, out IConfigGroup group);

	bool HasKey(string key);

	void DeleteKey(string key);

	void DeleteAll();
}
