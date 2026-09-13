using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public sealed class MethodBuilder : MethodInfo
{
	private RuntimeMethodHandle mhandle;

	private Type rtype;

	internal Type[] parameters;

	private MethodAttributes attrs;

	private MethodImplAttributes iattrs;

	private string name;

	private int table_idx;

	private byte[] code;

	private ILGenerator ilgen;

	private TypeBuilder type;

	internal ParameterBuilder[] pinfo;

	private CustomAttributeBuilder[] cattrs;

	private MethodInfo[] override_methods;

	private string pi_dll;

	private string pi_entry;

	private CharSet charset;

	private uint extra_flags;

	private CallingConvention native_cc;

	private CallingConventions call_conv;

	private bool init_locals = true;

	private IntPtr generic_container;

	internal GenericTypeParameterBuilder[] generic_params;

	private Type[] returnModReq;

	private Type[] returnModOpt;

	private Type[][] paramModReq;

	private Type[][] paramModOpt;

	private RefEmitPermissionSet[] permissions;

	public override bool ContainsGenericParameters
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public bool InitLocals
	{
		get
		{
			return init_locals;
		}
		set
		{
			init_locals = value;
		}
	}

	internal TypeBuilder TypeBuilder => type;

	public override RuntimeMethodHandle MethodHandle
	{
		get
		{
			throw NotSupported();
		}
	}

	internal RuntimeMethodHandle MethodHandleInternal => mhandle;

	public override Type ReturnType => rtype;

	public override Type ReflectedType => type;

	public override Type DeclaringType => type;

	public override string Name => name;

	public override MethodAttributes Attributes => attrs;

	public override ICustomAttributeProvider ReturnTypeCustomAttributes => null;

	public override CallingConventions CallingConvention => call_conv;

	[MonoTODO("Not implemented")]
	public string Signature
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal bool BestFitMapping
	{
		set
		{
			extra_flags = (uint)((extra_flags & -49) | (uint)(value ? 16 : 32));
		}
	}

	internal bool ThrowOnUnmappableChar
	{
		set
		{
			extra_flags = (uint)((extra_flags & -12289) | (uint)(value ? 4096 : 8192));
		}
	}

	internal bool ExactSpelling
	{
		set
		{
			extra_flags = (uint)((extra_flags & -2) | (uint)(value ? 1 : 0));
		}
	}

	internal bool SetLastError
	{
		set
		{
			extra_flags = (uint)((extra_flags & -65) | (uint)(value ? 64 : 0));
		}
	}

	public override bool IsGenericMethodDefinition => generic_params != null;

	public override bool IsGenericMethod => generic_params != null;

	public override Module Module => GetModule();

	public override ParameterInfo ReturnParameter => base.ReturnParameter;

	internal MethodBuilder(TypeBuilder tb, string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnModReq, Type[] returnModOpt, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt)
	{
		this.name = name;
		attrs = attributes;
		call_conv = callingConvention;
		rtype = returnType;
		this.returnModReq = returnModReq;
		this.returnModOpt = returnModOpt;
		this.paramModReq = paramModReq;
		this.paramModOpt = paramModOpt;
		if ((attributes & MethodAttributes.Static) == 0)
		{
			call_conv |= CallingConventions.HasThis;
		}
		if (parameterTypes != null)
		{
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if (parameterTypes[i] == null)
				{
					throw new ArgumentException("Elements of the parameterTypes array cannot be null", "parameterTypes");
				}
			}
			parameters = new Type[parameterTypes.Length];
			Array.Copy(parameterTypes, parameters, parameterTypes.Length);
		}
		type = tb;
		table_idx = get_next_table_index(this, 6, 1);
		((ModuleBuilder)tb.Module).RegisterToken(this, GetToken().Token);
	}

	internal MethodBuilder(TypeBuilder tb, string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnModReq, Type[] returnModOpt, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt, string dllName, string entryName, CallingConvention nativeCConv, CharSet nativeCharset)
		: this(tb, name, attributes, callingConvention, returnType, returnModReq, returnModOpt, parameterTypes, paramModReq, paramModOpt)
	{
		pi_dll = dllName;
		pi_entry = entryName;
		native_cc = nativeCConv;
		charset = nativeCharset;
	}

	public MethodToken GetToken()
	{
		return new MethodToken(0x6000000 | table_idx);
	}

	public override MethodInfo GetBaseDefinition()
	{
		return this;
	}

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		return iattrs;
	}

	public override ParameterInfo[] GetParameters()
	{
		if (!type.is_created)
		{
			throw NotSupported();
		}
		return GetParametersInternal();
	}

	internal override ParameterInfo[] GetParametersInternal()
	{
		if (parameters == null)
		{
			return null;
		}
		ParameterInfo[] array = new ParameterInfo[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			int num = i;
			ParameterBuilder[] array2 = pinfo;
			array[num] = RuntimeParameterInfo.New((array2 != null) ? array2[i + 1] : null, parameters[i], this, i + 1);
		}
		return array;
	}

	internal override int GetParametersCount()
	{
		if (parameters == null)
		{
			return 0;
		}
		return parameters.Length;
	}

	internal override Type GetParameterType(int pos)
	{
		return parameters[pos];
	}

	internal MethodBase RuntimeResolve()
	{
		return type.RuntimeResolve().GetMethod(this);
	}

	public Module GetModule()
	{
		return type.Module;
	}

	public void CreateMethodBody(byte[] il, int count)
	{
		if (il != null && (count < 0 || count > il.Length))
		{
			throw new ArgumentOutOfRangeException("Index was out of range.  Must be non-negative and less than the size of the collection.");
		}
		if (code != null || type.is_created)
		{
			throw new InvalidOperationException("Type definition of the method is complete.");
		}
		if (il == null)
		{
			code = null;
			return;
		}
		code = new byte[count];
		Array.Copy(il, code, count);
	}

	public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
	{
		GetILGenerator().Init(il, maxStack, localSignature, exceptionHandlers, tokenFixups);
	}

	public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		throw NotSupported();
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw NotSupported();
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		if (type.is_created)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, inherit);
		}
		throw NotSupported();
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		if (type.is_created)
		{
			return MonoCustomAttrs.GetCustomAttributes(this, attributeType, inherit);
		}
		throw NotSupported();
	}

	public ILGenerator GetILGenerator()
	{
		return GetILGenerator(64);
	}

	public ILGenerator GetILGenerator(int size)
	{
		if ((iattrs & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL || (iattrs & MethodImplAttributes.ManagedMask) != MethodImplAttributes.IL)
		{
			throw new InvalidOperationException("Method body should not exist.");
		}
		if (ilgen != null)
		{
			return ilgen;
		}
		ilgen = new ILGenerator(type.Module, ((ModuleBuilder)type.Module).GetTokenGenerator(), size);
		return ilgen;
	}

	public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string strParamName)
	{
		RejectIfCreated();
		if (position < 0 || parameters == null || position > parameters.Length)
		{
			throw new ArgumentOutOfRangeException("position");
		}
		ParameterBuilder parameterBuilder = new ParameterBuilder(this, position, attributes, strParamName);
		if (pinfo == null)
		{
			pinfo = new ParameterBuilder[parameters.Length + 1];
		}
		pinfo[position] = parameterBuilder;
		return parameterBuilder;
	}

	internal void check_override()
	{
		if (override_methods == null)
		{
			return;
		}
		MethodInfo[] array = override_methods;
		foreach (MethodInfo methodInfo in array)
		{
			if (methodInfo.IsVirtual && !base.IsVirtual)
			{
				throw new TypeLoadException($"Method '{name}' override '{methodInfo}' but it is not virtual");
			}
		}
	}

	internal void fixup()
	{
		if ((attrs & (MethodAttributes.Abstract | MethodAttributes.PinvokeImpl)) == 0 && (iattrs & (MethodImplAttributes)4099) == 0 && (ilgen == null || ilgen.ILOffset == 0) && (code == null || code.Length == 0))
		{
			throw new InvalidOperationException($"Method '{DeclaringType.FullName}.{Name}' does not have a method body.");
		}
		if (ilgen != null)
		{
			ilgen.label_fixup(this);
		}
	}

	internal void ResolveUserTypes()
	{
		rtype = TypeBuilder.ResolveUserType(rtype);
		TypeBuilder.ResolveUserTypes(parameters);
		TypeBuilder.ResolveUserTypes(returnModReq);
		TypeBuilder.ResolveUserTypes(returnModOpt);
		if (paramModReq != null)
		{
			Type[][] array = paramModReq;
			for (int i = 0; i < array.Length; i++)
			{
				TypeBuilder.ResolveUserTypes(array[i]);
			}
		}
		if (paramModOpt != null)
		{
			Type[][] array = paramModOpt;
			for (int i = 0; i < array.Length; i++)
			{
				TypeBuilder.ResolveUserTypes(array[i]);
			}
		}
	}

	internal void FixupTokens(Dictionary<int, int> token_map, Dictionary<int, MemberInfo> member_map)
	{
		if (ilgen != null)
		{
			ilgen.FixupTokens(token_map, member_map);
		}
	}

	internal void GenerateDebugInfo(ISymbolWriter symbolWriter)
	{
		if (ilgen != null && ilgen.HasDebugInfo)
		{
			SymbolToken symbolToken = new SymbolToken(GetToken().Token);
			symbolWriter.OpenMethod(symbolToken);
			symbolWriter.SetSymAttribute(symbolToken, "__name", Encoding.UTF8.GetBytes(Name));
			ilgen.GenerateDebugInfo(symbolWriter);
			symbolWriter.CloseMethod();
		}
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		if (customBuilder == null)
		{
			throw new ArgumentNullException("customBuilder");
		}
		switch (customBuilder.Ctor.ReflectedType.FullName)
		{
		case "System.Runtime.CompilerServices.MethodImplAttribute":
		{
			byte[] data = customBuilder.Data;
			int num = data[2];
			num |= data[3] << 8;
			iattrs |= (MethodImplAttributes)num;
			return;
		}
		case "System.Runtime.InteropServices.DllImportAttribute":
		{
			CustomAttributeBuilder.CustomAttributeInfo customAttributeInfo = CustomAttributeBuilder.decode_cattr(customBuilder);
			bool flag = true;
			pi_dll = (string)customAttributeInfo.ctorArgs[0];
			if (pi_dll == null || pi_dll.Length == 0)
			{
				throw new ArgumentException("DllName cannot be empty");
			}
			native_cc = System.Runtime.InteropServices.CallingConvention.Winapi;
			for (int i = 0; i < customAttributeInfo.namedParamNames.Length; i++)
			{
				string text = customAttributeInfo.namedParamNames[i];
				object obj = customAttributeInfo.namedParamValues[i];
				switch (text)
				{
				case "CallingConvention":
					native_cc = (CallingConvention)obj;
					break;
				case "CharSet":
					charset = (CharSet)obj;
					break;
				case "EntryPoint":
					pi_entry = (string)obj;
					break;
				case "ExactSpelling":
					ExactSpelling = (bool)obj;
					break;
				case "SetLastError":
					SetLastError = (bool)obj;
					break;
				case "PreserveSig":
					flag = (bool)obj;
					break;
				case "BestFitMapping":
					BestFitMapping = (bool)obj;
					break;
				case "ThrowOnUnmappableChar":
					ThrowOnUnmappableChar = (bool)obj;
					break;
				}
			}
			attrs |= MethodAttributes.PinvokeImpl;
			if (flag)
			{
				iattrs |= MethodImplAttributes.PreserveSig;
			}
			return;
		}
		case "System.Runtime.InteropServices.PreserveSigAttribute":
			iattrs |= MethodImplAttributes.PreserveSig;
			return;
		case "System.Runtime.CompilerServices.SpecialNameAttribute":
			attrs |= MethodAttributes.SpecialName;
			return;
		case "System.Security.SuppressUnmanagedCodeSecurityAttribute":
			attrs |= MethodAttributes.HasSecurity;
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
		if (con == null)
		{
			throw new ArgumentNullException("con");
		}
		if (binaryAttribute == null)
		{
			throw new ArgumentNullException("binaryAttribute");
		}
		SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
	}

	public void SetImplementationFlags(MethodImplAttributes attributes)
	{
		RejectIfCreated();
		iattrs = attributes;
	}

	public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
	{
	}

	[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead.")]
	public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
	{
		RejectIfCreated();
		throw new NotImplementedException();
	}

	[MonoTODO]
	public void SetSymCustomAttribute(string name, byte[] data)
	{
		RejectIfCreated();
		throw new NotImplementedException();
	}

	public override string ToString()
	{
		return "MethodBuilder [" + type.Name + "::" + name + "]";
	}

	[MonoTODO]
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return name.GetHashCode();
	}

	internal override int get_next_table_index(object obj, int table, int count)
	{
		return type.get_next_table_index(obj, table, count);
	}

	private void ExtendArray<T>(ref T[] array, T elem)
	{
		if (array == null)
		{
			array = new T[1];
		}
		else
		{
			T[] array2 = new T[array.Length + 1];
			Array.Copy(array, array2, array.Length);
			array = array2;
		}
		array[array.Length - 1] = elem;
	}

	internal void set_override(MethodInfo mdecl)
	{
		ExtendArray(ref override_methods, mdecl);
	}

	private void RejectIfCreated()
	{
		if (type.is_created)
		{
			throw new InvalidOperationException("Type definition of the method is complete.");
		}
	}

	private Exception NotSupported()
	{
		return new NotSupportedException("The invoked member is not supported in a dynamic module.");
	}

	public override MethodInfo MakeGenericMethod(params Type[] typeArguments)
	{
		if (!IsGenericMethodDefinition)
		{
			throw new InvalidOperationException("Method is not a generic method definition");
		}
		if (typeArguments == null)
		{
			throw new ArgumentNullException("typeArguments");
		}
		if (generic_params.Length != typeArguments.Length)
		{
			throw new ArgumentException("Incorrect length", "typeArguments");
		}
		for (int i = 0; i < typeArguments.Length; i++)
		{
			if (typeArguments[i] == null)
			{
				throw new ArgumentNullException("typeArguments");
			}
		}
		return new MethodOnTypeBuilderInst(this, typeArguments);
	}

	public override MethodInfo GetGenericMethodDefinition()
	{
		if (!IsGenericMethodDefinition)
		{
			throw new InvalidOperationException();
		}
		return this;
	}

	public override Type[] GetGenericArguments()
	{
		if (generic_params == null)
		{
			return null;
		}
		Type[] array = new Type[generic_params.Length];
		for (int i = 0; i < generic_params.Length; i++)
		{
			array[i] = generic_params[i];
		}
		return array;
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
			generic_params[i] = new GenericTypeParameterBuilder(type, this, text, i);
		}
		return generic_params;
	}

	public void SetReturnType(Type returnType)
	{
		rtype = returnType;
	}

	public void SetParameters(params Type[] parameterTypes)
	{
		if (parameterTypes == null)
		{
			return;
		}
		for (int i = 0; i < parameterTypes.Length; i++)
		{
			if (parameterTypes[i] == null)
			{
				throw new ArgumentException("Elements of the parameterTypes array cannot be null", "parameterTypes");
			}
		}
		parameters = new Type[parameterTypes.Length];
		Array.Copy(parameterTypes, parameters, parameterTypes.Length);
	}

	public void SetSignature(Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
	{
		SetReturnType(returnType);
		SetParameters(parameterTypes);
		returnModReq = returnTypeRequiredCustomModifiers;
		returnModOpt = returnTypeOptionalCustomModifiers;
		paramModReq = parameterTypeRequiredCustomModifiers;
		paramModOpt = parameterTypeOptionalCustomModifiers;
	}
}
