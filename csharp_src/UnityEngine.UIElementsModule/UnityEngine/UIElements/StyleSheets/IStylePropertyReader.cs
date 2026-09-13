namespace UnityEngine.UIElements.StyleSheets;

internal interface IStylePropertyReader
{
	StylePropertyID propertyID { get; }

	int specificity { get; }

	int valueCount { get; }

	bool IsValueType(int index, StyleValueType type);

	bool IsKeyword(int index, StyleValueKeyword keyword);

	string ReadAsString(int index);

	StyleLength ReadStyleLength(int index);

	StyleFloat ReadStyleFloat(int index);

	StyleInt ReadStyleInt(int index);

	StyleColor ReadStyleColor(int index);

	StyleInt ReadStyleEnum<T>(int index);

	StyleFont ReadStyleFont(int index);

	StyleBackground ReadStyleBackground(int index);

	StyleCursor ReadStyleCursor(int index);
}
