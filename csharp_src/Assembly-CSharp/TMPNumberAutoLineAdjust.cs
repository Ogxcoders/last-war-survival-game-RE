using System.Text;
using GameFramework.Localization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUIEx))]
public class TMPNumberAutoLineAdjust : MonoBehaviour
{
	[TextArea]
	private string originalText;

	private TextMeshProUGUIEx tmp;

	private bool isProcessing;

	private static readonly string GermanExtraChars = " .,%";

	private void Awake()
	{
		tmp = GetComponent<TextMeshProUGUIEx>();
	}

	public void FixDigitsLayoutNextFrame()
	{
		if (!isProcessing)
		{
			isProcessing = true;
			Invoke("AdjustTextNextFrame", 0.02f);
		}
	}

	private void AdjustTextNextFrame()
	{
		originalText = tmp.text;
		string text = InsertNewLinesBeforeWrappedDigitGroups(tmp);
		if (text != originalText)
		{
			tmp.text = text;
		}
		isProcessing = false;
	}

	private string InsertNewLinesBeforeWrappedDigitGroups(TextMeshProUGUIEx tmp)
	{
		TMP_TextInfo textInfo = tmp.textInfo;
		StringBuilder stringBuilder = new StringBuilder(tmp.text);
		int num = 0;
		int num2 = -1;
		for (int i = 0; i < textInfo.characterCount; i++)
		{
			char character = textInfo.characterInfo[i].character;
			if (IsPartOfNumberGroup(character))
			{
				if (num2 == -1)
				{
					num2 = i;
				}
			}
			else if (num2 != -1)
			{
				if (textInfo.characterInfo[num2].lineNumber != textInfo.characterInfo[i - 1].lineNumber)
				{
					stringBuilder.Insert(num2 + num, "\n");
					num++;
				}
				num2 = -1;
			}
		}
		if (num2 != -1 && textInfo.characterInfo[num2].lineNumber != textInfo.characterInfo[textInfo.characterCount - 1].lineNumber)
		{
			stringBuilder.Insert(num2 + num, "\n");
		}
		return stringBuilder.ToString();
	}

	private bool IsPartOfNumberGroup(char ch)
	{
		LocalizationManager localization = GameEntry.Localization;
		if (localization != null && localization.Language == Language.German)
		{
			if (!char.IsDigit(ch))
			{
				return GermanExtraChars.IndexOf(ch) >= 0;
			}
			return true;
		}
		if (!char.IsDigit(ch) && ch != '.')
		{
			return ch == ',';
		}
		return true;
	}
}
