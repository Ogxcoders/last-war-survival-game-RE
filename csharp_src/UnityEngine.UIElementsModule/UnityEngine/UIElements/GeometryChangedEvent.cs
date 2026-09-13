namespace UnityEngine.UIElements;

public class GeometryChangedEvent : EventBase<GeometryChangedEvent>
{
	public Rect oldRect { get; private set; }

	public Rect newRect { get; private set; }

	internal int layoutPass { get; set; }

	public static GeometryChangedEvent GetPooled(Rect oldRect, Rect newRect)
	{
		GeometryChangedEvent geometryChangedEvent = EventBase<GeometryChangedEvent>.GetPooled();
		geometryChangedEvent.oldRect = oldRect;
		geometryChangedEvent.newRect = newRect;
		return geometryChangedEvent;
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		oldRect = Rect.zero;
		newRect = Rect.zero;
		layoutPass = 0;
	}

	public GeometryChangedEvent()
	{
		LocalInit();
	}
}
