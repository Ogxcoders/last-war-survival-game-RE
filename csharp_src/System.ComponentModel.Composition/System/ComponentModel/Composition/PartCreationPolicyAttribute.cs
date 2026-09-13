namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class PartCreationPolicyAttribute : Attribute
{
	internal static PartCreationPolicyAttribute Default = new PartCreationPolicyAttribute(CreationPolicy.Any);

	internal static PartCreationPolicyAttribute Shared = new PartCreationPolicyAttribute(CreationPolicy.Shared);

	public CreationPolicy CreationPolicy { get; private set; }

	public PartCreationPolicyAttribute(CreationPolicy creationPolicy)
	{
		CreationPolicy = creationPolicy;
	}
}
