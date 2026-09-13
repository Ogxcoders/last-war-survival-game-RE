using GameFramework.Localization;
using UnityEngine;

namespace TMPro;

public class TMP_InputFieldEx : TMP_InputField
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

	protected override void Awake()
	{
		base.Awake();
		InitTMPComponents();
	}

	public void CheckisArabicLang()
	{
		LocalizationManager localization = GameEntry.Localization;
		bool isArabicLang = localization != null && localization.Language == Language.Arabic;
		TextMeshProUGUI textMeshProUGUI = m_Placeholder as TextMeshProUGUI;
		if (textMeshProUGUI != null)
		{
			SetTextAlign(isArabicLang, textMeshProUGUI);
		}
		TextMeshProUGUI textMeshProUGUI2 = base.textComponent as TextMeshProUGUI;
		if (textMeshProUGUI2 != null)
		{
			SetTextAlign(isArabicLang, textMeshProUGUI2);
		}
	}

	private void SetTextAlign(bool isArabicLang, TextMeshProUGUI textMeshProUGUI)
	{
		TextAlignmentOptions alignment = textMeshProUGUI.alignment;
		if (isArabicLang)
		{
			alignment &= (TextAlignmentOptions)(-2);
			alignment |= (TextAlignmentOptions)4;
			float x = textMeshProUGUI.margin.x;
			float z = textMeshProUGUI.margin.z;
			textMeshProUGUI.margin = new Vector4(z, textMeshProUGUI.margin.y, x, textMeshProUGUI.margin.w);
		}
		else
		{
			alignment &= (TextAlignmentOptions)(-5);
			alignment |= (TextAlignmentOptions)1;
		}
		textMeshProUGUI.alignment = alignment;
	}

	public void InputFieldContentForceMeshUpdate()
	{
		m_TextComponent.text = base.text;
		m_TextComponent.ForceMeshUpdate();
	}

	public int GetInputFieldLineCount()
	{
		return m_TextComponent.textInfo.lineCount;
	}

	public float GetInputFieldLineHeightByNum(int lineNum)
	{
		lineNum = Mathf.Max(1, lineNum);
		return m_TextComponent.textInfo.lineInfo[lineNum - 1].lineHeight;
	}

	public void InsertStrInCaretPos(string insertStr)
	{
		ActivateInputField();
		Append(insertStr);
	}

	public void InitTMPComponents()
	{
		if (base.textComponent is TextMeshProUGUIEx textMeshProUGUIEx)
		{
			textMeshProUGUIEx.ChangeAutoSetting(value: true);
		}
		if (m_Placeholder is TextMeshProUGUIEx textMeshProUGUIEx2)
		{
			textMeshProUGUIEx2.ChangeAutoSetting(value: true);
		}
	}
}
