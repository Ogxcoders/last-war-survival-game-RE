using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NiceJson;

[Serializable]
public class JsonArray : JsonNode, IEnumerable<JsonNode>, IEnumerable
{
	private List<JsonNode> m_list = new List<JsonNode>();

	public int Count => m_list.Count;

	public new JsonNode this[int index]
	{
		get
		{
			return m_list[index];
		}
		set
		{
			m_list[index] = value;
		}
	}

	public IEnumerator<JsonNode> GetEnumerator()
	{
		foreach (JsonNode item in m_list)
		{
			yield return item;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public void Add(JsonNode item)
	{
		m_list.Add(item);
	}

	public void AddRange(IEnumerable<JsonNode> collection)
	{
		m_list.AddRange(collection);
	}

	public void Insert(int index, JsonNode item)
	{
		m_list.Insert(index, item);
	}

	public void InsertRange(int index, IEnumerable<JsonNode> collection)
	{
		m_list.InsertRange(index, collection);
	}

	public void RemoveAt(int index)
	{
		m_list.RemoveAt(index);
	}

	public bool Remove(JsonNode item)
	{
		return m_list.Remove(item);
	}

	public void Clear()
	{
		m_list.Clear();
	}

	public override string ToJsonString()
	{
		if (m_list == null)
		{
			return "null";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append('[');
		foreach (JsonNode item in m_list)
		{
			if (item != null)
			{
				stringBuilder.Append(item.ToJsonString());
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
		stringBuilder.Append(']');
		return stringBuilder.ToString();
	}
}
