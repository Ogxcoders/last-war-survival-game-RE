using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class ConstructorOnTypeBuilderInst : ConstructorInfo
{
	internal TypeBuilderInstantiation instantiation;

	internal ConstructorInfo cb;

	public override Type DeclaringType => instantiation;

	public override string Name => cb.Name;

	public override Type ReflectedType => instantiation;

	public override Module Module => cb.Module;

	public override int MetadataToken => base.MetadataToken;

	public override RuntimeMethodHandle MethodHandle => cb.MethodHandle;

	public override MethodAttributes Attributes => cb.Attributes;

	public override CallingConventions CallingConvention => cb.CallingConvention;

	public override bool ContainsGenericParameters => false;

	public override bool IsGenericMethodDefinition => false;

	public override bool IsGenericMethod => false;

	public ConstructorOnTypeBuilderInst(TypeBuilderInstantiation instantiation, ConstructorInfo cb)
	{
		this.instantiation = instantiation;
		this.cb = cb;
	}

	public override bool IsDefined(Type attributeType, bool inherit)
	{
		return cb.IsDefined(attributeType, inherit);
	}

	public override object[] GetCustomAttributes(bool inherit)
	{
		return cb.GetCustomAttributes(inherit);
	}

	public override object[] GetCustomAttributes(Type attributeType, bool inherit)
	{
		return cb.GetCustomAttributes(attributeType, inherit);
	}

	public override MethodImplAttributes GetMethodImplementationFlags()
	{
		return cb.GetMethodImplementationFlags();
	}

	public override ParameterInfo[] GetParameters()
	{
		if (!instantiation.IsCreated)
		{
			throw new NotSupportedException();
		}
		return GetParametersInternal();
	}

	internal override ParameterInfo[] GetParametersInternal()
	{
		ParameterInfo[] array;
		if (cb is ConstructorBuilder)
		{
			ConstructorBuilder constructorBuilder = (ConstructorBuilder)cb;
			array = new ParameterInfo[constructorBuilder.parameters.Length];
			for (int i = 0; i < constructorBuilder.parameters.Length; i++)
			{
				Type type = instantiation.InflateType(constructorBuilder.parameters[i]);
				ParameterInfo[] array2 = array;
				int num = i;
				ParameterBuilder[] pinfo = constructorBuilder.pinfo;
				array2[num] = RuntimeParameterInfo.New((pinfo != null) ? pinfo[i] : null, type, this, i + 1);
			}
		}
		else
		{
			ParameterInfo[] parameters = cb.GetParameters();
			array = new ParameterInfo[parameters.Length];
			for (int j = 0; j < parameters.Length; j++)
			{
				Type type2 = instantiation.InflateType(parameters[j].ParameterType);
				array[j] = RuntimeParameterInfo.New(parameters[j], type2, this, j + 1);
			}
		}
		return array;
	}

	internal override Type[] GetParameterTypes()
	{
		if (cb is ConstructorBuilder)
		{
			return (cb as ConstructorBuilder).parameters;
		}
		ParameterInfo[] parameters = cb.GetParameters();
		Type[] array = new Type[parameters.Length];
		for (int i = 0; i < parameters.Length; i++)
		{
			array[i] = parameters[i].ParameterType;
		}
		return array;
	}

	internal ConstructorInfo RuntimeResolve()
	{
		return instantiation.InternalResolve().GetConstructor(cb);
	}

	internal override int GetParametersCount()
	{
		return cb.GetParametersCount();
	}

	public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		return cb.Invoke(obj, invokeAttr, binder, parameters, culture);
	}

	public override Type[] GetGenericArguments()
	{
		return cb.GetGenericArguments();
	}

	public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
	{
		throw new InvalidOperationException();
	}
}
