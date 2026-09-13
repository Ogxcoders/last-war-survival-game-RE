using System.Collections.Generic;

namespace System.Reflection;

public abstract class TypeInfo : Type, IReflectableType
{
	private const BindingFlags DeclaredOnlyLookup = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	public virtual Type[] GenericTypeParameters
	{
		get
		{
			if (!IsGenericTypeDefinition)
			{
				return Type.EmptyTypes;
			}
			return GetGenericArguments();
		}
	}

	public virtual IEnumerable<ConstructorInfo> DeclaredConstructors => GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

	public virtual IEnumerable<EventInfo> DeclaredEvents => GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

	public virtual IEnumerable<FieldInfo> DeclaredFields => GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

	public virtual IEnumerable<MemberInfo> DeclaredMembers => GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

	public virtual IEnumerable<MethodInfo> DeclaredMethods => GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

	public virtual IEnumerable<TypeInfo> DeclaredNestedTypes
	{
		get
		{
			Type[] nestedTypes = GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (Type type in nestedTypes)
			{
				yield return type.GetTypeInfo();
			}
		}
	}

	public virtual IEnumerable<PropertyInfo> DeclaredProperties => GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

	public virtual IEnumerable<Type> ImplementedInterfaces => GetInterfaces();

	TypeInfo IReflectableType.GetTypeInfo()
	{
		return this;
	}

	public virtual Type AsType()
	{
		return this;
	}

	public virtual EventInfo GetDeclaredEvent(string name)
	{
		return GetEvent(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public virtual FieldInfo GetDeclaredField(string name)
	{
		return GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public virtual MethodInfo GetDeclaredMethod(string name)
	{
		return GetMethod(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public virtual TypeInfo GetDeclaredNestedType(string name)
	{
		return GetNestedType(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.GetTypeInfo();
	}

	public virtual PropertyInfo GetDeclaredProperty(string name)
	{
		return GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	public virtual IEnumerable<MethodInfo> GetDeclaredMethods(string name)
	{
		MethodInfo[] methods = GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (MethodInfo methodInfo in methods)
		{
			if (methodInfo.Name == name)
			{
				yield return methodInfo;
			}
		}
	}

	public virtual bool IsAssignableFrom(TypeInfo typeInfo)
	{
		if (typeInfo == null)
		{
			return false;
		}
		if (this == typeInfo)
		{
			return true;
		}
		if (typeInfo.IsSubclassOf(this))
		{
			return true;
		}
		if (base.IsInterface)
		{
			return typeInfo.ImplementInterface(this);
		}
		if (IsGenericParameter)
		{
			Type[] genericParameterConstraints = GetGenericParameterConstraints();
			for (int i = 0; i < genericParameterConstraints.Length; i++)
			{
				if (!genericParameterConstraints[i].IsAssignableFrom(typeInfo))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}
}
