using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Text;

namespace System.Runtime;

internal static class UrlUtility
{
	private class UrlDecoder
	{
		private int _bufferSize;

		private int _numChars;

		private char[] _charBuffer;

		private int _numBytes;

		private byte[] _byteBuffer;

		private Encoding _encoding;

		private void FlushBytes()
		{
			if (_numBytes > 0)
			{
				_numChars += _encoding.GetChars(_byteBuffer, 0, _numBytes, _charBuffer, _numChars);
				_numBytes = 0;
			}
		}

		internal UrlDecoder(int bufferSize, Encoding encoding)
		{
			_bufferSize = bufferSize;
			_encoding = encoding;
			_charBuffer = new char[bufferSize];
		}

		internal void AddChar(char ch)
		{
			if (_numBytes > 0)
			{
				FlushBytes();
			}
			_charBuffer[_numChars++] = ch;
		}

		internal void AddByte(byte b)
		{
			if (_byteBuffer == null)
			{
				_byteBuffer = new byte[_bufferSize];
			}
			_byteBuffer[_numBytes++] = b;
		}

		internal string GetString()
		{
			if (_numBytes > 0)
			{
				FlushBytes();
			}
			if (_numChars > 0)
			{
				return new string(_charBuffer, 0, _numChars);
			}
			return string.Empty;
		}
	}

	[Serializable]
	private class HttpValueCollection : NameValueCollection
	{
		internal HttpValueCollection(string str, Encoding encoding)
			: base(StringComparer.OrdinalIgnoreCase)
		{
			if (!string.IsNullOrEmpty(str))
			{
				FillFromString(str, urlencoded: true, encoding);
			}
			base.IsReadOnly = false;
		}

		protected HttpValueCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		internal void FillFromString(string s, bool urlencoded, Encoding encoding)
		{
			int num = s?.Length ?? 0;
			for (int i = 0; i < num; i++)
			{
				int num2 = i;
				int num3 = -1;
				for (; i < num; i++)
				{
					switch (s[i])
					{
					case '=':
						if (num3 < 0)
						{
							num3 = i;
						}
						continue;
					default:
						continue;
					case '&':
						break;
					}
					break;
				}
				string text = null;
				string text2 = null;
				if (num3 >= 0)
				{
					text = s.Substring(num2, num3 - num2);
					text2 = s.Substring(num3 + 1, i - num3 - 1);
				}
				else
				{
					text2 = s.Substring(num2, i - num2);
				}
				if (urlencoded)
				{
					base.Add(UrlDecode(text, encoding), UrlDecode(text2, encoding));
				}
				else
				{
					base.Add(text, text2);
				}
				if (i == num - 1 && s[i] == '&')
				{
					base.Add(null, string.Empty);
				}
			}
		}

		public override string ToString()
		{
			return ToString(urlencoded: true, null);
		}

