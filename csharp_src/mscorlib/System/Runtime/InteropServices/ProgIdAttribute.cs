namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
[ComVisible(true)]
public sealed class ProgIdAttribute : Attribute
{
	internal string _val;

	public string Value => _val;

	public ProgIdAttribute(string progId)
	{
		_val = progId;
	}
}
