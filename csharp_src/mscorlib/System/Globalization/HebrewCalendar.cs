using System.Runtime.InteropServices;

namespace System.Globalization;

[Serializable]
[ComVisible(true)]
public class HebrewCalendar : Calendar
{
	internal class __DateBuffer
	{
		internal int year;

		internal int month;

		internal int day;
	}

	public static readonly int HebrewEra = 1;

	internal const int DatePartYear = 0;

	internal const int DatePartDayOfYear = 1;

	internal const int DatePartMonth = 2;

	internal const int DatePartDay = 3;

	internal const int DatePartDayOfWeek = 4;

	private const int HebrewYearOf1AD = 3760;

	private const int FirstGregorianTableYear = 1583;

	private const int LastGregorianTableYear = 2239;

	private const int TABLESIZE = 656;

	private const int MinHebrewYear = 5343;

	private const int MaxHebrewYear = 5999;

	private static readonly int[] HebrewTable = new int[1316]
	{
		7, 3, 17, 3, 0, 4, 11, 2, 21, 6,
		1, 3, 13, 2, 25, 4, 5, 3, 16, 2,
		27, 6, 9, 1, 20, 2, 0, 6, 11, 3,
		23, 4, 4, 2, 14, 3, 27, 4, 8, 2,
		18, 3, 28, 6, 11, 1, 22, 5, 2, 3,
		12, 3, 25, 4, 6, 2, 16, 3, 26, 6,
		8, 2, 20, 1, 0, 6, 11, 2, 24, 4,
		4, 3, 15, 2, 25, 6, 8, 1, 19, 2,
		29, 6, 9, 3, 22, 4, 3, 2, 13, 3,
		25, 4, 6, 3, 17, 2, 27, 6, 7, 3,
		19, 2, 31, 4, 11, 3, 23, 4, 5, 2,
		15, 3, 25, 6, 6, 2, 19, 1, 29, 6,
		10, 2, 22, 4, 3, 3, 14, 2, 24, 6,
		6, 1, 17, 3, 28, 5, 8, 3, 20, 1,
		32, 5, 12, 3, 22, 6, 4, 1, 16, 2,
		26, 6, 6, 3, 17, 2, 0, 4, 10, 3,
		22, 4, 3, 2, 14, 3, 24, 6, 5, 2,
		17, 1, 28, 6, 9, 2, 19, 3, 31, 4,
		13, 2, 23, 6, 3, 3, 15, 1, 27, 5,
		7, 3, 17, 3, 29, 4, 11, 2, 21, 6,
		3, 1, 14, 2, 25, 6, 5, 3, 16, 2,
		28, 4, 9, 3, 20, 2, 0, 6, 12, 1,
		23, 6, 4, 2, 14, 3, 26, 4, 8, 2,
		18, 3, 0, 4, 10, 3, 21, 5, 1, 3,
		13, 1, 24, 5, 5, 3, 15, 3, 27, 4,
		8, 2, 19, 3, 29, 6, 10, 2, 22, 4,
		3, 3, 14, 2, 26, 4, 6, 3, 18, 2,
		28, 6, 10, 1, 20, 6, 2, 2, 12, 3,
		24, 4, 5, 2, 16, 3, 28, 4, 8, 3,
		19, 2, 0, 6, 12, 1, 23, 5, 3, 3,
		14, 3, 26, 4, 7, 2, 17, 3, 28, 6,
		9, 2, 21, 4, 1, 3, 13, 2, 25, 4,
		5, 3, 16, 2, 27, 6, 9, 1, 19, 3,
		0, 5, 11, 3, 23, 4, 4, 2, 14, 3,
		25, 6, 7, 1, 18, 2, 28, 6, 9, 3,
		21, 4, 2, 2, 12, 3, 25, 4, 6, 2,
		16, 3, 26, 6, 8, 2, 20, 1, 0, 6,
		11, 2, 22, 6, 4, 1, 15, 2, 25, 6,
		6, 3, 18, 1, 29, 5, 9, 3, 22, 4,
		2, 3, 13, 2, 23, 6, 4, 3, 15, 2,
		27, 4, 7, 3, 19, 2, 31, 4, 11, 3,
		21, 6, 3, 2, 15, 1, 25, 6, 6, 2,
		17, 3, 29, 4, 10, 2, 20, 6, 3, 1,
		13, 3, 24, 5, 4, 3, 16, 1, 27, 5,
		7, 3, 17, 3, 0, 4, 11, 2, 21, 6,
		1, 3, 13, 2, 25, 4, 5, 3, 16, 2,
		29, 4, 9, 3, 19, 6, 30, 2, 13, 1,
		23, 6, 4, 2, 14, 3, 27, 4, 8, 2,
		18, 3, 0, 4, 11, 3, 22, 5, 2, 3,
		14, 1, 26, 5, 6, 3, 16, 3, 28, 4,
		10, 2, 20, 6, 30, 3, 11, 2, 24, 4,
		4, 3, 15, 2, 25, 6, 8, 1, 19, 2,
		29, 6, 9, 3, 22, 4, 3, 2, 13, 3,
		25, 4, 7, 2, 17, 3, 27, 6, 9, 1,
		21, 5, 1, 3, 11, 3, 23, 4, 5, 2,
		15, 3, 25, 6, 6, 2, 19, 1, 29, 6,
		10, 2, 22, 4, 3, 3, 14, 2, 24, 6,
		6, 1, 18, 2, 28, 6, 8, 3, 20, 4,
		2, 2, 12, 3, 24, 4, 4, 3, 16, 2,
		26, 6, 6, 3, 17, 2, 0, 4, 10, 3,
		22, 4, 3, 2, 14, 3, 24, 6, 5, 2,
		17, 1, 28, 6, 9, 2, 21, 4, 1, 3,
		13, 2, 23, 6, 5, 1, 15, 3, 27, 5,
		7, 3, 19, 1, 0, 5, 10, 3, 22, 4,
		2, 3, 13, 2, 24, 6, 4, 3, 15, 2,
		27, 4, 8, 3, 20, 4, 1, 2, 11, 3,
		22, 6, 3, 2, 15, 1, 25, 6, 7, 2,
		17, 3, 29, 4, 10, 2, 21, 6, 1, 3,
		13, 1, 24, 5, 5, 3, 15, 3, 27, 4,
		8, 2, 19, 6, 1, 1, 12, 2, 22, 6,
		3, 3, 14, 2, 26, 4, 6, 3, 18, 2,
		28, 6, 10, 1, 20, 6, 2, 2, 12, 3,
		24, 4, 5, 2, 16, 3, 28, 4, 9, 2,
		19, 6, 30, 3, 12, 1, 23, 5, 3, 3,
		14, 3, 26, 4, 7, 2, 17, 3, 28, 6,
		9, 2, 21, 4, 1, 3, 13, 2, 25, 4,
		5, 3, 16, 2, 27, 6, 9, 1, 19, 6,
		30, 2, 11, 3, 23, 4, 4, 2, 14, 3,
		27, 4, 7, 3, 18, 2, 28, 6, 11, 1,
		22, 5, 2, 3, 12, 3, 25, 4, 6, 2,
		16, 3, 26, 6, 8, 2, 20, 4, 30, 3,
		11, 2, 24, 4, 4, 3, 15, 2, 25, 6,
		8, 1, 18, 3, 29, 5, 9, 3, 22, 4,
		3, 2, 13, 3, 23, 6, 6, 1, 17, 2,
		27, 6, 7, 3, 20, 4, 1, 2, 11, 3,
		23, 4, 5, 2, 15, 3, 25, 6, 6, 2,
		19, 1, 29, 6, 10, 2, 20, 6, 3, 1,
		14, 2, 24, 6, 4, 3, 17, 1, 28, 5,
		8, 3, 20, 4, 1, 3, 12, 2, 22, 6,
		2, 3, 14, 2, 26, 4, 6, 3, 17, 2,
		0, 4, 10, 3, 20, 6, 1, 2, 14, 1,
		24, 6, 5, 2, 15, 3, 28, 4, 9, 2,
		19, 6, 1, 1, 12, 3, 23, 5, 3, 3,
		15, 1, 27, 5, 7, 3, 17, 3, 29, 4,
		11, 2, 21, 6, 1, 3, 12, 2, 25, 4,
		5, 3, 16, 2, 28, 4, 9, 3, 19, 6,
		30, 2, 12, 1, 23, 6, 4, 2, 14, 3,
		26, 4, 8, 2, 18, 3, 0, 4, 10, 3,
		22, 5, 2, 3, 14, 1, 25, 5, 6, 3,
		16, 3, 28, 4, 9, 2, 20, 6, 30, 3,
		11, 2, 23, 4, 4, 3, 15, 2, 27, 4,
		7, 3, 19, 2, 29, 6, 11, 1, 21, 6,
		3, 2, 13, 3, 25, 4, 6, 2, 17, 3,
		27, 6, 9, 1, 20, 5, 30, 3, 10, 3,
		22, 4, 3, 2, 14, 3, 24, 6, 5, 2,
		17, 1, 28, 6, 9, 2, 21, 4, 1, 3,
		13, 2, 23, 6, 5, 1, 16, 2, 27, 6,
		7, 3, 19, 4, 30, 2, 11, 3, 23, 4,
		3, 3, 14, 2, 25, 6, 5, 3, 16, 2,
		28, 4, 9, 3, 21, 4, 2, 2, 12, 3,
		23, 6, 4, 2, 16, 1, 26, 6, 8, 2,
		20, 4, 30, 3, 11, 2, 22, 6, 4, 1,
		14, 3, 25, 5, 6, 3, 18, 1, 29, 5,
		9, 3, 22, 4, 2, 3, 13, 2, 23, 6,
		4, 3, 15, 2, 27, 4, 7, 3, 20, 4,
		1, 2, 11, 3, 21, 6, 3, 2, 15, 1,
		25, 6, 6, 2, 17, 3, 29, 4, 10, 2,
		20, 6, 3, 1, 13, 3, 24, 5, 4, 3,
		17, 1, 28, 5, 8, 3, 18, 6, 1, 1,
		12, 2, 22, 6, 2, 3, 14, 2, 26, 4,
		6, 3, 17, 2, 28, 6, 10, 1, 20, 6,
		1, 2, 12, 3, 24, 4, 5, 2, 15, 3,
		28, 4, 9, 2, 19, 6, 33, 3, 12, 1,
		23, 5, 3, 3, 13, 3, 25, 4, 6, 2,
		16, 3, 26, 6, 8, 2, 20, 4, 30, 3,
		11, 2, 24, 4, 4, 3, 15, 2, 25, 6,
		8, 1, 18, 6, 33, 2, 9, 3, 22, 4,
		3, 2, 13, 3, 25, 4, 6, 3, 17, 2,
		27, 6, 9, 1, 21, 5, 1, 3, 11, 3,
		23, 4, 5, 2, 15, 3, 25, 6, 6, 2,
		19, 4, 33, 3, 10, 2, 22, 4, 3, 3,
		14, 2, 24, 6, 6, 1
	};

