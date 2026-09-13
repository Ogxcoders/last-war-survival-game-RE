using System;

namespace NiceJson;

[Serializable]
public class JsonBasic : JsonNode
{
	private object m_value;

	public object ValueObject => m_value;

	public JsonBasic(object value)
	{
		m_value = value;
	}

	public override string ToString()
	{
		return m_value.ToString();
	}

	public override string ToJsonString()
	{
		if (m_value == null)
		{
			return "null";
		}
		if (m_value is string)
		{
			return "\"" + JsonNode.EscapeString(m_value.ToString()) + "\"";
		}
		if (m_value is bool)
		{
			if ((bool)m_value)
			{
				return "true";
			}
			return "false";
		}
		return m_value.ToString();
	}
}
