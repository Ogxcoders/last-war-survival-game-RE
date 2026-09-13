using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGame.Biubiu.Client;

public class UIS5GamePlayerHpItemView : MonoBehaviour
{
	public Slider Slider_Self;

	public Slider Slider_Target;

	public Slider Background;

	public RectTransform Go_FengGe;

	private RectTransform RectTransform;

	private bool IsMe;

	private List<RectTransform> FenGeList = new List<RectTransform>();

	private List<Tween> Tweeners = new List<Tween>();

	private Slider SelfSlider
	{
		get
		{
			if (!IsMe)
			{
				return Slider_Target;
			}
			return Slider_Self;
		}
	}

	private Slider TargetSlider
	{
		get
		{
			if (!IsMe)
			{
				return Slider_Self;
			}
			return Slider_Target;
		}
	}

	private void Awake()
	{
		RectTransform = GetComponent<RectTransform>();
	}

	public void Refresh(bool isMe, int curHp, int maxHp)
	{
		IsMe = isMe;
		SelfSlider.gameObject.SetActive(value: true);
		TargetSlider.gameObject.SetActive(value: false);
		RectTransform.sizeDelta = new Vector2(48 * maxHp, RectTransform.sizeDelta.y);
		for (int i = 0; i < FenGeList.Count; i++)
		{
			FenGeList[i].gameObject.SetActive(value: false);
		}
		for (int j = 0; j < maxHp - 1; j++)
		{
			bool num = j < FenGeList.Count;
			RectTransform rectTransform = null;
			if (!num)
			{
				rectTransform = Object.Instantiate(Go_FengGe, RectTransform);
				FenGeList.Add(rectTransform);
			}
			else
			{
				rectTransform = FenGeList[j];
			}
			rectTransform.gameObject.SetActive(value: true);
			rectTransform.anchoredPosition = new Vector2(47 * (j + 1), rectTransform.anchoredPosition.y);
		}
		ClearTween();
		float endValue = (float)curHp / (float)maxHp;
		Tweeners.Add(SelfSlider.DOValue(endValue, 0.05f));
		Tweeners.Add(Background.DOValue(endValue, 0.2f));
	}

	private void OnDestroy()
	{
		ClearTween();
	}

	private void ClearTween()
	{
		for (int i = 0; i < Tweeners.Count; i++)
		{
			Tweeners[i].Kill();
		}
	}
}
