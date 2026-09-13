using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RegistryAuditRule : AuditRule
{
	public RegistryRights RegistryRights => (RegistryRights)base.AccessMask;

	public RegistryAuditRule(IdentityReference identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: this(identity, registryRights, isInherited: false, inheritanceFlags, propagationFlags, flags)
	{
	}

	internal RegistryAuditRule(IdentityReference identity, RegistryRights registryRights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: base(identity, (int)registryRights, isInherited, inheritanceFlags, propagationFlags, flags)
	{
	}

	public RegistryAuditRule(string identity, RegistryRights registryRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
		: this(new NTAccount(identity), registryRights, inheritanceFlags, propagationFlags, flags)
	{
	}
}
