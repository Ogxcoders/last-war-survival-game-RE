using System;

namespace UnityEngine.UIElements;

public class MinMaxSlider : BaseField<Vector2>
{
	public new class UxmlFactory : UxmlFactory<MinMaxSlider, UxmlTraits>
	{
	}

	public new class UxmlTraits : BaseField<Vector2>.UxmlTraits
	{
		private UxmlFloatAttributeDescription m_MinValue = new UxmlFloatAttributeDescription
		{
			name = "min-value",
			defaultValue = 0f
		};

		private UxmlFloatAttributeDescription m_MaxValue = new UxmlFloatAttributeDescription
		{
			name = "max-value",
			defaultValue = 10f
		};

		private UxmlFloatAttributeDescription m_LowLimit = new UxmlFloatAttributeDescription
		{
			name = "low-limit",
			defaultValue = float.MinValue
		};

		private UxmlFloatAttributeDescription m_HighLimit = new UxmlFloatAttributeDescription
		{
			name = "high-limit",
			defaultValue = float.MaxValue
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			MinMaxSlider minMaxSlider = (MinMaxSlider)ve;
			minMaxSlider.minValue = m_MinValue.GetValueFromBag(bag, cc);
			minMaxSlider.maxValue = m_MaxValue.GetValueFromBag(bag, cc);
			minMaxSlider.lowLimit = m_LowLimit.GetValueFromBag(bag, cc);
			minMaxSlider.highLimit = m_HighLimit.GetValueFromBag(bag, cc);
		}
	}

	private enum DragState
	{
		NoThumb,
		MinThumb,
		MiddleThumb,
		MaxThumb
	}

	private Vector2 m_DragElementStartPos;

	private Vector2 m_ValueStartPos;

	private Rect m_DragMinThumbRect;

	private Rect m_DragMaxThumbRect;

	private DragState m_DragState;

	private float m_MinLimit;

	private float m_MaxLimit;

	internal const float kDefaultHighValue = 10f;

	public new static readonly string ussClassName = "unity-min-max-slider";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	public static readonly string trackerUssClassName = ussClassName + "__tracker";

	public static readonly string draggerUssClassName = ussClassName + "__dragger";

	public static readonly string minThumbUssClassName = ussClassName + "__min-thumb";

	public static readonly string maxThumbUssClassName = ussClassName + "__max-thumb";

	internal VisualElement dragElement { get; private set; }

	private VisualElement dragMinThumb { get; set; }

	private VisualElement dragMaxThumb { get; set; }

	internal ClampedDragger<float> clampedDragger { get; private set; }

	public float minValue
	{
		get
		{
			return value.x;
		}
		set
		{
			base.value = ClampValues(new Vector2(value, base.rawValue.y));
		}
	}

	public float maxValue
	{
		get
		{
			return value.y;
		}
		set
		{
			base.value = ClampValues(new Vector2(base.rawValue.x, value));
		}
	}

