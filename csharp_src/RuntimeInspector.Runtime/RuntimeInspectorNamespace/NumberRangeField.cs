using System;
using System.Reflection;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class NumberRangeField : NumberField
{
	[SerializeField]
	private BoundSlider slider;

	public override void Initialize()
	{
		base.Initialize();
		BoundSlider boundSlider = slider;
		boundSlider.OnValueChanged = (BoundSlider.OnValueChangedDelegate)Delegate.Combine(boundSlider.OnValueChanged, new BoundSlider.OnValueChangedDelegate(OnSliderValueChanged));
	}

	public override bool CanBindTo(Type type, MemberInfo variable)
	{
		if (variable != null)
		{
			return variable.HasAttribute<RangeAttribute>();
		}
		return false;
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		RangeAttribute attribute = variable.GetAttribute<RangeAttribute>();
		slider.SetRange(Mathf.Max(attribute.min, numberHandler.MinValue), Mathf.Min(attribute.max, numberHandler.MaxValue));
		slider.BackingField.wholeNumbers = base.BoundVariableType != typeof(float) && base.BoundVariableType != typeof(double) && base.BoundVariableType != typeof(decimal);
	}

	protected override bool OnValueChanged(BoundInputField source, string input)
	{
		if (numberHandler.TryParse(input, out var value))
		{
			float num = numberHandler.ConvertToFloat(value);
			if (num >= slider.BackingField.minValue && num <= slider.BackingField.maxValue)
			{
				base.Value = value;
				return true;
			}
		}
		return false;
	}

	private void OnSliderValueChanged(BoundSlider source, float value)
	{
		if (!input.BackingField.isFocused)
		{
			base.Value = numberHandler.ConvertFromFloat(value);
			input.Text = numberHandler.ToString(base.Value);
			base.Inspector.RefreshDelayed();
		}
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		slider.Skin = base.Skin;
		float num = (1f - base.Skin.LabelWidthPercentage) / 3f;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)slider.transform).anchorMin = anchorMin;
		((RectTransform)slider.transform).anchorMax = new Vector2(1f - num, 1f);
		((RectTransform)input.transform).anchorMin = new Vector2(1f - num, 0f);
	}

	public override void Refresh()
	{
		base.Refresh();
		slider.Value = numberHandler.ConvertToFloat(base.Value);
	}
}
