using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class NumberField : InspectorField
{
	private static readonly HashSet<Type> supportedTypes = new HashSet<Type>
	{
		typeof(int),
		typeof(uint),
		typeof(long),
		typeof(ulong),
		typeof(byte),
		typeof(sbyte),
		typeof(short),
		typeof(ushort),
		typeof(char),
		typeof(float),
		typeof(double),
		typeof(decimal)
	};

	[SerializeField]
	protected BoundInputField input;

	protected INumberHandler numberHandler;

	public override void Initialize()
	{
		base.Initialize();
		input.Initialize();
		BoundInputField boundInputField = input;
		boundInputField.OnValueChanged = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField.OnValueChanged, new BoundInputField.OnValueChangedDelegate(OnValueChanged));
		BoundInputField boundInputField2 = input;
		boundInputField2.OnValueSubmitted = (BoundInputField.OnValueChangedDelegate)Delegate.Combine(boundInputField2.OnValueSubmitted, new BoundInputField.OnValueChangedDelegate(OnValueSubmitted));
		input.DefaultEmptyValue = "0";
	}

	public override bool SupportsType(Type type)
	{
		return supportedTypes.Contains(type);
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		if (base.BoundVariableType == typeof(float) || base.BoundVariableType == typeof(double) || base.BoundVariableType == typeof(decimal))
		{
			input.BackingField.contentType = InputField.ContentType.DecimalNumber;
		}
		else
		{
			input.BackingField.contentType = InputField.ContentType.IntegerNumber;
		}
		numberHandler = NumberHandlers.Get(base.BoundVariableType);
		input.Text = numberHandler.ToString(base.Value);
	}

	protected virtual bool OnValueChanged(BoundInputField source, string input)
	{
		if (numberHandler.TryParse(input, out var value))
		{
			base.Value = value;
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
		input.Skin = base.Skin;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)input.transform).anchorMin = anchorMin;
	}

	public override void Refresh()
	{
		object value = base.Value;
		base.Refresh();
		if (!numberHandler.ValuesAreEqual(base.Value, value))
		{
			input.Text = numberHandler.ToString(base.Value);
		}
	}
}
