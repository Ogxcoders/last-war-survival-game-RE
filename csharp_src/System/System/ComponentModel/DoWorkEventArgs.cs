using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public class DoWorkEventArgs : CancelEventArgs
{
	private object result;

	private object argument;

	[SRDescription("Argument passed into the worker handler from BackgroundWorker.RunWorkerAsync.")]
	public object Argument => argument;

	[SRDescription("Result from the worker function.")]
	public object Result
	{
		get
		{
			return result;
		}
		set
		{
			result = value;
		}
	}

	public DoWorkEventArgs(object argument)
	{
		this.argument = argument;
	}
}
