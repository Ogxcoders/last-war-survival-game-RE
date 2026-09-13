using System.Text;

namespace System.Net.Http.Headers;

internal class UriHeaderParser : HttpHeaderParser
{
	private UriKind _uriKind;

	internal static readonly UriHeaderParser RelativeOrAbsoluteUriParser = new UriHeaderParser((UriKind)300);

	private UriHeaderParser(UriKind uriKind)
		: base(supportsMultipleValues: false)
	{
		_uriKind = uriKind;
	}

	public override bool TryParseValue(string value, object storeValue, ref int index, out object parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(value) || index == value.Length)
		{
			return false;
		}
		string text = value;
		if (index > 0)
		{
			text = value.Substring(index);
		}
		if (!Uri.TryCreate(text, _uriKind, out var result))
		{
			text = DecodeUtf8FromString(text);
			if (!Uri.TryCreate(text, _uriKind, out result))
			{
				return false;
			}
		}
		index = value.Length;
		parsedValue = result;
		return true;
	}

	internal static string DecodeUtf8FromString(string input)
	{
		if (string.IsNullOrWhiteSpace(input))
		{
			return input;
		}
		bool flag = false;
		for (int i = 0; i < input.Length; i++)
		{
			if (input[i] > 'ÿ')
			{
				return input;
			}
			if (input[i] > '\u007f')
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			byte[] array = new byte[input.Length];
			for (int j = 0; j < input.Length; j++)
			{
				if (input[j] > 'ÿ')
				{
					return input;
				}
				array[j] = (byte)input[j];
			}
			try
			{
				return Encoding.GetEncoding("utf-8", EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback).GetString(array, 0, array.Length);
			}
			catch (ArgumentException)
			{
			}
		}
		return input;
	}

	public override string ToString(object value)
	{
		Uri uri = (Uri)value;
		if (uri.IsAbsoluteUri)
		{
			return uri.AbsoluteUri;
		}
		return uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
	}
}
