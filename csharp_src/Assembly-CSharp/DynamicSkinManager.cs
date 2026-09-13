using System;
using UnityEngine;

public class DynamicSkinManager : MonoBehaviour
{
	private bool async;

	public void Awake()
	{
		SceneSkinMeta baseSkinMeta = SceneSkinManager.Instance.GetBaseSkinMeta();
		if (baseSkinMeta == null)
		{
			return;
		}
		DynamicSkinActive[] componentsInChildren = GetComponentsInChildren<DynamicSkinActive>(includeInactive: true);
		foreach (DynamicSkinActive dynamicSkinActive in componentsInChildren)
		{
			if (dynamicSkinActive != null && dynamicSkinActive.enableIt)
			{
				dynamicSkinActive.SwitchSkin(baseSkinMeta.GetMapType(), baseSkinMeta.GetPreviewType());
			}
		}
	}

	public void SetAsync(bool isAsync)
	{
		async = isAsync;
	}

	public void ActiveSkin(int nSeasonType)
	{
		if (Enum.IsDefined(typeof(SeasonType), (SeasonType)nSeasonType))
		{
			ActiveSkin((SeasonType)nSeasonType);
		}
	}

	public void ActiveSkin(SeasonType theType)
	{
		DynamicSkinImage[] componentsInChildren = GetComponentsInChildren<DynamicSkinImage>(includeInactive: true);
		foreach (DynamicSkinImage dynamicSkinImage in componentsInChildren)
		{
			if (dynamicSkinImage != null)
			{
				dynamicSkinImage.SwitchSkin(theType, async);
			}
		}
		DynamicSkinText[] componentsInChildren2 = GetComponentsInChildren<DynamicSkinText>(includeInactive: true);
		foreach (DynamicSkinText dynamicSkinText in componentsInChildren2)
		{
			if (dynamicSkinText != null)
			{
				dynamicSkinText.SwitchSkin(theType);
			}
		}
		DynamicSkinCanvas[] componentsInChildren3 = GetComponentsInChildren<DynamicSkinCanvas>(includeInactive: true);
		foreach (DynamicSkinCanvas dynamicSkinCanvas in componentsInChildren3)
		{
			if (dynamicSkinCanvas != null)
			{
				dynamicSkinCanvas.SwitchSkin(theType);
			}
		}
		DynamicSkinActive[] componentsInChildren4 = GetComponentsInChildren<DynamicSkinActive>(includeInactive: true);
		foreach (DynamicSkinActive dynamicSkinActive in componentsInChildren4)
		{
			if (dynamicSkinActive != null)
			{
				dynamicSkinActive.SwitchSkin(theType);
			}
		}
	}
}
