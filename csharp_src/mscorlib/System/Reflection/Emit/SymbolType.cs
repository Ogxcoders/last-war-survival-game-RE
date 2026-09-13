using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal abstract class SymbolType : TypeInfo
{
	internal Type m_baseType;

	public override Guid GUID
	{
		get
		{
			throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
		}
	}

	public override Module Module
	{
		get
		{
			Type baseType = m_baseType;
			while (baseType is SymbolType)
			{
				baseType = ((SymbolType)baseType).m_baseType;
			}
			return baseType.Module;
		}
	}

	public override Assembly Assembly
	{
		get
		{
			Type baseType = m_baseType;
			while (baseType is SymbolType)
			{
				baseType = ((SymbolType)baseType).m_baseType;
			}
			return baseType.Assembly;
		}
	}

	public override RuntimeTypeHandle TypeHandle
	{
		get
		{
			throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
		}
	}

	public override string Namespace => m_baseType.Namespace;

	public override Type BaseType => typeof(Array);

	public override bool IsConstructedGenericType => false;

	public override string AssemblyQualifiedName
	{
		get
		{
			string text = FormatName(m_baseType.FullName);
			if (text == null)
			{
				return null;
			}
			return text + ", " + m_baseType.Assembly.FullName;
		}
	}

	public override string FullName => FormatName(m_baseType.FullName);

	public override string Name => FormatName(m_baseType.Name);

	public override Type UnderlyingSystemType => this;

	internal override bool IsUserType => m_baseType.IsUserType;

	public override bool IsAssignableFrom(TypeInfo typeInfo)
	{
		if (typeInfo == null)
		{
			return false;
		}
		return IsAssignableFrom(typeInfo.AsType());
	}

	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	[ComVisible(true)]
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override Type GetInterface(string name, bool ignoreCase)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override Type[] GetInterfaces()
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override EventInfo[] GetEvents()
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	[ComVisible(true)]
	public override InterfaceMapping GetInterfaceMap(Type interfaceType)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		Type baseType = m_baseType;
		while (baseType is SymbolType)
		{
			baseType = ((SymbolType)baseType).m_baseType;
		}
		return baseType.Attributes;
	}

	protected override bool IsPrimitiveImpl()
	{
		return false;
	}

	protected override bool IsValueTypeImpl()
	{
		return false;
	}

	protected override bool IsCOMObjectImpl()
	{
		return false;
	}

	public override Type GetElementType()
	{
		return m_baseType;
	}

	protected override bool HasElementTypeImpl()
	{
		return m_baseType != null;
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw new NotSupportedException(Environment.GetResourceString("Not supported in a non-reflected type."));
	}

	internal SymbolType(Type elementType)
	{
		m_baseType = elementType;
	}

	internal abstract string FormatName(string elementName);

	protected override bool IsArrayImpl()
	{
		return false;
	}

	protected override bool IsByRefImpl()
	{
		return false;
	}

	protected override bool IsPointerImpl()
	{
		return false;
	}

	public override Type MakeArrayType()
	{
		return new ArrayType(this, 0);
	}

	public override Type MakeArrayType(int rank)
	{
		if (rank < 1)
		{
			throw new IndexOutOfRangeException();
		}
		return new ArrayType(this, rank);
	}

	public override Type MakeByRefType()
	{
		return new ByRefType(this);
	}

	public override Type MakePointerType()
	{
		return new PointerType(this);
	}

	public override string ToString()
	{
		return FormatName(m_baseType.ToString());
	}

	internal override Type RuntimeResolve()
	{
		return InternalResolve();
	}
}
