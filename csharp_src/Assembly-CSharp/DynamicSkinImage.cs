using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[RequireComponent(typeof(MaskableGraphic))]
public class DynamicSkinImage : MonoBehaviour
{
	[Serializable]
	public class SkinInfo
	{
		public SeasonType key;

		public bool invert;

		public bool includePreview;

		public string imgPath;

		public Color color = Color.white;

		public Rect rect = Rect.zero;
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

	public void SwitchSkin(SeasonType type, bool async = false)
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
				ApplySkin(skinInfo, async);
				return;
			}
		}
		skins = m_skins;
		foreach (SkinInfo skinInfo2 in skins)
		{
			if (skinInfo2 != null && skinInfo2.invert && skinInfo2.key != type)
			{
				ApplySkin(skinInfo2, async);
				break;
			}
		}
	}

	public void ApplySkin(SkinInfo skin, bool async = false)
	{
		if (skin != null && !skin.rect.Equals(Rect.zero))
		{
			RectTransform component = GetComponent<RectTransform>();
			if (component != null)
			{
				component.Set_localPosition(skin.rect.x, skin.rect.y, 0f);
				component.Set_sizeDelta(skin.rect.width, skin.rect.height);
			}
		}
		if (skin == null || skin.imgPath.IsNullOrEmpty())
		{
			Image component2 = GetComponent<Image>();
			if (component2 != null)
			{
				component2.sprite = null;
				if (skin != null)
				{
					component2.color = skin.color;
				}
				return;
			}
			RawImage component3 = GetComponent<RawImage>();
			if (component3 != null)
			{
				component3.texture = null;
				if (skin != null)
				{
					component3.color = skin.color;
				}
			}
			return;
		}
		Image component4 = GetComponent<Image>();
		if (component4 != null)
		{
			if (async)
			{
				component4.LoadSpriteAsync(skin.imgPath);
			}
			else
			{
				component4.LoadSprite(skin.imgPath);
			}
			component4.color = skin.color;
			return;
		}
		RawImage component5 = GetComponent<RawImage>();
		if (component5 != null)
		{
			if (async)
			{
				component5.LoadSpriteAsync(skin.imgPath);
			}
			else
			{
				component5.LoadSprite(skin.imgPath);
			}
			component5.color = skin.color;
		}
	}
}
