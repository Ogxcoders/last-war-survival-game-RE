using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BaseUtils;

public class StringUtils
{
	private static string[] s_num_string = new string[31]
	{
		"0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30"
	};

	private static string[] s_roman_level = new string[30]
	{
		"I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X",
		"XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX",
		"XXI", "XXII", "XXIII", "XXIV", "XXV", "XXVI", "XXVII", "XXVIII", "XXIX", "XXX"
	};

	private static Dictionary<int, string> intValueToString = new Dictionary<int, string>(1000);

	private static int rep = 0;

	private static readonly double[] byteUnits = new double[4] { 1073741824.0, 1048576.0, 1024.0, 1.0 };

	private static readonly string[] byteUnitsNames = new string[4] { "GB", "MB", "KB", "B" };

	public static bool IsNullOrEmpty(string str)
	{
		return string.IsNullOrEmpty(str);
	}

	public static string IntToString(int variable)
	{
		if (variable >= 0 && variable < s_num_string.Length)
		{
			return s_num_string[variable];
		}
		if (variable == -1)
		{
			return "-1";
		}
		if (!intValueToString.TryGetValue(variable, out var value))
		{
			value = variable.ToString();
			if (intValueToString.Count < 2000)
			{
				intValueToString.Add(variable, value);
			}
		}
		return value;
	}

	public static string GetMD5(string msg)
	{
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		byte[] bytes = Encoding.UTF8.GetBytes(msg);
		byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes, 0, bytes.Length);
		mD5CryptoServiceProvider.Clear();
		string text = "";
		for (int i = 0; i < array.Length; i++)
		{
			text += Convert.ToString(array[i], 16).PadLeft(2, '0');
		}
		return text.PadLeft(32, '0');
	}

	public static ulong FnvHash64(string str)
	{
		ulong num = 14695981039346656037uL;
		for (int i = 0; i < str.Length; i++)
		{
			num ^= str[i];
			num *= 1099511628211L;
		}
		return num;
	}

	public static string GetFormattedSeperatorNum(long value)
	{
		string text = "";
		if (value < 0)
		{
			value = -value;
			text = "-";
		}
		return text + value.ToString("N0");
	}

	public static string GetFormattedSeperatorNum(string value)
	{
		if (value.Contains(".") || value.Contains("%"))
		{
			return value;
		}
		return value.ToLong().ToString("N0");
	}

	public static string GetFormattedInt(int value)
	{
		string text = "";
		if (value < 0)
		{
			value = -value;
			text = "-";
		}
		float num = (float)value / 1000f;
		float num2 = (float)value / 1000000f;
		if (num2 >= 1f)
		{
			return text + num2.ToString("F1") + "M";
		}
		if (num >= 1f)
		{
			return text + num.ToString("F1") + "K";
		}
		return text + value.ToString("");
	}

	public static string GetFormattedLong(long value)
	{
		string text = "";
		if (value < 0)
		{
			value = -value;
			text = "-";
		}
		float num = (float)value / 1000f;
		float num2 = (float)value / 1000000f;
		if (num2 >= 1f)
		{
			return text + num2.ToString("F1") + "M";
		}
		if (num >= 1f)
		{
			return text + num.ToString("F1") + "K";
		}
		return text + value.ToString("");
	}

	public static string GetFormattedStr(double value)
	{
		string text = "";
		if (value < 0.0)
		{
			value = 0.0 - value;
			text = "-";
		}
		double num = value / 1000.0;
		double num2 = value / 1000000.0;
		double num3 = value / 1000000000.0;
		if (num3 >= 1.0)
		{
			return text + num3.ToString("F1") + "G";
		}
		if (num2 >= 1.0)
		{
			return text + num2.ToString("F1") + "M";
		}
		if (num >= 1.0)
		{
			return text + num.ToString("F1") + "K";
		}
		return text + value.ToString("");
	}

	public static string GetFormattedPercentStr(double value)
	{
		string text = "";
		if (value < 0.0)
		{
			value = 0.0 - value;
			text = "-";
		}
		return text + value.ToString("P");
	}

	public static string GenerateRandomStr(int _codeCount)
	{
		string text = string.Empty;
		long num = DateTime.Now.Ticks + rep;
		rep++;
		Random random = new Random((int)(num & 0xFFFFFFFFu) | (int)(num >> rep));
		for (int i = 0; i < _codeCount; i++)
		{
			int num2 = random.Next();
			text += (char)(48 + (ushort)(num2 % 10));
		}
		return text;
	}

	public static string FormatDBString(string str)
	{
		return "'" + str + "'";
	}

	public static string ConvertToRomanFromInt(int lv)
	{
		if (lv < 1 || lv > 30)
		{
			return "";
		}
		return s_roman_level[lv - 1];
	}

	public static void SplitString(string str, char key, ref List<string> list)
	{
		list = str.Split(new char[1] { key }).ToList();
	}

	public static string[] SplitString(string str, char key)
	{
		return str.Split(new char[1] { key });
	}

	public static string FormatStringMaxLength(string str, int maxLen = 18)
	{
		if (str.Length > maxLen)
		{
			return str.Substring(0, maxLen);
		}
		return str;
	}

	public static string S2Sec(string rawStr)
	{
		if (rawStr == null || rawStr == "")
		{
			return "";
		}
		if (rawStr.Length <= 3)
		{
			return rawStr;
		}
		string text = rawStr;
		int num = rawStr.Length;
		while (num > 3)
		{
			num -= 3;
			text = text.Insert(num, ",");
		}
		return text;
	}

	public static string S2Sec(int rawNum)
	{
		return S2Sec(rawNum.ToString());
	}

	public static int TryParseInt(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return 0;
		}
		int result = 0;
		int.TryParse(str, out result);
		return result;
	}

	public static int BaseVersionCompare(string a, string b)
	{
		if (string.IsNullOrEmpty(a))
		{
			if (string.IsNullOrEmpty(b))
			{
				return 0;
			}
			return -1;
		}
		if (string.IsNullOrEmpty(b))
		{
			return 1;
		}
		string[] array = a.Split(new char[1] { '.' });
		string[] array2 = b.Split(new char[1] { '.' });
		int num = array.Length;
		int num2 = array2.Length;
		int num3 = Math.Max(num, num2);
		for (int i = 0; i < num3; i++)
		{
			string text = ((i < num) ? array[i] : null);
			string text2 = ((i < num2) ? array2[i] : null);
			int num4 = 0;
			int num5 = 0;
			if (!string.IsNullOrEmpty(text))
			{
				num4 = int.Parse(text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				num5 = int.Parse(text2);
			}
			if (num4 > num5)
			{
				return 1;
			}
			if (num4 < num5)
			{
				return -1;
			}
		}
		return 0;
	}

	public static string FormatBytes(ulong bytes)
	{
		string result = "0 B";
		if (bytes == 0L)
		{
			return result;
		}
		for (int i = 0; i < byteUnits.Length; i++)
		{
			double num = byteUnits[i];
			if ((double)bytes >= num)
			{
				result = $"{(double)bytes / num:##.##} {byteUnitsNames[i]}";
				break;
			}
		}
		return result;
	}
}
