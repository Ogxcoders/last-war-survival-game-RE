namespace System.Reflection;

public static class TypeExtensions
{
	public static ConstructorInfo GetConstructor(Type type, Type[] types)
	{
		Requires.NotNull(type, "type");
		return type.GetConstructor(types);
	}

	public static ConstructorInfo[] GetConstructors(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetConstructors();
	}

	public static ConstructorInfo[] GetConstructors(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetConstructors(bindingAttr);
	}

	public static MemberInfo[] GetDefaultMembers(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetDefaultMembers();
	}

	public static EventInfo GetEvent(Type type, string name)
	{
		Requires.NotNull(type, "type");
		return type.GetEvent(name);
	}

	public static EventInfo GetEvent(Type type, string name, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetEvent(name, bindingAttr);
	}

	public static EventInfo[] GetEvents(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetEvents();
	}

	public static EventInfo[] GetEvents(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetEvents(bindingAttr);
	}

	public static FieldInfo GetField(Type type, string name)
	{
		Requires.NotNull(type, "type");
		return type.GetField(name);
	}

	public static FieldInfo GetField(Type type, string name, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetField(name, bindingAttr);
	}

	public static FieldInfo[] GetFields(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetFields();
	}

	public static FieldInfo[] GetFields(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetFields(bindingAttr);
	}

	public static Type[] GetGenericArguments(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetGenericArguments();
	}

	public static Type[] GetInterfaces(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetInterfaces();
	}

	public static MemberInfo[] GetMember(Type type, string name)
	{
		Requires.NotNull(type, "type");
		return type.GetMember(name);
	}

	public static MemberInfo[] GetMember(Type type, string name, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetMember(name, bindingAttr);
	}

	public static MemberInfo[] GetMembers(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetMembers();
	}

	public static MemberInfo[] GetMembers(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetMembers(bindingAttr);
	}

	public static MethodInfo GetMethod(Type type, string name)
	{
		Requires.NotNull(type, "type");
		return type.GetMethod(name);
	}

	public static MethodInfo GetMethod(Type type, string name, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetMethod(name, bindingAttr);
	}

	public static MethodInfo GetMethod(Type type, string name, Type[] types)
	{
		Requires.NotNull(type, "type");
		return type.GetMethod(name, types);
	}

	public static MethodInfo[] GetMethods(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetMethods();
	}

	public static MethodInfo[] GetMethods(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetMethods(bindingAttr);
	}

	public static Type GetNestedType(Type type, string name, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetNestedType(name, bindingAttr);
	}

	public static Type[] GetNestedTypes(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetNestedTypes(bindingAttr);
	}

	public static PropertyInfo[] GetProperties(Type type)
	{
		Requires.NotNull(type, "type");
		return type.GetProperties();
	}

	public static PropertyInfo[] GetProperties(Type type, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetProperties(bindingAttr);
	}

	public static PropertyInfo GetProperty(Type type, string name)
	{
		Requires.NotNull(type, "type");
		return type.GetProperty(name);
	}

	public static PropertyInfo GetProperty(Type type, string name, BindingFlags bindingAttr)
	{
		Requires.NotNull(type, "type");
		return type.GetProperty(name, bindingAttr);
	}

	public static PropertyInfo GetProperty(Type type, string name, Type returnType)
	{
		Requires.NotNull(type, "type");
		return type.GetProperty(name, returnType);
	}

	public static PropertyInfo GetProperty(Type type, string name, Type returnType, Type[] types)
	{
		Requires.NotNull(type, "type");
		return type.GetProperty(name, returnType, types);
	}

	public static bool IsAssignableFrom(Type type, Type c)
	{
		Requires.NotNull(type, "type");
		return type.IsAssignableFrom(c);
	}

	public static bool IsInstanceOfType(Type type, object o)
	{
		Requires.NotNull(type, "type");
		return type.IsInstanceOfType(o);
	}
}
