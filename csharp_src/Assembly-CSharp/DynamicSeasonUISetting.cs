using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DynamicSeasonUISetting : MonoBehaviour
{
	public Image bgImage;

	public Image imgBackBtn;

	public RawImage titleImage;

	public MaskableGraphic titleImageOld;

	public void OnEnable()
	{
		DoSkin();
	}

	public void DoSkin()
	{
		SeasonDataManager.UISkinSetting skinSetting = SeasonDataManager.Instance.SkinSetting;
		if (skinSetting == null)
		{
			return;
		}
		if (bgImage != null && !skinSetting.bgPath.IsNullOrEmpty())
		{
			bgImage.color = skinSetting.bgColor;
			bgImage.LoadSprite(skinSetting.bgPath);
		}
		if (titleImage != null && !skinSetting.titlePath.IsNullOrEmpty())
		{
			DynamicSkinImage component = titleImage.GetComponent<DynamicSkinImage>();
			if (component != null)
			{
				component.enabled = false;
			}
			titleImage.enabled = true;
			titleImage.color = Color.white;
			titleImage.LoadSprite(skinSetting.titlePath);
			if (titleImageOld != null)
			{
				titleImageOld.enabled = false;
			}
		}
		else
		{
			SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
			if (curSkinMeta != null)
			{
				if (titleImage != null)
				{
					DynamicSkinImage component2 = titleImage.GetComponent<DynamicSkinImage>();
					if (component2 != null)
					{
						component2.enabled = true;
						component2.SwitchSkin(curSkinMeta.GetMapType());
						titleImage.enabled = true;
					}
					else
					{
						titleImage.enabled = false;
					}
				}
				if (titleImageOld != null)
				{
					titleImageOld.enabled = true;
					DynamicSkinImage component3 = titleImageOld.GetComponent<DynamicSkinImage>();
					if (component3 != null)
					{
						component3.enabled = true;
						component3.SwitchSkin(curSkinMeta.GetMapType());
					}
				}
			}
		}
		if (imgBackBtn != null && !skinSetting.backPath.IsNullOrEmpty())
		{
			imgBackBtn.LoadSprite(skinSetting.backPath);
		}
	}
}
