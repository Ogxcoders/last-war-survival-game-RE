using System;

namespace UnityEngine.TextCore;

[Serializable]
internal class TextStyle
{
	[SerializeField]
	private string m_Name;

	[SerializeField]
	private int m_HashCode;

	[SerializeField]
	private string m_OpeningDefinition = string.Empty;

	[SerializeField]
	private string m_ClosingDefinition = string.Empty;

	[SerializeField]
	private int[] m_OpeningTagArray;

	[SerializeField]
	private int[] m_ClosingTagArray;

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

	public void RefreshStyle()
	{
		m_HashCode = TextUtilities.GetHashCodeCaseInSensitive(m_Name);
		m_OpeningTagArray = new int[m_OpeningDefinition.Length];
		for (int i = 0; i < m_OpeningDefinition.Length; i++)
		{
			m_OpeningTagArray[i] = m_OpeningDefinition[i];
		}
		m_ClosingTagArray = new int[m_ClosingDefinition.Length];
		for (int j = 0; j < m_ClosingDefinition.Length; j++)
		{
			m_ClosingTagArray[j] = m_ClosingDefinition[j];
		}
	}
}
