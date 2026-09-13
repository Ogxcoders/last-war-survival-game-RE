using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
public sealed class ConstructorBuilder : ConstructorInfo
{
	private RuntimeMethodHandle mhandle;

	private ILGenerator ilgen;

	internal Type[] parameters;

	private MethodAttributes attrs;

	private MethodImplAttributes iattrs;

	private int table_idx;

	private CallingConventions call_conv;

	private TypeBuilder type;

	internal ParameterBuilder[] pinfo;

	private CustomAttributeBuilder[] cattrs;

	private bool init_locals = true;

	private Type[][] paramModReq;

	private Type[][] paramModOpt;

	private RefEmitPermissionSet[] permissions;

	[MonoTODO]
	public override CallingConventions CallingConvention => call_conv;

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
			throw not_supported();
		}
	}

	public override MethodAttributes Attributes => attrs;

	public override Type ReflectedType => type;

	public override Type DeclaringType => type;

	[Obsolete]
	public Type ReturnType => null;

	public override string Name
	{
		get
		{
			if ((attrs & MethodAttributes.Static) == 0)
			{
				return ConstructorInfo.ConstructorName;
			}
			return ConstructorInfo.TypeConstructorName;
		}
	}

	public string Signature => "constructor signature";

	public override Module Module => GetModule();

	internal ConstructorBuilder(TypeBuilder tb, MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] paramModReq, Type[][] paramModOpt)
	{
		attrs = attributes | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
		call_conv = callingConvention;
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
		this.paramModReq = paramModReq;
		this.paramModOpt = paramModOpt;
		table_idx = get_next_table_index(this, 6, 1);
		((ModuleBuilder)tb.Module).RegisterToken(this, GetToken().Token);
	}

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		return iattrs;
	}

	public override ParameterInfo[] GetParameters()
	{
		if (!type.is_created)
		{
			throw not_created();
		}
		return GetParametersInternal();
	}

	internal override ParameterInfo[] GetParametersInternal()
	{
		if (parameters == null)
		{
			return EmptyArray<ParameterInfo>.Value;
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
		return type.RuntimeResolve().GetConstructor(this);
	}

	public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		throw not_supported();
	}

	public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		throw not_supported();
	}

	public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
	{
	}

	public ParameterBuilder DefineParameter(int iSequence, ParameterAttributes attributes, string strParamName)
	{
		if (iSequence < 0 || iSequence > GetParametersCount())
		{
			throw new ArgumentOutOfRangeException("iSequence");
		}
		if (type.is_created)
		{
			throw not_after_created();
		}
		ParameterBuilder parameterBuilder = new ParameterBuilder(this, iSequence, attributes, strParamName);
		if (pinfo == null)
		{
			pinfo = new ParameterBuilder[parameters.Length + 1];
		}
		pinfo[iSequence] = parameterBuilder;
		return parameterBuilder;
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

	public ILGenerator GetILGenerator()
	{
		return GetILGenerator(64);
	}

	public ILGenerator GetILGenerator(int streamSize)
	{
		if (ilgen != null)
		{
			return ilgen;
		}
		ilgen = new ILGenerator(type.Module, ((ModuleBuilder)type.Module).GetTokenGenerator(), streamSize);
		return ilgen;
	}

	public void SetMethodBody(byte[] il, int maxStack, byte[] localSignature, IEnumerable<ExceptionHandler> exceptionHandlers, IEnumerable<int> tokenFixups)
	{
		GetILGenerator().Init(il, maxStack, localSignature, exceptionHandlers, tokenFixups);
	}

	public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
	{
		if (customBuilder == null)
		{
			throw new ArgumentNullException("customBuilder");
		}
		if (customBuilder.Ctor.ReflectedType.FullName == "System.Runtime.CompilerServices.MethodImplAttribute")
		{
			byte[] data = customBuilder.Data;
			int num = data[2];
			num |= data[3] << 8;
			SetImplementationFlags((MethodImplAttributes)num);
		}
		else if (cattrs != null)
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
		if (type.is_created)
		{
			throw not_after_created();
		}
		iattrs = attributes;
	}

	public Module GetModule()
	{
		return type.Module;
	}

	public MethodToken GetToken()
	{
		return new MethodToken(0x6000000 | table_idx);
	}

	[MonoTODO]
	public void SetSymCustomAttribute(string name, byte[] data)
	{
		if (type.is_created)
		{
			throw not_after_created();
		}
	}

	public override string ToString()
	{
		return "ConstructorBuilder ['" + type.Name + "']";
	}

	internal void fixup()
	{
		if ((attrs & (MethodAttributes.Abstract | MethodAttributes.PinvokeImpl)) == 0 && (iattrs & (MethodImplAttributes)4099) == 0 && (ilgen == null || ilgen.ILOffset == 0))
		{
			throw new InvalidOperationException("Method '" + Name + "' does not have a method body.");
		}
		if (ilgen != null)
		{
			ilgen.label_fixup(this);
		}
	}

	internal void ResolveUserTypes()
	{
		TypeBuilder.ResolveUserTypes(parameters);
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

	internal override int get_next_table_index(object obj, int table, int count)
	{
		return type.get_next_table_index(obj, table, count);
	}

	private void RejectIfCreated()
	{
		if (type.is_created)
		{
			throw new InvalidOperationException("Type definition of the method is complete.");
		}
	}

	private Exception not_supported()
	{
		return new NotSupportedException("The invoked member is not supported in a dynamic module.");
	}

	private Exception not_after_created()
	{
		return new InvalidOperationException("Unable to change after type has been created.");
	}

	private Exception not_created()
	{
		return new NotSupportedException("The type is not yet created.");
	}
}
