namespace UnityEngine.UIElements;

public class WheelEvent : MouseEventBase<WheelEvent>
{
	public Vector3 delta { get; private set; }

	public new static WheelEvent GetPooled(Event systemEvent)
	{
		WheelEvent wheelEvent = MouseEventBase<WheelEvent>.GetPooled(systemEvent);
		wheelEvent.imguiEvent = systemEvent;
		if (systemEvent != null)
		{
			wheelEvent.delta = systemEvent.delta;
		}
		return wheelEvent;
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		delta = Vector3.zero;
	}

	public WheelEvent()
	{
		LocalInit();
	}
}
