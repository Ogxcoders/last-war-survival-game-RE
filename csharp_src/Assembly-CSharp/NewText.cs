using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using GameFramework.Localization;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewText : Text
{
	public bool _useTextWithEllipsis = true;

	public bool _useTextBestFit;

	[SerializeField]
	private bool _useNoLineSpace;

	private const string no_breaking_space = "\u00a0";

	private static FastStringBuilder RTLFixedText = new FastStringBuilder(2048);

	private string[] Holder = new string[5];

	private ForceArabicText arabicComponent;

	private bool hasForceArabic;

	private static TextGenerator _generator = new TextGenerator();

	private StringBuilder _stringBuilder = new StringBuilder(50);

	private bool _lock;

	private string _compareValue = "";

	private bool _needNewString;

	private readonly UIVertex[] _tmpVerts = new UIVertex[4];

	private readonly string strRegex = "(\\！|\\？|\\，|\\。|\\《|\\》|\\）|\\：|\\“|\\‘|\\、|\\；|\\+|\\-)";

	private readonly string richTextRight = ">";

	private readonly string richTextLeft = "<";

	private IList<UILineInfo> _explainTextLine;

	public string oringinalText { get; set; }

	public override string text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(m_Text))
				{
					m_Text = "";
					SetVerticesDirty();
				}
				return;
			}
			if (_useNoLineSpace && value.Contains(" "))
			{
				value = value.Replace(" ", "\u00a0");
			}
			if (_useTextWithEllipsis)
			{
				value = GetTextWithEllipsis(value);
			}
			if (IsRtl(value, out var _))
			{
				if (!UIRunTimeConfig.IsArabic)
				{
					string text = value;
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (TextMeshProUGUIEx.isArabic(c))
						{
							int num = i;
							int num2 = i;
							StringBuilder stringBuilder = new StringBuilder();
							bool flag = i + 1 < text.Length && TextMeshProUGUIEx.isArabic(text[i + 1]);
							while (i < text.Length && (TextMeshProUGUIEx.isArabic(text[i]) || (text[i] == ' ' && flag)))
							{
								c = text[i];
								stringBuilder.Append(c);
								i++;
								num2 = i;
							}
							string text2 = ArabicFixer.FixText(stringBuilder.ToString(), (string str) => PopulateWithErrors(this, str));
							text = text.Remove(num, num2 - num);
							text = text.Insert(num, text2);
							i = num + text2.Length;
						}
					}
					value = text;
				}
				else
				{
					oringinalText = value;
					value = ArabicFixer.FixText(value, (string str) => PopulateWithErrors(this, str));
					if (IsHasVerPreferWidth() || IsHasHorPreferWidth() || IsHasVerOrHorPreferWidthInParent())
					{
						GameEntry.Timer.RegisterTimer(0.5f, delegate
						{
							if ((bool)this)
							{
								value = ArabicFixer.FixText(oringinalText, (string str) => PopulateWithErrors(this, str));
								m_Text = value;
								SetVerticesDirty();
								SetLayoutDirty();
							}
						});
					}
					if (!base.transform.parent.TryGetComponent<ArabicHorizonLayout>(out var _) && !hasForceArabic)
					{
						SetRtlAlign(TextAnchor.LowerRight, auto: true);
					}
				}
			}
			if (m_Text != value)
			{
				m_Text = value;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	public int VisibleLines { get; private set; }

	protected override void OnEnable()
	{
		base.OnEnable();
		if (string.IsNullOrEmpty(oringinalText) || (!IsHasVerPreferWidth() && !IsHasHorPreferWidth() && !IsHasVerOrHorPreferWidthInParent()))
		{
			return;
		}
		GameEntry.Timer.RegisterTimer(0.5f, delegate
		{
			if ((bool)this)
			{
				string text = ArabicFixer.FixText(oringinalText, (string str) => PopulateWithErrors(this, str));
				m_Text = text;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		});
	}

	private string GetTextWithEllipsis(string value)
	{
		_stringBuilder.Clear();
		_needNewString = false;
		_compareValue = "";
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i] == '<')
			{
				_lock = true;
				_needNewString = true;
			}
			else if (value[i] == '>')
			{
				_lock = false;
			}
			else if (!_lock)
			{
				_stringBuilder.Append(value[i]);
			}
		}
		if (_needNewString)
		{
			_compareValue = _stringBuilder.ToString();
		}
		else
		{
			_compareValue = value;
		}
		TextGenerationSettings generationSettings = GetGenerationSettings(base.rectTransform.rect.size);
		_generator.Populate(_compareValue, generationSettings);
		int characterCountVisible = _generator.characterCountVisible;
		string result = value;
		if (characterCountVisible > 0 && _compareValue.Length > characterCountVisible)
		{
			_lock = false;
			_stringBuilder.Clear();
			int length = value.Length;
			int num = 0;
			for (int j = 0; j <= length; j++)
			{
				if (value[j] == '<')
				{
					_lock = true;
				}
				else if (value[j] == '>')
				{
					_lock = false;
				}
				else if (!_lock)
				{
					num++;
					if (num > characterCountVisible)
					{
						break;
					}
					_stringBuilder.Append(value[j]);
				}
			}
			_stringBuilder.Append("...");
			result = _stringBuilder.ToString();
		}
		return result;
	}

	private void _UseFitSettings()
	{
		TextGenerationSettings generationSettings = GetGenerationSettings(base.rectTransform.rect.size);
		generationSettings.resizeTextForBestFit = false;
		if (!base.resizeTextForBestFit)
		{
			base.cachedTextGenerator.PopulateWithErrors(text, generationSettings, base.gameObject);
			return;
		}
		int num = base.resizeTextMinSize;
		int length = text.Length;
		int num2 = base.resizeTextMaxSize;
		while (num2 >= num)
		{
			generationSettings.fontSize = num2;
			base.cachedTextGenerator.PopulateWithErrors(text, generationSettings, base.gameObject);
			if (base.cachedTextGenerator.characterCountVisible != length)
			{
				num2--;
				continue;
			}
			break;
		}
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (null == base.font)
		{
			return;
		}
		m_DisableFontTextureRebuiltCallback = true;
		if (!_useTextBestFit)
		{
			base.OnPopulateMesh(toFill);
			if (IsActive())
			{
				StartCoroutine(ClearUpExplainMode());
			}
			return;
		}
		_UseFitSettings();
		IList<UIVertex> verts = base.cachedTextGenerator.verts;
		float num = 1f / base.pixelsPerUnit;
		int count = verts.Count;
		if (count <= 0)
		{
			toFill.Clear();
			return;
		}
		Vector2 vector = new Vector2(verts[0].position.x, verts[0].position.y) * num;
		vector = PixelAdjustPoint(vector) - vector;
		toFill.Clear();
		if (vector != Vector2.zero)
		{
			for (int i = 0; i < count; i++)
			{
				int num2 = i & 3;
				_tmpVerts[num2] = verts[i];
				_tmpVerts[num2].position *= num;
				_tmpVerts[num2].position.x += vector.x;
				_tmpVerts[num2].position.y += vector.y;
				if (num2 == 3)
				{
					toFill.AddUIVertexQuad(_tmpVerts);
				}
			}
		}
		else
		{
			for (int j = 0; j < count; j++)
			{
				int num3 = j & 3;
				_tmpVerts[num3] = verts[j];
				_tmpVerts[num3].position *= num;
				if (num3 == 3)
				{
					toFill.AddUIVertexQuad(_tmpVerts);
				}
			}
		}
		m_DisableFontTextureRebuiltCallback = false;
		VisibleLines = base.cachedTextGenerator.lineCount;
		if (IsActive())
		{
			StartCoroutine(ClearUpExplainMode());
		}
	}

	private IEnumerator ClearUpExplainMode()
	{
		yield return new WaitForSeconds(0.001f);
		_explainTextLine = base.cachedTextGenerator.lines;
		int num = 0;
		bool flag = false;
		_stringBuilder.Clear();
		_stringBuilder.Append(this.text);
		int i = 1;
		for (int count = _explainTextLine.Count; i < count; i++)
		{
			try
			{
				int startCharIdx = _explainTextLine[i].startCharIdx;
				if (startCharIdx >= this.text.Length)
				{
					num = startCharIdx;
				}
				else if (Regex.IsMatch(this.text[startCharIdx].ToString(), strRegex))
				{
					int num2 = startCharIdx - 1;
					bool flag2 = false;
					for (int num3 = startCharIdx - 1; num3 > 0; num3--)
					{
						string text = this.text[num3].ToString();
						bool flag3 = Regex.IsMatch(text, strRegex);
						if (flag2)
						{
							num2--;
							if (text == richTextLeft)
							{
								flag2 = false;
							}
						}
						else if (flag3)
						{
							num2--;
						}
						else
						{
							if (!(text == richTextRight))
							{
								break;
							}
							flag2 = true;
							num2--;
						}
					}
					string text2 = this.text[num2].ToString();
					if (text2 == "\n" || text2 == " ")
					{
						num = startCharIdx;
						continue;
					}
					if (num2 > num)
					{
						_stringBuilder.Insert(num2, "\n");
						flag = true;
						num = startCharIdx;
						break;
					}
					num = startCharIdx;
				}
				else
				{
					num = startCharIdx;
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("New Text ClearUpExplainMode " + this.text + " error ! Exception:" + ex.Message);
			}
		}
		if (flag)
		{
			string text3 = _stringBuilder.ToString();
			if (m_Text != text3)
			{
				m_Text = text3;
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
		_stringBuilder.Clear();
	}

	private bool IsRtl(string str, out int arabicCount)
	{
		arabicCount = 0;
		bool flag = GameEntry.Localization != null && GameEntry.Localization.Language == Language.Arabic;
		hasForceArabic = TryGetComponent<ForceArabicText>(out arabicComponent);
		if (hasForceArabic)
		{
			SetRtlAlign(flag ? arabicComponent.ArabicLangForceAlign : arabicComponent.NonArabicLangForceAlign, auto: false);
		}
		foreach (char c in str)
		{
			if ((c >= '\u0600' && c <= 'ۿ') || (c >= 'ﹰ' && c <= '\ufeff'))
			{
				arabicCount++;
			}
		}
		return arabicCount > 0;
	}

	private void SetRtlAlign(TextAnchor targetAlign, bool auto)
	{
		if (MirrorVersionConfig.IsMirrorVersionOpen)
		{
			return;
		}
		if (!auto)
		{
			base.alignment = targetAlign;
			return;
		}
		switch (base.alignment)
		{
		case TextAnchor.LowerLeft:
			base.alignment = TextAnchor.LowerRight;
			break;
		case TextAnchor.MiddleLeft:
			base.alignment = TextAnchor.MiddleRight;
			break;
		case TextAnchor.UpperLeft:
			base.alignment = TextAnchor.UpperRight;
			break;
		}
	}

	private string RTLTMProGetFixedText(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}
		bool flag = IsHasHorPreferWidth();
		Holder = input.Split(new char[1] { '\n' });
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < Holder.Length; i++)
		{
			base.cachedTextGenerator.PopulateWithErrors(Holder[i], GetGenerationSettings(base.rectTransform.rect.size), base.gameObject);
			RTLFixedText.Clear();
			RTLSupport.FixRTL(Holder[i], RTLFixedText, farsi: false, fixTextTags: true, preserveNumbers: true);
			string text = RTLFixedText.ToString();
			bool num = base.horizontalOverflow != HorizontalWrapMode.Overflow && !flag;
			RTLTMPro.UGUITextLines[] rTLTMProLinesInfo = GetRTLTMProLinesInfo(input);
			if (num && rTLTMProLinesInfo.Length != 0)
			{
				text = RTLSupport.InsertEoLToUguiText(text, rTLTMProLinesInfo);
			}
			stringBuilder.Append(text);
		}
		return stringBuilder.ToString();
	}

	private RTLTMPro.UGUITextLines[] GetRTLTMProLinesInfo(string input)
	{
		int count = base.cachedTextGenerator.lines.Count;
		RTLTMPro.UGUITextLines[] array = new RTLTMPro.UGUITextLines[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new RTLTMPro.UGUITextLines();
			array[i].startIndex = base.cachedTextGenerator.lines[i].startCharIdx;
			array[i].endIndex = ((i == count - 1) ? input.Length : base.cachedTextGenerator.lines[i + 1].startCharIdx);
		}
		return array;
	}

	public static ArabicSupport.UGUITextLines[] GetArabicSupportLinesInfo(TextGenerator generator, string input)
	{
		if (generator.rectExtents.width < 1f || generator.rectExtents.height < 1f)
		{
			ArabicSupport.UGUITextLines uGUITextLines = new ArabicSupport.UGUITextLines
			{
				startIndex = 0,
				endIndex = input.Length - 1
			};
			return new ArabicSupport.UGUITextLines[1] { uGUITextLines };
		}
		int count = generator.lines.Count;
		ArabicSupport.UGUITextLines[] array = new ArabicSupport.UGUITextLines[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new ArabicSupport.UGUITextLines();
			array[i].startIndex = generator.lines[i].startCharIdx;
			array[i].endIndex = ((i == count - 1) ? (input.Length - 1) : Math.Max(0, generator.lines[i + 1].startCharIdx - 1));
		}
		return array;
	}

	private bool IsHasHorPreferWidth()
	{
		if (base.gameObject.TryGetComponent<ContentSizeFitter>(out var component))
		{
			return component.horizontalFit == ContentSizeFitter.FitMode.PreferredSize;
		}
		return false;
	}

	private bool IsHasVerPreferWidth()
	{
		if (base.gameObject.TryGetComponent<ContentSizeFitter>(out var component))
		{
			return component.verticalFit == ContentSizeFitter.FitMode.PreferredSize;
		}
		return false;
	}

	private bool IsHasVerOrHorPreferWidthInParent()
	{
		Transform parent = base.gameObject.transform.parent;
		if (parent != null && parent.TryGetComponent<ContentSizeFitter>(out var component))
		{
			if (component.verticalFit != ContentSizeFitter.FitMode.PreferredSize)
			{
				return component.horizontalFit == ContentSizeFitter.FitMode.PreferredSize;
			}
			return true;
		}
		return false;
	}

	public static ArabicSupport.UGUITextLines[] PopulateWithErrors(Text text, string finalTxt)
	{
		text.cachedTextGenerator.PopulateWithErrors(finalTxt, text.GetGenerationSettings(text.rectTransform.rect.size), text.gameObject);
		return GetArabicSupportLinesInfo(text.cachedTextGenerator, finalTxt);
	}
}
