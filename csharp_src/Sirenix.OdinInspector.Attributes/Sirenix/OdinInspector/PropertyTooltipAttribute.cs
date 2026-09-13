using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class PropertyTooltipAttribute : Attribute
{
	public string Tooltip;

	public PropertyTooltipAttribute(string tooltip)
	{
		Tooltip = tooltip;
	}
}
