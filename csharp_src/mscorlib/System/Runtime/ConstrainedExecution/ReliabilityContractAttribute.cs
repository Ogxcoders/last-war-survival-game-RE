namespace System.Runtime.ConstrainedExecution;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Interface, Inherited = false)]
public sealed class ReliabilityContractAttribute : Attribute
{
	public Consistency ConsistencyGuarantee { get; }

	public Cer Cer { get; }

	public ReliabilityContractAttribute(Consistency consistencyGuarantee, Cer cer)
	{
		ConsistencyGuarantee = consistencyGuarantee;
		Cer = cer;
	}
}
