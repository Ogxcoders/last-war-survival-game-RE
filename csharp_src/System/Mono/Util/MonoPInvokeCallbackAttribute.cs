using System;
using System.Diagnostics;

namespace Mono.Util;

[Conditional("MONOTOUCH")]
[Conditional("FULL_AOT_RUNTIME")]
[AttributeUsage(AttributeTargets.Method)]
internal sealed class MonoPInvokeCallbackAttribute : Attribute
{
	public MonoPInvokeCallbackAttribute(Type t)
	{
	}
}
