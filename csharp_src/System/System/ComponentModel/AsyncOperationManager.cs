using System.Threading;

namespace System.ComponentModel;

public static class AsyncOperationManager
{
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static SynchronizationContext SynchronizationContext
	{
		get
		{
			if (SynchronizationContext.Current == null)
			{
				SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
			}
			return SynchronizationContext.Current;
		}
		set
		{
			SynchronizationContext.SetSynchronizationContext(value);
		}
	}

	public static AsyncOperation CreateOperation(object userSuppliedState)
	{
		return AsyncOperation.CreateOperation(userSuppliedState, SynchronizationContext);
	}
}
