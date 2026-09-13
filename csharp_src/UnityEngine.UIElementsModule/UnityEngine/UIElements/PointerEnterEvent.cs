namespace UnityEngine.UIElements;

public sealed class PointerEnterEvent : PointerEventBase<PointerEnterEvent>
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

	public PointerEnterEvent()
	{
		LocalInit();
	}
}
