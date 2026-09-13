using System.Globalization;

namespace UnityEngine.UIElements.StyleSheets;

internal static class StyleSheetExtensions
{
	public static string ReadAsString(this StyleSheet sheet, StyleValueHandle handle)
	{
		string empty = string.Empty;
		return handle.valueType switch
		{
			StyleValueType.Float => sheet.ReadFloat(handle).ToString(CultureInfo.InvariantCulture.NumberFormat), 
			StyleValueType.Dimension => sheet.ReadDimension(handle).ToString(), 
			StyleValueType.Color => sheet.ReadColor(handle).ToString(), 
			StyleValueType.ResourcePath => sheet.ReadResourcePath(handle), 
			StyleValueType.String => sheet.ReadString(handle), 
			StyleValueType.Enum => sheet.ReadEnum(handle), 
			StyleValueType.Variable => sheet.ReadVariable(handle), 
			StyleValueType.Keyword => sheet.ReadKeyword(handle).ToUssString(), 
			StyleValueType.AssetReference => sheet.ReadAssetReference(handle).ToString(), 
			StyleValueType.Function => sheet.ReadFunctionName(handle), 
			StyleValueType.FunctionSeparator => ",", 
			StyleValueType.ScalableImage => sheet.ReadScalableImage(handle).ToString(), 
			_ => string.Concat("Error reading value type (", handle.valueType, ") at index ", handle.valueIndex), 
		};
	}

	public static bool IsVarFunction(this StyleValueHandle handle)
	{
		return handle.valueType == StyleValueType.Function && handle.valueIndex == 1;
	}
}
