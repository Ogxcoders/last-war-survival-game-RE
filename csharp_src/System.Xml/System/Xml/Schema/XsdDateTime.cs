using System.Text;

namespace System.Xml.Schema;

internal struct XsdDateTime
{
	private enum DateTimeTypeCode
	{
		DateTime,
		Time,
		Date,
		GYearMonth,
		GYear,
		GMonthDay,
		GDay,
		GMonth,
		XdrDateTime
	}

	private enum XsdDateTimeKind
	{
		Unspecified,
		Zulu,
		LocalWestOfZulu,
		LocalEastOfZulu
	}

	private struct Parser
	{
		private const int leapYear = 1904;

		private const int firstMonth = 1;

		private const int firstDay = 1;

		public DateTimeTypeCode typeCode;

		public int year;

		public int month;

		public int day;

		public int hour;

		public int minute;

		public int second;

		public int fraction;

		public XsdDateTimeKind kind;

		public int zoneHour;

		public int zoneMinute;

		private string text;

		private int length;

		private static int[] Power10 = new int[7] { -1, 10, 100, 1000, 10000, 100000, 1000000 };

		public bool Parse(string text, XsdDateTimeFlags kinds)
		{
			this.text = text;
			length = text.Length;
			int i;
			for (i = 0; i < length && char.IsWhiteSpace(text[i]); i++)
			{
			}
			if (Test(kinds, XsdDateTimeFlags.DateTime | XsdDateTimeFlags.Date | XsdDateTimeFlags.XdrDateTimeNoTz | XsdDateTimeFlags.XdrDateTime) && ParseDate(i))
			{
				if (Test(kinds, XsdDateTimeFlags.DateTime) && ParseChar(i + Lzyyyy_MM_dd, 'T') && ParseTimeAndZoneAndWhitespace(i + Lzyyyy_MM_ddT))
				{
					typeCode = DateTimeTypeCode.DateTime;
					return true;
				}
				if (Test(kinds, XsdDateTimeFlags.Date) && ParseZoneAndWhitespace(i + Lzyyyy_MM_dd))
				{
					typeCode = DateTimeTypeCode.Date;
					return true;
				}
				if (Test(kinds, XsdDateTimeFlags.XdrDateTime) && (ParseZoneAndWhitespace(i + Lzyyyy_MM_dd) || (ParseChar(i + Lzyyyy_MM_dd, 'T') && ParseTimeAndZoneAndWhitespace(i + Lzyyyy_MM_ddT))))
				{
					typeCode = DateTimeTypeCode.XdrDateTime;
					return true;
				}
				if (Test(kinds, XsdDateTimeFlags.XdrDateTimeNoTz))
				{
					if (!ParseChar(i + Lzyyyy_MM_dd, 'T'))
					{
						typeCode = DateTimeTypeCode.XdrDateTime;
						return true;
					}
					if (ParseTimeAndWhitespace(i + Lzyyyy_MM_ddT))
					{
						typeCode = DateTimeTypeCode.XdrDateTime;
						return true;
					}
				}
			}
			if (Test(kinds, XsdDateTimeFlags.Time) && ParseTimeAndZoneAndWhitespace(i))
			{
				year = 1904;
				month = 1;
				day = 1;
				typeCode = DateTimeTypeCode.Time;
				return true;
			}
			if (Test(kinds, XsdDateTimeFlags.XdrTimeNoTz) && ParseTimeAndWhitespace(i))
			{
				year = 1904;
				month = 1;
				day = 1;
				typeCode = DateTimeTypeCode.Time;
				return true;
			}
			if (Test(kinds, XsdDateTimeFlags.GYearMonth | XsdDateTimeFlags.GYear) && Parse4Dig(i, ref year) && 1 <= year)
			{
				if (Test(kinds, XsdDateTimeFlags.GYearMonth) && ParseChar(i + Lzyyyy, '-') && Parse2Dig(i + Lzyyyy_, ref month) && 1 <= month && month <= 12 && ParseZoneAndWhitespace(i + Lzyyyy_MM))
				{
					day = 1;
					typeCode = DateTimeTypeCode.GYearMonth;
					return true;
				}
				if (Test(kinds, XsdDateTimeFlags.GYear) && ParseZoneAndWhitespace(i + Lzyyyy))
				{
					month = 1;
					day = 1;
					typeCode = DateTimeTypeCode.GYear;
					return true;
				}
			}
			if (Test(kinds, XsdDateTimeFlags.GMonthDay | XsdDateTimeFlags.GMonth) && ParseChar(i, '-') && ParseChar(i + Lz_, '-') && Parse2Dig(i + Lz__, ref month) && 1 <= month && month <= 12)
			{
				if (Test(kinds, XsdDateTimeFlags.GMonthDay) && ParseChar(i + Lz__mm, '-') && Parse2Dig(i + Lz__mm_, ref day) && 1 <= day && day <= DateTime.DaysInMonth(1904, month) && ParseZoneAndWhitespace(i + Lz__mm_dd))
				{
					year = 1904;
					typeCode = DateTimeTypeCode.GMonthDay;
					return true;
				}
				if (Test(kinds, XsdDateTimeFlags.GMonth) && (ParseZoneAndWhitespace(i + Lz__mm) || (ParseChar(i + Lz__mm, '-') && ParseChar(i + Lz__mm_, '-') && ParseZoneAndWhitespace(i + Lz__mm__))))
				{
					year = 1904;
					day = 1;
					typeCode = DateTimeTypeCode.GMonth;
					return true;
				}
			}
			if (Test(kinds, XsdDateTimeFlags.GDay) && ParseChar(i, '-') && ParseChar(i + Lz_, '-') && ParseChar(i + Lz__, '-') && Parse2Dig(i + Lz___, ref day) && 1 <= day && day <= DateTime.DaysInMonth(1904, 1) && ParseZoneAndWhitespace(i + Lz___dd))
			{
				year = 1904;
				month = 1;
				typeCode = DateTimeTypeCode.GDay;
				return true;
			}
			return false;
		}

