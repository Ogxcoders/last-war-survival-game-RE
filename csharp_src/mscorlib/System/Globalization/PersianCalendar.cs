namespace System.Globalization;

[Serializable]
public class PersianCalendar : Calendar
{
	public static readonly int PersianEra = 1;

	internal static long PersianEpoch = new DateTime(622, 3, 22).Ticks / 864000000000L;

	private const int ApproximateHalfYear = 180;

	internal const int DatePartYear = 0;

	internal const int DatePartDayOfYear = 1;

	internal const int DatePartMonth = 2;

	internal const int DatePartDay = 3;

	internal const int MonthsPerYear = 12;

	internal static int[] DaysToMonth = new int[13]
	{
		0, 31, 62, 93, 124, 155, 186, 216, 246, 276,
		306, 336, 366
	};

	internal const int MaxCalendarYear = 9378;

	internal const int MaxCalendarMonth = 10;

	internal const int MaxCalendarDay = 13;

	internal static DateTime minDate = new DateTime(622, 3, 22);

	internal static DateTime maxDate = DateTime.MaxValue;

	private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 1410;

	public override DateTime MinSupportedDateTime => minDate;

	public override DateTime MaxSupportedDateTime => maxDate;

	public override CalendarAlgorithmType AlgorithmType => CalendarAlgorithmType.SolarCalendar;

	internal override int BaseCalendarID => 1;

	internal override int ID => 22;

	public override int[] Eras => new int[1] { PersianEra };

