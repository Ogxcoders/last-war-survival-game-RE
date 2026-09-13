namespace System.Runtime.InteropServices.WindowsRuntime;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public sealed class DefaultInterfaceAttribute : Attribute
{
	private Type m_defaultInterface;

	public Type DefaultInterface => m_defaultInterface;

	public DefaultInterfaceAttribute(Type defaultInterface)
	{
		m_defaultInterface = defaultInterface;
	}
}
