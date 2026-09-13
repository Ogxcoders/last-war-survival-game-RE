using DG.Tweening;
using UnityEngine;

public class SandWormMono : MonoBehaviour
{
	[SerializeField]
	private SimpleAnimation anim;

	[SerializeField]
	private SpriteRenderer slider;

	[SerializeField]
	private SuperTextMesh text;

	[SerializeField]
	private TouchObjectEventTrigger touchEvent;

	[SerializeField]
	private GameObject hpBar;

	private const float oriHpBarWidth = 3f;

	private float curRatio = 1f;

	private Tweener _hpAnimation;

	private int pointId;

	private long uuid;

	private const string BornVFX = "Assets/Main/SeasonRes/S3/Prefabs/Effect/SmallSandwormBornVFX.prefab";

	private ITimer hideHpBarTimer;

	public void Dispose()
	{
		if (hideHpBarTimer != null)
		{
			GameEntry.Timer.CancelTimer(hideHpBarTimer);
			hideHpBarTimer = null;
		}
		hpBar?.SetActive(value: true);
	}

	private void Awake()
	{
		if (touchEvent != null)
		{
			touchEvent.onPointerClick = OnClick;
			touchEvent.previewIconPath = "Assets/Main/Sprites/LodIcon/lyp_daditu_jijieguai.png";
			touchEvent.previewType = WorldPreviewType.Boss;
		}
	}

	private void OnDestroy()
	{
		if (touchEvent != null)
		{
			touchEvent.onPointerClick = null;
			touchEvent.previewIconPath = null;
			touchEvent.previewName = null;
		}
	}

	public void Init(SandWormData param, int pointId, long uuid)
	{
		hpBar?.SetActive(value: true);
		slider.transform.localPosition = Vector3.zero;
		text.transform.localScale = Vector3.one;
		float num = (float)param.curHp / (float)param.maxHp;
		SetHpBar(num, num, ani: false);
		this.pointId = pointId;
		this.uuid = uuid;
		if (touchEvent != null)
		{
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", param.monsterId, "level");
			string templateData2 = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", param.monsterId, "name");
			touchEvent.previewName = GameEntry.Localization.GetString("science_condition", templateData, GameEntry.Localization.GetString(templateData2));
		}
	}

	public void Refresh(SandWormData param)
	{
		SetHpBar(curRatio, (float)param.curHp / (float)param.maxHp, ani: true);
	}

	private void SetHpBar(float fromRatio, float toRatio, bool ani)
	{
		if (ani)
		{
			if (_hpAnimation != null)
			{
				_hpAnimation.Kill();
			}
			_hpAnimation = DOTween.To(() => fromRatio, delegate(float b)
			{
				curRatio = b;
				float num3 = (fromRatio - b) / (fromRatio - toRatio);
				float num4 = 0f;
				if (num3 < 0.5f)
				{
					num4 = 0.5f * (num3 / 0.5f) + 1f;
					text.transform.localScale = new Vector3(num4, num4, num4);
				}
				else if (num3 < 1f)
				{
					num4 = 1.5f - 0.5f * ((num3 - 0.5f) / 0.5f);
					text.transform.localScale = new Vector3(num4, num4, num4);
				}
				float num5 = 3f * b;
				float num6 = (3f - num5) / 2f;
				slider.size = new Vector2(num5, slider.size.y);
				slider.transform.localPosition = new Vector3(0f - num6, 0f, 0f);
				text.text = (b * 100f).ToString("F2") + "%";
			}, toRatio, 1f).OnComplete(delegate
			{
				_hpAnimation = null;
				float value2 = toRatio;
				text.transform.localScale = Vector3.one;
				value2 = Mathf.Clamp(value2, 0f, 1f);
				float num3 = 3f * value2;
				float num4 = (3f - num3) / 2f;
				slider.size = new Vector2(num3, slider.size.y);
				slider.transform.localPosition = new Vector3(0f - num4, 0f, 0f);
				text.text = (toRatio * 100f).ToString("F2") + "%";
			});
		}
		else
		{
			curRatio = toRatio;
			float value = toRatio;
			value = Mathf.Clamp(value, 0f, 1f);
			float num = 3f * value;
			float num2 = (3f - num) / 2f;
			slider.size = new Vector2(num, slider.size.y);
			slider.transform.localPosition = new Vector3(0f - num2, 0f, 0f);
			text.text = (toRatio * 100f).ToString("F2") + "%";
		}
	}

	public void PlayAnim(SandWormAnim animEnum)
	{
		switch (animEnum)
		{
		case SandWormAnim.Idle:
			anim.Play("idle");
			break;
		case SandWormAnim.Appear:
			anim.Rewind("born");
			anim.Play("born");
			hpBar.SetActive(value: false);
			if (SceneManager.World is WorldScene worldScene)
			{
				worldScene.CreateVFX("Assets/Main/SeasonRes/S3/Prefabs/Effect/SmallSandwormBornVFX.prefab", base.transform.position, 2f);
			}
			hideHpBarTimer = GameEntry.Timer.RegisterTimer(4f, delegate
			{
				hpBar?.SetActive(value: true);
			});
			break;
		case SandWormAnim.Die:
			hpBar.SetActive(value: false);
			anim.Rewind("dead");
			anim.Play("dead");
			break;
		case SandWormAnim.AttackOnce:
			anim.Rewind("hit");
			anim.Play("hit");
			break;
		}
	}

	public void AttackOnce(Vector3 attackDir)
	{
		PlayAnim(SandWormAnim.AttackOnce);
	}

	private void OnClick()
	{
		GameEntry.Lua.Call("UIUtil.OnClickSandWorm", pointId, uuid);
	}
}
