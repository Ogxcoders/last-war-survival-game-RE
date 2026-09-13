using System.Collections.Generic;
using System.Text;

namespace UnityEngine.UIElements.StyleSheets;

internal class StylePropertyValueParser
{
	private string m_PropertyValue;

	private List<string> m_ValueList = new List<string>();

	private StringBuilder m_StringBuilder = new StringBuilder();

	private int m_ParseIndex = 0;

	public string[] Parse(string propertyValue)
	{
		m_PropertyValue = propertyValue;
		m_ValueList.Clear();
		m_StringBuilder.Remove(0, m_StringBuilder.Length);
		for (m_ParseIndex = 0; m_ParseIndex < m_PropertyValue.Length; m_ParseIndex++)
		{
			char c = m_PropertyValue[m_ParseIndex];
			switch (c)
			{
			case ' ':
				EatSpace();
				AddValuePart();
				break;
			case ',':
				EatSpace();
				AddValuePart();
				m_ValueList.Add(",");
				break;
			case '(':
				AppendFunction();
				break;
			default:
				m_StringBuilder.Append(c);
				break;
			}
		}
		string text = m_StringBuilder.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			m_ValueList.Add(text);
		}
		return m_ValueList.ToArray();
	}

	private void AddValuePart()
	{
		string item = m_StringBuilder.ToString();
		m_StringBuilder.Remove(0, m_StringBuilder.Length);
		m_ValueList.Add(item);
	}

	private void AppendFunction()
	{
		while (m_ParseIndex < m_PropertyValue.Length && m_PropertyValue[m_ParseIndex] != ')')
		{
			m_StringBuilder.Append(m_PropertyValue[m_ParseIndex]);
			m_ParseIndex++;
		}
		m_StringBuilder.Append(m_PropertyValue[m_ParseIndex]);
	}

	private void EatSpace()
	{
		while (m_ParseIndex + 1 < m_PropertyValue.Length && m_PropertyValue[m_ParseIndex + 1] == ' ')
		{
			m_ParseIndex++;
		}
	}
}
