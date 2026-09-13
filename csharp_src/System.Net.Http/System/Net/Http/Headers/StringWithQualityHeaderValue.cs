using System.Globalization;

namespace System.Net.Http.Headers;

public class StringWithQualityHeaderValue : ICloneable
{
	private string _value;

	private double? _quality;

	public string Value => _value;

	public double? Quality => _quality;

	public StringWithQualityHeaderValue(string value)
	{
		HeaderUtilities.CheckValidToken(value, "value");
		_value = value;
	}

	public StringWithQualityHeaderValue(string value, double quality)
	{
		HeaderUtilities.CheckValidToken(value, "value");
		if (quality < 0.0 || quality > 1.0)
		{
			throw new ArgumentOutOfRangeException("quality");
		}
		_value = value;
		_quality = quality;
	}

	private StringWithQualityHeaderValue(StringWithQualityHeaderValue source)
	{
		_value = source._value;
		_quality = source._quality;
	}

	private StringWithQualityHeaderValue()
	{
	}

	public override string ToString()
	{
		if (_quality.HasValue)
		{
			return _value + "; q=" + _quality.Value.ToString("0.0##", NumberFormatInfo.InvariantInfo);
		}
		return _value;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StringWithQualityHeaderValue stringWithQualityHeaderValue))
		{
			return false;
		}
		if (!string.Equals(_value, stringWithQualityHeaderValue._value, StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		if (_quality.HasValue)
		{
			if (stringWithQualityHeaderValue._quality.HasValue)
			{
				return _quality.Value == stringWithQualityHeaderValue._quality.Value;
			}
			return false;
		}
		return !stringWithQualityHeaderValue._quality.HasValue;
	}

	public override int GetHashCode()
	{
		int num = StringComparer.OrdinalIgnoreCase.GetHashCode(_value);
		if (_quality.HasValue)
		{
			num ^= _quality.Value.GetHashCode();
		}
		return num;
	}

	public static StringWithQualityHeaderValue Parse(string input)
	{
		int index = 0;
		return (StringWithQualityHeaderValue)GenericHeaderParser.SingleValueStringWithQualityParser.ParseValue(input, null, ref index);
	}

	public static bool TryParse(string input, out StringWithQualityHeaderValue parsedValue)
	{
		int index = 0;
		parsedValue = null;
		if (GenericHeaderParser.SingleValueStringWithQualityParser.TryParseValue(input, null, ref index, out var parsedValue2))
		{
			parsedValue = (StringWithQualityHeaderValue)parsedValue2;
			return true;
		}
		return false;
	}

	internal static int GetStringWithQualityLength(string input, int startIndex, out object parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(input) || startIndex >= input.Length)
		{
			return 0;
		}
		int tokenLength = HttpRuleParser.GetTokenLength(input, startIndex);
		if (tokenLength == 0)
		{
			return 0;
		}
		StringWithQualityHeaderValue stringWithQualityHeaderValue = new StringWithQualityHeaderValue();
		stringWithQualityHeaderValue._value = input.Substring(startIndex, tokenLength);
		int num = startIndex + tokenLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (num == input.Length || input[num] != ';')
		{
			parsedValue = stringWithQualityHeaderValue;
			return num - startIndex;
		}
		num++;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (!TryReadQuality(input, stringWithQualityHeaderValue, ref num))
		{
			return 0;
		}
		parsedValue = stringWithQualityHeaderValue;
		return num - startIndex;
	}

	private static bool TryReadQuality(string input, StringWithQualityHeaderValue result, ref int index)
	{
		int num = index;
		if (num == input.Length || (input[num] != 'q' && input[num] != 'Q'))
		{
			return false;
		}
		num++;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (num == input.Length || input[num] != '=')
		{
			return false;
		}
		num++;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		if (num == input.Length)
		{
			return false;
		}
		int numberLength = HttpRuleParser.GetNumberLength(input, num, allowDecimal: true);
		if (numberLength == 0)
		{
			return false;
		}
		double result2 = 0.0;
		if (!double.TryParse(input.Substring(num, numberLength), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out result2))
		{
			return false;
		}
		if (result2 < 0.0 || result2 > 1.0)
		{
			return false;
		}
		result._quality = result2;
		num += numberLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		index = num;
		return true;
	}

	object ICloneable.Clone()
	{
		return new StringWithQualityHeaderValue(this);
	}
}
