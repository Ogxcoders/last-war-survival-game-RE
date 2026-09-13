using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Joker;

public class UtilType
{
	private static ConcurrentDictionary<Type, Func<object>> _constructCache = new ConcurrentDictionary<Type, Func<object>>();

	private static ConcurrentDictionary<FieldInfo, Action<object, object>> _fieldSetters = new ConcurrentDictionary<FieldInfo, Action<object, object>>();

	public static object CreateInstance(Type type)
	{
		if (!_constructCache.TryGetValue(type, out var value))
		{
			value = Expression.Lambda<Func<object>>(Expression.New(type.GetConstructor(Type.EmptyTypes)), Array.Empty<ParameterExpression>()).Compile();
			_constructCache[type] = value;
		}
		return value();
	}

	private static Action<object, object> _CreateSetter(FieldInfo field)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object));
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object));
		UnaryExpression expression = Expression.Convert(parameterExpression, field.DeclaringType);
		return Expression.Lambda<Action<object, object>>(Expression.Assign(right: Expression.Convert(parameterExpression2, field.FieldType), left: Expression.Field(expression, field)), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
	}

	public static Action<object, object> GetOrCreateSetter(FieldInfo field)
	{
		if (!_fieldSetters.TryGetValue(field, out var value))
		{
			value = _CreateSetter(field);
			_fieldSetters[field] = value;
		}
		return value;
	}

	public static T CreateCollection<T>(Type elementType) where T : IList
	{
		return (T)CreateInstance(elementType);
	}

	public static Array CreateArray(Type elementType, int size)
	{
		return Array.CreateInstance(elementType, size);
	}

	public static bool IsArrayOrGenericList(FieldInfo field)
	{
		return IsArrayOrGenericList(field.FieldType);
	}

	public static bool IsArrayOrGenericList(PropertyInfo prop)
	{
		return IsArrayOrGenericList(prop.PropertyType);
	}

	public static bool IsGenericList(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			return true;
		}
		return false;
	}

	public static bool IsArrayOrGenericList(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (type.IsArray)
		{
			return true;
		}
		return IsGenericList(type);
	}

	public static bool IsDictionary(FieldInfo field)
	{
		return IsDictionary(field.FieldType);
	}

	public static bool IsDictionary(PropertyInfo prop)
	{
		return IsDictionary(prop.PropertyType);
	}

	public static bool IsDictionary(Type type)
	{
		if (type == null)
		{
			return false;
		}
		if (type.IsGenericType && typeof(IDictionary<, >).IsAssignableFrom(type.GetGenericTypeDefinition()))
		{
			return true;
		}
		if (typeof(IDictionary).IsAssignableFrom(type))
		{
			return true;
		}
		return false;
	}

	public static bool IsPrimitiveType(Type type)
	{
		if (!type.IsPrimitive && !(type == typeof(string)))
		{
			return type == typeof(decimal);
		}
		return true;
	}

	public static bool IsComposedOfPrimitiveType(Type type)
	{
		if (type == null)
		{
			return true;
		}
		if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			if (!IsPrimitiveType(elementType))
			{
				return IsComposedOfPrimitiveType(elementType);
			}
			return true;
		}
		if (IsDictionary(type))
		{
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (!IsPrimitiveType(genericArguments[i]))
				{
					return false;
				}
			}
			return true;
		}
		if (IsGenericList(type))
		{
			Type type2 = type.GetGenericArguments()[0];
			if (!IsPrimitiveType(type2))
			{
				return IsComposedOfPrimitiveType(type2);
			}
			return true;
		}
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
		for (int i = 0; i < fields.Length; i++)
		{
			if (!IsPrimitiveType(fields[i].FieldType))
			{
				return false;
			}
		}
		return true;
	}

	public static List<FieldInfo> GetFieldInfo(Type type)
	{
		List<FieldInfo> list = new List<FieldInfo>();
		FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
		list.AddRange(fields);
		while (type.BaseType != null)
		{
			type = type.BaseType;
			fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			list.InsertRange(0, fields);
		}
		return list;
	}

	public static Action<object, object>[] CreateSetters(Type type, List<FieldInfo> fields)
	{
		Action<object, object>[] array = new Action<object, object>[fields.Count];
		for (int i = 0; i < fields.Count; i++)
		{
			Action<object, object> orCreateSetter = GetOrCreateSetter(fields[i]);
			array[i] = orCreateSetter;
		}
		return array;
	}

	public static IDictionary CreateDictionary(Type[] types)
	{
		return CreateInstance(typeof(Dictionary<, >).MakeGenericType(types[0], types[1])) as IDictionary;
	}
}
