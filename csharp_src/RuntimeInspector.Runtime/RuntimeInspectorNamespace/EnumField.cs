using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class EnumField : InspectorField
{
	[SerializeField]
	private Image background;

	[SerializeField]
	private Image dropdownArrow;

	[SerializeField]
	private RectTransform templateRoot;

	[SerializeField]
	private RectTransform templateContentTransform;

	[SerializeField]
	private RectTransform templateItemTransform;

	[SerializeField]
	private Image templateBackground;

	[SerializeField]
	private Image templateCheckmark;

	[SerializeField]
	private Text templateText;

	[SerializeField]
	private Dropdown input;

	private static readonly Dictionary<Type, List<string>> enumNames = new Dictionary<Type, List<string>>();

	private static readonly Dictionary<Type, List<object>> enumValues = new Dictionary<Type, List<object>>();

	private List<string> currEnumNames;

	private List<object> currEnumValues;

	public override void Initialize()
	{
		base.Initialize();
		input.onValueChanged.AddListener(OnValueChanged);
	}

	public override bool SupportsType(Type type)
	{
		return type.IsEnum;
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		if (!enumNames.TryGetValue(base.BoundVariableType, out currEnumNames) || !enumValues.TryGetValue(base.BoundVariableType, out currEnumValues))
		{
			string[] names = Enum.GetNames(base.BoundVariableType);
			Array values = Enum.GetValues(base.BoundVariableType);
			currEnumNames = new List<string>(names.Length);
			currEnumValues = new List<object>(names.Length);
			for (int i = 0; i < names.Length; i++)
			{
				currEnumNames.Add(names[i]);
				currEnumValues.Add(values.GetValue(i));
			}
			enumNames[base.BoundVariableType] = currEnumNames;
			enumValues[base.BoundVariableType] = currEnumValues;
		}
		input.ClearOptions();
		input.AddOptions(currEnumNames);
	}

	protected override void OnInspectorChanged()
	{
		base.OnInspectorChanged();
		OnTransformParentChanged();
	}

	private void OnTransformParentChanged()
	{
		if ((bool)base.Inspector && (bool)base.Skin)
		{
			Vector2 sizeDelta = templateRoot.sizeDelta;
			sizeDelta.y = (((RectTransform)base.Inspector.Canvas.transform).rect.height - (float)base.Skin.LineHeight) * 0.5f;
			templateRoot.sizeDelta = sizeDelta;
		}
	}

	private void OnValueChanged(int input)
	{
		base.Value = currEnumValues[input];
		base.Inspector.RefreshDelayed();
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		OnTransformParentChanged();
		Vector2 sizeDelta = templateContentTransform.sizeDelta;
		sizeDelta.y = (float)base.Skin.LineHeight + 6f;
		templateContentTransform.sizeDelta = sizeDelta;
		Vector2 sizeDelta2 = templateItemTransform.sizeDelta;
		sizeDelta2.y = base.Skin.LineHeight;
		templateItemTransform.sizeDelta = sizeDelta2;
		float num = (float)base.Skin.LineHeight * 0.66f;
		Vector2 sizeDelta3 = templateText.rectTransform.sizeDelta;
		sizeDelta3.x -= num - templateCheckmark.rectTransform.sizeDelta.x;
		templateText.rectTransform.sizeDelta = sizeDelta3;
		templateCheckmark.rectTransform.sizeDelta = new Vector2(num, num);
		Vector2 sizeDelta4 = input.captionText.rectTransform.sizeDelta;
		sizeDelta4.x -= num - dropdownArrow.rectTransform.sizeDelta.x;
		input.captionText.rectTransform.sizeDelta = sizeDelta4;
		dropdownArrow.rectTransform.sizeDelta = new Vector2(num, num);
		background.color = base.Skin.InputFieldNormalBackgroundColor;
		dropdownArrow.color = base.Skin.TextColor.Tint(0.1f);
		input.captionText.SetSkinInputFieldText(base.Skin);
		templateText.SetSkinInputFieldText(base.Skin);
		templateBackground.color = base.Skin.InputFieldNormalBackgroundColor.Tint(0.075f);
		templateCheckmark.color = base.Skin.ToggleCheckmarkColor;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)input.transform).anchorMin = anchorMin;
	}

	public override void Refresh()
	{
		base.Refresh();
		int num = currEnumValues.IndexOf(base.Value);
		if (num != -1)
		{
			input.value = num;
		}
	}
}
