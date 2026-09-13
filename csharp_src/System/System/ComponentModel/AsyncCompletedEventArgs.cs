using System.Reflection;
using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public class AsyncCompletedEventArgs : EventArgs
{
	private readonly Exception error;

	private readonly bool cancelled;

	private readonly object userState;

	[SRDescription("True if operation was cancelled.")]
	public bool Cancelled => cancelled;

	[SRDescription("Exception that occurred during operation.  Null if no error.")]
	public Exception Error => error;

	[SRDescription("User-supplied state to identify operation.")]
	public object UserState => userState;

	[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public AsyncCompletedEventArgs()
	{
	}

	public AsyncCompletedEventArgs(Exception error, bool cancelled, object userState)
	{
		this.error = error;
		this.cancelled = cancelled;
		this.userState = userState;
	}

	protected void RaiseExceptionIfNecessary()
	{
		if (Error != null)
		{
			throw new TargetInvocationException(global::SR.GetString("An exception occurred during the operation, making the result invalid.  Check InnerException for exception details."), Error);
		}
		if (Cancelled)
		{
			throw new InvalidOperationException(global::SR.GetString("Operation has been cancelled."));
		}
	}
}
