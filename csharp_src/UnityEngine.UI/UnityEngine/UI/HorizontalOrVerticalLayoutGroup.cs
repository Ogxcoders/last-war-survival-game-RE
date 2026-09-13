namespace UnityEngine.UI;

[ExecuteAlways]
public abstract class HorizontalOrVerticalLayoutGroup : LayoutGroup
{
	[SerializeField]
	protected float m_Spacing;

	[SerializeField]
	protected bool m_ChildForceExpandWidth = true;

	[SerializeField]
	protected bool m_ChildForceExpandHeight = true;

	[SerializeField]
	protected bool m_ChildControlWidth = true;

	[SerializeField]
	protected bool m_ChildControlHeight = true;

	[SerializeField]
	protected bool m_ChildScaleWidth;

	[SerializeField]
	protected bool m_ChildScaleHeight;

	public float spacing
	{
		get
		{
			return m_Spacing;
		}
		set
		{
			SetProperty(ref m_Spacing, value);
		}
	}

	public bool childForceExpandWidth
	{
		get
		{
			return m_ChildForceExpandWidth;
		}
		set
		{
			SetProperty(ref m_ChildForceExpandWidth, value);
		}
	}

	public bool childForceExpandHeight
	{
		get
		{
			return m_ChildForceExpandHeight;
		}
		set
		{
			SetProperty(ref m_ChildForceExpandHeight, value);
		}
	}

	public bool childControlWidth
	{
		get
		{
			return m_ChildControlWidth;
		}
		set
		{
			SetProperty(ref m_ChildControlWidth, value);
		}
	}

	public bool childControlHeight
	{
		get
		{
			return m_ChildControlHeight;
		}
		set
		{
			SetProperty(ref m_ChildControlHeight, value);
		}
	}

	public bool childScaleWidth
	{
		get
		{
			return m_ChildScaleWidth;
		}
		set
		{
			SetProperty(ref m_ChildScaleWidth, value);
		}
	}

	public bool childScaleHeight
	{
		get
		{
			return m_ChildScaleHeight;
		}
		set
		{
			SetProperty(ref m_ChildScaleHeight, value);
		}
	}

	protected void CalcAlongAxis(int axis, bool isVertical)
	{
		float num = ((axis == 0) ? base.padding.horizontal : base.padding.vertical);
		bool controlSize = ((axis == 0) ? m_ChildControlWidth : m_ChildControlHeight);
		bool flag = ((axis == 0) ? m_ChildScaleWidth : m_ChildScaleHeight);
		bool childForceExpand = ((axis == 0) ? m_ChildForceExpandWidth : m_ChildForceExpandHeight);
		float num2 = num;
		float num3 = num;
		float num4 = 0f;
		bool flag2 = isVertical ^ (axis == 1);
		for (int i = 0; i < base.rectChildren.Count; i++)
		{
			RectTransform rectTransform = base.rectChildren[i];
			GetChildSizes(rectTransform, axis, controlSize, childForceExpand, out var min, out var preferred, out var flexible);
			if (flag)
			{
				float num5 = rectTransform.localScale[axis];
				min *= num5;
				preferred *= num5;
				flexible *= num5;
			}
			if (flag2)
			{
				num2 = Mathf.Max(min + num, num2);
				num3 = Mathf.Max(preferred + num, num3);
				num4 = Mathf.Max(flexible, num4);
			}
			else
			{
				num2 += min + spacing;
				num3 += preferred + spacing;
				num4 += flexible;
			}
		}
		if (!flag2 && base.rectChildren.Count > 0)
		{
			num2 -= spacing;
			num3 -= spacing;
		}
		num3 = Mathf.Max(num2, num3);
		SetLayoutInputForAxis(num2, num3, num4, axis);
	}

	protected void SetChildrenAlongAxis(int axis, bool isVertical)
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
				float startOffset = GetStartOffset(axis, num3 * num2);
				if (flag)
				{
					SetChildAlongAxisWithScale(rectTransform, axis, startOffset, num3, num2);
					continue;
				}
				float num4 = (num3 - rectTransform.sizeDelta[axis]) * alignmentOnAxis;
				SetChildAlongAxisWithScale(rectTransform, axis, startOffset + num4, num2);
			}
			return;
		}
		float num5 = ((axis == 0) ? base.padding.left : base.padding.top);
		float num6 = 0f;
		float num7 = num - GetTotalPreferredSize(axis);
		if (num7 > 0f)
		{
			if (GetTotalFlexibleSize(axis) == 0f)
			{
				num5 = GetStartOffset(axis, GetTotalPreferredSize(axis) - (float)((axis == 0) ? base.padding.horizontal : base.padding.vertical));
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
				SetChildAlongAxisWithScale(rectTransform2, axis, num5, num9, num8);
			}
			else
			{
				float num10 = (num9 - rectTransform2.sizeDelta[axis]) * alignmentOnAxis;
				SetChildAlongAxisWithScale(rectTransform2, axis, num5 + num10, num8);
			}
			num5 += num9 * num8 + spacing;
		}
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
}
