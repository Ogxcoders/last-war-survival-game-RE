using System;
using System.Collections.Generic;

namespace UnityEngine.TextCore;

[Serializable]
internal class TextStyleSheet : ScriptableObject
{
	private static TextStyleSheet s_Instance;

	[SerializeField]
	private List<TextStyle> m_StyleList = new List<TextStyle>(1);

	private Dictionary<int, TextStyle> m_StyleDictionary = new Dictionary<int, TextStyle>();

	public static TextStyleSheet instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = TextSettings.defaultStyleSheet;
				if (s_Instance == null)
				{
					return null;
				}
				s_Instance.LoadStyleDictionaryInternal();
			}
			return s_Instance;
		}
	}

	public static TextStyleSheet LoadDefaultStyleSheet()
	{
		s_Instance = null;
		return instance;
	}

	public static TextStyle GetStyle(int hashCode)
	{
		if (instance == null)
		{
			return null;
		}
		return instance.GetStyleInternal(hashCode);
	}

	private TextStyle GetStyleInternal(int hashCode)
	{
		if (m_StyleDictionary.TryGetValue(hashCode, out var value))
		{
			return value;
		}
		return null;
	}

	public void UpdateStyleDictionaryKey(int old_key, int new_key)
	{
		if (m_StyleDictionary.ContainsKey(old_key))
		{
			TextStyle value = m_StyleDictionary[old_key];
			m_StyleDictionary.Add(new_key, value);
			m_StyleDictionary.Remove(old_key);
		}
	}

	public static void RefreshStyles()
	{
		instance.LoadStyleDictionaryInternal();
	}

	private void LoadStyleDictionaryInternal()
	{
		m_StyleDictionary.Clear();
		for (int i = 0; i < m_StyleList.Count; i++)
		{
			m_StyleList[i].RefreshStyle();
			if (!m_StyleDictionary.ContainsKey(m_StyleList[i].hashCode))
			{
				m_StyleDictionary.Add(m_StyleList[i].hashCode, m_StyleList[i]);
			}
		}
	}
}
