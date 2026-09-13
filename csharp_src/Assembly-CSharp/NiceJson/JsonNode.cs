using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NiceJson;

[Serializable]
public abstract class JsonNode
{
	protected const char PP_IDENT_CHAR = '\t';

	protected const int PP_IDENT_COUNT = 1;

	protected const bool ESCAPE_SOLIDUS = false;

	protected const char CHAR_CURLY_OPEN = '{';

	protected const char CHAR_CURLY_CLOSED = '}';

	protected const char CHAR_SQUARED_OPEN = '[';

	protected const char CHAR_SQUARED_CLOSED = ']';

	protected const char CHAR_COLON = ':';

	protected const char CHAR_COMMA = ',';

	protected const char CHAR_QUOTE = '"';

	protected const char CHAR_NULL_LITERAL = 'n';

	protected const char CHAR_TRUE_LITERAL = 't';

	protected const char CHAR_FALSE_LITERAL = 'f';

	protected const char CHAR_SPACE = ' ';

	protected const char CHAR_BS = '\b';

	protected const char CHAR_FF = '\f';

	protected const char CHAR_RF = '\r';

	protected const char CHAR_NL = '\n';

	protected const char CHAR_HT = '\t';

	protected const char CHAR_ESCAPE = '\\';

	protected const char CHAR_SOLIDUS = '/';

	protected const char CHAR_ESCAPED_QUOTE = '"';

	protected const char CHAR_N = 'n';

	protected const char CHAR_R = 'r';

	protected const char CHAR_B = 'b';

	protected const char CHAR_T = 't';

	protected const char CHAR_F = 'f';

	protected const char CHAR_U = 'u';

	protected const string STRING_ESCAPED_BS = "\\b";

	protected const string STRING_ESCAPED_FF = "\\f";

	protected const string STRING_ESCAPED_RF = "\\r";

	protected const string STRING_ESCAPED_NL = "\\n";

	protected const string STRING_ESCAPED_TAB = "\\t";

	protected const string STRING_ESCAPED_ESCAPE = "\\\\";

	protected const string STRING_ESCAPED_SOLIDUS = "\\/";

	protected const string STRING_ESCAPED_ESCAPED_QUOTE = "\\\"";

	protected const string STRING_SPACE = " ";

	protected const string STRING_LITERAL_NULL = "null";

	protected const string STRING_LITERAL_TRUE = "true";

	protected const string STRING_LITERAL_FALSE = "false";

	protected const string STRING_ESCAPED_UNICODE_INIT = "\\u00";

	public JsonNode this[string key]
	{
		get
		{
			if (this is JsonObject)
			{
				return ((JsonObject)this)[key];
			}
			return null;
		}
		set
		{
			if (this is JsonObject)
			{
				((JsonObject)this)[key] = value;
			}
		}
	}

	public JsonNode this[int index]
	{
		get
		{
			if (this is JsonArray)
			{
				return ((JsonArray)this)[index];
			}
			return null;
		}
		set
		{
			if (this is JsonArray)
			{
				((JsonArray)this)[index] = value;
			}
		}
	}

	public bool ContainsKey(string key)
	{
		if (this is JsonObject)
		{
			return ((JsonObject)this).ContainsKey(key);
		}
		return false;
	}

