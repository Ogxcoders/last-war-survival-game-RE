using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class SystemAcl : CommonAcl
{
	public SystemAcl(bool isContainer, bool isDS, int capacity)
		: base(isContainer, isDS, capacity)
	{
	}

	public SystemAcl(bool isContainer, bool isDS, RawAcl rawAcl)
		: base(isContainer, isDS, rawAcl)
	{
	}

	public SystemAcl(bool isContainer, bool isDS, byte revision, int capacity)
		: base(isContainer, isDS, revision, capacity)
	{
	}

	public void AddAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		AddAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
	}

	public void AddAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		AddAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
	}

	public void AddAudit(SecurityIdentifier sid, ObjectAuditRule rule)
	{
		AddAudit(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	[MonoTODO]
	public bool RemoveAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public bool RemoveAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		throw new NotImplementedException();
	}

	public bool RemoveAudit(SecurityIdentifier sid, ObjectAuditRule rule)
	{
		return RemoveAudit(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	public void RemoveAuditSpecific(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		RemoveAceSpecific(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
	}

	public void RemoveAuditSpecific(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		RemoveAceSpecific(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
	}

	public void RemoveAuditSpecific(SecurityIdentifier sid, ObjectAuditRule rule)
	{
		RemoveAuditSpecific(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	public void SetAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		SetAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags);
	}

	public void SetAudit(AuditFlags auditFlags, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		SetAce(AceQualifier.SystemAudit, sid, accessMask, inheritanceFlags, propagationFlags, auditFlags, objectFlags, objectType, inheritedObjectType);
	}

	public void SetAudit(SecurityIdentifier sid, ObjectAuditRule rule)
	{
		SetAudit(rule.AuditFlags, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	internal override void ApplyCanonicalSortToExplicitAces()
	{
		int canonicalExplicitAceCount = GetCanonicalExplicitAceCount();
		ApplyCanonicalSortToExplicitAces(0, canonicalExplicitAceCount);
	}

	internal override int GetAceInsertPosition(AceQualifier aceQualifier)
	{
		return 0;
	}

	internal override bool IsAceMeaningless(GenericAce ace)
	{
		if (base.IsAceMeaningless(ace))
		{
			return true;
		}
		if (!IsValidAuditFlags(ace.AuditFlags))
		{
			return true;
		}
		QualifiedAce qualifiedAce = ace as QualifiedAce;
		if (null != qualifiedAce && AceQualifier.SystemAudit != qualifiedAce.AceQualifier && AceQualifier.SystemAlarm != qualifiedAce.AceQualifier)
		{
			return true;
		}
		return false;
	}

	private static bool IsValidAuditFlags(AuditFlags auditFlags)
	{
		if (auditFlags != AuditFlags.None)
		{
			return auditFlags == ((AuditFlags.Success | AuditFlags.Failure) & auditFlags);
		}
		return false;
	}
}
