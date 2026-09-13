namespace UnityEngine.UIElements;

public class UxmlColorAttributeDescription : TypedUxmlAttributeDescription<Color>
{
	public override string defaultValueAsString => base.defaultValue.ToString();

	public UxmlColorAttributeDescription()
	{
		base.type = "string";
		base.typeNamespace = "http://www.w3.org/2001/XMLSchema";
		base.defaultValue = new Color(0f, 0f, 0f, 1f);
	}

	public override Color GetValueFromBag(IUxmlAttributes bag, CreationContext cc)
	{
		return GetValueFromBag(bag, cc, (string s, Color color) => ConvertValueToColor(s, color), base.defaultValue);
	}

	public bool TryGetValueFromBag(IUxmlAttributes bag, CreationContext cc, ref Color value)
	{
		return TryGetValueFromBag(bag, cc, (string s, Color color) => ConvertValueToColor(s, color), base.defaultValue, ref value);
	}

	private static Color ConvertValueToColor(string v, Color defaultValue)
	{
		if (v == null || !ColorUtility.TryParseHtmlString(v, out var color))
		{
			return defaultValue;
		}
		return color;
	}
}
