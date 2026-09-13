using System;
using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView;

public class RadialLayoutGroup : LayoutGroup
{
	public float fDistance;

	[Range(0f, 360f)]
	public float MinAngle;

	[Range(0f, 360f)]
	public float MaxAngle;

	[Range(0f, 360f)]
	public float StartAngle;

	public bool OnlyLayoutVisible;

	protected override void OnEnable()
	{
		base.OnEnable();
		CalculateRadial();
	}

	public override void SetLayoutHorizontal()
	{
	}

	public override void SetLayoutVertical()
	{
	}

	public override void CalculateLayoutInputVertical()
	{
		CalculateRadial();
	}

	public override void CalculateLayoutInputHorizontal()
	{
		CalculateRadial();
	}

	protected override void OnDisable()
	{
		m_Tracker.Clear();
		LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
	}

	private void CalculateRadial()
	{
		m_Tracker.Clear();
		if (base.transform.childCount == 0)
		{
			return;
		}
		int num = 0;
		if (OnlyLayoutVisible)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				RectTransform rectTransform = (RectTransform)base.transform.GetChild(i);
				if (rectTransform != null && rectTransform.gameObject.activeSelf)
				{
					num++;
				}
			}
		}
		else
		{
			num = base.transform.childCount;
		}
		float num2 = (MaxAngle - MinAngle) / (float)num;
		float num3 = StartAngle;
		for (int j = 0; j < base.transform.childCount; j++)
		{
			RectTransform rectTransform2 = (RectTransform)base.transform.GetChild(j);
			if (rectTransform2 != null && (!OnlyLayoutVisible || rectTransform2.gameObject.activeSelf))
			{
				m_Tracker.Add(this, rectTransform2, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Pivot);
				Vector3 vector = new Vector3(Mathf.Cos(num3 * (MathF.PI / 180f)), Mathf.Sin(num3 * (MathF.PI / 180f)), 0f);
				rectTransform2.localPosition = vector * fDistance;
				Vector2 vector2 = (rectTransform2.pivot = new Vector2(0.5f, 0.5f));
				Vector2 anchorMin = (rectTransform2.anchorMax = vector2);
				rectTransform2.anchorMin = anchorMin;
				num3 += num2;
			}
		}
	}
}
