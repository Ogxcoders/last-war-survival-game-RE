using System;

namespace UnityEngine.UIElements;

public class ScrollView : VisualElement
{
	public new class UxmlFactory : UxmlFactory<ScrollView, UxmlTraits>
	{
	}

	public new class UxmlTraits : VisualElement.UxmlTraits
	{
		private UxmlEnumAttributeDescription<ScrollViewMode> m_ScrollViewMode = new UxmlEnumAttributeDescription<ScrollViewMode>
		{
			name = "mode",
			defaultValue = ScrollViewMode.Vertical
		};

		private UxmlBoolAttributeDescription m_ShowHorizontal = new UxmlBoolAttributeDescription
		{
			name = "show-horizontal-scroller"
		};

		private UxmlBoolAttributeDescription m_ShowVertical = new UxmlBoolAttributeDescription
		{
			name = "show-vertical-scroller"
		};

		private UxmlFloatAttributeDescription m_HorizontalPageSize = new UxmlFloatAttributeDescription
		{
			name = "horizontal-page-size",
			defaultValue = 20f
		};

		private UxmlFloatAttributeDescription m_VerticalPageSize = new UxmlFloatAttributeDescription
		{
			name = "vertical-page-size",
			defaultValue = 20f
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			ScrollView scrollView = (ScrollView)ve;
			scrollView.SetScrollViewMode(m_ScrollViewMode.GetValueFromBag(bag, cc));
			scrollView.showHorizontal = m_ShowHorizontal.GetValueFromBag(bag, cc);
			scrollView.showVertical = m_ShowVertical.GetValueFromBag(bag, cc);
			scrollView.horizontalPageSize = m_HorizontalPageSize.GetValueFromBag(bag, cc);
			scrollView.verticalPageSize = m_VerticalPageSize.GetValueFromBag(bag, cc);
		}
	}

	private bool m_ShowHorizontal;

	private bool m_ShowVertical;

	private VisualElement m_ContentContainer;

	public static readonly string ussClassName = "unity-scroll-view";

	public static readonly string viewportUssClassName = ussClassName + "__content-viewport";

	public static readonly string contentUssClassName = ussClassName + "__content-container";

	public static readonly string hScrollerUssClassName = ussClassName + "__horizontal-scroller";

	public static readonly string vScrollerUssClassName = ussClassName + "__vertical-scroller";

	public static readonly string horizontalVariantUssClassName = ussClassName + "--horizontal";

	public static readonly string verticalVariantUssClassName = ussClassName + "--vertical";

	public static readonly string verticalHorizontalVariantUssClassName = ussClassName + "--vertical-horizontal";

	public static readonly string scrollVariantUssClassName = ussClassName + "--scroll";

	public bool showHorizontal
	{
		get
		{
			return m_ShowHorizontal;
		}
		set
		{
			m_ShowHorizontal = value;
			UpdateScrollers(m_ShowHorizontal, m_ShowVertical);
		}
	}

	public bool showVertical
	{
		get
		{
			return m_ShowVertical;
		}
		set
		{
			m_ShowVertical = value;
			UpdateScrollers(m_ShowHorizontal, m_ShowVertical);
		}
	}

	internal bool needsHorizontal => showHorizontal || contentContainer.layout.width - base.layout.width > 0f;

	internal bool needsVertical => showVertical || contentContainer.layout.height - base.layout.height > 0f;

	public Vector2 scrollOffset
	{
		get
		{
			return new Vector2(horizontalScroller.value, verticalScroller.value);
		}
		set
		{
			if (value != scrollOffset)
			{
				horizontalScroller.value = value.x;
				verticalScroller.value = value.y;
				UpdateContentViewTransform();
			}
		}
	}

	public float horizontalPageSize
	{
		get
		{
			return horizontalScroller.slider.pageSize;
		}
		set
		{
			horizontalScroller.slider.pageSize = value;
		}
	}

	public float verticalPageSize
	{
		get
		{
			return verticalScroller.slider.pageSize;
		}
		set
		{
			verticalScroller.slider.pageSize = value;
		}
	}

	private float scrollableWidth => contentContainer.layout.width - contentViewport.layout.width;

	private float scrollableHeight => contentContainer.layout.height - contentViewport.layout.height;

