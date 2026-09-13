using System.Globalization;

namespace System.Runtime.Serialization;

public class DateTimeFormat
{
	private string formatString;

	private IFormatProvider formatProvider;

	private DateTimeStyles dateTimeStyles;

	public string FormatString => formatString;

	public IFormatProvider FormatProvider => formatProvider;

	public DateTimeStyles DateTimeStyles
	{
		get
		{
			return dateTimeStyles;
		}
		set
		{
			dateTimeStyles = value;
		}
	}

	public DateTimeFormat(string formatString)
		: this(formatString, DateTimeFormatInfo.CurrentInfo)
	{
	}

	public DateTimeFormat(string formatString, IFormatProvider formatProvider)
	{
		if (formatString == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("formatString");
		}
		if (formatProvider == null)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("formatProvider");
		}
		this.formatString = formatString;
		this.formatProvider = formatProvider;
		dateTimeStyles = DateTimeStyles.RoundtripKind;
	}
}
