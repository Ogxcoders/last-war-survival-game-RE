using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class ExposedVariablesEnumerator : IEnumerator<MemberInfo>, IEnumerator, IDisposable, IEnumerable<MemberInfo>, IEnumerable
{
	private int index;

	private readonly MemberInfo[] variables;

	private readonly List<VariableSet> hiddenVariables;

	private readonly List<VariableSet> exposedVariables;

	private readonly RuntimeInspector.VariableVisibility fieldVisibility;

	private readonly RuntimeInspector.VariableVisibility propertyVisibility;

	public MemberInfo Current => variables[index];

	object IEnumerator.Current => variables[index];

	public ExposedVariablesEnumerator(MemberInfo[] variables, List<VariableSet> hiddenVariables, List<VariableSet> exposedVariables, RuntimeInspector.VariableVisibility fieldVisibility, RuntimeInspector.VariableVisibility propertyVisibility)
	{
		index = -1;
		this.variables = variables;
		this.hiddenVariables = hiddenVariables;
		this.exposedVariables = exposedVariables;
		this.fieldVisibility = fieldVisibility;
		this.propertyVisibility = propertyVisibility;
	}

	public void Dispose()
	{
	}

	public IEnumerator<MemberInfo> GetEnumerator()
	{
		return this;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this;
	}

	public bool MoveNext()
	{
		if (variables == null)
		{
			return false;
		}
		while (++index < variables.Length)
		{
			if (ShouldExposeVariable(variables[index]))
			{
				return true;
			}
		}
		return false;
	}

	public void Reset()
	{
		index = -1;
	}

	private bool ShouldExposeVariable(MemberInfo variable)
	{
		string name = variable.Name;
		if (exposedVariables != null)
		{
			for (int i = 0; i < exposedVariables.Count; i++)
			{
				if (exposedVariables[i].variables.Contains(name))
				{
					return true;
				}
			}
		}
		if (hiddenVariables != null)
		{
			for (int j = 0; j < hiddenVariables.Count; j++)
			{
				if (hiddenVariables[j].variables.Contains(name))
				{
					return false;
				}
			}
		}
		if (variable is FieldInfo)
		{
			switch (fieldVisibility)
			{
			case RuntimeInspector.VariableVisibility.None:
				return false;
			case RuntimeInspector.VariableVisibility.All:
				return true;
			case RuntimeInspector.VariableVisibility.SerializableOnly:
			{
				FieldInfo fieldInfo = (FieldInfo)variable;
				if (!fieldInfo.IsPublic)
				{
					return fieldInfo.HasAttribute<SerializeField>();
				}
				return true;
			}
			}
		}
		else
		{
			switch (propertyVisibility)
			{
			case RuntimeInspector.VariableVisibility.None:
				return false;
			case RuntimeInspector.VariableVisibility.All:
				return true;
			case RuntimeInspector.VariableVisibility.SerializableOnly:
			{
				PropertyInfo propertyInfo = (PropertyInfo)variable;
				if (!propertyInfo.GetGetMethod(nonPublic: true).IsPublic)
				{
					return propertyInfo.HasAttribute<SerializeField>();
				}
				return true;
			}
			}
		}
		return true;
	}
}
