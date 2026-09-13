using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class RegistrySecurity : NativeObjectSecurity
{
	public override Type AccessRightType => typeof(RegistryRights);

	public override Type AccessRuleType => typeof(RegistryAccessRule);

	public override Type AuditRuleType => typeof(RegistryAuditRule);

	public RegistrySecurity()
		: base(isContainer: true, ResourceType.RegistryKey)
	{
	}

	internal RegistrySecurity(string name, AccessControlSections includeSections)
		: base(isContainer: true, ResourceType.RegistryKey, name, includeSections)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new RegistryAccessRule(identityReference, (RegistryRights)accessMask, isInherited, inheritanceFlags, propagationFlags, type);
	}

	public void AddAccessRule(RegistryAccessRule rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public bool RemoveAccessRule(RegistryAccessRule rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public void RemoveAccessRuleAll(RegistryAccessRule rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public void RemoveAccessRuleSpecific(RegistryAccessRule rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public void ResetAccessRule(RegistryAccessRule rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public void SetAccessRule(RegistryAccessRule rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new RegistryAuditRule(identityReference, (RegistryRights)accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
	}

	public void AddAuditRule(RegistryAuditRule rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public bool RemoveAuditRule(RegistryAuditRule rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public void RemoveAuditRuleAll(RegistryAuditRule rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public void RemoveAuditRuleSpecific(RegistryAuditRule rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public void SetAuditRule(RegistryAuditRule rule)
	{
		SetAuditRule((AuditRule)rule);
	}
}
