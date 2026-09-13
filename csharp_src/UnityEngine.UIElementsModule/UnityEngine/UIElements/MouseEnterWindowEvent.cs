namespace UnityEngine.UIElements;

public class MouseEnterWindowEvent : MouseEventBase<MouseEnterWindowEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Cancellable;
	}

	public MouseEnterWindowEvent()
	{
		LocalInit();
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
