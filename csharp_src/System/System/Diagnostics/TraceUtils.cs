using System.Collections;
using System.Globalization;

namespace System.Diagnostics;

internal static class TraceUtils
{
	internal static object GetRuntimeObject(string className, Type baseType, string initializeData)
	{
		return null;
	}

	private static object ConvertToBaseTypeOrEnum(string value, Type type)
	{
		if (type.IsEnum)
		{
			return Enum.Parse(type, value, ignoreCase: false);
		}
		return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
	}

	internal static void VerifyAttributes(IDictionary attributes, string[] supportedAttributes, object parent)
	{
	}
}
