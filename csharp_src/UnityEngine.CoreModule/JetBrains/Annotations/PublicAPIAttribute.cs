using System;

namespace JetBrains.Annotations;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
[MeansImplicitUse(ImplicitUseTargetFlags.WithMembers)]
public sealed class PublicAPIAttribute : Attribute
{
	[CanBeNull]
	public string Comment { get; }

	public PublicAPIAttribute()
	{
	}

	public PublicAPIAttribute([NotNull] string comment)
	{
		Comment = comment;
	}
}
