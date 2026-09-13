using System.Globalization;

namespace UnityEngine.UIElements;

public class UxmlDoubleAttributeDescription : TypedUxmlAttributeDescription<double>
{
	public override string defaultValueAsString => base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);

	public UxmlDoubleAttributeDescription()
	{
		base.type = "double";
		base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
		base.defaultValue = 0.0;
	}

	public override double GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
	{
		return GetValueFromBag(bag, cc, (string s, double d) => ConvertValueToDouble(s, d), base.defaultValue);
	}

	public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref double value)
	{
		return TryGetValueFromBag(bag, cc, (string s, double d) => ConvertValueToDouble(s, d), base.defaultValue, ref value);
	}

	private static double ConvertValueToDouble(string v, double defaultValue)
	{
		if (v == null || !double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}
}
