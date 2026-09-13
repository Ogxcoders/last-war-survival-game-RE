namespace UnityEngine.UIElements;

public class FocusOutEvent : FocusEventBase<FocusOutEvent>
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

	public FocusOutEvent()
	{
		LocalInit();
	}
}
