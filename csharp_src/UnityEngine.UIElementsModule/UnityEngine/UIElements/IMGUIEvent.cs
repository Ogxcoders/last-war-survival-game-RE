namespace UnityEngine.UIElements;

public class IMGUIEvent : EventBase<IMGUIEvent>
{
	public static IMGUIEvent GetPooled(Event systemEvent)
	{
		IMGUIEvent iMGUIEvent = EventBase<IMGUIEvent>.GetPooled();
		iMGUIEvent.imguiEvent = systemEvent;
		return iMGUIEvent;
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown | EventPropagation.Cancellable;
	}

	public IMGUIEvent()
	{
		LocalInit();
	}
}
