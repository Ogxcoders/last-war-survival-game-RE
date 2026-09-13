using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class AccessRule : AuthorizationRule
{
	private AccessControlType type;

	public AccessControlType AccessControlType => type;

	protected AccessRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags)
	{
		if (type < AccessControlType.Allow || type > AccessControlType.Deny)
		{
			throw new ArgumentException("Invalid access control type.", "type");
		}
		this.type = type;
	}
}
public class AccessRule<T> : AccessRule where T : struct
{
	public T Rights => (T)(object)base.AccessMask;

	public AccessRule(string identity, T rights, AccessControlType type)
		: this((IdentityReference)new NTAccount(identity), rights, type)
	{
	}

	public AccessRule(IdentityReference identity, T rights, AccessControlType type)
		: this(identity, rights, InheritanceFlags.None, PropagationFlags.None, type)
	{
	}

	public AccessRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: this((IdentityReference)new NTAccount(identity), rights, inheritanceFlags, propagationFlags, type)
	{
	}

	public AccessRule(IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: this(identity, (int)(object)rights, isInherited: false, inheritanceFlags, propagationFlags, type)
	{
	}

	internal AccessRule(IdentityReference identity, int rights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(identity, rights, isInherited, inheritanceFlags, propagationFlags, type)
	{
	}
}
