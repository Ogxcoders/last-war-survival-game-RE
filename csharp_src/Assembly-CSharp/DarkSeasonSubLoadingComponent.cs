using GameFramework.Localization;
using UnityEngine;

public class DarkSeasonSubLoadingComponent : BaseSubLoadingComponent
{
	public override void SetBg()
	{
		string imagePath = "Assets/Main/SingleSprites/cfm_loading_S4.png";
		SetImage(imagePath, background);
	}

	public override void SetIcon()
	{
		string text = "Assets/Main/Loading/Season/cfm_logo_S4.png";
		Language userLanguage = GameEntry.Setting.UserLanguage;
		Transform transform = base.transform.Find("Icon");
		if (transform != null && transform.TryGetComponent<LanguageLogoSelector>(out var component))
		{
			text = component.GetLogoPathFromConfig(userLanguage);
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "Assets/Main/Loading/Season/cfm_logo_S4.png";
		}
		SetImage(text, logoImage, isSetNativeSize: true, isBg: false);
	}
}
