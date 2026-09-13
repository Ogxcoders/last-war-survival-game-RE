using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class CryptoKeySecurity : NativeObjectSecurity
{
	public override Type AccessRightType => typeof(CryptoKeyRights);

	public override Type AccessRuleType => typeof(CryptoKeyAccessRule);

	public override Type AuditRuleType => typeof(CryptoKeyAuditRule);

	public CryptoKeySecurity()
		: base(isContainer: false, ResourceType.Unknown)
	{
	}

	public CryptoKeySecurity(CommonSecurityDescriptor securityDescriptor)
		: base(securityDescriptor, ResourceType.Unknown)
	{
	}

	public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new CryptoKeyAccessRule(identityReference, (CryptoKeyRights)accessMask, type);
	}

	public void AddAccessRule(CryptoKeyAccessRule rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public bool RemoveAccessRule(CryptoKeyAccessRule rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public void RemoveAccessRuleAll(CryptoKeyAccessRule rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public void RemoveAccessRuleSpecific(CryptoKeyAccessRule rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public void ResetAccessRule(CryptoKeyAccessRule rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public void SetAccessRule(CryptoKeyAccessRule rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new CryptoKeyAuditRule(identityReference, (CryptoKeyRights)accessMask, flags);
	}

	public void AddAuditRule(CryptoKeyAuditRule rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public bool RemoveAuditRule(CryptoKeyAuditRule rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public void RemoveAuditRuleAll(CryptoKeyAuditRule rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public void RemoveAuditRuleSpecific(CryptoKeyAuditRule rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public void SetAuditRule(CryptoKeyAuditRule rule)
	{
		SetAuditRule((AuditRule)rule);
	}
}
