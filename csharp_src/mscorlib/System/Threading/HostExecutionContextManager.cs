using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Threading;

public class HostExecutionContextManager
{
	[MonoTODO]
	public virtual HostExecutionContext Capture()
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public virtual void Revert(object previousState)
	{
		throw new NotImplementedException();
	}

	[MonoTODO]
	[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
	public virtual object SetHostExecutionContext(HostExecutionContext hostExecutionContext)
	{
		throw new NotImplementedException();
	}
}
