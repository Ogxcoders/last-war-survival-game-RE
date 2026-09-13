namespace System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
[ComVisible(false)]
public sealed class TypeIdentifierAttribute : Attribute
{
	internal string Scope_;

	internal string Identifier_;

	public string Scope => Scope_;

	public string Identifier => Identifier_;

	public TypeIdentifierAttribute()
	{
	}

	public TypeIdentifierAttribute(string scope, string identifier)
	{
		Scope_ = scope;
		Identifier_ = identifier;
	}
}
