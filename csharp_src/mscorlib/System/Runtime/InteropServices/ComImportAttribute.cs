using System.Reflection;

namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class ComImportAttribute : Attribute
{
	internal static Attribute GetCustomAttribute(RuntimeType type)
	{
		if ((type.Attributes & TypeAttributes.Import) == 0)
		{
			return null;
		}
		return new ComImportAttribute();
	}

	internal static bool IsDefined(RuntimeType type)
	{
		return (type.Attributes & TypeAttributes.Import) != 0;
	}
}
