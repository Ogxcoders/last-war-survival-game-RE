using System.Reflection;

namespace System.Xml.Linq;

internal static class XHelper
{
	internal static bool IsInstanceOfType(object o, Type type)
	{
		if (o == null)
		{
			return false;
		}
		return type.GetTypeInfo().IsAssignableFrom(o.GetType().GetTypeInfo());
	}
}
