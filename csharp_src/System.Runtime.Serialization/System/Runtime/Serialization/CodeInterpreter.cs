using System.Reflection;

namespace System.Runtime.Serialization;

internal static class CodeInterpreter
{
	internal static object ConvertValue(object arg, Type source, Type target)
	{
		return InternalConvert(arg, source, target, isAddress: false);
	}

	private static bool CanConvert(TypeCode typeCode)
	{
		if ((uint)(typeCode - 3) <= 11u)
		{
			return true;
		}
		return false;
	}

	private static object InternalConvert(object arg, Type source, Type target, bool isAddress)
	{
		if (target == source)
		{
			return arg;
		}
		if (target.IsValueType)
		{
			if (source.IsValueType)
			{
				if (!CanConvert(Type.GetTypeCode(target)))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An internal error has occurred. No conversion is possible to '{0}' - error generating code for serialization.", DataContract.GetClrTypeFullName(target))));
				}
				return target;
			}
			if (source.IsAssignableFrom(target))
			{
				return arg;
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An internal error has occurred. '{0}' is not assignable from '{1}' - error generating code for serialization.", DataContract.GetClrTypeFullName(target), DataContract.GetClrTypeFullName(source))));
		}
		if (target.IsAssignableFrom(source))
		{
			return arg;
		}
		if (source.IsAssignableFrom(target))
		{
			return arg;
		}
		if (target.IsInterface || source.IsInterface)
		{
			return arg;
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("An internal error has occurred. '{0}' is not assignable from '{1}' - error generating code for serialization.", DataContract.GetClrTypeFullName(target), DataContract.GetClrTypeFullName(source))));
	}

	public static object GetMember(MemberInfo memberInfo, object instance)
	{
		PropertyInfo propertyInfo = memberInfo as PropertyInfo;
		if (propertyInfo != null)
		{
			return propertyInfo.GetValue(instance);
		}
		return ((FieldInfo)memberInfo).GetValue(instance);
	}

	public static void SetMember(MemberInfo memberInfo, object instance, object value)
	{
		PropertyInfo propertyInfo = memberInfo as PropertyInfo;
		if (propertyInfo != null)
		{
			propertyInfo.SetValue(instance, value);
		}
		else
		{
			((FieldInfo)memberInfo).SetValue(instance, value);
		}
	}
}
