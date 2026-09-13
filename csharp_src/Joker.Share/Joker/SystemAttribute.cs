using System;

namespace Joker;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SystemAttribute : ClassAttribute
{
	public Type worldType;

	public SystemAttribute(Type worldType)
	{
		this.worldType = worldType;
	}
}
