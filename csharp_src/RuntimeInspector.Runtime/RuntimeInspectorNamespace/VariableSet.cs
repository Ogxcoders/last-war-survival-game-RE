using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RuntimeInspectorNamespace;

[Serializable]
public class VariableSet
{
	private const string INCLUDE_ALL_VARIABLES = "*";

	[SerializeField]
	private string m_type;

	public Type type;

	[SerializeField]
	private string[] m_variables;

	public HashSet<string> variables;

	public bool Init()
	{
		type = RuntimeInspectorUtils.GetType(m_type);
		if (type == null)
		{
			return false;
		}
		variables = new HashSet<string>();
		for (int i = 0; i < m_variables.Length; i++)
		{
			if (m_variables[i] != "*")
			{
				variables.Add(m_variables[i]);
				continue;
			}
			AddAllVariablesToSet();
			break;
		}
		return true;
	}

	private void AddAllVariablesToSet()
	{
		MemberInfo[] allVariables = type.GetAllVariables();
		if (allVariables != null)
		{
			for (int i = 0; i < allVariables.Length; i++)
			{
				variables.Add(allVariables[i].Name);
			}
		}
	}
}
