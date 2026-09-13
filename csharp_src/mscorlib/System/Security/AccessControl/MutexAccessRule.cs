using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class MutexAccessRule : AccessRule
{
	public MutexRights MutexRights => (MutexRights)base.AccessMask;

	public MutexAccessRule(IdentityReference identity, MutexRights eventRights, AccessControlType type)
		: base(identity, (int)eventRights, isInherited: false, InheritanceFlags.None, PropagationFlags.None, type)
	{
	}

	public MutexAccessRule(string identity, MutexRights eventRights, AccessControlType type)
		: this(new NTAccount(identity), eventRights, type)
	{
	}
}
