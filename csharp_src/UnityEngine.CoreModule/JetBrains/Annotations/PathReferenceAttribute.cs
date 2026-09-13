using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class PathReferenceAttribute : Attribute
{
	[CanBeNull]
	public string BasePath { get; }

	public PathReferenceAttribute()
	{
	}

	public PathReferenceAttribute([PathReference][NotNull] string basePath)
	{
		BasePath = basePath;
	}
}
