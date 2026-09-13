namespace UnityEngine.UIElements;

public class DragUpdatedEvent : DragAndDropEventBase<DragUpdatedEvent>
{
	public new static DragUpdatedEvent GetPooled(Event systemEvent)
	{
		if (systemEvent != null)
		{
			PointerDeviceState.PressButton(PointerId.mousePointerId, systemEvent.button);
		}
		DragUpdatedEvent dragUpdatedEvent = MouseEventBase<DragUpdatedEvent>.GetPooled(systemEvent);
		dragUpdatedEvent.button = 0;
		return dragUpdatedEvent;
	}

	internal static DragUpdatedEvent GetPooled(PointerMoveEvent pointerEvent)
	{
		return MouseEventBase<DragUpdatedEvent>.GetPooled(pointerEvent);
	}

	protected internal override void PostDispatch(IPanel panel)
	{
		EventBase eventBase = ((IMouseEventInternal)this).sourcePointerEvent as EventBase;
		if (eventBase == null)
		{
			(panel as BaseVisualElementPanel)?.CommitElementUnderPointers();
		}
		base.PostDispatch(panel);
	}
}
