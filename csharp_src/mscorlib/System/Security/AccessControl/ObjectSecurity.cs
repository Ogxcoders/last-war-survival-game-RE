using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.AccessControl;

public abstract class ObjectSecurity
{
	internal CommonSecurityDescriptor descriptor;

	private AccessControlSections sections_modified;

	private ReaderWriterLock rw_lock;

	public abstract Type AccessRightType { get; }

	public abstract Type AccessRuleType { get; }

	public abstract Type AuditRuleType { get; }

	public bool AreAccessRulesCanonical
	{
		get
		{
			ReadLock();
			try
			{
				return descriptor.IsDiscretionaryAclCanonical;
			}
			finally
			{
				ReadUnlock();
			}
		}
	}

	public bool AreAccessRulesProtected
	{
		get
		{
			ReadLock();
			try
			{
				return (descriptor.ControlFlags & ControlFlags.DiscretionaryAclProtected) != 0;
			}
			finally
			{
				ReadUnlock();
			}
		}
	}

	public bool AreAuditRulesCanonical
	{
		get
		{
			ReadLock();
			try
			{
				return descriptor.IsSystemAclCanonical;
			}
			finally
			{
				ReadUnlock();
			}
		}
	}

	public bool AreAuditRulesProtected
	{
		get
		{
			ReadLock();
			try
			{
				return (descriptor.ControlFlags & ControlFlags.SystemAclProtected) != 0;
			}
			finally
			{
				ReadUnlock();
			}
		}
	}

	internal AccessControlSections AccessControlSectionsModified
	{
		get
		{
			Reading();
			return sections_modified;
		}
		set
		{
			Writing();
			sections_modified = value;
		}
	}

	protected bool AccessRulesModified
	{
		get
		{
			return AreAccessControlSectionsModified(AccessControlSections.Access);
		}
		set
		{
			SetAccessControlSectionsModified(AccessControlSections.Access, value);
		}
	}

	protected bool AuditRulesModified
	{
		get
		{
			return AreAccessControlSectionsModified(AccessControlSections.Audit);
		}
		set
		{
			SetAccessControlSectionsModified(AccessControlSections.Audit, value);
		}
	}

	protected bool GroupModified
	{
		get
		{
			return AreAccessControlSectionsModified(AccessControlSections.Group);
		}
		set
		{
			SetAccessControlSectionsModified(AccessControlSections.Group, value);
		}
	}

	protected bool IsContainer => descriptor.IsContainer;

	protected bool IsDS => descriptor.IsDS;

	protected bool OwnerModified
	{
		get
		{
			return AreAccessControlSectionsModified(AccessControlSections.Owner);
		}
		set
		{
			SetAccessControlSectionsModified(AccessControlSections.Owner, value);
		}
	}

	protected ObjectSecurity()
	{
	}

	protected ObjectSecurity(CommonSecurityDescriptor securityDescriptor)
	{
		if (securityDescriptor == null)
		{
			throw new ArgumentNullException("securityDescriptor");
		}
		descriptor = securityDescriptor;
		rw_lock = new ReaderWriterLock();
	}

	protected ObjectSecurity(bool isContainer, bool isDS)
		: this(new CommonSecurityDescriptor(isContainer, isDS, ControlFlags.None, null, null, null, new DiscretionaryAcl(isContainer, isDS, 0)))
	{
	}

