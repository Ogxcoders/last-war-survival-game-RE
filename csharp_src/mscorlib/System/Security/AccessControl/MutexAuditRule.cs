using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class MutexAuditRule : AuditRule
{
	public MutexRights MutexRights => (MutexRights)base.AccessMask;

	public MutexAuditRule(IdentityReference identity, MutexRights eventRights, AuditFlags flags)
		: base(identity, (int)eventRights, isInherited: false, InheritanceFlags.None, PropagationFlags.None, flags)
	{
	}
}
