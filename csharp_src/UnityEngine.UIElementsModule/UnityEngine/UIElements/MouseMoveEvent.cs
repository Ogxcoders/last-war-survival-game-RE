namespace UnityEngine.UIElements;

public class MouseMoveEvent : MouseEventBase<MouseMoveEvent>
{
	public new static MouseMoveEvent GetPooled(Event systemEvent)
	{
		MouseMoveEvent mouseMoveEvent = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
		mouseMoveEvent.button = 0;
		return mouseMoveEvent;
	}

	internal static MouseMoveEvent GetPooled(PointerMoveEvent pointerEvent)
	{
		return MouseEventBase<MouseMoveEvent>.GetPooled(pointerEvent);
	}
}
