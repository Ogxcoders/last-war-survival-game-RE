using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web.Util;

namespace System.Web;

public sealed class HttpUtility
{
	private sealed class HttpQSCollection : NameValueCollection
	{
		public override string ToString()
		{
			int count = Count;
			if (count == 0)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			string[] allKeys = AllKeys;
			for (int i = 0; i < count; i++)
			{
				stringBuilder.AppendFormat("{0}={1}&", allKeys[i], UrlEncode(base[allKeys[i]]));
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Length--;
			}
			return stringBuilder.ToString();
		}
	}

	public static void HtmlAttributeEncode(string s, TextWriter output)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		HttpEncoder.Current.HtmlAttributeEncode(s, output);
	}

	public static string HtmlAttributeEncode(string s)
	{
		if (s == null)
		{
			return null;
		}
		using StringWriter stringWriter = new StringWriter();
		HttpEncoder.Current.HtmlAttributeEncode(s, stringWriter);
		return stringWriter.ToString();
	}

	public static string UrlDecode(string str)
	{
		return UrlDecode(str, Encoding.UTF8);
	}

	private static char[] GetChars(MemoryStream b, Encoding e)
	{
		return e.GetChars(b.GetBuffer(), 0, (int)b.Length);
	}

	private static void WriteCharBytes(IList buf, char ch, Encoding e)
	{
		if (ch > 'ÿ')
		{
			byte[] bytes = e.GetBytes(new char[1] { ch });
			foreach (byte b in bytes)
			{
				buf.Add(b);
			}
		}
		else
		{
			buf.Add((byte)ch);
		}
	}

	public static string UrlDecode(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		if (str.IndexOf('%') == -1 && str.IndexOf('+') == -1)
		{
			return str;
		}
		if (e == null)
		{
			e = Encoding.UTF8;
		}
		long num = str.Length;
		List<byte> list = new List<byte>();
		for (int i = 0; i < num; i++)
		{
			char c = str[i];
			if (c == '%' && i + 2 < num && str[i + 1] != '%')
			{
				int num2;
				if (str[i + 1] == 'u' && i + 5 < num)
				{
					num2 = GetChar(str, i + 2, 4);
					if (num2 != -1)
					{
						WriteCharBytes(list, (char)num2, e);
						i += 5;
					}
					else
					{
						WriteCharBytes(list, '%', e);
					}
				}
				else if ((num2 = GetChar(str, i + 1, 2)) != -1)
				{
					WriteCharBytes(list, (char)num2, e);
					i += 2;
				}
				else
				{
					WriteCharBytes(list, '%', e);
				}
			}
			else if (c == '+')
			{
				WriteCharBytes(list, ' ', e);
			}
			else
			{
				WriteCharBytes(list, c, e);
			}
		}
		byte[] bytes = list.ToArray();
		list = null;
		return e.GetString(bytes);
	}

	public static string UrlDecode(byte[] bytes, Encoding e)
	{
		if (bytes == null)
		{
			return null;
		}
		return UrlDecode(bytes, 0, bytes.Length, e);
	}

	private static int GetInt(byte b)
	{
		char c = (char)b;
		if (c >= '0' && c <= '9')
		{
			return c - 48;
		}
		if (c >= 'a' && c <= 'f')
		{
			return c - 97 + 10;
		}
		if (c >= 'A' && c <= 'F')
		{
			return c - 65 + 10;
		}
		return -1;
	}

	private static int GetChar(byte[] bytes, int offset, int length)
	{
		int num = 0;
		int num2 = length + offset;
		for (int i = offset; i < num2; i++)
		{
			int num3 = GetInt(bytes[i]);
			if (num3 == -1)
			{
				return -1;
			}
			num = (num << 4) + num3;
		}
		return num;
	}

	private static int GetChar(string str, int offset, int length)
	{
		int num = 0;
		int num2 = length + offset;
		for (int i = offset; i < num2; i++)
		{
			char c = str[i];
			if (c > '\u007f')
			{
				return -1;
			}
			int num3 = GetInt((byte)c);
			if (num3 == -1)
			{
				return -1;
			}
			num = (num << 4) + num3;
		}
		return num;
	}

	public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
	{
		if (bytes == null)
		{
			return null;
		}
		if (count == 0)
		{
			return string.Empty;
		}
		if (bytes == null)
		{
			throw new ArgumentNullException("bytes");
		}
		if (offset < 0 || offset > bytes.Length)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || offset + count > bytes.Length)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		StringBuilder stringBuilder = new StringBuilder();
		MemoryStream memoryStream = new MemoryStream();
		int num = count + offset;
		for (int i = offset; i < num; i++)
		{
			if (bytes[i] == 37 && i + 2 < count && bytes[i + 1] != 37)
			{
				int num2;
				if (bytes[i + 1] == 117 && i + 5 < num)
				{
					if (memoryStream.Length > 0)
					{
						stringBuilder.Append(GetChars(memoryStream, e));
						memoryStream.SetLength(0L);
					}
					num2 = GetChar(bytes, i + 2, 4);
					if (num2 != -1)
					{
						stringBuilder.Append((char)num2);
						i += 5;
						continue;
					}
				}
				else if ((num2 = GetChar(bytes, i + 1, 2)) != -1)
				{
					memoryStream.WriteByte((byte)num2);
					i += 2;
					continue;
				}
			}
			if (memoryStream.Length > 0)
			{
				stringBuilder.Append(GetChars(memoryStream, e));
				memoryStream.SetLength(0L);
			}
			if (bytes[i] == 43)
			{
				stringBuilder.Append(' ');
			}
			else
			{
				stringBuilder.Append((char)bytes[i]);
			}
		}
		if (memoryStream.Length > 0)
		{
			stringBuilder.Append(GetChars(memoryStream, e));
		}
		memoryStream = null;
		return stringBuilder.ToString();
	}

	public static byte[] UrlDecodeToBytes(byte[] bytes)
	{
		if (bytes == null)
		{
			return null;
		}
		return UrlDecodeToBytes(bytes, 0, bytes.Length);
	}

	public static byte[] UrlDecodeToBytes(string str)
	{
		return UrlDecodeToBytes(str, Encoding.UTF8);
	}

	public static byte[] UrlDecodeToBytes(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		return UrlDecodeToBytes(e.GetBytes(str));
	}

	public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
	{
		if (bytes == null)
		{
			return null;
		}
		if (count == 0)
		{
			return new byte[0];
		}
		int num = bytes.Length;
		if (offset < 0 || offset >= num)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (count < 0 || offset > num - count)
		{
			throw new ArgumentOutOfRangeException("count");
		}
		MemoryStream memoryStream = new MemoryStream();
		int num2 = offset + count;
		for (int i = offset; i < num2; i++)
		{
			char c = (char)bytes[i];
			switch (c)
			{
			case '+':
				c = ' ';
				break;
			case '%':
				if (i < num2 - 2)
				{
					int num3 = GetChar(bytes, i + 1, 2);
					if (num3 != -1)
					{
						c = (char)num3;
						i += 2;
					}
				}
				break;
			}
			memoryStream.WriteByte((byte)c);
		}
		return memoryStream.ToArray();
	}

	public static string UrlEncode(string str)
	{
		return UrlEncode(str, Encoding.UTF8);
	}

	public static string UrlEncode(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		if (str == string.Empty)
		{
			return string.Empty;
		}
		bool flag = false;
		int length = str.Length;
		for (int i = 0; i < length; i++)
		{
			char c = str[i];
			if ((c < '0' || (c < 'A' && c > '9') || (c > 'Z' && c < 'a') || c > 'z') && !HttpEncoder.NotEncoded(c))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return str;
		}
		byte[] bytes = new byte[e.GetMaxByteCount(str.Length)];
		int bytes2 = e.GetBytes(str, 0, str.Length, bytes, 0);
		return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes2));
	}

	public static string UrlEncode(byte[] bytes)
	{
		if (bytes == null)
		{
			return null;
		}
		if (bytes.Length == 0)
		{
			return string.Empty;
		}
		return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, 0, bytes.Length));
	}

	public static string UrlEncode(byte[] bytes, int offset, int count)
	{
		if (bytes == null)
		{
			return null;
		}
		if (bytes.Length == 0)
		{
			return string.Empty;
		}
		return Encoding.ASCII.GetString(UrlEncodeToBytes(bytes, offset, count));
	}

	public static byte[] UrlEncodeToBytes(string str)
	{
		return UrlEncodeToBytes(str, Encoding.UTF8);
	}

	public static byte[] UrlEncodeToBytes(string str, Encoding e)
	{
		if (str == null)
		{
			return null;
		}
		if (str.Length == 0)
		{
			return new byte[0];
		}
		byte[] bytes = e.GetBytes(str);
		return UrlEncodeToBytes(bytes, 0, bytes.Length);
	}

	public static byte[] UrlEncodeToBytes(byte[] bytes)
	{
		if (bytes == null)
		{
			return null;
		}
		if (bytes.Length == 0)
		{
			return new byte[0];
		}
		return UrlEncodeToBytes(bytes, 0, bytes.Length);
	}

	public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
	{
		if (bytes == null)
		{
			return null;
		}
		return HttpEncoder.Current.UrlEncode(bytes, offset, count);
	}

	public static string UrlEncodeUnicode(string str)
	{
		if (str == null)
		{
			return null;
		}
		return Encoding.ASCII.GetString(UrlEncodeUnicodeToBytes(str));
	}

	public static byte[] UrlEncodeUnicodeToBytes(string str)
	{
		if (str == null)
		{
			return null;
		}
		if (str.Length == 0)
		{
			return new byte[0];
		}
		MemoryStream memoryStream = new MemoryStream(str.Length);
		for (int i = 0; i < str.Length; i++)
		{
			HttpEncoder.UrlEncodeChar(str[i], memoryStream, isUnicode: true);
		}
		return memoryStream.ToArray();
	}

	public static string HtmlDecode(string s)
	{
		if (s == null)
		{
			return null;
		}
		using StringWriter stringWriter = new StringWriter();
		HttpEncoder.Current.HtmlDecode(s, stringWriter);
		return stringWriter.ToString();
	}

	public static void HtmlDecode(string s, TextWriter output)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (!string.IsNullOrEmpty(s))
		{
			HttpEncoder.Current.HtmlDecode(s, output);
		}
	}

	public static string HtmlEncode(string s)
	{
		if (s == null)
		{
			return null;
		}
		using StringWriter stringWriter = new StringWriter();
		HttpEncoder.Current.HtmlEncode(s, stringWriter);
		return stringWriter.ToString();
	}

	public static void HtmlEncode(string s, TextWriter output)
	{
		if (output == null)
		{
			throw new ArgumentNullException("output");
		}
		if (!string.IsNullOrEmpty(s))
		{
			HttpEncoder.Current.HtmlEncode(s, output);
		}
	}

	public static string HtmlEncode(object value)
	{
		if (value == null)
		{
			return null;
		}
		return HtmlEncode(value.ToString());
	}

	public static string JavaScriptStringEncode(string value)
	{
		return JavaScriptStringEncode(value, addDoubleQuotes: false);
	}

	public static string JavaScriptStringEncode(string value, bool addDoubleQuotes)
	{
		if (string.IsNullOrEmpty(value))
		{
			if (!addDoubleQuotes)
			{
				return string.Empty;
			}
			return "\"\"";
		}
		int length = value.Length;
		bool flag = false;
		for (int i = 0; i < length; i++)
		{
			char c = value[i];
			if ((c >= '\0' && c <= '\u001f') || c == '"' || c == '\'' || c == '<' || c == '>' || c == '\\')
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			if (!addDoubleQuotes)
			{
				return value;
			}
			return "\"" + value + "\"";
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (addDoubleQuotes)
		{
			stringBuilder.Append('"');
		}
		for (int j = 0; j < length; j++)
		{
			char c = value[j];
			if (c < '\0' || c > '\a')
			{
				switch (c)
				{
				default:
					if (c != '\'' && c != '<' && c != '>')
					{
						switch (c)
						{
						case 8:
							stringBuilder.Append("\\b");
							break;
						case 9:
							stringBuilder.Append("\\t");
							break;
						case 10:
							stringBuilder.Append("\\n");
							break;
						case 12:
							stringBuilder.Append("\\f");
							break;
						case 13:
							stringBuilder.Append("\\r");
							break;
						case 34:
							stringBuilder.Append("\\\"");
							break;
						case 92:
							stringBuilder.Append("\\\\");
							break;
						default:
							stringBuilder.Append(c);
							break;
						}
						continue;
					}
					break;
				case '\v':
				case '\u000e':
				case '\u000f':
				case '\u0010':
				case '\u0011':
				case '\u0012':
				case '\u0013':
				case '\u0014':
				case '\u0015':
				case '\u0016':
				case '\u0017':
				case '\u0018':
				case '\u0019':
				case '\u001a':
				case '\u001b':
				case '\u001c':
				case '\u001d':
				case '\u001e':
				case '\u001f':
					break;
				}
			}
			stringBuilder.AppendFormat("\\u{0:x4}", (int)c);
		}
		if (addDoubleQuotes)
		{
			stringBuilder.Append('"');
		}
		return stringBuilder.ToString();
	}

	public static string UrlPathEncode(string str)
	{
		return HttpEncoder.Current.UrlPathEncode(str);
	}

	public static NameValueCollection ParseQueryString(string query)
	{
		return ParseQueryString(query, Encoding.UTF8);
	}

	public static NameValueCollection ParseQueryString(string query, Encoding encoding)
	{
		if (query == null)
		{
			throw new ArgumentNullException("query");
		}
		if (encoding == null)
		{
			throw new ArgumentNullException("encoding");
		}
		if (query.Length == 0 || (query.Length == 1 && query[0] == '?'))
		{
			return new HttpQSCollection();
		}
		if (query[0] == '?')
		{
			query = query.Substring(1);
		}
		NameValueCollection result = new HttpQSCollection();
		ParseQueryString(query, encoding, result);
		return result;
	}

	internal static void ParseQueryString(string query, Encoding encoding, NameValueCollection result)
	{
		if (query.Length == 0)
		{
			return;
		}
		string text = HtmlDecode(query);
		int length = text.Length;
		int num = 0;
		bool flag = true;
		while (num <= length)
		{
			int num2 = -1;
			int num3 = -1;
			for (int i = num; i < length; i++)
			{
				if (num2 == -1 && text[i] == '=')
				{
					num2 = i + 1;
				}
				else if (text[i] == '&')
				{
					num3 = i;
					break;
				}
			}
			if (flag)
			{
				flag = false;
				if (text[num] == '?')
				{
					num++;
				}
			}
			string name;
			if (num2 == -1)
			{
				name = null;
				num2 = num;
			}
			else
			{
				name = UrlDecode(text.Substring(num, num2 - num - 1), encoding);
			}
			if (num3 < 0)
			{
				num = -1;
				num3 = text.Length;
			}
			else
			{
				num = num3 + 1;
			}
			string value = UrlDecode(text.Substring(num2, num3 - num2), encoding);
			result.Add(name, value);
			if (num == -1)
			{
				break;
			}
		}
	}
}
