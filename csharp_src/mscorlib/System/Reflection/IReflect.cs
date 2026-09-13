using System.Globalization;

namespace System.Reflection;

public interface IReflect
{
	Type UnderlyingSystemType { get; }

	MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers);

	MethodInfo GetMethod(string name, BindingFlags bindingAttr);

	MethodInfo[] GetMethods(BindingFlags bindingAttr);

	FieldInfo GetField(string name, BindingFlags bindingAttr);

	FieldInfo[] GetFields(BindingFlags bindingAttr);

	PropertyInfo GetProperty(string name, BindingFlags bindingAttr);

	PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers);

	PropertyInfo[] GetProperties(BindingFlags bindingAttr);

	MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

	MemberInfo[] GetMembers(BindingFlags bindingAttr);

	object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters);
}
