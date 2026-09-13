namespace UnityEngine.UIElements;

internal class PointerEventDispatchingStrategy : IEventDispatchingStrategy
{
	public bool CanDispatchEvent(EventBase evt)
	{
		return evt is IPointerEvent;
	}

	public virtual void DispatchEvent(EventBase evt, IPanel panel)
	{
		if (evt is IPointerEvent pointerEvent)
		{
			BaseVisualElementPanel baseVisualElementPanel = panel as BaseVisualElementPanel;
			bool flag = true;
			if (evt is IPointerEventInternal)
			{
				flag = ((IPointerEventInternal)pointerEvent).recomputeTopElementUnderPointer;
			}
			VisualElement visualElement = ((!flag) ? baseVisualElementPanel?.GetTopElementUnderPointer(pointerEvent.pointerId) : baseVisualElementPanel?.Pick(pointerEvent.position));
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
			if (baseVisualElementPanel != null && flag)
			{
				baseVisualElementPanel.SetElementUnderPointer(visualElement, evt);
			}
			if (evt.target != null)
			{
				EventDispatchUtilities.PropagateEvent(evt);
			}
			evt.stopDispatch = true;
		}
	}
}
