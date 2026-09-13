using System.Collections.Generic;
using System.Globalization;
using System.Net.Configuration;
using System.Text;

namespace System.Net;

internal sealed class HttpListenerRequestUriBuilder
{
	private enum ParsingResult
	{
		Success,
		InvalidString,
		EncodingError
	}

	private enum EncodingType
	{
		Primary,
		Secondary
	}

	private static readonly bool useCookedRequestUrl;

	private static readonly Encoding utf8Encoding;

	private static readonly Encoding ansiEncoding;

	private readonly string rawUri;

	private readonly string cookedUriScheme;

	private readonly string cookedUriHost;

	private readonly string cookedUriPath;

	private readonly string cookedUriQuery;

	private StringBuilder requestUriString;

	private List<byte> rawOctets;

	private string rawPath;

	private Uri requestUri;

	static HttpListenerRequestUriBuilder()
	{
		useCookedRequestUrl = SettingsSectionInternal.Section.HttpListenerUnescapeRequestUrl;
		utf8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
		ansiEncoding = Encoding.GetEncoding(0, new EncoderExceptionFallback(), new DecoderExceptionFallback());
	}

	private HttpListenerRequestUriBuilder(string rawUri, string cookedUriScheme, string cookedUriHost, string cookedUriPath, string cookedUriQuery)
	{
		this.rawUri = rawUri;
		this.cookedUriScheme = cookedUriScheme;
		this.cookedUriHost = cookedUriHost;
		this.cookedUriPath = AddSlashToAsteriskOnlyPath(cookedUriPath);
		if (cookedUriQuery == null)
		{
			this.cookedUriQuery = string.Empty;
		}
		else
		{
			this.cookedUriQuery = cookedUriQuery;
		}
	}

	public static Uri GetRequestUri(string rawUri, string cookedUriScheme, string cookedUriHost, string cookedUriPath, string cookedUriQuery)
	{
		return new HttpListenerRequestUriBuilder(rawUri, cookedUriScheme, cookedUriHost, cookedUriPath, cookedUriQuery).Build();
	}

	private Uri Build()
	{
		if (useCookedRequestUrl)
		{
			BuildRequestUriUsingCookedPath();
			if (requestUri == null)
			{
				BuildRequestUriUsingRawPath();
			}
		}
		else
		{
			BuildRequestUriUsingRawPath();
			if (requestUri == null)
			{
				BuildRequestUriUsingCookedPath();
			}
		}
		return requestUri;
	}

	private void BuildRequestUriUsingCookedPath()
	{
		if (!Uri.TryCreate(cookedUriScheme + Uri.SchemeDelimiter + cookedUriHost + cookedUriPath + cookedUriQuery, UriKind.Absolute, out requestUri))
		{
			LogWarning("BuildRequestUriUsingCookedPath", "Can't create Uri from string '{0}://{1}{2}{3}'.", cookedUriScheme, cookedUriHost, cookedUriPath, cookedUriQuery);
		}
	}

	private void BuildRequestUriUsingRawPath()
	{
		bool flag = false;
		rawPath = GetPath(rawUri);
		if (rawPath == string.Empty)
		{
			string text = rawPath;
			if (text == string.Empty)
			{
				text = "/";
			}
			flag = Uri.TryCreate(cookedUriScheme + Uri.SchemeDelimiter + cookedUriHost + text + cookedUriQuery, UriKind.Absolute, out requestUri);
		}
		else
		{
			ParsingResult parsingResult = BuildRequestUriUsingRawPath(GetEncoding(EncodingType.Primary));
			if (parsingResult == ParsingResult.EncodingError)
			{
				Encoding encoding = GetEncoding(EncodingType.Secondary);
				parsingResult = BuildRequestUriUsingRawPath(encoding);
			}
			flag = parsingResult == ParsingResult.Success;
		}
		if (!flag)
		{
			LogWarning("BuildRequestUriUsingRawPath", "Can't create Uri from string '{0}://{1}{2}{3}'.", cookedUriScheme, cookedUriHost, rawPath, cookedUriQuery);
		}
	}

	private static Encoding GetEncoding(EncodingType type)
	{
		if (type == EncodingType.Secondary)
		{
			return ansiEncoding;
		}
		return utf8Encoding;
	}

	private ParsingResult BuildRequestUriUsingRawPath(Encoding encoding)
	{
		rawOctets = new List<byte>();
		requestUriString = new StringBuilder();
		requestUriString.Append(cookedUriScheme);
		requestUriString.Append(Uri.SchemeDelimiter);
		requestUriString.Append(cookedUriHost);
		ParsingResult parsingResult = ParseRawPath(encoding);
		if (parsingResult == ParsingResult.Success)
		{
			requestUriString.Append(cookedUriQuery);
			if (!Uri.TryCreate(requestUriString.ToString(), UriKind.Absolute, out requestUri))
			{
				parsingResult = ParsingResult.InvalidString;
			}
		}
		if (parsingResult != ParsingResult.Success)
		{
			LogWarning("BuildRequestUriUsingRawPath", "Can't convert Uri path '{0}' using encoding '{1}'.", rawPath, encoding.EncodingName);
		}
		return parsingResult;
	}