		private bool ParseDate(int start)
		{
			if (Parse4Dig(start, ref year) && 1 <= year && ParseChar(start + Lzyyyy, '-') && Parse2Dig(start + Lzyyyy_, ref month) && 1 <= month && month <= 12 && ParseChar(start + Lzyyyy_MM, '-') && Parse2Dig(start + Lzyyyy_MM_, ref day) && 1 <= day)
			{
				return day <= DateTime.DaysInMonth(year, month);
			}
			return false;
		}

		private bool ParseTimeAndZoneAndWhitespace(int start)
		{
			if (ParseTime(ref start) && ParseZoneAndWhitespace(start))
			{
				return true;
			}
			return false;
		}

		private bool ParseTimeAndWhitespace(int start)
		{
			if (ParseTime(ref start))
			{
				while (start < length)
				{
					start++;
				}
				return start == length;
			}
			return false;
		}

		private bool ParseTime(ref int start)
		{
			if (Parse2Dig(start, ref hour) && hour < 24 && ParseChar(start + LzHH, ':') && Parse2Dig(start + LzHH_, ref minute) && minute < 60 && ParseChar(start + LzHH_mm, ':') && Parse2Dig(start + LzHH_mm_, ref second) && second < 60)
			{
				start += LzHH_mm_ss;
				if (ParseChar(start, '.'))
				{
					fraction = 0;
					int num = 0;
					int num2 = 0;
					while (++start < length)
					{
						int num3 = text[start] - 48;
						if (9u < (uint)num3)
						{
							break;
						}
						if (num < 7)
						{
							fraction = fraction * 10 + num3;
						}
						else if (num == 7)
						{
							if (5 < num3)
							{
								num2 = 1;
							}
							else if (num3 == 5)
							{
								num2 = -1;
							}
						}
						else if (num2 < 0 && num3 != 0)
						{
							num2 = 1;
						}
						num++;
					}
					if (num < 7)
					{
						if (num == 0)
						{
							return false;
						}
						fraction *= Power10[7 - num];
					}
					else
					{
						if (num2 < 0)
						{
							num2 = fraction & 1;
						}
						fraction += num2;
					}
				}
				return true;
			}
			hour = 0;
			return false;
		}

