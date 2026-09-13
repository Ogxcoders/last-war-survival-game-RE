using UnityEngine;

public class InputControl : MonoBehaviour
{
	private bool isPress;

	private bool isClick;

	private bool tempPress;

	private Vector2 oldMousePosition;

	private Vector2 tempMousePosition;

	public const float JUDGE_DISTANCE = 1f;

	public event MouseDownEvent EVENT_MOUSE_DOWN;

	public event MouseUpEvent EVENT_MOUSE_UP;

	public event MouseDragEvent EVENT_MOUSE_DRAG;

	public event MouseClickEvent EVENT_MOUSE_CLICK;

	private void Awake()
	{
		EVENT_MOUSE_DOWN += AvoidEmpty;
		EVENT_MOUSE_UP += AvoidEmpty;
		EVENT_MOUSE_DRAG += AvoidEmpty;
		EVENT_MOUSE_CLICK += AvoidEmpty;
	}

	private void Start()
	{
		isPress = false;
		isClick = false;
	}

	private void AvoidEmpty(Vector2 noUse)
	{
	}

	private void Update()
	{
		tempPress = Input.GetMouseButton(0);
		tempMousePosition = Input.mousePosition;
		if (tempPress != isPress)
		{
			if (tempPress)
			{
				isClick = true;
				this.EVENT_MOUSE_DOWN(tempMousePosition);
			}
			else
			{
				this.EVENT_MOUSE_UP(tempMousePosition);
				if (isClick)
				{
					this.EVENT_MOUSE_CLICK(tempMousePosition);
				}
				isClick = false;
			}
		}
		else if (isClick && JudgeMove(oldMousePosition, tempMousePosition))
		{
			isClick = false;
		}
		else if (tempPress && !isClick)
		{
			this.EVENT_MOUSE_DRAG(tempMousePosition - oldMousePosition);
		}
		isPress = tempPress;
		oldMousePosition = tempMousePosition;
	}

	private static bool JudgeMove(Vector2 p1, Vector2 p2)
	{
		if (!(Mathf.Abs(p1.x - p2.x) > 1f))
		{
			return Mathf.Abs(p1.y - p2.y) > 1f;
		}
		return true;
	}
}
