using System.Runtime.InteropServices;
using System.Security.Principal;

namespace System.Security.AccessControl;

[ComVisible(false)]
public sealed class SemaphoreAccessRule : AccessRule
{
	public SemaphoreRights SemaphoreRights => (SemaphoreRights)base.AccessMask;

	public SemaphoreAccessRule(IdentityReference identity, SemaphoreRights eventRights, AccessControlType type)
		: base(identity, (int)eventRights, isInherited: false, InheritanceFlags.None, PropagationFlags.None, type)
	{
	}

	public SemaphoreAccessRule(string identity, SemaphoreRights eventRights, AccessControlType type)
		: this(new NTAccount(identity), eventRights, type)
	{
	}
}
