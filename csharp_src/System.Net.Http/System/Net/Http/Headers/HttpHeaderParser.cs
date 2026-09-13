using System.Collections;
using System.Globalization;

namespace System.Net.Http.Headers;

internal abstract class HttpHeaderParser
{
	internal const string DefaultSeparator = ", ";

	private bool _supportsMultipleValues;

	private string _separator;

	public bool SupportsMultipleValues => _supportsMultipleValues;

	public string Separator => _separator;

	public virtual IEqualityComparer Comparer => null;

	protected HttpHeaderParser(bool supportsMultipleValues)
	{
		_supportsMultipleValues = supportsMultipleValues;
		if (supportsMultipleValues)
		{
			_separator = ", ";
		}
	}

	protected HttpHeaderParser(bool supportsMultipleValues, string separator)
	{
		_supportsMultipleValues = supportsMultipleValues;
		_separator = separator;
	}

	public abstract bool TryParseValue(string value, object storeValue, ref int index, out object parsedValue);

	public object ParseValue(string value, object storeValue, ref int index)
	{
		object parsedValue = null;
		if (!TryParseValue(value, storeValue, ref index, out parsedValue))
		{
			throw new FormatException(string.Format(CultureInfo.InvariantCulture, "The format of value '{0}' is invalid.", (value == null) ? "<null>" : value.Substring(index)));
		}
		return parsedValue;
	}

	public virtual string ToString(object value)
	{
		return value.ToString();
	}
}
