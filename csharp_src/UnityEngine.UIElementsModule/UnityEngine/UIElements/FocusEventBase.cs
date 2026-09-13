namespace UnityEngine.UIElements;

public abstract class FocusEventBase<T> : EventBase<T>, IFocusEvent where T : FocusEventBase<T>, new()
{
	public Focusable relatedTarget { get; private set; }

	public FocusChangeDirection direction { get; private set; }

	protected FocusController focusController { get; private set; }

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.TricklesDown;
		relatedTarget = null;
		direction = FocusChangeDirection.unspecified;
		focusController = null;
	}

	public static T GetPooled(IEventHandler target, Focusable relatedTarget, FocusChangeDirection direction, FocusController focusController)
	{
		T val = EventBase<T>.GetPooled();
		val.target = target;
		val.relatedTarget = relatedTarget;
		val.direction = direction;
		val.focusController = focusController;
		return val;
	}

	protected FocusEventBase()
	{
		LocalInit();
	}
}
