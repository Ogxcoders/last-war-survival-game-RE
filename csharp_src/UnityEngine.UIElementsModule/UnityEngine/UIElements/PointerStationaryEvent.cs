namespace UnityEngine.UIElements;

public sealed class PointerStationaryEvent : PointerEventBase<PointerStationaryEvent>
{
	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		((IPointerEventInternal)this).recomputeTopElementUnderPointer = true;
	}

	public PointerStationaryEvent()
	{
		LocalInit();
	}
}
