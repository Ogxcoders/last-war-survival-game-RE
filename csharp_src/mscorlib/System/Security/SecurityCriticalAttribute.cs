namespace System.Security;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
public sealed class SecurityCriticalAttribute : Attribute
{
	private SecurityCriticalScope _val;

	[Obsolete("SecurityCriticalScope is only used for .NET 2.0 transparency compatibility.")]
	public SecurityCriticalScope Scope => _val;

	public SecurityCriticalAttribute()
	{
	}

	public SecurityCriticalAttribute(SecurityCriticalScope scope)
	{
		_val = scope;
	}
}
