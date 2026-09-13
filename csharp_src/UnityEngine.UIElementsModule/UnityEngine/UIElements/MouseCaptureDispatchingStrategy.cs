using System;

namespace UnityEngine.UIElements;

internal class MouseCaptureDispatchingStrategy : IEventDispatchingStrategy
{
	[Flags]
	private enum EventBehavior
	{
		None = 0,
		IsCapturable = 1,
		IsSentExclusivelyToCapturingElement = 2
	}

	public bool CanDispatchEvent(EventBase evt)
	{
		return evt is IMouseEvent || evt.imguiEvent != null;
	}

	public void DispatchEvent(EventBase evt, IPanel panel)
	{
		EventBehavior eventBehavior = EventBehavior.None;
		IEventHandler eventHandler = panel?.GetCapturingElement(PointerId.mousePointerId);
		if (eventHandler == null)
		{
			return;
		}
		VisualElement visualElement = eventHandler as VisualElement;
		if (evt.eventTypeId != EventBase<MouseCaptureOutEvent>.TypeId() && visualElement != null && visualElement.panel == null)
		{
			visualElement.ReleaseMouse();
		}
		else
		{
			if (panel != null && visualElement != null && visualElement.panel.contextType != panel.contextType)
			{
				return;
			}
			IMouseEvent mouseEvent = evt as IMouseEvent;
			if (mouseEvent != null && (evt.target == null || evt.target == eventHandler))
			{
				eventBehavior = EventBehavior.IsCapturable;
				eventBehavior |= EventBehavior.IsSentExclusivelyToCapturingElement;
			}
			else if (evt.imguiEvent != null && evt.target == null)
			{
				eventBehavior = EventBehavior.IsCapturable;
			}
			if (evt.eventTypeId == EventBase<MouseEnterWindowEvent>.TypeId() || evt.eventTypeId == EventBase<MouseLeaveWindowEvent>.TypeId() || evt.eventTypeId == EventBase<WheelEvent>.TypeId())
			{
				eventBehavior = EventBehavior.None;
			}
			if ((eventBehavior & EventBehavior.IsCapturable) != EventBehavior.IsCapturable)
			{
				return;
			}
			BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
			if (mouseEvent != null && baseVisualElementPanel != null)
			{
				bool flag = true;
				if ((IMouseEventInternal)mouseEvent != null)
				{
					flag = ((IMouseEventInternal)mouseEvent).recomputeTopElementUnderMouse;
				}
				VisualElement newElementUnderPointer = (flag ? baseVisualElementPanel.Pick(mouseEvent.mousePosition) : baseVisualElementPanel.GetTopElementUnderPointer(PointerId.mousePointerId));
				if (flag)
				{
					baseVisualElementPanel.SetElementUnderPointer(newElementUnderPointer, evt);
				}
			}
			evt.dispatch = true;
			evt.target = eventHandler;
			evt.currentTarget = eventHandler;
			(eventHandler as CallbackEventHandler)?.HandleEventAtTargetPhase(evt);
			if ((eventBehavior & EventBehavior.IsSentExclusivelyToCapturingElement) != EventBehavior.IsSentExclusivelyToCapturingElement)
			{
				evt.target = null;
			}
			evt.currentTarget = null;
			evt.propagationPhase = PropagationPhase.None;
			evt.dispatch = false;
			evt.skipElements.Add(eventHandler);
			evt.stopDispatch = (eventBehavior & EventBehavior.IsSentExclusivelyToCapturingElement) == EventBehavior.IsSentExclusivelyToCapturingElement;
			if (evt.target is IMGUIContainer)
			{
				evt.propagateToIMGUI = true;
				evt.skipElements.Add(evt.target);
			}
			else
			{
				evt.propagateToIMGUI = false;
			}
		}
	}
}
