using UnityEngine;
using UnityEngine.UI;

public class ScrollRectDragHelper : MonoBehaviour
{
	public ScrollRect hostScrollRect;

	public ScrollRect targetScrollRect;

	[Range(1f, 100f)]
	public float dragIntencity = 50f;

	public bool freezeTop;

	public bool freezeBottom;

	private string Desc => "一个非常简单及粗糙的辅助滑动组件（ScrollRect）\n主要的作用是:\nA-ScrollRect嵌套了B-ScrollRect\n如果B没有显示完整，那么在滑动B，且触发了B的上下滑动边界时\n会把A一起带走...";

	private void Start()
	{
		if (!(hostScrollRect == null) && !(targetScrollRect == null) && (object)hostScrollRect != targetScrollRect)
		{
			hostScrollRect.onValueChanged.AddListener(OnHostScrollRectValueChanged);
		}
	}

	private void OnHostScrollRectValueChanged(Vector2 value)
	{
		if (value.y > 1.1f && !freezeTop)
		{
			Vector2 normalizedPosition = targetScrollRect.normalizedPosition;
			if (normalizedPosition.y < 1f)
			{
				float y = Mathf.Clamp01(normalizedPosition.y += dragIntencity * 0.001f);
				normalizedPosition.y = y;
				targetScrollRect.normalizedPosition = normalizedPosition;
			}
		}
		else if (value.y < -0.1f && !freezeBottom)
		{
			Vector2 normalizedPosition2 = targetScrollRect.normalizedPosition;
			if (normalizedPosition2.y > 0f)
			{
				float y2 = Mathf.Clamp01(normalizedPosition2.y -= dragIntencity * 0.001f);
				normalizedPosition2.y = y2;
				targetScrollRect.normalizedPosition = normalizedPosition2;
			}
		}
	}
}
