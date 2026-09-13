using System.Reflection;

namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[ComVisible(true)]
public sealed class PreserveSigAttribute : Attribute
{
	internal static Attribute GetCustomAttribute(RuntimeMethodInfo method)
	{
		if ((method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) == 0)
		{
			return null;
		}
		return new PreserveSigAttribute();
	}

	internal static bool IsDefined(RuntimeMethodInfo method)
	{
		return (method.GetMethodImplementationFlags() & MethodImplAttributes.PreserveSig) != 0;
	}
}
