using UnityEngine.UI;

public class ScrollRectWithoutEnable : ScrollRect
{
	public override bool IsActive()
	{
		if (base.gameObject.activeSelf)
		{
			return base.content != null;
		}
		return false;
	}
}
