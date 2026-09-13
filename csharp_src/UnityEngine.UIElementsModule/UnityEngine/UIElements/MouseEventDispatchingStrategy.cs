namespace UnityEngine.UIElements;

internal class MouseEventDispatchingStrategy : IEventDispatchingStrategy
{
	public bool CanDispatchEvent(EventBase evt)
	{
		return evt is IMouseEvent;
	}

	public void DispatchEvent(EventBase evt, IPanel panel)
	{
		if (!(evt is IMouseEvent mouseEvent))
		{
			return;
		}
		BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
		bool flag = true;
		if ((IMouseEventInternal)mouseEvent != null)
		{
			flag = ((IMouseEventInternal)mouseEvent).recomputeTopElementUnderMouse;
		}
		VisualElement visualElement = ((!flag) ? baseVisualElementPanel?.GetTopElementUnderPointer(PointerId.mousePointerId) : baseVisualElementPanel?.Pick(mouseEvent.mousePosition));
		if (evt.target == null && visualElement != null)
		{
			evt.propagateToIMGUI = false;
			evt.target = visualElement;
		}
		else if (evt.target == null && visualElement == null)
		{
			evt.target = panel?.visualTree;
		}
		else if (evt.target != null)
		{
			evt.propagateToIMGUI = false;
		}
		if (baseVisualElementPanel != null)
		{
			if (evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId() && (evt as MouseLeaveWindowEvent).pressedButtons == 0)
			{
				baseVisualElementPanel.ClearCachedElementUnderPointer(evt);
			}
			else if (flag)
			{
				baseVisualElementPanel.SetElementUnderPointer(visualElement, evt);
			}
		}
		if (evt.target != null)
		{
			EventDispatchUtilities.PropagateEvent(evt);
		}
		IMGUIContainer iMGUIContainer = baseVisualElementPanel?.rootIMGUIContainer;
		if (!evt.isPropagationStopped && panel != null && evt.imguiEvent != null && iMGUIContainer != null)
		{
			if (evt.propagateToIMGUI || evt.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId() || evt.target == iMGUIContainer)
			{
				evt.skipElements.Add(evt.target);
				EventDispatchUtilities.PropagateToIMGUIContainer(panel.visualTree, evt);
			}
			else
			{
				evt.skipElements.Add(evt.target);
				if (!evt.Skip(iMGUIContainer))
				{
					bool canAffectFocus = evt.target is Focusable { focusable: false } focusable && focusable.isIMGUIContainer;
					iMGUIContainer.SendEventToIMGUI(evt, canAffectFocus);
				}
			}
			if (evt.imguiEvent.rawType == EventType.Used)
			{
				evt.StopPropagation();
			}
		}
		evt.stopDispatch = true;
	}
}
