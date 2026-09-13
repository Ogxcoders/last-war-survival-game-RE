using System.Globalization;

namespace UnityEngine.UIElements;

public class UxmlFloatAttributeDescription : TypedUxmlAttributeDescription<float>
{
	public override string defaultValueAsString => base.defaultValue.ToString(CultureInfo.InvariantCulture.NumberFormat);

	public UxmlFloatAttributeDescription()
	{
		base.type = "float";
		base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
		base.defaultValue = 0f;
	}

	public override float GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
	{
		return GetValueFromBag(bag, cc, (string s, float f) => ConvertValueToFloat(s, f), base.defaultValue);
	}

	public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref float value)
	{
		return TryGetValueFromBag(bag, cc, (string s, float f) => ConvertValueToFloat(s, f), base.defaultValue, ref value);
	}

	private static float ConvertValueToFloat(string v, float defaultValue)
	{
		if (v == null || !float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			return defaultValue;
		}
		return result;
	}
}
