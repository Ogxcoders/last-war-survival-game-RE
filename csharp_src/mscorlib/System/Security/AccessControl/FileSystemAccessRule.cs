using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class FileSystemAccessRule : AccessRule
{
	public FileSystemRights FileSystemRights => (FileSystemRights)base.AccessMask;

	public FileSystemAccessRule(IdentityReference identity, FileSystemRights fileSystemRights, AccessControlType type)
		: this(identity, fileSystemRights, InheritanceFlags.None, PropagationFlags.None, type)
	{
	}

	public FileSystemAccessRule(string identity, FileSystemRights fileSystemRights, AccessControlType type)
		: this(new NTAccount(identity), fileSystemRights, InheritanceFlags.None, PropagationFlags.None, type)
	{
	}

	public FileSystemAccessRule(IdentityReference identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: this(identity, fileSystemRights, isInherited: false, inheritanceFlags, propagationFlags, type)
	{
	}

	internal FileSystemAccessRule(IdentityReference identity, FileSystemRights fileSystemRights, bool isInherited, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: base(identity, (int)fileSystemRights, isInherited, inheritanceFlags, propagationFlags, type)
	{
	}

	public FileSystemAccessRule(string identity, FileSystemRights fileSystemRights, InheritanceFlags inheritanceFlags, PropagationFlags propagationFlags, AccessControlType type)
		: this(new NTAccount(identity), fileSystemRights, inheritanceFlags, propagationFlags, type)
	{
	}
}
