namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class ComEventInterfaceAttribute : Attribute
{
	internal Type _SourceInterface;

	internal Type _EventProvider;

	public Type SourceInterface => _SourceInterface;

	public Type EventProvider => _EventProvider;

	public ComEventInterfaceAttribute(Type SourceInterface, Type EventProvider)
	{
		_SourceInterface = SourceInterface;
		_EventProvider = EventProvider;
	}
}
