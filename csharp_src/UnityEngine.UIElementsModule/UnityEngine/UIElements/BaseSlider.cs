using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public abstract class BaseSlider<TValueType> : BaseField<TValueType> where TValueType : IComparable<TValueType>
{
	[SerializeField]
	private TValueType m_LowValue;

	[SerializeField]
	private TValueType m_HighValue;

	private float m_PageSize;

	private Rect m_DragElementStartPos;

	private SliderDirection m_Direction;

	internal const float kDefaultPageSize = 0f;

	public new static readonly string ussClassName = "unity-base-slider";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	public static readonly string horizontalVariantUssClassName = ussClassName + "--horizontal";

	public static readonly string verticalVariantUssClassName = ussClassName + "--vertical";

	public static readonly string trackerUssClassName = ussClassName + "__tracker";

	public static readonly string draggerUssClassName = ussClassName + "__dragger";

	public static readonly string draggerBorderUssClassName = ussClassName + "__dragger-border";

	internal VisualElement dragElement { get; private set; }

	internal VisualElement dragBorderElement { get; private set; }

	public TValueType lowValue
	{
		get
		{
			return m_LowValue;
		}
		set
		{
			if (!EqualityComparer<TValueType>.Default.Equals(m_LowValue, value))
			{
				m_LowValue = value;
				ClampValue();
				UpdateDragElementPosition();
				SaveViewData();
			}
		}
	}

	public TValueType highValue
	{
		get
		{
			return m_HighValue;
		}
		set
		{
			if (!EqualityComparer<TValueType>.Default.Equals(m_HighValue, value))
			{
				m_HighValue = value;
				ClampValue();
				UpdateDragElementPosition();
				SaveViewData();
			}
		}
	}

	public TValueType range => SliderRange();

	public virtual float pageSize
	{
		get
		{
			return m_PageSize;
		}
		set
		{
			m_PageSize = value;
		}
	}

	internal ClampedDragger<TValueType> clampedDragger { get; private set; }

	public override TValueType value
	{
		get
		{
			return base.value;
		}
		set
		{
			TValueType clampedValue = GetClampedValue(value);
			base.value = clampedValue;
		}
	}

	public SliderDirection direction
	{
		get
		{
			return m_Direction;
		}
		set
		{
			m_Direction = value;
			if (m_Direction == SliderDirection.Horizontal)
			{
				RemoveFromClassList(verticalVariantUssClassName);
				AddToClassList(horizontalVariantUssClassName);
			}
			else
			{
				RemoveFromClassList(horizontalVariantUssClassName);
				AddToClassList(verticalVariantUssClassName);
			}
		}
	}

	private TValueType Clamp(TValueType value, TValueType lowBound, TValueType highBound)
	{
		TValueType result = value;
		if (lowBound.CompareTo(value) > 0)
		{
			result = lowBound;
		}
		else if (highBound.CompareTo(value) < 0)
		{
			result = highBound;
		}
		return result;
	}

	private TValueType GetClampedValue(TValueType newValue)
	{
		TValueType val = lowValue;
		TValueType val2 = highValue;
		if (val.CompareTo(val2) > 0)
		{
			TValueType val3 = val;
			val = val2;
			val2 = val3;
		}
		return Clamp(newValue, val, val2);
	}

	public override void SetValueWithoutNotify(TValueType newValue)
	{
		TValueType clampedValue = GetClampedValue(newValue);
		base.SetValueWithoutNotify(clampedValue);
		UpdateDragElementPosition();
	}

	internal BaseSlider(string label, TValueType start, TValueType end, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
		: base(label, (VisualElement)null)
	{
		AddToClassList(ussClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
		this.direction = direction;
		this.pageSize = pageSize;
		lowValue = start;
		highValue = end;
		base.pickingMode = PickingMode.Ignore;
		base.visualInput.pickingMode = PickingMode.Position;
		VisualElement visualElement = new VisualElement
		{
			name = "unity-tracker"
		};
		visualElement.AddToClassList(trackerUssClassName);
		base.visualInput.Add(visualElement);
		dragBorderElement = new VisualElement
		{
			name = "unity-dragger-border"
		};
		dragBorderElement.AddToClassList(draggerBorderUssClassName);
		base.visualInput.Add(dragBorderElement);
		dragElement = new VisualElement
		{
			name = "unity-dragger"
		};
		dragElement.RegisterCallback<GeometryChangedEvent>(UpdateDragElementPosition);
		dragElement.AddToClassList(draggerUssClassName);
		base.visualInput.Add(dragElement);
		clampedDragger = new ClampedDragger<TValueType>(this, SetSliderValueFromClick, SetSliderValueFromDrag);
		base.visualInput.AddManipulator(clampedDragger);
	}

	private void ClampValue()
	{
		value = base.rawValue;
	}

	internal abstract TValueType SliderLerpUnclamped(TValueType a, TValueType b, float interpolant);

	internal abstract float SliderNormalizeValue(TValueType currentValue, TValueType lowerValue, TValueType higherValue);

	internal abstract TValueType SliderRange();

	private void SetSliderValueFromDrag()
	{
		if (clampedDragger.dragDirection == ClampedDragger<TValueType>.DragDirection.Free)
		{
			Vector2 delta = clampedDragger.delta;
			if (direction == SliderDirection.Horizontal)
			{
				ComputeValueAndDirectionFromDrag(base.visualInput.resolvedStyle.width, dragElement.resolvedStyle.width, m_DragElementStartPos.x + delta.x);
			}
			else
			{
				ComputeValueAndDirectionFromDrag(base.visualInput.resolvedStyle.height, dragElement.resolvedStyle.height, m_DragElementStartPos.y + delta.y);
			}
		}
	}

	private void ComputeValueAndDirectionFromDrag(float sliderLength, float dragElementLength, float dragElementPos)
	{
		float num = sliderLength - dragElementLength;
		if (!(Mathf.Abs(num) < Mathf.Epsilon))
		{
			float interpolant = Mathf.Max(0f, Mathf.Min(dragElementPos, num)) / num;
			value = SliderLerpUnclamped(lowValue, highValue, interpolant);
		}
	}

	private void SetSliderValueFromClick()
	{
		if (clampedDragger.dragDirection == ClampedDragger<TValueType>.DragDirection.Free)
		{
			return;
		}
		if (clampedDragger.dragDirection == ClampedDragger<TValueType>.DragDirection.None)
		{
			if (Mathf.Approximately(pageSize, 0f))
			{
				float x = ((direction == SliderDirection.Horizontal) ? (clampedDragger.startMousePosition.x - dragElement.resolvedStyle.width / 2f) : dragElement.transform.position.x);
				float y = ((direction == SliderDirection.Horizontal) ? dragElement.transform.position.y : (clampedDragger.startMousePosition.y - dragElement.resolvedStyle.height / 2f));
				Vector3 position = new Vector3(x, y, 0f);
				dragElement.transform.position = position;
				dragBorderElement.transform.position = position;
				m_DragElementStartPos = new Rect(x, y, dragElement.resolvedStyle.width, dragElement.resolvedStyle.height);
				clampedDragger.dragDirection = ClampedDragger<TValueType>.DragDirection.Free;
				if (direction == SliderDirection.Horizontal)
				{
					ComputeValueAndDirectionFromDrag(base.visualInput.resolvedStyle.width, dragElement.resolvedStyle.width, m_DragElementStartPos.x);
				}
				else
				{
					ComputeValueAndDirectionFromDrag(base.visualInput.resolvedStyle.height, dragElement.resolvedStyle.height, m_DragElementStartPos.y);
				}
				return;
			}
			m_DragElementStartPos = new Rect(dragElement.transform.position.x, dragElement.transform.position.y, dragElement.resolvedStyle.width, dragElement.resolvedStyle.height);
		}
		if (direction == SliderDirection.Horizontal)
		{
			ComputeValueAndDirectionFromClick(base.visualInput.resolvedStyle.width, dragElement.resolvedStyle.width, dragElement.transform.position.x, clampedDragger.lastMousePosition.x);
		}
		else
		{
			ComputeValueAndDirectionFromClick(base.visualInput.resolvedStyle.height, dragElement.resolvedStyle.height, dragElement.transform.position.y, clampedDragger.lastMousePosition.y);
		}
	}

	internal virtual void ComputeValueAndDirectionFromClick(float sliderLength, float dragElementLength, float dragElementPos, float dragElementLastPos)
	{
		float num = sliderLength - dragElementLength;
		if (!(Mathf.Abs(num) < Mathf.Epsilon))
		{
			if (dragElementLastPos < dragElementPos && clampedDragger.dragDirection != ClampedDragger<TValueType>.DragDirection.LowToHigh)
			{
				clampedDragger.dragDirection = ClampedDragger<TValueType>.DragDirection.HighToLow;
				float interpolant = Mathf.Max(0f, Mathf.Min(dragElementPos - pageSize, num)) / num;
				value = SliderLerpUnclamped(lowValue, highValue, interpolant);
			}
			else if (dragElementLastPos > dragElementPos + dragElementLength && clampedDragger.dragDirection != ClampedDragger<TValueType>.DragDirection.HighToLow)
			{
				clampedDragger.dragDirection = ClampedDragger<TValueType>.DragDirection.LowToHigh;
				float interpolant2 = Mathf.Max(0f, Mathf.Min(dragElementPos + pageSize, num)) / num;
				value = SliderLerpUnclamped(lowValue, highValue, interpolant2);
			}
		}
	}

	public void AdjustDragElement(float factor)
	{
		bool flag = factor < 1f;
		dragElement.visible = flag;
		if (flag)
		{
			IStyle style = dragElement.style;
			dragElement.visible = true;
			if (direction == SliderDirection.Horizontal)
			{
				float b = ((base.resolvedStyle.minWidth == StyleKeyword.Auto) ? 0f : base.resolvedStyle.minWidth.value);
				style.width = Mathf.Round(Mathf.Max(base.visualInput.layout.width * factor, b));
			}
			else
			{
				float b2 = ((base.resolvedStyle.minHeight == StyleKeyword.Auto) ? 0f : base.resolvedStyle.minHeight.value);
				style.height = Mathf.Round(Mathf.Max(base.visualInput.layout.height * factor, b2));
			}
		}
		dragBorderElement.visible = dragElement.visible;
	}

	private void UpdateDragElementPosition(GeometryChangedEvent evt)
	{
		if (!(evt.oldRect.size == evt.newRect.size))
		{
			UpdateDragElementPosition();
		}
	}

	internal override void OnViewDataReady()
	{
		base.OnViewDataReady();
		UpdateDragElementPosition();
	}

	private bool SameValues(float a, float b, float epsilon)
	{
		return Mathf.Abs(b - a) < epsilon;
	}

	private void UpdateDragElementPosition()
	{
		if (base.panel == null)
		{
			return;
		}
		float num = SliderNormalizeValue(value, lowValue, highValue);
		if (direction == SliderDirection.Horizontal)
		{
			float width = dragElement.resolvedStyle.width;
			float num2 = 0f - dragElement.resolvedStyle.marginLeft - dragElement.resolvedStyle.marginRight;
			float num3 = base.visualInput.layout.width - width + num2;
			float num4 = num * num3;
			if (!float.IsNaN(num4))
			{
				float epsilon = base.scaledPixelsPerPoint * 0.5f;
				float x = dragElement.transform.position.x;
				if (!SameValues(x, num4, epsilon))
				{
					Vector3 position = new Vector3(num4, 0f, 0f);
					dragElement.transform.position = position;
					dragBorderElement.transform.position = position;
				}
			}
			return;
		}
		float height = dragElement.resolvedStyle.height;
		float num5 = base.visualInput.resolvedStyle.height - height;
		float num6 = num * num5;
		if (!float.IsNaN(num6))
		{
			float epsilon2 = base.scaledPixelsPerPoint * 0.5f;
			float y = dragElement.transform.position.y;
			if (!SameValues(y, num6, epsilon2))
			{
				Vector3 position2 = new Vector3(0f, num6, 0f);
				dragElement.transform.position = position2;
				dragBorderElement.transform.position = position2;
			}
		}
	}

	protected override void ExecuteDefaultAction(EventBase evt)
	{
		base.ExecuteDefaultAction(evt);
		if (evt != null && evt.eventTypeId == EventBase<GeometryChangedEvent>.TypeId())
		{
			UpdateDragElementPosition((GeometryChangedEvent)evt);
		}
	}
}