		private string ToString(bool urlencoded, IDictionary excludeKeys)
		{
			int count = Count;
			if (count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				string text = GetKey(i);
				if (excludeKeys != null && text != null && excludeKeys[text] != null)
				{
					continue;
				}
				if (urlencoded)
				{
					text = UrlEncodeUnicode(text);
				}
				string value = ((!string.IsNullOrEmpty(text)) ? (text + "=") : string.Empty);
				ArrayList arrayList = (ArrayList)BaseGet(i);
				int num = arrayList?.Count ?? 0;
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append('&');
				}
				switch (num)
				{
				case 1:
				{
					stringBuilder.Append(value);
					string text2 = (string)arrayList[0];
					if (urlencoded)
					{
						text2 = UrlEncodeUnicode(text2);
					}
					stringBuilder.Append(text2);
					continue;
				}
				case 0:
					stringBuilder.Append(value);
					continue;
				}
				for (int j = 0; j < num; j++)
				{
					if (j > 0)
					{
						stringBuilder.Append('&');
					}
					stringBuilder.Append(value);
					string text2 = (string)arrayList[j];
					if (urlencoded)
					{
						text2 = UrlEncodeUnicode(text2);
					}
					stringBuilder.Append(text2);
				}
			}
			return stringBuilder.ToString();
		}
	}

	public static NameValueCollection ParseQueryString(string query)
	{
		return ParseQueryString(query, Encoding.UTF8);
	}

	public static NameValueCollection ParseQueryString(string query, Encoding encoding)
	{
		if (query == null)
		{
			throw Fx.Exception.ArgumentNull("query");
		}
		if (encoding == null)
		{
			throw Fx.Exception.ArgumentNull("encoding");
		}
		if (query.Length > 0 && query[0] == '?')
		{
			query = query.Substring(1);
		}
		return new HttpValueCollection(query, encoding);
	}

	public static string UrlEncode(string str)
	{
		if (str == null)
		{
			return null;
		}
		return UrlEncode(str, Encoding.UTF8);
	}

	public static string UrlPathEncode(string str)
	{
		if (str == null)
		{
			return null;
		}
		int num = str.IndexOf('?');
		if (num >= 0)
		{
			return UrlPathEncode(str.Substring(0, num)) + str.Substring(num);
		}
		return UrlEncodeSpaces(UrlEncodeNonAscii(str, Encoding.UTF8));
	}

	public static string UrlEncode(string str, Encoding encoding)
	{
		if (str == null)
		{
			return null;
		}
		return Encoding.ASCII.GetString(UrlEncodeToBytes(str, encoding));
	}

	public static string UrlEncodeUnicode(string str)
	{
		if (str == null)
		{
			return null;
		}
		return UrlEncodeUnicodeStringToStringInternal(str, ignoreAscii: false);
	}

	private static string UrlEncodeUnicodeStringToStringInternal(string s, bool ignoreAscii)
	{
		int length = s.Length;
		StringBuilder stringBuilder = new StringBuilder(length);
		for (int i = 0; i < length; i++)
		{
			char c = s[i];
			if ((c & 0xFF80) == 0)
			{
				if (ignoreAscii || IsSafe(c))
				{
					stringBuilder.Append(c);
					continue;
				}
				if (c == ' ')
				{
					stringBuilder.Append('+');
					continue;
				}
				stringBuilder.Append('%');
				stringBuilder.Append(IntToHex(((int)c >> 4) & 0xF));
				stringBuilder.Append(IntToHex(c & 0xF));
			}
			else
			{
				stringBuilder.Append("%u");
				stringBuilder.Append(IntToHex(((int)c >> 12) & 0xF));
				stringBuilder.Append(IntToHex(((int)c >> 8) & 0xF));
				stringBuilder.Append(IntToHex(((int)c >> 4) & 0xF));
				stringBuilder.Append(IntToHex(c & 0xF));
			}
		}
		return stringBuilder.ToString();
	}

	private static string UrlEncodeNonAscii(string str, Encoding e)
	{
		if (string.IsNullOrEmpty(str))
		{
			return str;
		}
		if (e == null)
		{
			e = Encoding.UTF8;
		}
		byte[] bytes = e.GetBytes(str);
		bytes = UrlEncodeBytesToBytesInternalNonAscii(bytes, 0, bytes.Length, alwaysCreateReturnValue: false);
		return Encoding.ASCII.GetString(bytes);
	}

	private static string UrlEncodeSpaces(string str)
	{
		if (str != null && str.IndexOf(' ') >= 0)
		{
			str = str.Replace(" ", "%20");
		}
		return str;
	}

	public static byte[] UrlEncodeToBytes(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		byte[] bytes = e.GetBytes(str);
		return UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, alwaysCreateReturnValue: false);
	}

	public static string UrlDecode(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		return UrlDecodeStringFromStringInternal(str, e);
	}

	private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < count; i++)
		{
			char c = (char)bytes[offset + i];
			if (c == ' ')
			{
				num++;
			}
			else if (!IsSafe(c))
			{
				num2++;
			}
		}
		if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
		{
			return bytes;
		}
		byte[] array = new byte[count + num2 * 2];
		int num3 = 0;
		for (int j = 0; j < count; j++)
		{
			byte b = bytes[offset + j];
			char c2 = (char)b;
			if (IsSafe(c2))
			{
				array[num3++] = b;
				continue;
			}
			if (c2 == ' ')
			{
				array[num3++] = 43;
				continue;
			}
			array[num3++] = 37;
			array[num3++] = (byte)IntToHex((b >> 4) & 0xF);
			array[num3++] = (byte)IntToHex(b & 0xF);
		}
		return array;
	}

	private static bool IsNonAsciiByte(byte b)
	{
		if (b < 127)
		{
			return b < 32;
		}
		return true;
	}

	private static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
	{
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			if (IsNonAsciiByte(bytes[offset + i]))
			{
				num++;
			}
		}
		if (!alwaysCreateReturnValue && num == 0)
		{
			return bytes;
		}
		byte[] array = new byte[count + num * 2];
		int num2 = 0;
		for (int j = 0; j < count; j++)
		{
			byte b = bytes[offset + j];
			if (IsNonAsciiByte(b))
			{
				array[num2++] = 37;
				array[num2++] = (byte)IntToHex((b >> 4) & 0xF);
				array[num2++] = (byte)IntToHex(b & 0xF);
			}
			else
			{
				array[num2++] = b;
			}
		}
		return array;
	}

	private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
	{
		int length = s.Length;
		UrlDecoder urlDecoder = new UrlDecoder(length, e);
		for (int i = 0; i < length; i++)
		{
			char c = s[i];
			switch (c)
			{
			case '+':
				c = ' ';
				break;
			case '%':
				if (i >= length - 2)
				{
					break;
				}
				if (s[i + 1] == 'u' && i < length - 5)
				{
					int num = HexToInt(s[i + 2]);
					int num2 = HexToInt(s[i + 3]);
					int num3 = HexToInt(s[i + 4]);
					int num4 = HexToInt(s[i + 5]);
					if (num >= 0 && num2 >= 0 && num3 >= 0 && num4 >= 0)
					{
						c = (char)((num << 12) | (num2 << 8) | (num3 << 4) | num4);
						i += 5;
						urlDecoder.AddChar(c);
						continue;
					}
				}
				else
				{
					int num5 = HexToInt(s[i + 1]);
					int num6 = HexToInt(s[i + 2]);
					if (num5 >= 0 && num6 >= 0)
					{
						byte b = (byte)((num5 << 4) | num6);
						i += 2;
						urlDecoder.AddByte(b);
						continue;
					}
				}
				break;
			}
			if ((c & 0xFF80) == 0)
			{
				urlDecoder.AddByte((byte)c);
			}
			else
			{
				urlDecoder.AddChar(c);
			}
		}
		return urlDecoder.GetString();
	}

	private static int HexToInt(char h)
	{
		if (h < '0' || h > '9')
		{
			if (h < 'a' || h > 'f')
			{
				if (h < 'A' || h > 'F')
				{
					return -1;
				}
				return h - 65 + 10;
			}
			return h - 97 + 10;
		}
		return h - 48;
	}

	private static char IntToHex(int n)
	{
		if (n <= 9)
		{
			return (char)(n + 48);
		}
		return (char)(n - 10 + 97);
	}

	internal static bool IsSafe(char ch)
	{
		if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
		{
			return true;
		}
		switch (ch)
		{
		case '!':
		case '\'':
		case '(':
		case ')':
		case '*':
		case '-':
		case '.':
		case '_':
			return true;
		default:
			return false;
		}
	}
}
