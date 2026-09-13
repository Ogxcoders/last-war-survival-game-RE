namespace UnityEngine.UIElements;

public abstract class PointerCaptureEventBase<T> : EventBase<T>, IPointerCaptureEvent where T : PointerCaptureEventBase<T>, new()
{
	public IEventHandler relatedTarget { get; private set; }

	public int pointerId { get; private set; }

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown;
		relatedTarget = null;
		pointerId = PointerId.invalidPointerId;
	}

	public static T GetPooled(IEventHandler target, IEventHandler relatedTarget, int pointerId)
	{
		T val = EventBase<T>.GetPooled();
		val.target = target;
		val.relatedTarget = relatedTarget;
		val.pointerId = pointerId;
		return val;
	}

	protected PointerCaptureEventBase()
	{
		LocalInit();
	}
}
