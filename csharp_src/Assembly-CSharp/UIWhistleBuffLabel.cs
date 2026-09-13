using UnityEngine;

public class UIWhistleBuffLabel : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer slider;

	[SerializeField]
	private SuperTextMesh timeTxt;

	private float updateCd;

	private long startTime;

	private long endTime;

	private float oriWidth;

	private void Awake()
	{
		oriWidth = slider.size.x;
	}

	public void SetData(long startTime, long endTime)
	{
		this.startTime = startTime;
		this.endTime = endTime;
		RefreshView();
	}

	private void RefreshView()
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		long num = endTime - serverTime;
		timeTxt.text = GameEntry.Timer.MillisecondsToStringWithoutHour(num, ":");
		double num2 = (double)num / (double)(endTime - startTime);
		Vector2 size = slider.size;
		size.x = oriWidth * (float)num2;
		slider.size = size;
	}

	private void Update()
	{
		updateCd += Time.deltaTime;
		if (updateCd >= 1f)
		{
			updateCd -= 1f;
			RefreshView();
		}
	}
}
