using System;

namespace UnityEngine.UIElements;

public class SliderInt : BaseSlider<int>
{
	public new class UxmlFactory : UxmlFactory<SliderInt, UxmlTraits>
	{
	}

	public new class UxmlTraits : BaseFieldTraits<int, UxmlIntAttributeDescription>
	{
		private UxmlIntAttributeDescription m_LowValue = new UxmlIntAttributeDescription
		{
			name = "low-value"
		};

		private UxmlIntAttributeDescription m_HighValue = new UxmlIntAttributeDescription
		{
			name = "high-value",
			defaultValue = 10
		};

		private UxmlIntAttributeDescription m_PageSize = new UxmlIntAttributeDescription
		{
			name = "page-size",
			defaultValue = 0
		};

		private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
		{
			name = "direction",
			defaultValue = SliderDirection.Horizontal
		};

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			SliderInt sliderInt = (SliderInt)ve;
			sliderInt.lowValue = m_LowValue.GetValueFromBag(bag, cc);
			sliderInt.highValue = m_HighValue.GetValueFromBag(bag, cc);
			sliderInt.direction = m_Direction.GetValueFromBag(bag, cc);
			sliderInt.pageSize = m_PageSize.GetValueFromBag(bag, cc);
			base.Init(ve, bag, cc);
		}
	}

	internal const int kDefaultHighValue = 10;

	public new static readonly string ussClassName = "unity-slider-int";

	public new static readonly string labelUssClassName = ussClassName + "__label";

	public new static readonly string inputUssClassName = ussClassName + "__input";

	public override float pageSize
	{
		get
		{
			return base.pageSize;
		}
		set
		{
			base.pageSize = Mathf.RoundToInt(value);
		}
	}

	public SliderInt()
		: this(null)
	{
	}

	public SliderInt(int start, int end, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
		: this(null, start, end, direction, pageSize)
	{
	}

	public SliderInt(string label, int start = 0, int end = 10, SliderDirection direction = SliderDirection.Horizontal, float pageSize = 0f)
		: base(label, start, end, direction, pageSize)
	{
		AddToClassList(ussClassName);
		base.labelElement.AddToClassList(labelUssClassName);
		base.visualInput.AddToClassList(inputUssClassName);
	}

	internal override int SliderLerpUnclamped(int a, int b, float interpolant)
	{
		return Mathf.RoundToInt(Mathf.LerpUnclamped(a, b, interpolant));
	}

	internal override float SliderNormalizeValue(int currentValue, int lowerValue, int higherValue)
	{
		return ((float)currentValue - (float)lowerValue) / ((float)higherValue - (float)lowerValue);
	}

	internal override int SliderRange()
	{
		return Math.Abs(base.highValue - base.lowValue);
	}

	internal override void ComputeValueAndDirectionFromClick(float sliderLength, float dragElementLength, float dragElementPos, float dragElementLastPos)
	{
		if (Mathf.Approximately(pageSize, 0f))
		{
			base.ComputeValueAndDirectionFromClick(sliderLength, dragElementLength, dragElementPos, dragElementLastPos);
			return;
		}
		float f = sliderLength - dragElementLength;
		if (!(Mathf.Abs(f) < Mathf.Epsilon))
		{
			int num = (int)pageSize;
			if (base.lowValue > base.highValue)
			{
				num = -num;
			}
			if (dragElementLastPos < dragElementPos && base.clampedDragger.dragDirection != ClampedDragger<int>.DragDirection.LowToHigh)
			{
				base.clampedDragger.dragDirection = ClampedDragger<int>.DragDirection.HighToLow;
				value -= num;
			}
			else if (dragElementLastPos > dragElementPos + dragElementLength && base.clampedDragger.dragDirection != ClampedDragger<int>.DragDirection.HighToLow)
			{
				base.clampedDragger.dragDirection = ClampedDragger<int>.DragDirection.LowToHigh;
				value += num;
			}
		}
	}
}
