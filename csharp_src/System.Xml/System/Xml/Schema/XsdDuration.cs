using System.Text;

namespace System.Xml.Schema;

internal struct XsdDuration
{
	private enum Parts
	{
		HasNone = 0,
		HasYears = 1,
		HasMonths = 2,
		HasDays = 4,
		HasHours = 8,
		HasMinutes = 0x10,
		HasSeconds = 0x20
	}

	public enum DurationType
	{
		Duration,
		YearMonthDuration,
		DayTimeDuration
	}

	private int years;

	private int months;

	private int days;

	private int hours;

	private int minutes;

	private int seconds;

	private uint nanoseconds;

	private const uint NegativeBit = 2147483648u;

	public bool IsNegative => (nanoseconds & 0x80000000u) != 0;

	public int Years => years;

	public int Months => months;

	public int Days => days;

	public int Hours => hours;

	public int Minutes => minutes;

	public int Seconds => seconds;

	public int Nanoseconds => (int)(nanoseconds & 0x7FFFFFFF);

	public int Microseconds => Nanoseconds / 1000;

	public int Milliseconds => Nanoseconds / 1000000;

	public XsdDuration(bool isNegative, int years, int months, int days, int hours, int minutes, int seconds, int nanoseconds)
	{
		if (years < 0)
		{
			throw new ArgumentOutOfRangeException("years");
		}
		if (months < 0)
		{
			throw new ArgumentOutOfRangeException("months");
		}
		if (days < 0)
		{
			throw new ArgumentOutOfRangeException("days");
		}
		if (hours < 0)
		{
			throw new ArgumentOutOfRangeException("hours");
		}
		if (minutes < 0)
		{
			throw new ArgumentOutOfRangeException("minutes");
		}
		if (seconds < 0)
		{
			throw new ArgumentOutOfRangeException("seconds");
		}
		if (nanoseconds < 0 || nanoseconds > 999999999)
		{
			throw new ArgumentOutOfRangeException("nanoseconds");
		}
		this.years = years;
		this.months = months;
		this.days = days;
		this.hours = hours;
		this.minutes = minutes;
		this.seconds = seconds;
		this.nanoseconds = (uint)nanoseconds;
		if (isNegative)
		{
			this.nanoseconds |= 2147483648u;
		}
	}

	public XsdDuration(TimeSpan timeSpan)
		: this(timeSpan, DurationType.Duration)
	{
	}

	public XsdDuration(TimeSpan timeSpan, DurationType durationType)
	{
		long ticks = timeSpan.Ticks;
		bool flag;
		ulong num;
		if (ticks < 0)
		{
			flag = true;
			num = (ulong)(-ticks);
		}
		else
		{
			flag = false;
			num = (ulong)ticks;
		}
		if (durationType == DurationType.YearMonthDuration)
		{
			int num2 = (int)(num / 315360000000000L);
			int num3 = (int)(num % 315360000000000L / 25920000000000L);
			if (num3 == 12)
			{
				num2++;
				num3 = 0;
			}
			this = new XsdDuration(flag, num2, num3, 0, 0, 0, 0, 0);
			return;
		}
		nanoseconds = (uint)((int)(num % 10000000) * 100);
		if (flag)
		{
			nanoseconds |= 2147483648u;
		}
		years = 0;
		months = 0;
		days = (int)(num / 864000000000L);
		hours = (int)(num / 36000000000L % 24);
		minutes = (int)(num / 600000000 % 60);
		seconds = (int)(num / 10000000 % 60);
	}

	public XsdDuration(string s)
		: this(s, DurationType.Duration)
	{
	}

	public XsdDuration(string s, DurationType durationType)
	{
		XsdDuration result;
		Exception ex = TryParse(s, durationType, out result);
		if (ex != null)
		{
			throw ex;
		}
		years = result.Years;
		months = result.Months;
		days = result.Days;
		hours = result.Hours;
		minutes = result.Minutes;
		seconds = result.Seconds;
		nanoseconds = (uint)result.Nanoseconds;
		if (result.IsNegative)
		{
			nanoseconds |= 2147483648u;
		}
	}

	public XsdDuration Normalize()
	{
		int num = Years;
		int num2 = Months;
		int num3 = Days;
		int num4 = Hours;
		int num5 = Minutes;
		int num6 = Seconds;
		try
		{
			if (num2 >= 12)
			{
				checked
				{
					num += unchecked(num2 / 12);
				}
				num2 %= 12;
			}
			if (num6 >= 60)
			{
				checked
				{
					num5 += unchecked(num6 / 60);
				}
				num6 %= 60;
			}
			if (num5 >= 60)
			{
				checked
				{
					num4 += unchecked(num5 / 60);
				}
				num5 %= 60;
			}
			if (num4 >= 24)
			{
				checked
				{
					num3 += unchecked(num4 / 24);
				}
				num4 %= 24;
			}
		}
		catch (OverflowException)
		{
			throw new OverflowException(Res.GetString("Value '{0}' was either too large or too small for {1}.", ToString(), "Duration"));
		}
		return new XsdDuration(IsNegative, num, num2, num3, num4, num5, num6, Nanoseconds);
	}