	protected static string EscapeString(string s)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in s)
		{
			switch (c)
			{
			case '\\':
				stringBuilder.Append("\\\\");
				continue;
			case '/':
				stringBuilder.Append(c);
				continue;
			case '"':
				stringBuilder.Append("\\\"");
				continue;
			case '\n':
				stringBuilder.Append("\\n");
				continue;
			case '\r':
				stringBuilder.Append("\\r");
				continue;
			case '\t':
				stringBuilder.Append("\\t");
				continue;
			case '\b':
				stringBuilder.Append("\\b");
				continue;
			case '\f':
				stringBuilder.Append("\\f");
				continue;
			}
			if (c < ' ')
			{
				stringBuilder.Append("\\u00" + Convert.ToByte(c).ToString("x2").ToUpper());
			}
			else
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	protected static string UnescapeString(string s)
	{
		StringBuilder stringBuilder = new StringBuilder(s.Length);
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == '\\')
			{
				i++;
				switch (s[i])
				{
				case '\\':
					stringBuilder.Append(s[i]);
					break;
				case '/':
					stringBuilder.Append(s[i]);
					break;
				case '"':
					stringBuilder.Append(s[i]);
					break;
				case 'n':
					stringBuilder.Append('\n');
					break;
				case 'r':
					stringBuilder.Append('\r');
					break;
				case 't':
					stringBuilder.Append('\t');
					break;
				case 'b':
					stringBuilder.Append('\b');
					break;
				case 'f':
					stringBuilder.Append('\f');
					break;
				case 'u':
					stringBuilder.Append((char)int.Parse(s.Substring(i + 1, 4), NumberStyles.AllowHexSpecifier));
					i += 4;
					break;
				default:
					stringBuilder.Append(s[i]);
					break;
				}
			}
			else
			{
				stringBuilder.Append(s[i]);
			}
		}
		return stringBuilder.ToString();
	}

	public static implicit operator JsonNode(string value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator JsonNode(int value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator JsonNode(long value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator JsonNode(float value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator JsonNode(double value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator JsonNode(decimal value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator JsonNode(bool value)
	{
		return new JsonBasic(value);
	}

	public static implicit operator string(JsonNode value)
	{
		return value?.ToString();
	}

	public static implicit operator int(JsonNode value)
	{
		return (int)Convert.ChangeType(((JsonBasic)value).ValueObject, typeof(int));
	}

	public static implicit operator long(JsonNode value)
	{
		return (long)Convert.ChangeType(((JsonBasic)value).ValueObject, typeof(long));
	}

	public static implicit operator float(JsonNode value)
	{
		return (float)Convert.ChangeType(((JsonBasic)value).ValueObject, typeof(float));
	}

	public static implicit operator double(JsonNode value)
	{
		return (double)Convert.ChangeType(((JsonBasic)value).ValueObject, typeof(double));
	}

	public static implicit operator decimal(JsonNode value)
	{
		return (decimal)Convert.ChangeType(((JsonBasic)value).ValueObject, typeof(decimal));
	}

	public static implicit operator bool(JsonNode value)
	{
		return (bool)Convert.ChangeType(((JsonBasic)value).ValueObject, typeof(bool));
	}

	public static JsonNode ParseJsonString(string jsonString)
	{
		return ParseJsonPart(RemoveNonTokenChars(jsonString));
	}

	private static JsonNode ParseJsonPart(string jsonPart)
	{
		JsonNode result = null;
		if (jsonPart.Length == 0)
		{
			return result;
		}
		switch (jsonPart[0])
		{
		case '{':
		{
			JsonObject jsonObject = new JsonObject();
			List<string> list = SplitJsonParts(jsonPart.Substring(1, jsonPart.Length - 2));
			string[] array = new string[2];
			foreach (string item in list)
			{
				array = SplitKeyValuePart(item);
				if (array[0] != null)
				{
					jsonObject[UnescapeString(array[0])] = ParseJsonPart(array[1]);
				}
			}
			result = jsonObject;
			break;
		}
		case '[':
		{
			JsonArray jsonArray = new JsonArray();
			foreach (string item2 in SplitJsonParts(jsonPart.Substring(1, jsonPart.Length - 2)))
			{
				if (item2.Length > 0)
				{
					jsonArray.Add(ParseJsonPart(item2));
				}
			}
			result = jsonArray;
			break;
		}
		case '"':
			result = new JsonBasic(UnescapeString(jsonPart.Substring(1, jsonPart.Length - 2)));
			break;
		case 'f':
			result = new JsonBasic(false);
			break;
		case 't':
			result = new JsonBasic(true);
			break;
		case 'n':
			result = null;
			break;
		default:
		{
			long result2 = 0L;
			if (long.TryParse(jsonPart, NumberStyles.Any, CultureInfo.InvariantCulture, out result2))
			{
				result = ((result2 <= int.MaxValue && result2 >= int.MinValue) ? new JsonBasic((int)result2) : new JsonBasic(result2));
				break;
			}
			decimal result3 = default(decimal);
			if (decimal.TryParse(jsonPart, NumberStyles.Any, CultureInfo.InvariantCulture, out result3))
			{
				result = new JsonBasic(result3);
			}
			break;
		}
		}
		return result;
	}

	private static List<string> SplitJsonParts(string json)
	{
		List<string> list = new List<string>();
		int num = 0;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < json.Length; i++)
		{
			switch (json[i])
			{
			case ',':
				if (!flag && num == 0)
				{
					list.Add(json.Substring(num2, i - num2));
					num2 = i + 1;
				}
				break;
			case '"':
				if (i == 0 || json[i - 1] != '\\')
				{
					flag = !flag;
				}
				break;
			case '[':
			case '{':
				if (!flag)
				{
					num++;
				}
				break;
			case ']':
			case '}':
				if (!flag)
				{
					num--;
				}
				break;
			}
		}
		list.Add(json.Substring(num2));
		return list;
	}

	private static string[] SplitKeyValuePart(string json)
	{
		string[] array = new string[2];
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		while (num < json.Length && !flag2)
		{
			if (json[num] == '"' && (num == 0 || json[num - 1] != '\\'))
			{
				if (!flag)
				{
					flag = true;
					num++;
				}
				else
				{
					array[0] = json.Substring(1, num - 1);
					array[1] = json.Substring(num + 2);
					flag2 = true;
				}
			}
			else
			{
				num++;
			}
		}
		return array;
	}

	private static string RemoveNonTokenChars(string s)
	{
		int length = s.Length;
		char[] array = new char[length];
		int length2 = 0;
		bool flag = true;
		for (int i = 0; i < length; i++)
		{
			char c = s[i];
			if (c == '"' && (i == 0 || s[i - 1] != '\\'))
			{
				flag = !flag;
			}
			if (!flag || (c != ' ' && c != '\r' && c != '\n' && c != '\t' && c != '\b' && c != '\f'))
			{
				array[length2++] = c;
			}
		}
		return new string(array, 0, length2);
	}

	public abstract string ToJsonString();

	public string ToJsonPrettyPrintString()
	{
		string text = ToJsonString();
		string text2 = string.Empty;
		for (int i = 0; i < 1; i++)
		{
			text2 += "\t";
		}
		bool flag = false;
		string text3 = string.Empty;
		for (int j = 0; j < text.Length; j++)
		{
			switch (text[j])
			{
			case ':':
				if (!flag)
				{
					text = text.Insert(j + 1, " ");
				}
				break;
			case '"':
				if (j == 0 || text[j - 1] != '\\')
				{
					flag = !flag;
				}
				break;
			case ',':
				if (!flag)
				{
					text = text.Insert(j + 1, "\n" + text3);
				}
				break;
			case '[':
			case '{':
				if (!flag)
				{
					text3 += text2;
					text = text.Insert(j + 1, "\n" + text3);
				}
				break;
			case ']':
			case '}':
				if (!flag)
				{
					text3 = text3.Substring(0, text3.Length - text2.Length);
					text = text.Insert(j, "\n" + text3);
					j += text3.Length + 1;
				}
				break;
			}
		}
		return text;
	}
}
