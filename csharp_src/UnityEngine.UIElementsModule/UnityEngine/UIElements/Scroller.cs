using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class Scroller : VisualElement
{
	public new class UxmlFactory : UxmlFactory<Scroller, UxmlTraits>
	{
	}

	public new class UxmlTraits : VisualElement.UxmlTraits
	{
		private UxmlFloatAttributeDescription m_LowValue = new UxmlFloatAttributeDescription
		{
			name = "low-value",
			obsoleteNames = new string[1] { "lowValue" }
		};

		private UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription
		{
			name = "high-value",
			obsoleteNames = new string[1] { "highValue" }
		};

		private UxmlEnumAttributeDescription<SliderDirection> m_Direction = new UxmlEnumAttributeDescription<SliderDirection>
		{
			name = "direction",
			defaultValue = SliderDirection.Vertical
		};

		private UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription
		{
			name = "value"
		};

		public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
		{
			get
			{
				yield break;
			}
		}

		public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
		{
			base.Init(ve, bag, cc);
			Scroller scroller = (Scroller)ve;
			scroller.slider.lowValue = m_LowValue.GetValueFromBag(bag, cc);
			scroller.slider.highValue = m_HighValue.GetValueFromBag(bag, cc);
			scroller.direction = m_Direction.GetValueFromBag(bag, cc);
			scroller.value = m_Value.GetValueFromBag(bag, cc);
		}
	}

	internal const float kDefaultPageSize = 20f;

	public static readonly string ussClassName = "unity-scroller";

	public static readonly string horizontalVariantUssClassName = ussClassName + "--horizontal";

	public static readonly string verticalVariantUssClassName = ussClassName + "--vertical";

	public static readonly string sliderUssClassName = ussClassName + "__slider";

	public static readonly string lowButtonUssClassName = ussClassName + "__low-button";

	public static readonly string highButtonUssClassName = ussClassName + "__high-button";

	public Slider slider { get; private set; }

	public RepeatButton lowButton { get; private set; }

	public RepeatButton highButton { get; private set; }

	public float value
	{
		get
		{
			return slider.value;
		}
		set
		{
			slider.value = value;
		}
	}

	public float lowValue
	{
		get
		{
			return slider.lowValue;
		}
		set
		{
			slider.lowValue = value;
		}
	}

	public float highValue
	{
		get
		{
			return slider.highValue;
		}
		set
		{
			slider.highValue = value;
		}
	}

	public SliderDirection direction
	{
		get
		{
			return (base.resolvedStyle.flexDirection != FlexDirection.Row) ? SliderDirection.Vertical : SliderDirection.Horizontal;
		}
		set
		{
			if (value == SliderDirection.Horizontal)
			{
				base.style.flexDirection = FlexDirection.Row;
				AddToClassList(horizontalVariantUssClassName);
				RemoveFromClassList(verticalVariantUssClassName);
			}
			else
			{
				base.style.flexDirection = FlexDirection.Column;
				AddToClassList(verticalVariantUssClassName);
				RemoveFromClassList(horizontalVariantUssClassName);
			}
		}
	}

	public event Action<float> valueChanged;

	public Scroller()
		: this(0f, 0f, null)
	{
	}

	public Scroller(float lowValue, float highValue, Action<float> valueChanged, SliderDirection direction = SliderDirection.Vertical)
	{
		AddToClassList(ussClassName);
		this.direction = direction;
		this.valueChanged = valueChanged;
		slider = new Slider(lowValue, highValue, direction, 20f)
		{
			name = "unity-slider",
			viewDataKey = "Slider"
		};
		slider.AddToClassList(sliderUssClassName);
		slider.RegisterValueChangedCallback(OnSliderValueChange);
		lowButton = new RepeatButton(ScrollPageUp, 250L, 30L)
		{
			name = "unity-low-button"
		};
		lowButton.AddToClassList(lowButtonUssClassName);
		Add(lowButton);
		highButton = new RepeatButton(ScrollPageDown, 250L, 30L)
		{
			name = "unity-high-button"
		};
		highButton.AddToClassList(highButtonUssClassName);
		Add(highButton);
		Add(slider);
	}

	public void Adjust(float factor)
	{
		SetEnabled(factor < 1f);
		slider.AdjustDragElement(factor);
	}

	private void OnSliderValueChange(ChangeEvent<float> evt)
	{
		value = evt.newValue;
		this.valueChanged?.Invoke(slider.value);
		IncrementVersion(VersionChangeType.Repaint);
	}

	public void ScrollPageUp()
	{
		ScrollPageUp(1f);
	}

	public void ScrollPageDown()
	{
		ScrollPageDown(1f);
	}

	public void ScrollPageUp(float factor)
	{
		value -= factor * (slider.pageSize * ((slider.lowValue < slider.highValue) ? 1f : (-1f)));
	}

	public void ScrollPageDown(float factor)
	{
		value += factor * (slider.pageSize * ((slider.lowValue < slider.highValue) ? 1f : (-1f)));
	}
}
