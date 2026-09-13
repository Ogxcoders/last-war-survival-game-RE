using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class DiscretionaryAcl : CommonAcl
{
	public DiscretionaryAcl(bool isContainer, bool isDS, int capacity)
		: base(isContainer, isDS, capacity)
	{
	}

	public DiscretionaryAcl(bool isContainer, bool isDS, RawAcl rawAcl)
		: base(isContainer, isDS, rawAcl)
	{
	}

	public DiscretionaryAcl(bool isContainer, bool isDS, byte revision, int capacity)
		: base(isContainer, isDS, revision, capacity)
	{
	}

	public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		AddAce(GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
	}

	public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		AddAce(GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
	}

	public void AddAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
		AddAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	[MonoTODO]
	public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		throw new NotImplementedException();
	}

	public bool RemoveAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
		return RemoveAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		RemoveAceSpecific(GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
	}

	public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		RemoveAceSpecific(GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
	}

	public void RemoveAccessSpecific(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
		RemoveAccessSpecific(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags)
	{
		SetAce(GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None);
	}

	public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, int accessMask, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, ObjectAceFlags objectFlags, Guid objectType, Guid inheritedObjectType)
	{
		SetAce(GetAceQualifier(accessType), sid, accessMask, inheritanceFlags, propagationFlags, AuditFlags.None, objectFlags, objectType, inheritedObjectType);
	}

	public void SetAccess(AccessControlType accessType, SecurityIdentifier sid, ObjectAccessRule rule)
	{
		SetAccess(accessType, sid, rule.AccessMask, rule.InheritanceFlags, rule.PropagationFlags, rule.ObjectFlags, rule.ObjectType, rule.InheritedObjectType);
	}

	internal override void ApplyCanonicalSortToExplicitAces()
	{
		int canonicalExplicitAceCount = GetCanonicalExplicitAceCount();
		int canonicalExplicitDenyAceCount = GetCanonicalExplicitDenyAceCount();
		ApplyCanonicalSortToExplicitAces(0, canonicalExplicitDenyAceCount);
		ApplyCanonicalSortToExplicitAces(canonicalExplicitDenyAceCount, canonicalExplicitAceCount - canonicalExplicitDenyAceCount);
	}

	internal override int GetAceInsertPosition(AceQualifier aceQualifier)
	{
		if (aceQualifier == AceQualifier.AccessAllowed)
		{
			return GetCanonicalExplicitDenyAceCount();
		}
		return 0;
	}

	private static AceQualifier GetAceQualifier(AccessControlType accessType)
	{
		if (accessType == AccessControlType.Allow)
		{
			return AceQualifier.AccessAllowed;
		}
		if (AccessControlType.Deny == accessType)
		{
			return AceQualifier.AccessDenied;
		}
		throw new ArgumentOutOfRangeException("accessType");
	}

	internal override bool IsAceMeaningless(GenericAce ace)
	{
		if (base.IsAceMeaningless(ace))
		{
			return true;
		}
		if (ace.AuditFlags != AuditFlags.None)
		{
			return true;
		}
		QualifiedAce qualifiedAce = ace as QualifiedAce;
		if (null != qualifiedAce && qualifiedAce.AceQualifier != AceQualifier.AccessAllowed && AceQualifier.AccessDenied != qualifiedAce.AceQualifier)
		{
			return true;
		}
		return false;
	}
}
