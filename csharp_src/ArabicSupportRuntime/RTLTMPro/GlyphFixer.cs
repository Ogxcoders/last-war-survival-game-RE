using System.Collections.Generic;

namespace RTLTMPro;

public static class GlyphFixer
{
	public static Dictionary<char, char> EnglishToFarsiNumberMap = new Dictionary<char, char>
	{
		['0'] = '۰',
		['1'] = '۱',
		['2'] = '۲',
		['3'] = '۳',
		['4'] = '۴',
		['5'] = '۵',
		['6'] = '۶',
		['7'] = '۷',
		['8'] = '۸',
		['9'] = '۹'
	};

	public static Dictionary<char, char> EnglishToHinduNumberMap = new Dictionary<char, char>
	{
		['0'] = '٠',
		['1'] = '١',
		['2'] = '٢',
		['3'] = '٣',
		['4'] = '٤',
		['5'] = '٥',
		['6'] = '٦',
		['7'] = '٧',
		['8'] = '٨',
		['9'] = '٩'
	};

	public static void Fix(FastStringBuilder input, FastStringBuilder output, bool preserveNumbers, bool farsi, bool fixTextTags)
	{
		FixYah(input, farsi);
		output.SetValue(input);
		for (int i = 0; i < input.Length; i++)
		{
			bool flag = false;
			int num = input.Get(i);
			if (num == 1604 && i < input.Length - 1)
			{
				flag = HandleSpecialLam(input, output, i);
				if (flag)
				{
					num = output.Get(i);
				}
			}
			if (num == 1600 || num == 8204)
			{
				continue;
			}
			if (num < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num))
			{
				char c = GlyphTable.Convert((char)num);
				if (IsMiddleLetter(input, i))
				{
					output.Set(i, (ushort)(c + 3));
				}
				else if (IsFinishingLetter(input, i))
				{
					output.Set(i, (ushort)(c + 1));
				}
				else if (IsLeadingLetter(input, i))
				{
					output.Set(i, (ushort)(c + 2));
				}
				else
				{
					output.Set(i, c);
				}
			}
			if (flag)
			{
				i++;
			}
		}
		if (!preserveNumbers)
		{
			if (fixTextTags)
			{
				FixNumbersOutsideOfTags(output, farsi);
			}
			else
			{
				FixNumbers(output, farsi);
			}
		}
	}

	public static void FixYah(FastStringBuilder text, bool farsi)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (farsi && text.Get(i) == 1610)
			{
				text.Set(i, 1740);
			}
			else if (!farsi && text.Get(i) == 1740)
			{
				text.Set(i, 1610);
			}
		}
	}

	private static bool HandleSpecialLam(FastStringBuilder input, FastStringBuilder output, int i)
	{
		bool flag;
		switch (input.Get(i + 1))
		{
		case 1573:
			output.Set(i, 65271);
			flag = true;
			break;
		case 1575:
			output.Set(i, 65273);
			flag = true;
			break;
		case 1571:
			output.Set(i, 65269);
			flag = true;
			break;
		case 1570:
			output.Set(i, 65267);
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			output.Set(i + 1, 65535);
		}
		return flag;
	}

	public static void FixNumbers(FastStringBuilder text, bool farsi)
	{
		text.Replace(48, farsi ? 1776 : 1632);
		text.Replace(49, farsi ? 1777 : 1633);
		text.Replace(50, farsi ? 1778 : 1634);
		text.Replace(51, farsi ? 1779 : 1635);
		text.Replace(52, farsi ? 1780 : 1636);
		text.Replace(53, farsi ? 1781 : 1637);
		text.Replace(54, farsi ? 1782 : 1638);
		text.Replace(55, farsi ? 1783 : 1639);
		text.Replace(56, farsi ? 1784 : 1640);
		text.Replace(57, farsi ? 1785 : 1641);
	}

	public static void FixNumbersOutsideOfTags(FastStringBuilder text, bool farsi)
	{
		HashSet<char> hashSet = new HashSet<char>(EnglishToFarsiNumberMap.Keys);
		for (int i = 0; i < text.Length; i++)
		{
			int num = text.Get(i);
			if (num == 60)
			{
				bool flag = false;
				for (int j = i + 1; j < text.Length; j++)
				{
					int num2 = text.Get(j);
					if (j != i + 1 || num2 != 32)
					{
						switch (num2)
						{
						case 62:
							i = j;
							flag = true;
							break;
						default:
							continue;
						case 60:
							break;
						}
					}
					break;
				}
				if (flag)
				{
					continue;
				}
			}
			if (hashSet.Contains((char)num))
			{
				text.Set(i, farsi ? EnglishToFarsiNumberMap[(char)num] : EnglishToHinduNumberMap[(char)num]);
			}
		}
	}

	private static bool IsLeadingLetter(FastStringBuilder letters, int index)
	{
		int num = letters.Get(index);
		int num2 = 0;
		if (index != 0)
		{
			num2 = letters.Get(index - 1);
		}
		int num3 = 0;
		if (index < letters.Length - 1)
		{
			num3 = letters.Get(index + 1);
		}
		bool num4 = index == 0 || (num2 < 65535 && !TextUtils.IsGlyphFixedArabicCharacter((char)num2)) || num2 == 1569 || num2 == 1570 || num2 == 1571 || num2 == 1573 || num2 == 1572 || num2 == 1575 || num2 == 1583 || num2 == 1584 || num2 == 1585 || num2 == 1586 || num2 == 1688 || num2 == 1608 || num2 == 65153 || num2 == 65155 || num2 == 65159 || num2 == 65157 || num2 == 65165 || num2 == 65152 || num2 == 65193 || num2 == 65195 || num2 == 65197 || num2 == 65199 || num2 == 64394 || num2 == 65261 || num2 == 8204;
		bool flag = num != 32 && num != 1569 && num != 1571 && num != 1573 && num != 1570 && num != 1572 && num != 1575 && num != 1583 && num != 1584 && num != 1585 && num != 1586 && num != 1688 && num != 1608 && num != 8204;
		bool flag2 = index < letters.Length - 1 && num3 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num3) && num3 != 1569 && num3 != 8204;
		return num4 && flag && flag2;
	}

	private static bool IsFinishingLetter(FastStringBuilder letters, int index)
	{
		int num = letters.Get(index);
		int num2 = 0;
		if (index != 0)
		{
			num2 = letters.Get(index - 1);
		}
		bool num3 = index != 0 && num2 != 32 && num2 != 1569 && num2 != 1570 && num2 != 1571 && num2 != 1573 && num2 != 1572 && num2 != 1575 && num2 != 1583 && num2 != 1584 && num2 != 1585 && num2 != 1586 && num2 != 1688 && num2 != 1608 && num2 != 65152 && num2 != 65153 && num2 != 65155 && num2 != 65159 && num2 != 65157 && num2 != 65165 && num2 != 65193 && num2 != 65195 && num2 != 65197 && num2 != 65199 && num2 != 64394 && num2 != 65261 && num2 != 8204 && num2 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num2);
		bool flag = num != 32 && num != 8204 && num != 1569;
		return num3 && flag;
	}

	private static bool IsMiddleLetter(FastStringBuilder letters, int index)
	{
		int num = letters.Get(index);
		int num2 = 0;
		if (index != 0)
		{
			num2 = letters.Get(index - 1);
		}
		int num3 = 0;
		if (index < letters.Length - 1)
		{
			num3 = letters.Get(index + 1);
		}
		bool flag = index != 0 && num != 1569 && num != 1570 && num != 1571 && num != 1573 && num != 1572 && num != 1575 && num != 1583 && num != 1584 && num != 1585 && num != 1586 && num != 1688 && num != 1608 && num != 8204;
		bool flag2 = index != 0 && num2 != 65152 && num2 != 65153 && num2 != 65155 && num2 != 65159 && num2 != 65157 && num2 != 1575 && num2 != 1583 && num2 != 1584 && num2 != 1585 && num2 != 1586 && num2 != 1688 && num2 != 1608 && num2 != 1570 && num2 != 1571 && num2 != 1573 && num2 != 1572 && num2 != 1569 && num2 != 65165 && num2 != 65193 && num2 != 65195 && num2 != 65197 && num2 != 65199 && num2 != 64394 && num2 != 65261 && num2 != 8204 && num2 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num2);
		return index < letters.Length - 1 && num3 < 65535 && TextUtils.IsGlyphFixedArabicCharacter((char)num3) && num3 != 8204 && num3 != 1569 && num3 != 65152 && flag2 && flag;
	}
}
