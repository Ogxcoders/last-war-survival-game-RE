using System;

namespace UnityEngine.Bindings;

[VisibleToOtherModules]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property)]
internal class StaticAccessorAttribute : Attribute, IBindingsAttribute
{
	public string Name { get; set; }

	public StaticAccessorType Type { get; set; }

	public StaticAccessorAttribute()
	{
	}

	[VisibleToOtherModules]
	internal StaticAccessorAttribute(string name)
	{
		Name = name;
	}

	public StaticAccessorAttribute(StaticAccessorType type)
	{
		Type = type;
	}

	public StaticAccessorAttribute(string name, StaticAccessorType type)
	{
		Name = name;
		Type = type;
	}
}
