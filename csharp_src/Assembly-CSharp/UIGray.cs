using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class UIGray
{
	private static Material grayMat;

	private static Material etc1GrayMat;

	private const string ETC1GrayShaderName = "UI/GrayETC1";

	private const string NormalGrayShaderName = "Custom/ImageGray";

	private static Material GetEtc1GrayMaterial()
	{
		Material material = etc1GrayMat;
		if (material == null)
		{
			Shader shader = Shader.Find("UI/GrayETC1");
			if (shader == null)
			{
				Debug.LogError("GrayETC1 not found!");
				return null;
			}
			material = (etc1GrayMat = new Material(shader));
		}
		return material;
	}

	private static Material GetNormalGrayMaterial()
	{
		Material material = grayMat;
		if (material == null)
		{
			Shader shader = Shader.Find("Custom/ImageGray");
			if (shader == null)
			{
				Debug.LogError("GetGrayMat shader is null!");
				return null;
			}
			material = (grayMat = new Material(shader));
		}
		return material;
	}

	public static void DoCheckAfterSetSprite(Image image)
	{
		if (image == null || image.sprite == null || image.sprite.texture == null || image.material == null)
		{
			Debug.Log("DoCheckAfterSetSprite image == null or image.sprite == null");
			return;
		}
		bool flag = image.sprite.texture.format == TextureFormat.ETC_RGB4;
		if (image.material.shader.name == "UI/Default" || image.material.shader.name == "UI/DefaultETC1")
		{
			return;
		}
		if (flag)
		{
			if (image.material.shader.name != "UI/GrayETC1")
			{
				image.material = GetEtc1GrayMaterial();
				image.SetMaterialDirty();
			}
		}
		else if (image.material.shader.name != "Custom/ImageGray")
		{
			image.material = GetNormalGrayMaterial();
			image.SetMaterialDirty();
		}
	}

	public static void SetGray(Transform parent, bool bGray, bool canClick = false, bool withGraphic = false)
	{
		Graphic[] componentsInChildren = parent.GetComponentsInChildren<Graphic>(includeInactive: true);
		foreach (Graphic graphic in componentsInChildren)
		{
			if (!(graphic is TMP_SubMeshUI))
			{
				if (graphic is Image { sprite: var sprite } image)
				{
					image.material = ((!bGray) ? null : ((sprite != null && sprite.texture != null && sprite.texture.format == TextureFormat.ETC_RGB4) ? GetEtc1GrayMaterial() : GetNormalGrayMaterial()));
					image.SetMaterialDirty();
				}
				else if (graphic is RawImage rawImage)
				{
					rawImage.material = (bGray ? GetNormalGrayMaterial() : null);
				}
				else
				{
					graphic.material = ((bGray && withGraphic) ? GetNormalGrayMaterial() : null);
				}
			}
		}
		Button[] componentsInChildren2 = parent.GetComponentsInChildren<Button>(includeInactive: true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = canClick;
		}
		Toggle[] componentsInChildren3 = parent.GetComponentsInChildren<Toggle>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].enabled = canClick;
		}
	}

	public static void SetGrayWithIgnore(Transform parent, bool bGray, string ignoreName)
	{
		Image[] componentsInChildren = parent.GetComponentsInChildren<Image>(includeInactive: true);
		foreach (Image image in componentsInChildren)
		{
			if (!(image.name == ignoreName))
			{
				Sprite sprite = image.sprite;
				image.material = ((!bGray) ? null : ((sprite != null && sprite.texture != null && sprite.texture.format == TextureFormat.ETC_RGB4) ? GetEtc1GrayMaterial() : GetNormalGrayMaterial()));
				image.SetMaterialDirty();
			}
		}
	}

	public static void SetGrayNotRecursively(Image image, bool bGray)
	{
		Sprite sprite = image.sprite;
		image.material = ((!bGray) ? null : ((sprite != null && sprite.texture != null && sprite.texture.format == TextureFormat.ETC_RGB4) ? GetEtc1GrayMaterial() : GetNormalGrayMaterial()));
		image.SetMaterialDirty();
	}

	private static void SetGraphicGray(Graphic graphic)
	{
		if (!(graphic == null))
		{
			if (graphic is Image { sprite: var sprite } image)
			{
				image.material = ((sprite != null && sprite.texture != null && sprite.texture.format == TextureFormat.ETC_RGB4) ? GetEtc1GrayMaterial() : GetNormalGrayMaterial());
			}
			else
			{
				graphic.material = GetNormalGrayMaterial();
			}
			graphic.SetMaterialDirty();
		}
	}

	public static void SetGraphicGrayRecursively(Transform parent)
	{
		Graphic[] componentsInChildren = parent.GetComponentsInChildren<Graphic>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			SetGraphicGray(componentsInChildren[i]);
		}
	}

	public static void SetGraphicGrayRecursivelyWithIgnore(Transform parent, string ignore)
	{
		Graphic[] componentsInChildren = parent.GetComponentsInChildren<Graphic>();
		foreach (Graphic graphic in componentsInChildren)
		{
			if (!(graphic.name == ignore))
			{
				SetGraphicGray(graphic);
			}
		}
	}
}
