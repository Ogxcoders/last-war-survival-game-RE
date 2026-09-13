using System;
using System.Reflection;

namespace Google.Protobuf.Compatibility;

internal static class TypeExtensions
{
	internal static bool IsAssignableFrom(this Type target, Type c)
	{
		return target.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
	}

	internal static PropertyInfo GetProperty(this Type target, string name)
	{
		while (target != null)
		{
			TypeInfo typeInfo = target.GetTypeInfo();
			PropertyInfo declaredProperty = typeInfo.GetDeclaredProperty(name);
			if (declaredProperty != null && ((declaredProperty.CanRead && declaredProperty.GetMethod.IsPublic) || (declaredProperty.CanWrite && declaredProperty.SetMethod.IsPublic)))
			{
				return declaredProperty;
			}
			target = typeInfo.BaseType;
		}
		return null;
	}

	internal static MethodInfo GetMethod(this Type target, string name)
	{
		while (target != null)
		{
			TypeInfo typeInfo = target.GetTypeInfo();
			MethodInfo declaredMethod = typeInfo.GetDeclaredMethod(name);
			if (declaredMethod != null && declaredMethod.IsPublic)
			{
				return declaredMethod;
			}
			target = typeInfo.BaseType;
		}
		return null;
	}
}
