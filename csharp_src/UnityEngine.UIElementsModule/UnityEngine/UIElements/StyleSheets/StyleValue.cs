using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UnityEngine.UIElements.StyleSheets;

[StructLayout(LayoutKind.Explicit)]
[DebuggerDisplay("id = {id}, keyword = {keyword}, number = {number}, boolean = {boolean}, color = {color}, resource = {resource}")]
internal struct StyleValue
{
	[FieldOffset(0)]
	public StylePropertyID id;

	[FieldOffset(4)]
	public StyleKeyword keyword;

	[FieldOffset(8)]
	public float number;

	[FieldOffset(8)]
	public Length length;

	[FieldOffset(8)]
	public Color color;

	[FieldOffset(8)]
	public GCHandle resource;

	public static StyleValue Create(StylePropertyID id)
	{
		return new StyleValue
		{
			id = id
		};
	}

	public static StyleValue Create(StylePropertyID id, StyleKeyword keyword)
	{
		return new StyleValue
		{
			id = id,
			keyword = keyword
		};
	}

	public static StyleValue Create(StylePropertyID id, float number)
	{
		return new StyleValue
		{
			id = id,
			number = number
		};
	}

	public static StyleValue Create(StylePropertyID id, int number)
	{
		return new StyleValue
		{
			id = id,
			number = number
		};
	}

	public static StyleValue Create(StylePropertyID id, Color color)
	{
		return new StyleValue
		{
			id = id,
			color = color
		};
	}
}
