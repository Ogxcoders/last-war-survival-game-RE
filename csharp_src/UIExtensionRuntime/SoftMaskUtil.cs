using UnityEngine;
using UnityEngine.UI;

public static class SoftMaskUtil
{
	public static void SetGray(Transform parent, bool bGray)
	{
		SoftMaskable[] componentsInChildren = parent.GetComponentsInChildren<SoftMaskable>(includeInactive: true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetGray(bGray);
		}
	}

	public static void AddSoftMaskable(Transform parent)
	{
		Graphic[] componentsInChildren = parent.GetComponentsInChildren<Graphic>(includeInactive: true);
		foreach (Graphic graphic in componentsInChildren)
		{
			if (graphic.transform.GetComponent<SoftMaskable>() == null)
			{
				graphic.gameObject.AddComponent<SoftMaskable>();
			}
		}
	}
}
