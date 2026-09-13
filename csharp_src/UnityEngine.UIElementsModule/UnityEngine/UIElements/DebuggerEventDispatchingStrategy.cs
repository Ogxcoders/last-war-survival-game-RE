namespace UnityEngine.UIElements;

internal class DebuggerEventDispatchingStrategy : IEventDispatchingStrategy
{
	public bool CanDispatchEvent(EventBase evt)
	{
		return false;
	}

	public void DispatchEvent(EventBase evt, IPanel panel)
	{
	}

	public void PostDispatch(EventBase evt, IPanel panel)
	{
	}
}
