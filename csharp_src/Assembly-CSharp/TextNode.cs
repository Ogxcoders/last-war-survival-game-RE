using System;
using TMPro;
using UnityEngine;

[Serializable]
public class TextNode : IThemedNode
{
	public TextMeshProUGUI component;

	[HideInInspector]
	public Color originColor;

	[HideInInspector]
	public Material originMaterial;

	[HideInInspector]
	public bool parseSuccess = true;

	[HideInInspector]
	public int index = 2;

	public Color nightColor;

	public Material nightMaterial;

	public TextNode(TextMeshProUGUI component)
	{
		this.component = component;
		UpdateOrigin();
	}

	public void UpdateOrigin()
	{
		originColor = component.color;
		originMaterial = component.fontSharedMaterial;
		UpdateNightConfig();
	}

	private void UpdateNightConfig()
	{
	}

	public void ConvertTheme(ThemeMode mode)
	{
		if (ColorUtility.TryParseHtmlString("#" + ThemeConfigData.Config[index].Color, out var color))
		{
			nightColor = color;
		}
		switch (mode)
		{
		case ThemeMode.Night:
			component.color = nightColor;
			if (nightMaterial != null && originMaterial != null)
			{
				component.fontMaterial = nightMaterial;
			}
			break;
		case ThemeMode.Normal:
			component.color = originColor;
			if (nightMaterial != null && originMaterial != null)
			{
				component.fontMaterial = originMaterial;
			}
			break;
		}
	}
}
