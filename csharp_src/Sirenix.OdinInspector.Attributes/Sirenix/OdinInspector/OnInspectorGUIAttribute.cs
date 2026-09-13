using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class OnInspectorGUIAttribute : ShowInInspectorAttribute
{
	public string Prepend;

	public string Append;

	[Obsolete("Use the Prepend member instead.", false)]
	public string PrependMethodName;

	[Obsolete("Use the Append member instead.", false)]
	public string AppendMethodName;

	public OnInspectorGUIAttribute()
	{
	}

	public OnInspectorGUIAttribute(string action, bool append = true)
	{
		if (append)
		{
			Append = action;
		}
		else
		{
			Prepend = action;
		}
	}

	public OnInspectorGUIAttribute(string prepend, string append)
	{
		Prepend = prepend;
		Append = append;
	}
}
