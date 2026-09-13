using UnityEngine;
using UnityEngine.UI;

public class HoleMask : Mask
{
	public override bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (!base.isActiveAndEnabled)
		{
			return true;
		}
		return !RectTransformUtility.RectangleContainsScreenPoint(base.rectTransform, sp, eventCamera);
	}
}