	public VisualElement contentViewport { get; private set; }

	public Scroller horizontalScroller { get; private set; }

	public Scroller verticalScroller { get; private set; }

	public override VisualElement contentContainer => m_ContentContainer;

	private void UpdateContentViewTransform()
	{
		Vector3 position = contentContainer.transform.position;
		Vector2 vector = scrollOffset;
		position.x = GUIUtility.RoundToPixelGrid(0f - vector.x);
		position.y = GUIUtility.RoundToPixelGrid(0f - vector.y);
		contentContainer.transform.position = position;
		IncrementVersion(VersionChangeType.Repaint);
	}

	public void ScrollTo(VisualElement child)
	{
		if (child == null)
		{
			throw new ArgumentNullException("child");
		}
		if (!contentContainer.Contains(child))
		{
			throw new ArgumentException("Cannot scroll to a VisualElement that is not a child of the ScrollView content-container.");
		}
		float num = 0f;
		float num2 = 0f;
		if (scrollableHeight > 0f)
		{
			num = GetYDeltaOffset(child);
			verticalScroller.value = scrollOffset.y + num;
		}
		if (scrollableWidth > 0f)
		{
			num2 = GetXDeltaOffset(child);
			horizontalScroller.value = scrollOffset.x + num2;
		}
		if (num != 0f || num2 != 0f)
		{
			UpdateContentViewTransform();
		}
	}

	private float GetXDeltaOffset(VisualElement child)
	{
		float num = contentContainer.transform.position.x * -1f;
		Rect rect = contentViewport.worldBound;
		float num2 = rect.xMin + num;
		float num3 = rect.xMax + num;
		Rect rect2 = child.worldBound;
		float num4 = rect2.xMin + num;
		float num5 = rect2.xMax + num;
		if ((num4 >= num2 && num5 <= num3) || float.IsNaN(num4) || float.IsNaN(num5))
		{
			return 0f;
		}
		float deltaDistance = GetDeltaDistance(num2, num3, num4, num5);
		return deltaDistance * horizontalScroller.highValue / scrollableWidth;
	}

	private float GetYDeltaOffset(VisualElement child)
	{
		float num = contentContainer.transform.position.y * -1f;
		Rect rect = contentViewport.worldBound;
		float num2 = rect.yMin + num;
		float num3 = rect.yMax + num;
		Rect rect2 = child.worldBound;
		float num4 = rect2.yMin + num;
		float num5 = rect2.yMax + num;
		if ((num4 >= num2 && num5 <= num3) || float.IsNaN(num4) || float.IsNaN(num5))
		{
			return 0f;
		}
		float deltaDistance = GetDeltaDistance(num2, num3, num4, num5);
		return deltaDistance * verticalScroller.highValue / scrollableHeight;
	}

	private float GetDeltaDistance(float viewMin, float viewMax, float childBoundaryMin, float childBoundaryMax)
	{
		float num = childBoundaryMax - viewMax;
		if (num < -1f)
		{
			num = childBoundaryMin - viewMin;
		}
		return num;
	}

	public ScrollView()
		: this(ScrollViewMode.Vertical)
	{
	}

	public ScrollView(ScrollViewMode scrollViewMode)
	{
		AddToClassList(ussClassName);
		contentViewport = new VisualElement
		{
			name = "unity-content-viewport"
		};
		contentViewport.AddToClassList(viewportUssClassName);
		contentViewport.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		base.hierarchy.Add(contentViewport);
		m_ContentContainer = new VisualElement
		{
			name = "unity-content-container"
		};
		m_ContentContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		m_ContentContainer.AddToClassList(contentUssClassName);
		m_ContentContainer.usageHints = UsageHints.GroupTransform;
		contentViewport.Add(m_ContentContainer);
		SetScrollViewMode(scrollViewMode);
		horizontalScroller = new Scroller(0f, 100f, delegate(float value)
		{
			scrollOffset = new Vector2(value, scrollOffset.y);
			UpdateContentViewTransform();
		}, SliderDirection.Horizontal)
		{
			viewDataKey = "HorizontalScroller",
			visible = false
		};
		horizontalScroller.AddToClassList(hScrollerUssClassName);
		base.hierarchy.Add(horizontalScroller);
		verticalScroller = new Scroller(0f, 100f, delegate(float value)
		{
			scrollOffset = new Vector2(scrollOffset.x, value);
			UpdateContentViewTransform();
		})
		{
			viewDataKey = "VerticalScroller",
			visible = false
		};
		verticalScroller.AddToClassList(vScrollerUssClassName);
		base.hierarchy.Add(verticalScroller);
		RegisterCallback<WheelEvent>(OnScrollWheel);
		scrollOffset = Vector2.zero;
	}

