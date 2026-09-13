using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public sealed class TypeBuilder : TypeInfo
{
	private string tname;

	private string nspace;

	private Type parent;

	private Type nesting_type;

	internal Type[] interfaces;

	internal int num_methods;

	internal MethodBuilder[] methods;

	internal ConstructorBuilder[] ctors;

	internal PropertyBuilder[] properties;

	internal int num_fields;

	internal FieldBuilder[] fields;

	internal EventBuilder[] events;

	private CustomAttributeBuilder[] cattrs;

	internal TypeBuilder[] subtypes;

	internal TypeAttributes attrs;

	private int table_idx;

	private ModuleBuilder pmodule;

	private int class_size;

	private PackingSize packing_size;

	private IntPtr generic_container;

	private GenericTypeParameterBuilder[] generic_params;

	private RefEmitPermissionSet[] permissions;

	private TypeInfo created;

	private int state;

	private TypeName fullname;

	private bool createTypeCalled;

	private Type underlying_type;

	public const int UnspecifiedTypeSize = 0;

	public override Assembly Assembly => pmodule.Assembly;

	public override string AssemblyQualifiedName => fullname.DisplayName + ", " + Assembly.FullName;

	public override Type BaseType => parent;

	public override Type DeclaringType => nesting_type;

	public override Type UnderlyingSystemType
	{
		get
		{
			if (is_created)
			{
				return created.UnderlyingSystemType;
			}
			if (IsEnum)
			{
				if (underlying_type != null)
				{
					return underlying_type;
				}
				throw new InvalidOperationException("Enumeration type is not defined.");
			}
			return this;
		}
	}

	public override string FullName => fullname.DisplayName;

	public override Guid GUID
	{
		get
		{
			check_created();
			return created.GUID;
		}
	}

	public override Module Module => pmodule;

	public override string Name => tname;

	public override string Namespace => nspace;

	public PackingSize PackingSize => packing_size;

	public int Size => class_size;

	public override Type ReflectedType => nesting_type;

	public override RuntimeTypeHandle TypeHandle
	{
		get
		{
			check_created();
			return created.TypeHandle;
		}
	}

	public TypeToken TypeToken => new TypeToken(0x2000000 | table_idx);

	internal bool is_created => createTypeCalled;

	public override bool ContainsGenericParameters => generic_params != null;

	public override bool IsGenericParameter => false;

	public override GenericParameterAttributes GenericParameterAttributes => GenericParameterAttributes.None;

	public override bool IsGenericTypeDefinition => generic_params != null;

	public override bool IsGenericType => IsGenericTypeDefinition;

	[MonoTODO]
	public override int GenericParameterPosition => 0;

	public override MethodBase DeclaringMethod => null;

	internal override bool IsUserType => false;

	public override bool IsConstructedGenericType => false;

	public override bool IsTypeDefinition => true;

	protected override TypeAttributes GetAttributeFlagsImpl()
	{
		return attrs;
	}

	private TypeBuilder()
	{
		if (RuntimeType.MakeTypeBuilderInstantiation == null)
		{
			RuntimeType.MakeTypeBuilderInstantiation = TypeBuilderInstantiation.MakeGenericType;
		}
	}

	[PreserveDependency("DoTypeBuilderResolve", "System.AppDomain")]
	internal TypeBuilder(ModuleBuilder mb, TypeAttributes attr, int table_idx)
		: this()
	{
		parent = null;
		attrs = attr;
		class_size = 0;
		this.table_idx = table_idx;
		tname = ((table_idx == 1) ? "<Module>" : ("type_" + table_idx));
		nspace = string.Empty;
		fullname = TypeIdentifiers.WithoutEscape(tname);
		pmodule = mb;
	}

	internal TypeBuilder(ModuleBuilder mb, string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packing_size, int type_size, Type nesting_type)
		: this()
	{
		this.parent = ResolveUserType(parent);
		attrs = attr;
		class_size = type_size;
		this.packing_size = packing_size;
		this.nesting_type = nesting_type;
		check_name("fullname", name);
		if (parent == null && (attr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic && (attr & TypeAttributes.Abstract) == 0)
		{
			throw new InvalidOperationException("Interface must be declared abstract.");
		}
		int num = name.LastIndexOf('.');
		if (num != -1)
		{
			tname = name.Substring(num + 1);
			nspace = name.Substring(0, num);
		}
		else
		{
			tname = name;
			nspace = string.Empty;
		}
		if (interfaces != null)
		{
			this.interfaces = new Type[interfaces.Length];
			Array.Copy(interfaces, this.interfaces, interfaces.Length);
		}
		pmodule = mb;
		if ((attr & TypeAttributes.ClassSemanticsMask) == 0 && parent == null)
		{
			this.parent = typeof(object);
		}
		table_idx = mb.get_next_table_index(this, 2, 1);
		fullname = GetFullName();
	}

	[ComVisible(true)]
	public override bool IsSubclassOf(Type c)
	{
		if (c == null)
		{
			return false;
		}
		if (c == this)
		{
			return false;
		}
		Type baseType = parent;
		while (baseType != null)
		{
			if (c == baseType)
			{
				return true;
			}
			baseType = baseType.BaseType;
		}
		return false;
	}

	private TypeName GetFullName()
	{
		TypeIdentifier typeIdentifier = TypeIdentifiers.FromInternal(tname);
		if (nesting_type != null)
		{
			return TypeNames.FromDisplay(nesting_type.FullName).NestedName(typeIdentifier);
		}
		if (nspace != null && nspace.Length > 0)
		{
			return TypeIdentifiers.FromInternal(nspace, typeIdentifier);
		}
		return typeIdentifier;
	}

	public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
	{
	}

	[ComVisible(true)]
	public void AddInterfaceImplementation(Type interfaceType)
	{
		if (interfaceType == null)
		{
			throw new ArgumentNullException("interfaceType");
		}
		check_not_created();
		if (interfaces != null)
		{
			Type[] array = interfaces;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == interfaceType)
				{
					return;
				}
			}
			Type[] array2 = new Type[interfaces.Length + 1];
			interfaces.CopyTo(array2, 0);
			array2[interfaces.Length] = interfaceType;
			interfaces = array2;
		}
		else
		{
			interfaces = new Type[1];
			interfaces[0] = interfaceType;
		}
	}

	protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		check_created();
		if (created == typeof(object))
		{
			if (ctors == null)
			{
				return null;
			}
			ConstructorBuilder constructorBuilder = null;
			int num = 0;
			ConstructorBuilder[] array = ctors;
			foreach (ConstructorBuilder constructorBuilder2 in array)
			{
				if (callConvention == CallingConventions.Any || constructorBuilder2.CallingConvention == callConvention)
				{
					constructorBuilder = constructorBuilder2;
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			if (types == null)
			{
				if (num > 1)
				{
					throw new AmbiguousMatchException();
				}
				return constructorBuilder;
			}
			MethodBase[] array2 = new MethodBase[num];
			if (num == 1)
			{
				array2[0] = constructorBuilder;
			}
			else
			{
				num = 0;
				array = ctors;
				foreach (ConstructorInfo constructorInfo in array)
				{
					if (callConvention == CallingConventions.Any || constructorInfo.CallingConvention == callConvention)
					{
						array2[num++] = constructorInfo;
					}
				}
			}
			if (binder == null)
			{
				binder = Type.DefaultBinder;
			}
			return (ConstructorInfo)binder.SelectMethod(bindingAttr, array2, types, modifiers);
		}
		return created.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		if (!is_created)
		{
			throw new NotSupportedException();
		}
		return MonoCustomAttrs.IsDefined(this, attributeType, inherit);
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		check_created();
		return created.GetCustomAttributes(inherit);
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		check_created();
		return created.GetCustomAttributes(attributeType, inherit);
	}

	public TypeBuilder DefineNestedType(string name)
	{
		return DefineNestedType(name, TypeAttributes.NestedPrivate, pmodule.assemblyb.corlib_object_type, null);
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
	{
		return DefineNestedType(name, attr, pmodule.assemblyb.corlib_object_type, null);
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
	{
		return DefineNestedType(name, attr, parent, null);
	}

	private TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packSize, int typeSize)
	{
		if (interfaces != null)
		{
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (interfaces[i] == null)
				{
					throw new ArgumentNullException("interfaces");
				}
			}
		}
		TypeBuilder typeBuilder = new TypeBuilder(pmodule, name, attr, parent, interfaces, packSize, typeSize, this);
		typeBuilder.fullname = typeBuilder.GetFullName();
		pmodule.RegisterTypeName(typeBuilder, typeBuilder.fullname);
		if (subtypes != null)
		{
			TypeBuilder[] array = new TypeBuilder[subtypes.Length + 1];
			Array.Copy(subtypes, array, subtypes.Length);
			array[subtypes.Length] = typeBuilder;
			subtypes = array;
		}
		else
		{
			subtypes = new TypeBuilder[1];
			subtypes[0] = typeBuilder;
		}
		return typeBuilder;
	}

	[ComVisible(true)]
	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
	{
		return DefineNestedType(name, attr, parent, interfaces, PackingSize.Unspecified, 0);
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
	{
		return DefineNestedType(name, attr, parent, null, PackingSize.Unspecified, typeSize);
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize)
	{
		return DefineNestedType(name, attr, parent, null, packSize, 0);
	}

	public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, PackingSize packSize, int typeSize)
	{
		return DefineNestedType(name, attr, parent, null, packSize, typeSize);
	}

	[ComVisible(true)]
	public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes)
	{
		return DefineConstructor(attributes, callingConvention, parameterTypes, null, null);
	}

	[ComVisible(true)]
	public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
	{
		check_not_created();
		ConstructorBuilder constructorBuilder = new ConstructorBuilder(this, attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
		if (ctors != null)
		{
			ConstructorBuilder[] array = new ConstructorBuilder[ctors.Length + 1];
			Array.Copy(ctors, array, ctors.Length);
			array[ctors.Length] = constructorBuilder;
			ctors = array;
		}
		else
		{
			ctors = new ConstructorBuilder[1];
			ctors[0] = constructorBuilder;
		}
		return constructorBuilder;
	}

	[ComVisible(true)]
	public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
	{
		Type type = ((!(parent != null)) ? pmodule.assemblyb.corlib_object_type : parent);
		Type type2 = type;
		type = type.InternalResolve();
		if (type == typeof(object) || type == typeof(ValueType))
		{
			type = type2;
		}
		ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
		if (constructor == null)
		{
			throw new NotSupportedException("Parent does not have a default constructor. The default constructor must be explicitly defined.");
		}
		ConstructorBuilder constructorBuilder = DefineConstructor(attributes, CallingConventions.Standard, Type.EmptyTypes);
		ILGenerator iLGenerator = constructorBuilder.GetILGenerator();
		iLGenerator.Emit(OpCodes.Ldarg_0);
		iLGenerator.Emit(OpCodes.Call, constructor);
		iLGenerator.Emit(OpCodes.Ret);
		return constructorBuilder;
	}

	private void append_method(MethodBuilder mb)
	{
		if (methods != null)
		{
			if (methods.Length == num_methods)
			{
				MethodBuilder[] destinationArray = new MethodBuilder[methods.Length * 2];
				Array.Copy(methods, destinationArray, num_methods);
				methods = destinationArray;
			}
		}
		else
		{
			methods = new MethodBuilder[1];
		}
		methods[num_methods] = mb;
		num_methods++;
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
	{
		return DefineMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
	{
		return DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
	{
		check_name("name", name);
		check_not_created();
		if (base.IsInterface && ((attributes & MethodAttributes.Abstract) == 0 || (attributes & MethodAttributes.Virtual) == 0) && (attributes & MethodAttributes.Static) == 0)
		{
			throw new ArgumentException("Interface method must be abstract and virtual.");
		}
		if (returnType == null)
		{
			returnType = pmodule.assemblyb.corlib_void_type;
		}
		MethodBuilder methodBuilder = new MethodBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		append_method(methodBuilder);
		return methodBuilder;
	}

	public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		return DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
	}

	public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		check_name("name", name);
		check_name("dllName", dllName);
		check_name("entryName", entryName);
		if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
		{
			throw new ArgumentException("PInvoke methods must be static and native and cannot be abstract.");
		}
		if (base.IsInterface)
		{
			throw new ArgumentException("PInvoke methods cannot exist on interfaces.");
		}
		check_not_created();
		MethodBuilder methodBuilder = new MethodBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, dllName, entryName, nativeCallConv, nativeCharSet);
		append_method(methodBuilder);
		return methodBuilder;
	}

	public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		return DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes)
	{
		return DefineMethod(name, attributes, CallingConventions.Standard);
	}

	public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention)
	{
		return DefineMethod(name, attributes, callingConvention, null, null);
	}

	public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
	{
		if (methodInfoBody == null)
		{
			throw new ArgumentNullException("methodInfoBody");
		}
		if (methodInfoDeclaration == null)
		{
			throw new ArgumentNullException("methodInfoDeclaration");
		}
		check_not_created();
		if (methodInfoBody.DeclaringType != this)
		{
			throw new ArgumentException("method body must belong to this type");
		}
		if (methodInfoBody is MethodBuilder)
		{
			((MethodBuilder)methodInfoBody).set_override(methodInfoDeclaration);
		}
	}

	public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes)
	{
		return DefineField(fieldName, type, null, null, attributes);
	}

	public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
	{
		check_name("fieldName", fieldName);
		if (type == typeof(void))
		{
			throw new ArgumentException("Bad field type in defining field.");
		}
		check_not_created();
		FieldBuilder fieldBuilder = new FieldBuilder(this, fieldName, type, attributes, requiredCustomModifiers, optionalCustomModifiers);
		if (fields != null)
		{
			if (fields.Length == num_fields)
			{
				FieldBuilder[] destinationArray = new FieldBuilder[fields.Length * 2];
				Array.Copy(fields, destinationArray, num_fields);
				fields = destinationArray;
			}
			fields[num_fields] = fieldBuilder;
			num_fields++;
		}
		else
		{
			fields = new FieldBuilder[1];
			fields[0] = fieldBuilder;
			num_fields++;
		}
		if (IsEnum && underlying_type == null && (attributes & FieldAttributes.Static) == 0)
		{
			underlying_type = type;
		}
		return fieldBuilder;
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
	{
		return DefineProperty(name, attributes, (CallingConventions)0, returnType, null, null, parameterTypes, null, null);
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
	{
		return DefineProperty(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
	{
		return DefineProperty(name, attributes, (CallingConventions)0, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
	}

	public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
	{
		check_name("name", name);
		if (parameterTypes != null)
		{
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if (parameterTypes[i] == null)
				{
					throw new ArgumentNullException("parameterTypes");
				}
			}
		}
		check_not_created();
		PropertyBuilder propertyBuilder = new PropertyBuilder(this, name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
		if (properties != null)
		{
			Array.Resize(ref properties, properties.Length + 1);
			properties[properties.Length - 1] = propertyBuilder;
		}
		else
		{
			properties = new PropertyBuilder[1] { propertyBuilder };
		}
		return propertyBuilder;
	}

	[ComVisible(true)]
	public ConstructorBuilder DefineTypeInitializer()
	{
		return DefineConstructor(MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, null);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern TypeInfo create_runtime_class();

	private bool is_nested_in(Type t)
	{
		while (t != null)
		{
			if (t == this)
			{
				return true;
			}
			t = t.DeclaringType;
		}
		return false;
	}

	private bool has_ctor_method()
	{
		MethodAttributes methodAttributes = MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
		for (int i = 0; i < num_methods; i++)
		{
			MethodBuilder methodBuilder = methods[i];
			if (methodBuilder.Name == ConstructorInfo.ConstructorName && (methodBuilder.Attributes & methodAttributes) == methodAttributes)
			{
				return true;
			}
		}
		return false;
	}

	public Type CreateType()
	{
		return CreateTypeInfo();
	}

	public TypeInfo CreateTypeInfo()
	{
		if (createTypeCalled)
		{
			return created;
		}
		if (!base.IsInterface && parent == null && this != pmodule.assemblyb.corlib_object_type && FullName != "<Module>")
		{
			SetParent(pmodule.assemblyb.corlib_object_type);
		}
		if (fields != null)
		{
			FieldBuilder[] array = fields;
			foreach (FieldBuilder fieldBuilder in array)
			{
				if (fieldBuilder == null)
				{
					continue;
				}
				Type fieldType = fieldBuilder.FieldType;
				if (!fieldBuilder.IsStatic && fieldType is TypeBuilder && fieldType.IsValueType && fieldType != this && is_nested_in(fieldType))
				{
					TypeBuilder typeBuilder = (TypeBuilder)fieldType;
					if (!typeBuilder.is_created)
					{
						AppDomain.CurrentDomain.DoTypeBuilderResolve(typeBuilder);
						_ = typeBuilder.is_created;
					}
				}
			}
		}
		if (!base.IsInterface && !base.IsValueType && ctors == null && tname != "<Module>" && ((GetAttributeFlagsImpl() & TypeAttributes.Abstract) | TypeAttributes.Sealed) != (TypeAttributes.Abstract | TypeAttributes.Sealed) && !has_ctor_method())
		{
			DefineDefaultConstructor(MethodAttributes.Public);
		}
		createTypeCalled = true;
		if (parent != null && parent.IsSealed)
		{
			throw new TypeLoadException("Could not load type '" + FullName + "' from assembly '" + Assembly?.ToString() + "' because the parent type is sealed.");
		}
		if (parent == pmodule.assemblyb.corlib_enum_type && methods != null)
		{
			throw new TypeLoadException("Could not load type '" + FullName + "' from assembly '" + Assembly?.ToString() + "' because it is an enum with methods.");
		}
		if (interfaces != null)
		{
			Type[] array2 = interfaces;
			foreach (Type type in array2)
			{
				if (type.IsNestedPrivate && type.Assembly != Assembly)
				{
					throw new TypeLoadException("Could not load type '" + FullName + "' from assembly '" + Assembly?.ToString() + "' because it is implements the inaccessible interface '" + type.FullName + "'.");
				}
			}
		}
		if (methods != null)
		{
			bool flag = !base.IsAbstract;
			for (int j = 0; j < num_methods; j++)
			{
				MethodBuilder methodBuilder = methods[j];
				if (flag && methodBuilder.IsAbstract)
				{
					throw new InvalidOperationException("Type is concrete but has abstract method " + methodBuilder);
				}
				methodBuilder.check_override();
				methodBuilder.fixup();
			}
		}
		if (ctors != null)
		{
			ConstructorBuilder[] array3 = ctors;
			for (int i = 0; i < array3.Length; i++)
			{
				array3[i].fixup();
			}
		}
		ResolveUserTypes();
		created = create_runtime_class();
		if (created != null)
		{
			return created;
		}
		return this;
	}

	private void ResolveUserTypes()
	{
		parent = ResolveUserType(parent);
		ResolveUserTypes(interfaces);
		if (fields != null)
		{
			FieldBuilder[] array = fields;
			foreach (FieldBuilder fieldBuilder in array)
			{
				if (fieldBuilder != null)
				{
					fieldBuilder.ResolveUserTypes();
				}
			}
		}
		if (methods != null)
		{
			MethodBuilder[] array2 = methods;
			foreach (MethodBuilder methodBuilder in array2)
			{
				if (methodBuilder != null)
				{
					methodBuilder.ResolveUserTypes();
				}
			}
		}
		if (ctors == null)
		{
			return;
		}
		ConstructorBuilder[] array3 = ctors;
		foreach (ConstructorBuilder constructorBuilder in array3)
		{
			if (constructorBuilder != null)
			{
				constructorBuilder.ResolveUserTypes();
			}
		}
	}

	internal static void ResolveUserTypes(Type[] types)
	{
		if (types != null)
		{
			for (int i = 0; i < types.Length; i++)
			{
				types[i] = ResolveUserType(types[i]);
			}
		}
	}

	internal static Type ResolveUserType(Type t)
	{
		if (t != null && (t.GetType().Assembly != typeof(int).Assembly || t is TypeDelegator))
		{
			t = t.UnderlyingSystemType;
			if (t != null && (t.GetType().Assembly != typeof(int).Assembly || t is TypeDelegator))
			{
				throw new NotSupportedException("User defined subclasses of System.Type are not yet supported.");
			}
			return t;
		}
		return t;
	}

	internal void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map)
	{
		if (methods != null)
		{
			for (int i = 0; i < num_methods; i++)
			{
				methods[i].FixupTokens(token_map, member_map);
			}
		}
		if (ctors != null)
		{
			ConstructorBuilder[] array = ctors;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].FixupTokens(token_map, member_map);
			}
		}
		if (subtypes != null)
		{
			TypeBuilder[] array2 = subtypes;
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j].FixupTokens(token_map, member_map);
			}
		}
	}

	internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
	{
		symbolWriter.OpenNamespace(Namespace);
		if (methods != null)
		{
			for (int i = 0; i < num_methods; i++)
			{
				methods[i].GenerateDebugInfo(symbolWriter);
			}
		}
		if (ctors != null)
		{
			ConstructorBuilder[] array = ctors;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].GenerateDebugInfo(symbolWriter);
			}
		}
		symbolWriter.CloseNamespace();
		if (subtypes != null)
		{
			for (int k = 0; k < subtypes.Length; k++)
			{
				subtypes[k].GenerateDebugInfo(symbolWriter);
			}
		}
	}

	[ComVisible(true)]
	public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
	{
		if (is_created)
		{
			return created.GetConstructors(bindingAttr);
		}
		throw new NotSupportedException();
	}

	internal ConstructorInfo[] GetConstructorsInternal(BindingFlags bindingAttr)
	{
		if (ctors == null)
		{
			return new ConstructorInfo[0];
		}
		ArrayList arrayList = new ArrayList();
		ConstructorBuilder[] array = ctors;
		foreach (ConstructorBuilder constructorBuilder in array)
		{
			bool flag = false;
			MethodAttributes attributes = constructorBuilder.Attributes;
			if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
			{
				if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			flag = false;
			if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
			{
				if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
			{
				flag = true;
			}
			if (flag)
			{
				arrayList.Add(constructorBuilder);
			}
		}
		ConstructorInfo[] array2 = new ConstructorInfo[arrayList.Count];
		arrayList.CopyTo(array2);
		return array2;
	}

	public override Type GetElementType()
	{
		throw new NotSupportedException();
	}

	public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
	{
		check_created();
		return created.GetEvent(name, bindingAttr);
	}

	public override EventInfo[] GetEvents()
	{
		return GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
	}

	public override EventInfo[] GetEvents(BindingFlags bindingAttr)
	{
		if (is_created)
		{
			return created.GetEvents(bindingAttr);
		}
		throw new NotSupportedException();
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		if (created != null)
		{
			return created.GetField(name, bindingAttr);
		}
		if (fields == null)
		{
			return null;
		}
		FieldBuilder[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (fieldInfo == null || fieldInfo.Name != name)
			{
				continue;
			}
			bool flag = false;
			FieldAttributes attributes = fieldInfo.Attributes;
			if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
			{
				if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			flag = false;
			if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
			{
				if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
			{
				flag = true;
			}
			if (flag)
			{
				return fieldInfo;
			}
		}
		return null;
	}

	public override FieldInfo[] GetFields(BindingFlags bindingAttr)
	{
		if (created != null)
		{
			return created.GetFields(bindingAttr);
		}
		if (fields == null)
		{
			return new FieldInfo[0];
		}
		ArrayList arrayList = new ArrayList();
		FieldBuilder[] array = fields;
		foreach (FieldInfo fieldInfo in array)
		{
			if (fieldInfo == null)
			{
				continue;
			}
			bool flag = false;
			FieldAttributes attributes = fieldInfo.Attributes;
			if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
			{
				if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			flag = false;
			if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
			{
				if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
			{
				flag = true;
			}
			if (flag)
			{
				arrayList.Add(fieldInfo);
			}
		}
		FieldInfo[] array2 = new FieldInfo[arrayList.Count];
		arrayList.CopyTo(array2);
		return array2;
	}

	public override Type GetInterface(string name, bool ignoreCase)
	{
		check_created();
		return created.GetInterface(name, ignoreCase);
	}

	public override Type[] GetInterfaces()
	{
		if (is_created)
		{
			return created.GetInterfaces();
		}
		if (interfaces != null)
		{
			Type[] array = new Type[interfaces.Length];
			interfaces.CopyTo(array, 0);
			return array;
		}
		return Type.EmptyTypes;
	}

	public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
	{
		check_created();
		return created.GetMember(name, type, bindingAttr);
	}

	public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
	{
		check_created();
		return created.GetMembers(bindingAttr);
	}

	private MethodInfo[] GetMethodsByName(string name, BindingFlags bindingAttr, bool ignoreCase, Type reflected_type)
	{
		MethodInfo[] array2;
		MethodInfo[] array3;
		if ((bindingAttr & BindingFlags.DeclaredOnly) == 0 && parent != null)
		{
			MethodInfo[] array = parent.GetMethods(bindingAttr);
			ArrayList arrayList = new ArrayList(array.Length);
			bool flag = (bindingAttr & BindingFlags.FlattenHierarchy) != 0;
			foreach (MethodInfo methodInfo in array)
			{
				MethodAttributes attributes = methodInfo.Attributes;
				if ((!methodInfo.IsStatic || flag) && (attributes & MethodAttributes.MemberAccessMask) switch
				{
					MethodAttributes.Public => (bindingAttr & BindingFlags.Public) != 0, 
					MethodAttributes.Assembly => (bindingAttr & BindingFlags.NonPublic) != 0, 
					MethodAttributes.Private => false, 
					_ => (bindingAttr & BindingFlags.NonPublic) != 0, 
				})
				{
					arrayList.Add(methodInfo);
				}
			}
			if (methods == null)
			{
				array2 = new MethodInfo[arrayList.Count];
				arrayList.CopyTo(array2);
			}
			else
			{
				array2 = new MethodInfo[methods.Length + arrayList.Count];
				arrayList.CopyTo(array2, 0);
				methods.CopyTo(array2, arrayList.Count);
			}
		}
		else
		{
			array3 = methods;
			array2 = array3;
		}
		if (array2 == null)
		{
			return new MethodInfo[0];
		}
		ArrayList arrayList2 = new ArrayList();
		array3 = array2;
		foreach (MethodInfo methodInfo2 in array3)
		{
			if (methodInfo2 == null || (name != null && string.Compare(methodInfo2.Name, name, ignoreCase) != 0))
			{
				continue;
			}
			bool flag2 = false;
			MethodAttributes attributes = methodInfo2.Attributes;
			if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
			{
				if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
				{
					flag2 = true;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				flag2 = true;
			}
			if (!flag2)
			{
				continue;
			}
			flag2 = false;
			if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
			{
				if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
				{
					flag2 = true;
				}
			}
			else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
			{
				flag2 = true;
			}
			if (flag2)
			{
				arrayList2.Add(methodInfo2);
			}
		}
		MethodInfo[] array4 = new MethodInfo[arrayList2.Count];
		arrayList2.CopyTo(array4);
		return array4;
	}

	public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
	{
		return GetMethodsByName(null, bindingAttr, ignoreCase: false, this);
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		check_created();
		if (types == null)
		{
			return created.GetMethod(name, bindingAttr);
		}
		return created.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
	}

	public override Type GetNestedType(string name, BindingFlags bindingAttr)
	{
		check_created();
		if (subtypes == null)
		{
			return null;
		}
		TypeBuilder[] array = subtypes;
		foreach (TypeBuilder typeBuilder in array)
		{
			if (!typeBuilder.is_created)
			{
				continue;
			}
			if ((typeBuilder.attrs & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
			{
				if ((bindingAttr & BindingFlags.Public) == 0)
				{
					continue;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) == 0)
			{
				continue;
			}
			if (typeBuilder.Name == name)
			{
				return typeBuilder.created;
			}
		}
		return null;
	}

	public override Type[] GetNestedTypes(BindingFlags bindingAttr)
	{
		if (!is_created)
		{
			throw new NotSupportedException();
		}
		ArrayList arrayList = new ArrayList();
		if (subtypes == null)
		{
			return Type.EmptyTypes;
		}
		TypeBuilder[] array = subtypes;
		foreach (TypeBuilder typeBuilder in array)
		{
			bool flag = false;
			if ((typeBuilder.attrs & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic)
			{
				if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				flag = true;
			}
			if (flag)
			{
				arrayList.Add(typeBuilder);
			}
		}
		Type[] array2 = new Type[arrayList.Count];
		arrayList.CopyTo(array2);
		return array2;
	}

	public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
	{
		if (is_created)
		{
			return created.GetProperties(bindingAttr);
		}
		if (properties == null)
		{
			return new PropertyInfo[0];
		}
		ArrayList arrayList = new ArrayList();
		PropertyBuilder[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			bool flag = false;
			MethodInfo methodInfo = propertyInfo.GetGetMethod(nonPublic: true);
			if (methodInfo == null)
			{
				methodInfo = propertyInfo.GetSetMethod(nonPublic: true);
			}
			if (methodInfo == null)
			{
				continue;
			}
			MethodAttributes attributes = methodInfo.Attributes;
			if ((attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Public)
			{
				if ((bindingAttr & BindingFlags.Public) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default)
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			flag = false;
			if ((attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
			{
				if ((bindingAttr & BindingFlags.Static) != BindingFlags.Default)
				{
					flag = true;
				}
			}
			else if ((bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
			{
				flag = true;
			}
			if (flag)
			{
				arrayList.Add(propertyInfo);
			}
		}
		PropertyInfo[] array2 = new PropertyInfo[arrayList.Count];
		arrayList.CopyTo(array2);
		return array2;
	}

	protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
	{
		throw not_supported();
	}

	protected override bool HasElementTypeImpl()
	{
		if (!is_created)
		{
			return false;
		}
		return created.HasElementType;
	}

	public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
	{
		check_created();
		return created.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
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
		return (GetAttributeFlagsImpl() & TypeAttributes.Import) != 0;
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
		if (this == pmodule.assemblyb.corlib_value_type || this == pmodule.assemblyb.corlib_enum_type)
		{
			return false;
		}
		Type baseType = parent;
		while (baseType != null)
		{
			if (baseType == pmodule.assemblyb.corlib_value_type)
			{
				return true;
			}
			baseType = baseType.BaseType;
		}
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

	public override Type MakeGenericType(params Type[] typeArguments)
	{
		if (!IsGenericTypeDefinition)
		{
			throw new InvalidOperationException("not a generic type definition");
		}
		if (typeArguments == null)
		{
			throw new ArgumentNullException("typeArguments");
		}
		if (generic_params.Length != typeArguments.Length)
		{
			throw new ArgumentException($"The type or method has {generic_params.Length} generic parameter(s) but {typeArguments.Length} generic argument(s) where provided. A generic argument must be provided for each generic parameter.", "typeArguments");
		}
		for (int i = 0; i < typeArguments.Length; i++)
		{
			if (typeArguments[i] == null)
			{
				throw new ArgumentNullException("typeArguments");
			}
		}
		Type[] array = new Type[typeArguments.Length];
		typeArguments.CopyTo(array, 0);
		return pmodule.assemblyb.MakeGenericType(this, array);
	}

	public override Type MakePointerType()
	{
		return new PointerType(this);
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		if (customBuilder == null)
		{
			throw new ArgumentNullException("customBuilder");
		}
		switch (customBuilder.Ctor.ReflectedType.FullName)
		{
		case "System.Runtime.InteropServices.StructLayoutAttribute":
		{
			byte[] data = customBuilder.Data;
			int num = data[2] | (data[3] << 8);
			attrs &= ~TypeAttributes.LayoutMask;
			switch ((LayoutKind)num)
			{
			case LayoutKind.Auto:
				attrs |= TypeAttributes.NotPublic;
				break;
			case LayoutKind.Explicit:
				attrs |= TypeAttributes.ExplicitLayout;
				break;
			case LayoutKind.Sequential:
				attrs |= TypeAttributes.SequentialLayout;
				break;
			default:
				throw new Exception("Error in customattr");
			}
			Type obj = ((customBuilder.Ctor is ConstructorBuilder) ? ((ConstructorBuilder)customBuilder.Ctor).parameters[0] : customBuilder.Ctor.GetParametersInternal()[0].ParameterType);
			int num2 = 6;
			if (obj.FullName == "System.Int16")
			{
				num2 = 4;
			}
			int num3 = data[num2++];
			num3 |= data[num2++] << 8;
			for (int i = 0; i < num3; i++)
			{
				num2++;
				int num4;
				if (data[num2++] == 85)
				{
					num4 = CustomAttributeBuilder.decode_len(data, num2, out num2);
					CustomAttributeBuilder.string_from_bytes(data, num2, num4);
					num2 += num4;
				}
				num4 = CustomAttributeBuilder.decode_len(data, num2, out num2);
				string text = CustomAttributeBuilder.string_from_bytes(data, num2, num4);
				num2 += num4;
				int num5 = data[num2++];
				num5 |= data[num2++] << 8;
				num5 |= data[num2++] << 16;
				num5 |= data[num2++] << 24;
				switch (text)
				{
				case "CharSet":
					switch ((CharSet)num5)
					{
					case CharSet.None:
					case CharSet.Ansi:
						attrs &= ~TypeAttributes.StringFormatMask;
						break;
					case CharSet.Unicode:
						attrs &= ~TypeAttributes.AutoClass;
						attrs |= TypeAttributes.UnicodeClass;
						break;
					case CharSet.Auto:
						attrs &= ~TypeAttributes.UnicodeClass;
						attrs |= TypeAttributes.AutoClass;
						break;
					}
					break;
				case "Pack":
					packing_size = (PackingSize)num5;
					break;
				case "Size":
					class_size = num5;
					break;
				}
			}
			return;
		}
		case "System.Runtime.CompilerServices.SpecialNameAttribute":
			attrs |= TypeAttributes.SpecialName;
			return;
		case "System.SerializableAttribute":
			attrs |= TypeAttributes.Serializable;
			return;
		case "System.Runtime.InteropServices.ComImportAttribute":
			attrs |= TypeAttributes.Import;
			return;
		case "System.Security.SuppressUnmanagedCodeSecurityAttribute":
			attrs |= TypeAttributes.HasSecurity;
			break;
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

	[ComVisible(true)]
	public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
	{
		SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
	}

	public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
	{
		check_name("name", name);
		if (eventtype == null)
		{
			throw new ArgumentNullException("type");
		}
		check_not_created();
		EventBuilder eventBuilder = new EventBuilder(this, name, attributes, eventtype);
		if (events != null)
		{
			EventBuilder[] array = new EventBuilder[events.Length + 1];
			Array.Copy(events, array, events.Length);
			array[events.Length] = eventBuilder;
			events = array;
		}
		else
		{
			events = new EventBuilder[1];
			events[0] = eventBuilder;
		}
		return eventBuilder;
	}

	public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		FieldBuilder fieldBuilder = DefineUninitializedData(name, data.Length, attributes);
		fieldBuilder.SetRVAData(data);
		return fieldBuilder;
	}

	public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("Empty name is not legal", "name");
		}
		if (size <= 0 || size > 4128768)
		{
			throw new ArgumentException("Data size must be > 0 and < 0x3f0000");
		}
		check_not_created();
		string text = "$ArrayType$" + size;
		TypeIdentifier innerName = TypeIdentifiers.WithoutEscape(text);
		Type type = pmodule.GetRegisteredType(fullname.NestedName(innerName));
		if (type == null)
		{
			TypeBuilder typeBuilder = DefineNestedType(text, TypeAttributes.NestedPrivate | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, pmodule.assemblyb.corlib_value_type, null, PackingSize.Size1, size);
			typeBuilder.CreateType();
			type = typeBuilder;
		}
		return DefineField(name, type, attributes | FieldAttributes.Static | FieldAttributes.HasFieldRVA);
	}

	public void SetParent(Type parent)
	{
		check_not_created();
		if (parent == null)
		{
			if ((attrs & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic)
			{
				if ((attrs & TypeAttributes.Abstract) == 0)
				{
					throw new InvalidOperationException("Interface must be declared abstract.");
				}
				this.parent = null;
			}
			else
			{
				this.parent = typeof(object);
			}
		}
		else
		{
			this.parent = parent;
		}
		this.parent = ResolveUserType(this.parent);
	}

	internal int get_next_table_index(object obj, int table, int count)
	{
		return pmodule.get_next_table_index(obj, table, count);
	}

	[ComVisible(true)]
	public override InterfaceMapping GetInterfaceMap(Type interfaceType)
	{
		if (created == null)
		{
			throw new NotSupportedException("This method is not implemented for incomplete types.");
		}
		return created.GetInterfaceMap(interfaceType);
	}

	internal override Type InternalResolve()
	{
		check_created();
		return created;
	}

	internal override Type RuntimeResolve()
	{
		check_created();
		return created;
	}

	private Exception not_supported()
	{
		return new NotSupportedException("The invoked member is not supported in a dynamic module.");
	}

	private void check_not_created()
	{
		if (is_created)
		{
			throw new InvalidOperationException("Unable to change after type has been created.");
		}
	}

	private void check_created()
	{
		if (!is_created)
		{
			throw not_supported();
		}
	}

	private void check_name(string argName, string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException(argName);
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("Empty name is not legal", argName);
		}
		if (name[0] == '\0')
		{
			throw new ArgumentException("Illegal name", argName);
		}
	}

	public override string ToString()
	{
		return FullName;
	}

	[MonoTODO]
	public override bool IsAssignableFrom(Type c)
	{
		return base.IsAssignableFrom(c);
	}

	[MonoTODO("arrays")]
	internal bool IsAssignableTo(Type c)
	{
		if (c == this)
		{
			return true;
		}
		if (c.IsInterface)
		{
			if (parent != null && is_created && c.IsAssignableFrom(parent))
			{
				return true;
			}
			if (interfaces == null)
			{
				return false;
			}
			Type[] array = interfaces;
			foreach (Type c2 in array)
			{
				if (c.IsAssignableFrom(c2))
				{
					return true;
				}
			}
			if (!is_created)
			{
				return false;
			}
		}
		if (parent == null)
		{
			return c == typeof(object);
		}
		return c.IsAssignableFrom(parent);
	}

	public bool IsCreated()
	{
		return is_created;
	}

	public override Type[] GetGenericArguments()
	{
		if (generic_params == null)
		{
			return null;
		}
		Type[] array = new Type[generic_params.Length];
		generic_params.CopyTo(array, 0);
		return array;
	}

	public override Type GetGenericTypeDefinition()
	{
		if (generic_params == null)
		{
			throw new InvalidOperationException("Type is not generic");
		}
		return this;
	}

	public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
	{
		if (names == null)
		{
			throw new ArgumentNullException("names");
		}
		if (names.Length == 0)
		{
			throw new ArgumentException("names");
		}
		generic_params = new GenericTypeParameterBuilder[names.Length];
		for (int i = 0; i < names.Length; i++)
		{
			string text = names[i];
			if (text == null)
			{
				throw new ArgumentNullException("names");
			}
			generic_params[i] = new GenericTypeParameterBuilder(this, null, text, i);
		}
		return generic_params;
	}

	public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
	{
		if (type == null)
		{
			throw new ArgumentException("Type is not generic", "type");
		}
		if (!type.IsGenericType)
		{
			throw new ArgumentException("Type is not a generic type", "type");
		}
		if (type.IsGenericTypeDefinition)
		{
			throw new ArgumentException("Type cannot be a generic type definition", "type");
		}
		if (constructor == null)
		{
			throw new NullReferenceException();
		}
		if (!constructor.DeclaringType.IsGenericTypeDefinition)
		{
			throw new ArgumentException("constructor declaring type is not a generic type definition", "constructor");
		}
		if (constructor.DeclaringType != type.GetGenericTypeDefinition())
		{
			throw new ArgumentException("constructor declaring type is not the generic type definition of type", "constructor");
		}
		ConstructorInfo constructor2 = type.GetConstructor(constructor);
		if (constructor2 == null)
		{
			throw new ArgumentException("constructor not found");
		}
		return constructor2;
	}

	private static bool IsValidGetMethodType(Type type)
	{
		if (type is TypeBuilder || type is TypeBuilderInstantiation)
		{
			return true;
		}
		if (type.Module is ModuleBuilder)
		{
			return true;
		}
		if (type.IsGenericParameter)
		{
			return false;
		}
		Type[] genericArguments = type.GetGenericArguments();
		if (genericArguments == null)
		{
			return false;
		}
		for (int i = 0; i < genericArguments.Length; i++)
		{
			if (IsValidGetMethodType(genericArguments[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static MethodInfo GetMethod(Type type, MethodInfo method)
	{
		if (!IsValidGetMethodType(type))
		{
			throw new ArgumentException("type is not TypeBuilder but " + type.GetType(), "type");
		}
		if (type is TypeBuilder && type.ContainsGenericParameters)
		{
			type = type.MakeGenericType(type.GetGenericArguments());
		}
		if (!type.IsGenericType)
		{
			throw new ArgumentException("type is not a generic type", "type");
		}
		if (!method.DeclaringType.IsGenericTypeDefinition)
		{
			throw new ArgumentException("method declaring type is not a generic type definition", "method");
		}
		if (method.DeclaringType != type.GetGenericTypeDefinition())
		{
			throw new ArgumentException("method declaring type is not the generic type definition of type", "method");
		}
		if (method == null)
		{
			throw new NullReferenceException();
		}
		MethodInfo method2 = type.GetMethod(method);
		if (method2 == null)
		{
			throw new ArgumentException($"method {method.Name} not found in type {type}");
		}
		return method2;
	}

	public static FieldInfo GetField(Type type, FieldInfo field)
	{
		if (!type.IsGenericType)
		{
			throw new ArgumentException("Type is not a generic type", "type");
		}
		if (type.IsGenericTypeDefinition)
		{
			throw new ArgumentException("Type cannot be a generic type definition", "type");
		}
		if (field is FieldOnTypeBuilderInst)
		{
			throw new ArgumentException("The specified field must be declared on a generic type definition.", "field");
		}
		if (field.DeclaringType != type.GetGenericTypeDefinition())
		{
			throw new ArgumentException("field declaring type is not the generic type definition of type", "method");
		}
		FieldInfo field2 = type.GetField(field);
		if (field2 == null)
		{
			throw new Exception("field not found");
		}
		return field2;
	}

	public override bool IsAssignableFrom(TypeInfo typeInfo)
	{
		return base.IsAssignableFrom(typeInfo);
	}

	internal static bool SetConstantValue(Type destType, object value, ref object destValue)
	{
		if (value != null)
		{
			Type type = value.GetType();
			if (destType.IsByRef)
			{
				destType = destType.GetElementType();
			}
			destType = Nullable.GetUnderlyingType(destType) ?? destType;
			if (destType.IsEnum)
			{
				EnumBuilder enumBuilder;
				Type type2;
				TypeBuilder typeBuilder;
				if ((enumBuilder = destType as EnumBuilder) != null)
				{
					type2 = enumBuilder.GetEnumUnderlyingType();
					if ((!enumBuilder.GetTypeBuilder().is_created || !(type == enumBuilder.GetTypeBuilder().created)) && !(type == type2))
					{
						throw_argument_ConstantDoesntMatch();
					}
				}
				else if ((typeBuilder = destType as TypeBuilder) != null)
				{
					type2 = typeBuilder.underlying_type;
					if (type2 == null || (type != typeBuilder.UnderlyingSystemType && type != type2))
					{
						throw_argument_ConstantDoesntMatch();
					}
				}
				else
				{
					type2 = Enum.GetUnderlyingType(destType);
					if (type != destType && type != type2)
					{
						throw_argument_ConstantDoesntMatch();
					}
				}
				type = type2;
			}
			else if (!destType.IsAssignableFrom(type))
			{
				throw_argument_ConstantDoesntMatch();
			}
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
				destValue = value;
				return true;
			case TypeCode.String:
				destValue = value;
				return true;
			case TypeCode.DateTime:
			{
				long ticks = ((DateTime)value).Ticks;
				destValue = ticks;
				return true;
			}
			default:
				throw new ArgumentException(type.ToString() + " is not a supported constant type.");
			}
		}
		destValue = null;
		return true;
	}

	private static void throw_argument_ConstantDoesntMatch()
	{
		throw new ArgumentException("Constant does not match the defined type.");
	}
}
