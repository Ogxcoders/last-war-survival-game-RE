using System.Globalization;

namespace System.Net;

internal class HttpProtocolUtils
{
	private HttpProtocolUtils()
	{
	}

	internal static DateTime string2date(string S)
	{
		if (HttpDateParse.ParseHttpDate(S, out var dtOut))
		{
			return dtOut;
		}
		throw new ProtocolViolationException(global::SR.GetString("The value of the date string in the header is invalid."));
	}

	internal static string date2string(DateTime D)
	{
		DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
		return D.ToUniversalTime().ToString("R", dateTimeFormatInfo);
	}
}
