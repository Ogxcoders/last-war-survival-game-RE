using System;
using System.Text;
using GameFramework.Localization;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class TextMeshProEx : TextMeshPro
{
	private enum _HorizontalAlignmentOptions
	{
		Left = 1,
		Center = 2,
		Right = 4,
		Justified = 8,
		Flush = 0x10,
		Geometry = 0x20
	}

	private enum _VerticalAlignmentOptions
	{
		Top = 0x100,
		Middle = 0x200,
		Bottom = 0x400,
		Baseline = 0x800,
		Geometry = 0x1000,
		Capline = 0x2000
	}

	private ForceArabicText _arabicComponent;

	private bool hasForceArabic;

	private bool _hasInitArabicComponent;

	private bool _hasArabicComponent;

	public Action<PointerEventData> onPointerClick;

	private static FastStringBuilder RTLFixedText = new FastStringBuilder(2048);

	private bool hasArabicComponent
	{
		get
		{
			if (!_hasInitArabicComponent)
			{
				ForceArabicText forceArabicText = arabicComponent;
				_hasArabicComponent = forceArabicText != null;
			}
			return _hasArabicComponent;
		}
	}

	private ForceArabicText arabicComponent
	{
		get
		{
			if (!_hasInitArabicComponent)
			{
				TryGetComponent<ForceArabicText>(out _arabicComponent);
			}
			return _arabicComponent;
		}
	}

	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			string text = value;
			if (Application.isPlaying)
			{
				if (GameEntry.Localization != null && materialForRendering != null)
				{
					materialForRendering.shader.maximumLOD = ((GameEntry.Localization.Language == Language.Arabic) ? 300 : 100);
				}
				if (HasArabic(text, out var _))
				{
					LocalizationManager localization = GameEntry.Localization;
					if (localization == null || localization.Language != Language.Arabic)
					{
						base.isRightToLeftText = false;
						for (int i = 0; i < text.Length; i++)
						{
							char c = text[i];
							if (isArabic(c))
							{
								int num = i;
								int num2 = i;
								StringBuilder stringBuilder = new StringBuilder();
								bool flag = i + 1 < text.Length && isArabic(text[i + 1]);
								while (i < text.Length && (isArabic(text[i]) || (text[i] == ' ' && flag)))
								{
									c = text[i];
									stringBuilder.Append(c);
									i++;
									num2 = i;
								}
								string fixedText = GetFixedText(stringBuilder.ToString(), isReverse: false);
								text = text.Remove(num, num2 - num);
								text = text.Insert(num, fixedText);
								i = num + fixedText.Length;
							}
						}
						base.text = text;
						return;
					}
					base.isRightToLeftText = true;
					base.text = GetFixedText(text);
					bool flag2 = MirrorVersionConfig.IsMirrorVersionOpen && hasForceArabic && arabicComponent.isControlAlignByTextInAutoMirror;
					ArabicHorizonLayout component;
					bool num3 = base.transform.parent.TryGetComponent<ArabicHorizonLayout>(out component);
					bool flag3 = !MirrorVersionConfig.IsMirrorVersionOpen && hasForceArabic && arabicComponent.isControlNonAutoMirrorAlign;
					bool flag4 = MirrorVersionConfig.IsMirrorVersionOpen || flag3;
					if (!num3 && (!flag4 || flag2))
					{
						SetRtlAlign(TextAlignmentOptions.Right, auto: true);
					}
					if (hasForceArabic)
					{
						Material arabicTMProFontMaterial = arabicComponent.ArabicTMProFontMaterial;
						if (arabicTMProFontMaterial != null)
						{
							SetNewMaterial(arabicTMProFontMaterial);
						}
						if (arabicComponent.IsArabicDisableBoldFont)
						{
							base.fontStyle &= ~FontStyles.Bold;
						}
					}
				}
				else
				{
					base.isRightToLeftText = false;
					base.text = text;
				}
			}
			else
			{
				base.text = text;
			}
		}
	}

	public Color32 color32
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
		}
	}

	public void SetText(string newText)
	{
		text = newText;
	}

	internal void SetRealText(string newText)
	{
		base.text = newText;
	}

	public static TextAlignmentOptions ConvertAlignFormat(TextAnchor anchor, bool alignByGeometry)
	{
		_HorizontalAlignmentOptions horizontalAlignmentOptions;
		switch (anchor)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.MiddleLeft:
		case TextAnchor.LowerLeft:
			horizontalAlignmentOptions = _HorizontalAlignmentOptions.Left;
			break;
		case TextAnchor.UpperCenter:
		case TextAnchor.MiddleCenter:
		case TextAnchor.LowerCenter:
			horizontalAlignmentOptions = _HorizontalAlignmentOptions.Center;
			break;
		case TextAnchor.UpperRight:
		case TextAnchor.MiddleRight:
		case TextAnchor.LowerRight:
			horizontalAlignmentOptions = _HorizontalAlignmentOptions.Right;
			break;
		default:
			horizontalAlignmentOptions = _HorizontalAlignmentOptions.Center;
			break;
		}
		_VerticalAlignmentOptions verticalAlignmentOptions;
		switch (anchor)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.UpperCenter:
		case TextAnchor.UpperRight:
			verticalAlignmentOptions = (alignByGeometry ? _VerticalAlignmentOptions.Geometry : _VerticalAlignmentOptions.Top);
			break;
		case TextAnchor.MiddleLeft:
		case TextAnchor.MiddleCenter:
		case TextAnchor.MiddleRight:
			verticalAlignmentOptions = (alignByGeometry ? _VerticalAlignmentOptions.Geometry : _VerticalAlignmentOptions.Middle);
			break;
		case TextAnchor.LowerLeft:
		case TextAnchor.LowerCenter:
		case TextAnchor.LowerRight:
			verticalAlignmentOptions = (alignByGeometry ? _VerticalAlignmentOptions.Geometry : _VerticalAlignmentOptions.Bottom);
			break;
		default:
			verticalAlignmentOptions = _VerticalAlignmentOptions.Middle;
			break;
		}
		return (TextAlignmentOptions)((int)horizontalAlignmentOptions | (int)verticalAlignmentOptions);
	}

	public bool HasArabic(string str, out int arabicCount)
	{
		arabicCount = 0;
		if (string.IsNullOrEmpty(str))
		{
			return false;
		}
		LocalizationManager localization = GameEntry.Localization;
		bool flag = localization != null && localization.Language == Language.Arabic;
		hasForceArabic = hasArabicComponent;
		if (hasForceArabic && ((!MirrorVersionConfig.IsMirrorVersionOpen && arabicComponent.isControlNonAutoMirrorAlign) || (MirrorVersionConfig.IsMirrorVersionOpen && arabicComponent.isControlAutoMirrorAlign)))
		{
			bool flag2 = MirrorVersionConfig.IsMirrorVersionOpen && arabicComponent.isControlAutoMirrorAlign;
			SetRtlAlign(flag ? ConvertAlignFormat(flag2 ? arabicComponent.ArabicLangForceAlign_AutoMirror : arabicComponent.ArabicLangForceAlign, alignByGeometry: true) : ConvertAlignFormat(flag2 ? arabicComponent.NonArabicLangForceAlign_AutoMirror : arabicComponent.NonArabicLangForceAlign, alignByGeometry: true), auto: false);
		}
		return CheckArabicByChar(str, out arabicCount);
	}

	public static bool CheckArabicByChar(string str, out int arabicCount)
	{
		arabicCount = 0;
		int length = str.Length;
		for (int i = 0; i < length; i++)
		{
			if (isArabic(str[i]))
			{
				arabicCount++;
			}
		}
		return (float)arabicCount * 1f / (float)length >= 0.15f;
	}

	private void SetRtlAlign(TextAlignmentOptions targetAlign, bool auto)
	{
		if (!auto)
		{
			base.alignment = targetAlign;
			return;
		}
		switch (base.alignment)
		{
		case TextAlignmentOptions.BottomLeft:
			base.alignment = TextAlignmentOptions.BottomRight;
			break;
		case TextAlignmentOptions.MidlineLeft:
			base.alignment = TextAlignmentOptions.MidlineRight;
			break;
		case TextAlignmentOptions.TopLeft:
			base.alignment = TextAlignmentOptions.TopRight;
			break;
		case TextAlignmentOptions.BaselineLeft:
			base.alignment = TextAlignmentOptions.BaselineRight;
			break;
		case TextAlignmentOptions.CaplineLeft:
			base.alignment = TextAlignmentOptions.CaplineRight;
			break;
		case TextAlignmentOptions.Left:
			base.alignment = TextAlignmentOptions.Right;
			break;
		}
	}

	public static bool isArabic(char c)
	{
		if (c >= '\u0600' && c <= 'ۿ')
		{
			return true;
		}
		if (c >= 'ݐ' && c <= 'ݿ')
		{
			return true;
		}
		if (c >= 'ﭐ' && c <= 'ﰿ')
		{
			return true;
		}
		if (c >= 'ﹰ' && c <= 'ﻼ')
		{
			return true;
		}
		return false;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onPointerClick?.Invoke(eventData);
		Transform parent = base.transform.parent;
		while (parent != null && !ExecuteEvents.Execute(parent.gameObject, eventData, ExecuteEvents.pointerClickHandler))
		{
			parent = parent.parent;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Transform parent = base.transform.parent;
		while (parent != null && !ExecuteEvents.Execute(parent.gameObject, eventData, ExecuteEvents.pointerDownHandler))
		{
			parent = parent.parent;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Transform parent = base.transform.parent;
		while (parent != null && !ExecuteEvents.Execute(parent.gameObject, eventData, ExecuteEvents.pointerUpHandler))
		{
			parent = parent.parent;
		}
	}

	private string GetFixedText(string input, bool isReverse = true)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		RTLFixedText.Clear();
		RTLSupport.FixRTL(input, RTLFixedText, farsi: false, base.richText, preserveNumbers: true);
		if (isReverse)
		{
			RTLFixedText.Reverse();
		}
		return RTLFixedText.ToString();
	}

	public void SetNewMaterial(Material newMat)
	{
		if (newMat == null)
		{
			fontSharedMaterial = base.font.material;
		}
		else
		{
			fontSharedMaterial = newMat;
		}
	}

	public float GetWidth()
	{
		return base.rectTransform.rect.width;
	}
}
