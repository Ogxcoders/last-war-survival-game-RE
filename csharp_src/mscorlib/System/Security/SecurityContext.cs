using System.Security.Permissions;
using System.Threading;

namespace System.Security;

public sealed class SecurityContext : IDisposable
{
	private SecurityContext()
	{
	}

	public SecurityContext CreateCopy()
	{
		return this;
	}

	public static SecurityContext Capture()
	{
		return new SecurityContext();
	}

	public void Dispose()
	{
	}

	public static bool IsFlowSuppressed()
	{
		return false;
	}

	public static bool IsWindowsIdentityFlowSuppressed()
	{
		return false;
	}

	public static void RestoreFlow()
	{
	}

	[SecurityPermission(SecurityAction.Assert, ControlPrincipal = true)]
	[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
	public static void Run(SecurityContext securityContext, ContextCallback callback, object state)
	{
		callback(state);
	}

	[SecurityPermission(SecurityAction.LinkDemand, Infrastructure = true)]
	public static AsyncFlowControl SuppressFlow()
	{
		throw new NotSupportedException();
	}

	public static AsyncFlowControl SuppressFlowWindowsIdentity()
	{
		throw new NotSupportedException();
	}
}
