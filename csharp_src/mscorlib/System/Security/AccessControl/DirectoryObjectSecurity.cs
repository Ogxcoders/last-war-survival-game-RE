using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class DirectoryObjectSecurity : ObjectSecurity
{
	protected DirectoryObjectSecurity()
		: base(isContainer: true, isDS: true)
	{
	}

	protected DirectoryObjectSecurity(CommonSecurityDescriptor securityDescriptor)
		: base(securityDescriptor)
	{
	}

	private Exception GetNotImplementedException()
	{
		return new NotImplementedException();
	}

	public virtual AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type, Guid objectType, Guid inheritedObjectType)
	{
		throw GetNotImplementedException();
	}

	internal override AccessRule InternalAccessRuleFactory(QualifiedAce ace, Type targetType, AccessControlType type)
	{
		ObjectAce objectAce = ace as ObjectAce;
		if (null == objectAce || objectAce.ObjectAceFlags == ObjectAceFlags.None)
		{
			return base.InternalAccessRuleFactory(ace, targetType, type);
		}
		return AccessRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, type, objectAce.ObjectAceType, objectAce.InheritedObjectAceType);
	}

	public virtual AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags, Guid objectType, Guid inheritedObjectType)
	{
		throw GetNotImplementedException();
	}

	internal override AuditRule InternalAuditRuleFactory(QualifiedAce ace, Type targetType)
	{
		ObjectAce objectAce = ace as ObjectAce;
		if (null == objectAce || objectAce.ObjectAceFlags == ObjectAceFlags.None)
		{
			return base.InternalAuditRuleFactory(ace, targetType);
		}
		return AuditRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, ace.AuditFlags, objectAce.ObjectAceType, objectAce.InheritedObjectAceType);
	}

	public AuthorizationRuleCollection GetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		return InternalGetAccessRules(includeExplicit, includeInherited, targetType);
	}

	public AuthorizationRuleCollection GetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		return InternalGetAuditRules(includeExplicit, includeInherited, targetType);
	}

	protected void AddAccessRule(ObjectAccessRule rule)
	{
		ModifyAccess(AccessControlModification.Add, rule, out var _);
	}

	protected bool RemoveAccessRule(ObjectAccessRule rule)
	{
		bool modified;
		return ModifyAccess(AccessControlModification.Remove, rule, out modified);
	}

	protected void RemoveAccessRuleAll(ObjectAccessRule rule)
	{
		ModifyAccess(AccessControlModification.RemoveAll, rule, out var _);
	}

	protected void RemoveAccessRuleSpecific(ObjectAccessRule rule)
	{
		ModifyAccess(AccessControlModification.RemoveSpecific, rule, out var _);
	}

	protected void ResetAccessRule(ObjectAccessRule rule)
	{
		ModifyAccess(AccessControlModification.Reset, rule, out var _);
	}

	protected void SetAccessRule(ObjectAccessRule rule)
	{
		ModifyAccess(AccessControlModification.Set, rule, out var _);
	}

	protected override bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		if (!(rule is ObjectAccessRule objectAccessRule))
		{
			throw new ArgumentException("rule");
		}
		modified = true;
		WriteLock();
		try
		{
			switch (modification)
			{
			case AccessControlModification.Add:
				descriptor.DiscretionaryAcl.AddAccess(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), objectAccessRule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
				break;
			case AccessControlModification.Set:
				descriptor.DiscretionaryAcl.SetAccess(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), objectAccessRule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
				break;
			case AccessControlModification.Reset:
				PurgeAccessRules(objectAccessRule.IdentityReference);
				goto case AccessControlModification.Add;
			case AccessControlModification.Remove:
				modified = descriptor.DiscretionaryAcl.RemoveAccess(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), rule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
				break;
			case AccessControlModification.RemoveAll:
				PurgeAccessRules(objectAccessRule.IdentityReference);
				break;
			case AccessControlModification.RemoveSpecific:
				descriptor.DiscretionaryAcl.RemoveAccessSpecific(objectAccessRule.AccessControlType, ObjectSecurity.SidFromIR(objectAccessRule.IdentityReference), objectAccessRule.AccessMask, objectAccessRule.InheritanceFlags, objectAccessRule.PropagationFlags, objectAccessRule.ObjectFlags, objectAccessRule.ObjectType, objectAccessRule.InheritedObjectType);
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

	protected void AddAuditRule(ObjectAuditRule rule)
	{
		ModifyAudit(AccessControlModification.Add, rule, out var _);
	}

	protected bool RemoveAuditRule(ObjectAuditRule rule)
	{
		bool modified;
		return ModifyAudit(AccessControlModification.Remove, rule, out modified);
	}

	protected void RemoveAuditRuleAll(ObjectAuditRule rule)
	{
		ModifyAudit(AccessControlModification.RemoveAll, rule, out var _);
	}

	protected void RemoveAuditRuleSpecific(ObjectAuditRule rule)
	{
		ModifyAudit(AccessControlModification.RemoveSpecific, rule, out var _);
	}

	protected void SetAuditRule(ObjectAuditRule rule)
	{
		ModifyAudit(AccessControlModification.Set, rule, out var _);
	}

	protected override bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		if (!(rule is ObjectAuditRule objectAuditRule))
		{
			throw new ArgumentException("rule");
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
				descriptor.SystemAcl.AddAudit(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
				break;
			case AccessControlModification.Set:
				if (descriptor.SystemAcl == null)
				{
					descriptor.SystemAcl = new SystemAcl(base.IsContainer, base.IsDS, 1);
				}
				descriptor.SystemAcl.SetAudit(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
				break;
			case AccessControlModification.Remove:
				if (descriptor.SystemAcl == null)
				{
					modified = false;
				}
				else
				{
					modified = descriptor.SystemAcl.RemoveAudit(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
				}
				break;
			case AccessControlModification.RemoveAll:
				PurgeAuditRules(objectAuditRule.IdentityReference);
				break;
			case AccessControlModification.RemoveSpecific:
				if (descriptor.SystemAcl != null)
				{
					descriptor.SystemAcl.RemoveAuditSpecific(objectAuditRule.AuditFlags, ObjectSecurity.SidFromIR(objectAuditRule.IdentityReference), objectAuditRule.AccessMask, objectAuditRule.InheritanceFlags, objectAuditRule.PropagationFlags, objectAuditRule.ObjectFlags, objectAuditRule.ObjectType, objectAuditRule.InheritedObjectType);
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
