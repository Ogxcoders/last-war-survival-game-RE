using System.Collections.Generic;
using UnityEngine;

namespace RTLTMPro;

public static class LigatureFixer
{
	private static readonly Dictionary<char, char> MirroredCharsMap = new Dictionary<char, char>
	{
		['('] = ')',
		[')'] = '(',
		['['] = ']',
		[']'] = '[',
		['<'] = '>',
		['>'] = '<',
		['≥'] = '≤',
		['≤'] = '≥'
	};

	private static readonly HashSet<char> MirroredCharsSet = new HashSet<char>(MirroredCharsMap.Keys);

	private static void FlushBufferToOutput(List<int> buffer, FastStringBuilder output)
	{
		for (int i = 0; i < buffer.Count; i++)
		{
			output.Append(buffer[buffer.Count - 1 - i]);
		}
		buffer.Clear();
	}

	public static void Fix(FastStringBuilder input, FastStringBuilder output, bool farsi, bool fixTextTags, bool preserveNumbers)
	{
		List<int> list = new List<int>(512);
		List<int> list2 = new List<int>(512);
		list.Clear();
		list2.Clear();
		if (Application.isEditor)
		{
			char[] array = new char[input.Length];
			for (int num = input.Length - 1; num >= 0; num--)
			{
				array[num] = (char)input.Get(num);
			}
		}
		_ = input.Length;
		for (int num2 = input.Length - 1; num2 >= 0; num2--)
		{
			bool flag = num2 > 0 && num2 < input.Length - 1;
			bool flag2 = num2 == 0;
			bool flag3 = num2 == input.Length - 1;
			int num3 = input.Get(num2);
			int num4 = 0;
			if (!flag3)
			{
				num4 = input.Get(num2 + 1);
			}
			int num5 = 0;
			if (!flag2)
			{
				num5 = input.Get(num2 - 1);
			}
			if (num2 >= 3 && num3 == 65258 && num5 == 65248 && input.Get(num2 - 2) == 65247 && input.Get(num2 - 3) == 65165)
			{
				output.Append(65010);
				num2 -= 3;
				continue;
			}
			if (fixTextTags && num3 == 62 && num5 != 45)
			{
				bool flag4 = false;
				bool flag5 = false;
				int num6 = num2;
				list2.Add(num3);
				for (int num7 = num2 - 1; num7 >= 0; num7--)
				{
					int num8 = input.Get(num7);
					list2.Add(num8);
					if (num8 == 60)
					{
						int num9 = input.Get(num7 + 1);
						if (num9 != 32)
						{
							flag4 = true;
							if (num9 == 47)
							{
								flag5 = true;
							}
							num6 = num7;
						}
						break;
					}
				}
				if (flag4)
				{
					_ = input.Length - 1;
					FlushBufferToOutput(list2, output);
					num2 = num6;
					if (!flag5)
					{
						continue;
					}
					num2--;
					FastStringBuilder fastStringBuilder = new FastStringBuilder(2048);
					FastStringBuilder fastStringBuilder2 = new FastStringBuilder(2048);
					bool flag6 = false;
					int num10 = 0;
					while (num2 >= 0)
					{
						if (input.Get(num2) == 62 && (num2 == 0 || input.Get(num2 - 1) != 45))
						{
							for (int num11 = num2; num11 >= 0; num11--)
							{
								if (input.Get(num11) == 60)
								{
									if (input.Get(num11 + 1) != 47 && input.Get(num11 + 1) != 32)
									{
										if (num10 > 0)
										{
											num10--;
										}
										else
										{
											flag6 = true;
										}
									}
									else
									{
										num10++;
									}
									break;
								}
							}
							if (flag6)
							{
								break;
							}
						}
						fastStringBuilder.Append(input.Get(num2));
						num2--;
					}
					fastStringBuilder.Reverse();
					Fix(fastStringBuilder, fastStringBuilder2, farsi, fixTextTags, preserveNumbers);
					for (int i = 0; i < fastStringBuilder2.Length; i++)
					{
						output.Append(fastStringBuilder2.Get(i));
					}
					num2++;
					continue;
				}
				list2.Clear();
			}
			if (Char32Utils.IsPunctuation(num3) || Char32Utils.IsSymbol(num3))
			{
				bool flag7 = MirroredCharsSet.Contains((char)num3);
				if (flag7)
				{
					num3 = MirroredCharsMap[(char)num3];
				}
				bool flag8 = Char32Utils.IsRTLCharacter(num5);
				bool flag9 = Char32Utils.IsRTLCharacter(num4);
				bool flag10 = Char32Utils.IsNumber(num5, preserveNumbers, farsi);
				bool flag11 = Char32Utils.IsNumber(num4, preserveNumbers, farsi);
				Char32Utils.IsEnglishLetter(num5);
				Char32Utils.IsEnglishLetter(num4);
				bool flag12 = Char32Utils.IsWhiteSpace(num4);
				bool flag13 = Char32Utils.IsWhiteSpace(num5);
				bool flag14 = num3 == 95;
				bool flag15 = num3 == 46 || num3 == 1548 || num3 == 45;
				bool flag16 = num3 == 46 && flag10;
				bool flag17 = num3 == 58 && (flag8 || flag12);
				bool flag18 = num3 == 34 || num3 == 1548 || num3 == 1563 || num3 == 33;
				bool flag19 = num3 == 43 || num3 == 45 || num3 == 42 || num3 == 215 || num3 == 47;
				bool flag20 = num3 == 46 || num3 == 1548;
				bool flag21 = num3 == 62 && num5 == 45;
				bool flag22 = num3 == 45 && num5 == 60;
				if (flag)
				{
					if (flag11 && flag10 && Char32Utils.IsPunctuation(num3) && !flag7)
					{
						list.Add(num3);
					}
					else if (flag21)
					{
						num2--;
						output.Append('<');
						output.Append('-');
					}
					else if (flag22)
					{
						num2--;
						output.Append('-');
						output.Append('>');
					}
					else if ((flag9 && flag8) || (flag13 && flag15) || flag17 || flag18 || (flag12 && flag8) || (flag12 && flag20) || (flag9 && flag13) || ((flag9 || flag8) && flag14) || flag16 || (flag19 && (flag8 || flag13 || flag9)))
					{
						FlushBufferToOutput(list, output);
						output.Append(num3);
					}
					else if (!flag7)
					{
						list.Add(num3);
					}
					else
					{
						FlushBufferToOutput(list, output);
						output.Append(num3);
					}
				}
				else if (flag3)
				{
					bool flag23 = num3 == 33 || num3 == 1563 || num3 == 46 || num3 == 1567 || num3 == 34 || num3 == 58;
					if (flag7 || flag23)
					{
						FlushBufferToOutput(list, output);
						output.Append(num3);
					}
					else
					{
						list.Add(num3);
					}
				}
				else if (flag2)
				{
					FlushBufferToOutput(list, output);
					output.Append(num3);
				}
				continue;
			}
			if (flag)
			{
				bool flag24 = Char32Utils.IsEnglishLetter(num5);
				bool flag25 = Char32Utils.IsEnglishLetter(num4);
				bool flag26 = Char32Utils.IsNumber(num5, preserveNumbers, farsi);
				bool flag27 = Char32Utils.IsNumber(num4, preserveNumbers, farsi);
				bool flag28 = Char32Utils.IsSymbol(num5);
				bool flag29 = Char32Utils.IsSymbol(num4);
				bool flag30 = Char32Utils.IsPunctuation(num5);
				bool flag31 = Char32Utils.IsPunctuation(num4);
				bool flag32 = num5 == 32;
				bool flag33 = num4 == 32;
				bool flag34 = num5 == 58;
				bool flag35 = num3 == 32 || num3 == 160;
				bool flag36 = MirroredCharsSet.Contains((char)num5);
				if (flag35 && ((flag34 && !flag33) || flag36))
				{
					FlushBufferToOutput(list, output);
					output.Append(num3);
				}
				else if (flag35 && (flag25 || flag27 || flag29 || flag31 || flag33) && (flag24 || flag26 || flag28 || flag30 || flag32))
				{
					list.Add(num3);
					continue;
				}
			}
			if ((Char32Utils.IsLetter(num3) && !Char32Utils.IsRTLCharacter(num3)) || Char32Utils.IsNumber(num3, preserveNumbers, farsi))
			{
				list.Add(num3);
				continue;
			}
			if ((num3 >= 55296 && num3 <= 56319) || (num3 >= 56320 && num3 <= 57343))
			{
				list.Add(num3);
				continue;
			}
			FlushBufferToOutput(list, output);
			if (num3 != 65535 && num3 != 8204)
			{
				output.Append(num3);
			}
		}
		FlushBufferToOutput(list, output);
		if (Application.isEditor)
		{
			char[] array2 = new char[output.Length];
			for (int num12 = output.Length - 1; num12 >= 0; num12--)
			{
				array2[num12] = (char)output.Get(num12);
			}
		}
	}
}
