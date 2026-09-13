namespace UnityEngine.UIElements;

public class MouseLeaveWindowEvent : MouseEventBase<MouseLeaveWindowEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Cancellable;
		((IMouseEventInternal)this).recomputeTopElementUnderMouse = false;
	}

	public MouseLeaveWindowEvent()
	{
		LocalInit();
	}

	public new static MouseLeaveWindowEvent GetPooled(Event systemEvent)
	{
		if (systemEvent != null)
		{
			PointerDeviceState.ReleaseAllButtons(PointerId.mousePointerId);
		}
		return MouseEventBase<MouseLeaveWindowEvent>.GetPooled(systemEvent);
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
