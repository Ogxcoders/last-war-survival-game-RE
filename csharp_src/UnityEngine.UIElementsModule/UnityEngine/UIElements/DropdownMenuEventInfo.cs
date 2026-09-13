namespace UnityEngine.UIElements;

public class DropdownMenuEventInfo
{
	public EventModifiers modifiers { get; }

	public Vector2 mousePosition { get; }

	public Vector2 localMousePosition { get; }

	private char character { get; }

	private KeyCode keyCode { get; }

	public DropdownMenuEventInfo(EventBase e)
	{
		if (e is IMouseEvent mouseEvent)
		{
			mousePosition = mouseEvent.mousePosition;
			localMousePosition = mouseEvent.localMousePosition;
			modifiers = mouseEvent.modifiers;
			character = '\0';
			keyCode = KeyCode.None;
		}
		else if (e is IKeyboardEvent keyboardEvent)
		{
			character = keyboardEvent.character;
			keyCode = keyboardEvent.keyCode;
			modifiers = keyboardEvent.modifiers;
			mousePosition = Vector2.zero;
			localMousePosition = Vector2.zero;
		}
	}
}