	private ParsingResult ParseRawPath(Encoding encoding)
	{
		int num = 0;
		char c = '\0';
		while (num < rawPath.Length)
		{
			c = rawPath[num];
			if (c == '%')
			{
				num++;
				c = rawPath[num];
				if (c == 'u' || c == 'U')
				{
					if (!EmptyDecodeAndAppendRawOctetsList(encoding))
					{
						return ParsingResult.EncodingError;
					}
					if (!AppendUnicodeCodePointValuePercentEncoded(rawPath.Substring(num + 1, 4)))
					{
						return ParsingResult.InvalidString;
					}
					num += 5;
				}
				else
				{
					if (!AddPercentEncodedOctetToRawOctetsList(encoding, rawPath.Substring(num, 2)))
					{
						return ParsingResult.InvalidString;
					}
					num += 2;
				}
			}
			else
			{
				if (!EmptyDecodeAndAppendRawOctetsList(encoding))
				{
					return ParsingResult.EncodingError;
				}
				requestUriString.Append(c);
				num++;
			}
		}
		if (!EmptyDecodeAndAppendRawOctetsList(encoding))
		{
			return ParsingResult.EncodingError;
		}
		return ParsingResult.Success;
	}

	private bool AppendUnicodeCodePointValuePercentEncoded(string codePoint)
	{
		if (!int.TryParse(codePoint, NumberStyles.HexNumber, null, out var result))
		{
			LogWarning("AppendUnicodeCodePointValuePercentEncoded", "Can't convert percent encoded value '{0}'.", codePoint);
			return false;
		}
		string text = null;
		try
		{
			text = char.ConvertFromUtf32(result);
			AppendOctetsPercentEncoded(requestUriString, utf8Encoding.GetBytes(text));
			return true;
		}
		catch (ArgumentOutOfRangeException)
		{
			LogWarning("AppendUnicodeCodePointValuePercentEncoded", "Can't convert percent encoded value '{0}'.", codePoint);
		}
		catch (EncoderFallbackException ex2)
		{
			LogWarning("AppendUnicodeCodePointValuePercentEncoded", "Can't convert string '{0}' into UTF-8 bytes: {1}", text, ex2.Message);
		}
		return false;
	}

	private bool AddPercentEncodedOctetToRawOctetsList(Encoding encoding, string escapedCharacter)
	{
		if (!byte.TryParse(escapedCharacter, NumberStyles.HexNumber, null, out var result))
		{
			LogWarning("AddPercentEncodedOctetToRawOctetsList", "Can't convert percent encoded value '{0}'.", escapedCharacter);
			return false;
		}
		rawOctets.Add(result);
		return true;
	}

	private bool EmptyDecodeAndAppendRawOctetsList(Encoding encoding)
	{
		if (rawOctets.Count == 0)
		{
			return true;
		}
		string text = null;
		try
		{
			text = encoding.GetString(rawOctets.ToArray());
			if (encoding == utf8Encoding)
			{
				AppendOctetsPercentEncoded(requestUriString, rawOctets.ToArray());
			}
			else
			{
				AppendOctetsPercentEncoded(requestUriString, utf8Encoding.GetBytes(text));
			}
			rawOctets.Clear();
			return true;
		}
		catch (DecoderFallbackException ex)
		{
			LogWarning("EmptyDecodeAndAppendRawOctetsList", "Can't convert bytes '{0}' into UTF-16 characters: {1}", GetOctetsAsString(rawOctets), ex.Message);
		}
		catch (EncoderFallbackException ex2)
		{
			LogWarning("EmptyDecodeAndAppendRawOctetsList", "Can't convert string '{0}' into UTF-8 bytes: {1}", text, ex2.Message);
		}
		return false;
	}

	private static void AppendOctetsPercentEncoded(StringBuilder target, IEnumerable<byte> octets)
	{
		foreach (byte octet in octets)
		{
			target.Append('%');
			target.Append(octet.ToString("X2", CultureInfo.InvariantCulture));
		}
	}

	private static string GetOctetsAsString(IEnumerable<byte> octets)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = true;
		foreach (byte octet in octets)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				stringBuilder.Append(" ");
			}
			stringBuilder.Append(octet.ToString("X2", CultureInfo.InvariantCulture));
		}
		return stringBuilder.ToString();
	}

	private static string GetPath(string uriString)
	{
		int num = 0;
		if (uriString[0] != '/')
		{
			int num2 = 0;
			if (uriString.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
			{
				num2 = 7;
			}
			else if (uriString.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
			{
				num2 = 8;
			}
			if (num2 > 0)
			{
				num = uriString.IndexOf('/', num2);
				if (num == -1)
				{
					num = uriString.Length;
				}
			}
			else
			{
				uriString = "/" + uriString;
			}
		}
		int num3 = uriString.IndexOf('?');
		if (num3 == -1)
		{
			num3 = uriString.Length;
		}
		return AddSlashToAsteriskOnlyPath(uriString.Substring(num, num3 - num));
	}

	private static string AddSlashToAsteriskOnlyPath(string path)
	{
		if (path.Length == 1 && path[0] == '*')
		{
			return "/*";
		}
		return path;
	}

	private void LogWarning(string methodName, string message, params object[] args)
	{
		_ = Logging.On;
	}
}
