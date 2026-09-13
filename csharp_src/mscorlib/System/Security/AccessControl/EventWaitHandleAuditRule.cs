using System.Security.Principal;

namespace System.Security.AccessControl;

public sealed class EventWaitHandleAuditRule : AuditRule
{
	public EventWaitHandleRights EventWaitHandleRights => (EventWaitHandleRights)base.AccessMask;

	public EventWaitHandleAuditRule(IdentityReference identity, EventWaitHandleRights eventRights, AuditFlags flags)
		: base(identity, (int)eventRights, isInherited: false, InheritanceFlags.None, PropagationFlags.None, flags)
	{
		if (eventRights < EventWaitHandleRights.Modify || eventRights > EventWaitHandleRights.FullControl)
		{
			throw new ArgumentOutOfRangeException("eventRights");
		}
	}
}
