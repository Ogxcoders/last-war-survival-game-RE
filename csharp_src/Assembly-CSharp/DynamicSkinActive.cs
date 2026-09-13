using System;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class DynamicSkinActive : MonoBehaviour
{
	[Serializable]
	public class SkinInfo
	{
		public SeasonType key;

		public bool invert;

		public bool includePreview;

		public bool activeIt;
	}

	[SerializeField]
	public SkinInfo[] m_skins;

	[SerializeField]
	public bool enableIt = true;

	public void Awake()
	{
		if (enableIt)
		{
			SceneSkinMeta baseSkinMeta = SceneSkinManager.Instance.GetBaseSkinMeta();
			if (baseSkinMeta != null)
			{
				SwitchSkin(baseSkinMeta.GetMapType(), baseSkinMeta.GetPreviewType());
			}
		}
	}

	public void SwitchSkin(SeasonType type, SeasonType PreviewType)
	{
		if (m_skins == null || m_skins.Length == 0)
		{
			return;
		}
		if (PreviewType != SeasonType.Nothing)
		{
			SkinInfo[] skins = m_skins;
			foreach (SkinInfo skinInfo in skins)
			{
				if (skinInfo != null && !skinInfo.invert && skinInfo.includePreview && skinInfo.key == PreviewType)
				{
					ApplySkin(skinInfo);
					return;
				}
			}
		}
		SwitchSkin(type);
	}

	public void SwitchSkin(SeasonType type)
	{
		if (m_skins == null || m_skins.Length == 0)
		{
			return;
		}
		SkinInfo[] skins = m_skins;
		foreach (SkinInfo skinInfo in skins)
		{
			if (skinInfo != null && !skinInfo.invert && skinInfo.key == type)
			{
				ApplySkin(skinInfo);
				return;
			}
		}
		skins = m_skins;
		foreach (SkinInfo skinInfo2 in skins)
		{
			if (skinInfo2 != null && skinInfo2.invert && skinInfo2.key != type)
			{
				ApplySkin(skinInfo2);
				break;
			}
		}
	}

	public void ApplySkin(SkinInfo skin)
	{
		if (skin != null && !(base.gameObject == null))
		{
			base.gameObject.SetActive(skin.activeIt);
		}
	}
}
