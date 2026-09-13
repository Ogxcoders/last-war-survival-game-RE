using System;

namespace UnityEngine.Bindings;

[VisibleToOtherModules]
[AttributeUsage(AttributeTargets.Method)]
internal class FreeFunctionAttribute : NativeMethodAttribute
{
	public FreeFunctionAttribute()
	{
		base.IsFreeFunction = true;
	}

	public FreeFunctionAttribute(string name)
		: base(name, isFreeFunction: true)
	{
	}

	public FreeFunctionAttribute(string name, bool isThreadSafe)
		: base(name, isFreeFunction: true, isThreadSafe)
	{
	}
}
