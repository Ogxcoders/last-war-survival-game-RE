using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro;

[Serializable]
[ExcludeFromPreset]
public class TMP_StyleSheet : ScriptableObject
{
	[SerializeField]
	private List<TMP_Style> m_StyleList = new List<TMP_Style>(1);

	private Dictionary<int, TMP_Style> m_StyleLookupDictionary;

	internal List<TMP_Style> styles => m_StyleList;

	private void Reset()
	{
		LoadStyleDictionaryInternal();
	}

	public TMP_Style GetStyle(int hashCode)
	{
		if (m_StyleLookupDictionary == null)
		{
			LoadStyleDictionaryInternal();
		}
		if (m_StyleLookupDictionary.TryGetValue(hashCode, out var value))
		{
			return value;
		}
		return null;
	}

	public TMP_Style GetStyle(string name)
	{
		if (m_StyleLookupDictionary == null)
		{
			LoadStyleDictionaryInternal();
		}
		int hashCode = TMP_TextParsingUtilities.GetHashCode(name);
		if (m_StyleLookupDictionary.TryGetValue(hashCode, out var value))
		{
			return value;
		}
		return null;
	}

	public void RefreshStyles()
	{
		LoadStyleDictionaryInternal();
	}

	private void LoadStyleDictionaryInternal()
	{
		if (m_StyleLookupDictionary == null)
		{
			m_StyleLookupDictionary = new Dictionary<int, TMP_Style>();
		}
		else
		{
			m_StyleLookupDictionary.Clear();
		}
		for (int i = 0; i < m_StyleList.Count; i++)
		{
			m_StyleList[i].RefreshStyle();
			if (!m_StyleLookupDictionary.ContainsKey(m_StyleList[i].hashCode))
			{
				m_StyleLookupDictionary.Add(m_StyleList[i].hashCode, m_StyleList[i]);
			}
		}
		int hashCode = TMP_TextParsingUtilities.GetHashCode("Normal");
		if (!m_StyleLookupDictionary.ContainsKey(hashCode))
		{
			TMP_Style tMP_Style = new TMP_Style("Normal", string.Empty, string.Empty);
			m_StyleList.Add(tMP_Style);
			m_StyleLookupDictionary.Add(hashCode, tMP_Style);
		}
	}
}
