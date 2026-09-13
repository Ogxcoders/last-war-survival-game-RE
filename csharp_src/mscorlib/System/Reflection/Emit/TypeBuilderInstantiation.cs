using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal sealed class TypeBuilderInstantiation : TypeInfo
{
	internal Type generic_type;

	private Type[] type_arguments;

	private Hashtable fields;

	private Hashtable ctors;

	private Hashtable methods;

	private const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	internal bool IsCreated
	{
		get
		{
			TypeBuilder typeBuilder = generic_type as TypeBuilder;
			if (!(typeBuilder != null))
			{
				return true;
			}
			return typeBuilder.is_created;
		}
	}

	public override Type BaseType => generic_type.BaseType;

	public override Type UnderlyingSystemType => this;

	public override Assembly Assembly => generic_type.Assembly;

	public override Module Module => generic_type.Module;

	public override string Name => generic_type.Name;

	public override string Namespace => generic_type.Namespace;

	public override string FullName => format_name(full_name: true, assembly_qualified: false);

	public override string AssemblyQualifiedName => format_name(full_name: true, assembly_qualified: true);

	public override Guid GUID
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override bool ContainsGenericParameters
	{
		get
		{
			Type[] array = type_arguments;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].ContainsGenericParameters)
				{
					return true;
				}
			}
			return false;
		}
	}

	public override bool IsGenericTypeDefinition => false;

	public override bool IsGenericType => true;

	public override Type DeclaringType => generic_type.DeclaringType;

	public override RuntimeTypeHandle TypeHandle
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	internal override bool IsUserType
	{
		get
		{
			Type[] array = type_arguments;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].IsUserType)
				{
					return true;
				}
			}
			return false;
		}
	}

	public override bool IsTypeDefinition => false;

	public override bool IsConstructedGenericType => true;

	internal TypeBuilderInstantiation()
	{
		throw new InvalidOperationException();
	}

	internal TypeBuilderInstantiation(Type tb, Type[] args)
	{
		generic_type = tb;
		type_arguments = args;
	}

	internal override Type InternalResolve()
	{
		Type type = generic_type.InternalResolve();
		Type[] array = new Type[type_arguments.Length];
		for (int i = 0; i < type_arguments.Length; i++)
		{
			array[i] = type_arguments[i].InternalResolve();
		}
		return type.MakeGenericType(array);
	}

	internal override Type RuntimeResolve()
	{
		if (generic_type is TypeBuilder typeBuilder && !typeBuilder.IsCreated())
		{
			AppDomain.CurrentDomain.DoTypeBuilderResolve(typeBuilder);
		}
		for (int i = 0; i < type_arguments.Length; i++)
		{
			if (type_arguments[i] is TypeBuilder typeBuilder2 && !typeBuilder2.IsCreated())
			{
				AppDomain.CurrentDomain.DoTypeBuilderResolve(typeBuilder2);
			}
		}
		return InternalResolve();
	}

	private Type GetParentType()
	{
		return InflateType(generic_type.BaseType);
	}

	internal Type InflateType(Type type)
	{
		return InflateType(type, type_arguments, null);
	}

	internal Type InflateType(Type type, Type[] method_args)
	{
		return InflateType(type, type_arguments, method_args);
	}

	internal static Type InflateType(Type type, Type[] type_args, Type[] method_args)
	{
		if (type == null)
		{
			return null;
		}
		if (!type.IsGenericParameter && !type.ContainsGenericParameters)
		{
			return type;
		}
		if (type.IsGenericParameter)
		{
			if (type.DeclaringMethod == null)
			{
				if (type_args != null)
				{
					return type_args[type.GenericParameterPosition];
				}
				return type;
			}
			if (method_args != null)
			{
				return method_args[type.GenericParameterPosition];
			}
			return type;
		}
		if (type.IsPointer)
		{
			return InflateType(type.GetElementType(), type_args, method_args).MakePointerType();
		}
		if (type.IsByRef)
		{
			return InflateType(type.GetElementType(), type_args, method_args).MakeByRefType();
		}
		if (type.IsArray)
		{
			if (type.GetArrayRank() > 1)
			{
				return InflateType(type.GetElementType(), type_args, method_args).MakeArrayType(type.GetArrayRank());
			}
			if (type.ToString().EndsWith("[*]", StringComparison.Ordinal))
			{
				return InflateType(type.GetElementType(), type_args, method_args).MakeArrayType(1);
			}
			return InflateType(type.GetElementType(), type_args, method_args).MakeArrayType();
		}
		Type[] genericArguments = type.GetGenericArguments();
		for (int i = 0; i < genericArguments.Length; i++)
		{
			genericArguments[i] = InflateType(genericArguments[i], type_args, method_args);
		}
		return (type.IsGenericTypeDefinition ? type : type.GetGenericTypeDefinition()).MakeGenericType(genericArguments);
	}

	public override Type[] GetInterfaces()
	{
		throw new NotSupportedException();
	}

	protected override bool IsValueTypeImpl()
	{
		return generic_type.IsValueType;
	}

	internal override MethodInfo GetMethod(MethodInfo fromNoninstanciated)
	{
		if (methods == null)
		{
			methods = new Hashtable();
		}
		if (!methods.ContainsKey(fromNoninstanciated))
		{
			methods[fromNoninstanciated] = new MethodOnTypeBuilderInst(this, fromNoninstanciated);
		}
		return (MethodInfo)methods[fromNoninstanciated];
	}

	internal override ConstructorInfo GetConstructor(ConstructorInfo fromNoninstanciated)
	{
		if (ctors == null)
		{
			ctors = new Hashtable();
		}
		if (!ctors.ContainsKey(fromNoninstanciated))
		{
			ctors[fromNoninstanciated] = new ConstructorOnTypeBuilderInst(this, fromNoninstanciated);
		}
		return (ConstructorInfo)ctors[fromNoninstanciated];
	}

	internal override FieldInfo GetField(FieldInfo fromNoninstanciated)
	{
		if (fields == null)
		{
			fields = new Hashtable();
		}
		if (!fields.ContainsKey(fromNoninstanciated))
		{
			fields[fromNoninstanciated] = new FieldOnTypeBuilderInst(this, fromNoninstanciated);
		}
		return (FieldInfo)fields[fromNoninstanciated];
	}

	public override MethodInfo[] GetMethods(BindingFlags bf)
	{
		throw new NotSupportedException();
	}

	public override ConstructorInfo[] GetConstructors(BindingFlags bf)
	{
		throw new NotSupportedException();
	}

	public override FieldInfo[] GetFields(BindingFlags bf)
	{
		throw new NotSupportedException();
	}

	public override PropertyInfo[] GetProperties(BindingFlags bf)
	{
		throw new NotSupportedException();
	}

	public override EventInfo[] GetEvents(BindingFlags bf)
	{
		throw new NotSupportedException();
	}

	public override Type[] GetNestedTypes(BindingFlags bf)
	{
		throw new NotSupportedException();
	}

	public override bool IsAssignableFrom(Type c)
	{
		throw new NotSupportedException();
	}

	private string format_name(bool full_name, bool assembly_qualified)
	{
		StringBuilder stringBuilder = new StringBuilder(generic_type.FullName);
		stringBuilder.Append("[");
		for (int i = 0; i < type_arguments.Length; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			string text;
			if (full_name)
			{
				string fullName = type_arguments[i].Assembly.FullName;
				text = type_arguments[i].FullName;
				if (text != null && fullName != null)
				{
					text = text + ", " + fullName;
				}
			}
			else
			{
				text = type_arguments[i].ToString();
			}
			if (text == null)
			{
				return null;
			}
			if (full_name)
			{
				stringBuilder.Append("[");
			}
			stringBuilder.Append(text);
			if (full_name)
			{
				stringBuilder.Append("]");
			}
		}
		stringBuilder.Append("]");
		if (assembly_qualified)
		{
			stringBuilder.Append(", ");
			stringBuilder.Append(generic_type.Assembly.FullName);
		}
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		return format_name(full_name: false, assembly_qualified: false);
	}

	public override Type GetGenericTypeDefinition()
	{
		return generic_type;
	}

	public override Type[] GetGenericArguments()
	{
		Type[] array = new Type[type_arguments.Length];
		type_arguments.CopyTo(array, 0);
		return array;
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

	public override Type GetElementType()
	{
		throw new NotSupportedException();
	}

	protected override bool HasElementTypeImpl()
	{
		return false;
	}

	protected override bool IsCOMObjectImpl()
	{
		return false;
	}

	protected override bool IsPrimitiveImpl()
	{
		return false;
	}

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

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return generic_type.Attributes;
	}

	public override Type GetInterface(string name, bool ignoreCase)
	{
		throw new NotSupportedException();
	}

	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		throw new NotSupportedException();
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		throw new NotSupportedException();
	}

	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		throw new NotSupportedException();
	}

	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		throw new NotSupportedException();
	}

	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		throw new NotSupportedException();
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw new NotSupportedException();
	}

	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		throw new NotSupportedException();
	}

	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		throw new NotSupportedException();
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw new NotSupportedException();
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		if (IsCreated)
		{
			return generic_type.GetCustomAttributes(inherit);
		}
		throw new NotSupportedException();
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		if (IsCreated)
		{
			return generic_type.GetCustomAttributes(attributeType, inherit);
		}
		throw new NotSupportedException();
	}

	internal static Type MakeGenericType(Type type, Type[] typeArguments)
	{
		return new TypeBuilderInstantiation(type, typeArguments);
	}
}
