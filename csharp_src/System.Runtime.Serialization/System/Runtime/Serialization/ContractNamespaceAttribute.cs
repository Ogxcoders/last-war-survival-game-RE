namespace System.Runtime.Serialization;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, Inherited = false, AllowMultiple = true)]
public sealed class ContractNamespaceAttribute : Attribute
{
	private string clrNamespace;

	private string contractNamespace;

	public string ClrNamespace
	{
		get
		{
			return clrNamespace;
		}
		set
		{
			clrNamespace = value;
		}
	}

	public string ContractNamespace => contractNamespace;

	public ContractNamespaceAttribute(string contractNamespace)
	{
		this.contractNamespace = contractNamespace;
	}
}
