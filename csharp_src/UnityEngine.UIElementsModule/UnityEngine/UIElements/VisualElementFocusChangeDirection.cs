namespace UnityEngine.UIElements;

public class VisualElementFocusChangeDirection : FocusChangeDirection
{
	private static readonly VisualElementFocusChangeDirection s_Left = new VisualElementFocusChangeDirection((int)FocusChangeDirection.lastValue + 1);

	private static readonly VisualElementFocusChangeDirection s_Right = new VisualElementFocusChangeDirection((int)FocusChangeDirection.lastValue + 2);

	public static FocusChangeDirection left => s_Left;

	public static FocusChangeDirection right => s_Right;

	protected new static VisualElementFocusChangeDirection lastValue => s_Right;

	protected VisualElementFocusChangeDirection(int value)
		: base(value)
	{
	}
}
