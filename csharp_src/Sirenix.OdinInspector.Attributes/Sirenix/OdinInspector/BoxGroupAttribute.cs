using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class BoxGroupAttribute : PropertyGroupAttribute
{
	public bool ShowLabel;

	public bool CenterLabel;

	public string LabelText;

	public BoxGroupAttribute(string group, bool showLabel = true, bool centerLabel = false, float order = 0f)
		: base(group, order)
	{
		ShowLabel = showLabel;
		CenterLabel = centerLabel;
	}

	public BoxGroupAttribute()
		: this("_DefaultBoxGroup", showLabel: false)
	{
	}

	protected override void CombineValuesWith(PropertyGroupAttribute other)
	{
		BoxGroupAttribute boxGroupAttribute = other as BoxGroupAttribute;
		if (!ShowLabel || !boxGroupAttribute.ShowLabel)
		{
			ShowLabel = false;
			boxGroupAttribute.ShowLabel = false;
		}
		CenterLabel |= boxGroupAttribute.CenterLabel;
	}
}
