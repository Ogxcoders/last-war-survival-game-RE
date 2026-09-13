using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Http.Headers;

internal static class HeaderUtilities
{
	private const string qualityName = "q";

	internal const string ConnectionClose = "close";

	internal static readonly TransferCodingHeaderValue TransferEncodingChunked = new TransferCodingHeaderValue("chunked");

	internal static readonly NameValueWithParametersHeaderValue ExpectContinue = new NameValueWithParametersHeaderValue("100-continue");

	internal const string BytesUnit = "bytes";

	internal static readonly Action<HttpHeaderValueCollection<string>, string> TokenValidator = ValidateToken;

	private static readonly char[] s_hexUpperChars = new char[16]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F'
	};

	internal static void SetQuality(ObjectCollection<NameValueHeaderValue> parameters, double? value)
	{
		NameValueHeaderValue nameValueHeaderValue = NameValueHeaderValue.Find(parameters, "q");
		if (value.HasValue)
		{
			if (value < 0.0 || value > 1.0)
			{
				throw new ArgumentOutOfRangeException("value");
			}
			string value2 = value.Value.ToString("0.0##", NumberFormatInfo.InvariantInfo);
			if (nameValueHeaderValue != null)
			{
				nameValueHeaderValue.Value = value2;
			}
			else
			{
				parameters.Add(new NameValueHeaderValue("q", value2));
			}
		}
		else if (nameValueHeaderValue != null)
		{
			parameters.Remove(nameValueHeaderValue);
		}
	}

	internal static bool ContainsNonAscii(string input)
	{
		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] > '\u007f')
			{
				return true;
			}
		}
		return false;
	}

	internal static string Encode5987(string input)
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		byte[] array = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(input.Length));
		int bytes = Encoding.UTF8.GetBytes(input, 0, input.Length, array, 0);
		stringBuilder.Append("utf-8''");
		for (int i = 0; i < bytes; i++)
		{
			byte b = array[i];
			if (b > 127)
			{
				AddHexEscaped(b, stringBuilder);
			}
			else if (!HttpRuleParser.IsTokenChar((char)b) || b == 42 || b == 39 || b == 37)
			{
				AddHexEscaped(b, stringBuilder);
			}
			else
			{
				stringBuilder.Append((char)b);
			}
		}
		Array.Clear(array, 0, bytes);
		ArrayPool<byte>.Shared.Return(array);
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	private static void AddHexEscaped(byte c, StringBuilder destination)
	{
		destination.Append('%');
		destination.Append(s_hexUpperChars[(c & 0xF0) >> 4]);
		destination.Append(s_hexUpperChars[c & 0xF]);
	}

	internal static double? GetQuality(ObjectCollection<NameValueHeaderValue> parameters)
	{
		NameValueHeaderValue nameValueHeaderValue = NameValueHeaderValue.Find(parameters, "q");
		if (nameValueHeaderValue != null)
		{
			double result = 0.0;
			if (double.TryParse(nameValueHeaderValue.Value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out result))
			{
				return result;
			}
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(null, global::SR.Format("The 'q' value is invalid: '{0}'.", nameValueHeaderValue.Value));
			}
		}
		return null;
	}

	internal static void CheckValidToken(string value, string parameterName)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("The value cannot be null or empty.", parameterName);
		}
		if (HttpRuleParser.GetTokenLength(value, 0) != value.Length)
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", value));
		}
	}

	internal static void CheckValidComment(string value, string parameterName)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("The value cannot be null or empty.", parameterName);
		}
		int length = 0;
		if (HttpRuleParser.GetCommentLength(value, 0, out length) != HttpParseResult.Parsed || length != value.Length)
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", value));
		}
	}

	internal static void CheckValidQuotedString(string value, string parameterName)
	{
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("The value cannot be null or empty.", parameterName);
		}
		int length = 0;
		if (HttpRuleParser.GetQuotedStringLength(value, 0, out length) != HttpParseResult.Parsed || length != value.Length)
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", value));
		}
	}

	internal static bool AreEqualCollections<T>(ObjectCollection<T> x, ObjectCollection<T> y) where T : class
	{
		return AreEqualCollections(x, y, null);
	}

	internal static bool AreEqualCollections<T>(ObjectCollection<T> x, ObjectCollection<T> y, IEqualityComparer<T> comparer) where T : class
	{
		if (x == null)
		{
			if (y != null)
			{
				return y.Count == 0;
			}
			return true;
		}
		if (y == null)
		{
			return x.Count == 0;
		}
		if (x.Count != y.Count)
		{
			return false;
		}
		if (x.Count == 0)
		{
			return true;
		}
		bool[] array = new bool[x.Count];
		int num = 0;
		foreach (T item in x)
		{
			num = 0;
			bool flag = false;
			foreach (T item2 in y)
			{
				if (!array[num] && ((comparer == null && item.Equals(item2)) || (comparer != null && comparer.Equals(item, item2))))
				{
					array[num] = true;
					flag = true;
					break;
				}
				num++;
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	internal static int GetNextNonEmptyOrWhitespaceIndex(string input, int startIndex, bool skipEmptyValues, out bool separatorFound)
	{
		separatorFound = false;
		int num = startIndex + HttpRuleParser.GetWhitespaceLength(input, startIndex);
		if (num == input.Length || input[num] != ',')
		{
			return num;
		}
		separatorFound = true;
		num++;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (skipEmptyValues)
		{
			while (num < input.Length && input[num] == ',')
			{
				num++;
				num += HttpRuleParser.GetWhitespaceLength(input, num);
			}
		}
		return num;
	}

	internal static DateTimeOffset? GetDateTimeOffsetValue(HeaderDescriptor descriptor, HttpHeaders store)
	{
		object parsedValues = store.GetParsedValues(descriptor);
		if (parsedValues != null)
		{
			return (DateTimeOffset)parsedValues;
		}
		return null;
	}

	internal static TimeSpan? GetTimeSpanValue(HeaderDescriptor descriptor, HttpHeaders store)
	{
		object parsedValues = store.GetParsedValues(descriptor);
		if (parsedValues != null)
		{
			return (TimeSpan)parsedValues;
		}
		return null;
	}

	internal static bool TryParseInt32(string value, out int result)
	{
		return TryParseInt32(value, 0, value.Length, out result);
	}

	internal static bool TryParseInt32(string value, int offset, int length, out int result)
	{
		if (offset < 0 || length < 0 || offset > value.Length - length)
		{
			result = 0;
			return false;
		}
		int num = 0;
		int num2 = offset;
		int num3 = offset + length;
		while (num2 < num3)
		{
			int num4 = value[num2++] - 48;
			if ((uint)num4 > 9u || num > 214748364 || (num == 214748364 && num4 > 7))
			{
				result = 0;
				return false;
			}
			num = num * 10 + num4;
		}
		result = num;
		return true;
	}

	internal static bool TryParseInt64(string value, int offset, int length, out long result)
	{
		if (offset < 0 || length < 0 || offset > value.Length - length)
		{
			result = 0L;
			return false;
		}
		long num = 0L;
		int num2 = offset;
		int num3 = offset + length;
		while (num2 < num3)
		{
			int num4 = value[num2++] - 48;
			if ((uint)num4 > 9u || num > 922337203685477580L || (num == 922337203685477580L && num4 > 7))
			{
				result = 0L;
				return false;
			}
			num = num * 10 + num4;
		}
		result = num;
		return true;
	}

	internal static string DumpHeaders(params HttpHeaders[] headers)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("{\r\n");
		for (int i = 0; i < headers.Length; i++)
		{
			if (headers[i] == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, IEnumerable<string>> item in headers[i])
			{
				foreach (string item2 in item.Value)
				{
					stringBuilder.Append("  ");
					stringBuilder.Append(item.Key);
					stringBuilder.Append(": ");
					stringBuilder.Append(item2);
					stringBuilder.Append("\r\n");
				}
			}
		}
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}

	internal static bool IsValidEmailAddress(string value)
	{
		try
		{
			MailAddressParser.ParseAddress(value);
			return true;
		}
		catch (FormatException ex)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(null, global::SR.Format("Value '{0}' is not a valid email address. Error: {1}", value, ex.Message));
			}
		}
		return false;
	}

	private static void ValidateToken(HttpHeaderValueCollection<string> collection, string value)
	{
		CheckValidToken(value, "item");
	}
}
