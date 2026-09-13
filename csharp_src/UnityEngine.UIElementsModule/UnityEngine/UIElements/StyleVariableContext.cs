#define UNITY_ASSERTIONS
using System.Collections.Generic;

namespace UnityEngine.UIElements;

internal class StyleVariableContext
{
	public static readonly StyleVariableContext none = new StyleVariableContext();

	private int m_VariableHash;

	private List<StyleVariable> m_Variables;

	private List<int> m_SortedHash;

	public void Add(StyleVariable sv)
	{
		int hashCode = sv.GetHashCode();
		int num = m_SortedHash.BinarySearch(hashCode);
		if (num < 0)
		{
			m_SortedHash.Insert(~num, hashCode);
			m_Variables.Add(sv);
			m_VariableHash = ((m_Variables.Count == 0) ? sv.GetHashCode() : ((m_VariableHash * 397) ^ sv.GetHashCode()));
		}
	}

	public void AddInitialRange(StyleVariableContext other)
	{
		if (other.m_Variables.Count > 0)
		{
			Debug.Assert(m_Variables.Count == 0);
			m_VariableHash = other.m_VariableHash;
			m_Variables.AddRange(other.m_Variables);
			m_SortedHash.AddRange(other.m_SortedHash);
		}
	}

	public void Clear()
	{
		if (m_Variables.Count > 0)
		{
			m_Variables.Clear();
			m_VariableHash = 0;
			m_SortedHash.Clear();
		}
	}

	public StyleVariableContext()
	{
		m_Variables = new List<StyleVariable>();
		m_VariableHash = 0;
		m_SortedHash = new List<int>();
	}

	public StyleVariableContext(StyleVariableContext other)
	{
		m_Variables = new List<StyleVariable>(other.m_Variables);
		m_VariableHash = other.m_VariableHash;
		m_SortedHash = new List<int>(other.m_SortedHash);
	}

	public bool TryFindVariable(string name, out StyleVariable v)
	{
		for (int num = m_Variables.Count - 1; num >= 0; num--)
		{
			if (m_Variables[num].name == name)
			{
				v = m_Variables[num];
				return true;
			}
		}
		v = default(StyleVariable);
		return false;
	}

	public int GetVariableHash()
	{
		return m_VariableHash;
	}
}
