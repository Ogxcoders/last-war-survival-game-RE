using System.Collections.Generic;
using XLua;

public class ConfigCache
{
	private Dictionary<string, Dictionary<int, Dictionary<string, string>>> cacheConfig;

	public ConfigCache()
	{
		cacheConfig = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();
	}

	public string GetTemplateData(string tabName, int id, string colName)
	{
		if (cacheConfig.ContainsKey(tabName))
		{
			if (cacheConfig[tabName].ContainsKey(id))
			{
				if (cacheConfig[tabName][id].ContainsKey(colName))
				{
					return cacheConfig[tabName][id][colName];
				}
				string templateData = GameEntry.Lua.GetTemplateData(tabName, id, colName);
				cacheConfig[tabName][id].Add(colName, templateData);
				return templateData;
			}
			string templateData2 = GameEntry.Lua.GetTemplateData(tabName, id, colName);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(colName, templateData2);
			cacheConfig[tabName].Add(id, dictionary);
			return templateData2;
		}
		string templateData3 = GameEntry.Lua.GetTemplateData(tabName, id, colName);
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		dictionary2.Add(colName, templateData3);
		Dictionary<int, Dictionary<string, string>> dictionary3 = new Dictionary<int, Dictionary<string, string>>();
		dictionary3.Add(id, dictionary2);
		cacheConfig.Add(tabName, dictionary3);
		return templateData3;
	}

	public string TryGetTemplateData(string tabName, int id, string colName)
	{
		if (cacheConfig.ContainsKey(tabName) && cacheConfig[tabName].ContainsKey(id) && cacheConfig[tabName][id].ContainsKey(colName))
		{
			return cacheConfig[tabName][id][colName];
		}
		return string.Empty;
	}

	public void UpdateTemplateData(string tabName, int id, string colName, string value)
	{
		if (cacheConfig.ContainsKey(tabName))
		{
			if (cacheConfig[tabName].ContainsKey(id))
			{
				cacheConfig[tabName][id][colName] = value;
				return;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add(colName, value);
			cacheConfig[tabName].Add(id, dictionary);
		}
		else
		{
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			dictionary2.Add(colName, value);
			Dictionary<int, Dictionary<string, string>> dictionary3 = new Dictionary<int, Dictionary<string, string>>();
			dictionary3.Add(id, dictionary2);
			cacheConfig.Add(tabName, dictionary3);
		}
	}

	public void UpdateTemplateData(string tabName, int id, LuaTable rowData)
	{
		Dictionary<int, Dictionary<string, string>> dictionary = null;
		Dictionary<string, string> dictRow = null;
		if (cacheConfig.ContainsKey(tabName))
		{
			dictionary = cacheConfig[tabName];
			if (dictionary.ContainsKey(id))
			{
				dictRow = dictionary[id];
			}
			else
			{
				dictRow = new Dictionary<string, string>();
				dictionary.Add(id, dictRow);
			}
		}
		else
		{
			dictRow = new Dictionary<string, string>();
			dictionary = new Dictionary<int, Dictionary<string, string>>();
			dictionary.Add(id, dictRow);
			cacheConfig.Add(tabName, dictionary);
		}
		rowData?.ForEach(delegate(string key, string value)
		{
			dictRow[key] = value;
		});
	}

	public void reset()
	{
		cacheConfig.Clear();
	}
}
