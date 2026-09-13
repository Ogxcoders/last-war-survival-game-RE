namespace UnityEngine.UIElements;

public abstract class PanelChangedEventBase<T> : EventBase<T>, IPanelChangedEvent where T : PanelChangedEventBase<T>, new()
{
	public IPanel originPanel { get; private set; }

	public IPanel destinationPanel { get; private set; }

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		originPanel = null;
		destinationPanel = null;
	}

	public static T GetPooled(IPanel originPanel, IPanel destinationPanel)
	{
		T val = EventBase<T>.GetPooled();
		val.originPanel = originPanel;
		val.destinationPanel = destinationPanel;
		return val;
	}

	protected PanelChangedEventBase()
	{
		LocalInit();
	}
}
