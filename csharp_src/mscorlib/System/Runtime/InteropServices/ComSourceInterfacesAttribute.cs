namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
[ComVisible(true)]
public sealed class ComSourceInterfacesAttribute : Attribute
{
	internal string _val;

	public string Value => _val;

	public ComSourceInterfacesAttribute(string sourceInterfaces)
	{
		_val = sourceInterfaces;
	}

	public ComSourceInterfacesAttribute(Type sourceInterface)
	{
		_val = sourceInterface.FullName;
	}

	public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2)
	{
		_val = sourceInterface1.FullName + "\0" + sourceInterface2.FullName;
	}

	public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3)
	{
		_val = sourceInterface1.FullName + "\0" + sourceInterface2.FullName + "\0" + sourceInterface3.FullName;
	}

	public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4)
	{
		_val = sourceInterface1.FullName + "\0" + sourceInterface2.FullName + "\0" + sourceInterface3.FullName + "\0" + sourceInterface4.FullName;
	}
}
