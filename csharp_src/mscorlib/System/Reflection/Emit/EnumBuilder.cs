using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

public sealed class EnumBuilder : TypeInfo
{
	private TypeBuilder _tb;

	private FieldBuilder _underlyingField;

	private Type _underlyingType;

	public override Assembly Assembly => _tb.Assembly;

	public override string AssemblyQualifiedName => _tb.AssemblyQualifiedName;

	public override Type BaseType => _tb.BaseType;

	public override Type DeclaringType => _tb.DeclaringType;

	public override string FullName => _tb.FullName;

	public override Guid GUID => _tb.GUID;

	public override Module Module => _tb.Module;

	public override string Name => _tb.Name;

	public override string Namespace => _tb.Namespace;

	public override Type ReflectedType => _tb.ReflectedType;

	public override RuntimeTypeHandle TypeHandle => _tb.TypeHandle;

	public TypeToken TypeToken => _tb.TypeToken;

	public FieldBuilder UnderlyingField => _underlyingField;

	public override Type UnderlyingSystemType => _underlyingType;

	internal override bool IsUserType => false;

	public override bool IsConstructedGenericType => false;

	public override bool IsTypeDefinition => true;

	internal EnumBuilder(ModuleBuilder mb, string name, TypeAttributes visibility, Type underlyingType)
	{
		_tb = new TypeBuilder(mb, name, visibility | TypeAttributes.Sealed, typeof(Enum), null, PackingSize.Unspecified, 0, null);
		_underlyingType = underlyingType;
		_underlyingField = _tb.DefineField("value__", underlyingType, FieldAttributes.Private | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
		setup_enum_type(_tb);
	}

	internal TypeBuilder GetTypeBuilder()
	{
		return _tb;
	}

	internal override Type InternalResolve()
	{
		return _tb.InternalResolve();
	}

	internal override Type RuntimeResolve()
	{
		return _tb.RuntimeResolve();
	}

	public Type CreateType()
	{
		return _tb.CreateType();
	}

	public TypeInfo CreateTypeInfo()
	{
		return _tb.CreateTypeInfo();
	}

	public override Type GetEnumUnderlyingType()
	{
		return _underlyingType;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void setup_enum_type(Type t);

	public FieldBuilder DefineLiteral(string literalName, object literalValue)
	{
		FieldBuilder fieldBuilder = _tb.DefineField(literalName, this, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
		fieldBuilder.SetConstant(literalValue);
		return fieldBuilder;
	}

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return _tb.attrs;
	}

	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		return _tb.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
	}

	[ComVisible(true)]
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		return _tb.GetConstructors(bindingAttr);
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return _tb.GetCustomAttributes(inherit);
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return _tb.GetCustomAttributes(attributeType, inherit);
	}

	public override Type GetElementType()
	{
		return _tb.GetElementType();
	}

	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		return _tb.GetEvent(name, bindingAttr);
	}

	public override EventInfo[] GetEvents()
	{
		return _tb.GetEvents();
	}

	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		return _tb.GetEvents(bindingAttr);
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		return _tb.GetField(name, bindingAttr);
	}

	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		return _tb.GetFields(bindingAttr);
	}

	public override Type GetInterface(string name, bool ignoreCase)
	{
		return _tb.GetInterface(name, ignoreCase);
	}

	[ComVisible(true)]
	public override InterfaceMapping GetInterfaceMap(Type interfaceType)
	{
		return _tb.GetInterfaceMap(interfaceType);
	}

	public override Type[] GetInterfaces()
	{
		return _tb.GetInterfaces();
	}

	public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
	{
		return _tb.GetMember(name, type, bindingAttr);
	}

	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		return _tb.GetMembers(bindingAttr);
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		if (types == null)
		{
			return _tb.GetMethod(name, bindingAttr);
		}
		return _tb.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
	}

	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		return _tb.GetMethods(bindingAttr);
	}

	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		return _tb.GetNestedType(name, bindingAttr);
	}

	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		return _tb.GetNestedTypes(bindingAttr);
	}

	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		return _tb.GetProperties(bindingAttr);
	}

	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		throw CreateNotSupportedException();
	}

	protected override bool HasElementTypeImpl()
	{
		return _tb.HasElementType;
	}

	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		return _tb.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
	}

	protected override bool IsArrayImpl()
	{
		return false;
	}

	protected override bool IsByRefImpl()
	{
		return false;
	}

	protected override bool IsCOMObjectImpl()
	{
		return false;
	}

	protected override bool IsPointerImpl()
	{
		return false;
	}

	protected override bool IsPrimitiveImpl()
	{
		return false;
	}

	protected override bool IsValueTypeImpl()
	{
		return true;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return _tb.IsDefined(attributeType, inherit);
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

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		_tb.SetCustomAttribute(customBuilder);
	}

	[ComVisible(true)]
	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
		SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
	}

	private Exception CreateNotSupportedException()
	{
		return new NotSupportedException("The invoked member is not supported in a dynamic module.");
	}

	public override bool IsAssignableFrom(TypeInfo typeInfo)
	{
		return base.IsAssignableFrom(typeInfo);
	}
}
