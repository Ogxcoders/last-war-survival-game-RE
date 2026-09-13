using System;
using UnityEngine;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class DynamicSkinCanvas : MonoBehaviour
{
	[Serializable]
	public class SkinInfo
	{
		public SeasonType key;

		public bool invert;

		public bool includePreview;

		public float alpha = 1f;
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

	private void SwitchSkin(SeasonType type, SeasonType PreviewType)
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
		if (skin != null)
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.alpha = skin.alpha;
			}
		}
	}
}
