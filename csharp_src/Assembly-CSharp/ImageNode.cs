using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ImageNode : IThemedNode
{
	public Image component;

	private string _originPath;

	[HideInInspector]
	public bool parseSuccess;

	public string normalPath;

	public string nightPath;

	public bool ignore;

	public ImageNode(Image component)
	{
		this.component = component;
		_originPath = "";
		if (_originPath.Contains("DefaultSkin"))
		{
			normalPath = _originPath;
			nightPath = _originPath.Replace("DefaultSkin", "NightSkin");
			parseSuccess = true;
		}
		else if (_originPath.Contains("NightSkin"))
		{
			normalPath = _originPath.Replace("NightSkin", "DefaultSkin");
			nightPath = _originPath;
			parseSuccess = true;
		}
	}

	public void ConvertTheme(ThemeMode mode)
	{
		if (!ignore && !(component == null))
		{
			switch (mode)
			{
			case ThemeMode.Night:
				component.LoadSprite(nightPath);
				break;
			case ThemeMode.Normal:
				component.LoadSprite(normalPath);
				break;
			}
		}
	}
}
