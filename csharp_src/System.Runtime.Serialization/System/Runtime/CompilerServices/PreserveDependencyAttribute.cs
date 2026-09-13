namespace System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = true)]
internal sealed class PreserveDependencyAttribute : Attribute
{
	public string Condition { get; set; }

	public PreserveDependencyAttribute(string memberSignature)
	{
	}

	public PreserveDependencyAttribute(string memberSignature, string typeName)
	{
	}

	public PreserveDependencyAttribute(string memberSignature, string typeName, string assembly)
	{
	}
}
