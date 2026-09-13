using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class AuditRule : AuthorizationRule
{
	private AuditFlags auditFlags;

	public AuditFlags AuditFlags => auditFlags;

	protected AuditRule(IdentityReference identity, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags auditFlags)
		: base(identity, accessMask, isInherited, inheritanceFlags, propagationFlags)
	{
		if (auditFlags != ((AuditFlags.Success | AuditFlags.Failure) & auditFlags))
		{
			throw new ArgumentException("Invalid audit flags.", "auditFlags");
		}
		this.auditFlags = auditFlags;
	}
}
public class AuditRule<T> : AuditRule where T : struct
{
	public T Rights => (T)(object)base.AccessMask;

	public AuditRule(string identity, T rights, AuditFlags flags)
		: this((IdentityReference)new NTAccount(identity), rights, flags)
	{
	}

	public AuditRule(IdentityReference identity, T rights, AuditFlags flags)
		: this(identity, rights, InheritanceFlags.None, PropagationFlags.None, flags)
	{
	}

	public AuditRule(string identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: this((IdentityReference)new NTAccount(identity), rights, inheritanceFlags, propagationFlags, flags)
	{
	}

	public AuditRule(IdentityReference identity, T rights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: this(identity, (int)(object)rights, isInherited: false, inheritanceFlags, propagationFlags, flags)
	{
	}

	internal AuditRule(IdentityReference identity, int rights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(identity, rights, isInherited, inheritanceFlags, propagationFlags, flags)
	{
	}
}
