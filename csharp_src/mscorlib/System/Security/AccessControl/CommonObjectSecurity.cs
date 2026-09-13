namespace System.Security.AccessControl;

public abstract class CommonObjectSecurity : ObjectSecurity
{
	protected CommonObjectSecurity(bool isContainer)
		: base(isContainer, isDS: false)
	{
	}

	internal CommonObjectSecurity(CommonSecurityDescriptor securityDescriptor)
		: base(securityDescriptor)
	{
	}

	public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		return InternalGetAccessRules(includeExplicit, includeInherited, targetType);
	}

	public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		return InternalGetAuditRules(includeExplicit, includeInherited, targetType);
	}

	protected void AddAccessRule(AccessRule rule)
	{
		ModifyAccess(AccessControlModification.Add, rule, out var _);
	}

	protected bool RemoveAccessRule(AccessRule rule)
	{
		bool modified;
		return ModifyAccess(AccessControlModification.Remove, rule, out modified);
	}

	protected void RemoveAccessRuleAll(AccessRule rule)
	{
		ModifyAccess(AccessControlModification.RemoveAll, rule, out var _);
	}

	protected void RemoveAccessRuleSpecific(AccessRule rule)
	{
		ModifyAccess(AccessControlModification.RemoveSpecific, rule, out var _);
	}

	protected void ResetAccessRule(AccessRule rule)
	{
		ModifyAccess(AccessControlModification.Reset, rule, out var _);
	}

	protected void SetAccessRule(AccessRule rule)
	{
		ModifyAccess(AccessControlModification.Set, rule, out var _);
	}

	protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		modified = true;
		WriteLock();
		try
		{
			switch (modification)
			{
			case AccessControlModification.Add:
				descriptor.DiscretionaryAcl.AddAccess(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				break;
			case AccessControlModification.Set:
				descriptor.DiscretionaryAcl.SetAccess(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				break;
			case AccessControlModification.Reset:
				PurgeAccessRules(rule.IdentityReference);
				goto case AccessControlModification.Add;
			case AccessControlModification.Remove:
				modified = descriptor.DiscretionaryAcl.RemoveAccess(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				break;
			case AccessControlModification.RemoveAll:
				PurgeAccessRules(rule.IdentityReference);
				break;
			case AccessControlModification.RemoveSpecific:
				descriptor.DiscretionaryAcl.RemoveAccessSpecific(rule.AccessControlType, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				break;
			default:
				throw new ArgumentOutOfRangeException("modification");
			}
			if (modified)
			{
				base.AccessRulesModified = true;
			}
		}
		finally
		{
			WriteUnlock();
		}
		return modified;
	}

	protected void AddAuditRule(AuditRule rule)
	{
		ModifyAudit(AccessControlModification.Add, rule, out var _);
	}

	protected bool RemoveAuditRule(AuditRule rule)
	{
		bool modified;
		return ModifyAudit(AccessControlModification.Remove, rule, out modified);
	}

	protected void RemoveAuditRuleAll(AuditRule rule)
	{
		ModifyAudit(AccessControlModification.RemoveAll, rule, out var _);
	}

	protected void RemoveAuditRuleSpecific(AuditRule rule)
	{
		ModifyAudit(AccessControlModification.RemoveSpecific, rule, out var _);
	}

	protected void SetAuditRule(AuditRule rule)
	{
		ModifyAudit(AccessControlModification.Set, rule, out var _);
	}

	protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		modified = true;
		WriteLock();
		try
		{
			switch (modification)
			{
			case AccessControlModification.Add:
				if (descriptor.SystemAcl == null)
				{
					descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
				}
				descriptor.SystemAcl.AddAudit(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				break;
			case AccessControlModification.Set:
				if (descriptor.SystemAcl == null)
				{
					descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
				}
				descriptor.SystemAcl.SetAudit(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				break;
			case AccessControlModification.Remove:
				if (descriptor.SystemAcl == null)
				{
					modified = false;
				}
				else
				{
					modified = descriptor.SystemAcl.RemoveAudit(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				}
				break;
			case AccessControlModification.RemoveAll:
				PurgeAuditRules(rule.IdentityReference);
				break;
			case AccessControlModification.RemoveSpecific:
				if (descriptor.SystemAcl != null)
				{
					descriptor.SystemAcl.RemoveAuditSpecific(rule.AuditFlags, ObjectSecurity.SidFromIR(rule.IdentityReference), rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags);
				}
				break;
			default:
				throw new ArgumentOutOfRangeException("modification");
			case AccessControlModification.Reset:
				break;
			}
			if (modified)
			{
				base.AuditRulesModified = true;
			}
		}
		finally
		{
			WriteUnlock();
		}
		return modified;
	}
}
