using System.Runtime.InteropServices;
using System.Security;

namespace System.Globalization;

[Serializable]
[ComVisible(true)]
public class JapaneseCalendar : Calendar
{
	internal static readonly DateTime calendarMinValue = new DateTime(1868, 9, 8);

	internal static volatile EraInfo[] japaneseEraInfo;

	private const string c_japaneseErasHive = "System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

	private const string c_japaneseErasHivePermissionList = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

	internal static volatile Calendar s_defaultInstance;

	internal GregorianCalendarHelper helper;

	private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 99;

	[ComVisible(false)]
	public override DateTime MinSupportedDateTime => calendarMinValue;

	[ComVisible(false)]
	public override DateTime MaxSupportedDateTime => DateTime.MaxValue;

	[ComVisible(false)]
	public override CalendarAlgorithmType AlgorithmType => CalendarAlgorithmType.SolarCalendar;

	internal override int ID => 3;

	public override int[] Eras => helper.Eras;

	public override int TwoDigitYearMax
	{
		get
		{
			if (twoDigitYearMax == -1)
			{
				twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(ID, 99);
			}
			return twoDigitYearMax;
		}
		set
		{
			VerifyWritable();
			if (value < 99 || value > helper.MaxYear)
			{
				throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 99, helper.MaxYear));
			}
			twoDigitYearMax = value;
		}
	}

	internal static EraInfo[] GetEraInfo()
	{
		if (japaneseEraInfo == null)
		{
			japaneseEraInfo = GetErasFromRegistry();
			if (japaneseEraInfo == null)
			{
				japaneseEraInfo = new EraInfo[5]
				{
					new EraInfo(5, 2019, 5, 1, 2018, 1, 7981, "令和", "令", "R"),
					new EraInfo(4, 1989, 1, 8, 1988, 1, 31, "平成", "平", "H"),
					new EraInfo(3, 1926, 12, 25, 1925, 1, 64, "昭和", "昭", "S"),
					new EraInfo(2, 1912, 7, 30, 1911, 1, 15, "大正", "大", "T"),
					new EraInfo(1, 1868, 1, 1, 1867, 1, 45, "明治", "明", "M")
				};
			}
		}
		return japaneseEraInfo;
	}

	[SecuritySafeCritical]
	private static EraInfo[] GetErasFromRegistry()
	{
		return null;
	}

	private static int CompareEraRanges(EraInfo a, EraInfo b)
	{
		return b.ticks.CompareTo(a.ticks);
	}

	private static EraInfo GetEraFromValue(string value, string data)
	{
		if (value == null || data == null)
		{
			return null;
		}
		if (value.Length != 10)
		{
			return null;
		}
		if (!Number.TryParseInt32(value.Substring(0, 4), NumberStyles.None, NumberFormatInfo.InvariantInfo, out var result) || !Number.TryParseInt32(value.Substring(5, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out var result2) || !Number.TryParseInt32(value.Substring(8, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out var result3))
		{
			return null;
		}
		string[] array = data.Split(new char[1] { '_' });
		if (array.Length != 4)
		{
			return null;
		}
		if (array[0].Length == 0 || array[1].Length == 0 || array[2].Length == 0 || array[3].Length == 0)
		{
			return null;
		}
		return new EraInfo(0, result, result2, result3, result - 1, 1, 0, array[0], array[1], array[3]);
	}

	internal static Calendar GetDefaultInstance()
	{
		if (s_defaultInstance == null)
		{
			s_defaultInstance = new JapaneseCalendar();
		}
		return s_defaultInstance;
	}

	public JapaneseCalendar()
	{
		try
		{
			new CultureInfo("ja-JP");
		}
		catch (ArgumentException innerException)
		{
			throw new TypeInitializationException(GetType().FullName, innerException);
		}
		helper = new GregorianCalendarHelper(this, GetEraInfo());
	}

	public override DateTime AddMonths(DateTime time, int months)
	{
		return helper.AddMonths(time, months);
	}

	public override DateTime AddYears(DateTime time, int years)
	{
		return helper.AddYears(time, years);
	}

	public override int GetDaysInMonth(int year, int month, int era)
	{
		return helper.GetDaysInMonth(year, month, era);
	}

	public override int GetDaysInYear(int year, int era)
	{
		return helper.GetDaysInYear(year, era);
	}

	public override int GetDayOfMonth(DateTime time)
	{
		return helper.GetDayOfMonth(time);
	}

	public override DayOfWeek GetDayOfWeek(DateTime time)
	{
		return helper.GetDayOfWeek(time);
	}

	public override int GetDayOfYear(DateTime time)
	{
		return helper.GetDayOfYear(time);
	}

	public override int GetMonthsInYear(int year, int era)
	{
		return helper.GetMonthsInYear(year, era);
	}

	[ComVisible(false)]
	public override int GetWeekOfYear(DateTime time, CalendarWeekRule rule, DayOfWeek firstDayOfWeek)
	{
		return helper.GetWeekOfYear(time, rule, firstDayOfWeek);
	}

	public override int GetEra(DateTime time)
	{
		return helper.GetEra(time);
	}

	public override int GetMonth(DateTime time)
	{
		return helper.GetMonth(time);
	}

	public override int GetYear(DateTime time)
	{
		return helper.GetYear(time);
	}

	public override bool IsLeapDay(int year, int month, int day, int era)
	{
		return helper.IsLeapDay(year, month, day, era);
	}

	public override bool IsLeapYear(int year, int era)
	{
		return helper.IsLeapYear(year, era);
	}

	[ComVisible(false)]
	public override int GetLeapMonth(int year, int era)
	{
		return helper.GetLeapMonth(year, era);
	}

	public override bool IsLeapMonth(int year, int month, int era)
	{
		return helper.IsLeapMonth(year, month, era);
	}

	public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
	{
		return helper.ToDateTime(year, month, day, hour, minute, second, millisecond, era);
	}

	public override int ToFourDigitYear(int year)
	{
		if (year <= 0)
		{
			throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Positive number required."));
		}
		if (year > helper.MaxYear)
		{
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, helper.MaxYear));
		}
		return year;
	}

	internal static string[] EraNames()
	{
		EraInfo[] eraInfo = GetEraInfo();
		string[] array = new string[eraInfo.Length];
		for (int i = 0; i < eraInfo.Length; i++)
		{
			array[i] = eraInfo[eraInfo.Length - i - 1].eraName;
		}
		return array;
	}

	internal static string[] AbbrevEraNames()
	{
		EraInfo[] eraInfo = GetEraInfo();
		string[] array = new string[eraInfo.Length];
		for (int i = 0; i < eraInfo.Length; i++)
		{
			array[i] = eraInfo[eraInfo.Length - i - 1].abbrevEraName;
		}
		return array;
	}

	internal static string[] EnglishEraNames()
	{
		EraInfo[] eraInfo = GetEraInfo();
		string[] array = new string[eraInfo.Length];
		for (int i = 0; i < eraInfo.Length; i++)
		{
			array[i] = eraInfo[eraInfo.Length - i - 1].englishEraName;
		}
		return array;
	}

	internal override bool IsValidYear(int year, int era)
	{
		return helper.IsValidYear(year, era);
	}
}
