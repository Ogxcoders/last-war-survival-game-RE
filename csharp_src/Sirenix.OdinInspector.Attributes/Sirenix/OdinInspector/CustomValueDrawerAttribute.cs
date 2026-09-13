using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class CustomValueDrawerAttribute : Attribute
{
	public string Action;

	[Obsolete("Use the Action member instead.", false)]
	public string MethodName
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

	public CustomValueDrawerAttribute(string action)
	{
		Action = action;
	}
}
