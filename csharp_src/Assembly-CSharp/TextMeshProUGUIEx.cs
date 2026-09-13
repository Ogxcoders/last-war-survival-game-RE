using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using GameFramework.Localization;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TextMeshProUGUIEx : TextMeshProUGUI, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
	public enum HorizontalAlignmentOptions
	{
		Left = 1,
		Center = 2,
		Right = 4,
		Justified = 8,
		Flush = 0x10,
		Geometry = 0x20
	}

	public enum VerticalAlignmentOptions
	{
		Top = 0x100,
		Middle = 0x200,
		Bottom = 0x400,
		Baseline = 0x800,
		Geometry = 0x1000,
		Capline = 0x2000
	}

	private readonly Language[] AutoSizeLanguage = new Language[1] { Language.Russian };

	private bool arabicFontSizeProcessFlag;

	private ForceArabicText _arabicComponent;

	private bool _hasInitArabicComponent;

	private bool _hasArabicComponent;

	private bool hasForceArabic;

	private bool _hasRtlAlignSet;

	private TextAlignmentOptions _rtlAlignOrigin = TextAlignmentOptions.Left;

	private string originalText = "";

	private bool ignoreAutoSizeByLanguage;

	public Action<PointerEventData> onPointerClick;

	public Action<int> onCharacterPointerClick;

	private static FastStringBuilder RTLFixedText = new FastStringBuilder(2048);

	private StringBuilder _stringBuilder;

	private static readonly string Diacritics = "[\\u064B-\\u0652\\u0670]*";

	private static readonly string BoundaryStart = "(^|[\\s\\p{P}\\p{S}])";

	private static readonly string BoundaryEnd = "(?=$|[\\s\\p{P}\\p{S}])";

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
			originalText = value;
			if (Application.isPlaying)
			{
				if (GameEntry.Localization != null && materialForRendering != null)
				{
					materialForRendering.shader.maximumLOD = ((GameEntry.Localization.Language == Language.Arabic) ? 300 : 100);
				}
				SetSpanishTitleLineSpacing();
				SetAutoSizeByLanguage();
				int arabicCount;
				bool flag = HasArabic(text, out arabicCount);
				hasArabicChar = false;
				if (GameDefines.isArabicTMPFix)
				{
					hasArabicChar = flag;
				}
				if (flag)
				{
					bool flag2 = hasForceArabic && arabicComponent.isProcessArabicInNonArabicLang && (float)arabicCount * 1f / (float)value.Length >= 0.15f;
					isForceArabicLan = false;
					if (GameDefines.isArabicTMPFix)
					{
						isForceArabicLan = flag2;
					}
					isArabicLanType = false;
					LocalizationManager localization = GameEntry.Localization;
					if ((localization == null || localization.Language != Language.Arabic) && !flag2)
					{
						base.isRightToLeftText = false;
						if (!GameDefines.isArabicTMPFix)
						{
							for (int i = 0; i < text.Length; i++)
							{
								char c = text[i];
								if (isArabic(c))
								{
									int num = i;
									int num2 = i;
									StringBuilder stringBuilder = StringBuilder;
									bool flag3 = i + 1 < text.Length && isArabic(text[i + 1]);
									while (i < text.Length && (isArabic(text[i]) || (text[i] == ' ' && flag3)))
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
						}
						base.text = text;
					}
					else
					{
						if (GameDefines.isArabicTMPFix)
						{
							isArabicLanType = true;
						}
						if (text.Contains(" "))
						{
							text = ArabicPreprocessor.FixSpaces(text);
						}
						if (text.Contains("\t"))
						{
							text = ArabicPreprocessor.FixTabForRtl(text);
						}
						base.isRightToLeftText = true;
						if (GameDefines.isArabicTMPFix)
						{
							base.text = text;
						}
						else
						{
							base.text = GetFixedText(text);
						}
						bool flag4 = MirrorVersionConfig.IsMirrorVersionOpen && hasForceArabic && arabicComponent.isControlAlignByTextInAutoMirror;
						ArabicHorizonLayout component;
						bool num3 = base.transform.parent.TryGetComponent<ArabicHorizonLayout>(out component);
						bool flag5 = !MirrorVersionConfig.IsMirrorVersionOpen && hasForceArabic && arabicComponent.isControlNonAutoMirrorAlign;
						bool flag6 = MirrorVersionConfig.IsMirrorVersionOpen || flag5;
						if (!num3 && (!flag6 || flag4))
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
						if (!arabicFontSizeProcessFlag)
						{
							if (base.enableAutoSizing)
							{
								base.fontSizeMax += 2f;
							}
							else
							{
								base.fontSize += 2f;
							}
							arabicFontSizeProcessFlag = true;
						}
					}
				}
				else
				{
					if (_hasRtlAlignSet)
					{
						_hasRtlAlignSet = false;
						base.alignment = _rtlAlignOrigin;
					}
					base.isRightToLeftText = false;
					base.text = text;
				}
			}
			else
			{
				base.text = text;
			}
			TMPNumberAutoLineAdjust component2 = GetComponent<TMPNumberAutoLineAdjust>();
			if (component2 != null && component2.enabled)
			{
				component2.FixDigitsLayoutNextFrame();
			}
		}
	}

	private StringBuilder StringBuilder
	{
		get
		{
			if (_stringBuilder == null)
			{
				_stringBuilder = new StringBuilder();
			}
			else
			{
				_stringBuilder.Clear();
			}
			return _stringBuilder;
		}
		set
		{
			_stringBuilder = value;
		}
	}

	public void SetFontFillingUint(int uncode)
	{
		m_fontFillingUint = uncode;
	}

	public string GetOriginalText()
	{
		return originalText;
	}

	private void SetSpanishTitleLineSpacing()
	{
		if (GameEntry.Localization != null && GameEntry.Localization.Language == Language.Spanish)
		{
			TMP_FontAsset tMP_FontAsset = base.font;
			if (tMP_FontAsset != null && tMP_FontAsset.name.Equals("Title", StringComparison.OrdinalIgnoreCase))
			{
				base.lineSpacing = -30f;
			}
		}
	}

	private void SetAutoSizeByLanguage()
	{
		if (ignoreAutoSizeByLanguage)
		{
			return;
		}
		if (TryGetComponent<IgnoreAutoSizeByLanguageTag>(out var _))
		{
			if (base.enableAutoSizing)
			{
				base.enableAutoSizing = false;
				base.fontSize = base.fontSizeMax;
			}
			return;
		}
		Language language = GameEntry.Localization.Language;
		if (!AutoSizeLanguage.Contains(language))
		{
			return;
		}
		bool flag = base.rectTransform.rect.height == 0f;
		ContentSizeFitter component2;
		bool flag2 = TryGetComponent<ContentSizeFitter>(out component2) && component2.horizontalFit == ContentSizeFitter.FitMode.PreferredSize;
		if (base.overflowMode != TextOverflowModes.Ellipsis && !flag && !flag2)
		{
			base.fontSizeMin = 0f;
			if (!base.enableAutoSizing && base.enableWordWrapping)
			{
				base.enableAutoSizing = true;
				base.fontSizeMax = base.fontSize;
			}
		}
	}

	public void ChangeAutoSetting(bool value)
	{
		ignoreAutoSizeByLanguage = value;
	}

	public static TextAlignmentOptions ConvertAlignFormat(TextAnchor anchor, bool alignByGeometry)
	{
		HorizontalAlignmentOptions horizontalAlignmentOptions;
		switch (anchor)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.MiddleLeft:
		case TextAnchor.LowerLeft:
			horizontalAlignmentOptions = HorizontalAlignmentOptions.Left;
			break;
		case TextAnchor.UpperCenter:
		case TextAnchor.MiddleCenter:
		case TextAnchor.LowerCenter:
			horizontalAlignmentOptions = HorizontalAlignmentOptions.Center;
			break;
		case TextAnchor.UpperRight:
		case TextAnchor.MiddleRight:
		case TextAnchor.LowerRight:
			horizontalAlignmentOptions = HorizontalAlignmentOptions.Right;
			break;
		default:
			horizontalAlignmentOptions = HorizontalAlignmentOptions.Center;
			break;
		}
		VerticalAlignmentOptions verticalAlignmentOptions;
		switch (anchor)
		{
		case TextAnchor.UpperLeft:
		case TextAnchor.UpperCenter:
		case TextAnchor.UpperRight:
			verticalAlignmentOptions = (alignByGeometry ? VerticalAlignmentOptions.Geometry : VerticalAlignmentOptions.Top);
			break;
		case TextAnchor.MiddleLeft:
		case TextAnchor.MiddleCenter:
		case TextAnchor.MiddleRight:
			verticalAlignmentOptions = (alignByGeometry ? VerticalAlignmentOptions.Geometry : VerticalAlignmentOptions.Middle);
			break;
		case TextAnchor.LowerLeft:
		case TextAnchor.LowerCenter:
		case TextAnchor.LowerRight:
			verticalAlignmentOptions = (alignByGeometry ? VerticalAlignmentOptions.Geometry : VerticalAlignmentOptions.Bottom);
			break;
		default:
			verticalAlignmentOptions = VerticalAlignmentOptions.Middle;
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
		return arabicCount > 0;
	}

	private void SetRtlAlign(TextAlignmentOptions targetAlign, bool auto)
	{
		if (!_hasRtlAlignSet)
		{
			_rtlAlignOrigin = base.alignment;
		}
		_hasRtlAlignSet = true;
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
		OnCharacterPointClick(eventData);
		onPointerClick?.Invoke(eventData);
		Transform parent = base.transform.parent;
		while (parent != null && !ExecuteEvents.Execute(parent.gameObject, eventData, ExecuteEvents.pointerClickHandler))
		{
			parent = parent.parent;
		}
	}

	public bool ThroughPointerClickHandler(GameObject gameObject, PointerEventData eventData)
	{
		return ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerClickHandler);
	}

	public void OnCharacterPointClick(PointerEventData eventData)
	{
		if (onCharacterPointerClick != null)
		{
			int num = TMP_TextUtilities.FindIntersectingCharacter(this, eventData.position, eventData.pressEventCamera, visibleOnly: true);
			if (num != -1)
			{
				onCharacterPointerClick?.Invoke(num);
			}
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
		input = FixAllah(input);
		RTLFixedText.Clear();
		RTLSupport.FixRTL(input, RTLFixedText, farsi: false, base.richText, preserveNumbers: true);
		if (isReverse)
		{
			RTLFixedText.Reverse();
		}
		return RTLFixedText.ToString();
	}

	public static string FixAllah(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		input = Regex.Replace(input, BoundaryStart + "ا" + Diacritics + "ل" + Diacritics + "ل" + Diacritics + "ه" + Diacritics + BoundaryEnd, (Match match) => match.Groups[1].Value + char.ConvertFromUtf32(65010));
		input = Regex.Replace(input, BoundaryStart + "ل" + Diacritics + "ل" + Diacritics + "ه" + Diacritics + BoundaryEnd, (Match match) => match.Groups[1].Value + char.ConvertFromUtf32(57600));
		input = Regex.Replace(input, BoundaryStart + "ب" + Diacritics + "ا" + Diacritics + "ل" + Diacritics + "ل" + Diacritics + "ه" + Diacritics + BoundaryEnd, (Match match) => match.Groups[1].Value + char.ConvertFromUtf32(57601));
		return input;
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

	public void SetTextColorRange(List<int> indexList, Color color)
	{
		ForceMeshUpdate(ignoreActiveState: true);
		int characterCount = base.textInfo.characterCount;
		if (characterCount == 0)
		{
			return;
		}
		for (int i = 0; i < indexList.Count; i += 2)
		{
			int num = Mathf.Clamp(indexList[i], 0, characterCount - 1);
			int num2 = Mathf.Clamp(indexList[i + 1], 0, characterCount - 1);
			for (int j = num; j <= num2; j++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = base.textInfo.characterInfo[j];
				if (tMP_CharacterInfo.isVisible)
				{
					int vertexIndex = tMP_CharacterInfo.vertexIndex;
					int materialReferenceIndex = tMP_CharacterInfo.materialReferenceIndex;
					Color32[] colors = base.textInfo.meshInfo[materialReferenceIndex].colors32;
					for (int k = 0; k < 4; k++)
					{
						colors[vertexIndex + k] = color;
					}
				}
			}
		}
		UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
	}

	public string FixSpaces(string val)
	{
		return ArabicPreprocessor.FixSpaces(val);
	}
}
