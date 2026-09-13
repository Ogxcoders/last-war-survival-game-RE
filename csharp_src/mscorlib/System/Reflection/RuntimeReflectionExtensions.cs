using System.Collections.Generic;

namespace System.Reflection;

public static class RuntimeReflectionExtensions
{
	private const BindingFlags Everything = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	public static IEnumerable<FieldInfo> GetRuntimeFields(this Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public static IEnumerable<MethodInfo> GetRuntimeMethods(this Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public static IEnumerable<PropertyInfo> GetRuntimeProperties(this Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public static IEnumerable<EventInfo> GetRuntimeEvents(this Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public static FieldInfo GetRuntimeField(this Type type, string name)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetField(name);
	}

	public static MethodInfo GetRuntimeMethod(this Type type, string name, Type[] parameters)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetMethod(name, parameters);
	}

	public static PropertyInfo GetRuntimeProperty(this Type type, string name)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetProperty(name);
	}

	public static EventInfo GetRuntimeEvent(this Type type, string name)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		return type.GetEvent(name);
	}

	public static MethodInfo GetRuntimeBaseDefinition(this MethodInfo method)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		return method.GetBaseDefinition();
	}

	public static InterfaceMapping GetRuntimeInterfaceMap(this TypeInfo typeInfo, Type interfaceType)
	{
		if (typeInfo == null)
		{
			throw new ArgumentNullException("typeInfo");
		}
		return typeInfo.GetInterfaceMap(interfaceType);
	}

	public static MethodInfo GetMethodInfo(this Delegate del)
	{
		if ((object)del == null)
		{
			throw new ArgumentNullException("del");
		}
		return del.Method;
	}
}
