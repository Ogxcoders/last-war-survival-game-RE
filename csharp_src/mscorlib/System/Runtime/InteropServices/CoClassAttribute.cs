namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class CoClassAttribute : Attribute
{
	internal Type _CoClass;

	public Type CoClass => _CoClass;

	public CoClassAttribute(Type coClass)
	{
		_CoClass = coClass;
	}
}