	public abstract AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type);

	public abstract AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags);

	public IdentityReference GetGroup(Type targetType)
	{
		ReadLock();
		try
		{
			if (descriptor.Group == null)
			{
				return null;
			}
			return descriptor.Group.Translate(targetType);
		}
		finally
		{
			ReadUnlock();
		}
	}

	public IdentityReference GetOwner(Type targetType)
	{
		ReadLock();
		try
		{
			if (descriptor.Owner == null)
			{
				return null;
			}
			return descriptor.Owner.Translate(targetType);
		}
		finally
		{
			ReadUnlock();
		}
	}

	public byte[] GetSecurityDescriptorBinaryForm()
	{
		ReadLock();
		try
		{
			byte[] array = new byte[descriptor.BinaryLength];
			descriptor.GetBinaryForm(array, 0);
			return array;
		}
		finally
		{
			ReadUnlock();
		}
	}

	public string GetSecurityDescriptorSddlForm(AccessControlSections includeSections)
	{
		ReadLock();
		try
		{
			return descriptor.GetSddlForm(includeSections);
		}
		finally
		{
			ReadUnlock();
		}
	}

	public static bool IsSddlConversionSupported()
	{
		return GenericSecurityDescriptor.IsSddlConversionSupported();
	}

	public virtual bool ModifyAccessRule(AccessControlModification modification, AccessRule rule, out bool modified)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		if (!AccessRuleType.IsAssignableFrom(rule.GetType()))
		{
			throw new ArgumentException("rule");
		}
		return ModifyAccess(modification, rule, out modified);
	}

	public virtual bool ModifyAuditRule(AccessControlModification modification, AuditRule rule, out bool modified)
	{
		if (rule == null)
		{
			throw new ArgumentNullException("rule");
		}
		if (!AuditRuleType.IsAssignableFrom(rule.GetType()))
		{
			throw new ArgumentException("rule");
		}
		return ModifyAudit(modification, rule, out modified);
	}

	public virtual void PurgeAccessRules(IdentityReference identity)
	{
		if (null == identity)
		{
			throw new ArgumentNullException("identity");
		}
		WriteLock();
		try
		{
			descriptor.PurgeAccessControl(SidFromIR(identity));
		}
		finally
		{
			WriteUnlock();
		}
	}

	public virtual void PurgeAuditRules(IdentityReference identity)
	{
		if (null == identity)
		{
			throw new ArgumentNullException("identity");
		}
		WriteLock();
		try
		{
			descriptor.PurgeAudit(SidFromIR(identity));
		}
		finally
		{
			WriteUnlock();
		}
	}

	public void SetAccessRuleProtection(bool isProtected, bool preserveInheritance)
	{
		WriteLock();
		try
		{
			descriptor.SetDiscretionaryAclProtection(isProtected, preserveInheritance);
		}
		finally
		{
			WriteUnlock();
		}
	}

	public void SetAuditRuleProtection(bool isProtected, bool preserveInheritance)
	{
		WriteLock();
		try
		{
			descriptor.SetSystemAclProtection(isProtected, preserveInheritance);
		}
		finally
		{
			WriteUnlock();
		}
	}

	public void SetGroup(IdentityReference identity)
	{
		WriteLock();
		try
		{
			descriptor.Group = SidFromIR(identity);
			GroupModified = true;
		}
		finally
		{
			WriteUnlock();
		}
	}

	public void SetOwner(IdentityReference identity)
	{
		WriteLock();
		try
		{
			descriptor.Owner = SidFromIR(identity);
			OwnerModified = true;
		}
		finally
		{
			WriteUnlock();
		}
	}

	public void SetSecurityDescriptorBinaryForm(byte[] binaryForm)
	{
		SetSecurityDescriptorBinaryForm(binaryForm, AccessControlSections.All);
	}

	public void SetSecurityDescriptorBinaryForm(byte[] binaryForm, AccessControlSections includeSections)
	{
		CopySddlForm(new CommonSecurityDescriptor(IsContainer, IsDS, binaryForm, 0), includeSections);
	}

	public void SetSecurityDescriptorSddlForm(string sddlForm)
	{
		SetSecurityDescriptorSddlForm(sddlForm, AccessControlSections.All);
	}

	public void SetSecurityDescriptorSddlForm(string sddlForm, AccessControlSections includeSections)
	{
		CopySddlForm(new CommonSecurityDescriptor(IsContainer, IsDS, sddlForm), includeSections);
	}

	private void CopySddlForm(CommonSecurityDescriptor sourceDescriptor, AccessControlSections includeSections)
	{
		WriteLock();
		try
		{
			AccessControlSectionsModified |= includeSections;
			if ((includeSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				descriptor.SystemAcl = sourceDescriptor.SystemAcl;
			}
			if ((includeSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				descriptor.DiscretionaryAcl = sourceDescriptor.DiscretionaryAcl;
			}
			if ((includeSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				descriptor.Owner = sourceDescriptor.Owner;
			}
			if ((includeSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				descriptor.Group = sourceDescriptor.Group;
			}
		}
		finally
		{
			WriteUnlock();
		}
	}

	protected abstract bool ModifyAccess(AccessControlModification modification, AccessRule rule, out bool modified);

	protected abstract bool ModifyAudit(AccessControlModification modification, AuditRule rule, out bool modified);

	private Exception GetNotImplementedException()
	{
		return new NotImplementedException();
	}

	protected virtual void Persist(SafeHandle handle, AccessControlSections includeSections)
	{
		throw GetNotImplementedException();
	}

	protected virtual void Persist(string name, AccessControlSections includeSections)
	{
		throw GetNotImplementedException();
	}

	[MonoTODO]
	[HandleProcessCorruptedStateExceptions]
	protected virtual void Persist(bool enableOwnershipPrivilege, string name, AccessControlSections includeSections)
	{
		throw new NotImplementedException();
	}

	private void Reading()
	{
		if (!rw_lock.IsReaderLockHeld && !rw_lock.IsWriterLockHeld)
		{
			throw new InvalidOperationException("Either a read or a write lock must be held.");
		}
	}

	protected void ReadLock()
	{
		rw_lock.AcquireReaderLock(-1);
	}

	protected void ReadUnlock()
	{
		rw_lock.ReleaseReaderLock();
	}

	private void Writing()
	{
		if (!rw_lock.IsWriterLockHeld)
		{
			throw new InvalidOperationException("Write lock must be held.");
		}
	}

	protected void WriteLock()
	{
		rw_lock.AcquireWriterLock(-1);
	}

	protected void WriteUnlock()
	{
		rw_lock.ReleaseWriterLock();
	}

	internal AuthorizationRuleCollection InternalGetAccessRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		List<AuthorizationRule> list = new List<AuthorizationRule>();
		ReadLock();
		try
		{
			AceEnumerator enumerator = descriptor.DiscretionaryAcl.GetEnumerator();
			while (enumerator.MoveNext())
			{
				QualifiedAce qualifiedAce = enumerator.Current as QualifiedAce;
				if (null == qualifiedAce || (qualifiedAce.IsInherited && !includeInherited) || (!qualifiedAce.IsInherited && !includeExplicit))
				{
					continue;
				}
				AccessControlType type;
				if (qualifiedAce.AceQualifier == AceQualifier.AccessAllowed)
				{
					type = AccessControlType.Allow;
				}
				else
				{
					if (AceQualifier.AccessDenied != qualifiedAce.AceQualifier)
					{
						continue;
					}
					type = AccessControlType.Deny;
				}
				AccessRule item = InternalAccessRuleFactory(qualifiedAce, targetType, type);
				list.Add(item);
			}
		}
		finally
		{
			ReadUnlock();
		}
		return new AuthorizationRuleCollection(list.ToArray());
	}

	internal virtual AccessRule InternalAccessRuleFactory(QualifiedAce ace, Type targetType, AccessControlType type)
	{
		return AccessRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, type);
	}

	internal AuthorizationRuleCollection InternalGetAuditRules(bool includeExplicit, bool includeInherited, Type targetType)
	{
		List<AuthorizationRule> list = new List<AuthorizationRule>();
		ReadLock();
		try
		{
			if (descriptor.SystemAcl != null)
			{
				AceEnumerator enumerator = descriptor.SystemAcl.GetEnumerator();
				while (enumerator.MoveNext())
				{
					QualifiedAce qualifiedAce = enumerator.Current as QualifiedAce;
					if (!(null == qualifiedAce) && (!qualifiedAce.IsInherited || includeInherited) && (qualifiedAce.IsInherited || includeExplicit) && AceQualifier.SystemAudit == qualifiedAce.AceQualifier)
					{
						AuditRule item = InternalAuditRuleFactory(qualifiedAce, targetType);
						list.Add(item);
					}
				}
			}
		}
		finally
		{
			ReadUnlock();
		}
		return new AuthorizationRuleCollection(list.ToArray());
	}

	internal virtual AuditRule InternalAuditRuleFactory(QualifiedAce ace, Type targetType)
	{
		return AuditRuleFactory(ace.SecurityIdentifier.Translate(targetType), ace.AccessMask, ace.IsInherited, ace.InheritanceFlags, ace.PropagationFlags, ace.AuditFlags);
	}

	internal static SecurityIdentifier SidFromIR(IdentityReference identity)
	{
		if (null == identity)
		{
			throw new ArgumentNullException("identity");
		}
		return (SecurityIdentifier)identity.Translate(typeof(SecurityIdentifier));
	}

	private bool AreAccessControlSectionsModified(AccessControlSections mask)
	{
		return (AccessControlSectionsModified & mask) != 0;
	}

	private void SetAccessControlSectionsModified(AccessControlSections mask, bool modified)
	{
		if (modified)
		{
			AccessControlSectionsModified |= mask;
		}
		else
		{
			AccessControlSectionsModified &= ~mask;
		}
	}
}
public abstract class ObjectSecurity<T> : NativeObjectSecurity where T : struct
{
	public override Type AccessRightType => typeof(T);

	public override Type AccessRuleType => typeof(AccessRule<T>);

	public override Type AuditRuleType => typeof(AuditRule<T>);

	protected ObjectSecurity(bool isContainer, ResourceType resourceType)
		: base(isContainer, resourceType)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections)
		: base(isContainer, resourceType, safeHandle, includeSections)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections)
		: base(isContainer, resourceType, name, includeSections)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, SafeHandle safeHandle, AccessControlSections includeSections, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
		: base(isContainer, resourceType, safeHandle, includeSections, exceptionFromErrorCode, exceptionContext)
	{
	}

	protected ObjectSecurity(bool isContainer, ResourceType resourceType, string name, AccessControlSections includeSections, ExceptionFromErrorCode exceptionFromErrorCode, object exceptionContext)
		: base(isContainer, resourceType, name, includeSections, exceptionFromErrorCode, exceptionContext)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new AccessRule<T>(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, type);
	}

	public virtual void AddAccessRule(AccessRule<T> rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public virtual bool RemoveAccessRule(AccessRule<T> rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public virtual void RemoveAccessRuleAll(AccessRule<T> rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public virtual void RemoveAccessRuleSpecific(AccessRule<T> rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public virtual void ResetAccessRule(AccessRule<T> rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public virtual void SetAccessRule(AccessRule<T> rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new AuditRule<T>(identityReference, accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
	}

	public virtual void AddAuditRule(AuditRule<T> rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public virtual bool RemoveAuditRule(AuditRule<T> rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public virtual void RemoveAuditRuleAll(AuditRule<T> rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public virtual void RemoveAuditRuleSpecific(AuditRule<T> rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public virtual void SetAuditRule(AuditRule<T> rule)
	{
		SetAuditRule((AuditRule)rule);
	}

	protected void Persist(SafeHandle handle)
	{
		WriteLock();
		try
		{
			Persist(handle, base.AccessControlSectionsModified);
		}
		finally
		{
			WriteUnlock();
		}
	}

	protected void Persist(string name)
	{
		WriteLock();
		try
		{
			Persist(name, base.AccessControlSectionsModified);
		}
		finally
		{
			WriteUnlock();
		}
	}
}
