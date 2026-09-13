namespace UnityEngine;

public sealed class GUILayoutOption
{
	internal enum Type
	{
		fixedWidth,
		fixedHeight,
		minWidth,
		maxWidth,
		minHeight,
		maxHeight,
		stretchWidth,
		stretchHeight,
		alignStart,
		alignMiddle,
		alignEnd,
		alignJustify,
		equalSize,
		spacing
	}

	internal Type type;

	internal object value;

	internal GUILayoutOption(Type type, object value)
	{
		this.type = type;
		this.value = value;
	}
}