		private bool ParseZoneAndWhitespace(int start)
		{
			if (start < length)
			{
				char c = text[start];
				if (c == 'Z' || c == 'z')
				{
					kind = XsdDateTimeKind.Zulu;
					start++;
				}
				else if (start + 5 < length && Parse2Dig(start + Lz_, ref zoneHour) && zoneHour <= 99 && ParseChar(start + Lz_zz, ':') && Parse2Dig(start + Lz_zz_, ref zoneMinute) && zoneMinute <= 99)
				{
					switch (c)
					{
					case '-':
						kind = XsdDateTimeKind.LocalWestOfZulu;
						start += Lz_zz_zz;
						break;
					case '+':
						kind = XsdDateTimeKind.LocalEastOfZulu;
						start += Lz_zz_zz;
						break;
					}
				}
			}
			while (start < length && char.IsWhiteSpace(text[start]))
			{
				start++;
			}
			return start == length;
		}

		private bool Parse4Dig(int start, ref int num)
		{
			if (start + 3 < length)
			{
				int num2 = text[start] - 48;
				int num3 = text[start + 1] - 48;
				int num4 = text[start + 2] - 48;
				int num5 = text[start + 3] - 48;
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10 && 0 <= num4 && num4 < 10 && 0 <= num5 && num5 < 10)
				{
					num = ((num2 * 10 + num3) * 10 + num4) * 10 + num5;
					return true;
				}
			}
			return false;
		}

		private bool Parse2Dig(int start, ref int num)
		{
			if (start + 1 < length)
			{
				int num2 = text[start] - 48;
				int num3 = text[start + 1] - 48;
				if (0 <= num2 && num2 < 10 && 0 <= num3 && num3 < 10)
				{
					num = num2 * 10 + num3;
					return true;
				}
			}
			return false;
		}

		private bool ParseChar(int start, char ch)
		{
			if (start < length)
			{
				return text[start] == ch;
			}
			return false;
		}

