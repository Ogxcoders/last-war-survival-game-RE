using System;

namespace UnityEngine.Rendering.Universal;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public sealed class DisplayNameAttribute : Attribute
{
	public readonly string displayName;

	public DisplayNameAttribute(string displayName)
	{
		this.displayName = displayName;
	}
}
