namespace UnityEngine.UIElements;

public sealed class PointerUpEvent : PointerEventBase<PointerUpEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
	}

	public PointerUpEvent()
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
			mouseUpEvent.target.SendEvent(mouseUpEvent);
		}
		panel.ActivateCompatibilityMouseEvents(base.pointerId);
		base.PostDispatch(panel);
	}
}
