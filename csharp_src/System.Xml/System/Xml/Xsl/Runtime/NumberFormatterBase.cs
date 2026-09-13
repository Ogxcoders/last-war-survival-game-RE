using System.Text;

namespace System.Xml.Xsl.Runtime;

internal class NumberFormatterBase
{
	protected const int MaxAlphabeticValue = int.MaxValue;

	private const int MaxAlphabeticLength = 7;

	protected const int MaxRomanValue = 32767;

	private const string RomanDigitsUC = "IIVIXXLXCCDCM";

	private const string RomanDigitsLC = "iivixxlxccdcm";

	private static readonly int[] RomanDigitValue = new int[13]
	{
		1, 4, 5, 9, 10, 40, 50, 90, 100, 400,
		500, 900, 1000
	};

	private const string hiraganaAiueo = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをん";

	private const string hiraganaIroha = "いろはにほへとちりぬるをわかよたれそつねならむうゐのおくやまけふこえてあさきゆめみしゑひもせす";

	private const string katakanaAiueo = "アイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲン";

	private const string katakanaIroha = "イロハニホヘトチリヌルヲワカヨタレソツネナラムウヰノオクヤマケフコエテアサキユメミシヱヒモセスン";

	private const string katakanaAiueoHw = "ｱｲｳｴｵｶｷｸｹｺｻｼｽｾｿﾀﾁﾂﾃﾄﾅﾆﾇﾈﾉﾊﾋﾌﾍﾎﾏﾐﾑﾒﾓﾔﾕﾖﾗﾘﾙﾚﾛﾜｦﾝ";

	private const string katakanaIrohaHw = "ｲﾛﾊﾆﾎﾍﾄﾁﾘﾇﾙｦﾜｶﾖﾀﾚｿﾂﾈﾅﾗﾑｳヰﾉｵｸﾔﾏｹﾌｺｴﾃｱｻｷﾕﾒﾐｼヱﾋﾓｾｽﾝ";

	private const string cjkIdeographic = "〇一二三四五六七八九";

	public static void ConvertToAlphabetic(StringBuilder sb, double val, char firstChar, int totalChars)
	{
		char[] array = new char[7];
		int num = 7;
		int num2 = (int)val;
		while (num2 > totalChars)
		{
			int num3 = --num2 / totalChars;
			array[--num] = (char)(firstChar + (num2 - num3 * totalChars));
			num2 = num3;
		}
		array[--num] = (char)(firstChar + --num2);
		sb.Append(array, num, 7 - num);
	}

	public static void ConvertToRoman(StringBuilder sb, double val, bool upperCase)
	{
		int num = (int)val;
		string value = (upperCase ? "IIVIXXLXCCDCM" : "iivixxlxccdcm");
		int num2 = RomanDigitValue.Length;
		while (num2-- != 0)
		{
			while (num >= RomanDigitValue[num2])
			{
				num -= RomanDigitValue[num2];
				sb.Append(value, num2, 1 + (num2 & 1));
			}
		}
	}
}
