namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Field, Inherited = false)]
[ComVisible(true)]
public sealed class TypeLibVarAttribute : Attribute
{
	internal TypeLibVarFlags _val;

	public TypeLibVarFlags Value => _val;

	public TypeLibVarAttribute(TypeLibVarFlags flags)
	{
		_val = flags;
	}

	public TypeLibVarAttribute(short flags)
	{
		_val = (TypeLibVarFlags)flags;
	}
}
