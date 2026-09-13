using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class Vector2Field : InspectorField
{
	[SerializeField]
	private BoundInputField inputX;

	[SerializeField]
	private BoundInputField inputY;

	[SerializeField]
	private Text labelX;

	[SerializeField]
	private Text labelY;

	private bool isVector2Int;

	public override void Initialize()
	{
		base.Initialize();
		inputX.Initialize();
		inputY.Initialize();
		BoundInputField boundInputField = inputX;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField2 = inputY;
		boundInputField2.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField3 = inputX;
		boundInputField3.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField3.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField4 = inputY;
		boundInputField4.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField4.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		inputX.DefaultEmptyValue = "0";
		inputY.DefaultEmptyValue = "0";
	}

	public override bool SupportsType(Type type)
	{
		if (type == typeof(Vector2Int))
		{
			return true;
		}
		return type == typeof(Vector2);
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		isVector2Int = base.BoundVariableType == typeof(Vector2Int);
		if (isVector2Int)
		{
			Vector2Int vector2Int = (Vector2Int)base.Value;
			inputX.Text = vector2Int.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = vector2Int.y.ToString(RuntimeInspectorUtils.numberFormat);
		}
		else
		{
			Vector2 vector = (Vector2)base.Value;
			inputX.Text = vector.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = vector.y.ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private bool OnValueChanged(BoundInputField source, string input)
	{
		float result2;
		if (isVector2Int)
		{
			if (int.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out var result))
			{
				Vector2Int vector2Int = (Vector2Int)base.Value;
				if (source == inputX)
				{
					vector2Int.x = result;
				}
				else
				{
					vector2Int.y = result;
				}
				base.Value = vector2Int;
				return true;
			}
		}
		else if (float.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out result2))
		{
			Vector2 vector = (Vector2)base.Value;
			if (source == inputX)
			{
				vector.x = result2;
			}
			else
			{
				vector.y = result2;
			}
			base.Value = vector;
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
		inputX.Skin = base.Skin;
		inputY.Skin = base.Skin;
		float num = (1f - base.Skin.LabelWidthPercentage) / 3f;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage + num, 0f);
		Vector2 anchorMax = new Vector2(base.Skin.LabelWidthPercentage + 2f * num, 1f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)inputX.transform).SetAnchorMinMaxInputField(labelX.rectTransform, anchorMin, anchorMax);
		anchorMin.x += num;
		anchorMax.x = 1f;
		((RectTransform)inputY.transform).SetAnchorMinMaxInputField(labelY.rectTransform, anchorMin, anchorMax);
	}

	public override void Refresh()
	{
		if (isVector2Int)
		{
			Vector2Int vector2Int = (Vector2Int)base.Value;
			base.Refresh();
			Vector2Int vector2Int2 = (Vector2Int)base.Value;
			if (vector2Int2.x != vector2Int.x)
			{
				inputX.Text = vector2Int2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (vector2Int2.y != vector2Int.y)
			{
				inputY.Text = vector2Int2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
		else
		{
			Vector2 vector = (Vector2)base.Value;
			base.Refresh();
			Vector2 vector2 = (Vector2)base.Value;
			if (vector2.x != vector.x)
			{
				inputX.Text = vector2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (vector2.y != vector.y)
			{
				inputY.Text = vector2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
	}
}
