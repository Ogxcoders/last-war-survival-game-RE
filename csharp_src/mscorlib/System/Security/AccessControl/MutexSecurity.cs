using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;

namespace System.Security.AccessControl;

public sealed class MutexSecurity : NativeObjectSecurity
{
	public override Type AccessRightType => typeof(MutexRights);

	public override Type AccessRuleType => typeof(MutexAccessRule);

	public override Type AuditRuleType => typeof(MutexAuditRule);

	public MutexSecurity()
		: base(isContainer: false, ResourceType.KernelObject)
	{
	}

	public MutexSecurity(string name, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.KernelObject, name, includeSections, MutexExceptionFromErrorCode, null)
	{
	}

	internal MutexSecurity(SafeHandle handle, AccessControlSections includeSections)
		: base(isContainer: false, ResourceType.KernelObject, handle, includeSections, MutexExceptionFromErrorCode, null)
	{
	}

	public override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new MutexAccessRule(identityReference, (MutexRights)accessMask, type);
	}

	public void AddAccessRule(MutexAccessRule rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public bool RemoveAccessRule(MutexAccessRule rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public void RemoveAccessRuleAll(MutexAccessRule rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public void RemoveAccessRuleSpecific(MutexAccessRule rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public void ResetAccessRule(MutexAccessRule rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public void SetAccessRule(MutexAccessRule rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new MutexAuditRule(identityReference, (MutexRights)accessMask, flags);
	}

	public void AddAuditRule(MutexAuditRule rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public bool RemoveAuditRule(MutexAuditRule rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public void RemoveAuditRuleAll(MutexAuditRule rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public void RemoveAuditRuleSpecific(MutexAuditRule rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public void SetAuditRule(MutexAuditRule rule)
	{
		SetAuditRule((AuditRule)rule);
	}

	private static Exception MutexExceptionFromErrorCode(int errorCode, string name, SafeHandle handle, object context)
	{
		if (errorCode == 2)
		{
			return new WaitHandleCannotBeOpenedException();
		}
		return NativeObjectSecurity.DefaultExceptionFromErrorCode(errorCode, name, handle, context);
	}
}
