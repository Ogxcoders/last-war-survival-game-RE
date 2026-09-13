namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class TypeLibTypeAttribute : Attribute
{
	internal TypeLibTypeFlags _val;

	public TypeLibTypeFlags Value => _val;

	public TypeLibTypeAttribute(TypeLibTypeFlags flags)
	{
		_val = flags;
	}

	public TypeLibTypeAttribute(short flags)
	{
		_val = (TypeLibTypeFlags)flags;
	}
}
