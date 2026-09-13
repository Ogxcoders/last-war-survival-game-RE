namespace UnityEngine.UIElements;

public class DragEnterEvent : DragAndDropEventBase<DragEnterEvent>
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

	public DragEnterEvent()
	{
		LocalInit();
	}
}
