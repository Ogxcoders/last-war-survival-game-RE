using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Method)]
public sealed class MustUseReturnValueAttribute : Attribute
{
	[CanBeNull]
	public string Justification { get; }

	public MustUseReturnValueAttribute()
	{
	}

	public MustUseReturnValueAttribute([NotNull] string justification)
	{
		Justification = justification;
	}
}