		private static bool Test(XsdDateTimeFlags left, XsdDateTimeFlags right)
		{
			return (left & right) != 0;
		}
	}

	private DateTime dt;

	private uint extra;

	private const uint TypeMask = 4278190080u;

	private const uint KindMask = 16711680u;

	private const uint ZoneHourMask = 65280u;

	private const uint ZoneMinuteMask = 255u;

	private const int TypeShift = 24;

	private const int KindShift = 16;

	private const int ZoneHourShift = 8;

	private const short maxFractionDigits = 7;

	private static readonly int Lzyyyy = "yyyy".Length;

	private static readonly int Lzyyyy_ = "yyyy-".Length;

	private static readonly int Lzyyyy_MM = "yyyy-MM".Length;

	private static readonly int Lzyyyy_MM_ = "yyyy-MM-".Length;

	private static readonly int Lzyyyy_MM_dd = "yyyy-MM-dd".Length;

	private static readonly int Lzyyyy_MM_ddT = "yyyy-MM-ddT".Length;

	private static readonly int LzHH = "HH".Length;

	private static readonly int LzHH_ = "HH:".Length;

	private static readonly int LzHH_mm = "HH:mm".Length;

	private static readonly int LzHH_mm_ = "HH:mm:".Length;

	private static readonly int LzHH_mm_ss = "HH:mm:ss".Length;

	private static readonly int Lz_ = "-".Length;

	private static readonly int Lz_zz = "-zz".Length;

	private static readonly int Lz_zz_ = "-zz:".Length;

	private static readonly int Lz_zz_zz = "-zz:zz".Length;

	private static readonly int Lz__ = "--".Length;

	private static readonly int Lz__mm = "--MM".Length;

	private static readonly int Lz__mm_ = "--MM-".Length;

	private static readonly int Lz__mm__ = "--MM--".Length;

	private static readonly int Lz__mm_dd = "--MM-dd".Length;

	private static readonly int Lz___ = "---".Length;

	private static readonly int Lz___dd = "---dd".Length;

	private static readonly XmlTypeCode[] typeCodes = new XmlTypeCode[8]
	{
		XmlTypeCode.DateTime,
		XmlTypeCode.Time,
		XmlTypeCode.Date,
		XmlTypeCode.GYearMonth,
		XmlTypeCode.GYear,
		XmlTypeCode.GMonthDay,
		XmlTypeCode.GDay,
		XmlTypeCode.GMonth
	};

	private DateTimeTypeCode InternalTypeCode => (DateTimeTypeCode)((extra & 0xFF000000u) >> 24);

	private XsdDateTimeKind InternalKind => (XsdDateTimeKind)((extra & 0xFF0000) >> 16);

	public XmlTypeCode TypeCode => typeCodes[(int)InternalTypeCode];

	public DateTimeKind Kind => InternalKind switch
	{
		XsdDateTimeKind.Unspecified => DateTimeKind.Unspecified, 
		XsdDateTimeKind.Zulu => DateTimeKind.Utc, 
		_ => DateTimeKind.Local, 
	};

	public int Year => dt.Year;

	public int Month => dt.Month;

	public int Day => dt.Day;

	public int Hour => dt.Hour;

	public int Minute => dt.Minute;

	public int Second => dt.Second;

	public int Fraction => (int)(dt.Ticks - new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second).Ticks);

	public int ZoneHour => (int)((extra & 0xFF00) >> 8);

	public int ZoneMinute => (int)(extra & 0xFF);

	public XsdDateTime(string text)
		: this(text, XsdDateTimeFlags.AllXsd)
	{
	}

	public XsdDateTime(string text, XsdDateTimeFlags kinds)
	{
		this = default(XsdDateTime);
		Parser parser = default(Parser);
		if (!parser.Parse(text, kinds))
		{
			throw new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", text, kinds));
		}
		InitiateXsdDateTime(parser);
	}

	private XsdDateTime(Parser parser)
	{
		this = default(XsdDateTime);
		InitiateXsdDateTime(parser);
	}

	private void InitiateXsdDateTime(Parser parser)
	{
		dt = new DateTime(parser.year, parser.month, parser.day, parser.hour, parser.minute, parser.second);
		if (parser.fraction != 0)
		{
			dt = dt.AddTicks(parser.fraction);
		}
		extra = (uint)(((int)parser.typeCode << 24) | ((int)parser.kind << 16) | (parser.zoneHour << 8) | parser.zoneMinute);
	}

	internal static bool TryParse(string text, XsdDateTimeFlags kinds, out XsdDateTime result)
	{
		Parser parser = default(Parser);
		if (!parser.Parse(text, kinds))
		{
			result = default(XsdDateTime);
			return false;
		}
		result = new XsdDateTime(parser);
		return true;
	}

	public XsdDateTime(DateTime dateTime, XsdDateTimeFlags kinds)
	{
		dt = dateTime;
		DateTimeTypeCode dateTimeTypeCode = (DateTimeTypeCode)(Bits.LeastPosition((uint)kinds) - 1);
		int num = 0;
		int num2 = 0;
		XsdDateTimeKind xsdDateTimeKind;
		switch (dateTime.Kind)
		{
		case DateTimeKind.Unspecified:
			xsdDateTimeKind = XsdDateTimeKind.Unspecified;
			break;
		case DateTimeKind.Utc:
			xsdDateTimeKind = XsdDateTimeKind.Zulu;
			break;
		default:
		{
			TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
			if (utcOffset.Ticks < 0)
			{
				xsdDateTimeKind = XsdDateTimeKind.LocalWestOfZulu;
				num = -utcOffset.Hours;
				num2 = -utcOffset.Minutes;
			}
			else
			{
				xsdDateTimeKind = XsdDateTimeKind.LocalEastOfZulu;
				num = utcOffset.Hours;
				num2 = utcOffset.Minutes;
			}
			break;
		}
		}
		extra = (uint)(((int)dateTimeTypeCode << 24) | ((int)xsdDateTimeKind << 16) | (num << 8) | num2);
	}

	public XsdDateTime(DateTimeOffset dateTimeOffset)
		: this(dateTimeOffset, XsdDateTimeFlags.DateTime)
	{
	}

	public XsdDateTime(DateTimeOffset dateTimeOffset, XsdDateTimeFlags kinds)
	{
		dt = dateTimeOffset.DateTime;
		TimeSpan timeSpan = dateTimeOffset.Offset;
		DateTimeTypeCode dateTimeTypeCode = (DateTimeTypeCode)(Bits.LeastPosition((uint)kinds) - 1);
		XsdDateTimeKind xsdDateTimeKind;
		if (!(timeSpan.TotalMinutes < 0.0))
		{
			xsdDateTimeKind = ((!(timeSpan.TotalMinutes > 0.0)) ? XsdDateTimeKind.Zulu : XsdDateTimeKind.LocalEastOfZulu);
		}
		else
		{
			timeSpan = timeSpan.Negate();
			xsdDateTimeKind = XsdDateTimeKind.LocalWestOfZulu;
		}
		extra = (uint)(((int)dateTimeTypeCode << 24) | ((int)xsdDateTimeKind << 16) | (timeSpan.Hours << 8) | timeSpan.Minutes);
	}

	public DateTime ToZulu()
	{
		return InternalKind switch
		{
			XsdDateTimeKind.Zulu => new DateTime(dt.Ticks, DateTimeKind.Utc), 
			XsdDateTimeKind.LocalEastOfZulu => new DateTime(dt.Subtract(new TimeSpan(ZoneHour, ZoneMinute, 0)).Ticks, DateTimeKind.Utc), 
			XsdDateTimeKind.LocalWestOfZulu => new DateTime(dt.Add(new TimeSpan(ZoneHour, ZoneMinute, 0)).Ticks, DateTimeKind.Utc), 
			_ => dt, 
		};
	}

	public static implicit operator DateTime(XsdDateTime xdt)
	{
		DateTime dateTime;
		switch (xdt.InternalTypeCode)
		{
		case DateTimeTypeCode.GDay:
		case DateTimeTypeCode.GMonth:
			dateTime = new DateTime(DateTime.Now.Year, xdt.Month, xdt.Day);
			break;
		case DateTimeTypeCode.Time:
		{
			DateTime now = DateTime.Now;
			TimeSpan value = new DateTime(now.Year, now.Month, now.Day) - new DateTime(xdt.Year, xdt.Month, xdt.Day);
			dateTime = xdt.dt.Add(value);
			break;
		}
		default:
			dateTime = xdt.dt;
			break;
		}
		switch (xdt.InternalKind)
		{
		case XsdDateTimeKind.Zulu:
			dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Utc);
			break;
		case XsdDateTimeKind.LocalEastOfZulu:
		{
			long num = dateTime.Ticks - new TimeSpan(xdt.ZoneHour, xdt.ZoneMinute, 0).Ticks;
			if (num < DateTime.MinValue.Ticks)
			{
				num += TimeZoneInfo.Local.GetUtcOffset(dateTime).Ticks;
				if (num < DateTime.MinValue.Ticks)
				{
					num = DateTime.MinValue.Ticks;
				}
				return new DateTime(num, DateTimeKind.Local);
			}
			dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
			break;
		}
		case XsdDateTimeKind.LocalWestOfZulu:
		{
			long num = dateTime.Ticks + new TimeSpan(xdt.ZoneHour, xdt.ZoneMinute, 0).Ticks;
			if (num > DateTime.MaxValue.Ticks)
			{
				num += TimeZoneInfo.Local.GetUtcOffset(dateTime).Ticks;
				if (num > DateTime.MaxValue.Ticks)
				{
					num = DateTime.MaxValue.Ticks;
				}
				return new DateTime(num, DateTimeKind.Local);
			}
			dateTime = new DateTime(num, DateTimeKind.Utc).ToLocalTime();
			break;
		}
		}
		return dateTime;
	}

	public static implicit operator DateTimeOffset(XsdDateTime xdt)
	{
		DateTime dateTime;
		switch (xdt.InternalTypeCode)
		{
		case DateTimeTypeCode.GDay:
		case DateTimeTypeCode.GMonth:
			dateTime = new DateTime(DateTime.Now.Year, xdt.Month, xdt.Day);
			break;
		case DateTimeTypeCode.Time:
		{
			DateTime now = DateTime.Now;
			TimeSpan value = new DateTime(now.Year, now.Month, now.Day) - new DateTime(xdt.Year, xdt.Month, xdt.Day);
			dateTime = xdt.dt.Add(value);
			break;
		}
		default:
			dateTime = xdt.dt;
			break;
		}
		return xdt.InternalKind switch
		{
			XsdDateTimeKind.LocalEastOfZulu => new DateTimeOffset(dateTime, new TimeSpan(xdt.ZoneHour, xdt.ZoneMinute, 0)), 
			XsdDateTimeKind.LocalWestOfZulu => new DateTimeOffset(dateTime, new TimeSpan(-xdt.ZoneHour, -xdt.ZoneMinute, 0)), 
			XsdDateTimeKind.Zulu => new DateTimeOffset(dateTime, new TimeSpan(0L)), 
			_ => new DateTimeOffset(dateTime, TimeZoneInfo.Local.GetUtcOffset(dateTime)), 
		};
	}

	public static int Compare(XsdDateTime left, XsdDateTime right)
	{
		if (left.extra == right.extra)
		{
			return DateTime.Compare(left.dt, right.dt);
		}
		if (left.InternalTypeCode != right.InternalTypeCode)
		{
			throw new ArgumentException(Res.GetString("Cannot compare '{0}' and '{1}'.", left.TypeCode, right.TypeCode));
		}
		return DateTime.Compare(left.GetZuluDateTime(), right.GetZuluDateTime());
	}

	public int CompareTo(object value)
	{
		if (value == null)
		{
			return 1;
		}
		return Compare(this, (XsdDateTime)value);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(64);
		switch (InternalTypeCode)
		{
		case DateTimeTypeCode.DateTime:
			PrintDate(stringBuilder);
			stringBuilder.Append('T');
			PrintTime(stringBuilder);
			break;
		case DateTimeTypeCode.Time:
			PrintTime(stringBuilder);
			break;
		case DateTimeTypeCode.Date:
			PrintDate(stringBuilder);
			break;
		case DateTimeTypeCode.GYearMonth:
		{
			char[] array = new char[Lzyyyy_MM];
			IntToCharArray(array, 0, Year, 4);
			array[Lzyyyy] = '-';
			ShortToCharArray(array, Lzyyyy_, Month);
			stringBuilder.Append(array);
			break;
		}
		case DateTimeTypeCode.GYear:
		{
			char[] array = new char[Lzyyyy];
			IntToCharArray(array, 0, Year, 4);
			stringBuilder.Append(array);
			break;
		}
		case DateTimeTypeCode.GMonthDay:
		{
			char[] array = new char[Lz__mm_dd];
			array[0] = '-';
			array[Lz_] = '-';
			ShortToCharArray(array, Lz__, Month);
			array[Lz__mm] = '-';
			ShortToCharArray(array, Lz__mm_, Day);
			stringBuilder.Append(array);
			break;
		}
		case DateTimeTypeCode.GDay:
		{
			char[] array = new char[Lz___dd];
			array[0] = '-';
			array[Lz_] = '-';
			array[Lz__] = '-';
			ShortToCharArray(array, Lz___, Day);
			stringBuilder.Append(array);
			break;
		}
		case DateTimeTypeCode.GMonth:
		{
			char[] array = new char[Lz__mm__];
			array[0] = '-';
			array[Lz_] = '-';
			ShortToCharArray(array, Lz__, Month);
			array[Lz__mm] = '-';
			array[Lz__mm_] = '-';
			stringBuilder.Append(array);
			break;
		}
		}
		PrintZone(stringBuilder);
		return stringBuilder.ToString();
	}

	private void PrintDate(StringBuilder sb)
	{
		char[] array = new char[Lzyyyy_MM_dd];
		IntToCharArray(array, 0, Year, 4);
		array[Lzyyyy] = '-';
		ShortToCharArray(array, Lzyyyy_, Month);
		array[Lzyyyy_MM] = '-';
		ShortToCharArray(array, Lzyyyy_MM_, Day);
		sb.Append(array);
	}

	private void PrintTime(StringBuilder sb)
	{
		char[] array = new char[LzHH_mm_ss];
		ShortToCharArray(array, 0, Hour);
		array[LzHH] = ':';
		ShortToCharArray(array, LzHH_, Minute);
		array[LzHH_mm] = ':';
		ShortToCharArray(array, LzHH_mm_, Second);
		sb.Append(array);
		int num = Fraction;
		if (num != 0)
		{
			int num2 = 7;
			while (num % 10 == 0)
			{
				num2--;
				num /= 10;
			}
			array = new char[num2 + 1];
			array[0] = '.';
			IntToCharArray(array, 1, num, num2);
			sb.Append(array);
		}
	}

	private void PrintZone(StringBuilder sb)
	{
		switch (InternalKind)
		{
		case XsdDateTimeKind.Zulu:
			sb.Append('Z');
			break;
		case XsdDateTimeKind.LocalWestOfZulu:
		{
			char[] array = new char[Lz_zz_zz];
			array[0] = '-';
			ShortToCharArray(array, Lz_, ZoneHour);
			array[Lz_zz] = ':';
			ShortToCharArray(array, Lz_zz_, ZoneMinute);
			sb.Append(array);
			break;
		}
		case XsdDateTimeKind.LocalEastOfZulu:
		{
			char[] array = new char[Lz_zz_zz];
			array[0] = '+';
			ShortToCharArray(array, Lz_, ZoneHour);
			array[Lz_zz] = ':';
			ShortToCharArray(array, Lz_zz_, ZoneMinute);
			sb.Append(array);
			break;
		}
		}
	}

	private void IntToCharArray(char[] text, int start, int value, int digits)
	{
		while (digits-- != 0)
		{
			text[start + digits] = (char)(value % 10 + 48);
			value /= 10;
		}
	}

	private void ShortToCharArray(char[] text, int start, int value)
	{
		text[start] = (char)(value / 10 + 48);
		text[start + 1] = (char)(value % 10 + 48);
	}

	private DateTime GetZuluDateTime()
	{
		return InternalKind switch
		{
			XsdDateTimeKind.Zulu => dt, 
			XsdDateTimeKind.LocalEastOfZulu => dt.Subtract(new TimeSpan(ZoneHour, ZoneMinute, 0)), 
			XsdDateTimeKind.LocalWestOfZulu => dt.Add(new TimeSpan(ZoneHour, ZoneMinute, 0)), 
			_ => dt.ToUniversalTime(), 
		};
	}
}
