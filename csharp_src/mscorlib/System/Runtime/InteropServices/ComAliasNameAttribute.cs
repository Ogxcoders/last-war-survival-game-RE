namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
[ComVisible(true)]
public sealed class ComAliasNameAttribute : Attribute
{
	internal string _val;

	public string Value => _val;

	public ComAliasNameAttribute(string alias)
	{
		_val = alias;
	}
}
