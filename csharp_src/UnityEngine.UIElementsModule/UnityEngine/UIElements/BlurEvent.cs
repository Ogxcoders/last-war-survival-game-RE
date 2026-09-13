namespace UnityEngine.UIElements;

public class BlurEvent : FocusEventBase<BlurEvent>
{
	protected internal override void PreDispatch(IPanel panel)
	{
		base.PreDispatch(panel);
		if (base.relatedTarget == null)
		{
			base.focusController.DoFocusChange(null);
		}
	}
}
