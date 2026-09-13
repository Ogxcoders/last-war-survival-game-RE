namespace UnityEngine.UIElements;

public class TooltipEvent : EventBase<TooltipEvent>
{
	public string tooltip { get; set; }

	public Rect rect { get; set; }

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown;
		rect = default(Rect);
		tooltip = string.Empty;
	}

	internal static TooltipEvent GetPooled(string tooltip, Rect rect)
	{
		TooltipEvent tooltipEvent = EventBase<TooltipEvent>.GetPooled();
		tooltipEvent.tooltip = tooltip;
		tooltipEvent.rect = rect;
		return tooltipEvent;
	}

	public TooltipEvent()
	{
		LocalInit();
	}
}
