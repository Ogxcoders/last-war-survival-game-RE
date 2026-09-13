using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework;

public static class AssemblyUtil
{
	private static readonly Assembly[] s_Assemblies;

	private static readonly Dictionary<string, Type> s_CachedTypes;

	static AssemblyUtil()
	{
		s_Assemblies = null;
		s_CachedTypes = new Dictionary<string, Type>();
		s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
	}

	public static Assembly[] GetAssemblies()
	{
		return s_Assemblies;
	}

	public static Type[] GetTypes()
	{
		List<Type> list = new List<Type>();
		for (int i = 0; i < s_Assemblies.Length; i++)
		{
			list.AddRange(s_Assemblies[i].GetTypes());
		}
		return list.ToArray();
	}

	public static Type GetType(string typeName)
	{
		if (string.IsNullOrEmpty(typeName))
		{
			throw new GameFrameworkException("Type name is invalid.");
		}
		Type value = null;
		if (s_CachedTypes.TryGetValue(typeName, out value))
		{
			return value;
		}
		value = Type.GetType(typeName);
		if (value != null)
		{
			s_CachedTypes.Add(typeName, value);
			return value;
		}
		Assembly[] array = s_Assemblies;
		foreach (Assembly assembly in array)
		{
			value = Type.GetType($"{typeName}, {assembly.FullName}");
			if (value != null)
			{
				s_CachedTypes.Add(typeName, value);
				return value;
			}
		}
		return null;
	}
}
