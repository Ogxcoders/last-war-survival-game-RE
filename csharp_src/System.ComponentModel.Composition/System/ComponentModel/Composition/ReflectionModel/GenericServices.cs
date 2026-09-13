using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel;

internal static class GenericServices
{
	internal static IList<Type> GetPureGenericParameters(this Type type)
	{
		Assumes.NotNull(type);
		if (type.IsGenericType && type.ContainsGenericParameters)
		{
			List<Type> pureGenericParameters = new List<Type>();
			TraverseGenericType(type, delegate(Type t)
			{
				if (t.IsGenericParameter)
				{
					pureGenericParameters.Add(t);
				}
			});
			return pureGenericParameters;
		}
		return Type.EmptyTypes;
	}

	internal static int GetPureGenericArity(this Type type)
	{
		Assumes.NotNull(type);
		int genericArity = 0;
		if (type.IsGenericType && type.ContainsGenericParameters)
		{
			new List<Type>();
			TraverseGenericType(type, delegate(Type t)
			{
				if (t.IsGenericParameter)
				{
					genericArity++;
				}
			});
		}
		return genericArity;
	}

	private static void TraverseGenericType(Type type, Action<Type> onType)
	{
		if (type.IsGenericType)
		{
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				TraverseGenericType(genericArguments[i], onType);
			}
		}
		onType(type);
	}

	public static int[] GetGenericParametersOrder(Type type)
	{
		return (from parameter in type.GetPureGenericParameters()
			select parameter.GenericParameterPosition).ToArray();
	}

	public static string GetGenericName(string originalGenericName, int[] genericParametersOrder, int genericArity)
	{
		string[] array = new string[genericArity];
		for (int i = 0; i < genericParametersOrder.Length; i++)
		{
			array[genericParametersOrder[i]] = string.Format(CultureInfo.InvariantCulture, "{{{0}}}", i);
		}
		CultureInfo invariantCulture = CultureInfo.InvariantCulture;
		object[] args = array;
		return string.Format(invariantCulture, originalGenericName, args);
	}

	public static T[] Reorder<T>(T[] original, int[] genericParametersOrder)
	{
		T[] array = new T[genericParametersOrder.Length];
		for (int i = 0; i < genericParametersOrder.Length; i++)
		{
			array[i] = original[genericParametersOrder[i]];
		}
		return array;
	}

	public static IEnumerable<Type> CreateTypeSpecializations(this Type[] types, Type[] specializationTypes)
	{
		return types?.Select((Type type) => type.CreateTypeSpecialization(specializationTypes));
	}

	public static Type CreateTypeSpecialization(this Type type, Type[] specializationTypes)
	{
		if (!type.ContainsGenericParameters)
		{
			return type;
		}
		if (type.IsGenericParameter)
		{
			return specializationTypes[type.GenericParameterPosition];
		}
		Type[] genericArguments = type.GetGenericArguments();
		Type[] array = new Type[genericArguments.Length];
		for (int i = 0; i < genericArguments.Length; i++)
		{
			Type type2 = genericArguments[i];
			array[i] = (type2.IsGenericParameter ? specializationTypes[type2.GenericParameterPosition] : type2);
		}
		return type.GetGenericTypeDefinition().MakeGenericType(array);
	}

	public static bool CanSpecialize(Type type, IEnumerable<Type> constraints, GenericParameterAttributes attributes)
	{
		if (CanSpecialize(type, constraints))
		{
			return CanSpecialize(type, attributes);
		}
		return false;
	}

	public static bool CanSpecialize(Type type, IEnumerable<Type> constraintTypes)
	{
		if (constraintTypes == null)
		{
			return true;
		}
		foreach (Type constraintType in constraintTypes)
		{
			if (constraintType != null && !constraintType.IsAssignableFrom(type))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanSpecialize(Type type, GenericParameterAttributes attributes)
	{
		if (attributes == GenericParameterAttributes.None)
		{
			return true;
		}
		if ((attributes & GenericParameterAttributes.ReferenceTypeConstraint) != GenericParameterAttributes.None && type.IsValueType)
		{
			return false;
		}
		if ((attributes & GenericParameterAttributes.DefaultConstructorConstraint) != GenericParameterAttributes.None && !type.IsValueType && type.GetConstructor(Type.EmptyTypes) == null)
		{
			return false;
		}
		if ((attributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.None)
		{
			if (!type.IsValueType)
			{
				return false;
			}
			if (Nullable.GetUnderlyingType(type) != null)
			{
				return false;
			}
		}
		return true;
	}
}
