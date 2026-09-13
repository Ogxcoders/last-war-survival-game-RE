namespace UnityEngine.UIElements;

public sealed class PointerCancelEvent : PointerEventBase<PointerCancelEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown;
		((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
	}

	public PointerCancelEvent()
	{
		LocalInit();
	}

	protected internal override void PostDispatch(IPanel panel)
	{
		if (PointerType.IsDirectManipulationDevice(base.pointerType))
		{
			panel.ReleasePointer(base.pointerId);
			if (panel is BaseVisualElementPanel baseVisualElementPanel)
			{
				baseVisualElementPanel.ClearCachedElementUnderPointer(this);
			}
		}
		if (panel.ShouldSendCompatibilityMouseEvents(this))
		{
			using MouseUpEvent mouseUpEvent = MouseUpEvent.GetPooled(this);
			mouseUpEvent.target = base.target;
			base.target.SendEvent(mouseUpEvent);
		}
		base.PostDispatch(panel);
	}
}
