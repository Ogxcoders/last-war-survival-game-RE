using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public class LabelTextAttribute : Attribute
{
	public string Text;

	public bool NicifyText;

	public LabelTextAttribute(string text)
	{
		Text = text;
	}

	public LabelTextAttribute(string text, bool nicifyText)
	{
		Text = text;
		NicifyText = nicifyText;
	}
}
