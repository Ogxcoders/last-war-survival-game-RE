using TMPro;
using UnityEngine;

public class ArabicSubLoadingComponent : BaseSubLoadingComponent
{
	public RectTransform SpineParent;

	public RectTransform Spine;

	public override void CSOpen()
	{
		loadingText.fontSize = 40f;
		float x = SpineParent.rect.width / 810f;
		Spine.localScale = new Vector3(x, 1f, 1f);
	}

	public override void SetVersionText()
	{
		base.SetVersionText();
		versionText.alignment = TextAlignmentOptions.TopLeft;
	}
}
