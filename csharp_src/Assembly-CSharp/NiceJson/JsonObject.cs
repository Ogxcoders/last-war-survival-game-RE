using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NiceJson;

[Serializable]
public class JsonObject : JsonNode, IEnumerable
{
	private Dictionary<string, JsonNode> m_dictionary = new Dictionary<string, JsonNode>();

	public Dictionary<string, JsonNode>.KeyCollection Keys => m_dictionary.Keys;

	public Dictionary<string, JsonNode>.ValueCollection Values => m_dictionary.Values;

	public new JsonNode this[string key]
	{
		get
		{
			return m_dictionary[key];
		}
		set
		{
			m_dictionary[key] = value;
		}
	}

	public int Count => m_dictionary.Count;

	public void Add(string key, JsonNode value)
	{
		m_dictionary.Add(key, value);
	}

	public bool Remove(string key)
	{
		return m_dictionary.Remove(key);
	}

	public new bool ContainsKey(string key)
	{
		return m_dictionary.ContainsKey(key);
	}

	public bool ContainsValue(JsonNode value)
	{
		return m_dictionary.ContainsValue(value);
	}

	public void Clear()
	{
		m_dictionary.Clear();
	}

	public IEnumerator GetEnumerator()
	{
		foreach (KeyValuePair<string, JsonNode> item in m_dictionary)
		{
			yield return item;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public override string ToJsonString()
	{
		if (m_dictionary == null)
		{
			return "null";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('{');
		foreach (string key in m_dictionary.Keys)
		{
			stringBuilder.Append('"');
			stringBuilder.Append(JsonNode.EscapeString(key));
			stringBuilder.Append('"');
			stringBuilder.Append(':');
			if (m_dictionary[key] != null)
			{
				stringBuilder.Append(m_dictionary[key].ToJsonString());
			}
			else
			{
				stringBuilder.Append("null");
			}
			stringBuilder.Append(',');
		}
		if (stringBuilder[stringBuilder.Length - 1] == ',')
		{
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
		}
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}
}
