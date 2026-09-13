namespace System.Net.Http.Headers;

internal class ProductInfoHeaderParser : HttpHeaderParser
{
	private const string separator = " ";

	internal static readonly ProductInfoHeaderParser SingleValueParser = new ProductInfoHeaderParser(supportsMultipleValues: false);

	internal static readonly ProductInfoHeaderParser MultipleValueParser = new ProductInfoHeaderParser(supportsMultipleValues: true);

	private ProductInfoHeaderParser(bool supportsMultipleValues)
		: base(supportsMultipleValues, " ")
	{
	}

	public override bool TryParseValue(string value, object storeValue, ref int index, out object parsedValue)
	{
		parsedValue = null;
		if (string.IsNullOrEmpty(value) || index == value.Length)
		{
			return false;
		}
		int num = index + HttpRuleParser.GetWhitespaceLength(value, index);
		if (num == value.Length)
		{
			return false;
		}
		ProductInfoHeaderValue parsedValue2 = null;
		int productInfoLength = ProductInfoHeaderValue.GetProductInfoLength(value, num, out parsedValue2);
		if (productInfoLength == 0)
		{
			return false;
		}
		num += productInfoLength;
		if (num < value.Length)
		{
			char c = value[num - 1];
			if (c != ' ' && c != '\t')
			{
				return false;
			}
		}
		index = num;
		parsedValue = parsedValue2;
		return true;
	}
}
