using System.Collections.Generic;
using System.Text;

namespace System.Net.Http.Headers;

public class TransferCodingHeaderValue : ICloneable
{
	private ObjectCollection<NameValueHeaderValue> _parameters;

	private string _value;

	public string Value => _value;

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

	internal TransferCodingHeaderValue()
	{
	}

	protected TransferCodingHeaderValue(TransferCodingHeaderValue source)
	{
		_value = source._value;
		if (source._parameters == null)
		{
			return;
		}
		foreach (NameValueHeaderValue parameter in source._parameters)
		{
			Parameters.Add((NameValueHeaderValue)((ICloneable)parameter).Clone());
		}
	}

	public TransferCodingHeaderValue(string value)
	{
		HeaderUtilities.CheckValidToken(value, "value");
		_value = value;
	}

	public static TransferCodingHeaderValue Parse(string input)
	{
		int index = 0;
		return (TransferCodingHeaderValue)TransferCodingHeaderParser.SingleValueParser.ParseValue(input, null, ref index);
	}

	public static bool TryParse(string input, out TransferCodingHeaderValue parsedValue)
	{
		int index = 0;
		parsedValue = null;
		if (TransferCodingHeaderParser.SingleValueParser.TryParseValue(input, null, ref index, out var parsedValue2))
		{
			parsedValue = (TransferCodingHeaderValue)parsedValue2;
			return true;
		}
		return false;
	}

	internal static int GetTransferCodingLength(string input, int startIndex, Func<TransferCodingHeaderValue> transferCodingCreator, out TransferCodingHeaderValue parsedValue)
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
		string value = input.Substring(startIndex, tokenLength);
		int num = startIndex + tokenLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		TransferCodingHeaderValue transferCodingHeaderValue = null;
		if (num < input.Length && input[num] == ';')
		{
			transferCodingHeaderValue = transferCodingCreator();
			transferCodingHeaderValue._value = value;
			num++;
			int nameValueListLength = NameValueHeaderValue.GetNameValueListLength(input, num, ';', (ObjectCollection<NameValueHeaderValue>)transferCodingHeaderValue.Parameters);
			if (nameValueListLength == 0)
			{
				return 0;
			}
			parsedValue = transferCodingHeaderValue;
			return num + nameValueListLength - startIndex;
		}
		transferCodingHeaderValue = transferCodingCreator();
		transferCodingHeaderValue._value = value;
		parsedValue = transferCodingHeaderValue;
		return num - startIndex;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(_value);
		NameValueHeaderValue.ToString(_parameters, ';', leadingSeparator: true, stringBuilder);
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TransferCodingHeaderValue transferCodingHeaderValue))
		{
			return false;
		}
		if (string.Equals(_value, transferCodingHeaderValue._value, StringComparison.OrdinalIgnoreCase))
		{
			return HeaderUtilities.AreEqualCollections(_parameters, transferCodingHeaderValue._parameters);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return StringComparer.OrdinalIgnoreCase.GetHashCode(_value) ^ NameValueHeaderValue.GetHashCode(_parameters);
	}

	object ICloneable.Clone()
	{
		return new TransferCodingHeaderValue(this);
	}
}
