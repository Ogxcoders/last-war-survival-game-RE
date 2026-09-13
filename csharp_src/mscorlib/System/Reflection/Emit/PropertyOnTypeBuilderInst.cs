using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[StructLayout(LayoutKind.Sequential)]
internal class PropertyOnTypeBuilderInst : PropertyInfo
{
	private TypeBuilderInstantiation instantiation;

	private PropertyInfo prop;

	public override PropertyAttributes Attributes
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override bool CanRead
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override bool CanWrite
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public override Type PropertyType => instantiation.InflateType(prop.PropertyType);

	public override Type DeclaringType => instantiation.InflateType(prop.DeclaringType);

	public override Type ReflectedType => instantiation;

	public override string Name => prop.Name;

	internal PropertyOnTypeBuilderInst(TypeBuilderInstantiation instantiation, PropertyInfo prop)
	{
		this.instantiation = instantiation;
		this.prop = prop;
	}

	public override MethodInfo[] GetAccessors(bool nonPublic)
	{
		MethodInfo getMethod = GetGetMethod(nonPublic);
		MethodInfo setMethod = GetSetMethod(nonPublic);
		int num = 0;
		if (getMethod != null)
		{
			num++;
		}
		if (setMethod != null)
		{
			num++;
		}
		MethodInfo[] array = new MethodInfo[num];
		num = 0;
		if (getMethod != null)
		{
			array[num++] = getMethod;
		}
		if (setMethod != null)
		{
			array[num] = setMethod;
		}
		return array;
	}

	public override MethodInfo GetGetMethod(bool nonPublic)
	{
		MethodInfo methodInfo = prop.GetGetMethod(nonPublic);
		if (methodInfo != null && prop.DeclaringType == instantiation.generic_type)
		{
			methodInfo = TypeBuilder.GetMethod(instantiation, methodInfo);
		}
		return methodInfo;
	}

	public override ParameterInfo[] GetIndexParameters()
	{
		MethodInfo getMethod = GetGetMethod(nonPublic: true);
		if (getMethod != null)
		{
			return getMethod.GetParameters();
		}
		return EmptyArray<ParameterInfo>.Value;
	}

	public override MethodInfo GetSetMethod(bool nonPublic)
	{
		MethodInfo methodInfo = prop.GetSetMethod(nonPublic);
		if (methodInfo != null && prop.DeclaringType == instantiation.generic_type)
		{
			methodInfo = TypeBuilder.GetMethod(instantiation, methodInfo);
		}
		return methodInfo;
	}

	public override string ToString()
	{
		return $"{PropertyType} {Name}";
	}

	public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
	{
		throw new NotSupportedException();
	}

	public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
	{
		throw new NotSupportedException();
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
}
