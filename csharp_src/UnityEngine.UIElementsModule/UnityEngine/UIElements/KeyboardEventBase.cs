namespace UnityEngine.UIElements;

public abstract class KeyboardEventBase<T> : EventBase<T>, IKeyboardEvent where T : KeyboardEventBase<T>, new()
{
	public EventModifiers modifiers { get; protected set; }

	public char character { get; protected set; }

	public KeyCode keyCode { get; protected set; }

	public bool shiftKey => (modifiers & EventModifiers.Shift) != 0;

	public bool ctrlKey => (modifiers & EventModifiers.Control) != 0;

	public bool commandKey => (modifiers & EventModifiers.Command) != 0;

	public bool altKey => (modifiers & EventModifiers.Alt) != 0;

	public bool actionKey
	{
		get
		{
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
			{
				return commandKey;
			}
			return ctrlKey;
		}
	}

	protected override void Init()
	{
		base.Init();
		LocalInit();
	}

	private void LocalInit()
	{
		base.propagation = EventPropagation.Bubbles | EventPropagation.TricklesDown | EventPropagation.Cancellable;
		modifiers = EventModifiers.None;
		character = '\0';
		keyCode = KeyCode.None;
	}

	public static T GetPooled(char c, KeyCode keyCode, EventModifiers modifiers)
	{
		T val = EventBase<T>.GetPooled();
		val.modifiers = modifiers;
		val.character = c;
		val.keyCode = keyCode;
		return val;
	}

	public static T GetPooled(Event systemEvent)
	{
		T val = EventBase<T>.GetPooled();
		val.imguiEvent = systemEvent;
		if (systemEvent != null)
		{
			val.modifiers = systemEvent.modifiers;
			val.character = systemEvent.character;
			val.keyCode = systemEvent.keyCode;
		}
		return val;
	}

	protected KeyboardEventBase()
	{
		LocalInit();
	}
}
