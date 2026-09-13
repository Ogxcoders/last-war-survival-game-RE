namespace UnityEngine.UIElements;

public sealed class PointerLeaveEvent : PointerEventBase<PointerLeaveEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.TricklesDown;
	}

	public PointerLeaveEvent()
	{
		LocalInit();
	}
}
