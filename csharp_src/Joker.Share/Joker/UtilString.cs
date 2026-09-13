using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Joker;

public static class UtilString
{
	private static Func<string, object[], string> _formater;

	public static void Init(Func<string, object[], string> formater)
	{
		_formater = formater;
	}

	public static string Format(string formater, params object[] args)
	{
		if (_formater == null)
		{
			return string.Format(formater, args);
		}
		return _formater(formater, args);
	}

	public static IEnumerable<byte> ToBytes(this string str)
	{
		return Encoding.Default.GetBytes(str);
	}

	public static byte[] ToByteArray(this string str)
	{
		return Encoding.Default.GetBytes(str);
	}

	public static byte[] ToUtf8(this string str)
	{
		return Encoding.UTF8.GetBytes(str);
	}

	public static byte[] HexToBytes(this string hexString)
	{
		if (hexString.Length % 2 != 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
		}
		byte[] array = new byte[hexString.Length / 2];
		for (int i = 0; i < array.Length; i++)
		{
			string text = "";
			text += hexString[i * 2];
			text += hexString[i * 2 + 1];
			array[i] = byte.Parse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
		return array;
	}

	public static string Fmt(this string text, params object[] args)
	{
		return string.Format(text, args);
	}

	public static string ListToString<T>(this List<T> list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (T item in list)
		{
			stringBuilder.Append(item);
			stringBuilder.Append(",");
		}
		return stringBuilder.ToString();
	}

	public static string ArrayToString<T>(this T[] args)
	{
		if (args == null)
		{
			return "";
		}
		string text = " [";
		for (int i = 0; i < args.Length; i++)
		{
			text += args[i];
			if (i != args.Length - 1)
			{
				text += ", ";
			}
		}
		return text + "]";
	}

	public static string ArrayToString<T>(this T[] args, int index, int count)
	{
		if (args == null)
		{
			return "";
		}
		string text = " [";
		for (int i = index; i < count + index; i++)
		{
			text += args[i];
			if (i != args.Length - 1)
			{
				text += ", ";
			}
		}
		return text + "]";
	}
}
