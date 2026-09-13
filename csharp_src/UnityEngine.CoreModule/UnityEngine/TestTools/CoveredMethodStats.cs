using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine.Bindings;

namespace UnityEngine.TestTools;

[NativeType(CodegenOptions.Custom, "ManagedCoveredMethodStats", Header = "Runtime/Scripting/ScriptingCoverage.bindings.h")]
public struct CoveredMethodStats
{
	public MethodBase method;

	public int totalSequencePoints;

	public int uncoveredSequencePoints;

	private string GetTypeDisplayName(Type t)
	{
		if ((object)t == typeof(int))
		{
			return "int";
		}
		if ((object)t == typeof(bool))
		{
			return "bool";
		}
		if ((object)t == typeof(float))
		{
			return "float";
		}
		if ((object)t == typeof(double))
		{
			return "double";
		}
		if ((object)t == typeof(void))
		{
			return "void";
		}
		if ((object)t == typeof(string))
		{
			return "string";
		}
		if (t.IsGenericType && (object)t.GetGenericTypeDefinition() == typeof(List<>))
		{
			return "System.Collections.Generic.List<" + GetTypeDisplayName(t.GetGenericArguments()[0]) + ">";
		}
		if (t.IsArray && t.GetArrayRank() == 1)
		{
			return GetTypeDisplayName(t.GetElementType()) + "[]";
		}
		return t.FullName;
	}

	public override string ToString()
	{
		if ((object)method == null)
		{
			return "<no method>";
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(GetTypeDisplayName(method.DeclaringType));
		stringBuilder.Append(".");
		stringBuilder.Append(method.Name);
		stringBuilder.Append("(");
		bool flag = false;
		ParameterInfo[] parameters = method.GetParameters();
		foreach (ParameterInfo parameterInfo in parameters)
		{
			if (flag)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append(GetTypeDisplayName(parameterInfo.ParameterType));
			stringBuilder.Append(" ");
			stringBuilder.Append(parameterInfo.Name);
			flag = true;
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}
}
