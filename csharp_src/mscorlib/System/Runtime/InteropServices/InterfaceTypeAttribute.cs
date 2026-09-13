namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class InterfaceTypeAttribute : Attribute
{
	internal ComInterfaceType _val;

	public ComInterfaceType Value => _val;

	public InterfaceTypeAttribute(ComInterfaceType interfaceType)
	{
		_val = interfaceType;
	}

	public InterfaceTypeAttribute(short interfaceType)
	{
		_val = (ComInterfaceType)interfaceType;
	}
}