	public override Vector2 value
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = ClampValues(value);
		}
	}

	public float range => Math.Abs(highLimit - lowLimit);

	public float lowLimit
	{
		get
		{
			return m_MinLimit;
		}
		set
		{
			if (!Mathf.Approximately(m_MinLimit, value))
			{
				if (value > m_MaxLimit)
				{
					throw new ArgumentException("lowLimit is greater than highLimit");
				}
				m_MinLimit = value;
				this.value = base.rawValue;
				UpdateDragElementPosition();
				if (!string.IsNullOrEmpty(base.viewDataKey))
				{
					SaveViewData();
				}
			}
		}
	}

	public float highLimit
	{
		get
		{
			return m_MaxLimit;
		}
		set
		{
			if (!Mathf.Approximately(m_MaxLimit, value))
			{
				if (value < m_MinLimit)
				{
					throw new ArgumentException("highLimit is smaller than lowLimit");
				}
				m_MaxLimit = value;
				this.value = base.rawValue;
				UpdateDragElementPosition();
				if (!string.IsNullOrEmpty(base.viewDataKey))
				{
					SaveViewData();
				}
			}
		}
	}

	public override void SetValueWithoutNotify(Vector2 newValue)
	{
		base.SetValueWithoutNotify(ClampValues(newValue));
		UpdateDragElementPosition();
	}

	public MinMaxSlider()
		: this(null)
	{
	}

	public MinMaxSlider(float minValue, float maxValue, float minLimit, float maxLimit)
		: this(null, minValue, maxValue, minLimit, maxLimit)
	{
	}

	public MinMaxSlider(string label, float minValue = 0f, float maxValue = 10f, float minLimit = float.MinValue, float maxLimit = float.MaxValue)
		: base(label, (VisualElement)null)
	{
		lowLimit = minLimit;
		highLimit = maxLimit;
		this.minValue = minValue;
		this.maxValue = maxValue;
		AddToClassList(ussClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
		base.pickingMode = PickingMode.Ignore;
		m_DragState = DragState.NoThumb;
		base.visualInput.pickingMode = PickingMode.Position;
		VisualElement visualElement = new VisualElement
		{
			name = "unity-tracker"
		};
		visualElement.AddToClassList(trackerUssClassName);
		base.visualInput.Add(visualElement);
		dragElement = new VisualElement
		{
			name = "unity-dragger"
		};
		dragElement.AddToClassList(draggerUssClassName);
		dragElement.RegisterCallback<GeometryChangedEvent>(UpdateDragElementPosition);
		base.visualInput.Add(dragElement);
		dragMinThumb = new VisualElement
		{
			name = "unity-thumb-min"
		};
		dragMaxThumb = new VisualElement
		{
			name = "unity-thumb-max"
		};
		dragMinThumb.AddToClassList(minThumbUssClassName);
		dragMaxThumb.AddToClassList(maxThumbUssClassName);
		dragElement.Add(dragMinThumb);
		dragElement.Add(dragMaxThumb);
		clampedDragger = new ClampedDragger<float>(null, SetSliderValueFromClick, SetSliderValueFromDrag);
		base.visualInput.AddManipulator(clampedDragger);
		m_MinLimit = minLimit;
		m_MaxLimit = maxLimit;
		base.rawValue = ClampValues(new Vector2(minValue, maxValue));
		UpdateDragElementPosition();
	}

	private Vector2 ClampValues(Vector2 valueToClamp)
	{
		if (m_MinLimit > m_MaxLimit)
		{
			m_MinLimit = m_MaxLimit;
		}
		Vector2 result = default(Vector2);
		if (valueToClamp.y > m_MaxLimit)
		{
			valueToClamp.y = m_MaxLimit;
		}
		result.x = Mathf.Clamp(valueToClamp.x, m_MinLimit, valueToClamp.y);
		result.y = Mathf.Clamp(valueToClamp.y, valueToClamp.x, m_MaxLimit);
		return result;
	}

	private void UpdateDragElementPosition(GeometryChangedEvent evt)
	{
		if (!(evt.oldRect.size == evt.newRect.size))
		{
			UpdateDragElementPosition();
		}
	}

	private void UpdateDragElementPosition()
	{
		if (base.panel != null)
		{
			float num = 0f - dragElement.resolvedStyle.marginLeft - dragElement.resolvedStyle.marginRight;
			int num2 = dragElement.resolvedStyle.unitySliceLeft + dragElement.resolvedStyle.unitySliceRight;
			float num3 = Mathf.Round(SliderLerpUnclamped(dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width + num - (float)dragElement.resolvedStyle.unitySliceRight, SliderNormalizeValue(minValue, lowLimit, highLimit)) - (float)dragElement.resolvedStyle.unitySliceLeft);
			float num4 = Mathf.Round(SliderLerpUnclamped(dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width + num - (float)dragElement.resolvedStyle.unitySliceRight, SliderNormalizeValue(maxValue, lowLimit, highLimit)) + (float)dragElement.resolvedStyle.unitySliceRight);
			dragElement.style.width = Mathf.Max(num2, num4 - num3);
			dragElement.style.left = num3;
			m_DragMinThumbRect = new Rect(dragElement.resolvedStyle.left, dragElement.layout.yMin, dragElement.resolvedStyle.unitySliceLeft, dragElement.resolvedStyle.height);
			m_DragMaxThumbRect = new Rect(dragElement.resolvedStyle.left + (dragElement.resolvedStyle.width - (float)dragElement.resolvedStyle.unitySliceRight), dragElement.layout.yMin, dragElement.resolvedStyle.unitySliceRight, dragElement.resolvedStyle.height);
			dragMaxThumb.style.left = dragElement.resolvedStyle.width - (float)dragElement.resolvedStyle.unitySliceRight;
			dragMaxThumb.style.top = 0f;
			dragMinThumb.style.width = m_DragMinThumbRect.width;
			dragMinThumb.style.height = m_DragMinThumbRect.height;
			dragMinThumb.style.left = 0f;
			dragMinThumb.style.top = 0f;
			dragMaxThumb.style.width = m_DragMaxThumbRect.width;
			dragMaxThumb.style.height = m_DragMaxThumbRect.height;
		}
	}

	internal float SliderLerpUnclamped(float a, float b, float interpolant)
	{
		return Mathf.LerpUnclamped(a, b, interpolant);
	}

	internal float SliderNormalizeValue(float currentValue, float lowerValue, float higherValue)
	{
		return (currentValue - lowerValue) / (higherValue - lowerValue);
	}

	private float ComputeValueFromPosition(float positionToConvert)
	{
		float num = 0f;
		num = SliderNormalizeValue(positionToConvert, dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width - (float)dragElement.resolvedStyle.unitySliceRight);
		return SliderLerpUnclamped(lowLimit, highLimit, num);
	}

	protected override void ExecuteDefaultAction(EventBase evt)
	{
		base.ExecuteDefaultAction(evt);
		if (evt != null && evt.eventTypeId == EventBase<GeometryChangedEvent>.TypeId())
		{
			UpdateDragElementPosition((GeometryChangedEvent)evt);
		}
	}

	private void SetSliderValueFromDrag()
	{
		if (clampedDragger.dragDirection == ClampedDragger<float>.DragDirection.Free)
		{
			float x = m_DragElementStartPos.x;
			float dragElementEndPos = x + clampedDragger.delta.x;
			ComputeValueFromDraggingThumb(x, dragElementEndPos);
		}
	}

	private void SetSliderValueFromClick()
	{
		if (clampedDragger.dragDirection != ClampedDragger<float>.DragDirection.Free)
		{
			if (m_DragMinThumbRect.Contains(clampedDragger.startMousePosition))
			{
				m_DragState = DragState.MinThumb;
			}
			else if (m_DragMaxThumbRect.Contains(clampedDragger.startMousePosition))
			{
				m_DragState = DragState.MaxThumb;
			}
			else if (dragElement.layout.Contains(clampedDragger.startMousePosition))
			{
				m_DragState = DragState.MiddleThumb;
			}
			else
			{
				m_DragState = DragState.NoThumb;
			}
			if (m_DragState == DragState.NoThumb)
			{
				m_DragElementStartPos = new Vector2(clampedDragger.startMousePosition.x, dragElement.resolvedStyle.top);
				clampedDragger.dragDirection = ClampedDragger<float>.DragDirection.Free;
				ComputeValueDragStateNoThumb(dragElement.resolvedStyle.unitySliceLeft, base.visualInput.layout.width - (float)dragElement.resolvedStyle.unitySliceRight, m_DragElementStartPos.x);
				m_DragState = DragState.MiddleThumb;
				m_ValueStartPos = value;
			}
			else
			{
				m_ValueStartPos = value;
				clampedDragger.dragDirection = ClampedDragger<float>.DragDirection.Free;
				m_DragElementStartPos = clampedDragger.startMousePosition;
			}
		}
	}

	private void ComputeValueDragStateNoThumb(float lowLimitPosition, float highLimitPosition, float dragElementPos)
	{
		float num = ((dragElementPos < lowLimitPosition) ? lowLimit : ((!(dragElementPos > highLimitPosition)) ? ComputeValueFromPosition(dragElementPos) : highLimit));
		float num2 = maxValue - minValue;
		float num3 = num - num2;
		float y = num;
		if (num3 < lowLimit)
		{
			num3 = lowLimit;
			y = num3 + num2;
		}
		value = new Vector2(num3, y);
	}

	private void ComputeValueFromDraggingThumb(float dragElementStartPos, float dragElementEndPos)
	{
		float num = ComputeValueFromPosition(dragElementStartPos);
		float num2 = ComputeValueFromPosition(dragElementEndPos);
		float num3 = num2 - num;
		switch (m_DragState)
		{
		case DragState.MiddleThumb:
		{
			Vector2 vector = value;
			vector.x = m_ValueStartPos.x + num3;
			vector.y = m_ValueStartPos.y + num3;
			float num5 = m_ValueStartPos.y - m_ValueStartPos.x;
			if (vector.x < lowLimit)
			{
				vector.x = lowLimit;
				vector.y = lowLimit + num5;
			}
			else if (vector.y > highLimit)
			{
				vector.y = highLimit;
				vector.x = highLimit - num5;
			}
			value = vector;
			break;
		}
		case DragState.MinThumb:
		{
			float num6 = m_ValueStartPos.x + num3;
			if (num6 > maxValue)
			{
				num6 = maxValue;
			}
			else if (num6 < lowLimit)
			{
				num6 = lowLimit;
			}
			value = new Vector2(num6, maxValue);
			break;
		}
		case DragState.MaxThumb:
		{
			float num4 = m_ValueStartPos.y + num3;
			if (num4 < minValue)
			{
				num4 = minValue;
			}
			else if (num4 > highLimit)
			{
				num4 = highLimit;
			}
			value = new Vector2(minValue, num4);
			break;
		}
		}
	}
}
