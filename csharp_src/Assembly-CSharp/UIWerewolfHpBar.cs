using DG.Tweening;
using UnityEngine;

public class UIWerewolfHpBar : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer hpSlider;

	[SerializeField]
	private SpriteRenderer hpBg;

	[SerializeField]
	private Transform root;

	private const float X_OFFSET = -0.16f;

	private const float WIDTH = 0.33f;

	private const float HEIGHT = 0.44f;

	private int maxHp;

	private int fromHp;

	private int toHp;

	private Tweener _tweener;

	public void Init(int maxHp)
	{
		this.maxHp = maxHp;
		hpBg.size = new Vector2((float)maxHp * 0.33f, 0.44f);
		hpBg.transform.localPosition = new Vector3((float)maxHp * -0.16f, 0f, 0f);
		hpSlider.transform.localPosition = new Vector3((float)maxHp * -0.16f, 0f, 0f);
	}

	public void SetData(byte lostHp, bool ani = false)
	{
		int num = maxHp - lostHp;
		if (!ani || num != toHp)
		{
			fromHp = toHp;
			toHp = num;
			if (ani)
			{
				SetSliderWithAnim();
			}
			else
			{
				SetSlider(toHp, 1f);
			}
		}
	}

	public void SetSliderWithAnim()
	{
		if (_tweener != null)
		{
			_tweener.Kill();
		}
		_tweener = DOTween.To(() => fromHp, delegate(float b)
		{
			float num = ((float)fromHp - b) / (float)(fromHp - toHp);
			float size = 1f;
			if (num < 0.5f)
			{
				size = 0.5f * (num / 0.5f) + 1f;
			}
			else if (num < 1f)
			{
				size = 1.5f - 0.5f * ((num - 0.5f) / 0.5f);
			}
			SetSlider(b, size);
		}, toHp, 1f).OnComplete(delegate
		{
			_tweener = null;
			SetSlider(toHp, 1f);
		});
	}

	private void SetSlider(float curHp, float size)
	{
		hpSlider.size = new Vector2(curHp * 0.33f, 0.44f);
		root.localScale = new Vector3(size, size, size);
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
