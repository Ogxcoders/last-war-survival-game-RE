namespace System.Net.Http.Headers;

internal class ByteArrayHeaderParser : HttpHeaderParser
{
	internal static readonly ByteArrayHeaderParser Parser = new ByteArrayHeaderParser();

	private ByteArrayHeaderParser()
		: base(supportsMultipleValues: false)
	{
	}

	public override string ToString(object value)
	{
		return Convert.ToBase64String((byte[])value);
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
		try
		{
			parsedValue = Convert.FromBase64String(text);
			index = value.Length;
			return true;
		}
		catch (FormatException ex)
		{
			if (NetEventSource.IsEnabled)
			{
				NetEventSource.Error(this, global::SR.Format("Value '{0}' is not a valid Base64 string. Error: {1}", text, ex.Message));
			}
		}
		return false;
	}
}
