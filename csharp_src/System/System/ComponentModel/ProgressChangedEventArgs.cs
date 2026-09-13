using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public class ProgressChangedEventArgs : EventArgs
{
	private readonly int progressPercentage;

	private readonly object userState;

	[SRDescription("Percentage progress made in operation.")]
	public int ProgressPercentage => progressPercentage;

	[SRDescription("User-supplied state to identify operation.")]
	public object UserState => userState;

	public ProgressChangedEventArgs(int progressPercentage, object userState)
	{
		this.progressPercentage = progressPercentage;
		this.userState = userState;
	}
}
