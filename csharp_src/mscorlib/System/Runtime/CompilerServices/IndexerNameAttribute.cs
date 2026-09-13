namespace System.Runtime.CompilerServices;

[Serializable]
[AttributeUsage(AttributeTargets.Property, Inherited = true)]
public sealed class IndexerNameAttribute : Attribute
{
	public IndexerNameAttribute(string indexerName)
	{
	}
}
