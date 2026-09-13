using System.Globalization;

namespace System.Xml.Xsl.Runtime;

internal class DecimalFormat
{
	public NumberFormatInfo info;

	public char digit;

	public char zeroDigit;

	public char patternSeparator;

	internal DecimalFormat(NumberFormatInfo info, char digit, char zeroDigit, char patternSeparator)
	{
		this.info = info;
		this.digit = digit;
		this.zeroDigit = zeroDigit;
		this.patternSeparator = patternSeparator;
	}
}
