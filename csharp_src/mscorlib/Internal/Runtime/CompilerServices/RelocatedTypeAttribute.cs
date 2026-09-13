using System;
using System.Diagnostics;

namespace Internal.Runtime.CompilerServices;

[Conditional("ALWAYSREMOVED")]
[AttributeUsage(AttributeTargets.All)]
internal class RelocatedTypeAttribute : Attribute
{
	public RelocatedTypeAttribute(string originalAssemblySimpleName)
	{
	}
}
