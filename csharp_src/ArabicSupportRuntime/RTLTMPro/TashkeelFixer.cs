using System.Collections.Generic;

namespace RTLTMPro;

public static class TashkeelFixer
{
	private static readonly List<TashkeelLocation> TashkeelLocations = new List<TashkeelLocation>(100);

	private static readonly string ShaddaDammatan = new string(new char[2] { '\u0651', '\u064c' });

	private static readonly string ShaddaKasratan = new string(new char[2] { '\u0651', '\u064d' });

	private static readonly string ShaddaSuperscriptAlef = new string(new char[2] { '\u0651', '\u0670' });

	private static readonly string ShaddaFatha = new string(new char[2] { '\u0651', '\u064e' });

	private static readonly string ShaddaDamma = new string(new char[2] { '\u0651', '\u064f' });

	private static readonly string ShaddaKasra = new string(new char[2] { '\u0651', '\u0650' });

	private static readonly string ShaddaWithFathaIsolatedForm = 'ﱠ'.ToString();

	private static readonly string ShaddaWithDammaIsolatedForm = 'ﱡ'.ToString();

	private static readonly string ShaddaWithKasraIsolatedForm = 'ﱢ'.ToString();

	private static readonly string ShaddaWithDammatanIsolatedForm = 'ﱞ'.ToString();

	private static readonly string ShaddaWithKasratanIsolatedForm = 'ﱟ'.ToString();

	private static readonly string ShaddaWithSuperscriptAlefIsolatedForm = 'ﱣ'.ToString();

	private static readonly HashSet<char> TashkeelCharactersSet = new HashSet<char>
	{
		'\u064b', '\u064c', '\u064d', '\u064e', '\u064f', '\u0650', '\u0651', '\u0652', '\u0653', '\u0670',
		'ﱞ', 'ﱟ', 'ﱠ', 'ﱡ', 'ﱢ', 'ﱣ'
	};

	private static readonly Dictionary<char, char> ShaddaCombinationMap = new Dictionary<char, char>
	{
		['\u064c'] = 'ﱞ',
		['\u064d'] = 'ﱟ',
		['\u064e'] = 'ﱠ',
		['\u064f'] = 'ﱡ',
		['\u0650'] = 'ﱢ',
		['\u0670'] = 'ﱣ'
	};

	public static void RemoveTashkeel(FastStringBuilder input)
	{
		TashkeelLocations.Clear();
		int num = 0;
		for (int i = 0; i < input.Length; i++)
		{
			int num2 = input.Get(i);
			if (Char32Utils.IsUnicode16Char(num2) && TashkeelCharactersSet.Contains((char)num2))
			{
				TashkeelLocations.Add(new TashkeelLocation((TashkeelCharacters)num2, i));
				continue;
			}
			input.Set(num, num2);
			num++;
		}
		input.Length = num;
	}

	public static void RestoreTashkeel(FastStringBuilder letters)
	{
		foreach (TashkeelLocation tashkeelLocation in TashkeelLocations)
		{
			letters.Insert(tashkeelLocation.Position, tashkeelLocation.Tashkeel);
		}
	}

	public static void FixShaddaCombinations(FastStringBuilder input)
	{
		int num = 0;
		int num2 = 0;
		while (num2 < input.Length)
		{
			int num3 = input.Get(num2);
			int num4 = ((num2 < input.Length - 1) ? input.Get(num2 + 1) : 0);
			if (num3 == 1617 && ShaddaCombinationMap.ContainsKey((char)num4))
			{
				input.Set(num, ShaddaCombinationMap[(char)num4]);
				num++;
				num2 += 2;
			}
			else
			{
				input.Set(num, num3);
				num++;
				num2++;
			}
		}
		input.Length = num;
	}
}
