using System;

namespace UnityEngine.UIElements;

public class Slider : BaseSlider<float>
{
	public new class UxmlFactory : UxmlFactory<Slider, UxmlTraits>
	{
	}

	public new class UxmlTraits : BaseFieldTraits<float, UxmlFloatAttributeDescription>
	{
		private UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription
		{
			name = "low-value"
		};

		private UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription
		{
			name = "high-value",
			defaultValue = 10f
		};

		private UxmlFloatAttributeDescription m_PageSize = new UxmlFloatAttributeDescription
		{
			name = "page-size",
			defaultValue = 0f
		};

		private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
		{
			name = "direction",
			defaultValue = SliderDirection.Horizontal
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			Slider slider = (Slider)ve;
			slider.lowValue = m_LowValue.GetValueFromBag(bag, cc);
			slider.highValue = m_HighValue.GetValueFromBag(bag, cc);
			slider.direction = m_Direction.GetValueFromBag(bag, cc);
			slider.pageSize = m_PageSize.GetValueFromBag(bag, cc);
			base.Init(ve, bag, cc);
		}
	}

	internal const float kDefaultHighValue = 10f;

	public new static readonly string ussClassName = "unity-slider";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	public Slider()
		: this(null)
	{
	}

	public Slider(float start, float end, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
		: this(null, start, end, direction, pageSize)
	{
	}

	public Slider(string label, float start = 0f, float end = 10f, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
		: base(label, start, end, direction, pageSize)
	{
		AddToClassList(ussClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
	}

	internal override float SliderLerpUnclamped(float a, float b, float interpolant)
	{
		return Mathf.LerpUnclamped(a, b, interpolant);
	}

	internal override float SliderNormalizeValue(float currentValue, float lowerValue, float higherValue)
	{
		return (currentValue - lowerValue) / (higherValue - lowerValue);
	}

	internal override float SliderRange()
	{
		return Math.Abs(base.highValue - base.lowValue);
	}
}
