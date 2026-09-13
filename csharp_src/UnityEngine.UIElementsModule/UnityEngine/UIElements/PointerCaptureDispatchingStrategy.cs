namespace UnityEngine.UIElements;

internal class PointerCaptureDispatchingStrategy : IEventDispatchingStrategy
{
	public bool CanDispatchEvent(EventBase evt)
	{
		return evt is IPointerEvent;
	}

	public void DispatchEvent(EventBase evt, IPanel panel)
	{
		if (!(evt is IPointerEvent pointerEvent))
		{
			return;
		}
		IEventHandler capturingElement = panel.GetCapturingElement(pointerEvent.pointerId);
		if (capturingElement == null)
		{
			return;
		}
		VisualElement visualElement = capturingElement as VisualElement;
		if (evt.eventTypeId != EventBase<PointerCaptureOutEvent>.TypeId() && visualElement != null && visualElement.panel == null)
		{
			panel.ReleasePointer(pointerEvent.pointerId);
		}
		else if (evt.target == null || evt.target == capturingElement)
		{
			if (evt.eventTypeId != EventBase<PointerCaptureEvent>.TypeId() && evt.eventTypeId != EventBase<PointerCaptureOutEvent>.TypeId())
			{
				panel.ProcessPointerCapture(pointerEvent.pointerId);
			}
			evt.dispatch = true;
			evt.target = capturingElement;
			evt.currentTarget = capturingElement;
			evt.propagationPhase = PropagationPhase.AtTarget;
			capturingElement.HandleEvent(evt);
			evt.currentTarget = null;
			evt.propagationPhase = PropagationPhase.None;
			evt.dispatch = false;
			evt.stopDispatch = true;
			evt.propagateToIMGUI = false;
		}
	}
}
