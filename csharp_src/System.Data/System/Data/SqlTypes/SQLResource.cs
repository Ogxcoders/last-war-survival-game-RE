namespace System.Data.SqlTypes;

internal static class SQLResource
{
	internal static string NullString => "Null";

	internal static string MessageString => "Message";

	internal static string ArithOverflowMessage => "Arithmetic Overflow.";

	internal static string DivideByZeroMessage => "Divide by zero error encountered.";

	internal static string NullValueMessage => "Data is Null. This method or property cannot be called on Null values.";

	internal static string TruncationMessage => "Numeric arithmetic causes truncation.";

	internal static string DateTimeOverflowMessage => "SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.";

	internal static string ConcatDiffCollationMessage => "Two strings to be concatenated have different collation.";

	internal static string CompareDiffCollationMessage => "Two strings to be compared have different collation.";

	internal static string InvalidFlagMessage => "Invalid flag value.";

	internal static string NumeToDecOverflowMessage => "Conversion from SqlDecimal to Decimal overflows.";

	internal static string ConversionOverflowMessage => "Conversion overflows.";

	internal static string InvalidDateTimeMessage => "Invalid SqlDateTime.";

	internal static string TimeZoneSpecifiedMessage => "A time zone was specified. SqlDateTime does not support time zones.";

	internal static string InvalidArraySizeMessage => "Invalid array size.";

	internal static string InvalidPrecScaleMessage => "Invalid numeric precision/scale.";

	internal static string FormatMessage => "The input wasn't in a correct format.";

	internal static string NotFilledMessage => "SQL Type has not been loaded with data.";

	internal static string AlreadyFilledMessage => "SQL Type has already been loaded with data.";

	internal static string ClosedXmlReaderMessage => "Invalid attempt to access a closed XmlReader.";

	internal static string InvalidOpStreamClosed(string method)
	{
		return global::SR.Format("Invalid attempt to call {0} when the stream is closed.", method);
	}

	internal static string InvalidOpStreamNonWritable(string method)
	{
		return global::SR.Format("Invalid attempt to call {0} when the stream non-writable.", method);
	}

	internal static string InvalidOpStreamNonReadable(string method)
	{
		return global::SR.Format("Invalid attempt to call {0} when the stream non-readable.", method);
	}

	internal static string InvalidOpStreamNonSeekable(string method)
	{
		return global::SR.Format("Invalid attempt to call {0} when the stream is non-seekable.", method);
	}
}
