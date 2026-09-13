namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
[ComVisible(true)]
public sealed class LCIDConversionAttribute : Attribute
{
	internal int _val;

	public int Value => _val;

	public LCIDConversionAttribute(int lcid)
	{
		_val = lcid;
	}
}
