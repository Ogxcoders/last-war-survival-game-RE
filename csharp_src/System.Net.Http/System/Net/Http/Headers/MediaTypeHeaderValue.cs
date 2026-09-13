using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace System.Net.Http.Headers;

public class MediaTypeHeaderValue : ICloneable
{
	private const string charSet = "charset";

	private ObjectCollection<NameValueHeaderValue> _parameters;

	private string _mediaType;

	public string CharSet
	{
		get
		{
			return NameValueHeaderValue.Find(_parameters, "charset")?.Value;
		}
		set
		{
			NameValueHeaderValue nameValueHeaderValue = NameValueHeaderValue.Find(_parameters, "charset");
			if (string.IsNullOrEmpty(value))
			{
				if (nameValueHeaderValue != null)
				{
					_parameters.Remove(nameValueHeaderValue);
				}
			}
			else if (nameValueHeaderValue != null)
			{
				nameValueHeaderValue.Value = value;
			}
			else
			{
				Parameters.Add(new NameValueHeaderValue("charset", value));
			}
		}
	}

	public ICollection<NameValueHeaderValue> Parameters
	{
		get
		{
			if (_parameters == null)
			{
				_parameters = new ObjectCollection<NameValueHeaderValue>();
			}
			return _parameters;
		}
	}

	public string MediaType
	{
		get
		{
			return _mediaType;
		}
		set
		{
			CheckMediaTypeFormat(value, "value");
			_mediaType = value;
		}
	}

	internal MediaTypeHeaderValue()
	{
	}

	protected MediaTypeHeaderValue(MediaTypeHeaderValue source)
	{
		_mediaType = source._mediaType;
		if (source._parameters == null)
		{
			return;
		}
		foreach (NameValueHeaderValue parameter in source._parameters)
		{
			Parameters.Add((NameValueHeaderValue)((ICloneable)parameter).Clone());
		}
	}

	public MediaTypeHeaderValue(string mediaType)
	{
		CheckMediaTypeFormat(mediaType, "mediaType");
		_mediaType = mediaType;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(_mediaType);
		NameValueHeaderValue.ToString(_parameters, ';', leadingSeparator: true, stringBuilder);
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is MediaTypeHeaderValue mediaTypeHeaderValue))
		{
			return false;
		}
		if (string.Equals(_mediaType, mediaTypeHeaderValue._mediaType, StringComparison.OrdinalIgnoreCase))
		{
			return HeaderUtilities.AreEqualCollections(_parameters, mediaTypeHeaderValue._parameters);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return StringComparer.OrdinalIgnoreCase.GetHashCode(_mediaType) ^ NameValueHeaderValue.GetHashCode(_parameters);
	}

	public static MediaTypeHeaderValue Parse(string input)
	{
		int index = 0;
		return (MediaTypeHeaderValue)MediaTypeHeaderParser.SingleValueParser.ParseValue(input, null, ref index);
	}

	public static bool TryParse(string input, out MediaTypeHeaderValue parsedValue)
	{
		int index = 0;
		parsedValue = null;
		if (MediaTypeHeaderParser.SingleValueParser.TryParseValue(input, null, ref index, out var parsedValue2))
		{
			parsedValue = (MediaTypeHeaderValue)parsedValue2;
			return true;
		}
		return false;
	}

	internal static int GetMediaTypeLength(string input, int startIndex, Func<MediaTypeHeaderValue> mediaTypeCreator, out MediaTypeHeaderValue parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(input) || startIndex >= input.Length)
		{
			return 0;
		}
		string mediaType = null;
		int mediaTypeExpressionLength = GetMediaTypeExpressionLength(input, startIndex, out mediaType);
		if (mediaTypeExpressionLength == 0)
		{
			return 0;
		}
		int num = startIndex + mediaTypeExpressionLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		MediaTypeHeaderValue mediaTypeHeaderValue = null;
		if (num < input.Length && input[num] == ';')
		{
			mediaTypeHeaderValue = mediaTypeCreator();
			mediaTypeHeaderValue._mediaType = mediaType;
			num++;
			int nameValueListLength = NameValueHeaderValue.GetNameValueListLength(input, num, ';', (ObjectCollection<NameValueHeaderValue>)mediaTypeHeaderValue.Parameters);
			if (nameValueListLength == 0)
			{
				return 0;
			}
			parsedValue = mediaTypeHeaderValue;
			return num + nameValueListLength - startIndex;
		}
		mediaTypeHeaderValue = mediaTypeCreator();
		mediaTypeHeaderValue._mediaType = mediaType;
		parsedValue = mediaTypeHeaderValue;
		return num - startIndex;
	}

	private static int GetMediaTypeExpressionLength(string input, int startIndex, out string mediaType)
	{
		mediaType = null;
		int tokenLength = HttpRuleParser.GetTokenLength(input, startIndex);
		if (tokenLength == 0)
		{
			return 0;
		}
		int num = startIndex + tokenLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (num >= input.Length || input[num] != '/')
		{
			return 0;
		}
		num++;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		int tokenLength2 = HttpRuleParser.GetTokenLength(input, num);
		if (tokenLength2 == 0)
		{
			return 0;
		}
		int num2 = num + tokenLength2 - startIndex;
		if (tokenLength + tokenLength2 + 1 == num2)
		{
			mediaType = input.Substring(startIndex, num2);
		}
		else
		{
			mediaType = input.Substring(startIndex, tokenLength) + "/" + input.Substring(num, tokenLength2);
		}
		return num2;
	}

	private static void CheckMediaTypeFormat(string mediaType, string parameterName)
	{
		if (string.IsNullOrEmpty(mediaType))
		{
			throw new ArgumentException("The value cannot be null or empty.", parameterName);
		}
		if (GetMediaTypeExpressionLength(mediaType, 0, out var mediaType2) == 0 || mediaType2.Length != mediaType.Length)
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", mediaType));
		}
	}

	object ICloneable.Clone()
	{
		return new MediaTypeHeaderValue(this);
	}
}
