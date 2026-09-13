using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl;

public abstract class FileSystemSecurity : NativeObjectSecurity
{
	public override Type AccessRightType => typeof(FileSystemRights);

	public override Type AccessRuleType => typeof(FileSystemAccessRule);

	public override Type AuditRuleType => typeof(FileSystemAuditRule);

	internal FileSystemSecurity(bool isContainer)
		: base(isContainer, ResourceType.FileObject)
	{
	}

	internal FileSystemSecurity(bool isContainer, string name, AccessControlSections includeSections)
		: base(isContainer, ResourceType.FileObject, name, includeSections)
	{
	}

	internal FileSystemSecurity(bool isContainer, SafeHandle handle, AccessControlSections includeSections)
		: base(isContainer, ResourceType.FileObject, handle, includeSections)
	{
	}

	public sealed override AccessRule AccessRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
	{
		return new FileSystemAccessRule(identityReference, (FileSystemRights)accessMask, isInherited, inheritanceFlags, propagationFlags, type);
	}

	public void AddAccessRule(FileSystemAccessRule rule)
	{
		AddAccessRule((AccessRule)rule);
	}

	public bool RemoveAccessRule(FileSystemAccessRule rule)
	{
		return RemoveAccessRule((AccessRule)rule);
	}

	public void RemoveAccessRuleAll(FileSystemAccessRule rule)
	{
		RemoveAccessRuleAll((AccessRule)rule);
	}

	public void RemoveAccessRuleSpecific(FileSystemAccessRule rule)
	{
		RemoveAccessRuleSpecific((AccessRule)rule);
	}

	public void ResetAccessRule(FileSystemAccessRule rule)
	{
		ResetAccessRule((AccessRule)rule);
	}

	public void SetAccessRule(FileSystemAccessRule rule)
	{
		SetAccessRule((AccessRule)rule);
	}

	public sealed override AuditRule AuditRuleFactory(IdentityReference identityReference, int accessMask, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AuditFlags flags)
	{
		return new FileSystemAuditRule(identityReference, (FileSystemRights)accessMask, isInherited, inheritanceFlags, propagationFlags, flags);
	}

	public void AddAuditRule(FileSystemAuditRule rule)
	{
		AddAuditRule((AuditRule)rule);
	}

	public bool RemoveAuditRule(FileSystemAuditRule rule)
	{
		return RemoveAuditRule((AuditRule)rule);
	}

	public void RemoveAuditRuleAll(FileSystemAuditRule rule)
	{
		RemoveAuditRuleAll((AuditRule)rule);
	}

	public void RemoveAuditRuleSpecific(FileSystemAuditRule rule)
	{
		RemoveAuditRuleSpecific((AuditRule)rule);
	}

	public void SetAuditRule(FileSystemAuditRule rule)
	{
		SetAuditRule((AuditRule)rule);
	}
}