	public TimeSpan ToTimeSpan()
	{
		return ToTimeSpan(DurationType.Duration);
	}

	public TimeSpan ToTimeSpan(DurationType durationType)
	{
		TimeSpan result;
		Exception ex = TryToTimeSpan(durationType, out result);
		if (ex != null)
		{
			throw ex;
		}
		return result;
	}

	internal Exception TryToTimeSpan(out TimeSpan result)
	{
		return TryToTimeSpan(DurationType.Duration, out result);
	}

	internal Exception TryToTimeSpan(DurationType durationType, out TimeSpan result)
	{
		Exception ex = null;
		ulong num = 0uL;
		checked
		{
			try
			{
				if (durationType != DurationType.DayTimeDuration)
				{
					num += ((ulong)years + unchecked(checked((ulong)months) / 12)) * 365;
					num += unchecked(checked((ulong)months) % 12) * 30;
				}
				if (durationType != DurationType.YearMonthDuration)
				{
					num += (ulong)days;
					num *= 24;
					num += (ulong)hours;
					num *= 60;
					num += (ulong)minutes;
					num *= 60;
					num += (ulong)seconds;
					num *= 10000000;
					num += unchecked(checked((ulong)Nanoseconds) / 100);
				}
				else
				{
					num *= 864000000000L;
				}
				if (IsNegative)
				{
					if (num == 9223372036854775808uL)
					{
						result = new TimeSpan(long.MinValue);
					}
					else
					{
						result = new TimeSpan(-(long)num);
					}
				}
				else
				{
					result = new TimeSpan((long)num);
				}
				return null;
			}
			catch (OverflowException)
			{
				result = TimeSpan.MinValue;
				return new OverflowException(Res.GetString("Value '{0}' was either too large or too small for {1}.", durationType, "TimeSpan"));
			}
		}
	}

	public override string ToString()
	{
		return ToString(DurationType.Duration);
	}

	internal string ToString(DurationType durationType)
	{
		StringBuilder stringBuilder = new StringBuilder(20);
		if (IsNegative)
		{
			stringBuilder.Append('-');
		}
		stringBuilder.Append('P');
		if (durationType != DurationType.DayTimeDuration)
		{
			if (years != 0)
			{
				stringBuilder.Append(XmlConvert.ToString(years));
				stringBuilder.Append('Y');
			}
			if (months != 0)
			{
				stringBuilder.Append(XmlConvert.ToString(months));
				stringBuilder.Append('M');
			}
		}
		if (durationType != DurationType.YearMonthDuration)
		{
			if (days != 0)
			{
				stringBuilder.Append(XmlConvert.ToString(days));
				stringBuilder.Append('D');
			}
			if (hours != 0 || minutes != 0 || seconds != 0 || Nanoseconds != 0)
			{
				stringBuilder.Append('T');
				if (hours != 0)
				{
					stringBuilder.Append(XmlConvert.ToString(hours));
					stringBuilder.Append('H');
				}
				if (minutes != 0)
				{
					stringBuilder.Append(XmlConvert.ToString(minutes));
					stringBuilder.Append('M');
				}
				int num = Nanoseconds;
				if (seconds != 0 || num != 0)
				{
					stringBuilder.Append(XmlConvert.ToString(seconds));
					if (num != 0)
					{
						stringBuilder.Append('.');
						int length = stringBuilder.Length;
						stringBuilder.Length += 9;
						int num2 = stringBuilder.Length - 1;
						for (int num3 = num2; num3 >= length; num3--)
						{
							int num4 = num % 10;
							stringBuilder[num3] = (char)(num4 + 48);
							if (num2 == num3 && num4 == 0)
							{
								num2--;
							}
							num /= 10;
						}
						stringBuilder.Length = num2 + 1;
					}
					stringBuilder.Append('S');
				}
			}
			if (stringBuilder[stringBuilder.Length - 1] == 'P')
			{
				stringBuilder.Append("T0S");
			}
		}
		else if (stringBuilder[stringBuilder.Length - 1] == 'P')
		{
			stringBuilder.Append("0M");
		}
		return stringBuilder.ToString();
	}

	internal static Exception TryParse(string s, out XsdDuration result)
	{
		return TryParse(s, DurationType.Duration, out result);
	}

