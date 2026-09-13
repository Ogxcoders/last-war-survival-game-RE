using System.Collections.Generic;
using System.Text;

namespace System.Net.Http.Headers;

public class NameValueWithParametersHeaderValue : NameValueHeaderValue, ICloneable
{
	private static readonly Func<NameValueHeaderValue> s_nameValueCreator = CreateNameValue;

	private ObjectCollection<NameValueHeaderValue> _parameters;

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

	public NameValueWithParametersHeaderValue(string name)
		: base(name)
	{
	}

	public NameValueWithParametersHeaderValue(string name, string value)
		: base(name, value)
	{
	}

	internal NameValueWithParametersHeaderValue()
	{
	}

	protected NameValueWithParametersHeaderValue(NameValueWithParametersHeaderValue source)
		: base(source)
	{
		if (source._parameters == null)
		{
			return;
		}
		foreach (NameValueHeaderValue parameter in source._parameters)
		{
			Parameters.Add((NameValueHeaderValue)((ICloneable)parameter).Clone());
		}
	}

	public override bool Equals(object obj)
	{
		if (base.Equals(obj))
		{
			if (!(obj is NameValueWithParametersHeaderValue nameValueWithParametersHeaderValue))
			{
				return false;
			}
			return HeaderUtilities.AreEqualCollections(_parameters, nameValueWithParametersHeaderValue._parameters);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode() ^ NameValueHeaderValue.GetHashCode(_parameters);
	}

	public override string ToString()
	{
		string value = base.ToString();
		StringBuilder stringBuilder = StringBuilderCache.Acquire();
		stringBuilder.Append(value);
		NameValueHeaderValue.ToString(_parameters, ';', leadingSeparator: true, stringBuilder);
		return StringBuilderCache.GetStringAndRelease(stringBuilder);
	}

	public new static NameValueWithParametersHeaderValue Parse(string input)
	{
		int index = 0;
		return (NameValueWithParametersHeaderValue)GenericHeaderParser.SingleValueNameValueWithParametersParser.ParseValue(input, null, ref index);
	}

	public static bool TryParse(string input, out NameValueWithParametersHeaderValue parsedValue)
	{
		int index = 0;
		parsedValue = null;
		if (GenericHeaderParser.SingleValueNameValueWithParametersParser.TryParseValue(input, null, ref index, out var parsedValue2))
		{
			parsedValue = (NameValueWithParametersHeaderValue)parsedValue2;
			return true;
		}
		return false;
	}

	internal static int GetNameValueWithParametersLength(string input, int startIndex, out object parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(input) || startIndex >= input.Length)
		{
			return 0;
		}
		NameValueHeaderValue parsedValue2 = null;
		int nameValueLength = NameValueHeaderValue.GetNameValueLength(input, startIndex, s_nameValueCreator, out parsedValue2);
		if (nameValueLength == 0)
		{
			return 0;
		}
		int num = startIndex + nameValueLength;
		num += HttpRuleParser.GetWhitespaceLength(input, num);
		NameValueWithParametersHeaderValue nameValueWithParametersHeaderValue = parsedValue2 as NameValueWithParametersHeaderValue;
		if (num < input.Length && input[num] == ';')
		{
			num++;
			int nameValueListLength = NameValueHeaderValue.GetNameValueListLength(input, num, ';', (ObjectCollection<NameValueHeaderValue>)nameValueWithParametersHeaderValue.Parameters);
			if (nameValueListLength == 0)
			{
				return 0;
			}
			parsedValue = nameValueWithParametersHeaderValue;
			return num + nameValueListLength - startIndex;
		}
		parsedValue = nameValueWithParametersHeaderValue;
		return num - startIndex;
	}

	private static NameValueHeaderValue CreateNameValue()
	{
		return new NameValueWithParametersHeaderValue();
	}

	object ICloneable.Clone()
	{
		return new NameValueWithParametersHeaderValue(this);
	}
}
