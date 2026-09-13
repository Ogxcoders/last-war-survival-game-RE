using System;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class Vector4Field : InspectorField
{
	[SerializeField]
	private BoundInputField inputX;

	[SerializeField]
	private BoundInputField inputY;

	[SerializeField]
	private BoundInputField inputZ;

	[SerializeField]
	private BoundInputField inputW;

	[SerializeField]
	private Text labelX;

	[SerializeField]
	private Text labelY;

	[SerializeField]
	private Text labelZ;

	[SerializeField]
	private Text labelW;

	private bool isQuaternion;

	protected override float HeightMultiplier => 2f;

	public override void Initialize()
	{
		base.Initialize();
		inputX.Initialize();
		inputY.Initialize();
		inputZ.Initialize();
		inputW.Initialize();
		BoundInputField boundInputField = inputX;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField2 = inputY;
		boundInputField2.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField3 = inputZ;
		boundInputField3.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField3.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField4 = inputW;
		boundInputField4.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField4.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField5 = inputX;
		boundInputField5.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField5.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField6 = inputY;
		boundInputField6.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField6.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField7 = inputZ;
		boundInputField7.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField7.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		BoundInputField boundInputField8 = inputW;
		boundInputField8.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField8.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		inputX.DefaultEmptyValue = "0";
		inputY.DefaultEmptyValue = "0";
		inputZ.DefaultEmptyValue = "0";
		inputW.DefaultEmptyValue = "0";
	}

	public override bool SupportsType(Type type)
	{
		if (!(type == typeof(Vector4)))
		{
			return type == typeof(Quaternion);
		}
		return true;
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		isQuaternion = base.BoundVariableType == typeof(Quaternion);
		if (isQuaternion)
		{
			Quaternion quaternion = (Quaternion)base.Value;
			inputX.Text = quaternion.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = quaternion.y.ToString(RuntimeInspectorUtils.numberFormat);
			inputZ.Text = quaternion.z.ToString(RuntimeInspectorUtils.numberFormat);
			inputW.Text = quaternion.w.ToString(RuntimeInspectorUtils.numberFormat);
		}
		else
		{
			Vector4 vector = (Vector4)base.Value;
			inputX.Text = vector.x.ToString(RuntimeInspectorUtils.numberFormat);
			inputY.Text = vector.y.ToString(RuntimeInspectorUtils.numberFormat);
			inputZ.Text = vector.z.ToString(RuntimeInspectorUtils.numberFormat);
			inputW.Text = vector.w.ToString(RuntimeInspectorUtils.numberFormat);
		}
	}

	private bool OnValueChanged(BoundInputField source, string input)
	{
		if (float.TryParse(input, NumberStyles.Float, RuntimeInspectorUtils.numberFormat, out var result))
		{
			if (isQuaternion)
			{
				Quaternion quaternion = (Quaternion)base.Value;
				if (source == inputX)
				{
					quaternion.x = result;
				}
				else if (source == inputY)
				{
					quaternion.y = result;
				}
				else if (source == inputZ)
				{
					quaternion.z = result;
				}
				else
				{
					quaternion.w = result;
				}
				base.Value = quaternion;
			}
			else
			{
				Vector4 vector = (Vector4)base.Value;
				if (source == inputX)
				{
					vector.x = result;
				}
				else if (source == inputY)
				{
					vector.y = result;
				}
				else if (source == inputZ)
				{
					vector.z = result;
				}
				else
				{
					vector.w = result;
				}
				base.Value = vector;
			}
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
		labelW.SetSkinText(base.Skin);
		inputX.Skin = base.Skin;
		inputY.Skin = base.Skin;
		inputZ.Skin = base.Skin;
		inputW.Skin = base.Skin;
		float num = (1f - base.Skin.LabelWidthPercentage) / 3f;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage + num, 0f);
		Vector2 anchorMax = new Vector2(base.Skin.LabelWidthPercentage + 2f * num, 1f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)inputX.transform).SetAnchorMinMaxInputField(labelX.rectTransform, new Vector2(anchorMin.x, 0.5f), anchorMax);
		((RectTransform)inputZ.transform).SetAnchorMinMaxInputField(labelZ.rectTransform, anchorMin, new Vector2(anchorMax.x, 0.5f));
		anchorMin.x += num;
		anchorMax.x = 1f;
		((RectTransform)inputY.transform).SetAnchorMinMaxInputField(labelY.rectTransform, new Vector2(anchorMin.x, 0.5f), anchorMax);
		((RectTransform)inputW.transform).SetAnchorMinMaxInputField(labelW.rectTransform, anchorMin, new Vector2(anchorMax.x, 0.5f));
	}

	public override void Refresh()
	{
		if (isQuaternion)
		{
			Quaternion quaternion = (Quaternion)base.Value;
			base.Refresh();
			Quaternion quaternion2 = (Quaternion)base.Value;
			if (quaternion2.x != quaternion.x)
			{
				inputX.Text = quaternion2.x.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (quaternion2.y != quaternion.y)
			{
				inputY.Text = quaternion2.y.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (quaternion2.z != quaternion.z)
			{
				inputZ.Text = quaternion2.z.ToString(RuntimeInspectorUtils.numberFormat);
			}
			if (quaternion2.w != quaternion.w)
			{
				inputW.Text = quaternion2.w.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
		else
		{
			Vector4 vector = (Vector4)base.Value;
			base.Refresh();
			Vector4 vector2 = (Vector4)base.Value;
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
			if (vector2.w != vector.w)
			{
				inputW.Text = vector2.w.ToString(RuntimeInspectorUtils.numberFormat);
			}
		}
	}
}