	private static readonly int[,] LunarMonthLen = new int[7, 14]
	{
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0
		},
		{
			0, 30, 29, 29, 29, 30, 29, 30, 29, 30,
			29, 30, 29, 0
		},
		{
			0, 30, 29, 30, 29, 30, 29, 30, 29, 30,
			29, 30, 29, 0
		},
		{
			0, 30, 30, 30, 29, 30, 29, 30, 29, 30,
			29, 30, 29, 0
		},
		{
			0, 30, 29, 29, 29, 30, 30, 29, 30, 29,
			30, 29, 30, 29
		},
		{
			0, 30, 29, 30, 29, 30, 30, 29, 30, 29,
			30, 29, 30, 29
		},
		{
			0, 30, 30, 30, 29, 30, 30, 29, 30, 29,
			30, 29, 30, 29
		}
	};

	internal static readonly DateTime calendarMinValue = new DateTime(1583, 1, 1);

	internal static readonly DateTime calendarMaxValue = new DateTime(new DateTime(2239, 9, 29, 23, 59, 59, 999).Ticks + 9999);

	private const int DEFAULT_TWO_DIGIT_YEAR_MAX = 5790;

	public override DateTime MinSupportedDateTime => calendarMinValue;

	public override DateTime MaxSupportedDateTime => calendarMaxValue;

	public override CalendarAlgorithmType AlgorithmType => CalendarAlgorithmType.LunisolarCalendar;

	internal override int ID => 8;

	public override int[] Eras => new int[1] { HebrewEra };

	public override int TwoDigitYearMax
	{
		get
		{
			if (twoDigitYearMax == -1)
			{
				twoDigitYearMax = Calendar.GetSystemTwoDigitYearSetting(ID, 5790);
			}
			return twoDigitYearMax;
		}
		set
		{
			VerifyWritable();
			if (value != 99)
			{
				CheckHebrewYearValue(value, HebrewEra, "value");
			}
			twoDigitYearMax = value;
		}
	}

	private static void CheckHebrewYearValue(int y, int era, string varName)
	{
		CheckEraRange(era);
		if (y > 5999 || y < 5343)
		{
			throw new ArgumentOutOfRangeException(varName, string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 5343, 5999));
		}
	}

	private void CheckHebrewMonthValue(int year, int month, int era)
	{
		int monthsInYear = GetMonthsInYear(year, era);
		if (month < 1 || month > monthsInYear)
		{
			throw new ArgumentOutOfRangeException("month", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, monthsInYear));
		}
	}

	private void CheckHebrewDayValue(int year, int month, int day, int era)
	{
		int daysInMonth = GetDaysInMonth(year, month, era);
		if (day < 1 || day > daysInMonth)
		{
			throw new ArgumentOutOfRangeException("day", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 1, daysInMonth));
		}
	}

	internal static void CheckEraRange(int era)
	{
		if (era != 0 && era != HebrewEra)
		{
			throw new ArgumentOutOfRangeException("era", Environment.GetResourceString("Era value was not valid."));
		}
	}

	private static void CheckTicksRange(long ticks)
	{
		if (ticks < calendarMinValue.Ticks || ticks > calendarMaxValue.Ticks)
		{
			throw new ArgumentOutOfRangeException("time", string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Specified time is not supported in this calendar. It should be between {0} (Gregorian date) and {1} (Gregorian date), inclusive."), calendarMinValue, calendarMaxValue));
		}
	}

	internal static int GetResult(__DateBuffer result, int part)
	{
		return part switch
		{
			0 => result.year, 
			2 => result.month, 
			3 => result.day, 
			_ => throw new InvalidOperationException(Environment.GetResourceString("Internal Error in DateTime and Calendar operations.")), 
		};
	}

	internal static int GetLunarMonthDay(int gregorianYear, __DateBuffer lunarDate)
	{
		int num = gregorianYear - 1583;
		if (num < 0 || num > 656)
		{
			throw new ArgumentOutOfRangeException("gregorianYear");
		}
		num *= 2;
		lunarDate.day = HebrewTable[num];
		int result = HebrewTable[num + 1];
		switch (lunarDate.day)
		{
		case 0:
			lunarDate.month = 5;
			lunarDate.day = 1;
			break;
		case 30:
			lunarDate.month = 3;
			break;
		case 31:
			lunarDate.month = 5;
			lunarDate.day = 2;
			break;
		case 32:
			lunarDate.month = 5;
			lunarDate.day = 3;
			break;
		case 33:
			lunarDate.month = 3;
			lunarDate.day = 29;
			break;
		default:
			lunarDate.month = 4;
			break;
		}
		return result;
	}

	internal virtual int GetDatePart(long ticks, int part)
	{
		CheckTicksRange(ticks);
		DateTime dateTime = new DateTime(ticks);
		int year = dateTime.Year;
		int month = dateTime.Month;
		int day = dateTime.Day;
		__DateBuffer _DateBuffer = new __DateBuffer();
		_DateBuffer.year = year + 3760;
		int num = GetLunarMonthDay(year, _DateBuffer);
		__DateBuffer _DateBuffer2 = new __DateBuffer();
		_DateBuffer2.year = _DateBuffer.year;
		_DateBuffer2.month = _DateBuffer.month;
		_DateBuffer2.day = _DateBuffer.day;
		long absoluteDate = GregorianCalendar.GetAbsoluteDate(year, month, day);
		if (month == 1 && day == 1)
		{
			return GetResult(_DateBuffer2, part);
		}
		long num2 = absoluteDate - GregorianCalendar.GetAbsoluteDate(year, 1, 1);
		if (num2 + _DateBuffer.day <= LunarMonthLen[num, _DateBuffer.month])
		{
			_DateBuffer2.day += (int)num2;
			return GetResult(_DateBuffer2, part);
		}
		_DateBuffer2.month++;
		_DateBuffer2.day = 1;
		num2 -= LunarMonthLen[num, _DateBuffer.month] - _DateBuffer.day;
		if (num2 > 1)
		{
			while (num2 > LunarMonthLen[num, _DateBuffer2.month])
			{
				num2 -= LunarMonthLen[num, _DateBuffer2.month++];
				if (_DateBuffer2.month > 13 || LunarMonthLen[num, _DateBuffer2.month] == 0)
				{
					_DateBuffer2.year++;
					num = HebrewTable[(year + 1 - 1583) * 2 + 1];
					_DateBuffer2.month = 1;
				}
			}
			_DateBuffer2.day += (int)(num2 - 1);
		}
		return GetResult(_DateBuffer2, part);
	}

	public override DateTime AddMonths(DateTime time, int months)
	{
		try
		{
			int num = GetDatePart(time.Ticks, 0);
			int datePart = GetDatePart(time.Ticks, 2);
			int num2 = GetDatePart(time.Ticks, 3);
			int num3;
			if (months >= 0)
			{
				int monthsInYear;
				for (num3 = datePart + months; num3 > (monthsInYear = GetMonthsInYear(num, 0)); num3 -= monthsInYear)
				{
					num++;
				}
			}
			else if ((num3 = datePart + months) <= 0)
			{
				months = -months;
				months -= datePart;
				num--;
				int monthsInYear;
				while (months > (monthsInYear = GetMonthsInYear(num, 0)))
				{
					num--;
					months -= monthsInYear;
				}
				monthsInYear = GetMonthsInYear(num, 0);
				num3 = monthsInYear - months;
			}
			int daysInMonth = GetDaysInMonth(num, num3);
			if (num2 > daysInMonth)
			{
				num2 = daysInMonth;
			}
			return new DateTime(ToDateTime(num, num3, num2, 0, 0, 0, 0).Ticks + time.Ticks % 864000000000L);
		}
		catch (ArgumentException)
		{
			throw new ArgumentOutOfRangeException("months", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Value to add was out of range.")));
		}
	}

	public override DateTime AddYears(DateTime time, int years)
	{
		int datePart = GetDatePart(time.Ticks, 0);
		int num = GetDatePart(time.Ticks, 2);
		int num2 = GetDatePart(time.Ticks, 3);
		datePart += years;
		CheckHebrewYearValue(datePart, 0, "years");
		int monthsInYear = GetMonthsInYear(datePart, 0);
		if (num > monthsInYear)
		{
			num = monthsInYear;
		}
		int daysInMonth = GetDaysInMonth(datePart, num);
		if (num2 > daysInMonth)
		{
			num2 = daysInMonth;
		}
		long ticks = ToDateTime(datePart, num, num2, 0, 0, 0, 0).Ticks + time.Ticks % 864000000000L;
		Calendar.CheckAddResult(ticks, MinSupportedDateTime, MaxSupportedDateTime);
		return new DateTime(ticks);
	}

	public override int GetDayOfMonth(DateTime time)
	{
		return GetDatePart(time.Ticks, 3);
	}

	public override DayOfWeek GetDayOfWeek(DateTime time)
	{
		return (DayOfWeek)((int)(time.Ticks / 864000000000L + 1) % 7);
	}

	internal static int GetHebrewYearType(int year, int era)
	{
		CheckHebrewYearValue(year, era, "year");
		return HebrewTable[(year - 3760 - 1583) * 2 + 1];
	}

	public override int GetDayOfYear(DateTime time)
	{
		int year = GetYear(time);
		DateTime dateTime = ((year != 5343) ? ToDateTime(year, 1, 1, 0, 0, 0, 0, 0) : new DateTime(1582, 9, 27));
		return (int)((time.Ticks - dateTime.Ticks) / 864000000000L) + 1;
	}

	public override int GetDaysInMonth(int year, int month, int era)
	{
		CheckEraRange(era);
		int hebrewYearType = GetHebrewYearType(year, era);
		CheckHebrewMonthValue(year, month, era);
		int num = LunarMonthLen[hebrewYearType, month];
		if (num == 0)
		{
			throw new ArgumentOutOfRangeException("month", Environment.GetResourceString("Month must be between one and twelve."));
		}
		return num;
	}

	public override int GetDaysInYear(int year, int era)
	{
		CheckEraRange(era);
		int hebrewYearType = GetHebrewYearType(year, era);
		if (hebrewYearType < 4)
		{
			return 352 + hebrewYearType;
		}
		return 382 + (hebrewYearType - 3);
	}

	public override int GetEra(DateTime time)
	{
		return HebrewEra;
	}

	public override int GetMonth(DateTime time)
	{
		return GetDatePart(time.Ticks, 2);
	}

	public override int GetMonthsInYear(int year, int era)
	{
		if (!IsLeapYear(year, era))
		{
			return 12;
		}
		return 13;
	}

	public override int GetYear(DateTime time)
	{
		return GetDatePart(time.Ticks, 0);
	}

	public override bool IsLeapDay(int year, int month, int day, int era)
	{
		if (IsLeapMonth(year, month, era))
		{
			CheckHebrewDayValue(year, month, day, era);
			return true;
		}
		if (IsLeapYear(year, 0) && month == 6 && day == 30)
		{
			return true;
		}
		CheckHebrewDayValue(year, month, day, era);
		return false;
	}

	public override int GetLeapMonth(int year, int era)
	{
		if (IsLeapYear(year, era))
		{
			return 7;
		}
		return 0;
	}

	public override bool IsLeapMonth(int year, int month, int era)
	{
		bool num = IsLeapYear(year, era);
		CheckHebrewMonthValue(year, month, era);
		if (num && month == 7)
		{
			return true;
		}
		return false;
	}

	public override bool IsLeapYear(int year, int era)
	{
		CheckHebrewYearValue(year, era, "year");
		return (7L * (long)year + 1) % 19 < 7;
	}

	private static int GetDayDifference(int lunarYearType, int month1, int day1, int month2, int day2)
	{
		if (month1 == month2)
		{
			return day1 - day2;
		}
		bool flag = month1 > month2;
		if (flag)
		{
			int num = month1;
			int num2 = day1;
			month1 = month2;
			day1 = day2;
			month2 = num;
			day2 = num2;
		}
		int num3 = LunarMonthLen[lunarYearType, month1] - day1;
		month1++;
		while (month1 < month2)
		{
			num3 += LunarMonthLen[lunarYearType, month1++];
		}
		num3 += day2;
		if (!flag)
		{
			return -num3;
		}
		return num3;
	}

	private static DateTime HebrewToGregorian(int hebrewYear, int hebrewMonth, int hebrewDay, int hour, int minute, int second, int millisecond)
	{
		int num = hebrewYear - 3760;
		__DateBuffer _DateBuffer = new __DateBuffer();
		int lunarMonthDay = GetLunarMonthDay(num, _DateBuffer);
		if (hebrewMonth == _DateBuffer.month && hebrewDay == _DateBuffer.day)
		{
			return new DateTime(num, 1, 1, hour, minute, second, millisecond);
		}
		int dayDifference = GetDayDifference(lunarMonthDay, hebrewMonth, hebrewDay, _DateBuffer.month, _DateBuffer.day);
		return new DateTime(new DateTime(num, 1, 1).Ticks + dayDifference * 864000000000L + Calendar.TimeToTicks(hour, minute, second, millisecond));
	}

	public override DateTime ToDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, int era)
	{
		CheckHebrewYearValue(year, era, "year");
		CheckHebrewMonthValue(year, month, era);
		CheckHebrewDayValue(year, month, day, era);
		DateTime result = HebrewToGregorian(year, month, day, hour, minute, second, millisecond);
		CheckTicksRange(result.Ticks);
		return result;
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
		if (year > 5999 || year < 5343)
		{
			throw new ArgumentOutOfRangeException("year", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Valid values are between {0} and {1}, inclusive."), 5343, 5999));
		}
		return year;
	}
}