	public override int TwoDigitYearMax
	{
		get
		{
			if (twoDigitYearMax == -1)
			{
				twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(ID, 1410);
			}
			return twoDigitYearMax;
		}
		set
		{
			VerifyWritable();
			if (value < 99 || value > 9378)
			{
				throw new ArgumentOutOfRangeException("value", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 99, 9378));
			}
			twoDigitYearMax = value;
		}
	}

	private long GetAbsoluteDatePersian(int year, int month, int day)
	{
		if (year >= 1 && year <= 9378 && month >= 1 && month <= 12)
		{
			int num = DaysInPreviousMonths(month) + day - 1;
			int num2 = (int)(365.242189 * (double)(year - 1));
			return CalendricalCalculationsHelper.PersianNewYearOnOrBefore(PersianEpoch + num2 + 180) + num;
		}
		throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Year, Month, and Day parameters describe an un-representable DateTime."));
	}

	internal static void CheckTicksRange(long ticks)
	{
		if (ticks < minDate.Ticks || ticks > maxDate.Ticks)
		{
			throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive."), minDate, maxDate));
		}
	}

	internal static void CheckEraRange(int era)
	{
		if (era != 0 && era != PersianEra)
		{
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("Era value was not valid."));
		}
	}

	internal static void CheckYearRange(int year, int era)
	{
		CheckEraRange(era);
		if (year < 1 || year > 9378)
		{
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 9378));
		}
	}

	internal static void CheckYearMonthRange(int year, int month, int era)
	{
		CheckYearRange(year, era);
		if (year == 9378 && month > 10)
		{
			throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 10));
		}
		if (month < 1 || month > 12)
		{
			throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("Month must be between one and twelve."));
		}
	}

	private static int MonthFromOrdinalDay(int ordinalDay)
	{
		int i;
		for (i = 0; ordinalDay > DaysToMonth[i]; i++)
		{
		}
		return i;
	}

	private static int DaysInPreviousMonths(int month)
	{
		month--;
		return DaysToMonth[month];
	}

	internal int GetDatePart(long ticks, int part)
	{
		CheckTicksRange(ticks);
		long num = ticks / 864000000000L + 1;
		int num2 = (int)Math.Floor((double)(CalendricalCalculationsHelper.PersianNewYearOnOrBefore(num) - PersianEpoch) / 365.242189 + 0.5) + 1;
		if (part == 0)
		{
			return num2;
		}
		int num3 = (int)(num - CalendricalCalculationsHelper.GetNumberOfDays(ToDateTime(num2, 1, 1, 0, 0, 0, 0, 1)));
		if (part == 1)
		{
			return num3;
		}
		int num4 = MonthFromOrdinalDay(num3);
		if (part == 2)
		{
			return num4;
		}
		int result = num3 - DaysInPreviousMonths(num4);
		if (part == 3)
		{
			return result;
		}
		throw new InvalidOperationException(Environment.GetResourceString("Internal Error in DateTime and Calendar operations."));
	}

	public override DateTime AddMonths(DateTime time, int months)
	{
		if (months < -120000 || months > 120000)
		{
			throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), -120000, 120000));
		}
		int datePart = GetDatePart(time.Ticks, 0);
		int datePart2 = GetDatePart(time.Ticks, 2);
		int num = GetDatePart(time.Ticks, 3);
		int num2 = datePart2 - 1 + months;
		if (num2 >= 0)
		{
			datePart2 = num2 % 12 + 1;
			datePart += num2 / 12;
		}
		else
		{
			datePart2 = 12 + (num2 + 1) % 12;
			datePart += (num2 - 11) / 12;
		}
		int daysInMonth = GetDaysInMonth(datePart, datePart2);
		if (num > daysInMonth)
		{
			num = daysInMonth;
		}
		long ticks = GetAbsoluteDatePersian(datePart, datePart2, num) * 864000000000L + time.Ticks % 864000000000L;
		Calendar.CheckAddResult(ticks, MinSupportedDateTime, MaxSupportedDateTime);
		return new DateTime(ticks);
	}

	public override DateTime AddYears(DateTime time, int years)
	{
		return AddMonths(time, years * 12);
	}

	public override int GetDayOfMonth(DateTime time)
	{
		return GetDatePart(time.Ticks, 3);
	}

	public override DayOfWeek GetDayOfWeek(DateTime time)
	{
		return (DayOfWeek)((int)(time.Ticks / 864000000000L + 1) % 7);
	}

	public override int GetDayOfYear(DateTime time)
	{
		return GetDatePart(time.Ticks, 1);
	}

	public override int GetDaysInMonth(int year, int month, int era)
	{
		CheckYearMonthRange(year, month, era);
		if (month == 10 && year == 9378)
		{
			return 13;
		}
		int num = DaysToMonth[month] - DaysToMonth[month - 1];
		if (month == 12 && !IsLeapYear(year))
		{
			num--;
		}
		return num;
	}

	public override int GetDaysInYear(int year, int era)
	{
		CheckYearRange(year, era);
		if (year == 9378)
		{
			return DaysToMonth[9] + 13;
		}
		if (!IsLeapYear(year, 0))
		{
			return 365;
		}
		return 366;
	}

	public override int GetEra(DateTime time)
	{
		CheckTicksRange(time.Ticks);
		return PersianEra;
	}

	public override int GetMonth(DateTime time)
	{
		return GetDatePart(time.Ticks, 2);
	}

	public override int GetMonthsInYear(int year, int era)
	{
		CheckYearRange(year, era);
		if (year == 9378)
		{
			return 10;
		}
		return 12;
	}

	public override int GetYear(DateTime time)
	{
		return GetDatePart(time.Ticks, 0);
	}

	public override bool IsLeapDay(int year, int month, int day, int era)
	{
		int daysInMonth = GetDaysInMonth(year, month, era);
		if (day < 1 || day > daysInMonth)
		{
			throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Day must be between 1 and {0} for month {1}."), daysInMonth, month));
		}
		if (IsLeapYear(year, era) && month == 12)
		{
			return day == 30;
		}
		return false;
	}

	public override int GetLeapMonth(int year, int era)
	{
		CheckYearRange(year, era);
		return 0;
	}

	public override bool IsLeapMonth(int year, int month, int era)
	{
		CheckYearMonthRange(year, month, era);
		return false;
	}

	public override bool IsLeapYear(int year, int era)
	{
		CheckYearRange(year, era);
		if (year == 9378)
		{
			return false;
		}
		return GetAbsoluteDatePersian(year + 1, 1, 1) - GetAbsoluteDatePersian(year, 1, 1) == 366;
	}

	public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
	{
		int daysInMonth = GetDaysInMonth(year, month, era);
		if (day < 1 || day > daysInMonth)
		{
			throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Day must be between 1 and {0} for month {1}."), daysInMonth, month));
		}
		long absoluteDatePersian = GetAbsoluteDatePersian(year, month, day);
		if (absoluteDatePersian >= 0)
		{
			return new DateTime(absoluteDatePersian * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
		}
		throw new ArgumentOutOfRangeException(null, Environment.GetResourceString("Year, Month, and Day parameters describe an un-representable DateTime."));
	}

	public override int ToFourDigitYear(int year)
	{
		if (year < 0)
		{
			throw new ArgumentOutOfRangeException("year", Environment.GetResourceString("Non-negative number required."));
		}
		if (year < 100)
		{
			return base.ToFourDigitYear(year);
		}
		if (year > 9378)
		{
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, 9378));
		}
		return year;
	}
}
