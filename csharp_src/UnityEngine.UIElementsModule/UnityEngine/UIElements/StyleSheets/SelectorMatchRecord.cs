namespace UnityEngine.UIElements.StyleSheets;

internal struct SelectorMatchRecord
{
	public StyleSheet sheet;

	public int styleSheetIndexInStack;

	public StyleComplexSelector complexSelector;

	public SelectorMatchRecord(StyleSheet sheet, int styleSheetIndexInStack)
	{
		this = default(SelectorMatchRecord);
		this.sheet = sheet;
		this.styleSheetIndexInStack = styleSheetIndexInStack;
	}

	public static int Compare(SelectorMatchRecord a, SelectorMatchRecord b)
	{
		if (a.sheet.isUnityStyleSheet != b.sheet.isUnityStyleSheet)
		{
			return (!a.sheet.isUnityStyleSheet) ? 1 : (-1);
		}
		int num = a.complexSelector.specificity.CompareTo(b.complexSelector.specificity);
		if (num == 0)
		{
			num = a.styleSheetIndexInStack.CompareTo(b.styleSheetIndexInStack);
		}
		if (num == 0)
		{
			num = a.complexSelector.orderInStyleSheet.CompareTo(b.complexSelector.orderInStyleSheet);
		}
		return num;
	}
}
