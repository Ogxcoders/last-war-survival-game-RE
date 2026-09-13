namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Module, Inherited = false)]
[ComVisible(true)]
public sealed class DefaultCharSetAttribute : Attribute
{
	internal CharSet _CharSet;

	public CharSet CharSet => _CharSet;

	public DefaultCharSetAttribute(CharSet charSet)
	{
		_CharSet = charSet;
	}
}
