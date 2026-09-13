using System.Globalization;

namespace UnityEngine.UIElements;

public class UxmlIntAttributeDescription : TypedUxmlAttributeDescription<int>
{
	public override string defaultValueAsString => base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);

	public UxmlIntAttributeDescription()
	{
		base.type = "int";
		base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
		base.defaultValue = 0;
	}

	public override int GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
	{
		return GetValueFromBag(bag, cc, (string s, int i) => ConvertValueToInt(s, i), base.defaultValue);
	}

	public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref int value)
	{
		return TryGetValueFromBag(bag, cc, (string s, int i) => ConvertValueToInt(s, i), base.defaultValue, ref value);
	}

	private static int ConvertValueToInt(string v, int defaultValue)
	{
		if (v == null || !int.TryParse(v, out var result))
		{
			return defaultValue;
		}
		return result;
	}
}
