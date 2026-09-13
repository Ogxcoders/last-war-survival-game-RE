using DG.Tweening;
using TMPro;
using UnityEngine;

public class MeteoriteWorldLastKillNotice : MonoBehaviour
{
	public SpriteRenderer srBg;

	public SpriteRenderer srMeteoriteIcon;

	public TextMeshPro tmpRes;

	public SpriteRenderer srMeteoriteIcon2;

	public TextMeshPro tmpRes2;

	public UIPlayerHead playerHead;

	public Transform animNode;

	private Tweener tween;

	public SpriteRenderer srHeadFrame;

	public SpriteRenderer srHead;

	public TextMeshPro tmpTip;

	private float duration;

	public void Init(string uuid, string headPic, int picVer, string iconPic, int score, string iconPic2, int score2, float duration = 2f)
	{
		this.duration = duration;
		tmpRes.text = $"+{score}";
		animNode.localPosition = Vector3.zero;
		tween = animNode.DOMoveY(1.5f, duration * 0.5f);
		srHead.LoadSprite("Assets/Main/Sprites/UI/LWCommon/Sprite/zyf_touxiang_da_hui");
		playerHead.SetData(uuid, headPic, picVer);
		srMeteoriteIcon.LoadSprite(iconPic);
		tmpTip.text = GameEntry.Localization.GetString("yuntieBattle_tips_1030");
		if (!string.IsNullOrEmpty(iconPic2) && score2 > 0)
		{
			tmpRes2.text = $"+{score2}";
			srMeteoriteIcon2.LoadSprite(iconPic2);
			tmpRes2.gameObject.SetActive(value: true);
			srMeteoriteIcon2.gameObject.SetActive(value: true);
			Vector2 size = srBg.size;
			size.x = 4.9f;
			srBg.size = size;
			Vector3 localPosition = playerHead.transform.localPosition;
			localPosition.x = -0.62f;
			playerHead.transform.localPosition = localPosition;
			localPosition = tmpRes.transform.localPosition;
			localPosition.x = -0.21f;
			tmpRes.transform.localPosition = localPosition;
			localPosition = srMeteoriteIcon.transform.localPosition;
			localPosition.x = 0.1f;
			srMeteoriteIcon.transform.localPosition = localPosition;
		}
		else
		{
			tmpRes2.gameObject.SetActive(value: false);
			srMeteoriteIcon2.gameObject.SetActive(value: false);
			Vector2 size2 = srBg.size;
			size2.x = 3.25f;
			srBg.size = size2;
			Vector3 localPosition2 = playerHead.transform.localPosition;
			localPosition2.x = -0.39f;
			playerHead.transform.localPosition = localPosition2;
			localPosition2 = tmpRes.transform.localPosition;
			localPosition2.x = -0.02f;
			tmpRes.transform.localPosition = localPosition2;
			localPosition2 = srMeteoriteIcon.transform.localPosition;
			localPosition2.x = 0.28f;
			srMeteoriteIcon.transform.localPosition = localPosition2;
		}
	}

	public bool DeltaUpdate(int lod, float deltaTime)
	{
		duration -= deltaTime;
		return duration <= 0f;
	}

	public void Dispose()
	{
		if (tween != null)
		{
			tween.Kill();
			tween = null;
		}
	}
}
