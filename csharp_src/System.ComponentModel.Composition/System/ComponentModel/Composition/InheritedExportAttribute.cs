namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class InheritedExportAttribute : ExportAttribute
{
	public InheritedExportAttribute()
		: this(null, null)
	{
	}

	public InheritedExportAttribute(Type contractType)
		: this(null, contractType)
	{
	}

	public InheritedExportAttribute(string contractName)
		: this(contractName, null)
	{
	}

	public InheritedExportAttribute(string contractName, Type contractType)
		: base(contractName, contractType)
	{
	}
}
