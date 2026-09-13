using DG.Tweening;
using UnityEngine;

public class UIBossHpBar : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer hpSlider;

	[SerializeField]
	private SuperTextMesh hpText;

	private float oriHpBarWidth;

	private Tweener _hpAnimation;

	private void Awake()
	{
		oriHpBarWidth = hpSlider.size.x;
	}

	public void RefreshView(float preHpRatio = 0f, float newHpRatio = 0f, bool ani = false)
	{
		if (oriHpBarWidth == 0f)
		{
			return;
		}
		if (ani)
		{
			if (_hpAnimation != null)
			{
				_hpAnimation.Kill();
			}
			_hpAnimation = DOTween.To(() => preHpRatio, delegate(float b)
			{
				float num = (preHpRatio - b) / (preHpRatio - newHpRatio);
				float size = 1f;
				if (num < 0.5f)
				{
					size = 0.5f * (num / 0.5f) + 1f;
				}
				else if (num < 1f)
				{
					size = 1.5f - 0.5f * ((num - 0.5f) / 0.5f);
				}
				SetHpBarValue(b, size);
			}, newHpRatio, 1f).OnComplete(delegate
			{
				_hpAnimation = null;
				SetHpBarValue(newHpRatio, 1f);
			});
		}
		else
		{
			SetHpBarValue(newHpRatio, 1f);
		}
	}

	private void SetHpBarValue(float ratio, float size)
	{
		float value = ratio * 0.01f;
		value = Mathf.Clamp(value, 0f, 1f);
		float num = oriHpBarWidth * value;
		float num2 = (oriHpBarWidth - num) / 2f;
		hpSlider.size = new Vector2(num, hpSlider.size.y);
		hpSlider.transform.localPosition = new Vector3(0f - num2, 0f, 0f);
		if (0.0001 < (double)ratio && (double)ratio <= 0.01)
		{
			ratio = 0.01f;
		}
		hpText.text = ratio.ToString("F2") + "%";
		hpText.transform.localScale = new Vector3(size, size, size);
	}

	public void Dispose()
	{
		if (_hpAnimation != null)
		{
			_hpAnimation.Kill();
		}
		_hpAnimation = null;
	}
}
