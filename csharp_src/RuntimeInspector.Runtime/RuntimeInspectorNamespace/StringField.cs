using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class StringField : InspectorField
{
	public enum Mode
	{
		OnValueChange,
		OnSubmit
	}

	[SerializeField]
	private BoundInputField input;

	private Mode m_setterMode;

	private int lineCount = 1;

	public Mode SetterMode
	{
		get
		{
			return m_setterMode;
		}
		set
		{
			m_setterMode = value;
			input.CacheTextOnValueChange = m_setterMode == Mode.OnValueChange;
		}
	}

	protected override float HeightMultiplier => lineCount;

	public override void Initialize()
	{
		base.Initialize();
		input.Initialize();
		BoundInputField boundInputField = input;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField2 = input;
		boundInputField2.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		input.DefaultEmptyValue = string.Empty;
	}

	public override bool SupportsType(Type type)
	{
		return type == typeof(string);
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		int num = lineCount;
		if (variable == null)
		{
			lineCount = 1;
		}
		else
		{
			MultilineAttribute attribute = variable.GetAttribute<MultilineAttribute>();
			if (attribute != null)
			{
				lineCount = Mathf.Max(1, attribute.lines);
			}
			else if (variable.HasAttribute<TextAreaAttribute>())
			{
				lineCount = 3;
			}
			else
			{
				lineCount = 1;
			}
		}
		if (num != lineCount)
		{
			input.BackingField.lineType = ((lineCount > 1) ? InputField.LineType.MultiLineNewline : InputField.LineType.SingleLine);
			input.BackingField.textComponent.alignment = ((lineCount <= 1) ? TextAnchor.MiddleLeft : TextAnchor.UpperLeft);
			OnSkinChanged();
		}
	}

	protected override void OnUnbound()
	{
		base.OnUnbound();
		SetterMode = Mode.OnValueChange;
	}

	private bool OnValueChanged(BoundInputField source, string input)
	{
		if (m_setterMode == Mode.OnValueChange)
		{
			base.Value = input;
		}
		return true;
	}

	private bool OnValueSubmitted(BoundInputField source, string input)
	{
		if (m_setterMode == Mode.OnSubmit)
		{
			base.Value = input;
		}
		base.Inspector.RefreshDelayed();
		return true;
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		input.Skin = base.Skin;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)input.transform).anchorMin = anchorMin;
	}

	public override void Refresh()
	{
		base.Refresh();
		if (base.Value == null)
		{
			input.Text = string.Empty;
		}
		else
		{
			input.Text = (string)base.Value;
		}
	}
}
