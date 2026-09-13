using System.Collections.Generic;

namespace RTLTMPro;

public static class RTLSupport
{
	public const int DefaultBufferSize = 2048;

	private static FastStringBuilder inputBuilder;

	private static FastStringBuilder glyphFixerOutput;

	static RTLSupport()
	{
		inputBuilder = new FastStringBuilder(2048);
		glyphFixerOutput = new FastStringBuilder(2048);
	}

	public static void FixRTL(string input, FastStringBuilder output, bool farsi = true, bool fixTextTags = true, bool preserveNumbers = false)
	{
		inputBuilder.SetValue(input);
		TashkeelFixer.RemoveTashkeel(inputBuilder);
		GlyphFixer.Fix(inputBuilder, glyphFixerOutput, preserveNumbers, farsi, fixTextTags);
		TashkeelFixer.RestoreTashkeel(glyphFixerOutput);
		TashkeelFixer.FixShaddaCombinations(glyphFixerOutput);
		LigatureFixer.Fix(glyphFixerOutput, output, farsi, fixTextTags, preserveNumbers);
		if (fixTextTags)
		{
			RichTextFixer.Fix(output);
		}
		inputBuilder.Clear();
	}

	public static string InsertEoLToUguiText(string input, UGUITextLines[] linesInfo)
	{
		List<string> list = new List<string>();
		int num = linesInfo.Length;
		int num2 = 0;
		int num3 = 0;
		int num4 = linesInfo[num2].endIndex - linesInfo[num2].startIndex + 1;
		bool flag = false;
		for (int num5 = input.Length - 1; num5 >= 0; num5--)
		{
			num3++;
			if (num3 >= num4)
			{
				list.Add(input.Substring(num5, num3));
				flag = true;
			}
			if (flag)
			{
				num3 = 0;
				num2++;
				if (num2 >= num)
				{
					break;
				}
				num4 = linesInfo[num2].endIndex - linesInfo[num2].startIndex + 1;
				flag = false;
			}
		}
		list.Add(input.Substring(0, num3));
		string text = list[0];
		for (int i = 1; i < list.Count; i++)
		{
			text = text + "\n" + list[i];
		}
		return text;
	}
}
