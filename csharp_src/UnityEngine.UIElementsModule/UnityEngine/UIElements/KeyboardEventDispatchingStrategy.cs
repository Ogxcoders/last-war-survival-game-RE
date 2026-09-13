namespace UnityEngine.UIElements;

internal class KeyboardEventDispatchingStrategy : IEventDispatchingStrategy
{
	public bool CanDispatchEvent(EventBase evt)
	{
		return evt is IKeyboardEvent;
	}

	public void DispatchEvent(EventBase evt, IPanel panel)
	{
		if (panel != null)
		{
			Focusable leafFocusedElement = panel.focusController.GetLeafFocusedElement();
			if (leafFocusedElement != null)
			{
				if (leafFocusedElement.isIMGUIContainer)
				{
					IMGUIContainer iMGUIContainer = (IMGUIContainer)leafFocusedElement;
					if (!evt.Skip(iMGUIContainer) && iMGUIContainer.SendEventToIMGUI(evt))
					{
						evt.StopPropagation();
						evt.PreventDefault();
					}
				}
				else
				{
					evt.target = leafFocusedElement;
					EventDispatchUtilities.PropagateEvent(evt);
				}
			}
			else
			{
				evt.target = panel.visualTree;
				EventDispatchUtilities.PropagateEvent(evt);
				if (!evt.isPropagationStopped)
				{
					EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
				}
			}
		}
		evt.propagateToIMGUI = false;
		evt.stopDispatch = true;
	}
}
