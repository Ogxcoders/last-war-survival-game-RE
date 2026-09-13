using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ColorField : InspectorField
{
	[SerializeField]
	private RectTransform colorPickerArea;

	[SerializeField]
	private PointerEventListener inputColor;

	private Image colorImg;

	private bool isColor32;

	public override void Initialize()
	{
		base.Initialize();
		colorImg = inputColor.GetComponent<Image>();
		inputColor.PointerClick += ShowColorPicker;
	}

	public override bool SupportsType(Type type)
	{
		if (!(type == typeof(Color)))
		{
			return type == typeof(Color32);
		}
		return true;
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		isColor32 = base.BoundVariableType == typeof(Color32);
	}

	private void ShowColorPicker(PointerEventData eventData)
	{
		Color initialColor = (isColor32 ? ((Color)(Color32)base.Value) : ((Color)base.Value));
		ColorPicker.Instance.Skin = base.Inspector.Skin;
		ColorPicker.Instance.Show(OnColorChanged, null, initialColor, base.Inspector.Canvas);
	}

	private void OnColorChanged(Color32 color)
	{
		colorImg.color = color;
		if (isColor32)
		{
			base.Value = color;
		}
		else
		{
			base.Value = (Color)color;
		}
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		Vector2 anchorMin = new Vector2(base.Skin.LabelWidthPercentage, 0f);
		variableNameMask.rectTransform.anchorMin = anchorMin;
		colorPickerArea.anchorMin = anchorMin;
	}

	public override void Refresh()
	{
		base.Refresh();
		if (isColor32)
		{
			colorImg.color = (Color32)base.Value;
		}
		else
		{
			colorImg.color = (Color)base.Value;
		}
	}
}
