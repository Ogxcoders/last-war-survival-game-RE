using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class IndentAttribute : Attribute
{
	public int IndentLevel;

	public IndentAttribute(int indentLevel = 1)
	{
		IndentLevel = indentLevel;
	}
}
