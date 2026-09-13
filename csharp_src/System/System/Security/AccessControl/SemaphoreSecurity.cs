using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl;

[ComVisible(false)]
public sealed class SemaphoreSecurity : NativeObjectSecurity
{
	public override Type AccessRightType => typeof(SemaphoreRights);

	public override Type AccessRuleType => typeof(SemaphoreAccessRule);

	public override Type AuditRuleType => typeof(SemaphoreAuditRule);

	public SemaphoreSecurity()
		: base(isContainer: false, ResourceType.KernelObject)
	{
	}

	public SemaphoreSecurity(string name, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.KernelObject, name, includeSections)
	{
	}

	internal SemaphoreSecurity(SafeHandle handle, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.KernelObject, handle, includeSections)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new SemaphoreAccessRule(identityReference, (SemaphoreRights)accessMask, type);
	}

	public void AddAccessRule(SemaphoreAccessRule rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public bool RemoveAccessRule(SemaphoreAccessRule rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public void RemoveAccessRuleAll(SemaphoreAccessRule rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public void RemoveAccessRuleSpecific(SemaphoreAccessRule rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public void ResetAccessRule(SemaphoreAccessRule rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public void SetAccessRule(SemaphoreAccessRule rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new SemaphoreAuditRule(identityReference, (SemaphoreRights)accessMask, flags);
	}

	public void AddAuditRule(SemaphoreAuditRule rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public bool RemoveAuditRule(SemaphoreAuditRule rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public void RemoveAuditRuleAll(SemaphoreAuditRule rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public void RemoveAuditRuleSpecific(SemaphoreAuditRule rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public void SetAuditRule(SemaphoreAuditRule rule)
	{
		SetAuditRule((AuditRule)rule);
	}

	internal void Persist(SafeHandle handle)
	{
		WriteLock();
		try
		{
			Persist(handle, (AccessControlSections)((base.AccessRulesModified ? 2 : 0) | (base.AuditRulesModified ? 1 : 0) | (base.OwnerModified ? 4 : 0) | (base.GroupModified ? 8 : 0)), null);
		}
		finally
		{
			WriteUnlock();
		}
	}
}
