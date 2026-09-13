using System.Linq;
using System.Reflection;

namespace Microsoft.Internal;

internal static class AttributeServices
{
	public static T[] GetAttributes<T>(this ICustomAttributeProvider attributeProvider) where T : class
	{
		return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit: false);
	}

	public static T[] GetAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit) where T : class
	{
		return (T[])attributeProvider.GetCustomAttributes(typeof(T), inherit);
	}

	public static T GetFirstAttribute<T>(this ICustomAttributeProvider attributeProvider) where T : class
	{
		return attributeProvider.GetAttributes<T>().FirstOrDefault();
	}

	public static T GetFirstAttribute<T>(this ICustomAttributeProvider attributeProvider, bool inherit) where T : class
	{
		return attributeProvider.GetAttributes<T>(inherit).FirstOrDefault();
	}

	public static bool IsAttributeDefined<T>(this ICustomAttributeProvider attributeProvider) where T : class
	{
		return attributeProvider.IsDefined(typeof(T), inherit: false);
	}

	public static bool IsAttributeDefined<T>(this ICustomAttributeProvider attributeProvider, bool inherit) where T : class
	{
		return attributeProvider.IsDefined(typeof(T), inherit);
	}
}
