using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class BoolField : InspectorField
{
	[SerializeField]
	private Image toggleBackground;

	[SerializeField]
	private Toggle input;

	public override void Initialize()
	{
		base.Initialize();
		input.onValueChanged.AddListener(OnValueChanged);
	}

	public override bool SupportsType(Type type)
	{
		return type == typeof(bool);
	}

	private void OnValueChanged(bool input)
	{
		base.Value = input;
		base.Inspector.RefreshDelayed();
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		toggleBackground.color = base.Skin.InputFieldNormalBackgroundColor;
		input.graphic.color = base.Skin.ToggleCheckmarkColor;
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		((RectTransform)input.transform).anchorMin = anchorMin;
	}

	public override void Refresh()
	{
		base.Refresh();
		input.isOn = (bool)base.Value;
	}
}
