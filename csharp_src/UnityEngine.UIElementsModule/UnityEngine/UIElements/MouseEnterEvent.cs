namespace UnityEngine.UIElements;

public class MouseEnterEvent : MouseEventBase<MouseEnterEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.TricklesDown | EventPropagation.Cancellable;
	}

	public MouseEnterEvent()
	{
		LocalInit();
	}
}
