using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Layout/Bidirectional Horizontal Layout Group", 153)]
public class BidirectionalHorizontalLayoutGroup : HorizontalOrVerticalLayoutGroup
{
	private static readonly Vector2 TopLeft = Vector2.up;

	private static readonly Vector2 BottomLeft = Vector2.zero;

	private static readonly Vector2 TopRight = Vector2.right + Vector2.up;

	private static readonly Vector2 BottomRight = Vector2.right;

	[SerializeField]
	protected bool m_IsReverse;

	public bool IsReverse
	{
		get
		{
			return m_IsReverse;
		}
		set
		{
			SetProperty(ref m_IsReverse, value);
		}
	}

	protected BidirectionalHorizontalLayoutGroup()
	{
	}

	public override void CalculateLayoutInputHorizontal()
	{
		base.CalculateLayoutInputHorizontal();
		CalcAlongAxis(0, isVertical: false);
	}

	public override void CalculateLayoutInputVertical()
	{
		CalcAlongAxis(1, isVertical: false);
	}

	public override void SetLayoutHorizontal()
	{
		SetChildrenAlongAxisWithReverse(0, isVertical: false, m_IsReverse);
	}

	public override void SetLayoutVertical()
	{
		SetChildrenAlongAxisWithReverse(1, isVertical: false, isReverse: false);
	}

	private void GetChildSizes(RectTransform child, int axis, bool controlSize, bool childForceExpand, out float min, out float preferred, out float flexible)
	{
		if (!controlSize)
		{
			min = child.sizeDelta[axis];
			preferred = min;
			flexible = 0f;
		}
		else
		{
			min = LayoutUtility.GetMinSize(child, axis);
			preferred = LayoutUtility.GetPreferredSize(child, axis);
			flexible = LayoutUtility.GetFlexibleSize(child, axis);
		}
		if (childForceExpand)
		{
			flexible = Mathf.Max(flexible, 1f);
		}
	}

	protected void SetChildrenAlongAxisWithReverse(int axis, bool isVertical, bool isReverse)
	{
		float num = base.rectTransform.rect.size[axis];
		bool flag = ((axis == 0) ? m_ChildControlWidth : m_ChildControlHeight);
		bool flag2 = ((axis == 0) ? m_ChildScaleWidth : m_ChildScaleHeight);
		bool childForceExpand = ((axis == 0) ? m_ChildForceExpandWidth : m_ChildForceExpandHeight);
		float alignmentOnAxis = GetAlignmentOnAxis(axis);
		if (isVertical ^ (axis == 1))
		{
			float value = num - (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical);
			for (int i = 0; i < base.rectChildren.Count; i++)
			{
				RectTransform rectTransform = base.rectChildren[i];
				GetChildSizes(rectTransform, axis, flag, childForceExpand, out var min, out var preferred, out var flexible);
				float num2 = (flag2 ? rectTransform.localScale[axis] : 1f);
				float num3 = Mathf.Clamp(value, min, (flexible > 0f) ? num : preferred);
				float startOffsetWithReverse = GetStartOffsetWithReverse(axis, isReverse, num3 * num2);
				if (flag)
				{
					SetChildAlongAxisWithScaleAndDirection(rectTransform, axis, isReverse, startOffsetWithReverse, num3, num2);
					continue;
				}
				float num4 = (num3 - rectTransform.sizeDelta[axis]) * alignmentOnAxis;
				SetChildAlongAxisWithScaleAndDirection(rectTransform, axis, isReverse, startOffsetWithReverse + num4, num2);
			}
			return;
		}
		float num5 = ((axis != 0) ? (m_IsReverse ? base.padding.bottom : base.padding.top) : (m_IsReverse ? base.padding.right : base.padding.left));
		float num6 = 0f;
		float num7 = num - GetTotalPreferredSize(axis);
		if (num7 > 0f)
		{
			if (GetTotalFlexibleSize(axis) == 0f)
			{
				num5 = GetStartOffsetWithReverse(axis, isReverse, GetTotalPreferredSize(axis) - (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical));
			}
			else if (GetTotalFlexibleSize(axis) > 0f)
			{
				num6 = num7 / GetTotalFlexibleSize(axis);
			}
		}
		float t = 0f;
		if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
		{
			t = Mathf.Clamp01((num - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));
		}
		for (int j = 0; j < base.rectChildren.Count; j++)
		{
			RectTransform rectTransform2 = base.rectChildren[j];
			GetChildSizes(rectTransform2, axis, flag, childForceExpand, out var min2, out var preferred2, out var flexible2);
			float num8 = (flag2 ? rectTransform2.localScale[axis] : 1f);
			float num9 = Mathf.Lerp(min2, preferred2, t);
			num9 += flexible2 * num6;
			if (flag)
			{
				SetChildAlongAxisWithScaleAndDirection(rectTransform2, axis, isReverse, num5, num9, num8);
			}
			else
			{
				float num10 = (num9 - rectTransform2.sizeDelta[axis]) * alignmentOnAxis;
				SetChildAlongAxisWithScaleAndDirection(rectTransform2, axis, isReverse, num5 + num10, num8);
			}
			num5 += num9 * num8 + base.spacing;
		}
	}

	protected void SetChildAlongAxisWithScaleAndDirection(RectTransform rect, int axis, bool reverse, float pos, float size, float scaleFactor)
	{
		if (!(rect == null))
		{
			m_Tracker.Add(this, rect, (DrivenTransformProperties)(0xF00 | ((axis == 0) ? 4098 : 8196)));
			if (axis == 0)
			{
				rect.anchorMin = (reverse ? TopRight : TopLeft);
				rect.anchorMax = (reverse ? TopRight : TopLeft);
			}
			Vector2 sizeDelta = rect.sizeDelta;
			sizeDelta[axis] = size;
			rect.sizeDelta = sizeDelta;
			Vector2 anchoredPosition = rect.anchoredPosition;
			anchoredPosition[axis] = (((axis == 0) ^ reverse) ? (pos + size * rect.pivot[axis] * scaleFactor) : (0f - pos - size * (1f - rect.pivot[axis]) * scaleFactor));
			rect.anchoredPosition = anchoredPosition;
		}
	}

	protected void SetChildAlongAxisWithScaleAndDirection(RectTransform rect, int axis, bool isReverse, float pos, float scaleFactor)
	{
		if (!(rect == null))
		{
			m_Tracker.Add(this, rect, (DrivenTransformProperties)(0xF00 | ((axis == 0) ? 2 : 4)));
			if (axis == 0)
			{
				rect.anchorMin = (isReverse ? TopRight : TopLeft);
				rect.anchorMax = (isReverse ? TopRight : TopLeft);
			}
			Vector2 anchoredPosition = rect.anchoredPosition;
			anchoredPosition[axis] = (((axis == 0) ^ isReverse) ? (pos + rect.sizeDelta[axis] * rect.pivot[axis] * scaleFactor) : (0f - pos - rect.sizeDelta[axis] * (1f - rect.pivot[axis]) * scaleFactor));
			rect.anchoredPosition = anchoredPosition;
		}
	}

	protected float GetStartOffsetWithReverse(int axis, bool isReverse, float requiredSpaceWithoutPadding)
	{
		float num = requiredSpaceWithoutPadding + (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical);
		float num2 = base.rectTransform.rect.size[axis] - num;
		float alignmentOnAxis = GetAlignmentOnAxis(axis);
		return (float)((axis != 0) ? (isReverse ? base.padding.bottom : base.padding.top) : (isReverse ? base.padding.right : base.padding.left)) + num2 * alignmentOnAxis;
	}
}
