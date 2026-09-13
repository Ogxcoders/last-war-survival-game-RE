using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
[ComVisible(true)]
public sealed class GenericTypeParameterBuilder : TypeInfo
{
	private TypeBuilder tbuilder;

	private MethodBuilder mbuilder;

	private string name;

	private int index;

	private Type base_type;

	private Type[] iface_constraints;

	private CustomAttributeBuilder[] cattrs;

	private GenericParameterAttributes attrs;

	public override Type UnderlyingSystemType => this;

	public override Assembly Assembly => tbuilder.Assembly;

	public override string AssemblyQualifiedName => null;

	public override Type BaseType => base_type;

	public override string FullName => null;

	public override Guid GUID
	{
		get
		{
			throw not_supported();
		}
	}

	public override string Name => name;

	public override string Namespace => null;

	public override Module Module => tbuilder.Module;

	public override Type DeclaringType
	{
		get
		{
			if (!(mbuilder != null))
			{
				return tbuilder;
			}
			return mbuilder.DeclaringType;
		}
	}

	public override Type ReflectedType => DeclaringType;

	public override RuntimeTypeHandle TypeHandle
	{
		get
		{
			throw not_supported();
		}
	}

	public override bool ContainsGenericParameters => true;

	public override bool IsGenericParameter => true;

	public override bool IsGenericType => false;

	public override bool IsGenericTypeDefinition => false;

	public override GenericParameterAttributes GenericParameterAttributes
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override int GenericParameterPosition => index;

	public override MethodBase DeclaringMethod => mbuilder;

	internal override bool IsUserType => false;

	public void SetBaseTypeConstraint(Type baseTypeConstraint)
	{
		base_type = baseTypeConstraint ?? typeof(object);
	}

	[ComVisible(true)]
	public void SetInterfaceConstraints(params Type[] interfaceConstraints)
	{
		iface_constraints = interfaceConstraints;
	}

	public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
	{
		attrs = genericParameterAttributes;
	}

	internal GenericTypeParameterBuilder(TypeBuilder tbuilder, MethodBuilder mbuilder, string name, int index)
	{
		this.tbuilder = tbuilder;
		this.mbuilder = mbuilder;
		this.name = name;
		this.index = index;
	}

	internal override Type InternalResolve()
	{
		if (mbuilder != null)
		{
			return MethodBase.GetMethodFromHandle(mbuilder.MethodHandleInternal, mbuilder.TypeBuilder.InternalResolve().TypeHandle).GetGenericArguments()[index];
		}
		return tbuilder.InternalResolve().GetGenericArguments()[index];
	}

	internal override Type RuntimeResolve()
	{
		if (mbuilder != null)
		{
			return MethodBase.GetMethodFromHandle(mbuilder.MethodHandleInternal, mbuilder.TypeBuilder.RuntimeResolve().TypeHandle).GetGenericArguments()[index];
		}
		return tbuilder.RuntimeResolve().GetGenericArguments()[index];
	}

	[ComVisible(true)]
	public override bool IsSubclassOf(Type c)
	{
		throw not_supported();
	}

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return TypeAttributes.Public;
	}

	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw not_supported();
	}

	[ComVisible(true)]
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override EventInfo[] GetEvents()
	{
		throw not_supported();
	}

	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override Type GetInterface(string name, bool ignoreCase)
	{
		throw not_supported();
	}

	public override Type[] GetInterfaces()
	{
		throw not_supported();
	}

	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw not_supported();
	}

	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		throw not_supported();
	}

	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		throw not_supported();
	}

	protected override bool HasElementTypeImpl()
	{
		return false;
	}

	public override bool IsAssignableFrom(Type c)
	{
		throw not_supported();
	}

	public override bool IsAssignableFrom(TypeInfo typeInfo)
	{
		if (typeInfo == null)
		{
			return false;
		}
		return IsAssignableFrom(typeInfo.AsType());
	}

	public override bool IsInstanceOfType(object o)
	{
		throw not_supported();
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
		if (!(base_type != null))
		{
			return false;
		}
		return base_type.IsValueType;
	}

	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		throw not_supported();
	}

	public override Type GetElementType()
	{
		throw not_supported();
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw not_supported();
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw not_supported();
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw not_supported();
	}

	[ComVisible(true)]
	public override InterfaceMapping GetInterfaceMap(Type interfaceType)
	{
		throw not_supported();
	}

	public override Type[] GetGenericArguments()
	{
		throw new InvalidOperationException();
	}

	public override Type GetGenericTypeDefinition()
	{
		throw new InvalidOperationException();
	}

	public override Type[] GetGenericParameterConstraints()
	{
		throw new InvalidOperationException();
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		if (customBuilder == null)
		{
			throw new ArgumentNullException("customBuilder");
		}
		if (cattrs != null)
		{
			CustomAttributeBuilder[] array = new CustomAttributeBuilder[cattrs.Length + 1];
			cattrs.CopyTo(array, 0);
			array[cattrs.Length] = customBuilder;
			cattrs = array;
		}
		else
		{
			cattrs = new CustomAttributeBuilder[1];
			cattrs[0] = customBuilder;
		}
	}

	[MonoTODO("unverified implementation")]
	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
		SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
	}

	private Exception not_supported()
	{
		return new NotSupportedException();
	}

	public override string ToString()
	{
		return name;
	}

	[MonoTODO]
	public override bool Equals(object o)
	{
		return base.Equals(o);
	}

	[MonoTODO]
	public override int GetHashCode()
	{
		return base.GetHashCode();
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

	public override Type MakeGenericType(params Type[] typeArguments)
	{
		throw new InvalidOperationException(Environment.GetResourceString("{0} is not a GenericTypeDefinition. MakeGenericType may only be called on a type for which Type.IsGenericTypeDefinition is true."));
	}

	public override Type MakePointerType()
	{
		return new PointerType(this);
	}
}
