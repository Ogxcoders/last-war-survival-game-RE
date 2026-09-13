using System;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_Style
{
	internal static TMP_Style k_NormalStyle;

	[SerializeField]
	private string m_Name;

	[SerializeField]
	private int m_HashCode;

	[SerializeField]
	private string m_OpeningDefinition;

	[SerializeField]
	private string m_ClosingDefinition;

	[SerializeField]
	private int[] m_OpeningTagArray;

	[SerializeField]
	private int[] m_ClosingTagArray;

	[SerializeField]
	internal uint[] m_OpeningTagUnicodeArray;

	[SerializeField]
	internal uint[] m_ClosingTagUnicodeArray;

	public static TMP_Style NormalStyle
	{
		get
		{
			if (k_NormalStyle == null)
			{
				k_NormalStyle = new TMP_Style("Normal", string.Empty, string.Empty);
			}
			return k_NormalStyle;
		}
	}

	public string name
	{
		get
		{
			return m_Name;
		}
		set
		{
			if (value != m_Name)
			{
				m_Name = value;
			}
		}
	}

	public int hashCode
	{
		get
		{
			return m_HashCode;
		}
		set
		{
			if (value != m_HashCode)
			{
				m_HashCode = value;
			}
		}
	}

	public string styleOpeningDefinition => m_OpeningDefinition;

	public string styleClosingDefinition => m_ClosingDefinition;

	public int[] styleOpeningTagArray => m_OpeningTagArray;

	public int[] styleClosingTagArray => m_ClosingTagArray;

	internal TMP_Style(string styleName, string styleOpeningDefinition, string styleClosingDefinition)
	{
		m_Name = styleName;
		m_HashCode = TMP_TextParsingUtilities.GetHashCode(styleName);
		m_OpeningDefinition = styleOpeningDefinition;
		m_ClosingDefinition = styleClosingDefinition;
		RefreshStyle();
	}

	public void RefreshStyle()
	{
		m_HashCode = TMP_TextParsingUtilities.GetHashCode(m_Name);
		int length = m_OpeningDefinition.Length;
		m_OpeningTagArray = new int[length];
		m_OpeningTagUnicodeArray = new uint[length];
		for (int i = 0; i < length; i++)
		{
			m_OpeningTagArray[i] = m_OpeningDefinition[i];
			m_OpeningTagUnicodeArray[i] = m_OpeningDefinition[i];
		}
		int length2 = m_ClosingDefinition.Length;
		m_ClosingTagArray = new int[length2];
		m_ClosingTagUnicodeArray = new uint[length2];
		for (int j = 0; j < length2; j++)
		{
			m_ClosingTagArray[j] = m_ClosingDefinition[j];
			m_ClosingTagUnicodeArray[j] = m_ClosingDefinition[j];
		}
	}
}
