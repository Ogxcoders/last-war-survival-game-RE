using DG.Tweening;
using UnityEngine;

public class UIFlowerCarHpBar : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer hpSlider;

	[SerializeField]
	private SpriteRenderer armorSlider;

	[SerializeField]
	private SuperTextMesh ratioText;

	private float oriHpBarWidth;

	private Tweener _tweener;

	private float fromArmor = 100f;

	private float fromHp = 100f;

	private float toArmor = 100f;

	private float toHp = 100f;

	private const float EPSILON = 0.0001f;

	private const float ACCURACY = 0.01f;

	private void Awake()
	{
		oriHpBarWidth = hpSlider.size.x;
	}

	public void SetData(float newHp, float newArmor, bool ani = false)
	{
		if (0.0001f < newHp && newHp < 0.01f)
		{
			newHp = 0.01f;
		}
		if (0.0001f < newArmor && newArmor < 0.01f)
		{
			newArmor = 0.01f;
		}
		if (ani && Mathf.Abs(newHp - toHp) < 0.0001f && Mathf.Abs(newArmor - toArmor) < 0.0001f)
		{
			return;
		}
		fromArmor = toArmor;
		fromHp = toHp;
		toArmor = newArmor;
		toHp = newHp;
		if (ani)
		{
			float num = Mathf.Abs(toArmor - fromArmor);
			float num2 = Mathf.Abs(toHp - fromHp);
			if (num > num2)
			{
				SetArmorSliderAndTextWithAni();
			}
			else
			{
				SetHpSliderAndTextWithAni();
			}
		}
		else
		{
			SetSliderAndText(toArmor, toHp, 1f);
		}
	}

	private void SetArmorSliderAndTextWithAni()
	{
		if (_tweener != null)
		{
			_tweener.Kill();
		}
		_tweener = DOTween.To(() => fromArmor, delegate(float b)
		{
			float num = (fromArmor - b) / (fromArmor - toArmor);
			float size = 1f;
			if (num < 0.5f)
			{
				size = 0.5f * (num / 0.5f) + 1f;
			}
			else if (num < 1f)
			{
				size = 1.5f - 0.5f * ((num - 0.5f) / 0.5f);
			}
			SetSlider(armorSlider, b);
			SetText(b, size);
		}, toArmor, 1f).OnComplete(delegate
		{
			_tweener = null;
			SetSliderAndText(toArmor, toHp, 1f);
		});
	}

	private void SetHpSliderAndTextWithAni()
	{
		if (_tweener != null)
		{
			_tweener.Kill();
		}
		_tweener = DOTween.To(() => fromHp, delegate(float b)
		{
			float num = (fromHp - b) / (fromHp - toHp);
			float size = 1f;
			if (num < 0.5f)
			{
				size = 0.5f * (num / 0.5f) + 1f;
			}
			else if (num < 1f)
			{
				size = 1.5f - 0.5f * ((num - 0.5f) / 0.5f);
			}
			SetSlider(hpSlider, b);
			SetText(b, size);
		}, toHp, 1f).OnComplete(delegate
		{
			_tweener = null;
			SetSliderAndText(toArmor, toHp, 1f);
		});
	}

	private void SetSliderAndText(float armor, float hp, float size)
	{
		SetSlider(armorSlider, armor);
		SetSlider(hpSlider, hp);
		SetText((0.0001f < armor) ? armor : hp, size);
	}

	private void SetSlider(SpriteRenderer slider, float ratio)
	{
		float value = ratio * 0.01f;
		value = Mathf.Clamp(value, 0f, 1f);
		float num = oriHpBarWidth * value;
		float num2 = (oriHpBarWidth - num) / 2f;
		slider.size = new Vector2(num, slider.size.y);
		slider.transform.localPosition = new Vector3(0f - num2, 0f, 0f);
	}

	private void SetText(float ratio, float size)
	{
		ratioText.text = ratio.ToString("F2") + "%";
		ratioText.transform.localScale = new Vector3(size, size, size);
	}

	public void Dispose()
	{
		if (_tweener != null)
		{
			_tweener.Kill();
		}
		_tweener = null;
	}
}
