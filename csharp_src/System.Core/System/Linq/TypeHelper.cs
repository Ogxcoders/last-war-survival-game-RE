using System.Collections.Generic;
using System.Reflection;

namespace System.Linq;

internal static class TypeHelper
{
	internal static Type FindGenericType(Type definition, Type type)
	{
		bool? flag = null;
		while (type != null && type != typeof(object))
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == definition)
			{
				return type;
			}
			if (!flag.HasValue)
			{
				flag = definition.IsInterface;
			}
			if (flag == true)
			{
				Type[] interfaces = type.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					Type type3 = FindGenericType(definition, type2);
					if (type3 != null)
					{
						return type3;
					}
				}
			}
			type = type.BaseType;
		}
		return null;
	}

	internal static IEnumerable<MethodInfo> GetStaticMethods(this Type type)
	{
		return from m in type.GetRuntimeMethods()
			where m.IsStatic
			select m;
	}
}
