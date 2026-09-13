namespace UnityEngine.UIElements;

public class MouseDownEvent : MouseEventBase<MouseDownEvent>
{
	public new static MouseDownEvent GetPooled(Event systemEvent)
	{
		if (systemEvent != null)
		{
			PointerDeviceState.PressButton(PointerId.mousePointerId, systemEvent.button);
		}
		return MouseEventBase<MouseDownEvent>.GetPooled(systemEvent);
	}

	private static MouseDownEvent MakeFromPointerEvent(IPointerEvent pointerEvent)
	{
		if (pointerEvent != null && pointerEvent.button >= 0)
		{
			PointerDeviceState.PressButton(PointerId.mousePointerId, pointerEvent.button);
		}
		return MouseEventBase<MouseDownEvent>.GetPooled(pointerEvent);
	}

	internal static MouseDownEvent GetPooled(PointerDownEvent pointerEvent)
	{
		return MakeFromPointerEvent(pointerEvent);
	}

	internal static MouseDownEvent GetPooled(PointerMoveEvent pointerEvent)
	{
		return MakeFromPointerEvent(pointerEvent);
	}
}
