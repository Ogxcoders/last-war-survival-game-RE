using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class HorizontalGroupAttribute : PropertyGroupAttribute
{
	public float Width;

	public float MarginLeft;

	public float MarginRight;

	public float PaddingLeft;

	public float PaddingRight;

	public float MinWidth;

	public float MaxWidth;

	public string Title;

	public float LabelWidth;

	public HorizontalGroupAttribute(string group, float width = 0f, int marginLeft = 0, int marginRight = 0, float order = 0f)
		: base(group, order)
	{
		Width = width;
		MarginLeft = marginLeft;
		MarginRight = marginRight;
	}

	public HorizontalGroupAttribute(float width = 0f, int marginLeft = 0, int marginRight = 0, float order = 0f)
		: this("_DefaultHorizontalGroup", width, marginLeft, marginRight, order)
	{
	}

	protected override void CombineValuesWith(PropertyGroupAttribute other)
	{
		Title = Title ?? (other as HorizontalGroupAttribute).Title;
		LabelWidth = Math.Max(LabelWidth, (other as HorizontalGroupAttribute).LabelWidth);
	}
}
