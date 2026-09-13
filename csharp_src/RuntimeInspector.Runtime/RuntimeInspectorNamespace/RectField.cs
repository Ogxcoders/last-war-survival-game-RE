using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class RectField : InspectorField
{
	[SerializeField]
	private BoundInputField inputX;

	[SerializeField]
	private BoundInputField inputY;

	[SerializeField]
	private BoundInputField inputW;

	[SerializeField]
	private BoundInputField inputH;

	[SerializeField]
	private Text labelX;

	[SerializeField]
	private Text labelY;

	[SerializeField]
	private Text labelW;

	[SerializeField]
	private Text labelH;

	private bool isRectInt;

	protected override float HeightMultiplier => 2f;

	public override void Initialize()
	{
		base.Initialize();
		inputX.Initialize();
		inputY.Initialize();
		inputW.Initialize();
		inputH.Initialize();
		BoundInputField boundInputField = inputX;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField2 = inputY;
		boundInputField2.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField3 = inputW;
		boundInputField3.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField3.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField4 = inputH;
		boundInputField4.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField4.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField5 = inputX;
		boundInputField5.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField5.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField6 = inputY;
		boundInputField6.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField6.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField7 = inputW;
		boundInputField7.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField7.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField8 = inputH;
		boundInputField8.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField8.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		inputX.DefaultEmptyValue = "0";
		inputY.DefaultEmptyValue = "0";
		inputW.DefaultEmptyValue = "0";
		inputH.DefaultEmptyValue = "0";
	}

	public override bool SupportsType(Type type)
	{
		if (type == typeof(RectInt))
		{
			return true;
		}
		return type == typeof(Rect);
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		isRectInt = base.BoundVariableType == typeof(RectInt);
		if (isRectInt)
		{
			RectInt rectInt = (RectInt)base.Value;
			inputX.Text = rectInt.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = rectInt.y.ToString(RuntimeInspectorUtils.numberFormat);
			inputW.Text = rectInt.width.ToString(RuntimeInspectorUtils.numberFormat);
			inputH.Text = rectInt.height.ToString(RuntimeInspectorUtils.numberFormat);
		}
		else
		{
			Rect rect = (Rect)base.Value;
			inputX.Text = rect.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = rect.y.ToString(RuntimeInspectorUtils.numberFormat);
			inputW.Text = rect.width.ToString(RuntimeInspectorUtils.numberFormat);
			inputH.Text = rect.height.ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private bool OnValueChanged(BoundInputField source, string input)
	{
		float result2;
		if (isRectInt)
		{
			if (int.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out var result))
			{
				RectInt rectInt = (RectInt)base.Value;
				if (source == inputX)
				{
					rectInt.x = result;
				}
				else if (source == inputY)
				{
					rectInt.y = result;
				}
				else if (source == inputW)
				{
					rectInt.width = result;
				}
				else
				{
					rectInt.height = result;
				}
				base.Value = rectInt;
				return true;
			}
		}
		else if (float.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out result2))
		{
			Rect rect = (Rect)base.Value;
			if (source == inputX)
			{
				rect.x = result2;
			}
			else if (source == inputY)
			{
				rect.y = result2;
			}
			else if (source == inputW)
			{
				rect.width = result2;
			}
			else
			{
				rect.height = result2;
			}
			base.Value = rect;
			return true;
		}
		return false;
	}

	private bool OnValueSubmitted(BoundInputField source, string input)
	{
		base.Inspector.RefreshDelayed();
		return OnValueChanged(source, input);
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		labelX.SetSkinText(base.Skin);
		labelY.SetSkinText(base.Skin);
		labelW.SetSkinText(base.Skin);
		labelH.SetSkinText(base.Skin);
		inputX.Skin = base.Skin;
		inputY.Skin = base.Skin;
		inputW.Skin = base.Skin;
		inputH.Skin = base.Skin;
		float num = (1f - base.Skin.LabelWidthPercentage) / 3f;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage + num, 0f);
		Vector2 anchorMax = new Vector2(base.Skin.LabelWidthPercentage + 2f * num, 1f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)inputX.transform).SetAnchorMinMaxInputField(labelX.rectTransform, new Vector2(anchorMin.x, 0.5f), anchorMax);
		((RectTransform)inputW.transform).SetAnchorMinMaxInputField(labelW.rectTransform, anchorMin, new Vector2(anchorMax.x, 0.5f));
		anchorMin.x += num;
		anchorMax.x = 1f;
		((RectTransform)inputY.transform).SetAnchorMinMaxInputField(labelY.rectTransform, new Vector2(anchorMin.x, 0.5f), anchorMax);
		((RectTransform)inputH.transform).SetAnchorMinMaxInputField(labelH.rectTransform, anchorMin, new Vector2(anchorMax.x, 0.5f));
	}

	public override void Refresh()
	{
		if (isRectInt)
		{
			RectInt rectInt = (RectInt)base.Value;
			base.Refresh();
			RectInt rectInt2 = (RectInt)base.Value;
			if (rectInt2.x != rectInt.x)
			{
				inputX.Text = rectInt2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (rectInt2.y != rectInt.y)
			{
				inputY.Text = rectInt2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (rectInt2.width != rectInt.width)
			{
				inputW.Text = rectInt2.width.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (rectInt2.height != rectInt.height)
			{
				inputH.Text = rectInt2.height.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
		else
		{
			Rect rect = (Rect)base.Value;
			base.Refresh();
			Rect rect2 = (Rect)base.Value;
			if (rect2.x != rect.x)
			{
				inputX.Text = rect2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (rect2.y != rect.y)
			{
				inputY.Text = rect2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (rect2.width != rect.width)
			{
				inputW.Text = rect2.width.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (rect2.height != rect.height)
			{
				inputH.Text = rect2.height.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
	}
}
