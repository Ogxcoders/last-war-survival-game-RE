namespace UnityEngine.UIElements;

public class DragExitedEvent : DragAndDropEventBase<DragExitedEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown;
	}

	public DragExitedEvent()
	{
		LocalInit();
	}

	public new static DragExitedEvent GetPooled(Event systemEvent)
	{
		if (systemEvent != null)
		{
			PointerDeviceState.ReleaseButton(PointerId.mousePointerId, systemEvent.button);
		}
		return MouseEventBase<DragExitedEvent>.GetPooled(systemEvent);
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
