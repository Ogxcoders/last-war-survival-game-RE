using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace System.Xml;

internal static class MailBnfHelper
{
	private static bool[] s_fqtext;

	private static bool[] s_ttext;

	private static bool[] s_digits;

	private static bool[] s_boundary;

	static MailBnfHelper()
	{
		s_fqtext = new bool[128];
		s_ttext = new bool[128];
		s_digits = new bool[128];
		s_boundary = new bool[128];
		for (int i = 1; i <= 9; i++)
		{
			s_fqtext[i] = true;
		}
		s_fqtext[11] = true;
		s_fqtext[12] = true;
		for (int j = 14; j <= 33; j++)
		{
			s_fqtext[j] = true;
		}
		for (int k = 35; k <= 91; k++)
		{
			s_fqtext[k] = true;
		}
		for (int l = 93; l <= 127; l++)
		{
			s_fqtext[l] = true;
		}
		for (int m = 33; m <= 126; m++)
		{
			s_ttext[m] = true;
		}
		s_ttext[40] = false;
		s_ttext[41] = false;
		s_ttext[60] = false;
		s_ttext[62] = false;
		s_ttext[64] = false;
		s_ttext[44] = false;
		s_ttext[59] = false;
		s_ttext[58] = false;
		s_ttext[92] = false;
		s_ttext[34] = false;
		s_ttext[47] = false;
		s_ttext[91] = false;
		s_ttext[93] = false;
		s_ttext[63] = false;
		s_ttext[61] = false;
		for (int n = 48; n <= 57; n++)
		{
			s_digits[n] = true;
		}
		for (int num = 48; num <= 57; num++)
		{
			s_boundary[num] = true;
		}
		for (int num2 = 65; num2 <= 90; num2++)
		{
			s_boundary[num2] = true;
		}
		for (int num3 = 97; num3 <= 122; num3++)
		{
			s_boundary[num3] = true;
		}
		s_boundary[39] = true;
		s_boundary[40] = true;
		s_boundary[41] = true;
		s_boundary[43] = true;
		s_boundary[95] = true;
		s_boundary[44] = true;
		s_boundary[45] = true;
		s_boundary[46] = true;
		s_boundary[47] = true;
		s_boundary[58] = true;
		s_boundary[61] = true;
		s_boundary[63] = true;
		s_boundary[32] = true;
	}

	public static bool SkipCFWS(string data, ref int offset)
	{
		int num = 0;
		while (offset < data.Length)
		{
			if (data[offset] > '\u007f')
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME header has an invalid character ('{0}', {1} in hexadecimal value).", data[offset], ((int)data[offset]).ToString("X", CultureInfo.InvariantCulture))));
			}
			if (data[offset] == '\\' && num > 0)
			{
				offset += 2;
			}
			else if (data[offset] == '(')
			{
				num++;
			}
			else if (data[offset] == ')')
			{
				num--;
			}
			else if (data[offset] != ' ' && data[offset] != '\t' && num == 0)
			{
				return true;
			}
			offset++;
		}
		return false;
	}

	public static string ReadQuotedString(string data, ref int offset, StringBuilder builder)
	{
		int num = ++offset;
		StringBuilder stringBuilder = ((builder != null) ? builder : new StringBuilder());
		while (offset < data.Length)
		{
			if (data[offset] == '\\')
			{
				stringBuilder.Append(data, num, offset - num);
				num = ++offset;
			}
			else
			{
				if (data[offset] == '"')
				{
					stringBuilder.Append(data, num, offset - num);
					offset++;
					if (builder == null)
					{
						return stringBuilder.ToString();
					}
					return null;
				}
				if (data[offset] >= s_fqtext.Length || !s_fqtext[(uint)data[offset]])
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME header has an invalid character ('{0}', {1} in hexadecimal value).", data[offset], ((int)data[offset]).ToString("X", CultureInfo.InvariantCulture))));
				}
			}
			offset++;
		}
		throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("Malformed MIME header.")));
	}

	public static string ReadParameterAttribute(string data, ref int offset, StringBuilder builder)
	{
		if (!SkipCFWS(data, ref offset))
		{
			return null;
		}
		return ReadToken(data, ref offset, null);
	}

	public static string ReadParameterValue(string data, ref int offset, StringBuilder builder)
	{
		if (!SkipCFWS(data, ref offset))
		{
			return string.Empty;
		}
		if (offset < data.Length && data[offset] == '"')
		{
			return ReadQuotedString(data, ref offset, builder);
		}
		return ReadToken(data, ref offset, builder);
	}

	public static string ReadToken(string data, ref int offset, StringBuilder builder)
	{
		int num = offset;
		while (offset < data.Length)
		{
			if (data[offset] > s_ttext.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(SR.GetString("MIME header has an invalid character ('{0}', {1} in hexadecimal value).", data[offset], ((int)data[offset]).ToString("X", CultureInfo.InvariantCulture))));
			}
			if (!s_ttext[(uint)data[offset]])
			{
				break;
			}
			offset++;
		}
		return data.Substring(num, offset - num);
	}

	public static string ReadDigits(string data, ref int offset, StringBuilder builder)
	{
		int num = offset;
		StringBuilder stringBuilder = ((builder != null) ? builder : new StringBuilder());
		while (offset < data.Length && data[offset] < s_digits.Length && s_digits[(uint)data[offset]])
		{
			offset++;
		}
		stringBuilder.Append(data, num, offset - num);
		if (builder == null)
		{
			return stringBuilder.ToString();
		}
		return null;
	}

	public static bool IsValidMimeBoundary(string data)
	{
		int num = data?.Length ?? 0;
		if (num == 0 || num > 70 || data[num - 1] == ' ')
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			if (data[i] >= s_boundary.Length || !s_boundary[(uint)data[i]])
			{
				return false;
			}
		}
		return true;
	}
}
