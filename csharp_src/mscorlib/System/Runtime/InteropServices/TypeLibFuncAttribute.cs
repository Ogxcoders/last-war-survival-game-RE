namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[ComVisible(true)]
public sealed class TypeLibFuncAttribute : Attribute
{
	internal TypeLibFuncFlags _val;

	public TypeLibFuncFlags Value => _val;

	public TypeLibFuncAttribute(TypeLibFuncFlags flags)
	{
		_val = flags;
	}

	public TypeLibFuncAttribute(short flags)
	{
		_val = (TypeLibFuncFlags)flags;
	}
}