	internal static Exception TryParse(string s, DurationType durationType, out XsdDuration result)
	{
		Parts parts = Parts.HasNone;
		result = default(XsdDuration);
		s = s.Trim();
		int length = s.Length;
		int offset = 0;
		int i = 0;
		int result2;
		if (offset < length)
		{
			if (s[offset] == '-')
			{
				offset++;
				result.nanoseconds = 2147483648u;
			}
			else
			{
				result.nanoseconds = 0u;
			}
			if (offset < length && s[offset++] == 'P')
			{
				if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
				{
					goto IL_02d8;
				}
				if (offset < length)
				{
					if (s[offset] != 'Y')
					{
						goto IL_00bb;
					}
					if (i != 0)
					{
						parts |= Parts.HasYears;
						result.years = result2;
						if (++offset == length)
						{
							goto IL_0298;
						}
						if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
						{
							goto IL_02d8;
						}
						if (offset < length)
						{
							goto IL_00bb;
						}
					}
				}
			}
		}
		goto IL_02b5;
		IL_02d8:
		return new OverflowException(Res.GetString("Value '{0}' was either too large or too small for {1}.", s, durationType));
		IL_013f:
		if (s[offset] != 'T')
		{
			goto IL_0291;
		}
		if (i == 0)
		{
			offset++;
			if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
			{
				goto IL_02d8;
			}
			if (offset < length)
			{
				if (s[offset] != 'H')
				{
					goto IL_01b2;
				}
				if (i != 0)
				{
					parts |= Parts.HasHours;
					result.hours = result2;
					if (++offset == length)
					{
						goto IL_0298;
					}
					if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
					{
						goto IL_02d8;
					}
					if (offset < length)
					{
						goto IL_01b2;
					}
				}
			}
		}
		goto IL_02b5;
		IL_00bb:
		if (s[offset] != 'M')
		{
			goto IL_00fd;
		}
		if (i != 0)
		{
			parts |= Parts.HasMonths;
			result.months = result2;
			if (++offset == length)
			{
				goto IL_0298;
			}
			if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
			{
				goto IL_02d8;
			}
			if (offset < length)
			{
				goto IL_00fd;
			}
		}
		goto IL_02b5;
		IL_01f5:
		if (s[offset] == '.')
		{
			offset++;
			parts |= Parts.HasSeconds;
			result.seconds = result2;
			if (TryParseDigits(s, ref offset, eatDigits: true, out result2, out i) != null)
			{
				goto IL_02d8;
			}
			if (i == 0)
			{
				result2 = 0;
			}
			while (i > 9)
			{
				result2 /= 10;
				i--;
			}
			for (; i < 9; i++)
			{
				result2 *= 10;
			}
			result.nanoseconds |= (uint)result2;
			if (offset >= length || s[offset] != 'S')
			{
				goto IL_02b5;
			}
			if (++offset == length)
			{
				goto IL_0298;
			}
		}
		else if (s[offset] == 'S')
		{
			if (i == 0)
			{
				goto IL_02b5;
			}
			parts |= Parts.HasSeconds;
			result.seconds = result2;
			if (++offset == length)
			{
				goto IL_0298;
			}
		}
		goto IL_0291;
		IL_02b5:
		return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, durationType));
		IL_01b2:
		if (s[offset] != 'M')
		{
			goto IL_01f5;
		}
		if (i != 0)
		{
			parts |= Parts.HasMinutes;
			result.minutes = result2;
			if (++offset == length)
			{
				goto IL_0298;
			}
			if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
			{
				goto IL_02d8;
			}
			if (offset < length)
			{
				goto IL_01f5;
			}
		}
		goto IL_02b5;
		IL_0291:
		if (i == 0 && offset == length)
		{
			goto IL_0298;
		}
		goto IL_02b5;
		IL_00fd:
		if (s[offset] != 'D')
		{
			goto IL_013f;
		}
		if (i != 0)
		{
			parts |= Parts.HasDays;
			result.days = result2;
			if (++offset == length)
			{
				goto IL_0298;
			}
			if (TryParseDigits(s, ref offset, eatDigits: false, out result2, out i) != null)
			{
				goto IL_02d8;
			}
			if (offset < length)
			{
				goto IL_013f;
			}
		}
		goto IL_02b5;
		IL_0298:
		if (parts != Parts.HasNone)
		{
			if (durationType == DurationType.DayTimeDuration)
			{
				if ((parts & (Parts)3) == 0)
				{
					goto IL_02b3;
				}
			}
			else if (durationType != DurationType.YearMonthDuration || (parts & (Parts)(-4)) == 0)
			{
				goto IL_02b3;
			}
		}
		goto IL_02b5;
		IL_02b3:
		return null;
	}

	private static string TryParseDigits(string s, ref int offset, bool eatDigits, out int result, out int numDigits)
	{
		int num = offset;
		int length = s.Length;
		result = 0;
		numDigits = 0;
		while (offset < length && s[offset] >= '0' && s[offset] <= '9')
		{
			int num2 = s[offset] - 48;
			if (result > (int.MaxValue - num2) / 10)
			{
				if (!eatDigits)
				{
					return "Value '{0}' was either too large or too small for {1}.";
				}
				numDigits = offset - num;
				while (offset < length && s[offset] >= '0' && s[offset] <= '9')
				{
					offset++;
				}
				return null;
			}
			result = result * 10 + num2;
			offset++;
		}
		numDigits = offset - num;
		return null;
	}
}
