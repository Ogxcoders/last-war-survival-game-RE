using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
[Conditional("UNITY_EDITOR")]
public sealed class SuffixLabelAttribute : Attribute
{
	public string Label;

	public bool Overlay;

	public SuffixLabelAttribute(string label, bool overlay = false)
	{
		Label = label;
		Overlay = overlay;
	}
}
