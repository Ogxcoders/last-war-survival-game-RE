namespace UnityEngine.UIElements.StyleSheets;

internal static class StyleSheetApplicator
{
	public static void ApplyAlign(IStylePropertyReader reader, ref StyleInt property)
	{
		if (reader.IsKeyword(0, StyleValueKeyword.Auto))
		{
			StyleInt styleInt = new StyleInt(0);
			styleInt.specificity = reader.specificity;
			StyleInt styleInt2 = styleInt;
			property = styleInt2;
		}
		else if (!reader.IsValueType(0, StyleValueType.Enum))
		{
			Debug.LogError("Invalid value for align property " + reader.ReadAsString(0));
		}
		else
		{
			property = reader.ReadStyleEnum<Align>(0);
		}
	}

	public static void ApplyDisplay(IStylePropertyReader reader, ref StyleInt property)
	{
		if (reader.IsKeyword(0, StyleValueKeyword.None))
		{
			StyleInt styleInt = new StyleInt(1);
			styleInt.specificity = reader.specificity;
			StyleInt styleInt2 = styleInt;
			property = styleInt2;
		}
		else if (!reader.IsValueType(0, StyleValueType.Enum))
		{
			Debug.LogError("Invalid value for display property " + reader.ReadAsString(0));
		}
		else
		{
			property = reader.ReadStyleEnum<DisplayStyle>(0);
		}
	}
}
