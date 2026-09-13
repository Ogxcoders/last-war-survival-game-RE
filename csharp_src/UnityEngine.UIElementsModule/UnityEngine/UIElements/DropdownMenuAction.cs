using System;

namespace UnityEngine.UIElements;

public class DropdownMenuAction : DropdownMenuItem
{
	[Flags]
	public enum Status
	{
		None = 0,
		Normal = 1,
		Disabled = 2,
		Checked = 4,
		Hidden = 8
	}

	private readonly Action<DropdownMenuAction> actionCallback;

	private readonly Func<DropdownMenuAction, Status> actionStatusCallback;

	public string name { get; }

	public Status status { get; private set; }

	public DropdownMenuEventInfo eventInfo { get; private set; }

	public object userData { get; private set; }

	public static Status AlwaysEnabled(DropdownMenuAction a)
	{
		return Status.Normal;
	}

	public static Status AlwaysDisabled(DropdownMenuAction a)
	{
		return Status.Disabled;
	}

	public DropdownMenuAction(string actionName, Action<DropdownMenuAction> actionCallback, Func<DropdownMenuAction, Status> actionStatusCallback, object userData = null)
	{
		name = actionName;
		this.actionCallback = actionCallback;
		this.actionStatusCallback = actionStatusCallback;
		this.userData = userData;
	}

	public void UpdateActionStatus(DropdownMenuEventInfo eventInfo)
	{
		this.eventInfo = eventInfo;
		status = actionStatusCallback?.Invoke(this) ?? Status.Hidden;
	}

	public void Execute()
	{
		actionCallback?.Invoke(this);
	}
}
