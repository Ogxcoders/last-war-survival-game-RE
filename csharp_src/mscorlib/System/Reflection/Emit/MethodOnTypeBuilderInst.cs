using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class MethodOnTypeBuilderInst : MethodInfo
{
	private Type instantiation;

	private MethodInfo base_method;

	private Type[] method_arguments;

	private MethodInfo generic_method_definition;

	public override Type DeclaringType => instantiation;

	public override string Name => base_method.Name;

	public override Type ReflectedType => instantiation;

	public override Type ReturnType => base_method.ReturnType;

	public override Module Module => base_method.Module;

	public override int MetadataToken => base.MetadataToken;

	public override RuntimeMethodHandle MethodHandle
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override MethodAttributes Attributes => base_method.Attributes;

	public override CallingConventions CallingConvention => base_method.CallingConvention;

	public override bool ContainsGenericParameters
	{
		get
		{
			if (base_method.ContainsGenericParameters)
			{
				return true;
			}
			if (!base_method.IsGenericMethodDefinition)
			{
				throw new NotSupportedException();
			}
			if (method_arguments == null)
			{
				return true;
			}
			Type[] array = method_arguments;
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

	public override bool IsGenericMethodDefinition
	{
		get
		{
			if (base_method.IsGenericMethodDefinition)
			{
				return method_arguments == null;
			}
			return false;
		}
	}

	public override bool IsGenericMethod => base_method.IsGenericMethodDefinition;

	public override ParameterInfo ReturnParameter
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override ICustomAttributeProvider ReturnTypeCustomAttributes
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public MethodOnTypeBuilderInst(TypeBuilderInstantiation instantiation, MethodInfo base_method)
	{
		this.instantiation = instantiation;
		this.base_method = base_method;
	}

	internal MethodOnTypeBuilderInst(MethodOnTypeBuilderInst gmd, Type[] typeArguments)
	{
		instantiation = gmd.instantiation;
		base_method = gmd.base_method;
		method_arguments = new Type[typeArguments.Length];
		typeArguments.CopyTo(method_arguments, 0);
		generic_method_definition = gmd;
	}

	internal MethodOnTypeBuilderInst(MethodInfo method, Type[] typeArguments)
	{
		instantiation = method.DeclaringType;
		base_method = ExtractBaseMethod(method);
		method_arguments = new Type[typeArguments.Length];
		typeArguments.CopyTo(method_arguments, 0);
		if (base_method != method)
		{
			generic_method_definition = method;
		}
	}

	private static MethodInfo ExtractBaseMethod(MethodInfo info)
	{
		if (info is MethodBuilder)
		{
			return info;
		}
		if (info is MethodOnTypeBuilderInst)
		{
			return ((MethodOnTypeBuilderInst)info).base_method;
		}
		if (info.IsGenericMethod)
		{
			info = info.GetGenericMethodDefinition();
		}
		Type declaringType = info.DeclaringType;
		if (!declaringType.IsGenericType || declaringType.IsGenericTypeDefinition)
		{
			return info;
		}
		return (MethodInfo)declaringType.Module.ResolveMethod(info.MetadataToken);
	}

	internal Type[] GetTypeArgs()
	{
		if (!instantiation.IsGenericType || instantiation.IsGenericParameter)
		{
			return null;
		}
		return instantiation.GetGenericArguments();
	}

	internal MethodInfo RuntimeResolve()
	{
		MethodInfo methodInfo = instantiation.InternalResolve().GetMethod(base_method);
		if (method_arguments != null)
		{
			Type[] array = new Type[method_arguments.Length];
			for (int i = 0; i < method_arguments.Length; i++)
			{
				array[i] = method_arguments[i].InternalResolve();
			}
			methodInfo = methodInfo.MakeGenericMethod(array);
		}
		return methodInfo;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		throw new NotSupportedException();
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		throw new NotSupportedException();
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		throw new NotSupportedException();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(ReturnType.ToString());
		stringBuilder.Append(" ");
		stringBuilder.Append(base_method.Name);
		stringBuilder.Append("(");
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		return base_method.GetMethodImplementationFlags();
	}

	public override ParameterInfo[] GetParameters()
	{
		return GetParametersInternal();
	}

	internal override ParameterInfo[] GetParametersInternal()
	{
		throw new NotSupportedException();
	}

	internal override int GetParametersCount()
	{
		return base_method.GetParametersCount();
	}

	public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		throw new NotSupportedException();
	}

	public override MethodInfo MakeGenericMethod(params Type[] methodInstantiation)
	{
		if (!base_method.IsGenericMethodDefinition || method_arguments != null)
		{
			throw new InvalidOperationException("Method is not a generic method definition");
		}
		if (methodInstantiation == null)
		{
			throw new ArgumentNullException("methodInstantiation");
		}
		if (base_method.GetGenericArguments().Length != methodInstantiation.Length)
		{
			throw new ArgumentException("Incorrect length", "methodInstantiation");
		}
		for (int i = 0; i < methodInstantiation.Length; i++)
		{
			if (methodInstantiation[i] == null)
			{
				throw new ArgumentNullException("methodInstantiation");
			}
		}
		return new MethodOnTypeBuilderInst(this, methodInstantiation);
	}

	public override Type[] GetGenericArguments()
	{
		if (!base_method.IsGenericMethodDefinition)
		{
			return null;
		}
		Type[] obj = method_arguments ?? base_method.GetGenericArguments();
		Type[] array = new Type[obj.Length];
		obj.CopyTo(array, 0);
		return array;
	}

	public override MethodInfo GetGenericMethodDefinition()
	{
		return generic_method_definition ?? base_method;
	}

	public override MethodInfo GetBaseDefinition()
	{
		throw new NotSupportedException();
	}
}
