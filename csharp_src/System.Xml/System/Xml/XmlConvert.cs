using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace System.Xml;

public class XmlConvert
{
	private static XmlCharType xmlCharType = XmlCharType.Instance;

	internal static char[] crt = new char[3] { '\n', '\r', '\t' };

	private static readonly int c_EncodedCharLength = 7;

	private static volatile Regex c_EncodeCharPattern;

	private static volatile Regex c_DecodeCharPattern;

	private static volatile string[] s_allDateTimeFormats;

	internal static readonly char[] WhitespaceChars = new char[4] { ' ', '\t', '\n', '\r' };

	private static string[] AllDateTimeFormats
	{
		get
		{
			if (s_allDateTimeFormats == null)
			{
				CreateAllDateTimeFormats();
			}
			return s_allDateTimeFormats;
		}
	}

	public static string EncodeName(string name)
	{
		return EncodeName(name, first: true, local: false);
	}

	public static string EncodeNmToken(string name)
	{
		return EncodeName(name, first: false, local: false);
	}

	public static string EncodeLocalName(string name)
	{
		return EncodeName(name, first: true, local: true);
	}

	public static string DecodeName(string name)
	{
		if (name == null || name.Length == 0)
		{
			return name;
		}
		StringBuilder stringBuilder = null;
		int length = name.Length;
		int num = 0;
		int num2 = name.IndexOf('_');
		IEnumerator enumerator = null;
		if (num2 >= 0)
		{
			if (c_DecodeCharPattern == null)
			{
				c_DecodeCharPattern = new Regex("_[Xx]([0-9a-fA-F]{4}|[0-9a-fA-F]{8})_");
			}
			enumerator = c_DecodeCharPattern.Matches(name, num2).GetEnumerator();
			int num3 = -1;
			if (enumerator != null && enumerator.MoveNext())
			{
				num3 = ((Match)enumerator.Current).Index;
			}
			for (int i = 0; i < length - c_EncodedCharLength + 1; i++)
			{
				if (i != num3)
				{
					continue;
				}
				if (enumerator.MoveNext())
				{
					num3 = ((Match)enumerator.Current).Index;
				}
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length + 20);
				}
				stringBuilder.Append(name, num, i - num);
				if (name[i + 6] != '_')
				{
					int num4 = FromHex(name[i + 2]) * 268435456 + FromHex(name[i + 3]) * 16777216 + FromHex(name[i + 4]) * 1048576 + FromHex(name[i + 5]) * 65536 + FromHex(name[i + 6]) * 4096 + FromHex(name[i + 7]) * 256 + FromHex(name[i + 8]) * 16 + FromHex(name[i + 9]);
					if (num4 >= 65536)
					{
						if (num4 <= 1114111)
						{
							num = i + c_EncodedCharLength + 4;
							XmlCharType.SplitSurrogateChar(num4, out var lowChar, out var highChar);
							stringBuilder.Append(highChar);
							stringBuilder.Append(lowChar);
						}
					}
					else
					{
						num = i + c_EncodedCharLength + 4;
						stringBuilder.Append((char)num4);
					}
					i += c_EncodedCharLength - 1 + 4;
				}
				else
				{
					num = i + c_EncodedCharLength;
					stringBuilder.Append((char)(FromHex(name[i + 2]) * 4096 + FromHex(name[i + 3]) * 256 + FromHex(name[i + 4]) * 16 + FromHex(name[i + 5])));
					i += c_EncodedCharLength - 1;
				}
			}
			if (num == 0)
			{
				return name;
			}
			if (num < length)
			{
				stringBuilder.Append(name, num, length - num);
			}
			return stringBuilder.ToString();
		}
		return name;
	}

	private static string EncodeName(string name, bool first, bool local)
	{
		if (string.IsNullOrEmpty(name))
		{
			return name;
		}
		StringBuilder stringBuilder = null;
		int length = name.Length;
		int num = 0;
		int i = 0;
		int num2 = name.IndexOf('_');
		IEnumerator enumerator = null;
		if (num2 >= 0)
		{
			if (c_EncodeCharPattern == null)
			{
				c_EncodeCharPattern = new Regex("(?<=_)[Xx]([0-9a-fA-F]{4}|[0-9a-fA-F]{8})_");
			}
			enumerator = c_EncodeCharPattern.Matches(name, num2).GetEnumerator();
		}
		int num3 = -1;
		if (enumerator != null && enumerator.MoveNext())
		{
			num3 = ((Match)enumerator.Current).Index - 1;
		}
		if (first && ((!xmlCharType.IsStartNCNameCharXml4e(name[0]) && (local || (!local && name[0] != ':'))) || num3 == 0))
		{
			if (stringBuilder == null)
			{
				stringBuilder = new StringBuilder(length + 20);
			}
			stringBuilder.Append("_x");
			if (length > 1 && XmlCharType.IsHighSurrogate(name[0]) && XmlCharType.IsLowSurrogate(name[1]))
			{
				int highChar = name[0];
				stringBuilder.Append(XmlCharType.CombineSurrogateChar(name[1], highChar).ToString("X8", CultureInfo.InvariantCulture));
				i++;
				num = 2;
			}
			else
			{
				stringBuilder.Append(((int)name[0]).ToString("X4", CultureInfo.InvariantCulture));
				num = 1;
			}
			stringBuilder.Append("_");
			i++;
			if (num3 == 0 && enumerator.MoveNext())
			{
				num3 = ((Match)enumerator.Current).Index - 1;
			}
		}
		for (; i < length; i++)
		{
			if ((local && !xmlCharType.IsNCNameCharXml4e(name[i])) || (!local && !xmlCharType.IsNameCharXml4e(name[i])) || num3 == i)
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(length + 20);
				}
				if (num3 == i && enumerator.MoveNext())
				{
					num3 = ((Match)enumerator.Current).Index - 1;
				}
				stringBuilder.Append(name, num, i - num);
				stringBuilder.Append("_x");
				if (length > i + 1 && XmlCharType.IsHighSurrogate(name[i]) && XmlCharType.IsLowSurrogate(name[i + 1]))
				{
					int highChar2 = name[i];
					stringBuilder.Append(XmlCharType.CombineSurrogateChar(name[i + 1], highChar2).ToString("X8", CultureInfo.InvariantCulture));
					num = i + 2;
					i++;
				}
				else
				{
					stringBuilder.Append(((int)name[i]).ToString("X4", CultureInfo.InvariantCulture));
					num = i + 1;
				}
				stringBuilder.Append("_");
			}
		}
		if (num == 0)
		{
			return name;
		}
		if (num < length)
		{
			stringBuilder.Append(name, num, length - num);
		}
		return stringBuilder.ToString();
	}

	private static int FromHex(char digit)
	{
		if (digit > '9')
		{
			return ((digit <= 'F') ? (digit - 65) : (digit - 97)) + 10;
		}
		return digit - 48;
	}

	internal static byte[] FromBinHexString(string s)
	{
		return FromBinHexString(s, allowOddCount: true);
	}

	internal static byte[] FromBinHexString(string s, bool allowOddCount)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		return BinHexDecoder.Decode(s.ToCharArray(), allowOddCount);
	}

	internal static string ToBinHexString(byte[] inArray)
	{
		if (inArray == null)
		{
			throw new ArgumentNullException("inArray");
		}
		return BinHexEncoder.Encode(inArray, 0, inArray.Length);
	}

	public static string VerifyName(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentNullException("name", Res.GetString("The empty string '' is not a valid name."));
		}
		int num = ValidateNames.ParseNameNoNamespaces(name, 0);
		if (num != name.Length)
		{
			throw CreateInvalidNameCharException(name, num, ExceptionType.XmlException);
		}
		return name;
	}

	internal static Exception TryVerifyName(string name)
	{
		if (name == null || name.Length == 0)
		{
			return new XmlException("The empty string '' is not a valid name.", string.Empty);
		}
		int num = ValidateNames.ParseNameNoNamespaces(name, 0);
		if (num != name.Length)
		{
			return new XmlException((num == 0) ? "Name cannot begin with the '{0}' character, hexadecimal value {1}." : "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(name, num));
		}
		return null;
	}

	internal static string VerifyQName(string name)
	{
		return VerifyQName(name, ExceptionType.XmlException);
	}

	internal static string VerifyQName(string name, ExceptionType exceptionType)
	{
		if (name == null || name.Length == 0)
		{
			throw new ArgumentNullException("name");
		}
		int colonOffset = -1;
		int num = ValidateNames.ParseQName(name, 0, out colonOffset);
		if (num != name.Length)
		{
			throw CreateException("The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(name, num), exceptionType, 0, num + 1);
		}
		return name;
	}

	public static string VerifyNCName(string name)
	{
		return VerifyNCName(name, ExceptionType.XmlException);
	}

	internal static string VerifyNCName(string name, ExceptionType exceptionType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentNullException("name", Res.GetString("The empty string '' is not a valid local name."));
		}
		int num = ValidateNames.ParseNCName(name, 0);
		if (num != name.Length)
		{
			throw CreateInvalidNameCharException(name, num, exceptionType);
		}
		return name;
	}

	internal static Exception TryVerifyNCName(string name)
	{
		int num = ValidateNames.ParseNCName(name);
		if (num == 0 || num != name.Length)
		{
			return ValidateNames.GetInvalidNameException(name, 0, num);
		}
		return null;
	}

	public static string VerifyTOKEN(string token)
	{
		if (token == null || token.Length == 0)
		{
			return token;
		}
		if (token[0] == ' ' || token[token.Length - 1] == ' ' || token.IndexOfAny(crt) != -1 || token.IndexOf("  ", StringComparison.Ordinal) != -1)
		{
			throw new XmlException("line-feed (#xA) or tab (#x9) characters, leading or trailing spaces and sequences of one or more spaces (#x20) are not allowed in 'xs:token'.", token);
		}
		return token;
	}

	internal static Exception TryVerifyTOKEN(string token)
	{
		if (token == null || token.Length == 0)
		{
			return null;
		}
		if (token[0] == ' ' || token[token.Length - 1] == ' ' || token.IndexOfAny(crt) != -1 || token.IndexOf("  ", StringComparison.Ordinal) != -1)
		{
			return new XmlException("line-feed (#xA) or tab (#x9) characters, leading or trailing spaces and sequences of one or more spaces (#x20) are not allowed in 'xs:token'.", token);
		}
		return null;
	}

	public static string VerifyNMTOKEN(string name)
	{
		return VerifyNMTOKEN(name, ExceptionType.XmlException);
	}

	internal static string VerifyNMTOKEN(string name, ExceptionType exceptionType)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw CreateException("Invalid NmToken value '{0}'.", name, exceptionType);
		}
		int num = ValidateNames.ParseNmtokenNoNamespaces(name, 0);
		if (num != name.Length)
		{
			throw CreateException("The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(name, num), exceptionType, 0, num + 1);
		}
		return name;
	}

	internal static Exception TryVerifyNMTOKEN(string name)
	{
		if (name == null || name.Length == 0)
		{
			return new XmlException("The empty string '' is not a valid name.", string.Empty);
		}
		int num = ValidateNames.ParseNmtokenNoNamespaces(name, 0);
		if (num != name.Length)
		{
			return new XmlException("The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(name, num));
		}
		return null;
	}

	internal static string VerifyNormalizedString(string str)
	{
		if (str.IndexOfAny(crt) != -1)
		{
			throw new XmlSchemaException("Carriage return (#xD), line feed (#xA), and tab (#x9) characters are not allowed in xs:normalizedString.", str);
		}
		return str;
	}

	internal static Exception TryVerifyNormalizedString(string str)
	{
		if (str.IndexOfAny(crt) != -1)
		{
			return new XmlSchemaException("Carriage return (#xD), line feed (#xA), and tab (#x9) characters are not allowed in xs:normalizedString.", str);
		}
		return null;
	}

	public static string VerifyXmlChars(string content)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		VerifyCharData(content, ExceptionType.XmlException);
		return content;
	}

	public static string VerifyPublicId(string publicId)
	{
		if (publicId == null)
		{
			throw new ArgumentNullException("publicId");
		}
		int num = xmlCharType.IsPublicId(publicId);
		if (num != -1)
		{
			throw CreateInvalidCharException(publicId, num, ExceptionType.XmlException);
		}
		return publicId;
	}

	public static string VerifyWhitespace(string content)
	{
		if (content == null)
		{
			throw new ArgumentNullException("content");
		}
		int num = xmlCharType.IsOnlyWhitespaceWithPos(content);
		if (num != -1)
		{
			throw new XmlException("The Whitespace or SignificantWhitespace node can contain only XML white space characters. '{0}' is not an XML white space character.", XmlException.BuildCharExceptionArgs(content, num), 0, num + 1);
		}
		return content;
	}

	public static bool IsStartNCNameChar(char ch)
	{
		return (xmlCharType.charProperties[(uint)ch] & 4) != 0;
	}

	public static bool IsNCNameChar(char ch)
	{
		return (xmlCharType.charProperties[(uint)ch] & 8) != 0;
	}

	public static bool IsXmlChar(char ch)
	{
		return (xmlCharType.charProperties[(uint)ch] & 0x10) != 0;
	}

	public static bool IsXmlSurrogatePair(char lowChar, char highChar)
	{
		if (XmlCharType.IsHighSurrogate(highChar))
		{
			return XmlCharType.IsLowSurrogate(lowChar);
		}
		return false;
	}

	public static bool IsPublicIdChar(char ch)
	{
		return xmlCharType.IsPubidChar(ch);
	}

	public static bool IsWhitespaceChar(char ch)
	{
		return (xmlCharType.charProperties[(uint)ch] & 1) != 0;
	}

	public static string ToString(bool value)
	{
		if (!value)
		{
			return "false";
		}
		return "true";
	}

	public static string ToString(char value)
	{
		return value.ToString(null);
	}

	public static string ToString(decimal value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	[CLSCompliant(false)]
	public static string ToString(sbyte value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(short value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(int value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(long value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(byte value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	[CLSCompliant(false)]
	public static string ToString(ushort value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	[CLSCompliant(false)]
	public static string ToString(uint value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	[CLSCompliant(false)]
	public static string ToString(ulong value)
	{
		return value.ToString(null, NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(float value)
	{
		if (float.IsNegativeInfinity(value))
		{
			return "-INF";
		}
		if (float.IsPositiveInfinity(value))
		{
			return "INF";
		}
		if (IsNegativeZero(value))
		{
			return "-0";
		}
		return value.ToString("R", NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(double value)
	{
		if (double.IsNegativeInfinity(value))
		{
			return "-INF";
		}
		if (double.IsPositiveInfinity(value))
		{
			return "INF";
		}
		if (IsNegativeZero(value))
		{
			return "-0";
		}
		return value.ToString("R", NumberFormatInfo.InvariantInfo);
	}

	public static string ToString(TimeSpan value)
	{
		return new XsdDuration(value).ToString();
	}

	[Obsolete("Use XmlConvert.ToString() that takes in XmlDateTimeSerializationMode")]
	public static string ToString(DateTime value)
	{
		return ToString(value, "yyyy-MM-ddTHH:mm:ss.fffffffzzzzzz");
	}

	public static string ToString(DateTime value, string format)
	{
		return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
	}

	public static string ToString(DateTime value, XmlDateTimeSerializationMode dateTimeOption)
	{
		switch (dateTimeOption)
		{
		case XmlDateTimeSerializationMode.Local:
			value = SwitchToLocalTime(value);
			break;
		case XmlDateTimeSerializationMode.Utc:
			value = SwitchToUtcTime(value);
			break;
		case XmlDateTimeSerializationMode.Unspecified:
			value = new DateTime(value.Ticks, DateTimeKind.Unspecified);
			break;
		default:
			throw new ArgumentException(Res.GetString("The '{0}' value for the 'dateTimeOption' parameter is not an allowed value for the 'XmlDateTimeSerializationMode' enumeration.", dateTimeOption, "dateTimeOption"));
		case XmlDateTimeSerializationMode.RoundtripKind:
			break;
		}
		return new XsdDateTime(value, XsdDateTimeFlags.DateTime).ToString();
	}

	public static string ToString(DateTimeOffset value)
	{
		return new XsdDateTime(value).ToString();
	}

	public static string ToString(DateTimeOffset value, string format)
	{
		return value.ToString(format, DateTimeFormatInfo.InvariantInfo);
	}

	public static string ToString(Guid value)
	{
		return value.ToString();
	}

	public static bool ToBoolean(string s)
	{
		s = TrimString(s);
		switch (s)
		{
		case "1":
		case "true":
			return true;
		case "0":
		case "false":
			return false;
		default:
			throw new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Boolean"));
		}
	}

	internal static Exception TryToBoolean(string s, out bool result)
	{
		s = TrimString(s);
		switch (s)
		{
		case "0":
		case "false":
			result = false;
			return null;
		case "1":
		case "true":
			result = true;
			return null;
		default:
			result = false;
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Boolean"));
		}
	}

	public static char ToChar(string s)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		if (s.Length != 1)
		{
			throw new FormatException(Res.GetString("String must be exactly one character long."));
		}
		return s[0];
	}

	internal static Exception TryToChar(string s, out char result)
	{
		if (!char.TryParse(s, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Char"));
		}
		return null;
	}

	public static decimal ToDecimal(string s)
	{
		return decimal.Parse(s, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToDecimal(string s, out decimal result)
	{
		if (!decimal.TryParse(s, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Decimal"));
		}
		return null;
	}

	internal static decimal ToInteger(string s)
	{
		return decimal.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToInteger(string s, out decimal result)
	{
		if (!decimal.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Integer"));
		}
		return null;
	}

	[CLSCompliant(false)]
	public static sbyte ToSByte(string s)
	{
		return sbyte.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToSByte(string s, out sbyte result)
	{
		if (!sbyte.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "SByte"));
		}
		return null;
	}

	public static short ToInt16(string s)
	{
		return short.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToInt16(string s, out short result)
	{
		if (!short.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Int16"));
		}
		return null;
	}

	public static int ToInt32(string s)
	{
		return int.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToInt32(string s, out int result)
	{
		if (!int.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Int32"));
		}
		return null;
	}

	public static long ToInt64(string s)
	{
		return long.Parse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToInt64(string s, out long result)
	{
		if (!long.TryParse(s, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Int64"));
		}
		return null;
	}

	public static byte ToByte(string s)
	{
		return byte.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToByte(string s, out byte result)
	{
		if (!byte.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Byte"));
		}
		return null;
	}

	[CLSCompliant(false)]
	public static ushort ToUInt16(string s)
	{
		return ushort.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToUInt16(string s, out ushort result)
	{
		if (!ushort.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "UInt16"));
		}
		return null;
	}

	[CLSCompliant(false)]
	public static uint ToUInt32(string s)
	{
		return uint.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToUInt32(string s, out uint result)
	{
		if (!uint.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "UInt32"));
		}
		return null;
	}

	[CLSCompliant(false)]
	public static ulong ToUInt64(string s)
	{
		return ulong.Parse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo);
	}

	internal static Exception TryToUInt64(string s, out ulong result)
	{
		if (!ulong.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "UInt64"));
		}
		return null;
	}

	public static float ToSingle(string s)
	{
		s = TrimString(s);
		if (s == "-INF")
		{
			return float.NegativeInfinity;
		}
		if (s == "INF")
		{
			return float.PositiveInfinity;
		}
		float num = float.Parse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo);
		if (num == 0f && s[0] == '-')
		{
			return -0f;
		}
		return num;
	}

	internal static Exception TryToSingle(string s, out float result)
	{
		s = TrimString(s);
		if (s == "-INF")
		{
			result = float.NegativeInfinity;
			return null;
		}
		if (s == "INF")
		{
			result = float.PositiveInfinity;
			return null;
		}
		if (!float.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Single"));
		}
		if (result == 0f && s[0] == '-')
		{
			result = -0f;
		}
		return null;
	}

	public static double ToDouble(string s)
	{
		s = TrimString(s);
		if (s == "-INF")
		{
			return double.NegativeInfinity;
		}
		if (s == "INF")
		{
			return double.PositiveInfinity;
		}
		double num = double.Parse(s, NumberStyles.Float, NumberFormatInfo.InvariantInfo);
		if (num == 0.0 && s[0] == '-')
		{
			return -0.0;
		}
		return num;
	}

	internal static Exception TryToDouble(string s, out double result)
	{
		s = TrimString(s);
		if (s == "-INF")
		{
			result = double.NegativeInfinity;
			return null;
		}
		if (s == "INF")
		{
			result = double.PositiveInfinity;
			return null;
		}
		if (!double.TryParse(s, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent, NumberFormatInfo.InvariantInfo, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Double"));
		}
		if (result == 0.0 && s[0] == '-')
		{
			result = -0.0;
		}
		return null;
	}

	internal static double ToXPathDouble(object o)
	{
		if (o is string value)
		{
			string text = TrimString(value);
			if (text.Length != 0 && text[0] != '+' && double.TryParse(text, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var result))
			{
				return result;
			}
			return double.NaN;
		}
		if (o is double)
		{
			return (double)o;
		}
		if (o is bool)
		{
			if (!(bool)o)
			{
				return 0.0;
			}
			return 1.0;
		}
		try
		{
			return Convert.ToDouble(o, NumberFormatInfo.InvariantInfo);
		}
		catch (FormatException)
		{
		}
		catch (OverflowException)
		{
		}
		catch (ArgumentNullException)
		{
		}
		return double.NaN;
	}

	internal static string ToXPathString(object value)
	{
		if (value is string result)
		{
			return result;
		}
		if (value is double num)
		{
			return num.ToString("R", NumberFormatInfo.InvariantInfo);
		}
		if (value is bool)
		{
			if (!(bool)value)
			{
				return "false";
			}
			return "true";
		}
		return Convert.ToString(value, NumberFormatInfo.InvariantInfo);
	}

	internal static double XPathRound(double value)
	{
		double num = Math.Round(value);
		if (value - num != 0.5)
		{
			return num;
		}
		return num + 1.0;
	}

	public static TimeSpan ToTimeSpan(string s)
	{
		XsdDuration xsdDuration;
		try
		{
			xsdDuration = new XsdDuration(s);
		}
		catch (Exception)
		{
			throw new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "TimeSpan"));
		}
		return xsdDuration.ToTimeSpan();
	}

	internal static Exception TryToTimeSpan(string s, out TimeSpan result)
	{
		XsdDuration result2;
		Exception ex = XsdDuration.TryParse(s, out result2);
		if (ex != null)
		{
			result = TimeSpan.MinValue;
			return ex;
		}
		return result2.TryToTimeSpan(out result);
	}

	private static void CreateAllDateTimeFormats()
	{
		if (s_allDateTimeFormats == null)
		{
			s_allDateTimeFormats = new string[24]
			{
				"yyyy-MM-ddTHH:mm:ss.FFFFFFFzzzzzz", "yyyy-MM-ddTHH:mm:ss.FFFFFFF", "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ", "HH:mm:ss.FFFFFFF", "HH:mm:ss.FFFFFFFZ", "HH:mm:ss.FFFFFFFzzzzzz", "yyyy-MM-dd", "yyyy-MM-ddZ", "yyyy-MM-ddzzzzzz", "yyyy-MM",
				"yyyy-MMZ", "yyyy-MMzzzzzz", "yyyy", "yyyyZ", "yyyyzzzzzz", "--MM-dd", "--MM-ddZ", "--MM-ddzzzzzz", "---dd", "---ddZ",
				"---ddzzzzzz", "--MM--", "--MM--Z", "--MM--zzzzzz"
			};
		}
	}

	[Obsolete("Use XmlConvert.ToDateTime() that takes in XmlDateTimeSerializationMode")]
	public static DateTime ToDateTime(string s)
	{
		return ToDateTime(s, AllDateTimeFormats);
	}

	public static DateTime ToDateTime(string s, string format)
	{
		return DateTime.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
	}

	public static DateTime ToDateTime(string s, string[] formats)
	{
		return DateTime.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
	}

	public static DateTime ToDateTime(string s, XmlDateTimeSerializationMode dateTimeOption)
	{
		DateTime dateTime = new XsdDateTime(s, XsdDateTimeFlags.AllXsd);
		switch (dateTimeOption)
		{
		case XmlDateTimeSerializationMode.Local:
			dateTime = SwitchToLocalTime(dateTime);
			break;
		case XmlDateTimeSerializationMode.Utc:
			dateTime = SwitchToUtcTime(dateTime);
			break;
		case XmlDateTimeSerializationMode.Unspecified:
			dateTime = new DateTime(dateTime.Ticks, DateTimeKind.Unspecified);
			break;
		default:
			throw new ArgumentException(Res.GetString("The '{0}' value for the 'dateTimeOption' parameter is not an allowed value for the 'XmlDateTimeSerializationMode' enumeration.", dateTimeOption, "dateTimeOption"));
		case XmlDateTimeSerializationMode.RoundtripKind:
			break;
		}
		return dateTime;
	}

	public static DateTimeOffset ToDateTimeOffset(string s)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		return new XsdDateTime(s, XsdDateTimeFlags.AllXsd);
	}

	public static DateTimeOffset ToDateTimeOffset(string s, string format)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		return DateTimeOffset.ParseExact(s, format, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
	}

	public static DateTimeOffset ToDateTimeOffset(string s, string[] formats)
	{
		if (s == null)
		{
			throw new ArgumentNullException("s");
		}
		return DateTimeOffset.ParseExact(s, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite);
	}

	public static Guid ToGuid(string s)
	{
		return new Guid(s);
	}

	internal static Exception TryToGuid(string s, out Guid result)
	{
		Exception result2 = null;
		result = Guid.Empty;
		try
		{
			result = new Guid(s);
		}
		catch (ArgumentException)
		{
			result2 = new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Guid"));
		}
		catch (FormatException)
		{
			result2 = new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Guid"));
		}
		return result2;
	}

	private static DateTime SwitchToLocalTime(DateTime value)
	{
		return value.Kind switch
		{
			DateTimeKind.Local => value, 
			DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Local), 
			DateTimeKind.Utc => value.ToLocalTime(), 
			_ => value, 
		};
	}

	private static DateTime SwitchToUtcTime(DateTime value)
	{
		return value.Kind switch
		{
			DateTimeKind.Utc => value, 
			DateTimeKind.Unspecified => new DateTime(value.Ticks, DateTimeKind.Utc), 
			DateTimeKind.Local => value.ToUniversalTime(), 
			_ => value, 
		};
	}

	internal static Uri ToUri(string s)
	{
		if (s != null && s.Length > 0)
		{
			s = TrimString(s);
			if (s.Length == 0 || s.IndexOf("##", StringComparison.Ordinal) != -1)
			{
				throw new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Uri"));
			}
		}
		if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out var result))
		{
			throw new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Uri"));
		}
		return result;
	}

	internal static Exception TryToUri(string s, out Uri result)
	{
		result = null;
		if (s != null && s.Length > 0)
		{
			s = TrimString(s);
			if (s.Length == 0 || s.IndexOf("##", StringComparison.Ordinal) != -1)
			{
				return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Uri"));
			}
		}
		if (!Uri.TryCreate(s, UriKind.RelativeOrAbsolute, out result))
		{
			return new FormatException(Res.GetString("The string '{0}' is not a valid {1} value.", s, "Uri"));
		}
		return null;
	}

	internal static bool StrEqual(char[] chars, int strPos1, int strLen1, string str2)
	{
		if (strLen1 != str2.Length)
		{
			return false;
		}
		int i;
		for (i = 0; i < strLen1 && chars[strPos1 + i] == str2[i]; i++)
		{
		}
		return i == strLen1;
	}

	internal static string TrimString(string value)
	{
		return value.Trim(WhitespaceChars);
	}

	internal static string TrimStringStart(string value)
	{
		return value.TrimStart(WhitespaceChars);
	}

	internal static string TrimStringEnd(string value)
	{
		return value.TrimEnd(WhitespaceChars);
	}

	internal static string[] SplitString(string value)
	{
		return value.Split(WhitespaceChars, StringSplitOptions.RemoveEmptyEntries);
	}

	internal static string[] SplitString(string value, StringSplitOptions splitStringOptions)
	{
		return value.Split(WhitespaceChars, splitStringOptions);
	}

	internal static bool IsNegativeZero(double value)
	{
		if (value == 0.0 && DoubleToInt64Bits(value) == DoubleToInt64Bits(-0.0))
		{
			return true;
		}
		return false;
	}

	private unsafe static long DoubleToInt64Bits(double value)
	{
		return *(long*)(&value);
	}

	internal static void VerifyCharData(string data, ExceptionType exceptionType)
	{
		VerifyCharData(data, exceptionType, exceptionType);
	}

	internal static void VerifyCharData(string data, ExceptionType invCharExceptionType, ExceptionType invSurrogateExceptionType)
	{
		if (data == null || data.Length == 0)
		{
			return;
		}
		int num = 0;
		int length = data.Length;
		while (true)
		{
			if (num < length && (xmlCharType.charProperties[(uint)data[num]] & 0x10) != 0)
			{
				num++;
				continue;
			}
			if (num == length)
			{
				return;
			}
			if (!XmlCharType.IsHighSurrogate(data[num]))
			{
				break;
			}
			if (num + 1 == length)
			{
				throw CreateException("The surrogate pair is invalid. Missing a low surrogate character.", invSurrogateExceptionType, 0, num + 1);
			}
			if (XmlCharType.IsLowSurrogate(data[num + 1]))
			{
				num += 2;
				continue;
			}
			throw CreateInvalidSurrogatePairException(data[num + 1], data[num], invSurrogateExceptionType, 0, num + 1);
		}
		throw CreateInvalidCharException(data, num, invCharExceptionType);
	}

	internal static void VerifyCharData(char[] data, int offset, int len, ExceptionType exceptionType)
	{
		if (data == null || len == 0)
		{
			return;
		}
		int num = offset;
		int num2 = offset + len;
		while (true)
		{
			if (num < num2 && (xmlCharType.charProperties[(uint)data[num]] & 0x10) != 0)
			{
				num++;
				continue;
			}
			if (num == num2)
			{
				return;
			}
			if (!XmlCharType.IsHighSurrogate(data[num]))
			{
				break;
			}
			if (num + 1 == num2)
			{
				throw CreateException("The surrogate pair is invalid. Missing a low surrogate character.", exceptionType, 0, offset - num + 1);
			}
			if (XmlCharType.IsLowSurrogate(data[num + 1]))
			{
				num += 2;
				continue;
			}
			throw CreateInvalidSurrogatePairException(data[num + 1], data[num], exceptionType, 0, offset - num + 1);
		}
		throw CreateInvalidCharException(data, len, num, exceptionType);
	}

	internal static string EscapeValueForDebuggerDisplay(string value)
	{
		StringBuilder stringBuilder = null;
		int i = 0;
		int num = 0;
		for (; i < value.Length; i++)
		{
			char c = value[i];
			if (c < ' ' || c == '"')
			{
				if (stringBuilder == null)
				{
					stringBuilder = new StringBuilder(value.Length + 4);
				}
				if (i - num > 0)
				{
					stringBuilder.Append(value, num, i - num);
				}
				num = i + 1;
				switch (c)
				{
				case '"':
					stringBuilder.Append("\\\"");
					break;
				case '\r':
					stringBuilder.Append("\\r");
					break;
				case '\n':
					stringBuilder.Append("\\n");
					break;
				case '\t':
					stringBuilder.Append("\\t");
					break;
				default:
					stringBuilder.Append(c);
					break;
				}
			}
		}
		if (stringBuilder == null)
		{
			return value;
		}
		if (i - num > 0)
		{
			stringBuilder.Append(value, num, i - num);
		}
		return stringBuilder.ToString();
	}

	internal static Exception CreateException(string res, ExceptionType exceptionType)
	{
		return CreateException(res, exceptionType, 0, 0);
	}

	internal static Exception CreateException(string res, ExceptionType exceptionType, int lineNo, int linePos)
	{
		return exceptionType switch
		{
			ExceptionType.ArgumentException => new ArgumentException(Res.GetString(res)), 
			_ => new XmlException(res, string.Empty, lineNo, linePos), 
		};
	}

	internal static Exception CreateException(string res, string arg, ExceptionType exceptionType)
	{
		return CreateException(res, arg, exceptionType, 0, 0);
	}

	internal static Exception CreateException(string res, string arg, ExceptionType exceptionType, int lineNo, int linePos)
	{
		return exceptionType switch
		{
			ExceptionType.ArgumentException => new ArgumentException(Res.GetString(res, arg)), 
			_ => new XmlException(res, arg, lineNo, linePos), 
		};
	}

	internal static Exception CreateException(string res, string[] args, ExceptionType exceptionType)
	{
		return CreateException(res, args, exceptionType, 0, 0);
	}

	internal static Exception CreateException(string res, string[] args, ExceptionType exceptionType, int lineNo, int linePos)
	{
		switch (exceptionType)
		{
		case ExceptionType.ArgumentException:
			return new ArgumentException(Res.GetString(res, args));
		default:
			return new XmlException(res, args, lineNo, linePos);
		}
	}

	internal static Exception CreateInvalidSurrogatePairException(char low, char hi)
	{
		return CreateInvalidSurrogatePairException(low, hi, ExceptionType.ArgumentException);
	}

	internal static Exception CreateInvalidSurrogatePairException(char low, char hi, ExceptionType exceptionType)
	{
		return CreateInvalidSurrogatePairException(low, hi, exceptionType, 0, 0);
	}

	internal static Exception CreateInvalidSurrogatePairException(char low, char hi, ExceptionType exceptionType, int lineNo, int linePos)
	{
		string[] array = new string[2];
		uint num = hi;
		array[0] = num.ToString("X", CultureInfo.InvariantCulture);
		num = low;
		array[1] = num.ToString("X", CultureInfo.InvariantCulture);
		string[] args = array;
		return CreateException("The surrogate pair (0x{0}, 0x{1}) is invalid. A high surrogate character (0xD800 - 0xDBFF) must always be paired with a low surrogate character (0xDC00 - 0xDFFF).", args, exceptionType, lineNo, linePos);
	}

	internal static Exception CreateInvalidHighSurrogateCharException(char hi)
	{
		return CreateInvalidHighSurrogateCharException(hi, ExceptionType.ArgumentException);
	}

	internal static Exception CreateInvalidHighSurrogateCharException(char hi, ExceptionType exceptionType)
	{
		return CreateInvalidHighSurrogateCharException(hi, exceptionType, 0, 0);
	}

	internal static Exception CreateInvalidHighSurrogateCharException(char hi, ExceptionType exceptionType, int lineNo, int linePos)
	{
		uint num = hi;
		return CreateException("Invalid high surrogate character (0x{0}). A high surrogate character must have a value from range (0xD800 - 0xDBFF).", num.ToString("X", CultureInfo.InvariantCulture), exceptionType, lineNo, linePos);
	}

	internal static Exception CreateInvalidCharException(char[] data, int length, int invCharPos)
	{
		return CreateInvalidCharException(data, length, invCharPos, ExceptionType.ArgumentException);
	}

	internal static Exception CreateInvalidCharException(char[] data, int length, int invCharPos, ExceptionType exceptionType)
	{
		return CreateException("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, length, invCharPos), exceptionType, 0, invCharPos + 1);
	}

	internal static Exception CreateInvalidCharException(string data, int invCharPos)
	{
		return CreateInvalidCharException(data, invCharPos, ExceptionType.ArgumentException);
	}

	internal static Exception CreateInvalidCharException(string data, int invCharPos, ExceptionType exceptionType)
	{
		return CreateException("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(data, invCharPos), exceptionType, 0, invCharPos + 1);
	}

	internal static Exception CreateInvalidCharException(char invChar, char nextChar)
	{
		return CreateInvalidCharException(invChar, nextChar, ExceptionType.ArgumentException);
	}

	internal static Exception CreateInvalidCharException(char invChar, char nextChar, ExceptionType exceptionType)
	{
		return CreateException("'{0}', hexadecimal value {1}, is an invalid character.", XmlException.BuildCharExceptionArgs(invChar, nextChar), exceptionType);
	}

	internal static Exception CreateInvalidNameCharException(string name, int index, ExceptionType exceptionType)
	{
		return CreateException((index == 0) ? "Name cannot begin with the '{0}' character, hexadecimal value {1}." : "The '{0}' character, hexadecimal value {1}, cannot be included in a name.", XmlException.BuildCharExceptionArgs(name, index), exceptionType, 0, index + 1);
	}

	internal static ArgumentException CreateInvalidNameArgumentException(string name, string argumentName)
	{
		if (name != null)
		{
			return new ArgumentException(Res.GetString("The empty string '' is not a valid name."), argumentName);
		}
		return new ArgumentNullException(argumentName);
	}
}
