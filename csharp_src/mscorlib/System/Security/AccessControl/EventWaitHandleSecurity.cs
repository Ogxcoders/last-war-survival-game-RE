using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class EventWaitHandleSecurity : NativeObjectSecurity
{
	public override Type AccessRightType => typeof(EventWaitHandleRights);

	public override Type AccessRuleType => typeof(EventWaitHandleAccessRule);

	public override Type AuditRuleType => typeof(EventWaitHandleAuditRule);

	public EventWaitHandleSecurity()
		: base(isContainer: false, ResourceType.KernelObject)
	{
	}

	internal EventWaitHandleSecurity(SafeHandle handle, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.KernelObject, handle, includeSections)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new EventWaitHandleAccessRule(identityReference, (EventWaitHandleRights)accessMask, type);
	}

	public void AddAccessRule(EventWaitHandleAccessRule rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public bool RemoveAccessRule(EventWaitHandleAccessRule rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public void RemoveAccessRuleAll(EventWaitHandleAccessRule rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public void RemoveAccessRuleSpecific(EventWaitHandleAccessRule rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public void ResetAccessRule(EventWaitHandleAccessRule rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public void SetAccessRule(EventWaitHandleAccessRule rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new EventWaitHandleAuditRule(identityReference, (EventWaitHandleRights)accessMask, flags);
	}

	public void AddAuditRule(EventWaitHandleAuditRule rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public bool RemoveAuditRule(EventWaitHandleAuditRule rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public void RemoveAuditRuleAll(EventWaitHandleAuditRule rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public void RemoveAuditRuleSpecific(EventWaitHandleAuditRule rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public void SetAuditRule(EventWaitHandleAuditRule rule)
	{
		SetAuditRule((AuditRule)rule);
	}

	internal void Persist(SafeHandle handle)
	{
		PersistModifications(handle);
	}
}
