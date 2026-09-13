namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class PartMetadataAttribute : Attribute
{
	public string Name { get; private set; }

	public object Value { get; private set; }

	public PartMetadataAttribute(string name, object value)
	{
		Name = name ?? string.Empty;
		Value = value;
	}
}
