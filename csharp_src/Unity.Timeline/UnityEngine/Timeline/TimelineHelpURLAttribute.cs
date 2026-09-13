using System;
using System.Diagnostics;

namespace UnityEngine.Timeline;

[Conditional("UNITY_EDITOR")]
internal class TimelineHelpURLAttribute : Attribute
{
	public TimelineHelpURLAttribute(Type type)
	{
	}
}
