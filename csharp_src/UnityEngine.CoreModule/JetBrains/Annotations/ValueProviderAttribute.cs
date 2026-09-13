using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
public sealed class ValueProviderAttribute : Attribute
{
	[NotNull]
	public string Name { get; }

	public ValueProviderAttribute([NotNull] string name)
	{
		Name = name;
	}
}
