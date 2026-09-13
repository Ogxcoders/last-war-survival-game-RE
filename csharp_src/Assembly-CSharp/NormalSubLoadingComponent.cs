using GameFramework.Localization;
using UnityEngine;

public class NormalSubLoadingComponent : BaseSubLoadingComponent
{
	public override void SetBg()
	{
		string defaultBgPath = _defaultBgPath;
		SetImage(defaultBgPath, background);
	}

	public override void SetIcon()
	{
		string text = _defaultLogoPath;
		Language userLanguage = GameEntry.Setting.UserLanguage;
		Transform transform = base.transform.Find("Icon");
		if (transform != null && transform.TryGetComponent<LanguageLogoSelector>(out var component))
		{
			text = component.GetLogoPathFromConfig(userLanguage);
		}
		if (string.IsNullOrEmpty(text))
		{
			text = _defaultLogoPath;
		}
		SetImage(text, logoImage, isSetNativeSize: true, isBg: false);
	}
}
