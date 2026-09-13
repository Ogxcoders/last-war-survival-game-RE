using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;

namespace Microsoft.Internal;

internal static class ReflectionServices
{
	public static Assembly Assembly(this MemberInfo member)
	{
		Type type = member as Type;
		if (type != null)
		{
			return type.Assembly;
		}
		return member.DeclaringType.Assembly;
	}

	public static bool IsVisible(this ConstructorInfo constructor)
	{
		if (constructor.DeclaringType.IsVisible)
		{
			return constructor.IsPublic;
		}
		return false;
	}

	public static bool IsVisible(this FieldInfo field)
	{
		if (field.DeclaringType.IsVisible)
		{
			return field.IsPublic;
		}
		return false;
	}

	public static bool IsVisible(this MethodInfo method)
	{
		if (!method.DeclaringType.IsVisible)
		{
			return false;
		}
		if (!method.IsPublic)
		{
			return false;
		}
		if (method.IsGenericMethod)
		{
			Type[] genericArguments = method.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (!genericArguments[i].IsVisible)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static string GetDisplayName(Type declaringType, string name)
	{
		Assumes.NotNull(declaringType);
		return declaringType.GetDisplayName() + "." + name;
	}

	public static string GetDisplayName(this MemberInfo member)
	{
		Assumes.NotNull(member);
		MemberTypes memberType = member.MemberType;
		if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
		{
			return AttributedModelServices.GetTypeIdentity((Type)member);
		}
		return GetDisplayName(member.DeclaringType, member.Name);
	}

	internal static bool TryGetGenericInterfaceType(Type instanceType, Type targetOpenInterfaceType, out Type targetClosedInterfaceType)
	{
		Assumes.IsTrue(targetOpenInterfaceType.IsInterface);
		Assumes.IsTrue(targetOpenInterfaceType.IsGenericTypeDefinition);
		Assumes.IsTrue(!instanceType.IsGenericTypeDefinition);
		if (instanceType.IsInterface && instanceType.IsGenericType && instanceType.UnderlyingSystemType.GetGenericTypeDefinition() == targetOpenInterfaceType.UnderlyingSystemType)
		{
			targetClosedInterfaceType = instanceType;
			return true;
		}
		try
		{
			Type type = instanceType.GetInterface(targetOpenInterfaceType.Name, ignoreCase: false);
			if (type != null && type.UnderlyingSystemType.GetGenericTypeDefinition() == targetOpenInterfaceType.UnderlyingSystemType)
			{
				targetClosedInterfaceType = type;
				return true;
			}
		}
		catch (AmbiguousMatchException)
		{
		}
		targetClosedInterfaceType = null;
		return false;
	}

	internal static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
	{
		return type.GetInterfaces().Concat(new Type[1] { type }).SelectMany((Type itf) => itf.GetProperties());
	}

	internal static IEnumerable<MethodInfo> GetAllMethods(this Type type)
	{
		IEnumerable<MethodInfo> declaredMethods = type.GetDeclaredMethods();
		Type baseType = type.BaseType;
		if (baseType.UnderlyingSystemType != typeof(object))
		{
			return declaredMethods.Concat(baseType.GetAllMethods());
		}
		return declaredMethods;
	}

	private static IEnumerable<MethodInfo> GetDeclaredMethods(this Type type)
	{
		MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < methods.Length; i++)
		{
			yield return methods[i];
		}
	}

	public static IEnumerable<FieldInfo> GetAllFields(this Type type)
	{
		IEnumerable<FieldInfo> declaredFields = type.GetDeclaredFields();
		Type baseType = type.BaseType;
		if (baseType.UnderlyingSystemType != typeof(object))
		{
			return declaredFields.Concat(baseType.GetAllFields());
		}
		return declaredFields;
	}

	private static IEnumerable<FieldInfo> GetDeclaredFields(this Type type)
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		for (int i = 0; i < fields.Length; i++)
		{
			yield return fields[i];
		}
	}
}