	internal void SetScrollViewMode(ScrollViewMode scrollViewMode)
	{
		RemoveFromClassList(verticalVariantUssClassName);
		RemoveFromClassList(horizontalVariantUssClassName);
		RemoveFromClassList(verticalHorizontalVariantUssClassName);
		RemoveFromClassList(scrollVariantUssClassName);
		switch (scrollViewMode)
		{
		case ScrollViewMode.Vertical:
			AddToClassList(verticalVariantUssClassName);
			AddToClassList(scrollVariantUssClassName);
			break;
		case ScrollViewMode.Horizontal:
			AddToClassList(horizontalVariantUssClassName);
			AddToClassList(scrollVariantUssClassName);
			break;
		case ScrollViewMode.VerticalAndHorizontal:
			AddToClassList(scrollVariantUssClassName);
			AddToClassList(verticalHorizontalVariantUssClassName);
			break;
		}
	}

	private void OnGeometryChanged(GeometryChangedEvent evt)
	{
		if (!(evt.oldRect.size == evt.newRect.size))
		{
			bool flag = needsVertical;
			bool flag2 = needsHorizontal;
			if (evt.layoutPass > 0)
			{
				flag = flag || verticalScroller.visible;
				flag2 = flag2 || horizontalScroller.visible;
			}
			UpdateScrollers(flag2, flag);
			UpdateContentViewTransform();
		}
	}

	private void UpdateScrollers(bool displayHorizontal, bool displayVertical)
	{
		float factor = ((contentContainer.layout.width > Mathf.Epsilon) ? (contentViewport.layout.width / contentContainer.layout.width) : 1f);
		float factor2 = ((contentContainer.layout.height > Mathf.Epsilon) ? (contentViewport.layout.height / contentContainer.layout.height) : 1f);
		horizontalScroller.Adjust(factor);
		verticalScroller.Adjust(factor2);
		horizontalScroller.SetEnabled(contentContainer.layout.width - contentViewport.layout.width > 0f);
		verticalScroller.SetEnabled(contentContainer.layout.height - contentViewport.layout.height > 0f);
		contentViewport.style.marginRight = (displayVertical ? verticalScroller.layout.width : 0f);
		horizontalScroller.style.right = (displayVertical ? verticalScroller.layout.width : 0f);
		contentViewport.style.marginBottom = (displayHorizontal ? horizontalScroller.layout.height : 0f);
		verticalScroller.style.bottom = (displayHorizontal ? horizontalScroller.layout.height : 0f);
		if (displayHorizontal && scrollableWidth > 0f)
		{
			horizontalScroller.lowValue = 0f;
			horizontalScroller.highValue = scrollableWidth;
		}
		else
		{
			horizontalScroller.value = 0f;
		}
		if (displayVertical && scrollableHeight > 0f)
		{
			verticalScroller.lowValue = 0f;
			verticalScroller.highValue = scrollableHeight;
		}
		else
		{
			verticalScroller.value = 0f;
		}
		if (horizontalScroller.visible != displayHorizontal)
		{
			horizontalScroller.visible = displayHorizontal;
		}
		if (verticalScroller.visible != displayVertical)
		{
			verticalScroller.visible = displayVertical;
		}
	}

	private void OnScrollWheel(WheelEvent evt)
	{
		float value = verticalScroller.value;
		if (contentContainer.layout.height - base.layout.height > 0f)
		{
			if (evt.delta.y < 0f)
			{
				verticalScroller.ScrollPageUp(Mathf.Abs(evt.delta.y));
			}
			else if (evt.delta.y > 0f)
			{
				verticalScroller.ScrollPageDown(Mathf.Abs(evt.delta.y));
			}
		}
		if (verticalScroller.value != value)
		{
			evt.StopPropagation();
		}
	}
}
