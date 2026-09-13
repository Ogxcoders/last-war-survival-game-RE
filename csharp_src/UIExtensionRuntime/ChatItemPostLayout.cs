using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

[DisallowMultipleComponent]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[LuaCallCSharp(GenFlag.No)]
public class ChatItemPostLayout : UIBehaviour, ILayoutElement, ILayoutGroup, ILayoutController
{
	public Vector2 minSize;

	public Vector2 padding;

	[NonSerialized]
	private RectTransform m_Rect;

	protected RectTransform rectTransform
	{
		get
		{
			if (m_Rect == null)
			{
				m_Rect = GetComponent<RectTransform>();
			}
			return m_Rect;
		}
	}

	public float minWidth { get; }

	public float preferredWidth { get; private set; }

	public float flexibleWidth { get; }

	public float minHeight { get; }

	public float preferredHeight { get; private set; }

	public float flexibleHeight { get; }

	public int layoutPriority { get; }

	protected override void OnEnable()
	{
		base.OnEnable();
		SetDirty();
	}

	protected override void OnDisable()
	{
		LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		base.OnDisable();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		SetDirty();
	}

	protected void OnTransformChildrenChanged()
	{
		SetDirty();
	}

	public void SetDirty()
	{
		if (IsActive())
		{
			if (!CanvasUpdateRegistry.IsRebuildingLayout())
			{
				LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
			}
			else
			{
				StartCoroutine(DelayedSetDirty(rectTransform));
			}
		}
	}

	private IEnumerator DelayedSetDirty(RectTransform rect)
	{
		yield return null;
		LayoutRebuilder.MarkLayoutForRebuild(rect);
	}

	[ContextMenu("ForceUpdate")]
	public void ForceUpdate()
	{
		SetDirty();
	}

	public void CalculateSize()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		__CalculateSize();
	}

	private void __CalculateSize()
	{
		float a = minSize.x;
		float a2 = minSize.y;
		if (this.rectTransform.childCount > 0)
		{
			int i = 0;
			for (int childCount = this.rectTransform.childCount; i < childCount; i++)
			{
				RectTransform rectTransform = this.rectTransform.GetChild(i) as RectTransform;
				if (!(rectTransform == null) && rectTransform.gameObject.activeInHierarchy)
				{
					ILayoutIgnorer component = rectTransform.GetComponent<ILayoutIgnorer>();
					if (component == null || !component.ignoreLayout)
					{
						Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(this.rectTransform, rectTransform);
						a = Mathf.Max(a, Mathf.Max(Mathf.Abs(bounds.max.x), Mathf.Abs(bounds.min.x)));
						a2 = Mathf.Max(a2, 0f - bounds.min.y);
					}
				}
			}
		}
		preferredWidth = a;
		preferredHeight = a2;
	}

	public void CalculateLayoutInputHorizontal()
	{
	}

	public void CalculateLayoutInputVertical()
	{
	}

	public void SetLayoutHorizontal()
	{
	}

	public void SetLayoutVertical()
	{
	}

	public void SetPreferredSize(float width, float height)
	{
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + padding.x);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + padding.y);
	}

	public void SetPreferredSize()
	{
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth + padding.x);
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight + padding.y);
	}

	public void SetPreferredWidth(float width)
	{
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + padding.x);
	}

	public void SetPreferredHeight()
	{
		rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, preferredHeight + padding.y);
	}
}
