namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
public sealed class ExportMetadataAttribute : Attribute
{
	public string Name { get; private set; }

	public object Value { get; private set; }

	public bool IsMultiple { get; set; }

	public ExportMetadataAttribute(string name, object value)
	{
		Name = name ?? string.Empty;
		Value = value;
	}
}
