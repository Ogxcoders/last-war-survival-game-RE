namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
[ComVisible(true)]
public sealed class AutomationProxyAttribute : Attribute
{
	internal bool _val;

	public bool Value => _val;

	public AutomationProxyAttribute(bool val)
	{
		_val = val;
	}
}
