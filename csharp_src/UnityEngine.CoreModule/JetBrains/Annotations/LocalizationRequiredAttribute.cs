using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.All)]
public sealed class LocalizationRequiredAttribute : Attribute
{
	public bool Required { get; }

	public LocalizationRequiredAttribute()
		: this(required: true)
	{
	}

	public LocalizationRequiredAttribute(bool required)
	{
		Required = required;
	}
}
