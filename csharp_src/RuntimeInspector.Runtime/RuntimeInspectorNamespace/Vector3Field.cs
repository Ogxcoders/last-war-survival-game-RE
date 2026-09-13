using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class Vector3Field : InspectorField
{
	[SerializeField]
	private BoundInputField inputX;

	[SerializeField]
	private BoundInputField inputY;

	[SerializeField]
	private BoundInputField inputZ;

	[SerializeField]
	private Text labelX;

	[SerializeField]
	private Text labelY;

	[SerializeField]
	private Text labelZ;

	private bool isVector3Int;

	public override void Initialize()
	{
		base.Initialize();
		inputX.Initialize();
		inputY.Initialize();
		inputZ.Initialize();
		BoundInputField boundInputField = inputX;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField2 = inputY;
		boundInputField2.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField3 = inputZ;
		boundInputField3.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField3.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField4 = inputX;
		boundInputField4.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField4.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField5 = inputY;
		boundInputField5.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField5.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField6 = inputZ;
		boundInputField6.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField6.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		inputX.DefaultEmptyValue = "0";
		inputY.DefaultEmptyValue = "0";
		inputZ.DefaultEmptyValue = "0";
	}

	public override bool SupportsType(Type type)
	{
		if (type == typeof(Vector3Int))
		{
			return true;
		}
		return type == typeof(Vector3);
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		isVector3Int = base.BoundVariableType == typeof(Vector3Int);
		if (isVector3Int)
		{
			Vector3Int vector3Int = (Vector3Int)base.Value;
			inputX.Text = vector3Int.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = vector3Int.y.ToString(RuntimeInspectorUtils.numberFormat);
			inputZ.Text = vector3Int.z.ToString(RuntimeInspectorUtils.numberFormat);
		}
		else
		{
			Vector3 vector = (Vector3)base.Value;
			inputX.Text = vector.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = vector.y.ToString(RuntimeInspectorUtils.numberFormat);
			inputZ.Text = vector.z.ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private bool OnValueChanged(BoundInputField source, string input)
	{
		float result2;
		if (isVector3Int)
		{
			if (int.TryParse(input, NumberStyles.Integer, RuntimeInspectorUtils.numberFormat, out var result))
			{
				Vector3Int vector3Int = (Vector3Int)base.Value;
				if (source == inputX)
				{
					vector3Int.x = result;
				}
				else if (source == inputY)
				{
					vector3Int.y = result;
				}
				else
				{
					vector3Int.z = result;
				}
				base.Value = vector3Int;
				return true;
			}
		}
		else if (float.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out result2))
		{
			Vector3 vector = (Vector3)base.Value;
			if (source == inputX)
			{
				vector.x = result2;
			}
			else if (source == inputY)
			{
				vector.y = result2;
			}
			else
			{
				vector.z = result2;
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
		labelZ.SetSkinText(base.Skin);
		inputX.Skin = base.Skin;
		inputY.Skin = base.Skin;
		inputZ.Skin = base.Skin;
		float num = (1f - base.Skin.LabelWidthPercentage) / 3f;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		Vector2 anchorMax = new Vector2(base.Skin.LabelWidthPercentage + num, 1f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)inputX.transform).SetAnchorMinMaxInputField(labelX.rectTransform, anchorMin, anchorMax);
		anchorMin.x += num;
		anchorMax.x += num;
		((RectTransform)inputY.transform).SetAnchorMinMaxInputField(labelY.rectTransform, anchorMin, anchorMax);
		anchorMin.x += num;
		anchorMax.x = 1f;
		((RectTransform)inputZ.transform).SetAnchorMinMaxInputField(labelZ.rectTransform, anchorMin, anchorMax);
	}

	public override void Refresh()
	{
		if (isVector3Int)
		{
			Vector3Int vector3Int = (Vector3Int)base.Value;
			base.Refresh();
			Vector3Int vector3Int2 = (Vector3Int)base.Value;
			if (vector3Int2.x != vector3Int.x)
			{
				inputX.Text = vector3Int2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (vector3Int2.y != vector3Int.y)
			{
				inputY.Text = vector3Int2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (vector3Int2.z != vector3Int.z)
			{
				inputZ.Text = vector3Int2.z.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
		else
		{
			Vector3 vector = (Vector3)base.Value;
			base.Refresh();
			Vector3 vector2 = (Vector3)base.Value;
			if (vector2.x != vector.x)
			{
				inputX.Text = vector2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (vector2.y != vector.y)
			{
				inputY.Text = vector2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (vector2.z != vector.z)
			{
				inputZ.Text = vector2.z.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
	}
}
