using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[Conditional("UNITY_EDITOR")]
public sealed class DisplayAsStringAttribute : Attribute
{
	public bool Overflow;

	public DisplayAsStringAttribute()
	{
		Overflow = true;
	}

	public DisplayAsStringAttribute(bool overflow)
	{
		Overflow = overflow;
	}
}
