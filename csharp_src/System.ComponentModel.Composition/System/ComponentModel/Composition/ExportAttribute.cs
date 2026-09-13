namespace System.ComponentModel.Composition;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class ExportAttribute : Attribute
{
	public string ContractName { get; private set; }

	public Type ContractType { get; private set; }

	public ExportAttribute()
		: this(null, null)
	{
	}

	public ExportAttribute(Type contractType)
		: this(null, contractType)
	{
	}

	public ExportAttribute(string contractName)
		: this(contractName, null)
	{
	}

	public ExportAttribute(string contractName, Type contractType)
	{
		ContractName = contractName;
		ContractType = contractType;
	}
}
