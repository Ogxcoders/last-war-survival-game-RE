using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public class ModuleBuilder : Module
{
	internal IntPtr _impl;

	internal Assembly assembly;

	internal string fqname;

	internal string name;

	internal string scopename;

	internal bool is_resource;

	internal int token;

	private UIntPtr dynamic_image;

	private int num_types;

	private TypeBuilder[] types;

	private CustomAttributeBuilder[] cattrs;

	private byte[] guid;

	private int table_idx;

	internal AssemblyBuilder assemblyb;

	private MethodBuilder[] global_methods;

	private FieldBuilder[] global_fields;

	private bool is_main;

	private MonoResource[] resources;

	private IntPtr unparented_classes;

	private int[] table_indexes;

	private TypeBuilder global_type;

	private Type global_type_created;

	private Dictionary<TypeName, TypeBuilder> name_cache;

	private Dictionary<string, int> us_string_cache;

	private bool transient;

	private ModuleBuilderTokenGenerator token_gen;

	private Hashtable resource_writers;

	private ISymbolWriter symbolWriter;

	private static bool has_warned_about_symbolWriter;

	private static int typeref_tokengen = 33554431;

	private static int typedef_tokengen = 50331647;

	private static int typespec_tokengen = 469762047;

	private static int memberref_tokengen = 184549375;

	private static int methoddef_tokengen = 117440511;

	private Dictionary<MemberInfo, int> inst_tokens;

	private Dictionary<MemberInfo, int> inst_tokens_open;

	public override string FullyQualifiedName
	{
		get
		{
			string fullPath = fqname;
			if (fullPath == null)
			{
				return null;
			}
			if (assemblyb.AssemblyDir != null)
			{
				fullPath = Path.Combine(assemblyb.AssemblyDir, fullPath);
				fullPath = Path.GetFullPath(fullPath);
			}
			return fullPath;
		}
	}

	internal string FileName => fqname;

	internal bool IsMain
	{
		set
		{
			is_main = value;
		}
	}

	public override Assembly Assembly => assemblyb;

	public override string Name => name;

	public override string ScopeName => name;

	public override Guid ModuleVersionId => GetModuleVersionId();

	public override int MetadataToken => RuntimeModule.get_MetadataToken(this);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void basic_init(ModuleBuilder ab);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void set_wrappers_type(ModuleBuilder mb, Type ab);

	internal ModuleBuilder(AssemblyBuilder assb, string name, string fullyqname, bool emitSymbolInfo, bool transient)
	{
		this.name = (scopename = name);
		fqname = fullyqname;
		this.assembly = (assemblyb = assb);
		this.transient = transient;
		guid = Guid.FastNewGuidArray();
		table_idx = get_next_table_index(this, 0, 1);
		name_cache = new Dictionary<TypeName, TypeBuilder>();
		us_string_cache = new Dictionary<string, int>(512);
		basic_init(this);
		CreateGlobalType();
		if (assb.IsRun)
		{
			Type ab = new TypeBuilder(this, TypeAttributes.Abstract, 16777215).CreateType();
			set_wrappers_type(this, ab);
		}
		if (!emitSymbolInfo)
		{
			return;
		}
		Assembly assembly = Assembly.LoadWithPartialName("Mono.CompilerServices.SymbolWriter");
		Type type = null;
		if (assembly != null)
		{
			type = assembly.GetType("Mono.CompilerServices.SymbolWriter.SymbolWriterImpl");
		}
		if (type == null)
		{
			WarnAboutSymbolWriter("Failed to load the default Mono.CompilerServices.SymbolWriter assembly");
		}
		else
		{
			try
			{
				symbolWriter = (ISymbolWriter)Activator.CreateInstance(type, this);
			}
			catch (MissingMethodException)
			{
				WarnAboutSymbolWriter("The default Mono.CompilerServices.SymbolWriter is not available on this platform");
				return;
			}
		}
		string text = fqname;
		if (assemblyb.AssemblyDir != null)
		{
			text = Path.Combine(assemblyb.AssemblyDir, text);
		}
		symbolWriter.Initialize(IntPtr.Zero, text, fFullBuild: true);
	}

	private static void WarnAboutSymbolWriter(string message)
	{
		if (!has_warned_about_symbolWriter)
		{
			has_warned_about_symbolWriter = true;
			Console.Error.WriteLine("WARNING: {0}", message);
		}
	}

	public bool IsTransient()
	{
		return transient;
	}

	public void CreateGlobalFunctions()
	{
		if (global_type_created != null)
		{
			throw new InvalidOperationException("global methods already created");
		}
		if (global_type != null)
		{
			global_type_created = global_type.CreateType();
		}
	}

	public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		FieldAttributes fieldAttributes = attributes & ~FieldAttributes.ReservedMask;
		FieldBuilder fieldBuilder = DefineDataImpl(name, data.Length, fieldAttributes | FieldAttributes.HasFieldRVA);
		fieldBuilder.SetRVAData(data);
		return fieldBuilder;
	}

	public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
	{
		return DefineDataImpl(name, size, attributes & ~FieldAttributes.ReservedMask);
	}

	private FieldBuilder DefineDataImpl(string name, int size, FieldAttributes attributes)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException("name cannot be empty", "name");
		}
		if (global_type_created != null)
		{
			throw new InvalidOperationException("global fields already created");
		}
		if (size <= 0 || size >= 4128768)
		{
			throw new ArgumentException("Data size must be > 0 and < 0x3f0000", (string)null);
		}
		CreateGlobalType();
		string className = "$ArrayType$" + size;
		Type type = GetType(className, throwOnError: false, ignoreCase: false);
		if (type == null)
		{
			TypeBuilder typeBuilder = DefineType(className, TypeAttributes.Public | TypeAttributes.ExplicitLayout | TypeAttributes.Sealed, assemblyb.corlib_value_type, null, PackingSize.Size1, size);
			typeBuilder.CreateType();
			type = typeBuilder;
		}
		FieldBuilder fieldBuilder = global_type.DefineField(name, type, attributes | FieldAttributes.Static);
		if (global_fields != null)
		{
			FieldBuilder[] array = new FieldBuilder[global_fields.Length + 1];
			Array.Copy(global_fields, array, global_fields.Length);
			array[global_fields.Length] = fieldBuilder;
			global_fields = array;
		}
		else
		{
			global_fields = new FieldBuilder[1];
			global_fields[0] = fieldBuilder;
		}
		return fieldBuilder;
	}

	private void addGlobalMethod(MethodBuilder mb)
	{
		if (global_methods != null)
		{
			MethodBuilder[] array = new MethodBuilder[global_methods.Length + 1];
			Array.Copy(global_methods, array, global_methods.Length);
			array[global_methods.Length] = mb;
			global_methods = array;
		}
		else
		{
			global_methods = new MethodBuilder[1];
			global_methods[0] = mb;
		}
	}

	public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes)
	{
		return DefineGlobalMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);
	}

	public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
	{
		return DefineGlobalMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
	}

	public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if ((attributes & MethodAttributes.Static) == 0)
		{
			throw new ArgumentException("global methods must be static");
		}
		if (global_type_created != null)
		{
			throw new InvalidOperationException("global methods already created");
		}
		CreateGlobalType();
		MethodBuilder methodBuilder = global_type.DefineMethod(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
		addGlobalMethod(methodBuilder);
		return methodBuilder;
	}

	public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		return DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
	}

	public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if ((attributes & MethodAttributes.Static) == 0)
		{
			throw new ArgumentException("global methods must be static");
		}
		if (global_type_created != null)
		{
			throw new InvalidOperationException("global methods already created");
		}
		CreateGlobalType();
		MethodBuilder methodBuilder = global_type.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
		addGlobalMethod(methodBuilder);
		return methodBuilder;
	}

	public TypeBuilder DefineType(string name)
	{
		return DefineType(name, TypeAttributes.NotPublic);
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr)
	{
		if ((attr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.NotPublic)
		{
			return DefineType(name, attr, null, null);
		}
		return DefineType(name, attr, typeof(object), null);
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent)
	{
		return DefineType(name, attr, parent, null);
	}

	private void AddType(TypeBuilder tb)
	{
		if (types != null)
		{
			if (types.Length == num_types)
			{
				TypeBuilder[] destinationArray = new TypeBuilder[types.Length * 2];
				Array.Copy(types, destinationArray, num_types);
				types = destinationArray;
			}
		}
		else
		{
			types = new TypeBuilder[1];
		}
		types[num_types] = tb;
		num_types++;
	}

	private TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces, PackingSize packingSize, int typesize)
	{
		if (name == null)
		{
			throw new ArgumentNullException("fullname");
		}
		TypeIdentifier key = TypeIdentifiers.FromInternal(name);
		if (name_cache.ContainsKey(key))
		{
			throw new ArgumentException("Duplicate type name within an assembly.");
		}
		TypeBuilder typeBuilder = new TypeBuilder(this, name, attr, parent, interfaces, packingSize, typesize, null);
		AddType(typeBuilder);
		name_cache.Add(key, typeBuilder);
		return typeBuilder;
	}

	internal void RegisterTypeName(TypeBuilder tb, TypeName name)
	{
		name_cache.Add(name, tb);
	}

	internal TypeBuilder GetRegisteredType(TypeName name)
	{
		TypeBuilder value = null;
		name_cache.TryGetValue(name, out value);
		return value;
	}

	[ComVisible(true)]
	public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
	{
		return DefineType(name, attr, parent, interfaces, PackingSize.Unspecified, 0);
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize)
	{
		return DefineType(name, attr, parent, null, PackingSize.Unspecified, typesize);
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize)
	{
		return DefineType(name, attr, parent, null, packsize, 0);
	}

	public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
	{
		return DefineType(name, attr, parent, null, packingSize, typesize);
	}

	public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
	{
		return new MonoArrayMethod(arrayClass, methodName, callingConvention, returnType, parameterTypes);
	}

	public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
	{
		TypeIdentifier key = TypeIdentifiers.FromInternal(name);
		if (name_cache.ContainsKey(key))
		{
			throw new ArgumentException("Duplicate type name within an assembly.");
		}
		EnumBuilder enumBuilder = new EnumBuilder(this, name, visibility, underlyingType);
		TypeBuilder typeBuilder = enumBuilder.GetTypeBuilder();
		AddType(typeBuilder);
		name_cache.Add(key, typeBuilder);
		return enumBuilder;
	}

	[ComVisible(true)]
	public override Type GetType(string className)
	{
		return GetType(className, throwOnError: false, ignoreCase: false);
	}

	[ComVisible(true)]
	public override Type GetType(string className, bool ignoreCase)
	{
		return GetType(className, throwOnError: false, ignoreCase);
	}

	private TypeBuilder search_in_array(TypeBuilder[] arr, int validElementsInArray, TypeName className)
	{
		for (int i = 0; i < validElementsInArray; i++)
		{
			if (string.Compare(className.DisplayName, arr[i].FullName, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				return arr[i];
			}
		}
		return null;
	}

	private TypeBuilder search_nested_in_array(TypeBuilder[] arr, int validElementsInArray, TypeName className)
	{
		for (int i = 0; i < validElementsInArray; i++)
		{
			if (string.Compare(className.DisplayName, arr[i].Name, ignoreCase: true, CultureInfo.InvariantCulture) == 0)
			{
				return arr[i];
			}
		}
		return null;
	}

	private TypeBuilder GetMaybeNested(TypeBuilder t, IEnumerable<TypeName> nested)
	{
		TypeBuilder typeBuilder = t;
		foreach (TypeName item in nested)
		{
			if (typeBuilder.subtypes == null)
			{
				return null;
			}
			typeBuilder = search_nested_in_array(typeBuilder.subtypes, typeBuilder.subtypes.Length, item);
			if (typeBuilder == null)
			{
				return null;
			}
		}
		return typeBuilder;
	}

	[ComVisible(true)]
	public override Type GetType(string className, bool throwOnError, bool ignoreCase)
	{
		if (className == null)
		{
			throw new ArgumentNullException("className");
		}
		if (className.Length == 0)
		{
			throw new ArgumentException("className");
		}
		TypeBuilder value = null;
		if (types == null && throwOnError)
		{
			throw new TypeLoadException(className);
		}
		TypeSpec typeSpec = TypeSpec.Parse(className);
		if (!ignoreCase)
		{
			TypeName key = typeSpec.TypeNameWithoutModifiers();
			name_cache.TryGetValue(key, out value);
		}
		else
		{
			if (types != null)
			{
				value = search_in_array(types, num_types, typeSpec.Name);
			}
			if (!typeSpec.IsNested && value != null)
			{
				value = GetMaybeNested(value, typeSpec.Nested);
			}
		}
		if (value == null && throwOnError)
		{
			throw new TypeLoadException(className);
		}
		if (value != null && (typeSpec.HasModifiers || typeSpec.IsByRef))
		{
			Type type = value;
			if ((object)value != null)
			{
				TypeBuilder typeBuilder = value;
				if (typeBuilder.is_created)
				{
					type = typeBuilder.CreateType();
				}
			}
			foreach (ModifierSpec modifier in typeSpec.Modifiers)
			{
				if (modifier is PointerSpec)
				{
					type = type.MakePointerType();
				}
				else if (modifier is ArraySpec)
				{
					ArraySpec arraySpec = modifier as ArraySpec;
					if (arraySpec.IsBound)
					{
						return null;
					}
					type = ((arraySpec.Rank != 1) ? type.MakeArrayType(arraySpec.Rank) : type.MakeArrayType());
				}
			}
			if (typeSpec.IsByRef)
			{
				type = type.MakeByRefType();
			}
			value = type as TypeBuilder;
			if (value == null)
			{
				return type;
			}
		}
		if (value != null && value.is_created)
		{
			return value.CreateType();
		}
		return value;
	}

	internal int get_next_table_index(object obj, int table, int count)
	{
		if (table_indexes == null)
		{
			table_indexes = new int[64];
			for (int i = 0; i < 64; i++)
			{
				table_indexes[i] = 1;
			}
			table_indexes[2] = 2;
		}
		int result = table_indexes[table];
		table_indexes[table] += count;
		return result;
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
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

	public ISymbolWriter GetSymWriter()
	{
		return symbolWriter;
	}

	public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
	{
		if (symbolWriter != null)
		{
			return symbolWriter.DefineDocument(url, language, languageVendor, documentType);
		}
		return null;
	}

	public override Type[] GetTypes()
	{
		if (types == null)
		{
			return Type.EmptyTypes;
		}
		int num = num_types;
		Type[] array = new Type[num];
		Array.Copy(types, array, num);
		for (int i = 0; i < array.Length; i++)
		{
			if (types[i].is_created)
			{
				array[i] = types[i].CreateType();
			}
		}
		return array;
	}

	public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException("name cannot be empty");
		}
		if (transient)
		{
			throw new InvalidOperationException("The module is transient");
		}
		if (!assemblyb.IsSave)
		{
			throw new InvalidOperationException("The assembly is transient");
		}
		ResourceWriter resourceWriter = new ResourceWriter(new MemoryStream());
		if (resource_writers == null)
		{
			resource_writers = new Hashtable();
		}
		resource_writers[name] = resourceWriter;
		if (resources != null)
		{
			MonoResource[] destinationArray = new MonoResource[resources.Length + 1];
			Array.Copy(resources, destinationArray, resources.Length);
			resources = destinationArray;
		}
		else
		{
			resources = new MonoResource[1];
		}
		int num = resources.Length - 1;
		resources[num].name = name;
		resources[num].attrs = attribute;
		return resourceWriter;
	}

	public IResourceWriter DefineResource(string name, string description)
	{
		return DefineResource(name, description, ResourceAttributes.Public);
	}

	[MonoTODO]
	public void DefineUnmanagedResource(byte[] resource)
	{
		if (resource == null)
		{
			throw new ArgumentNullException("resource");
		}
		throw new NotImplementedException();
	}

	[MonoTODO]
	public void DefineUnmanagedResource(string resourceFileName)
	{
		if (resourceFileName == null)
		{
			throw new ArgumentNullException("resourceFileName");
		}
		if (resourceFileName == string.Empty)
		{
			throw new ArgumentException("resourceFileName");
		}
		if (!File.Exists(resourceFileName) || Directory.Exists(resourceFileName))
		{
			throw new FileNotFoundException("File '" + resourceFileName + "' does not exist or is a directory.");
		}
		throw new NotImplementedException();
	}

	public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name == string.Empty)
		{
			throw new ArgumentException("name cannot be empty");
		}
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		if (transient)
		{
			throw new InvalidOperationException("The module is transient");
		}
		if (!assemblyb.IsSave)
		{
			throw new InvalidOperationException("The assembly is transient");
		}
		if (resources != null)
		{
			MonoResource[] destinationArray = new MonoResource[resources.Length + 1];
			Array.Copy(resources, destinationArray, resources.Length);
			resources = destinationArray;
		}
		else
		{
			resources = new MonoResource[1];
		}
		int num = resources.Length - 1;
		resources[num].name = name;
		resources[num].attrs = attribute;
		resources[num].stream = stream;
	}

	[MonoTODO]
	public void SetSymCustomAttribute(string name, byte[] data)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public void SetUserEntryPoint(MethodInfo entryPoint)
	{
		if (entryPoint == null)
		{
			throw new ArgumentNullException("entryPoint");
		}
		if (entryPoint.DeclaringType.Module != this)
		{
			throw new InvalidOperationException("entryPoint is not contained in this module");
		}
		throw new NotImplementedException();
	}

	public MethodToken GetMethodToken(MethodInfo method)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		return new MethodToken(GetToken(method));
	}

	public MethodToken GetMethodToken(MethodInfo method, IEnumerable<Type> optionalParameterTypes)
	{
		if (method == null)
		{
			throw new ArgumentNullException("method");
		}
		return new MethodToken(GetToken(method, optionalParameterTypes));
	}

	public MethodToken GetArrayMethodToken(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
	{
		return GetMethodToken(GetArrayMethod(arrayClass, methodName, callingConvention, returnType, parameterTypes));
	}

	[ComVisible(true)]
	public MethodToken GetConstructorToken(ConstructorInfo con)
	{
		if (con == null)
		{
			throw new ArgumentNullException("con");
		}
		return new MethodToken(GetToken(con));
	}

	public MethodToken GetConstructorToken(ConstructorInfo constructor, IEnumerable<Type> optionalParameterTypes)
	{
		if (constructor == null)
		{
			throw new ArgumentNullException("constructor");
		}
		return new MethodToken(GetToken(constructor, optionalParameterTypes));
	}

	public FieldToken GetFieldToken(FieldInfo field)
	{
		if (field == null)
		{
			throw new ArgumentNullException("field");
		}
		return new FieldToken(GetToken(field));
	}

	[MonoTODO]
	public SignatureToken GetSignatureToken(byte[] sigBytes, int sigLength)
	{
		throw new NotImplementedException();
	}

	public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
	{
		if (sigHelper == null)
		{
			throw new ArgumentNullException("sigHelper");
		}
		return new SignatureToken(GetToken(sigHelper));
	}

	public StringToken GetStringConstant(string str)
	{
		if (str == null)
		{
			throw new ArgumentNullException("str");
		}
		return new StringToken(GetToken(str));
	}

	public TypeToken GetTypeToken(Type type)
	{
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (type.IsByRef)
		{
			throw new ArgumentException("type can't be a byref type", "type");
		}
		if (!IsTransient() && type.Module is ModuleBuilder && ((ModuleBuilder)type.Module).IsTransient())
		{
			throw new InvalidOperationException("a non-transient module can't reference a transient module");
		}
		return new TypeToken(GetToken(type));
	}

	public TypeToken GetTypeToken(string name)
	{
		return GetTypeToken(GetType(name));
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int getUSIndex(ModuleBuilder mb, string str);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int getToken(ModuleBuilder mb, object obj, bool create_open_instance);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int getMethodToken(ModuleBuilder mb, MethodBase method, Type[] opt_param_types);

	internal int GetToken(string str)
	{
		if (!us_string_cache.TryGetValue(str, out var value))
		{
			value = getUSIndex(this, str);
			us_string_cache[str] = value;
		}
		return value;
	}

	private int GetPseudoToken(MemberInfo member, bool create_open_instance)
	{
		Dictionary<MemberInfo, int> dictionary = (create_open_instance ? inst_tokens_open : inst_tokens);
		int value;
		if (dictionary == null)
		{
			dictionary = new Dictionary<MemberInfo, int>(ReferenceEqualityComparer<MemberInfo>.Instance);
			if (create_open_instance)
			{
				inst_tokens_open = dictionary;
			}
			else
			{
				inst_tokens = dictionary;
			}
		}
		else if (dictionary.TryGetValue(member, out value))
		{
			return value;
		}
		if (member is TypeBuilderInstantiation || member is SymbolType)
		{
			value = typespec_tokengen--;
		}
		else if (member is FieldOnTypeBuilderInst)
		{
			value = memberref_tokengen--;
		}
		else if (member is ConstructorOnTypeBuilderInst)
		{
			value = memberref_tokengen--;
		}
		else if (member is MethodOnTypeBuilderInst)
		{
			value = memberref_tokengen--;
		}
		else if (member is FieldBuilder)
		{
			value = memberref_tokengen--;
		}
		else if (member is TypeBuilder)
		{
			value = ((create_open_instance && (member as TypeBuilder).ContainsGenericParameters) ? typespec_tokengen-- : ((!(member.Module == this)) ? typeref_tokengen-- : typedef_tokengen--));
		}
		else
		{
			if (member is EnumBuilder)
			{
				return dictionary[member] = GetPseudoToken((member as EnumBuilder).GetTypeBuilder(), create_open_instance);
			}
			if (member is ConstructorBuilder)
			{
				value = ((!(member.Module == this) || (member as ConstructorBuilder).TypeBuilder.ContainsGenericParameters) ? memberref_tokengen-- : methoddef_tokengen--);
			}
			else if (member is MethodBuilder)
			{
				MethodBuilder methodBuilder = member as MethodBuilder;
				value = ((!(member.Module == this) || methodBuilder.TypeBuilder.ContainsGenericParameters || methodBuilder.IsGenericMethodDefinition) ? memberref_tokengen-- : methoddef_tokengen--);
			}
			else
			{
				if (!(member is GenericTypeParameterBuilder))
				{
					throw new NotImplementedException();
				}
				value = typespec_tokengen--;
			}
		}
		dictionary[member] = value;
		RegisterToken(member, value);
		return value;
	}

	internal int GetToken(MemberInfo member)
	{
		if (member is ConstructorBuilder || member is MethodBuilder || member is FieldBuilder)
		{
			return GetPseudoToken(member, create_open_instance: false);
		}
		return getToken(this, member, create_open_instance: true);
	}

	internal int GetToken(MemberInfo member, bool create_open_instance)
	{
		if (member is TypeBuilderInstantiation || member is FieldOnTypeBuilderInst || member is ConstructorOnTypeBuilderInst || member is MethodOnTypeBuilderInst || member is SymbolType || member is FieldBuilder || member is TypeBuilder || member is ConstructorBuilder || member is MethodBuilder || member is GenericTypeParameterBuilder || member is EnumBuilder)
		{
			return GetPseudoToken(member, create_open_instance);
		}
		return getToken(this, member, create_open_instance);
	}

	internal int GetToken(MethodBase method, IEnumerable<Type> opt_param_types)
	{
		if (method is ConstructorBuilder || method is MethodBuilder)
		{
			return GetPseudoToken(method, create_open_instance: false);
		}
		if (opt_param_types == null)
		{
			return getToken(this, method, create_open_instance: true);
		}
		List<Type> list = new List<Type>(opt_param_types);
		return getMethodToken(this, method, list.ToArray());
	}

	internal int GetToken(MethodBase method, Type[] opt_param_types)
	{
		if (method is ConstructorBuilder || method is MethodBuilder)
		{
			return GetPseudoToken(method, create_open_instance: false);
		}
		return getMethodToken(this, method, opt_param_types);
	}

	internal int GetToken(SignatureHelper helper)
	{
		return getToken(this, helper, create_open_instance: true);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void RegisterToken(object obj, int token);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern object GetRegisteredToken(int token);

	internal TokenGenerator GetTokenGenerator()
	{
		if (token_gen == null)
		{
			token_gen = new ModuleBuilderTokenGenerator(this);
		}
		return token_gen;
	}

	internal static object RuntimeResolve(object obj)
	{
		if (obj is MethodBuilder)
		{
			return (obj as MethodBuilder).RuntimeResolve();
		}
		if (obj is ConstructorBuilder)
		{
			return (obj as ConstructorBuilder).RuntimeResolve();
		}
		if (obj is FieldBuilder)
		{
			return (obj as FieldBuilder).RuntimeResolve();
		}
		if (obj is GenericTypeParameterBuilder)
		{
			return (obj as GenericTypeParameterBuilder).RuntimeResolve();
		}
		if (obj is FieldOnTypeBuilderInst)
		{
			return (obj as FieldOnTypeBuilderInst).RuntimeResolve();
		}
		if (obj is MethodOnTypeBuilderInst)
		{
			return (obj as MethodOnTypeBuilderInst).RuntimeResolve();
		}
		if (obj is ConstructorOnTypeBuilderInst)
		{
			return (obj as ConstructorOnTypeBuilderInst).RuntimeResolve();
		}
		if (obj is Type)
		{
			return (obj as Type).RuntimeResolve();
		}
		throw new NotImplementedException(obj.GetType().FullName);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void build_metadata(ModuleBuilder mb);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void WriteToFile(IntPtr handle);

	private void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map, Dictionary<MemberInfo, int> inst_tokens, bool open)
	{
		foreach (KeyValuePair<MemberInfo, int> inst_token in inst_tokens)
		{
			MemberInfo key = inst_token.Key;
			int value = inst_token.Value;
			MemberInfo memberInfo = null;
			if (key is TypeBuilderInstantiation || key is SymbolType)
			{
				memberInfo = (key as Type).RuntimeResolve();
			}
			else if (key is FieldOnTypeBuilderInst)
			{
				memberInfo = (key as FieldOnTypeBuilderInst).RuntimeResolve();
			}
			else if (key is ConstructorOnTypeBuilderInst)
			{
				memberInfo = (key as ConstructorOnTypeBuilderInst).RuntimeResolve();
			}
			else if (key is MethodOnTypeBuilderInst)
			{
				memberInfo = (key as MethodOnTypeBuilderInst).RuntimeResolve();
			}
			else if (key is FieldBuilder)
			{
				memberInfo = (key as FieldBuilder).RuntimeResolve();
			}
			else if (key is TypeBuilder)
			{
				memberInfo = (key as TypeBuilder).RuntimeResolve();
			}
			else if (key is EnumBuilder)
			{
				memberInfo = (key as EnumBuilder).RuntimeResolve();
			}
			else if (key is ConstructorBuilder)
			{
				memberInfo = (key as ConstructorBuilder).RuntimeResolve();
			}
			else if (key is MethodBuilder)
			{
				memberInfo = (key as MethodBuilder).RuntimeResolve();
			}
			else
			{
				if (!(key is GenericTypeParameterBuilder))
				{
					throw new NotImplementedException();
				}
				memberInfo = (key as GenericTypeParameterBuilder).RuntimeResolve();
			}
			int value2 = GetToken(memberInfo, open);
			token_map[value] = value2;
			member_map[value] = memberInfo;
			RegisterToken(memberInfo, value);
		}
	}

	private void FixupTokens()
	{
		Dictionary<int, int> token_map = new Dictionary<int, int>();
		Dictionary<int, MemberInfo> member_map = new Dictionary<int, MemberInfo>();
		if (inst_tokens != null)
		{
			FixupTokens(token_map, member_map, inst_tokens, open: false);
		}
		if (inst_tokens_open != null)
		{
			FixupTokens(token_map, member_map, inst_tokens_open, open: true);
		}
		if (types != null)
		{
			for (int i = 0; i < num_types; i++)
			{
				types[i].FixupTokens(token_map, member_map);
			}
		}
	}

	internal void Save()
	{
		if (transient && !is_main)
		{
			return;
		}
		if (types != null)
		{
			for (int i = 0; i < num_types; i++)
			{
				if (!types[i].is_created)
				{
					throw new NotSupportedException("Type '" + types[i].FullName + "' was not completed.");
				}
			}
		}
		FixupTokens();
		if (global_type != null && global_type_created == null)
		{
			global_type_created = global_type.CreateType();
		}
		if (resources != null)
		{
			for (int j = 0; j < resources.Length; j++)
			{
				if (resource_writers != null && resource_writers[resources[j].name] is IResourceWriter resourceWriter)
				{
					ResourceWriter obj = (ResourceWriter)resourceWriter;
					obj.Generate();
					MemoryStream memoryStream = (MemoryStream)obj._output;
					resources[j].data = new byte[memoryStream.Length];
					memoryStream.Seek(0L, SeekOrigin.Begin);
					memoryStream.Read(resources[j].data, 0, (int)memoryStream.Length);
					continue;
				}
				Stream stream = resources[j].stream;
				if (stream != null)
				{
					try
					{
						long length = stream.Length;
						resources[j].data = new byte[length];
						stream.Seek(0L, SeekOrigin.Begin);
						stream.Read(resources[j].data, 0, (int)length);
					}
					catch
					{
					}
				}
			}
		}
		build_metadata(this);
		string text = fqname;
		if (assemblyb.AssemblyDir != null)
		{
			text = Path.Combine(assemblyb.AssemblyDir, text);
		}
		try
		{
			File.Delete(text);
		}
		catch
		{
		}
		using (FileStream fileStream = new FileStream(text, FileMode.Create, FileAccess.Write))
		{
			WriteToFile(fileStream.Handle);
		}
		File.SetAttributes(text, (FileAttributes)(-2147483648));
		if (types != null && symbolWriter != null)
		{
			for (int k = 0; k < num_types; k++)
			{
				types[k].GenerateDebugInfo(symbolWriter);
			}
			symbolWriter.Close();
		}
	}

	internal void CreateGlobalType()
	{
		if (global_type == null)
		{
			global_type = new TypeBuilder(this, TypeAttributes.NotPublic, 1);
		}
	}

	internal override Guid GetModuleVersionId()
	{
		return new Guid(guid);
	}

	public override bool IsResource()
	{
		return false;
	}

	protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
	{
		if (global_type_created == null)
		{
			return null;
		}
		if (types == null)
		{
			return global_type_created.GetMethod(name);
		}
		return global_type_created.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
	}

	public override FieldInfo ResolveField(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return RuntimeModule.ResolveField(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	public override MemberInfo ResolveMember(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return RuntimeModule.ResolveMember(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	internal MemberInfo ResolveOrGetRegisteredToken(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		MemberInfo memberInfo = RuntimeModule.ResolveMemberToken(_impl, metadataToken, RuntimeModule.ptrs_from_types(genericTypeArguments), RuntimeModule.ptrs_from_types(genericMethodArguments), out var error);
		if (memberInfo != null)
		{
			return memberInfo;
		}
		memberInfo = GetRegisteredToken(metadataToken) as MemberInfo;
		if (memberInfo == null)
		{
			throw RuntimeModule.resolve_token_exception(Name, metadataToken, error, "MemberInfo");
		}
		return memberInfo;
	}

	public override MethodBase ResolveMethod(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return RuntimeModule.ResolveMethod(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	public override string ResolveString(int metadataToken)
	{
		return RuntimeModule.ResolveString(this, _impl, metadataToken);
	}

	public override byte[] ResolveSignature(int metadataToken)
	{
		return RuntimeModule.ResolveSignature(this, _impl, metadataToken);
	}

	public override Type ResolveType(int metadataToken, Type[] genericTypeArguments, Type[] genericMethodArguments)
	{
		return RuntimeModule.ResolveType(this, _impl, metadataToken, genericTypeArguments, genericMethodArguments);
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return base.IsDefined(attributeType, inherit);
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return GetCustomAttributes(null, inherit);
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		if (cattrs == null || cattrs.Length == 0)
		{
			return Array.Empty<object>();
		}
		if (attributeType is TypeBuilder)
		{
			throw new InvalidOperationException("First argument to GetCustomAttributes can't be a TypeBuilder");
		}
		List<object> list = new List<object>();
		for (int i = 0; i < cattrs.Length; i++)
		{
			Type type = cattrs[i].Ctor.GetType();
			if (type is TypeBuilder)
			{
				throw new InvalidOperationException("Can't construct custom attribute for TypeBuilder type");
			}
			if (attributeType == null || attributeType.IsAssignableFrom(type))
			{
				list.Add(cattrs[i].Invoke());
			}
		}
		return list.ToArray();
	}

	public override FieldInfo GetField(string name, BindingFlags bindingAttr)
	{
		if (global_type_created == null)
		{
			throw new InvalidOperationException("Module-level fields cannot be retrieved until after the CreateGlobalFunctions method has been called for the module.");
		}
		return global_type_created.GetField(name, bindingAttr);
	}

	public override FieldInfo[] GetFields(BindingFlags bindingFlags)
	{
		if (global_type_created == null)
		{
			throw new InvalidOperationException("Module-level fields cannot be retrieved until after the CreateGlobalFunctions method has been called for the module.");
		}
		return global_type_created.GetFields(bindingFlags);
	}

	public override MethodInfo[] GetMethods(BindingFlags bindingFlags)
	{
		if (global_type_created == null)
		{
			throw new InvalidOperationException("Module-level methods cannot be retrieved until after the CreateGlobalFunctions method has been called for the module.");
		}
		return global_type_created.GetMethods(bindingFlags);
	}
}
