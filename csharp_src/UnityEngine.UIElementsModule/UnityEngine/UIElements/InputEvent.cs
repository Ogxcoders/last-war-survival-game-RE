namespace UnityEngine.UIElements;

public class InputEvent : EventBase<InputEvent>
{
	public string previousData { get; protected set; }

	public string newData { get; protected set; }

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown;
		previousData = null;
		newData = null;
	}

	public static InputEvent GetPooled(string previousData, string newData)
	{
		InputEvent inputEvent = EventBase<InputEvent>.GetPooled();
		inputEvent.previousData = previousData;
		inputEvent.newData = newData;
		return inputEvent;
	}

	public InputEvent()
	{
		LocalInit();
	}
}
