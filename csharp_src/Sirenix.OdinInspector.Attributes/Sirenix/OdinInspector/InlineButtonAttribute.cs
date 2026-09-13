using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class InlineButtonAttribute : Attribute
{
	public string Action;

	public string Label;

	public string ShowIf;

	[Obsolete("Use the Action member instead.", false)]
	public string MemberMethod
	{
		get
		{
			return Action;
		}
		set
		{
			Action = value;
		}
	}

	public InlineButtonAttribute(string action, string label = null)
	{
		Action = action;
		Label = label;
	}
}
