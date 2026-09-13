using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/Content Size Fitter With Limit Size")]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class ContentSizeFitterEx : UIBehaviour, ILayoutSelfController, ILayoutController
{
	public enum FitMode
	{
		Unconstrained,
		MinSize,
		PreferredSize
	}

	internal static class SetPropertyUtility
	{
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}

	[SerializeField]
	protected FitMode m_HorizontalFit;

	[SerializeField]
	protected FitMode m_VerticalFit;

	[NonSerialized]
	private RectTransform m_Rect;

	[SerializeField]
	public int maxWidth;

	[SerializeField]
	public int maxHeight;

	[SerializeField]
	public int minWidth;

	[SerializeField]
	public int minHeight;

	private DrivenRectTransformTracker m_Tracker;

	public FitMode horizontalFit
	{
		get
		{
			return m_HorizontalFit;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_HorizontalFit, value))
			{
				SetDirty();
			}
		}
	}

	public FitMode verticalFit
	{
		get
		{
			return m_VerticalFit;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_VerticalFit, value))
			{
				SetDirty();
			}
		}
	}

	private RectTransform rectTransform
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

	protected ContentSizeFitterEx()
	{
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		SetDirty();
	}

	protected override void OnDisable()
	{
		m_Tracker.Clear();
		LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		base.OnDisable();
	}

	protected override void OnRectTransformDimensionsChange()
	{
		SetDirty();
	}

	private void HandleSelfFittingAlongAxis(int axis, float size)
	{
		FitMode fitMode = ((axis == 0) ? horizontalFit : verticalFit);
		if (fitMode == FitMode.Unconstrained)
		{
			m_Tracker.Add(this, rectTransform, DrivenTransformProperties.None);
			return;
		}
		m_Tracker.Add(this, rectTransform, (axis == 0) ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY);
		size = ((fitMode != FitMode.MinSize) ? LayoutUtility.GetPreferredSize(m_Rect, axis) : LayoutUtility.GetMinSize(m_Rect, axis));
		if (maxWidth > 0 && axis == 0)
		{
			size = Math.Min(size, maxWidth);
		}
		if (maxHeight > 0 && axis == 1)
		{
			size = Math.Min(size, maxHeight);
		}
		if (minWidth > 0 && axis == 0)
		{
			size = Math.Max(size, minWidth);
		}
		if (minHeight > 0 && axis == 1)
		{
			size = Math.Max(size, minHeight);
		}
		rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
	}

	public virtual void SetLayoutHorizontal()
	{
		m_Tracker.Clear();
		HandleSelfFittingAlongAxis(0, m_Rect.rect.width);
	}

	public virtual void SetLayoutVertical()
	{
		HandleSelfFittingAlongAxis(1, m_Rect.rect.height);
	}

	protected void SetDirty()
	{
		if (IsActive())
		{
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}
	}
}
