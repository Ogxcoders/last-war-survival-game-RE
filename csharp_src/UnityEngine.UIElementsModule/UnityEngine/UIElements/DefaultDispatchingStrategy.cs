namespace UnityEngine.UIElements;

internal class DefaultDispatchingStrategy : IEventDispatchingStrategy
{
	public bool CanDispatchEvent(EventBase evt)
	{
		if (evt is IMGUIEvent)
		{
			return false;
		}
		return true;
	}

	public void DispatchEvent(EventBase evt, IPanel panel)
	{
		if (evt.target != null)
		{
			evt.propagateToIMGUI = evt.target is IMGUIContainer;
			EventDispatchUtilities.PropagateEvent(evt);
		}
		else if (!evt.isPropagationStopped && panel != null && (evt.propagateToIMGUI || evt.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId()))
		{
			EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
		}
		evt.stopDispatch = true;
	}
}
