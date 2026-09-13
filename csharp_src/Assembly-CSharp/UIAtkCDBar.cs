using System;
using UnityEngine;

public class UIAtkCDBar : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer hpSlider;

	[SerializeField]
	private SuperTextMesh hpText;

	private float oriHpBarWidth;

	private long atkStartTime;

	private long atkEndTime;

	private void Awake()
	{
		oriHpBarWidth = hpSlider.size.x;
	}

	public void SetValue(long startTime, long endTime)
	{
		atkStartTime = startTime;
		atkEndTime = endTime;
		OnUpdate();
	}

	public void OnUpdate()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (atkStartTime != 0L)
		{
			if (serverTime > atkEndTime)
			{
				serverTime = atkEndTime;
			}
			else if (serverTime < atkStartTime)
			{
				serverTime = atkStartTime;
			}
			float value = (float)(serverTime - atkStartTime) / (float)(atkEndTime - atkStartTime);
			value = Mathf.Clamp(value, 0f, 1f);
			float num = oriHpBarWidth * value;
			float num2 = (oriHpBarWidth - num) / 2f;
			hpSlider.size = new Vector2(num, hpSlider.size.y);
			hpSlider.transform.localPosition = new Vector3(0f - num2, 0f, 0f);
			hpText.text = Math.Floor((float)(atkEndTime - serverTime) / 1000f).ToString();
		}
	}

	public void Dispose()
	{
	}
}
