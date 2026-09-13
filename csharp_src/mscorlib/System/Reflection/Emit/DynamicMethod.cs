using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
[ComVisible(true)]
public sealed class DynamicMethod : MethodInfo
{
	private static class AnonHostModuleHolder
	{
		public static readonly Module anon_host_module;

		public static Module AnonHostModule => anon_host_module;

		static AnonHostModuleHolder()
		{
			AssemblyName name = new AssemblyName
			{
				Name = "Anonymously Hosted DynamicMethods Assembly"
			};
			anon_host_module = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run).GetManifestModule();
		}
	}

	private RuntimeMethodHandle mhandle;

	private string name;

	private Type returnType;

	private Type[] parameters;

	private MethodAttributes attributes;

	private CallingConventions callingConvention;

	private Module module;

	private bool skipVisibility;

	private bool init_locals = true;

	private ILGenerator ilgen;

	private int nrefs;

	private object[] refs;

	private IntPtr referenced_by;

	private Type owner;

	private Delegate deleg;

	private RuntimeMethodInfo method;

	private ParameterBuilder[] pinfo;

	internal bool creating;

	private DynamicILInfo il_info;

	public override MethodAttributes Attributes => attributes;

	public override CallingConventions CallingConvention => callingConvention;

	public override Type DeclaringType => null;

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

	public override RuntimeMethodHandle MethodHandle => mhandle;

	public override Module Module => module;

	public override string Name => name;

	public override Type ReflectedType => null;

	[MonoTODO("Not implemented")]
	public override ParameterInfo ReturnParameter
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override Type ReturnType => returnType;

	[MonoTODO("Not implemented")]
	public override ICustomAttributeProvider ReturnTypeCustomAttributes
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m)
		: this(name, returnType, parameterTypes, m, skipVisibility: false)
	{
	}

	public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
		: this(name, returnType, parameterTypes, owner, skipVisibility: false)
	{
	}

	public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		: this(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, m, skipVisibility)
	{
	}

	public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
		: this(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, owner, skipVisibility)
	{
	}

	public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
		: this(name, attributes, callingConvention, returnType, parameterTypes, owner, owner.Module, skipVisibility, anonHosted: false)
	{
	}

	public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Module m, bool skipVisibility)
		: this(name, attributes, callingConvention, returnType, parameterTypes, null, m, skipVisibility, anonHosted: false)
	{
	}

	public DynamicMethod(string name, Type returnType, Type[] parameterTypes)
		: this(name, returnType, parameterTypes, restrictedSkipVisibility: false)
	{
	}

	[MonoTODO("Visibility is not restricted")]
	public DynamicMethod(string name, Type returnType, Type[] parameterTypes, bool restrictedSkipVisibility)
		: this(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnType, parameterTypes, null, null, restrictedSkipVisibility, anonHosted: true)
	{
	}

	private DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, Module m, bool skipVisibility, bool anonHosted)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (returnType == null)
		{
			returnType = typeof(void);
		}
		if (m == null && !anonHosted)
		{
			throw new ArgumentNullException("m");
		}
		if (returnType.IsByRef)
		{
			throw new ArgumentException("Return type can't be a byref type", "returnType");
		}
		if (parameterTypes != null)
		{
			for (int i = 0; i < parameterTypes.Length; i++)
			{
				if (parameterTypes[i] == null)
				{
					throw new ArgumentException("Parameter " + i + " is null", "parameterTypes");
				}
			}
		}
		if (owner != null && (owner.IsArray || owner.IsInterface))
		{
			throw new ArgumentException("Owner can't be an array or an interface.");
		}
		if (m == null)
		{
			m = AnonHostModuleHolder.AnonHostModule;
		}
		this.name = name;
		this.attributes = attributes | MethodAttributes.Static;
		this.callingConvention = callingConvention;
		this.returnType = returnType;
		parameters = parameterTypes;
		this.owner = owner;
		module = m;
		this.skipVisibility = skipVisibility;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void create_dynamic_method(DynamicMethod m);

	private void CreateDynMethod()
	{
		lock (this)
		{
			if (!(mhandle.Value == IntPtr.Zero))
			{
				return;
			}
			if (ilgen == null || ilgen.ILOffset == 0)
			{
				throw new InvalidOperationException("Method '" + name + "' does not have a method body.");
			}
			ilgen.label_fixup(this);
			try
			{
				creating = true;
				if (refs != null)
				{
					for (int i = 0; i < refs.Length; i++)
					{
						if (refs[i] is DynamicMethod)
						{
							DynamicMethod dynamicMethod = (DynamicMethod)refs[i];
							if (!dynamicMethod.creating)
							{
								dynamicMethod.CreateDynMethod();
							}
						}
					}
				}
			}
			finally
			{
				creating = false;
			}
			create_dynamic_method(this);
			ilgen = null;
		}
	}

	[ComVisible(true)]
	public sealed override Delegate CreateDelegate(Type delegateType)
	{
		if (delegateType == null)
		{
			throw new ArgumentNullException("delegateType");
		}
		if ((object)deleg != null)
		{
			return deleg;
		}
		CreateDynMethod();
		deleg = Delegate.CreateDelegate(delegateType, null, this);
		return deleg;
	}

	[ComVisible(true)]
	public sealed override Delegate CreateDelegate(Type delegateType, object target)
	{
		if (delegateType == null)
		{
			throw new ArgumentNullException("delegateType");
		}
		CreateDynMethod();
		return Delegate.CreateDelegate(delegateType, target, this);
	}

	public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
	{
		if (position < 0 || position > parameters.Length)
		{
			throw new ArgumentOutOfRangeException("position");
		}
		RejectIfCreated();
		ParameterBuilder parameterBuilder = new ParameterBuilder(this, position, attributes, parameterName);
		if (pinfo == null)
		{
			pinfo = new ParameterBuilder[parameters.Length + 1];
		}
		pinfo[position] = parameterBuilder;
		return parameterBuilder;
	}

	public override MethodInfo GetBaseDefinition()
	{
		return this;
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return new object[1]
		{
			new MethodImplAttribute(GetMethodImplementationFlags())
		};
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		if (attributeType == null)
		{
			throw new ArgumentNullException("attributeType");
		}
		if (attributeType.IsAssignableFrom(typeof(MethodImplAttribute)))
		{
			return new object[1]
			{
				new MethodImplAttribute(GetMethodImplementationFlags())
			};
		}
		return EmptyArray<object>.Value;
	}

	public DynamicILInfo GetDynamicILInfo()
	{
		if (il_info == null)
		{
			il_info = new DynamicILInfo(this);
		}
		return il_info;
	}

	public ILGenerator GetILGenerator()
	{
		return GetILGenerator(64);
	}

	public ILGenerator GetILGenerator(int streamSize)
	{
		if ((GetMethodImplementationFlags() & MethodImplAttributes.CodeTypeMask) != MethodImplAttributes.IL || (GetMethodImplementationFlags() & MethodImplAttributes.ManagedMask) != MethodImplAttributes.IL)
		{
			throw new InvalidOperationException("Method body should not exist.");
		}
		if (ilgen != null)
		{
			return ilgen;
		}
		ilgen = new ILGenerator(Module, new DynamicMethodTokenGenerator(this), streamSize);
		return ilgen;
	}

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		return MethodImplAttributes.NoInlining;
	}

	public override ParameterInfo[] GetParameters()
	{
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
		if (parameters != null)
		{
			return parameters.Length;
		}
		return 0;
	}

	internal override Type GetParameterType(int pos)
	{
		return parameters[pos];
	}

	public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		try
		{
			CreateDynMethod();
			if (method == null)
			{
				method = new RuntimeMethodInfo(mhandle);
			}
			return method.Invoke(obj, invokeAttr, binder, parameters, culture);
		}
		catch (MethodAccessException inner)
		{
			throw new TargetInvocationException("Method cannot be invoked.", inner);
		}
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		if (attributeType == null)
		{
			throw new ArgumentNullException("attributeType");
		}
		if (attributeType.IsAssignableFrom(typeof(MethodImplAttribute)))
		{
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		string text = string.Empty;
		ParameterInfo[] parametersInternal = GetParametersInternal();
		for (int i = 0; i < parametersInternal.Length; i++)
		{
			if (i > 0)
			{
				text += ", ";
			}
			text += parametersInternal[i].ParameterType.Name;
		}
		return ReturnType.Name + " " + Name + "(" + text + ")";
	}

	private void RejectIfCreated()
	{
		if (mhandle.Value != IntPtr.Zero)
		{
			throw new InvalidOperationException("Type definition of the method is complete.");
		}
	}

	internal int AddRef(object reference)
	{
		if (refs == null)
		{
			refs = new object[4];
		}
		if (nrefs >= refs.Length - 1)
		{
			object[] destinationArray = new object[refs.Length * 2];
			Array.Copy(refs, destinationArray, refs.Length);
			refs = destinationArray;
		}
		refs[nrefs] = reference;
		refs[nrefs + 1] = null;
		nrefs += 2;
		return nrefs - 1;
	}
}
